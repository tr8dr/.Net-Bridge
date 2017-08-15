// 
// General:
//      This file is pary of .NET Bridge
//
// Copyright:
//      2010 Jonathan Shore
//      2017 Jonathan Shore and Contributors
//
// License:
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at:
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
//

using System;
using System.Collections.Generic;
using bridge.common.utils;

namespace bridge.common.time
{
	/// <summary>
	/// Quick calculation of time of day given a clock timestamp and timezone
	/// </summary>
	public class ZTimeOfDay
	{
		/// <summary>
		/// Create time zone from .NET timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZTimeOfDay (ZTimeZone zone)
		{
			_zone = zone;
			_zone_offset = (long)zone.BaseUtcOffset.TotalMilliseconds;
		}
		
		
		
		// Functions
		
		
		/// <summary>
		/// Determines whether given UTC time is in daylight savings or not (for this zone)
		/// </summary>
		/// <returns>
		/// <c>true</c> if clock is in daylight savings; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='clock'>
		/// time in UTC ms since Jan 1 1970
		/// </param>
		public ZTime timeOf (long clock)
		{			
			if (clock > _good_end || clock < _good_start)
				NewTime (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var netclock = adjclock * 10000L + 621355968000000000L;
			var hour = (int)((netclock  % TicksPerDay) / TicksPerHour);
			var min = (int)((netclock % TicksPerHour) / TicksPerMinute);
			var sec = (int)((netclock % TicksPerMinute) / TicksPerSecond);
			var ms = (int)((netclock % TicksPerSecond) / TicksPerMillisecond);
			
			return new ZTime (hour, min, sec, ms);
		}
		
		
		// Implementation: DST
		
		
		private void NewTime (DateTime time)
		{
			var rule = GetApplicableRule (_zone.Composed, time);
			var year = time.Year;
			var utcoffset = _zone.BaseUtcOffset;
			
			if (rule != null)
			{
				var Rstart = rule.DaylightTransitionStart;
				var Rend = rule.DaylightTransitionEnd;
				
				DateTime DST_start = TransitionPoint (
					Rstart, year);
				DateTime DST_end = TransitionPoint (
					Rend, 
					year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
				
				DST_start -= utcoffset;
				DST_end -= (utcoffset + rule.DaylightDelta);
	
				_DST_start = (DST_start.Ticks - 621355968000000000L) / 10000L;
				_DST_end = (DST_end.Ticks - 621355968000000000L) / 10000L;
				_DST_offset = (long)rule.DaylightDelta.TotalMilliseconds;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_good_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_good_end = (yend.Ticks - 621355968000000000L) / 10000L;
			}
			else
			{
				_DST_start = long.MaxValue;
				_DST_end = 0;
				_DST_offset = 0;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_good_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_good_end = (yend.Ticks - 621355968000000000L) / 10000L;

			}
		}
		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (TimeZoneInfo zone, DateTime time)
		{
			var Tclock = time.Ticks + zone.BaseUtcOffset.Ticks;
			
			// use local to avoid another thread changing reference during test
			var srule = _rule;
			if (srule != null && IsAppropriateRule (srule, Tclock))
				return srule;
			
			var rulelist = zone.GetAdjustmentRules();
			foreach (var rule in rulelist)
			{
				if (IsAppropriateRule (rule, Tclock))
					return _rule = rule;
			}
			
			return null;
		}

		
		private bool IsAppropriateRule (TimeZoneInfo.AdjustmentRule rule, long Tclock)
		{
			if (rule.DateStart.Ticks > Tclock)
				return false;
			if (rule.DateEnd.Ticks >= Tclock)
				return true;
			else
				return false;
		}
		
		
		private DateTime TransitionPoint (TimeZoneInfo.TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
				return new DateTime (year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;

			DayOfWeek first = (new DateTime (year, transition.Month, 1)).DayOfWeek;
			DayOfWeek target = transition.DayOfWeek;

			// locate first dayofweek 
			int dayadjust = (first > target) ? (7 - (int)first + 1) : ((int)target - (int)first + 1);
			// roll to the nth
			int day = dayadjust + (transition.Week - 1) * 7;
			if (day >  DateTime.DaysInMonth (year, transition.Month))
				day -= 7;
			
			return new DateTime (year, transition.Month, day) + transition.TimeOfDay.TimeOfDay;
		}
		
		
		private static int GetYear (long netclock)
		{
			int totaldays = (int)(netclock / TicksPerDay);
			int num400 = (totaldays / dp400);
			totaldays -=  num400 * dp400;
		
			// leap year adjustment
			int num100 = (totaldays / dp100);
			if (num100 == 4)
				num100 = 3;
			
			totaldays -= (num100 * dp100);

			int num4 = totaldays / dp4;
			totaldays -= (num4 * dp4);

			int numyears = totaldays / 365 ;
			
			// another leap adjustment
			if (numyears == 4)
				numyears = 3;
			
			return num400*400 + num100*100 + num4*4 + numyears + 1;
		}
		
		
		// Constants
		
		private const int 			dp400 = 146097;
		private const int			dp100 = 36524;
		private const int			dp4 = 1461;
		
		private const long 			TicksPerDay = 864000000000L;
		private const long 			TicksPerHour = 36000000000L;
		private const long 			TicksPerMillisecond = 10000L;
		private const long 			TicksPerMinute = 600000000L;
		private const long 			TicksPerSecond = 10000000L;	
		
		
		// Variables
		
		private TimeZoneInfo.AdjustmentRule 	_rule; 
		private ZTimeZone						_zone;
		private long							_zone_offset;

		private long							_good_start;
		private long							_good_end;
		
		private long							_DST_start;
		private long							_DST_end;
		private long							_DST_offset;
	}
}


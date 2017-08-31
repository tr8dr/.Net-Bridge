// 
// General:
//      This file is part of .NET Bridge
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

namespace bridge.common.time
{
				
	/// <summary>
	/// date/time info generator
	/// </summary>
	public class ZDateTimeInfoGenerator
	{
		public ZDateTimeInfoGenerator (ZTimeZone zone)
		{
			_zone = zone;
			_zone_offset = (long)zone.BaseUtcOffset.TotalMilliseconds;
		}
		
		
		// Functions
	
	
		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZDateTimeInfo DateAndTimeFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var timepart = (int)(adjclock % (24L*3600L*1000L));
			
			var hour = timepart / (3600*1000);
			var min = (timepart % (3600*1000)) / 60000;
			var sec = (timepart % 60000) / 1000;
			var ms = timepart % 1000;
			
			// determine new date if clock does not fall within day range
			var netclock = adjclock * 10000L + 621355968000000000L;
			if (netclock < _day_start || netclock > _day_end)
 				NewYMD (netclock);
				
			return new ZDateTimeInfo (_year, _month, _day, hour, min, sec, ms);
		}



		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZDateTimeInfo DateFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			var netclock = adjclock * 10000L + 621355968000000000L;
			
			// determine new date if clock does not fall within day range
			if (netclock < _day_start || netclock > _day_end)
 				NewYMD (netclock);
				
			return new ZDateTimeInfo (_year, _month, _day);
		}

		
		
		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZTime TimeFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var netclock = adjclock * 10000L + 621355968000000000L;
			var timepart = (int)((netclock % TicksPerDay) / 10000L);
			
			var hour = timepart / (3600*1000);
			var min = (timepart % (3600*1000)) / 60000;
			var sec = (timepart % 60000) / 1000;
			var ms = timepart % 1000;
				
			return new ZTime (hour,min,sec,ms);
		}

		

		// Implementation: DST
		
		
		private void NewYMD (long netclock)
		{
			// n = number of days since 1/1/0001
            int n = (int)(netclock / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            int y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            int y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            int y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            int y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
            _year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
            
            // n = day number within year
            n -= y1 * DaysPerYear;

			// Leap year calculation looks different from IsLeapYear since y1, y4, and y100 are relative to year 1, not year 0
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear? DaysToMonth366: DaysToMonth365;
            
			// All months have less than 32 days, so n >> 5 is a good conservative estimate for the month
            var month = n >> 5 + 1;
            // m = 1-based month number
            while (n >= days[month]) month++;
			_month = month;
			
            // Return 1-based day-of-month
            _day = n - days[month - 1] + 1;
			
			// setup day window
			_day_start = (netclock / TicksPerDay) * TicksPerDay;
			_day_end = (_day_start + TicksPerDay - 1);
		}
		
		
		private void NewDST (DateTime time)
		{
			var rule = GetApplicableRule (_zone, time);
			var year = time.Year;
			var utcoffset = _zone.Underlier.BaseUtcOffset;
			
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
				
				_year_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_year_end = (yend.Ticks - 621355968000000000L) / 10000L;
			}
			else
			{
				_DST_start = long.MaxValue;
				_DST_end = 0;
				_DST_offset = 0;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_year_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_year_end = (yend.Ticks - 621355968000000000L) / 10000L;

			}
		}
		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (ZTimeZone ourzone, DateTime time)
		{
			TimeZoneInfo zone = ourzone.Underlier;
			if (zone == TimeZoneInfo.Utc)
				return null;
			
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
		
		
		
		// Constants
		
		private const int 				dp400 = 146097;
		private const int				dp100 = 36524;
		private const int				dp4 = 1461;

		// Number of 100ns ticks per time unit
        private const long 				TicksPerMillisecond = 10000;
        private const long 				TicksPerSecond = TicksPerMillisecond * 1000;
        private const long 				TicksPerMinute = TicksPerSecond * 60;
        private const long 				TicksPerHour = TicksPerMinute * 60;
        private const long 				TicksPerDay = TicksPerHour * 24;
    
        // Number of milliseconds per time unit
        private const int 				MillisPerSecond = 1000;
        private const int 				MillisPerMinute = MillisPerSecond * 60;
        private const int 				MillisPerHour = MillisPerMinute * 60;
        private const int 				MillisPerDay = MillisPerHour * 24;
    
        // Number of days in a non-leap year
        private const int 				DaysPerYear = 365;
        // Number of days in 4 years
        private const int 				DaysPer4Years = DaysPerYear * 4 + 1;
        // Number of days in 100 years
        private const int 				DaysPer100Years = DaysPer4Years * 25 - 1;
        // Number of days in 400 years
        private const int 				DaysPer400Years = DaysPer100Years * 4 + 1;
    
        // Number of days from 1/1/0001 to 12/31/1600
        private const int 				DaysTo1601 = DaysPer400Years * 4;
        // Number of days from 1/1/0001 to 12/30/1899
        private const int 				DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
        // Number of days from 1/1/0001 to 12/31/9999
        private const int 				DaysTo10000 = DaysPer400Years * 25 - 366;
    
        private const long 				FileTimeOffset = DaysTo1601 * TicksPerDay;
        private const long 				DoubleDateOffset = DaysTo1899 * TicksPerDay;

		// The minimum OA date is 0100/01/01 & the maximum OA date is 9999/12/31
        private const long 				OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;
        // All OA dates must be greater than (not >=) OADateMinAsDouble
        private const double 			OADateMinAsDouble = -657435.0;
        // All OA dates must be less than (not <=) OADateMaxAsDouble
        private const double 			OADateMaxAsDouble = 2958466.0;
    
        private static readonly int[] DaysToMonth365 = 
			{ 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly int[] DaysToMonth366 = 
			{ 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

		
		// Variables
		
		private TimeZoneInfo.AdjustmentRule 	_rule; 
		private ZTimeZone						_zone;
		private long							_zone_offset;

		private long							_year_start;
		private long							_year_end;
		private long							_day_start;
		private long							_day_end;
		
		private long							_DST_start;
		private long							_DST_end;
		private long							_DST_offset;
		
		private int								_year;
		private int								_month;
		private int								_day;
	}

}


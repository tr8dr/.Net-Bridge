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
using System.Collections.Generic;
using bridge.common.utils;
using System.Threading;

namespace bridge.common.time
{
	/// <summary>
	/// TimeZone functionality & timezone conversion.  The .NET implementation is too cumbersome and slow
	/// </summary>
	public class ZTimeZone
	{
		public readonly static ZTimeZone		GMT = new ZTimeZone (TimeZoneInfo.Utc);
		public readonly static ZTimeZone		UTC = new ZTimeZone (TimeZoneInfo.Utc);
		public readonly static ZTimeZone		Local = new ZTimeZone ("Local");
		
		public readonly static ZTimeZone		NewYork = new ZTimeZone ("America/New_York");
		public readonly static ZTimeZone		London = new ZTimeZone ("Europe/London");
		public readonly static ZTimeZone		Tokyo = new ZTimeZone ("Asia/Tokyo");
		public readonly static ZTimeZone		Auckland = new ZTimeZone ("Pacific/Auckland");

		
		/// <summary>
		/// Create time zone from .NET timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZTimeZone (TimeZoneInfo zone)
		{
			Composed = zone;
			Underlier = GetUnderlyingZone (zone);

			_generator = new ThreadLocal<ZDateTimeInfoGenerator> (() => new ZDateTimeInfoGenerator(this));
		}
		
		
		/// <summary>
		/// Create time zone from name
		/// </summary>
		/// <param name='zonename'>
		/// name of timezone
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Is thrown when an zone name is invalid
		/// </exception>
		public ZTimeZone (string zonename)
		{
			Composed = FindInternal (zonename);
			Underlier = GetUnderlyingZone (Composed);

			if (Composed == null)
				throw new ArgumentException ("unknown timezone: " + zonename);

			_generator = new ThreadLocal<ZDateTimeInfoGenerator> (() => new ZDateTimeInfoGenerator(this));
		}
		
		
		// Properties
		
		public TimeZoneInfo Composed
			{ get; private set; }

		public TimeZoneInfo Underlier
			{ get; private set; }
		
		public TimeSpan BaseUtcOffset
			{ get { return Composed.BaseUtcOffset; } }
		
		public string Id
			{ get { return Composed.Id; } }
		
		public ZDateTimeInfoGenerator Generator
			{ get { return _generator.Value; } }
		
		
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
		public bool IsDaylightSavingTime (long clock)
		{
			var Nclock = clock * 10000L + 621355968000000000L;
			return IsDaylightSavingTime (new DateTime (Nclock, DateTimeKind.Utc));			
		}

		
		/// <summary>
		/// Determines whether given UTC time is in daylight savings or not (for this zone)
		/// </summary>
		/// <returns>
		/// <c>true</c> if clock is in daylight savings; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='clock'>
		/// datetime in UTC form
		/// </param>
		public bool IsDaylightSavingTime (DateTime clock)
		{
			var rule = GetApplicableRule (clock);
			return IsDaylightSavingTime (rule, clock);
		}
		
		
		/// <summary>
		/// Gets the DST info for given UTC time
		/// </summary>
		/// <param name='clock'>
		/// UTC time in milliseconds since Jan 1, 1970
		/// </param>
		public ZDaylightSavings GetDSTInfoFor (long clock)
		{
			var Nclock = clock * 10000L + 621355968000000000L;
			return GetDSTInfoFor (new DateTime (Nclock, DateTimeKind.Utc));			
		}
		
		
		/// <summary>
		/// Gets the DST info for given UTC time
		/// </summary>
		/// <param name='clock'>
		/// UTC time
		/// </param>
		public ZDaylightSavings GetDSTInfoFor (DateTime clock)
		{
			var dstrule = GetApplicableRule (clock);
			return GetDSTInfoFor (dstrule, clock);
		}
		
		
		/// <summary>
		/// Convert the specified utc Datetime to a shifted UTC Datetime yielding Hour, Minute, etc. according to the time
		/// in this time zone
		/// </summary>
		/// <param name='utc'>
		/// UTC date/time
		/// </param>
		public DateTime Convert (DateTime utc)
		{
			if (utc.Kind != DateTimeKind.Utc)
				throw new Exception ("provided datetime must be in UTC form");
			
			var dstrule = GetApplicableRule (utc);
			if (IsDaylightSavingTime (dstrule, utc))
				return utc + BaseUtcOffset + dstrule.DaylightDelta;
			else
				return utc + BaseUtcOffset;
		}
		
		
		/// <summary>
		/// Convert the specified utc Datetime to a shifted UTC Datetime yielding Hour, Minute, etc. according to the time
		/// in this time zone
		/// </summary>
		/// <param name='clock'>
		/// UTC date/time in ms since Jan 1, 1970
		/// </param>
		public DateTime Convert (long clock)
		{
			DateTime utc = new DateTime (clock * 10000L + 621355968000000000L, DateTimeKind.Utc);
			var dstrule = GetApplicableRule (utc);
			if (IsDaylightSavingTime (dstrule, utc))
				return utc + BaseUtcOffset + dstrule.DaylightDelta;
			else
				return utc + BaseUtcOffset;
		}
		
		
		/// <summary>
		/// Find zone associated with name
		/// </summary>
		/// <param name='zonename'>
		/// name of time zone
		/// </param>
		public static ZTimeZone Find (string zonename)
		{
			return new ZTimeZone (FindInternal (zonename));
		}
		
		
		// Implementation: Zone Find


		/// <summary>
		/// Gets the underlying zone (for instance if GMT+5, would return GMT)
		/// </summary>
		/// <param name='composed'>
		/// Composed.
		/// </param>
		private static TimeZoneInfo GetUnderlyingZone (TimeZoneInfo composed)
		{
			var id = composed.DisplayName;

			var split = id.IndexOfAny(new char[] {'-','+'});
			if (split < 0)
				return composed;

            var smain = id[0] != '(' ? id.Substring(0, split) : id.Substring(1, split-1);
			return FindZone (smain);
		}

		
		/// <summary>
		/// Locate timezone by name or allow offset specification, such as "GMT+2", "GMT-5", "UTC+1"
		/// </summary>
		/// <param name='id'>
		/// Timezone name or name + offset
		/// </param>
		private static TimeZoneInfo FindInternal (string id)
		{
			int split = id.IndexOfAny(new char[] {'-','+'});
			if (split < 0)
			{
				return FindZone (id);
			}
			else
			{
				string smain = id.Substring(0, split);
				int ioffset = int.Parse(id.Substring(split+1)); 
				
				TimeZoneInfo main = FindZone (smain);
				TimeSpan offset = main.BaseUtcOffset.Add (new TimeSpan(ioffset, 0, 0));
				
				return TimeZoneInfo.CreateCustomTimeZone (id, offset, id, id);
			}
		}
		
		
		/// <summary>
		/// Finds the zone by ID.
		/// </summary>
		private static TimeZoneInfo FindZone (string zone)
		{
			if (_zones == null)
				Initialize ();
			
			TimeZoneInfo info = null;
			if (_zones.TryGetValue(zone, out info))
				return info;
			
			try
			{	
				return TimeZoneInfo.FindSystemTimeZoneById (zone);
			}
			catch
			{
				throw new Exception ("timezone " + zone + " not found, entries: " + StringUtils.ToString(_zones.Keys));
			}
		}
		
		
		/// <summary>
		/// Finds the timezone matching one of the known aliases (unfortunately this differs by platform)
		/// </summary>
		private static TimeZoneInfo FindAny (params string[] aliases)
		{
			foreach (var alias in aliases)
			{
				try
					{ return TimeZoneInfo.FindSystemTimeZoneById (alias); }
				catch
					{ }
			}
			
			throw new Exception ("could not find any time zones by alias: " + aliases[0] + ", ...");
		}
		
		
		// Meta
		
		
		public override string ToString ()
		{
			 return Composed.ToString();
		}
		
		
		// Implementation: DST

		
		private bool IsDaylightSavingTime (TimeZoneInfo.AdjustmentRule rule, DateTime time)
		{
			if (rule == null)
				return false;
					
			var Rstart = rule.DaylightTransitionStart;
			var Rend = rule.DaylightTransitionEnd;
			
			DateTime DST_start = TransitionPoint (
				Rstart, time.Year);
			DateTime DST_end = TransitionPoint (
				Rend, 
				time.Year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
			
			DST_start -= Underlier.BaseUtcOffset;
			DST_end -= (Underlier.BaseUtcOffset + rule.DaylightDelta);

			return (time >= DST_start && time < DST_end);			
		}

		
		private ZDaylightSavings GetDSTInfoFor (TimeZoneInfo.AdjustmentRule rule, DateTime time)
		{
			if (rule != null)
			{
				var Rstart = rule.DaylightTransitionStart;
				var Rend = rule.DaylightTransitionEnd;
			
				DateTime DST_start = TransitionPoint (
					Rstart, time.Year);
				DateTime DST_end = TransitionPoint (
					Rend, 
					time.Year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
			
				DST_start -= Underlier.BaseUtcOffset;
				DST_end -= (Underlier.BaseUtcOffset + rule.DaylightDelta);
				
				return new ZDaylightSavings (DST_start, DST_end, (long)rule.DaylightDelta.TotalMilliseconds);
			}
			else
			{
				return new ZDaylightSavings (DateTime.MaxValue, DateTime.MinValue, 0L);
			}
		}

		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (DateTime time)
		{
			var zone = Underlier;
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

		
		private static bool IsAppropriateRule (TimeZoneInfo.AdjustmentRule rule, long Tclock)
		{
			if (rule.DateStart.Ticks > Tclock)
				return false;
			if (rule.DateEnd.Ticks >= Tclock)
				return true;
			else
				return false;
		}
		
		
		private static DateTime TransitionPoint (TimeZoneInfo.TransitionTime transition, int year)
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
		
		
		// Initialization
		
		
		private static void Install (TimeZoneInfo zone, params string[] aliases)
		{
			foreach (string alias in aliases)
				_zones[alias] = zone;
        }
		
		
		private static void Initialize ()
		{
            lock (typeof(ZTimeZone))
            {
                if (_zones == null)
                    _zones = new Dictionary<string, TimeZoneInfo>(StringComparer.OrdinalIgnoreCase);
				
				// special hack for windows because local timezone lookup broken (waiting for mono folks)
				if (SystemUtils.IsWindows)
					Install (FindAny("America/New_York", "Eastern Standard Time"), "Local");
				else
					Install (TimeZoneInfo.Local, "Local");
				
                Install(
					FindAny("America/New_York", "Eastern Standard Time"), 
					"America/New_York", "America/New York", "EST", "GMT-5");
                Install(
					FindAny("Asia/Tokyo", "Tokyo Standard Time"), 
					"Asia/Tokyo", "JST", "GMT+9");
                Install(
					FindAny("Europe/London", "GMT Standard Time"), 
					"Europe/London", "GMT");
				Install(
					FindAny("Asia/Seoul","Korea Standard Time"), "Asia/Seoul");
				Install(
					FindAny("Asia/Bangkok", "SE Asia Standard Time"), 
					"Asia/Jakarta", "Asia/Bangkok", "Asia/Saigon");
				Install(
					FindAny("Europe/Moscow", "Russian Standard Time"), 
					"Europe/Moscow");
				Install(
					FindAny("America/Sao_Paulo", "E. South America Standard Time"), "America/Sao_Paulo");
				Install(
					FindAny("Asia/Shanghai", "China Standard Time"), 
					"Asia/Beijing", "Asia/Shanghai", "Asia/Hong_Kong");
				Install(
					FindAny("Asia/Singapore","Singapore Standard Time"), 
					"Asia/Singapore", "Asia/Brunei", "Asia/Manila", "Asia/Kuala_Lumpur");
				Install(
					FindAny("Australia/Perth","W. Australia Standard Time"), 
					"Australia/Perth");
				Install(
					FindAny("Asia/Taipei","Taipei Standard Time"), 
					"Asia/Taipei");
				Install(
					FindAny("Australia/Sydney","AUS Eastern Standard Time"), 
					"Australia/Sydney");
				Install(
					FindAny("Europe/Berlin","W. Europe Standard Time"), 
					"Europe/Frankfurt", "Europe/Berlin");
				Install(
					FindAny("Europe/Prague","Central Europe Standard Time"), 
					"Europe/Budapest", "Europe/Prague", "Europe/Belgrade");
				Install(
					FindAny("Europe/Athens","GTB Standard Time"), 
					"Europe/Athens", "Europe/Bucharest");
				Install(
					FindAny("Africa/Johannesburg","South Africa Standard Time"), 
					"Africa/Johannesburg");
				Install(
					FindAny("Europe/Istanbul","Turkey Standard Time"), 
					"Europe/Istanbul");
				Install(
					FindAny("Europe/Paris","Romance Standard Time"), 
					"Europe/Paris", "Europe/Brussels", "Europe/Copenhagen");
				Install(
					FindAny("Europe/Warsaw","Central European Standard Time"), 
					"Europe/Warsaw");
				Install(
					FindAny("Asia/Jerusalem","Israel Standard Time"), 
					"Asia/Tel_Aviv", "Asia/Jerusalem");
				Install(
					FindAny("Asia/Riyadh","Arab Standard Time"), 
					"Asia/Riyadh", "Asia/Qatar", "Asia/Kuwait", "Asia/Bahrain", "Asia/Aden");
				Install(
					FindAny("Pacific/Auckland","New Zealand Standard Time"), 
					"Pacific/Auckland");
				Install(
					FindAny("America/Buenos_Aires","Argentina Standard Time"), 
					"America/Buenos_Aires");
				Install(
					FindAny("America/Lima","SA Pacific Standard Time"), 
					"America/Bogota", "America/Lima", "America/Cayman", "America/Jamaica");
				Install(
					FindAny("America/Caracas","Venezuela Standard Time"), 
					"America/Caracas");
				Install(
					FindAny("Asia/Manila","Pacific SA Standard Time"), 
					"Asia/Santiago", "Asia/Manila");
				Install(
					FindAny("Asia/Kolkata","India Standard Time"), 
					"Asia/Mumbai","Asia/Kolkata");
				Install(
					FindAny("Asia/Almaty","Central Asia Standard Time"), 
					"Asia/Almaty");

                Install(TimeZoneInfo.Utc, "GMT", "UTC");
            }
		}

				
		// Variables
		
		static IDictionary<string,TimeZoneInfo>		_zones;
		TimeZoneInfo.AdjustmentRule 				_rule; 
		ThreadLocal<ZDateTimeInfoGenerator> 		_generator;
	}
			

}


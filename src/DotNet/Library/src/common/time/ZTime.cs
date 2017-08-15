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
using System.Text;
using System.Collections;

using bridge.common.utils;


namespace bridge.common.time
{
	/// <summary>
	/// Represents a time (absent of date), to millisecond granularity
	/// <ul>
	/// 	<li>13:30</li>
	/// 	<li>13:30:10:400</li>
	/// </ul>
	/// </summary>
	[Serializable]
	public struct ZTime
	{
		public readonly static ZTime		TIME_SOD = new ZTime (0,0,0,0);
		public readonly static ZTime		TIME_EOD = new ZTime (23,59,59,999);
		
		
		// Ctors
		
		
		/// <summary>
		/// Create time from string
		/// </summary>
		/// <param name='time'>
		/// time in HH:MM:SS:mmm format (or abbreviated)
		/// </param>
		public ZTime (string time)
		{
			string hr = StringUtils.Or (StringUtils.Field (time, 0, ':'), "0");
			string min = StringUtils.Or (StringUtils.Field (time, 1, ':'), "0");
			string sec = StringUtils.Or (StringUtils.Field (time, 2, ':'), "0");
			string ms = StringUtils.Or (StringUtils.Field (time, 3, ':'), "0");
			
			_time = 
				int.Parse(hr) * 3600 * 1000 +
				int.Parse(min) * 60 * 1000 +
				int.Parse(sec) * 1000 +
				int.Parse(ms);
		}
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.common.time.ZTime"/> struct.
		/// </summary>
		/// <param name='hr'>
		/// Hour
		/// </param>
		/// <param name='min'>
		/// Minutes
		/// </param>
		/// <param name='sec'>
		/// Seconds.
		/// </param>
		/// <param name='ms'>
		/// Milliseconds.
		/// </param>
		public ZTime (int hr, int min, int sec = 0, int ms = 0)
		{
			_time = hr * 3600 * 1000 + min * 60*1000 + sec * 1000 + ms;
		}
		
		
		// Properties
		
		
		public int Hour
			{ get { return _time / (3600 * 1000); } }
		
		public int Minute
			{ get { return (_time / (60*1000)) % 60; } }
		
		public int Second
			{ get { return (_time / 1000) % 60; } }
				
		public int MilliSecond
			{ get { return _time % 1000; } }
		
		public long UntilEndOfDay
			{ get { return 24L*3600000L - (long)_time; } }
		
		public long FromStartOfDay
			{ get { return _time; } }
		
		
		// Operations
		
				
		/// <summary>
		/// Convert to date/time (within current day, may be before current time)
		/// </summary>
		/// <returns>
		/// The date/time set according to this time spec.
		/// </returns>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime ToTime (long Tnow = 0L, ZTimeZone zone = null)
		{
			if (zone == null)
				zone = ZTimeZone.Local;
			
			if (Tnow == 0L)
				Tnow = Clock.Now;

			ZDateTime now = new ZDateTime (Tnow, zone);
			return new ZDateTime (now.Year, now.Month, now.Day, Hour, Minute, Second, MilliSecond, zone);
		}
	
		
		/// <summary>
		/// Convert to date/time (within current day if time after current time, otherwise following day)
		/// </summary>
		public ZDateTime ToNextTime (long Tnow = 0L, ZTimeZone zone = null)
		{
			if (zone == null)
				zone = ZTimeZone.Local;

			if (Tnow == 0L)
				Tnow = Clock.Now;
			
			var Tproj = ToTime(Tnow, zone);
			if (Tproj.Clock >= Tnow)
				return Tproj;
			else
				return Tproj.Add (1, 0, 0, 0, 0);
		}
		
		/// <summary>
		/// Provide offset in milliseconds from other time (if this time is > other will be +, otherwise -)
		/// </summary>
		/// <returns>
		/// Millisecond difference
		/// </returns>
		/// <param name='other'>
		/// Other time
		/// </param>
		public long OffsetFrom (ZTime other)
		{
			return Difference (this, other);
		}
		
		
		/// <summary>
		/// Provide the difference Ta and Tb in milliseconds (Ta - Tb).
		/// </summary>
		/// <param name='Ta'>
		/// Ta.
		/// </param>
		/// <param name='Tb'>
		/// Tb.
		/// </param>
		public static long Difference (ZTime Ta, ZTime Tb)
		{
			return Ta._time - Tb._time;
		}
		
		
		// Class Methods
		
	
		
		/// <summary>
		/// Get Time component of given date
		/// </summary>
		/// <param name='date'>
		/// date (date/time)
		/// </param>
		public static ZTime TimeOf (ZDateTime date)
		{
			return new ZTime (date.Hour, date.Minute, date.Second, date.Millisecond);
		}
	
		
		/// <summary>
		/// Get Time component of given date
		/// </summary>
		/// <param name='clock'>
		/// Clock (UTC ms since Jan 1 1970).
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZTime TimeOf (long clock, ZTimeZone zone = null)
		{
			zone = zone ?? ZTimeZone.Local;
			return TimeOf (new ZDateTime (clock, zone));
		}
		
		
		
		// Meta
		
		public override int GetHashCode ()
		{
			return _time;
		}
		
		public override bool Equals (object obj)
		{
			ZTime o = (ZTime)obj;
			return _time == o._time;
		}

		public static implicit operator ZTime (string s)
			{ return new ZTime (s); }
		
		public static bool operator== (ZTime a, ZTime b)
			{ return a._time == b._time; }
		
		public static bool operator!= (ZTime a, ZTime b)
			{ return a._time != b._time; }
		
		public static bool operator< (ZTime a, ZTime b)
			{ return a._time < b._time; }
		
		public static bool operator> (ZTime a, ZTime b)
			{ return a._time > b._time; }
		
		public static bool operator<= (ZTime a, ZTime b)
			{ return a._time <= b._time; }
		
		public static bool operator>= (ZTime a, ZTime b)
			{ return a._time >= b._time; }
		
		public static int operator- (ZTime a, ZTime b)
			{ return a._time - b._time; }
		
		
		public override string ToString ()
			{ return "" + Hour + ":" + Minute + ":" + Second + ":" + MilliSecond; }
		
		
		// Variable
		
		private int			_time;
	}
}

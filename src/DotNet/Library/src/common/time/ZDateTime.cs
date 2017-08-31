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
using bridge.common.parsing.dates;
using bridge.common.utils;


namespace bridge.common.time
{
	/// <summary>
	/// DateTime with time zone.  The missing paired timezone is a major oversight in .NET
	/// </summary>
	public struct ZDateTime
	{	
		/// <summary>
		/// Create a zoned datetime, converting the local time into the equivalent time of given zone
		/// </summary>
		/// <param name='local'>
		/// Local time
		/// </param>
		/// <param name='zone'>
		/// Zone to convert to
		/// </param>
		public ZDateTime (DateTime local, ZTimeZone zone)
	    {
			_parts = 0L;
			switch (local.Kind)
			{
				case DateTimeKind.Local:
					DateTime utc = local.ToUniversalTime();
					_utc = (utc.Ticks - 621355968000000000L) / 10000L; 
		        	_zone = zone;
					break;			
					
				case DateTimeKind.Utc:
					_utc = (local.Ticks - 621355968000000000L) / 10000L; 
	        		_zone = zone;
					break;

				default:
					throw new ArgumentException ("cannot handled unspecified datetimes");
			}
	    }
		
		
		/// <summary>
		/// Create a date in given timezone given UTC clock value
		/// </summary>
		/// <param name='clock'>
		/// # of ms since Jan 1 1970
		/// </param>
		/// <param name='zone'>
		/// Zone to present in
		/// </param>
		public ZDateTime (long clock, ZTimeZone zone)
	    {
			_utc = clock;
	        _zone = zone;
			_parts = 0L;
	    }

		
		
		/// <summary>
		/// Specify a time in the given time zone
		/// </summary>
		/// <param name='year'>
		/// Year.
		/// </param>
		/// <param name='month'>
		/// Month.
		/// </param>
		/// <param name='day'>
		/// Day.
		/// </param>
		/// <param name='hr'>
		/// Hr.
		/// </param>
		/// <param name='mins'>
		/// Mins.
		/// </param>
		/// <param name='secs'>
		/// Secs.
		/// </param>
		/// <param name='ms'>
		/// Ms.
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (int year, int month, int day, int hr, int mins, int secs, int ms, ZTimeZone zone)
	    {
			DateTime baseutc = new DateTime (year, month, Math.Max(day,1), hr, mins, secs, ms, DateTimeKind.Utc);
			DateTime utc = (baseutc - zone.BaseUtcOffset);
			
			var dst = zone.GetDSTInfoFor (utc);
			if (zone.IsDaylightSavingTime (utc))
				_utc = (utc.Ticks - 621355968000000000L) / 10000L - dst.Offset;
			else
				_utc = (utc.Ticks - 621355968000000000L) / 10000L;
				
	        _zone = zone;
			_parts = 
				((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27) |
				((ulong)hr << 22) | ((ulong)mins << 16) | ((ulong)secs << 10) | (ulong)ms;
	    }
		
		
		/// <summary>
		/// Specify a time in the given time zone
		/// </summary>
		/// <param name='year'>
		/// Year.
		/// </param>
		/// <param name='month'>
		/// Month.
		/// </param>
		/// <param name='day'>
		/// Day.
		/// </param>
		/// <param name='hr'>
		/// Hr.
		/// </param>
		/// <param name='mins'>
		/// Mins.
		/// </param>
		/// <param name='secs'>
		/// Secs.
		/// </param>
		/// <param name='ms'>
		/// Ms.
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (int year, int month, int day, int hr, int mins, int secs, int ms, string zone)
			: this (year, month, day, hr, mins, secs, ms, new ZTimeZone(zone))
	    {
	    }
		
		
		/// <summary>
		/// Create date from string & zone spec
		/// </summary>
		/// <param name='date'>
		/// Date as string in some default known format
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (string date, ZTimeZone zone = null)
	    {
			if (zone == null)
				zone = DefaultZoneFor (date);
			
			ZDateTime parsed = Parse (date, zone);

			_utc = parsed.Clock; 
	        _zone = parsed.TimeZone;
			_parts = parsed._parts;
	    }
		
		// Properties
		
	    public DateTime UTC 
			{ get { return new DateTime(_utc * 10000L + 621355968000000000L, DateTimeKind.Utc); } }
		
		public long Ticks
			{ get { return _utc * 10000L + 621355968000000000L; } }
		
		public DateTime Adjusted 
			{ get { return _zone.Convert (_utc); } }
	
	    public ZTimeZone TimeZone 
			{ get { return _zone; } }
		
		public long Clock
			{ get { return _utc; } }
		
		public int Year
			{ get { Convert(); return (int)(_parts >> 36); } }
		
		public int Month
			{ get { Convert(); return (int)(_parts >> 32) & 0x0000000f; } }
		
		public int Day
			{ get { Convert(); return (int)(_parts >> 27) & 0x0000001f; } }
		
		public int Hour
			{ get { Convert(); return (int)(_parts >> 22) & 0x0000001f; } }
		
		public int Minute
			{ get { Convert(); return (int)(_parts >> 16) & 0x0000003f; } }
		
		public int Second
			{ get { Convert(); return (int)(_parts >> 10) & 0x0000003f; } }
		
		public int Millisecond
			{ get { Convert(); return (int)(_parts & (long)0x000003ff); } }
		
		public ZTime Time
		{ 
			get 
			{ 
				var adj = Adjusted;
				return new ZTime (adj.Hour, adj.Minute, adj.Second, adj.Millisecond); 
			} 
		}
		
		
		public static ZDateTime Now
			{ get { return new ZDateTime (SystemUtils.ClockMillis, ZTimeZone.UTC); } }


		public bool IsJustDate
			{ get { return Hour == 0 && Minute == 0 && Second == 0 && Millisecond == 0; } }

		
		// Operations
		
		
		/// <summary>
		/// Time now in appropriate timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZDateTime TimeNowFor (ZTimeZone zone)
		{
			if (zone == ZTimeZone.GMT)
				return Now;
			else
				return new ZDateTime (SystemUtils.ClockMillis, zone);	
		}


		/// <summary>
		/// Convert time to the specified zone.
		/// </summary>
		/// <param name="zone">Zone.</param>
		public ZDateTime Convert (ZTimeZone zone)
		{
			if (zone == TimeZone)
				return this;
			else
				return new ZDateTime (Clock, zone);	
		}

		
		/// <summary>
		/// Add the specified hrs, mins, secs and ms.
		/// </summary>
		/// <param name='days'>
		/// Days to add.
		/// </param>
		/// <param name='hrs'>
		/// Hrs to add.
		/// </param>
		/// <param name='mins'>
		/// Mins to add.
		/// </param>
		/// <param name='secs'>
		/// Secs to add.
		/// </param>
		/// <param name='ms'>
		/// Milliseconds to add.
		/// </param>
		public ZDateTime Add (int days, int hrs, int mins, int secs, int ms)
		{
			TimeSpan span = new TimeSpan (days, hrs, mins, secs, ms);			
			return new ZDateTime (_utc + span.Ticks / 10000L, _zone);
		}
		
		
		/// <summary>
		/// Add the specified delta.
		/// </summary>
		/// <param name='delta'>
		/// Delta.
		/// </param>
		public ZDateTime Add (TimeSpan delta)
		{
			return new ZDateTime (_utc + delta.Ticks / 10000L, _zone);
		}
		
		
		// Meta
		
		
		public override int GetHashCode ()
		{
			return (int)_utc + _zone.GetHashCode();
		}
		
		public override bool Equals (object obj)
		{
			ZDateTime o = (ZDateTime)obj;
			return _utc == o._utc && _zone == o._zone;
		}
		
		public static implicit operator long (ZDateTime time)
			{ return time.Clock; }

		public static implicit operator ZDateTime (long time)
			{ return new ZDateTime (time, ZTimeZone.UTC); }

		
		public static bool operator== (ZDateTime a, ZDateTime b)
			{ return a._utc == b._utc; }

		public static bool operator!= (ZDateTime a, ZDateTime b)
			{ return a._utc != b._utc; }

		public static bool operator< (ZDateTime a, ZDateTime b)
			{ return a._utc < b._utc; }
		
		public static bool operator> (ZDateTime a, ZDateTime b)
			{ return a._utc > b._utc; }
		
		public static bool operator>= (ZDateTime a, ZDateTime b)
			{ return a._utc >= b._utc; }
		
		public static bool operator<= (ZDateTime a, ZDateTime b)
			{ return a._utc <= b._utc; }

		
		public override string ToString ()
		{
			return string.Format ("{0} {1}", Adjusted, _zone);
		}
		
		
		
		/// <summary>
		/// provide date as xml gmt form (2007-01-04T23:11:02.340Z).
		/// </summary>
		public string ToXmlDateTime ()
		{
			return string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}.{6:000}Z", Year, Month, Day, Hour, Minute, Second, Millisecond);
		}
		
		
		/// <summary>
		/// provide date as xml gmt form (2007-01-04T23:11:02.340Z).
		/// </summary>
		public static string ToXmlDateTime (long time)
		{
			var ztime = new ZDateTime (time, ZTimeZone.UTC);
			return ztime.ToXmlDateTime ();
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340).
		/// </summary>
		public string ToExcelDateTime ()
		{
			return string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:000}", Year, Month, Day, Hour, Minute, Second, Millisecond);
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340)..
		/// </summary>
		public static string ToExcelDateTime (long time)
		{
			var ztime = new ZDateTime (time, ZTimeZone.UTC);
			return ztime.ToExcelDateTime ();
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340).
		/// </summary>
		public string ToExcelDateTime (ZTimeZone zone)
		{
			var ztime = new ZDateTime (Clock, zone);
			return ztime.ToExcelDateTime();
		}

		
		/// <summary>
		/// Parse date from date & zone spec string
		/// </summary>
		/// <param name='date'>
		/// Date as string in some default known format
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZDateTime Parse (string date, string zone)
	    {
			if (date == null || zone == null)
				throw new ArgumentException ("date string or timezone is null");

			ZTimeZone czone = ZTimeZone.Find (zone);
			return new ZDateTime (date, czone);
	    }


		/// <summary>
		/// Minimum of 2 times
		/// </summary>
		/// <param name="a">The 1st time.</param>
		/// <param name="b">The 2nd time.</param>
		public static ZDateTime Min (ZDateTime a, ZDateTime b)
		{
			if (a.Clock < b.Clock)
				return a;
			else
				return b;
		}


		/// <summary>
		/// Max of 2 times
		/// </summary>
		/// <param name="a">The 1st time.</param>
		/// <param name="b">The 2nd time.</param>
		public static ZDateTime Max (ZDateTime a, ZDateTime b)
		{
			if (a.Clock > b.Clock)
				return a;
			else
				return b;
		}


		// Implementation


		private static ZTimeZone DefaultZoneFor (string date)
		{
			if (date[date.Length-1] == 'Z')
				return ZTimeZone.UTC;
			else
				return ZTimeZone.Local;
		}
		
		
		private static ZDateTime Parse (string date, ZTimeZone zone)
		{
			DateParser parser = DateParser.DefaultParser;
			return parser.Parse (date, zone);
		}
		
		
		private void Convert ()
		{
			if (_parts > 0)
				return;
			
			var generator = _zone.Generator;
			var info = generator.DateAndTimeFor (_utc);
			_parts = info.Encoded;
		}
				
				
		// Variables
		
		private long			_utc;
		private ulong			_parts;
	    private ZTimeZone		_zone;
	}

}

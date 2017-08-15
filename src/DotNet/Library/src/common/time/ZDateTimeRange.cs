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


namespace bridge.common.time
{
	/// <summary>
	/// Specific date/time range.
	/// </summary>
	public class ZDateTimeRange
	{
		public ZDateTimeRange (ZDateTime start, ZDateTime end)
		{
			_start = start;
			_end = end;
		}
		
		public ZDateTimeRange (string start, string end, ZTimeZone zone)
		{
			_start = new ZDateTime (start, zone);
			_end = new ZDateTime (end, zone);
		}
		
		// Properties
		
		public ZDateTime Start
			{ get { return _start; } }

		public ZDateTime End
			{ get { return _end; } }

		
		// Operation
		
		
		/// <summary>
		/// Determine whether given time within the specified time range.
		/// </summary>
		public bool Within (DateTime time)
		{
			long clock = (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
			return clock >= _start.Clock && clock <= _end.Clock;
		}

		/// <summary>
		/// Determine whether given time within the specified time range.
		/// </summary>
		public bool Within (ZDateTime time)
		{
			long clock = time.Clock;
			return clock >= _start.Clock && clock <= _end.Clock;
		}

		
		/// <summary>
		/// Determine whether given clock time within the specified time range.   Note that the clock
		/// must be in UTC ms since Jan 1 1970 (this is not the default in .NET, so requires conversion).
		/// </summary>
		public bool Within (long clock)
		{
			return clock >= _start.Clock && clock <= _end.Clock;
		}
		
		
		/// <summary>
		/// Determine whether given time range intersects with this one
		/// </summary>
		/// <param name='Tstart'>
		/// start time
		/// </param>
		/// <param name='Tend'>
		/// end time
		/// </param>
		public bool Intersects (long Tstart, long Tend)
		{
			if (Tstart <= _start.Clock && Tend >= _start.Clock)
				return true;
			if (Tstart > _start.Clock && Tstart < _end.Clock)
				return true;
			else
				return false;
		}

		
		public override string ToString ()
		{
			return "[" + _start + ", " + _end + "]"; 
		}
		
		
		// Variables
		
		private ZDateTime		_start;
		private ZDateTime		_end;
	}
}

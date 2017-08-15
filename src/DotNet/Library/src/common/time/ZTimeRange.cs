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
using System.Diagnostics;

using bridge.common.utils;


namespace bridge.common.time
{
	/// <summary>
	/// Represents a time range
	/// </summary>
	[Serializable]
	public class ZTimeRange
	{
		
		/// <summary>
		/// Create interval spanning start to end times
		/// </summary>
		/// <param name='start'>
		/// Start time.
		/// </param>
		/// <param name='end'>
		/// End time.
		/// </param>
		public ZTimeRange (ZTime start, ZTime end)
		{
			_start = start;
			_end = end;
			Debug.Assert (end.MilliSecond >= start.MilliSecond);
		}
		
		
		/// <summary>
		/// Create interval, parsing from string form:
		/// <p>
		/// Form can be one of:
		/// <ul>
		/// 	<li>[<time>, <time>]</li>
		/// 	<li><time>,<time></li>
		/// </ul>
		/// </summary>
		/// <param name='interval'>
		/// Interval.
		/// </param>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
		public ZTimeRange (string interval)
		{
			int isplit = interval.IndexOf (',');
			if (isplit == -1)
				throw new Exception ("time interval not in proper format: " + interval);
			
			string sstart = interval.Substring (0, isplit).Trim ();
			string send = interval.Substring (isplit+1).Trim();
			
			if (sstart[0] == '[')
				sstart = sstart.Substring (1);
			if (send[send.Length-1] == ']')
				send = send.Substring (0, send.Length-1);
			
			_start = new ZTime (sstart);
			_end = new ZTime (send);
			Debug.Assert (_end.MilliSecond >= _start.MilliSecond);
		}
		
		
		// Properties
		

		public ZTime StartTime
			{ get { return _start; } }
		
		public ZTime EndTime
			{ get { return _end; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Determine if time for given date within time-interval
		/// </summary>
		/// <param name='date'>
		/// date from which time is extracted (in local timezone)
		/// </param>
		public bool Within (ZDateTime date)
			{ return Within (ZTime.TimeOf(date)); }
	
		
		/// <summary>
		/// Determine if time for given clock time within time-interval
		/// </summary>
		/// <param name='clock'>
		/// clock stamp from which time is extracted (in local timezone)
		/// </param>
		public bool Within (long clock)
			{ return Within (ZTime.TimeOf(clock)); }
	
		
		/// <summary>
		/// Determine if time within time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool Within (ZTime time)
			{ return _start <= time && _end >= time; }
		

		/// <summary>
		/// Determine if time before time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool Before (ZTime time)
			{ return _start > time; }
		
		
		/// <summary>
		/// etermine if time after time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool After (ZTime time)
			{ return _end < time; }
	
		
		// Meta
		
		
		public override string ToString()
			{ return "[" + _start + ", " + _end + "]"; }

		public static implicit operator ZTimeRange (string range)
			{ return new ZTimeRange (range); }
		
		
		// Variables
		
		private ZTime	_start;
		private ZTime	_end;
	}
}

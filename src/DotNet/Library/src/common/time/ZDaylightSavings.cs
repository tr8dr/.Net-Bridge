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
	/// Daylight savings information for a particular date / timezone
	/// </summary>
	public struct ZDaylightSavings
	{
		public ZDaylightSavings (long DST_start, long DST_end, long DST_offset)
		{
			_DST_start = DST_start;
			_DST_end = DST_end;
			_DST_offset = DST_offset;
		}

		public ZDaylightSavings (DateTime DST_start, DateTime DST_end, long DST_offset)
		{
			_DST_start = (DST_start.Ticks - 621355968000000000L) / 10000L;
			_DST_end = (DST_end.Ticks - 621355968000000000L) / 10000L;
			_DST_offset = DST_offset;
		}
		
		
		// Properties
		
		/// <summary>
		/// Start of DST in UTC time (in ms since Jan 1, 1970)
		/// </summary>
		public long Start
			{ get { return _DST_start; } }
		
		/// <summary>
		/// End of DST in UTC time (in ms since Jan 1, 1970)
		/// </summary>
		public long End
			{ get { return _DST_end; } }
		
		/// <summary>
		/// Daylight savings time offset in milliseconds
		/// </summary>
		public long Offset
			{ get { return _DST_offset; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Determines whether the given utc time is in daylight savings time or not
		/// </summary>
		/// <returns>
		/// <c>true</c> if should apply daylight savings, false otherwise
		/// </returns>
		/// <param name='utc'>
		/// UTC time
		/// </param>
		public bool IsInDaylightSavings (DateTime utc)
		{
			var clock = (utc.Ticks - 621355968000000000L) / 10000L;
			return (clock >= Start && clock <= End);
		}
		
		/// <summary>
		/// Determines whether the given utc time is in daylight savings time or not
		/// </summary>
		/// <returns>
		/// <c>true</c> if should apply daylight savings, false otherwise
		/// </returns>
		/// <param name='utc'>
		/// UTC time in milliseconds since Jan 1 1970
		/// </param>
		public bool IsInDaylightSavings (long utc)
		{
			return (utc >= Start && utc <= End);
		}
		
		
		// Variables
		
		private long		_DST_start;
		private long		_DST_end;
		private long		_DST_offset;
	}
}


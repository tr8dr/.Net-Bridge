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
	public struct ZDateTimeInfo
	{
		public ZDateTimeInfo (int year, int month, int day, int hr, int min, int sec, int ms = 0)
		{
			_parts =
				((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27) |
				((ulong)hr << 22) | ((ulong)min << 16) | ((ulong)sec << 10) | (ulong)ms;
		}

		public ZDateTimeInfo (int year, int month, int day)
		{
			_parts = ((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27);
		}

		
		// Properties
		
		public int Year
			{ get { return (int)((_parts >> 36) & 0x000007ffL); } }
		
		public int Month
			{ get { return (int)((_parts >> 32) & 0x0000000fL); } }
		
		public int Day
			{ get { return (int)((_parts >> 27) & 0x0000001fL); } }
		
		public int Hour
			{ get { return (int)((_parts >> 22) & 0x0000001fL); } }
		
		public int Minute
			{ get { return (int)((_parts >> 16) & 0x0000003fL); } }
		
		public int Second
			{ get { return (int)((_parts >> 10) & 0x0000003fL); } }
		
		public int Millisecond
			{ get { return (int)(_parts & 0x000003ffL); } }
		
		public ulong Encoded
			{ get { return _parts; } }
		
		// Variables
		
		private ulong	_parts;
	}
	
}


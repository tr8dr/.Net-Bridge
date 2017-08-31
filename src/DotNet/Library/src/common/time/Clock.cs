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
using System.Text;
using System.Collections.Generic;
using bridge.common.utils;


namespace bridge.common.time
{	
	/// <summary>
	/// Time & clock related functions
	/// </summary>
	public class Clock
	{

		/// <summary>
		/// Gets or overrides current time.  Time is presented in UTC milliseconds since Jan 1 1970.
		/// </summary>
		/// <value>
		/// The clock now.
		/// </value>
		public static long Now
		{
			get
			{
				if (_Toverride > 0)
					return _Toverride;
				else
					return SystemUtils.ClockMillis;
			}
			set
				{ _Toverride = value; }
		}
		

		/// <summary>
		/// Provide current clock in local date/time
		/// </summary>
		public static ZDateTime Local
		{
			get 
			{
				return new ZDateTime (Now, ZTimeZone.Local);
			}
		}


		/// <summary>
		/// Provide current clock in UTC date/time
		/// </summary>
		public static ZDateTime UTC
		{
			get 
			{
				return new ZDateTime (Now, ZTimeZone.UTC);
			}
		}


		/// <summary>
		/// Determines whether is realtime or not.  It requires an observation period of
		/// 5 seconds before it decided whether is realtime or not.
		/// </summary>
		/// <returns>-1 if not realtime, 0 if uncertain, 1 if realtime</returns>
		/// <param name="Tnow">given time.</param>
		public static int IsRealTime (long Tnow)
		{
			if (_realtime != 0)
				return _realtime;

			if (_Tobservation_real == 0L)
			{
				_Tobservation_real = SystemUtils.ClockMillis;
				_Tobservation_given = Tnow;
			}

			var dt_given = Tnow - _Tobservation_given; 
			if (dt_given < 5000L)
				return 0;

			var dt_real = SystemUtils.ClockMillis - _Tobservation_real;
			var speedup = (double)dt_given / (double)dt_real;

			if (speedup >= 1.5)
				return _realtime = -1;
			else
				return _realtime = 1;
		}
		
		
		// Variables

		[ThreadStatic] static long			_Toverride;
		[ThreadStatic] static long			_Tobservation_given;
		[ThreadStatic] static long			_Tobservation_real;
		[ThreadStatic] static int			_realtime = 0;
	}
}

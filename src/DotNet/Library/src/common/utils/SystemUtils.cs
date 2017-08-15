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
using System.Diagnostics;
using System.IO;

namespace bridge.common.utils
{
	/// <summary>
	/// Various system utilities
	/// </summary>
	public static class SystemUtils
	{
		static SystemUtils ()
		{
			var Tbasis = DateTime.UtcNow.Ticks;
			_clockoffset = Tbasis - Stopwatch.GetTimestamp();
			_epochoffset = _clockoffset - 621355968000000000L;
		}
		
		
		/// <summary>
		/// Idnicates whether is windows or unix
		/// </summary>
		/// <value>
		/// <c>true</c> if running on windows; otherwise, <c>false</c>.
		/// </value>
		public static bool IsWindows
			{ get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }

		
		/// <summary>
		/// Gets the path of the root temporary directory
		/// </summary>
		public static string TmpPath
		{
			get
			{
				if (IsWindows)
					return Path.GetTempPath();
				else
					return "/tmp";
			}
		}
		
		
		/// <summary>
		/// Adds to the system path
		/// </summary>
		/// <param name='path'>
		/// Path to be added
		/// </param>
        public static void AddPath (string path)
        {
            var pathvar = IsWindows ? "Path" : "PATH";
            var opath = Environment.GetEnvironmentVariable(pathvar);
            var npath = opath + Path.PathSeparator + path;
            Environment.SetEnvironmentVariable (pathvar, npath);
        }
		
		
		/// <summary>
		/// Adds to the library path
		/// </summary>
		/// <param name='path'>
		/// Path to be added
		/// </param>
        public static void AddLibPath (string path)
        {
            if (IsWindows)
			{
            	var opath = Environment.GetEnvironmentVariable("Path");
				if (opath.Contains (path))
					return;
				
            	var npath = opath + Path.PathSeparator + path;
            	Environment.SetEnvironmentVariable ("Path", npath);
			}
			else
			{
            	var opath_linux = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
				if (opath_linux == null || !opath_linux.Contains (path))
				{
					var npath = StringUtils.IsBlank(opath_linux) ? path : opath_linux + Path.PathSeparator + path;
            		Environment.SetEnvironmentVariable ("LD_LIBRARY_PATH", npath);
				}
            	var opath_osx = Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH");
				if (opath_osx == null || !opath_osx.Contains (path))
				{
					var npath = StringUtils.IsBlank(opath_osx) ? path : opath_osx + Path.PathSeparator + path;
            		Environment.SetEnvironmentVariable ("DYLD_LIBRARY_PATH", npath);
				}
			}
        }
		
		
		/// <summary>
		/// Gets the clock time since epoch (Jan 1 1970) in milliseconds
		/// </summary>
		public static long ClockMillis
		{
			get 
			{
				return (Stopwatch.GetTimestamp() + _epochoffset) / 10000L;
			}
		}
		
		
		/// <summary>
		/// Gets the clock time since epoch (Jan 1 1970) in milliseconds
		/// </summary>
		public static long ClockMicro
		{
			get 
			{
				return (Stopwatch.GetTimestamp() + _epochoffset) / 10L;
			}
		}

		
		/// <summary>
		/// Gets the clock time in 10ths of a microsecond on a .NET clock basis
		/// </summary>
		public static long Ticks
		{
			get 
			{
				return Stopwatch.GetTimestamp() + _clockoffset;
			}
		}
		
		
		// Variables
		
		static readonly long 	_clockoffset;
		static readonly long 	_epochoffset;
	}

}


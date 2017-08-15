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
using System.Threading;

namespace bridge.common.system
{
	/// <summary>
	/// Exclusive lock for a resource
	/// </summary>
	public class ExclusiveLock : ILock
	{		
		/// <summary>
		/// Obtain exclusive lock
		/// </summary>
		public void Lock ()
		{
			Monitor.Enter (this);
		}
		
		/// <summary>
		/// Exit exclusive lock
		/// </summary>
		public void Unlock ()
		{
			Monitor.Exit (this);
		}
		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			if (timeout > 0)
				return Monitor.TryEnter (this, timeout);
			else
				return Monitor.TryEnter (this);
		}	
		
	}
}


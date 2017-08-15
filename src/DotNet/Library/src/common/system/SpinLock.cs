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
using System.Diagnostics;
using bridge.common.utils;

namespace bridge.common.system
{
	/// <summary>
	/// Highly efficient lock for high-frequency locked-sections with thread-reentry (unlike
	/// the .NET SpinLock implementation).
	/// </summary>
	public class SpinLock : ILock
	{	
		// Properties

		/// <summary>
		/// Returns owner of lock (note that this may be out-of-date)
		/// </summary>
		public int Owner
			{ get { return _depth == 0 ? 0 : _owner; } }

		/// <summary>
		/// Determines whether lock is open for this thread (again, may be dated)
		/// </summary>
		public bool IsOpen
			{ get { return _depth == 0 ? true : _owner == Thread.CurrentThread.ManagedThreadId; } }
		
		/// <summary>
		/// Determines lock depth
		/// </summary>
		public int Depth
			{ get { return _depth; } }


		// Operations


		/// <summary>
		/// Obtain exclusive lock
		/// 
		/// We use a staggered spin time to reduce the contention for the atomic variables
		/// </summary>
		public void Lock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;

			// spin is a structure, so should be cheap to instantiate (BTW)
			SpinWait spin = new SpinWait();

			while (true)
			{
				// enter the guard (which doubles as the recursion depth)
				var depth = Interlocked.Increment (ref _depth);

				// if depth == 1 then we have ownership
				if (depth == 1)
				{
					_owner = tid;
					return;
				}

				// if depth > 1 and owner is us, then we are good
				else if (_owner == tid)
					return;

				// decrement depth since we did not acquire
				Interlocked.Decrement (ref _depth);

				// spinning part, busy-wait for a while
				spin.SpinOnce ();
			}

		}
		
		/// <summary>
		/// Exit exclusive lock
		/// </summary>
		public void Unlock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;
			if (tid != _owner)
				throw new Exception ("attempted to unlock lock not owned by thread: " + tid);

			var depth = Interlocked.Decrement (ref _depth);
			if (depth == 0)
				Thread.Yield();
		}

		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			var tid = Thread.CurrentThread.ManagedThreadId;

			// spin is a structure, so should be cheap to instantiate (BTW)
			SpinWait spin = new SpinWait();
			var Tstart = timeout > 0L ? SystemUtils.ClockMillis : 0L;

			while (true)
			{
				// enter the guard (which doubles as the recursion depth)
				var depth = Interlocked.Increment (ref _depth);

				// if depth == 1 then we have ownership
				if (depth == 1)
				{
					_owner = tid;
					return true;
				}
				
				// if depth > 1 and owner is us, then we are good
				else if (_owner == tid)
					return true;

				// decrement depth since we did not acquire
				Interlocked.Decrement (ref _depth);

				if (timeout == 0)
					return false;
				if ((SystemUtils.ClockMillis - Tstart) >= timeout)
					return false;
				
				// spinning part, busy-wait for a while
				spin.SpinOnce ();
			}
		}	


		// Meta

		public override string ToString ()
		{
			return string.Format ("[SpinLock: Depth={0}, Owner={1}, IsOpen={2}]", _depth, _owner, IsOpen);
		}


		// Constants

		static readonly int		NoOwner = 0x7fffffff;
		
		// Variables

		private int				_depth = 0;
		volatile int			_owner = NoOwner;
	}
}


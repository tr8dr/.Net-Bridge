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
using System.Diagnostics; 
using System.Threading;
using System.Collections.Generic; 

namespace bridge.common.system 
{ 
	/// <summary>
	/// Fair lock with reentrancy
	/// 
	/// Uses monitors to affect the locking, hence not the fastest locking solution
	/// </summary>
	public sealed class ReentrantFairLockByMonitor : ILock
	{ 

		// Properties

		public bool IsOwner 
			{ get { return _owner == Thread.CurrentThread.ManagedThreadId; } }

		public int Depth
			{ get { return _depth; } }


		// Functions


		/// <summary>
		/// Enter the lock
		/// </summary>
		public void Lock () 
		{ 
			var tid = Thread.CurrentThread.ManagedThreadId;
			lock (_lock)
			{
				// quick check for reentrant exit
				if (_owner == tid)
					{ _depth++; return; }

				// enqueue our thread ID for servicing
				_queue.Enqueue (tid);

				// wait until we can grab the lock
				while (_queue.Peek() != tid)
					Monitor.Wait (_lock);

				_owner = tid;
				_depth++;
			}
		} 


		/// <summary>
		/// Exit the lock
		/// </summary>
		public void Unlock () 
		{ 
			var tid = Thread.CurrentThread.ManagedThreadId;

			lock (_lock)
			{
				if (--_depth > 0)
					return;

				if (_queue.Dequeue() != tid)
					throw new ArgumentException ("thread not owning lock attempted to unlock lock");

				_owner = int.MinValue;
				if (_queue.Count > 0)
					Monitor.PulseAll (_lock);
			}
		} 

		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			throw new NotImplementedException ("try-lock not implemented");
		}	
		

		// Variables

		volatile int			_depth = 0;
		volatile int			_owner = int.MinValue;

		private Queue<int>		_queue = new Queue<int>();
		private object			_lock = new object();
	} 
}


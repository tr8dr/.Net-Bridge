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
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace bridge.common.system
{
	using MonoTicketLock = System.Threading.SpinLock;

	/// <summary>
	/// Highly efficient reentrant lock with no context switching (busy-waits) and with 
	/// FIFO thread selection.
	/// 
	/// This implementation is adapted from the mono SpinLock, but with reentrant adaptations
	/// </summary>
	public class TicketLock : ILock
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

			// enter the guard (which doubles as the recursion depth)
			var depth = Interlocked.Increment (ref _depth);

			// if depth == 1 then we may have ownership, but need to see if we were next (otherwise block)
			if (depth == 1)
				FairEnter (tid);

			// if depth > 1 and owner is us, then we are good
			else if (_owner == tid)
				return;

			// need to wait for our turn
			else
				FairEnter (tid);
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
			{
				_owner = int.MinValue;
				FairExit ();
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
			var tid = Thread.CurrentThread.ManagedThreadId;
			
			// enter the guard (which doubles as the recursion depth)
			var depth = Interlocked.Increment (ref _depth);
			
			// if depth == 1 then we may have ownership, but need to see if we were next (otherwise block)
			if (depth == 1)
				return FairEnter (tid, timeout);
			
			// if depth > 1 and owner is us, then we are good
			else if (_owner == tid)
				return true;
			
			// need to wait for our turn
			else
				return FairEnter (tid, timeout);
		}	


		// Meta

		public override string ToString ()
		{
			return string.Format ("[TicketLock: Depth={0}, Owner={1}, IsOpen={2}]", _depth, _owner, IsOpen);
		}


		#region Ticket-Based Entry


		/// <summary>
		/// Wait until it is our turn to lock
		/// </summary>
		private void FairEnter (int tid)
		{
			Interlocked.Decrement (ref _depth);

			var taken = false;
			while (!taken)
				_ticketlock.Enter (ref taken);

			_owner = tid;
		}


		/// <summary>
		/// Wait until it is our turn to lock or until # of ms elapsed
		/// </summary>
		private bool FairEnter (int tid, int ms)
		{
			Interlocked.Decrement (ref _depth);

			var taken = false;
			_ticketlock.TryEnter (ms, ref taken);

			if (taken)
				_owner = tid;

			return taken;
		}


		/// <summary>
		/// Exit the lock
		/// </summary>
		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.Success)]
		private void FairExit ()
		{
			_ticketlock.Exit (true);
		}


		#endregion


		// Variables

		private int					_depth = 0;
		private int					_owner;
		private MonoTicketLock		_ticketlock = new MonoTicketLock ();
	}
}


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
using System.Threading;

namespace bridge.common.system
{
	/// <summary>
	/// Read write lock allowing single exclusive writer and multiple readers
	/// </summary>
	public class ReadWriteLock
	{
		public ReadWriteLock (LockRecursionPolicy policy = LockRecursionPolicy.SupportsRecursion)
		{
			_lock = new ReaderWriterLockSlim (policy);
			_rlock = new ReaderLock (this);
			_wlock = new WriterLock (this);
		}
		
		
		/// <summary>
		/// Reader lock that can be locked / unlocked by a particular reader thread
		/// </summary>
		public ILock ReadLock
			{ get { return _rlock; } }

		/// <summary>
		/// Writer lock that can be locked / unlocked by a particular writer thread
		/// </summary>
		public ILock WriteLock
			{ get { return _wlock; } }
		

        // Operations

		/// <summary>
		/// Wait for next write (as seen by the release of a write lock");
		/// </summary>
        public void WaitForNextWrite()
        {
			if (_written == null)
				_written = _lock;

			lock (_written)
			{
				Monitor.Wait (_written);
			}
        }
		

		// classes
		
		
		private class ReaderLock : ILock
		{
			public ReaderLock (ReadWriteLock src)
			{
				_lock = src;
			}
			
			public void Lock ()
				{ _lock._lock.EnterReadLock (); }
			
			public void Unlock ()
				{ _lock._lock.ExitReadLock (); }
			
			public bool TryLock (int timeout = 0)
				{ return _lock._lock.TryEnterReadLock (timeout); }
				
			
			// Variables
			
			private ReadWriteLock	_lock; 
		}

				
		
		private class WriterLock : ILock
		{
			public WriterLock (ReadWriteLock src)
			{
				_lock = src;
			}
			
			public void Lock ()
				{ _lock._lock.EnterWriteLock(); }
			
			public void Unlock ()
			{ 
				_lock._lock.ExitWriteLock();
				if (_lock._written != null)
				{
					lock (_lock._written) Monitor.PulseAll (_lock._written);
				}
			}
			
			public bool TryLock (int timeout = 0)
			{ 
				return _lock._lock.TryEnterWriteLock (timeout);
			}
				
			
			// Variables
			
			private ReadWriteLock	_lock; 
		}

		
		// Variables
		
		private ReaderWriterLockSlim	_lock;
		
		private ReaderLock				_rlock;		
		private WriterLock				_wlock;
        private object                  _written;
	}
}


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
using System.Collections.Concurrent;
using System.Threading;

using GFSpinLock = bridge.common.system.SpinLock;


namespace bridge.common.system
{


	/// <summary>
	/// Single threaded pool for objects reuse: this is not thread-safe, hence either should be thread-local
	/// <p/>
	/// Subclasses should add a Alloc with arguments, refering to Alloc() to get unparameterized object
	/// </summary>
	public sealed class STObjectPool<T> where T : new()
	{
		public STObjectPool (int maxsize, Func<T> creator = null)
		{
			if (creator != null)
				_creator = creator;
			else
				_creator = () => new T();
			
			_pool = new T[maxsize];
			_in = 0;
			_out = 0;
		}
		
		
		// Properties
		
		public int PoolSize
			{ get { return _pool.Length; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Get / Create an allocation of class instance
		/// </summary>
		public T Alloc ()
		{
			if (_in == _out)
				return _creator();

			var next = (_out + 1) % _pool.Length;
			var obj = _pool[_out];
			_out = next;

			return obj;
		}
		

		/// <summary>
		/// Free the specified obj.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public void Free (T obj)
		{
			if (obj == null)
				return;

			var next = (_in + 1) % _pool.Length;

			// if pool full return
			if (next == _out)
				return;

			// add to queue
			_pool[_in] = obj;
			_in = next;
		}


		// Variables
		
		readonly Func<T>	_creator;
		readonly T[]		_pool;
		int					_in;
		int					_out;
	}


	/// <summary>
	/// Pool objects for reuse: this lock-free implementation taken from Mono source with modifications
	/// <p/>
	/// Subclasses should add a Alloc with arguments, refering to Alloc() to get unparameterized object
	/// </summary>
	public sealed class MTObjectPool<T> where T : new()
	{
		public MTObjectPool (int maxsize, Func<T> creator = null)
		{
			if (creator != null)
				_creator = creator;
			else
				_creator = () => new T();
			
			_pool = new T[maxsize];
			_index_add = 0L;
			_index_remove = 0L;
		}
		
		
		// Properties
		
		public int PoolSize
			{ get { return _pool.Length; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Get / Create an allocation of class instance
		/// </summary>
		public T Alloc ()
		{
			if (_index_remove == _index_add)
				return _creator();
			if ((_index_add & ~SignalBit) - 1L == _index_remove)
				return _creator();
			
			long i;
			T result;
			int tries = 3;
			
			do 
			{
				i = _index_remove;
				// check to see whether queue exhausted (index_add % size)
				if ((_index_add & ~SignalBit) - 1L == i || tries == 0)
					return _creator();
				
				result = _pool[(int)(i % _pool.Length)];				
			} 
			while (Interlocked.CompareExchange (ref _index_remove, i + 1, i) != i && --tries > 0);
			
			return result;
		}
		
		
		/// <summary>
		/// Free the specified obj.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public void Free (T obj)
		{
			if (obj == null || (_index_add - _index_remove) >= _pool.Length - 1)
				return;
			
			long idx;
			int outer_tries = 3;
			do 
			{
				int inner_tries = 10;
				do
				{
					idx = _index_add;
				}
				while ((idx & SignalBit) > 0L && --inner_tries > 0);
				
				if ((idx - _index_remove) >= (_pool.Length - 1))
					return;
				
			} 
			while (Interlocked.CompareExchange (ref _index_add, idx + 1 + SignalBit, idx) != idx && --outer_tries > 0);
			
			_pool[(int)(idx % _pool.Length)] = obj;
			_index_add = _index_add - SignalBit;			
		}
		
		
		// Constants
		
		const long			SignalBit = 0x0800000000000000;
		
		// Variables
		
		readonly Func<T>	_creator;
		readonly T[]		_pool;
		long				_index_add;
		long				_index_remove;
	}

}


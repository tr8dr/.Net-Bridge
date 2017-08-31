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
using System.Collections.Generic;
using System.Threading;


namespace src.common.collections
{
	/// <summary>
	/// Blocking queue.
	/// </summary>
	public class BlockingQueue<T>
	{
		public BlockingQueue (int size)
		{
			_queue = new Queue<T> (size);
		}

		public BlockingQueue ()
		{
			_queue = new Queue<T> ();
		}


		// Properties

		public int Count
			{ get { return _queue.Count; } }


		// Operations

		/// <summary>
		/// Enqueue the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Enqueue (T item)
		{
			lock (_queue)
			{
				_queue.Enqueue (item);

				if (_queue.Count == 1)
					Monitor.PulseAll (_queue);
			}
		}


		/// <summary>
		/// Dequeue next item or wait until becomes available
		/// </summary>
		public T Dequeue ()
		{
			lock (_queue)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait (_queue);
				}

				return _queue.Dequeue ();
			}
		}
		

		/// <summary>
		/// Peek at next item or wait until becomes available
		/// </summary>
		public T Peek ()
		{
			lock (_queue)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait (_queue);
				}

				return _queue.Peek ();
			}
		}

		
		/// <summary>
		/// Dequeue next item if available, otherwise return false
		/// </summary>
		public bool TryDequeue (out T value)
		{
			lock (_queue)
			{
				if (_queue.Count > 0)
				{
					value = _queue.Dequeue ();
					return true;
				}
				else
				{
					value = default(T);
					return false;
				}
			}
		}


		// Variables

		private readonly Queue<T>	_queue;
	}
}


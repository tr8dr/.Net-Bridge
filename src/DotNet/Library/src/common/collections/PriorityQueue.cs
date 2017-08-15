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

namespace bridge.common.collections
{
	/// <summary>
	/// Priority queue, implemented as a binary heap.  Provides the following:
	/// <list>
	/// 	<item>enqueue		[enqueues an element such that an element can be later dequeued in priority order]</item>
	/// 	<item>dequeue		[dequeues element from heap with the highest priority]</item>
	/// 	<item>remove		[removes an element out of order]</item>
	/// </list>
	/// <p/>
	/// A delegate providing priorities for object put on the queue must be provided in construction.  
	/// Priorities must be invariant, or if variant must be removed and re-inserted when priority changes.
	/// <p/>
	/// Higher #s are considered to be higher priority and can be positive or negative in value.
	/// </summary>
	public class PriorityQueue<T> where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.common.collections.PriorityQueue`1"/> class.
		/// </summary>
		/// <param name="priority">Priority function</param>
		public PriorityQueue (Func<T,double> priority)
		{
			_queue = new T[512];
			_priority = priority;
			_count = 0;
		}
		
		
		// Properties
		
		public int Count
			{ get { return _count; } }


		// Functions
		
		
		/// <summary>
		/// Enqueue prioritized object
		/// <p/>
		/// Item will be retrieved with dequeue not in FIFO order rather priority
		/// order.  Highest priority objects will be dequeued sooner.
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Enqueue (T item)
		{
			_count++;
			if (_count >= _queue.Length)
				Grow ();
			
			_queue[_count-1] = item;
			SettleUpward (item, _count);
		}
		
		
		/// <summary>
		/// Dequeue highest priority item from queue
		/// <p/>
		/// Item is removed from the queue such that the next highest priority item will
		/// be retrieved in the next call to Dequeue().
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when one tries to dequeue on an empty queue
		/// </exception>
		public T Dequeue ()
		{
			if (_count == 0)
				throw new ArgumentException ("Dequeue: queue empty");
	
			// get leafmost node & adjust queue size
			T top = _queue[0];
			T leafy = _queue[--_count];
			_queue[0] = leafy;
			_queue[_count] = null;

			// adjust placement in queue
			if (_count > 1)
				SettleDownward (leafy, 1);
	
			return top;			
		}
		
		
		/// <summary>
		/// Dequeue highest priority item from queue
		/// <p/>
		/// Item is removed from the queue such that the next highest priority item will
		/// be retrieved in the next call to Dequeue().
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when one tries to dequeue on an empty queue
		/// </exception>
		public bool TryDequeue (out T item)
		{
			if (_count == 0)
			{
				item = default(T);
				return false;
			}
	
			// get leafmost node & adjust queue size
			T top = _queue[0];
			T leafy = _queue[--_count];
			_queue[0] = leafy;
			_queue[_count] = null;
	
			// adjust placement in queue
			if (_count > 1)
				SettleDownward (leafy, 1);
	
			item = top;
			return true;
		}
		
		
		/// <summary>
		/// Peek at top of queue
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when called on empty queue
		/// </exception>
		public T Peek ()
		{
			if (_count > 0)
				return _queue[0];
			else
				throw new ArgumentException ("Dequeue: queue empty");
		}
		
		
		/// <summary>
		/// Peek at top of queue
		/// </summary>
		public bool TryPeek (out T item)
		{
			if (_count > 0)
			{
				item = _queue[0];
				return true;
			} 
			else
			{
				item = null;
				return false;
			}
		}
		
		
		/// <summary>
		/// Clear the queue
		/// </summary>
		public void Clear ()
		{
			for (int i = 0 ; i < _count ; i++)
				_queue[i] = null;
			
			_count = 0;
		}
		
		
		/// <summary>
		/// Remove an item matching given
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public bool Remove (T item)
		{
			int pos = FindIndexOf (item);
			if (pos < 0)
				return false;
				
			// get leafmost node & adjust queue size
			T leafy = _queue[--_count];
			_queue[pos] = leafy;
			_queue[_count] = null;
			
			// if item removed is leaf, nothing to do
			if (pos == _count)
				return true;;
	
			// otherwise, settle leaf into deleted spot and adjust into heap, in [1..n] space
			if (_count > 0)
				Settle (leafy, pos+1);
			
			return true;
		}
		
		
		// Implementation
		
		
		/// <summary>
		/// Resettle object in queue (either upwards or downwards)
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// place to start settling from, in [1..n] space.
		/// </param>
		private void Settle (T obj, int start)
		{
			// get parent
			T parent = (start > 1) ? 
				_queue[start/2] : null;
	
			// get priorities
			var Tpriority = _priority (obj);
			var Ppriority = (parent != null) ? _priority(parent) : double.MaxValue;
	
			if (Tpriority > Ppriority)
				SettleUpward (obj, start);
			else
				SettleDownward (obj, start);
		}
	
	
		/// <summary>
		/// Resettle object in queue downwards; assumes object is smaller than parent at starting position
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// start place to start settling from, in [1..n] space.
		/// </param>
		private void SettleDownward (T obj, int start)
		{
			// get size
			var size = _count;
			int i = start;
	
			var Tpriority = _priority(obj);
		
			// find position downwards
			while (i < size)
			{
				int ci = i*2;
	
				// get children
				T childA = (size >= ci) ? 
					_queue[ci-1] : default(T);
				T childB = (size > ci) ? 
					_queue[ci+1-1] : default(T);
	
				// get priorities
				var Apriority = (childA != null) ? _priority(childA) : double.MinValue;
				var Bpriority = (childB != null) ? _priority(childB) : double.MinValue;
	
				// determine whether current node a proper resting place
				if (Tpriority >= Apriority && Tpriority >= Bpriority)
					{ _queue[i-1] = obj; return; }
	
				// otherwise must swap down with the largest child
				if (Apriority >= Bpriority)
					{ _queue[i-1] = childA;  i = ci; }
				else
					{ _queue[i-1] = childB;  i = ci+1; }
			}
	
			// at right-most leaf
			_queue[i-1] = obj; 
		}
		
		
		/// <summary>
		/// Resettle object in queue upwards; assumes children are smaller than
		/// node at start position
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// place to start settling from, in [1..n] space.
		/// </param>
		private void SettleUpward (T obj, int start)
		{
			// place in at starting position
			_queue[start-1] = obj;
	
			// find proper place for object by moving through heap (upwards towards root)
			for (int i = start ; i > 1 ; i = i / 2)
			{
				// get priority of parent
				T parent = _queue[i/2-1];
				T child = _queue[i-1];
					
				// if parent lower-priority, swap
				if (_priority(child) > _priority(parent))
				{ 
					_queue[i-1] = parent; 
					_queue[i/2-1] = child; 
				} else
					return;
			}
		}
		
		
		private void Grow ()
		{
			
			var nlen = Math.Min (_count * 2, _count + 1024);
			T[] nqueue = new T[nlen];
			Array.Copy (_queue, 0, nqueue, 0, _queue.Length);
			_queue = nqueue;
		}
		
		
		private int FindIndexOf (T obj)
		{
			for (int i = 0 ; i < _count ; i++)
			{
				if (_queue[i].Equals (obj))
					return i;
			}
			
			return -1;
		}
		
		
		// Variables
		
		private T[]				_queue;
		private int				_count;
		private Func<T,double>	_priority;
	}
}


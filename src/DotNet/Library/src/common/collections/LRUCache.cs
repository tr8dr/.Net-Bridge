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
using System.Collections;

namespace bridge.common.collections
{
	/// <summary>
	/// Resource usage delegate.  Measures memory, instance, or other resources
	/// </summary>
	public delegate long ResourceUsage<T> (T obj); 
	
	/// <summary>
	/// Resource usage delegate.  Measures memory, instance, or other resources
	/// </summary>
	public delegate void Disposal<T> (T obj); 


	/// <summary>
	/// LRU cache that uses approximate resource usage to decide when to retire elements
	/// </summary>
	public class LRUCache<K,T> : IEnumerable<KeyValuePair<K,T>>
	{
		/// <summary>
		/// Create LRU cache
		/// </summary>
		/// <param name='measure'>
		/// Resource measurement delegate applied to each element
		/// </param>
		/// <param name='maxresource'>
		/// Maximum resource utilization allowed, after which will trim back on an LRU basis
		/// </param>
		public LRUCache (
			ResourceUsage<T> measure, 
			long maxresource,
			Disposal<T> disposal = null)
		{
			_measure = measure;
			_maxresource = maxresource;
			_disposal = disposal;
			_mru = new LinkedQueue<Pair<K,T>> ();;
		}
		
		
		// Properties
		
		public long CurrentUtilization
			{ get { return _curresource; } }
		
		public long MaxUtilization
			{ get { return _maxresource; } set { _maxresource = value; } }

		
		// Accessors
		
		
		/// <summary>
		/// Access element in cache by key (retrieving or setting)
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public T this[K key]
		{
			get 
			{
				Pair<K,T> node = null;
				if (!_cache.TryGetValue(key, out node))
					return default(T); 
						
				Touch(node);
				return node.Obj;
			}
			set 
			{
				Pair<K,T> node = null;
				if (_cache.TryGetValue(key, out node))
				{
					node.Obj = value;
					Touch(node);
				}
				else
				{
					New (key, value);
					Prune();
				}
			}
		}
		
		
		/// <summary>
		/// Remove object associated with specific key
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public bool Remove (K key)
		{
			Pair<K,T> node = null;
			if (!_cache.TryGetValue(key, out node))
				return false; 

			var obj = node.Obj;
			_cache.Remove(key);
			_mru.Remove (node);
			
			if (_disposal != null)
				_disposal(obj);
			
			return true;
		}
		
		
		/// <summary>
		/// Clear the cache
		/// </summary>
		public void Clear ()
		{
			foreach (Pair<K,T> node in _mru)
			{
				if (_disposal != null)
					_disposal (node.Obj);
			}

			// clear
			_cache.Clear();
			_mru .Clear();
			_curresource = 0;
		}
		
		
		// Meta

		
		public IEnumerator<KeyValuePair<K,T>> GetEnumerator ()
		{
			foreach (Pair<K,T> node in _mru)
				yield return new KeyValuePair<K, T> (node.Key, node.Obj);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (Pair<K,T> node in _mru)
				yield return new KeyValuePair<K, T> (node.Key, node.Obj);
		}
		
		
		#region Implementation

		
		private void Touch (Pair<K,T> node)
		{				
			_mru.MoveToFront (node);
		}
		
		
		private Pair<K,T> New (K key, T obj)
		{
			var node = new Pair<K,T> { Key = key, Obj = obj };

			_mru.Prepend (node);
			_cache [key] = node;

			_curresource += _measure(obj);
			return node;
		}
		
		
		/// <summary>
		/// Remove objects in LRU order until resource utilization satisfied, however preserving at least one entry
		/// </summary>
		private void Prune ()
		{
			if (_curresource <= _maxresource || _cache.Count == 0)
				return;

			var node = _mru.Back;
			while (node != null && _curresource > _maxresource)
			{
				var pnode = node.Prior;
				var obj = node.Obj;
				var key = node.Key;

				// adjust resource utilization down
				_curresource -= _measure(obj);

				// remove from map
				_cache.Remove (key);
				// remove node
				_mru.Remove (node);

				// dispose
				if (_disposal != null)
					_disposal (obj);

				// setup for next							
				node = pnode;
			}

			if (node == null)
				_curresource = 0;
		}


		#endregion

		
		// Variables
		
		private ResourceUsage<T>			_measure;
		private long						_maxresource;
		private long						_curresource;
		private Disposal<T> 				_disposal;
		
		private IDictionary<K,Pair<K,T>>	_cache = new Dictionary<K,Pair<K,T>>();
		private LinkedQueue<Pair<K,T>>		_mru;
	}
	
	
	/// <summary>
	/// LRU linked list node
	/// </summary>
	sealed class Pair<K,T> : LinkedNode<Pair<K,T>>
	{
		public K		Key;
		public T		Obj;
	}
}


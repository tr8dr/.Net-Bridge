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

namespace bridge.common.collections
{
	/// <summary>
	/// Sorted list
	/// </summary>
	public class SortedList<V> : IList<V>
	{
		public SortedList (Comparison<V> cmp, int capacity)
		{
			_cmp = cmp;
			_list = new List<V> (capacity);
		}

		
		public SortedList (Comparison<V> cmp)
		{
			_cmp = cmp;
			_list = new List<V> ();
		}
		
		
		// Properties
		
		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		public bool IsReadOnly
			{ get { return false; } } 
		
		/// <summary>
		/// Gets the # of elements in the list
		/// </summary>
		public int Count
			{ get { return _list.Count; } }
		
		
		/// <summary>
		/// Gets the ith element in the list
		/// </summary>
		/// <param name='ith'>
		/// Ith.
		/// </param>
		public V this[int ith]
		{ 
			get 
			{ 
				return _list[ith]; 
			} 
			set 
			{ 
				throw new ArgumentException ("cannot support setting list elements by index, as is a sorted list"); 
			}
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Add the specified item (in sorted order)
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Add (V item)
		{
			if (_list.Count > 0)
			{
				var idx = FindLE (item);
				_list.Insert (idx+1, item);
			} 
			else
			{
				_list.Add (item);
			}
		}
		
		
		/// <summary>
		/// Adds the elements of the specified collection to the end of the list.
		/// </summary>
		/// <param name='collection'>
		/// The collection whose elements are added to the end of the list.
		/// </param>
		public void AddRange (IEnumerable<V> collection)
		{
			_list.AddRange (collection);
			_list.Sort (_cmp);
		}
		
		
		/// <summary>
		/// Clear the list
		/// </summary>
		public void Clear ()
		{
			_list.Clear();
		}
		
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name='item'>
		/// item to determine containment for
		/// </param>
		public bool Contains (V item)
		{
			return _list.Contains(item);
		}
		
		
		/// <summary>
		/// Returns the index of given item
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public int IndexOf (V item)
		{
			return _list.IndexOf (item);
		}
		
		
		/// <summary>
		/// Insert the specified item at index.
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Insert (int index, V item)
		{
			_list.Insert (index, item);
			_list.Sort (_cmp);
		}
		
		
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name='item'>
		/// item to be removed
		/// </param>
		public bool Remove (V item)
		{
			return _list.Remove (item);
		}
		
		
		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		public void RemoveAt (int index)
		{
			_list.RemoveAt (index);
		}
		
		
		/// <summary>
		/// Gets the sublist starting from index of given length
		/// </summary>
		/// <returns>
		/// The range.
		/// </returns>
		/// <param name='start'>
		/// Start.
		/// </param>
		/// <param name='length'>
		/// Length.
		/// </param>
		public SortedList<V> GetRange (int start, int length)
		{
			var nlist = new SortedList<V> (_cmp, length);
			for (int i = 0 ; i < length ; i++)
				nlist._list[i] = _list[i + start];
			
			return nlist;
		}
		
		
		/// <summary>
		/// Removes the given range
		/// </summary>
		/// <param name='start'>
		/// Start index
		/// </param>
		/// <param name='length'>
		/// Length of range to be removed
		/// </param>
		public void RemoveRange (int start, int length)
		{
			_list.RemoveRange (start, length);
		}
		
		
		// Meta
		
		
		public void CopyTo (V[] array, int arrayIndex)
		{
			_list.CopyTo (array, arrayIndex);
		}
		
		
		public override bool Equals (object obj)
		{
			return _list.Equals (obj);
		}
		
		
		public System.Collections.IEnumerator GetEnumerator ()
		{
			return _list.GetEnumerator();
		}
		
		IEnumerator<V> IEnumerable<V>.GetEnumerator ()
		{
			return _list.GetEnumerator();
		}
		
		public override int GetHashCode ()
		{
			return _list.GetHashCode ();
		}
		
		
		// Implementation
		
		
		
		/// <summary>
		/// Finds the max(v[index]) <= element
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		private int FindLE (V item)
		{
			var Istart = 0;
			var Iend = Count-1;
			var Iguess = 0;
			var cmp =0;
			
			while (Istart < Iend)
			{
				Iguess = Istart + (Iend - Istart) / 2;
				V v = _list[Iguess];
				
				cmp = _cmp (item, v);
				if (cmp == 0)
					return Iguess;
				
				if (Iguess == Istart)
					return Iguess;
				
				if (cmp > 0)
					Istart = Iguess; 
				else
					Iend = Iguess;			
			}
			
			if (cmp > 0)
				return Iguess;
			else
				return -1;
			
		}
		
		
		// Variables
		
		private Comparison<V>		_cmp;
		private List<V>				_list;
	}
}


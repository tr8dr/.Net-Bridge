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


namespace bridge.math.matrix
{
	/// <summary>
	/// List of names used as indices for rows or columns
	/// <para>
	/// Keeps track of position of elements in list so that can directly look into a matrix structure
	/// </para>
	/// </summary>
	public class IndexByName<T> : List<T>, IIndexByName
	{
		public IndexByName (params T[] names)
		{
			int i = 0;
			foreach (T item in names)
			{
				Add(item);
				_ordering[item.ToString()] = i++;
			}
		}
		
		public IndexByName (IEnumerable<T> c)
		{
			int i = 0;
			foreach (T item in c)
			{
				Add(item);
				_ordering[item.ToString()] = i++;
			}
		}
		
		
		// Properties
		
		
		/// <summary>
		/// Given a name determines the position in matrix or other structure
		/// </summary>
		/// <value>
		/// The ordering.
		/// </value>
		public Dictionary<string,int> Ordering 
			{ get { return _ordering;} }
		
		
		/// <summary>
		/// Gets the name list as string[]
		/// </summary>
		public string[] NameList
		{ 
			get 
			{ 
				string[] list = new string[Count];
				for (int i = 0 ; i < Count ; i++)
					list[i] = this[i].ToString();
				
				return list;
			} 
		}

		
		// Operations
		
		
		/// <summary>
		/// Add the specified name.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public new void Add (T name)
		{
			base.Add (name);
			
			var key = name.ToString();
			_ordering[key] = Count-1;
		}

		
		/// <summary>
		/// Removes name at index, shifting list
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		public new void RemoveAt (int index)
		{
			base.RemoveAt(index);
			_ordering.Clear();
			
			int i = 0;
			foreach (var o in this)
				_ordering[o.ToString()] = i++; 
		}
		
		
		/// <summary>
		/// Clear list
		/// </summary>
		public new void Clear ()
		{
			base.Clear();
			_ordering.Clear();
		}
		
		
		/// <summary>
		/// Index of the name
		/// </summary>
		/// <param name='item'>
		/// name
		/// </param>
		public new int IndexOf (T name)
		{
			return _ordering[name.ToString()];
		}


		/// <summary>
		/// Index of the name
		/// </summary>
		/// <param name='item'>
		/// name
		/// </param>
		public bool TryIndexOf (T name, out int idx)
		{
			return _ordering.TryGetValue (name.ToString(), out idx);
		}

		
		/// <summary>
		/// Insert the name at specified index
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		public new void Insert (int index, T name)
		{
			base.Insert(index, name);
			_ordering.Clear();
			
			int i = 0;
			foreach (var o in this)
				_ordering[o.ToString()] = i++; 
		}
		
		/// <summary>
		/// Converts index to string[]
		/// </summary>
		/// <returns>
		/// The new array containing a copy of the list's elements.
		/// </returns>
		public new string[] ToArray()
		{
			string[] list = new string[Count];
			for (int i = 0 ; i < Count ; i++)
				list[i] = this[i].ToString ();
			
			return list;
		}
		
		
		// Variables
		
		private Dictionary<string,int>	_ordering = new Dictionary<string,int>();
	}
}


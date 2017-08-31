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

namespace bridge.common.collections
{
	/// <summary>
	/// Comparison delegate, returns {-1, 0, 1} to indicate <, = , > relationships
	/// </summary>
	public delegate int Compare<in T> (T a, T b);
	
	
	/// <summary>
	/// Compare delegate implementations
	/// </summary>
	public static class Comparisons
	{
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (int a, int b)
		{
			return a - b;
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (int a, int b)
		{
			return b - a;
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (long a, long b)
		{
			if (a > b)
				return 1;
			if (b < a)
				return -1;
			else
				return 0;
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (long a, long b)
		{
			if (a > b)
				return -1;
			if (b < a)
				return 1;
			else
				return 0;
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (string a, string b)
		{
			return a.CompareTo(b);
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (string a, string b)
		{
			return b.CompareTo(a);
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompareIgnoreCase (string a, string b)
		{
			return string.Compare (a, b, StringComparison.OrdinalIgnoreCase);
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompareIgnoreCase (string a, string b)
		{
			return string.Compare (b, a, StringComparison.OrdinalIgnoreCase);
		}

	}
}


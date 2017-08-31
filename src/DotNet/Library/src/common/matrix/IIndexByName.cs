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


namespace bridge.math.matrix
{
	/// <summary>
	/// List of names used as indices for rows or columns
	/// <para>
	/// Keeps track of position of elements in list so that can directly look into a matrix structure
	/// </para>
	/// </summary>
	public interface IIndexByName
	{		
		/// <summary>
		/// Given a name determines the position in matrix or other structure
		/// </summary>
		/// <value>
		/// The ordering.
		/// </value>
		Dictionary<string,int> 				Ordering		{ get; }
		
		/// <summary>
		/// Gets the name list as string[]
		/// </summary>
		string[]							NameList		{ get; }

		/// <summary>
		/// Gets the count.
		/// </summary>
		int									Count			{ get; }
	}
}


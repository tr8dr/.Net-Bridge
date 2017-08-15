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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using bridge.common.parsing;
using System.Collections.Generic;
using bridge.common.utils;
using bridge.common.serialization;
using System.Text;
using System.Threading;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace bridge.math.matrix
{
	/// <summary>
	/// Dense vector with index
	/// </summary>
	public sealed class IndexedVector : DenseVector
	{
		
		/// <summary>
		/// Initializes a new vector with given size
		/// </summary>
		/// <param name='size'>
		/// length of vector
		/// </param>
		/// <param name='names'>
		/// Optional name-based index
		/// </param>
		public IndexedVector (int size, IIndexByName names = null)
			: base (size)
		{
			_index = names;
		}


		/// <summary>
		/// Initializes a new vector with data array
        /// </summary>
        /// <param name="data">raw vector in form of array</param>
		/// <param name='names'>Optional name-based index</param>
		public IndexedVector(double[] data, IIndexByName names = null)
			: base(data)
		{
			_index = names;
		}


		/// <summary>
		/// Initializes a new vector with data array
		/// </param>
		/// <param name='names'>
		/// Optional name-based index
		/// </param>
		public IndexedVector(double[] data, string[] names)
			: base(data)
		{
			if (names != null)
				_index = new IndexByName<string>(names);
			else
				_index = null;
		}

		
		// Properties
		

        /// <summary>
        /// Gets the vector as an array
        /// </summary>
        public double[] Data
            { get { return base.AsArray(); } }

		
		/// <summary>
		/// Gets / Sets the named index for this vector (may be null if not provided)
		/// </summary>
		public IIndexByName Indices
			{ get { return _index; } set { _index = value; } }
		
		/// <summary>
		/// Gets the row names as string[]
		/// </summary>
		public string[] Names
			{ get { return _index != null ? _index.NameList : null; } }
		
		
		// Operations
					
		
		/// <summary>
		/// Gets or sets the element given by name on the vector
		/// </summary>
		/// <param name='name'>
		/// Row index name
		/// </param>
		public double this [string name]
		{
			get 
			{ 
				int idx = _index.Ordering[name];
				return base.At(idx); 
			}
			set 
			{ 
				int idx = _index.Ordering[name];
				base.At(idx, value); 
			}
		}


        // Variables

        private IIndexByName _index;
	}

}



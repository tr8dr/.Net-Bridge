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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace bridge.math.matrix
{
	/// <summary>
	/// Sub vector is a read-only view on a sub-range of a parent vector
	/// </summary>
	public class EvaluatedVector : Vector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.SubVector"/> class.
		/// </summary>
		/// <param name='underlier'>
		/// Underlying parent vector
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='n'>
		/// Length of sub vector or defaults to remaining length from offset
		/// </param>
		public EvaluatedVector (int size, Func<int,double> filler)
			: base (new EvaluatedStorage (size, filler))
		{
		}
		
		
		#region Implementation

		private class EvaluatedStorage : VectorStorage<double>
		{
			public EvaluatedStorage (int size, Func<int,double> filler)
				: base (size)
			{
				_filler = filler;
			}

			// Properties

			/// <summary>
			/// True if the vector storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }


			// Functions

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int index)
			{
				return _filler (index);
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="value">The value to set the element to. </param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int index, double value)
			{
				throw new AccessViolationException ("cannot write to an evaluated vector");
			}


			// Variables

			private Func<int,double> _filler;
		}


		#endregion
	}
}


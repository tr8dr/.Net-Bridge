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
using MathNet.Numerics.LinearAlgebra.Storage;

namespace bridge.math.matrix
{
	/// <summary>
	/// Column View vector is a read-only view on a matrix column
	/// </summary>
	public class ColumnViewVector : Vector
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
		public ColumnViewVector (Matrix<double> underlier, int col)
			: base (new SubviewStorage (underlier, col))
		{
			_data = (SubviewStorage)Storage;
		}

		// Properties

		public int Column
			{ get { return ((SubviewStorage)Storage).Column; } }

		public Matrix<double> Underlier
			{ get { return ((SubviewStorage)Storage).Underlier; } }


		#region Storage

		/// <summary>
		/// Storage for subview
		/// </summary>
		private class SubviewStorage : VectorStorage<double>
		{
			public SubviewStorage (Matrix<double> underlier, int col)
				: base (underlier.RowCount)
			{
				Underlier = underlier;
				Column = col;
			}

			// properties

			/// <summary>
			/// The underlier.
			/// </summary>
			public Matrix<double> Underlier;

			/// <summary>
			/// The offset.
			/// </summary>
			public int Column;
		
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
				return Underlier [index, Column];
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="value">The value to set the element to. </param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int index, double value)
			{
				Underlier [index, Column] = value;
			}
		}

		#endregion

		// Variables

		private SubviewStorage		_data;
	}
}


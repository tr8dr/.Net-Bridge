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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Storage;


namespace bridge.math.matrix
{
	/// <summary>
	/// View onto a matrix
	/// </summary>
	public class SubviewMatrix : Matrix
	{
		public SubviewMatrix (Matrix<double> underlier, int rowoffset, int coloffset, int nrows, int ncols)
			: base (new SubviewStorage (underlier, rowoffset, coloffset, nrows, ncols))
		{
			_data = (SubviewStorage)Storage;
		}


		/// <summary>
		/// Gets the underlying data for this matrix
		/// </summary>
		public double[] Data
			{ get { return MatrixUtils.DataOf (_data.Underlier); } }


		#region Storage

		/// <summary>
		/// Storage for subview
		/// </summary>
		private class SubviewStorage : MatrixStorage<double>
		{
			public SubviewStorage (Matrix<double> underlier, int rowoffset, int coloffset, int nrows, int ncols)
				: base (nrows, ncols)
			{
				Underlier = underlier;
				RowOffset = rowoffset;
				ColOffset = coloffset;
			}

			// properties

			/// <summary>
			/// The underlier.
			/// </summary>
			public Matrix<double> Underlier;

			/// <summary>
			/// The row offset.
			/// </summary>
			public int RowOffset;

			/// <summary>
			/// The column offset.
			/// </summary>
			public int ColOffset;

			/// <summary>
			/// True if the matrix storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }

			/// <summary>
			/// True if all fields of this matrix can be set to any value.
			///  False if some fields are fixed, like on a diagonal matrix.
			/// </summary>
			/// <value><c>true</c> if this instance is fully mutable; otherwise, <c>false</c>.</value>
			public override bool IsFullyMutable
				{ get { return true; } }


			// Functions

			/// <summary>
			/// True if the specified field can be set to any value.
			///  False if the field is fixed, like an off-diagonal field on a diagonal matrix.
			/// </summary>
			/// <returns><c>true</c> if this instance is mutable at the specified row column; otherwise, <c>false</c>.</returns>
			/// <param name="row">Row.</param>
			/// <param name="column">Column.</param>
			public override bool IsMutableAt (int row, int column)
			{
				return true;
			}

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="row">The row of the element.</param>
			/// <param name="column">The column of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int ri, int ci)
			{
				return Underlier [RowOffset + ri, ColOffset + ci];
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="row">The row of the element.</param>
			/// <param name="column">The column of the element.</param>
			/// <param name="value">The value to set the element to.</param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int ri, int ci, double value)
			{
				Underlier [RowOffset + ri, ColOffset + ci] = value;
			}
		}

		#endregion


		// Variables

		private SubviewStorage _data;
	}
}


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
	/// Matrix with named indices & expansion
	/// </summary>
	public class IndexedMatrix : DenseMatrix
	{
		/// <summary>
		/// Matrix with no size (and no data allocation)
		/// <p/>
		/// Should only be used in the context of serialization or internal functions
		/// </summary>
		public IndexedMatrix ()
			: this (0,0)
		{
		}
			
				
		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			int rows, int cols, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)
			
			: base (rows, cols)
		{
			_rowindices = rownames;
			_colindices = colnames;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.IndexedMatrix"/> class.
		/// </summary>
		/// <param name="src">Source matrix.</param>
		/// <param name="rownames">Row names.</param>
		/// <param name="colnames">Col names.</param>
		public IndexedMatrix (
			Matrix<double> src, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)

			: base (src.RowCount, src.ColumnCount)
		{
			_rowindices = rownames;
			_colindices = colnames;
			src.CopyTo (this);
		}


		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			double[] data,
			int rows, int cols, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)

			: base (rows, cols, data)
		{
			_rowindices = rownames;
			_colindices = colnames;
		}


		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			double[] data,
			int rows, int cols, 
			string[] rownames = null, 
			string[] colnames = null)

			: base (rows, cols, data)
		{
			_rowindices = rownames != null ? new IndexByName<string> (rownames) : null;
			_colindices = colnames != null ? new IndexByName<string> (colnames) : null;
		}

		
		// Properties
		
		
		/// <summary>
		/// Gets the named row indices for this matrix (may be null if not given)
		/// </summary>
		public IIndexByName RowIndices
			{ get { return _rowindices; } }
		
		/// <summary>
		/// Gets the named col indices for this matrix (may be null if not given)
		/// </summary>
		public IIndexByName ColIndices
			{ get { return _colindices; } }
		
		/// <summary>
		/// Gets the row names as string[]
		/// </summary>
		public string[] RowNames
			{ get { return _rowindices != null ? _rowindices.NameList : null; } }
		
		/// <summary>
		/// Gets the col names as string[]
		/// </summary>
		public string[] ColNames
			{ get { return _colindices != null ? _colindices.NameList : null; } }
		
		
		/// <summary>
		/// Gets the underlying data for this vector
		/// </summary>
		public double[] Data
			{ get { return ((DenseColumnMajorMatrixStorage<double>)Storage).Data; } }
		
		
		// Functions
				
		
		/// <summary>
		/// Gets or sets the element given by row,col name on the matrix
		/// </summary>
		/// <param name='row'>
		/// Row index name
		/// </param>
		/// <param name='col'>
		/// Col index name
		/// </param>
		public virtual double this [string row, string col]
		{
			get 
			{ 
				int ridx = _rowindices.Ordering[row];
				int cidx = _colindices.Ordering[col];
				return base[ridx, cidx]; 
			}
			set 
			{ 
				int ridx = _rowindices.Ordering[row];
				int cidx = _colindices.Ordering[col];
				base[ridx, cidx] = value; 
			}
		}
		
		
		/// <summary>
		/// Sets all values in the matrix to given value
		/// </summary>
		/// <param name='newv'>
		/// New value to set
		/// </param>
		public void SetAll (double newv)
		{
			var n = RowCount*ColumnCount;
			var data = Data;

			for (int i = 0 ; i < n ; i++)
				data[i] = newv;
		}


		/// <summary>
		/// Rolls the rows in the matrix up or down, removing the K rows and filling with the lower (upper).   The
		/// remaining rows at the end of the shift have undefined values and should be overwritten.
		/// </summary>
		/// <param name="shift">If shift is negative, shifts rows up, and positive, shifts down</param>
		public void ShiftRows (int shift)
		{
			var ncol = ColumnCount;
			var nrow = RowCount;

			var data = Data;
			if (shift < 0)
			{
				for (int ci = 0; ci < ncol; ci++)
				{
					Array.Copy (data, ci * ncol - shift, data, ci * ncol, (nrow + shift));
				}
			}
			else
			{
				for (int ci = 0; ci < ncol; ci++)
				{
					Array.Copy (data, ci * ncol, data, ci * ncol + shift, (nrow - shift));
				}
			}
		}

					
		
		// Variables

		protected IIndexByName		
			_rowindices;
		protected IIndexByName		
			_colindices;
	}
}


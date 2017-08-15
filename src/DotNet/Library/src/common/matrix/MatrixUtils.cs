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
using MathNet.Numerics.LinearAlgebra;

namespace bridge.math.matrix
{
	public class MatrixUtils
	{
		public enum InterpStyle
			{ Linear, Flat }

		/// <summary>
		/// Gets the underlying matrix data
		/// </summary>
		/// <param name='m'>
		/// M.
		/// </param>
		public static double[] DataOf (Matrix<double> matrix)
		{
			IndexedMatrix imat = matrix as IndexedMatrix;
			if (imat != null)
				return imat.Data;
			
			DenseMatrix dmat = matrix as DenseMatrix;
			if (dmat != null)
				return dmat.Values;
			else
				throw new ArgumentException ("cannot get underlying data for matrix of type: " + matrix.GetType());
		}
		

		/// <summary>
		/// Get the data for a vector
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="v">V.</param>
		/// <param name="copyif">If set to <c>true</c> will copy the data if inaccessible.</param>
		public static double[] DataOf (Vector<double> v, bool copyif = true)
		{
			var va = v as IndexedVector;
			if (va != null)
				return va.Data;
			
			var vb = v as SubviewVector;
			if (vb != null)
				return vb.Data;
			
			var vc = v as DenseVector;
			if (vc != null)
				return vc.Values;

			if (!copyif)
				throw new ArgumentException ("cannot access the data for the given vector, without copying");

			double[] data = new double[v.Count];
			for (int i = 0 ; i < v.Count; i++)
				data[i] = v[i];
			
			return data;
		}
		
		
		/// <summary>
		/// Get offset within raw data for start of vector
		/// </summary>
		public static int DataOffsetOf (Vector<double> v)
		{
			var va = v as SubviewVector;
			if (va != null)
				return va.DataOffset;
			else
				return 0;
		}


		/// <summary>
		/// Set all values of a vector to constant
		/// </summary>
		/// <param name="v">Vector.</param>
		/// <param name="constant">value to set in</param>
		public static void SetAll (Vector<double> v, double constant)
		{
			var len = v.Count;
			for (int i = 0; i < len; i++)
				v[i] = constant;
		}


		/// <summary>
		/// Indices of this vector (as string[] or null)
		/// </summary>
		/// <param name="vec">Vec.</param>
		public static IIndexByName IndicesOf (Vector<double> vec)
		{
			var ivec = vec as IndexedVector;
			if (ivec != null)
				return ivec.Indices != null ? ivec.Indices : null;
			else
				return null;
		}
		
		
		/// <summary>
		/// Row Indices of this matrix (as string[] or null)
		/// </summary>
		/// <param name="mat">Matrix.</param>
		public static IIndexByName RowIndicesOf (Matrix<double> mat)
		{
			var imat = mat as IndexedMatrix;
			if (imat != null)
				return imat.RowIndices != null ? imat.RowIndices : null;
			else
				return null;
		}
		
		
		/// <summary>
		/// Column Indices of this matrix (as string[] or null)
		/// </summary>
		/// <param name="mat">Matrix.</param>
		public static IIndexByName ColIndicesOf (Matrix<double> mat)
		{
			var imat = mat as IndexedMatrix;
			if (imat != null)
				return imat.ColIndices != null ? imat.ColIndices : null;
			else
				return null;
		}


		/// <summary>
		/// Converts the given matrix to an indexed matrix
		/// </summary>
		/// <param name='m'>
		/// M.
		/// </param>
		public static IndexedMatrix ToIndexedMatrix (Matrix<double> matrix)
		{
			IndexedMatrix imat = matrix as IndexedMatrix;
			if (imat != null)
				return imat;

			IIndexByName index = null;
			DenseMatrix dmat = matrix as DenseMatrix;
			if (dmat != null)
				return new IndexedMatrix (dmat.Values, dmat.RowCount, dmat.ColumnCount, index, index);

			imat = new IndexedMatrix (matrix.RowCount, matrix.ColumnCount, index, index);
			for (int ri = 0 ; ri < matrix.RowCount ; ri++)
			{
				for (int ci = 0 ; ci < matrix.ColumnCount ; ci++)
					imat[ri,ci] = matrix[ri,ci];
			}

			return imat;
		}

		
		/// <summary>
		/// Convert vectors that are either not dense or not indexed to dense
		/// </summary>
		public static Vector<double> ToDenseVector (Vector<double> v)
		{
			var va = v as IndexedVector;
			if (va != null)
				return va;
			
			var vc = v as DenseVector;
			if (vc != null)
				return vc;
			
			var len = v.Count;
			DenseVector newv = new DenseVector (len);
			
			for (int i = 0 ; i < len ; i++)
				newv[i] = v[i];
			
			return newv;
		}


		/// <summary>
		/// Combine vectors as columns to create new matrix
		/// </summary>
		/// <param name="cvecs">Column vector list</param>
		public static Matrix<double> CBind (Vector<double>[] cvecs)
		{
			var v1 = cvecs [0];
			IIndexByName idx = (v1 is IndexedVector) ? ((IndexedVector)v1).Indices : null;

			var mat = new IndexedMatrix (v1.Count, cvecs.Length, idx, null);
			for (int ci = 0 ; ci < cvecs.Length ; ci++)
			{
				var v = cvecs [ci];
				for (int ri = 0; ri < v1.Count; ri++)
					mat [ri, ci] = v [ri];
			}

			return mat;
		}

		
		/// <summary>
		/// Random matrix
		/// </summary>
		/// <returns>
		/// The matrix.
		/// </returns>
		/// <param name='nrows'>
		/// Nrows.
		/// </param>
		/// <param name='ncols'>
		/// Ncols.
		/// </param>
		public static DenseMatrix RandomMatrix (int nrows, int ncols)
		{
			DenseMatrix m = new DenseMatrix (nrows, ncols);
			Random rand = new Random ();
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					m[ri,ci] = rand.NextDouble();
				}
			}
			
			return m;
		}
		
		
		/// <summary>
		/// Determines whether indexed matrix contains a seried of bars, based on column headers
		/// </summary>
		public static bool IsBars (IndexedMatrix m)
		{
			IIndexByName colnames = m.ColIndices;
			if (colnames == null)
				return false;
			
			var ordering = colnames.Ordering;
			if (ordering.ContainsKey ("close") || ordering.ContainsKey("Close"))
				return true;
			else
				return false;
		}
		
		
		/// <summary>
		/// Determines whether indexed matrix contains a seried of quotes, based on column headers
		/// </summary>
		public static bool IsQuotes (IndexedMatrix m)
		{
			IIndexByName colnames = m.ColIndices;
			if (colnames == null)
				return false;
			
			var ordering = colnames.Ordering;
			if (ordering.ContainsKey ("bid") || ordering.ContainsKey("Bid"))
				return true;
			else
				return false;
		}


		/// <summary>
		/// Fills in missing values, interpolating in a flat or linear fashion
		/// </summary>
		/// <param name="vec">Vector to fill.</param>
		/// <param name="style">Interpolation style.</param>
		public static void FillNA (Vector<double> vec, InterpStyle style)
		{
			var nrows = vec.Count;

			// find 1st non-NA
			var iend = 0;
			while (iend < nrows && Double.IsNaN(vec[iend])) iend++;

			// fill in prior NA stub
			for (int ri = 0 ; ri < iend; ri++)
				vec [ri] = vec [iend];

			var istart = 0;
			while (iend < nrows) 
			{
				istart = iend + 1;
				while (istart < nrows && !Double.IsNaN(vec[istart])) istart++;
				iend = istart;
				while (iend < nrows && Double.IsNaN(vec[iend])) iend++;

				var Vs = vec[istart-1];
				var Ve = (iend < nrows && style == InterpStyle.Linear) ? vec[iend] : Vs;

				var dpdt = (Ve-Vs) / (iend-istart+1);
				for (int i = istart ; i < iend ; i++)
				{
					Vs += dpdt;
					vec[i] = Vs;
				}
			}
		}
		
		
	}
}


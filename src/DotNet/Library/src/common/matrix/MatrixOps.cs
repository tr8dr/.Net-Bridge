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
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace bridge.math.matrix
{
	/// <summary>
	/// Various matrix operations
	/// </summary>
	public static class MatrixOps
	{
		/// <summary>
		/// Scale columns of transposed of matrix by vector entries C = A^T Diag(b)
		/// </summary>
		/// <param name='A'>
		/// 	matrix A
		/// </param>
		/// <param name='b'>
		/// 	diagonal vector b
		/// </param>
		/// <returns>
		/// 	new matrix C = A^T Diag(b)
		/// </returns>
		public static Matrix<double> MultMtD (Matrix<double> A, Vector<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, A.RowCount);
			
			var n = A.RowCount;
			var m = A.ColumnCount;
			
			Debug.Assert (n == b.Count, "mismatch between number of rows in A and diagonal vector");
			
			for (int ri = 0 ; ri < n ; ri++)
			{
				var scalar = b[ri];				
				for (int ci = 0 ; ci < m ; ci++)
				{
					mout[ci,ri]= A[ri,ci] * scalar;
				}
			}
			
			return mout;
		}
		
		
		/// <summary>
		/// Multiply M'M with a scale
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultSMtM (Matrix<double> A, double S = 1.0, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, A.ColumnCount);

			var nrow = A.RowCount;
			var ncol = A.ColumnCount;

			for (int ri = 0 ; ri < ncol ; ri++)
			{
				for (int ci = 0 ; ci < ncol ; ci++)
				{
					double sum = 0;
					for (int i = 0 ; i < nrow ; i++)
						sum += A[i,ri] * A[i,ci];
					
					mout [ri,ci] = sum * S;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply M' D M, where D is a diagonalized vector
		/// </summary>
		/// <param name='M'>matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name='mout'>Output matrix.</param>
		public static Matrix<double> MultMtDM (Matrix<double> M, Vector<double> D, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (M.ColumnCount, M.ColumnCount);

			var nrow = M.RowCount;
			var ncol = M.ColumnCount;
			for (int ri = 0; ri < ncol; ri++) 
			{
				for (int ci = 0; ci < ncol; ci++) 
				{
					double sum = 0;
					for (int i = 0; i < nrow; i++)
						sum += M [i, ri] * D[i] * M [i, ci];

					mout [ri, ci] = sum;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply X' D Y, where D is a diagonalized vector
		/// </summary>
		/// <param name='X'>X matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name="Y">Y matrix</param>
		/// <param name='mout'>Output matrix.</param>
		public static Matrix<double> MultXtDY (Matrix<double> X, Vector<double> D, Matrix<double> Y, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (X.ColumnCount, Y.ColumnCount);

			var nrow = X.RowCount;
			var ncol1 = X.ColumnCount;
			var ncol2 = Y.ColumnCount;

			for (int ri = 0; ri < ncol1; ri++) 
			{
				for (int ci = 0; ci < ncol2; ci++) 
				{
					double sum = 0;
					for (int i = 0; i < nrow; i++)
						sum += X [i, ri] * D [i] * Y [i, ci];

					mout [ri, ci] = sum;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply X' D y, where D is a diagonalized vector
		/// </summary>
		/// <param name='M'>matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name="y">Y vector</param>
		/// <param name='mout'>Output matrix.</param>
		public static Vector<double> MultXtDy (Matrix<double> X, Vector<double> D, Vector<double> y, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (X.ColumnCount);

			var nrow = X.RowCount;
			var ncol = X.ColumnCount;

			for (int ri = 0; ri < ncol; ri++) 
			{
				double sum = 0;
				for (int i = 0; i < nrow; i++)
					sum += X [i, ri] * D [i] * y [i];

				vout [ri] = sum;
			}

			return vout;
		}
		
		
		/// <summary>
		/// Multiply A'B, where A & B are separate matrices, but 
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMtM (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, B.ColumnCount);
			
			A.TransposeThisAndMultiply (B, mout);
			return mout;
		}
		
		/// <summary>
		/// Multiply AB', where A & B are separate matrices, but 
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMMt (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.RowCount, B.RowCount);
			
			A.TransposeAndMultiply (B, mout);
			return mout;
		}
		
		/// <summary>
		/// Multiply two matrices
		/// </summary>
		/// <returns>
		/// O = A * B
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMM (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.RowCount, B.ColumnCount);
			
			A.Multiply (B, mout);
			return mout;
		}
		
		
		/// <summary>
		/// Multiply matrix with vector v = A b
		/// </summary>
		/// <returns>
		/// v = A * b
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> MultMV (Matrix<double> A, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (A.RowCount);
			
			A.Multiply (b, vout);
			return vout;
		}
		
		/// <summary>
		/// Multiply matrix transpose with vector v = A'b
		/// </summary>
		/// <returns>
		/// v = A' * b
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> MultMtV (Matrix<double> A, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (A.ColumnCount);
			
			A.TransposeThisAndMultiply (b, vout);
			return vout;
		}
		
			
		/// <summary>
		/// Add two vectors
		/// </summary>
		/// <returns>
		/// v = a + b
		/// </returns>
		/// <param name='a'>
		/// vector a
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> AddVV (Vector<double> a, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (a.Count);
			
			var n = a.Count;
			
			for (int i = 0 ; i < n ; i++)
				vout[i] = a[i] + b[i];
			
			return vout;
		}

		/// <summary>
		/// Dot product of 2 vectors
		/// </summary>
		/// <returns>The product.</returns>
		/// <param name="a">The 1st vectpr.</param>
		/// <param name="b">The 2nd vector.</param>
		public static double DotProduct (Vector<double> a, Vector<double> b)
		{
			var n = a.Count;
			var cum = 0.0;
			for (int i = 0 ; i < n ; i++)
				cum += a[i] * b[i];

			return cum;
		}

			
		/// <summary>
		/// Add two matrix
		/// </summary>
		/// <returns>
		/// m = a + S b
		/// </returns>
		/// <param name='a'>
		/// matrix a
		/// </param>
		/// <param name='b'>
		/// matrix b.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> AddMM (Matrix<double> a, Matrix<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (a.RowCount, a.ColumnCount);
			
			var nrows = a.RowCount;
			var ncols = a.ColumnCount;
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					mout[ri,ci] = a[ri,ci] + b[ri,ci];
				}
			}

			return mout;
		}
		
			
		/// <summary>
		/// Subtract two matrices
		/// </summary>
		/// <returns>
		/// m = a - b
		/// </returns>
		/// <param name='a'>
		/// matrix a
		/// </param>
		/// <param name='b'>
		/// matrix b.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> SubMM (Matrix<double> a, Matrix<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (a.RowCount, a.ColumnCount);
			
			var nrows = a.RowCount;
			var ncols = a.ColumnCount;
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					mout[ri,ci] = a[ri,ci] - b[ri,ci];
				}
			}

			return mout;
		}
		
			
		/// <summary>
		/// Subtract two vectors a-b
		/// </summary>
		/// <returns>
		/// v = a - b
		/// </returns>
		/// <param name='a'>
		/// vector a
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> SubVV (Vector<double> a, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (a.Count);
			
			var n = a.Count;
			
			for (int i = 0 ; i < n ; i++)
				vout[i] = a[i] - b[i];
			
			return vout;
		}

		
		/// <summary>
		/// Solve X x = b, for x
		/// </summary>
		/// <param name='A'>
		/// matrix A
		/// </param>
		/// <param name='b'>
		/// vector b
		/// </param>
		/// <param name='x'>
		/// output vector x
		/// </param>
		public static Vector<double> Solve (Matrix<double> A, Vector<double> b, Vector<double> x = null)
		{
			return MultMV (A.Inverse(), b, x);
		}


		/// <summary>
		/// Diagonal matrix from vector
		/// </summary>
		/// <param name="diag">Diagonal values.</param>
		public static Matrix<double> Diag (Vector<double> diag)
		{
			var n = diag.Count;
			var M = new DenseMatrix (n,n);

			for (int i = 0; i < n; i++)
				M [i, i] = diag [i];

			return M;
		}


		/// <summary>
		/// Identity matrix
		/// </summary>
		/// <param name="n">N.</param>
		public static Matrix<double> Identity (int n)
		{
			var M = new DenseMatrix (n,n);

			for (int i = 0; i < n; i++)
				M [i, i] = 1.0;

			return M;
		}


		/// <summary>
		/// Raise matrix to given power
		/// </summary>
		/// <param name="A">square matrix to be raised to power</param>
		/// <param name="pow">power to raise to.</param>
		public static Matrix<double> Pow (Matrix<double> A, double pow)
		{
			var svd = A.Svd ();
			var W = svd.W;
			var U = svd.U;

			var D = new DenseVector (W.RowCount);

			for (int i = 0; i < D.Count; i++)
				D [i] = Math.Pow (W [i, i], pow);

			return U * MatrixOps.Diag(D) * U.Transpose ();
		}


		/// <summary>
		/// Sets all values in vector to given
		/// </summary>
		/// <param name="vec">Vector.</param>
		/// <param name="v">Value to set to.</param>
		public static void SetAll (Vector<double> vec, double v)
		{
			for (int i = 0; i < vec.Count; i++)
				vec [i] = v;
		}
	}
}


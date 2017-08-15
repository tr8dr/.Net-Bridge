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

namespace bridge.common.utils
{
	/// <summary>
	/// Misc Array utils.
	/// </summary>
	public static class ArrayUtils
	{
		/// <summary>
		/// Fill the vector with given value
		/// </summary>
		/// <param name='vec'>
		/// Vector
		/// </param>
		/// <param name='val'>
		/// Value.
		/// </param>
		public static void Fill<T> (T[] vec, T val)
		{
			for (int i = 0 ; i < vec.Length ; i++)
				vec[i] = val;
		}


		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		public static int Find (double[] xvec, double x, int Istart = 0, int Iend = -1)
		{
			if (Iend < 0)
				Iend = xvec.Length - 1;
			
			var Xend = xvec[Iend];
			var Xstart = xvec[Istart];
			
			var m = (double)(Iend-Istart+1) / (Xend - Xstart);
			var Iguess = Istart + (int)(m * (x - Xstart));
			return Find (xvec, x, Istart, Iend, Iguess);
		}

		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		public static int Find (Vector<double> xvec, double x, int Istart = 0, int Iend = -1)
		{
			if (Iend < 0)
				Iend = xvec.Count - 1;
			
			var Xend = xvec[Iend];
			var Xstart = xvec[Istart];
			
			var m = (double)(Iend-Istart+1) / (Xend - Xstart);
			var Iguess = Istart + (int)(m * (x - Xstart));
			return Find (xvec, x, Istart, Iend, Iguess);
		}
		
		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		/// <param name='Iguess'>
		/// initial guess.
		/// </param>
		/// <returns>
		/// index of x with max(x) <= target x or -1 if cannot find
		/// </returns>
		public static int Find (double[] xvec, double x, int Istart, int Iend, int Iguess)
		{
			int Ilastguess = -1;
			
			while (Istart <= Iend)
			{
				if (Iguess < Istart)
					return Ilastguess;
				if (Iguess > Iend)
					return Iend;
				
				double Xguess = xvec[Iguess];
				
				if (Xguess == x)
					return Iguess;
	
				if (Xguess > x)
				{
					Iend = Iguess-1;
					if (Iend < Istart)
						return Ilastguess;
					if (Iend == Istart)
						return Iend;
				} 
				else
				{ 
					Istart = Iguess+1; 
					Ilastguess = Iguess; 
				}
				
				if (Istart >= Iend)
					return Ilastguess;
							
				double Xend = xvec[Iend];
				double Xstart = xvec[Istart];
				
				double m = (double)(Iend-Istart+1) / (double)(Xend - Xstart);
				Iguess = Istart + (int)((x - Xstart) * m);
			}
			
			if (Ilastguess != -1)
				return Ilastguess;
			if (xvec[Istart] <= x)
				return Istart;
			else
				return -1;
		}

		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		/// <param name='Iguess'>
		/// initial guess.
		/// </param>
		/// <returns>
		/// index of x with max(x) <= target x or -1 if cannot find
		/// </returns>
		public static int Find (Vector<double> xvec, double x, int Istart, int Iend, int Iguess)
		{
			int Ilastguess = -1;
			
			while (Istart <= Iend)
			{
				if (Iguess < Istart)
					return Ilastguess;
				if (Iguess > Iend)
					return Iend;
				
				double Xguess = xvec[Iguess];
				
				if (Xguess == x)
					return Iguess;
	
				if (Xguess > x)
				{
					Iend = Iguess-1;
					if (Iend < Istart)
						return Ilastguess;
					if (Iend == Istart)
						return Iend;
				} 
				else
				{ 
					Istart = Iguess+1; 
					Ilastguess = Iguess; 
				}
				
				if (Istart >= Iend)
					return Ilastguess;
							
				double Xend = xvec[Iend];
				double Xstart = xvec[Istart];
				
				double m = (double)(Iend-Istart+1) / (double)(Xend - Xstart);
				Iguess = Istart + (int)((x - Xstart) * m);
			}
			
			if (Ilastguess != -1)
				return Ilastguess;
			if (xvec[Istart] <= x)
				return Istart;
			else
				return -1;
		}
			
		
		/// <summary>
		/// Adds the amount to the appropriate 1D cell with aliasing
		/// </summary>
		/// <param name='vector'>
		/// Vector
		/// </param>
		/// <param name='domain'>
		/// Index of vector domain
		/// </param>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='amount'>
		/// Amount to be added at vector[X]
		/// </param>
		public static void AddAliased (Vector<double> vector, Vector<double> domain, double x, double amount)
		{
			var i = ArrayUtils.Find (domain, x);
			
			// lower boundary
			if (i <= 0)
				vector[0] += amount;
			
			// upper boundary
			else if (i >= (domain.Count-1))
				vector[domain.Count-1] += amount; 
			
			// in the middle, alias
			else
			{
				var width = domain[i+1] - domain[i];
				var dx = x - domain[i];
				var p = dx / width;
				
				vector[i] += (1-p) * amount;
				vector[i+1] += p * amount;
			}
		}
		

		/// <summary>
		/// Adds the amount to the appropriate 1D cell with aliasing
		/// </summary>
		/// <param name='vector'>
		/// Vector
		/// </param>
		/// <param name='domain'>
		/// Index of vector domain
		/// </param>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='amount'>
		/// Amount to be added at vector[X]
		/// </param>
		public static void AddAliased (
			Vector<double> vector, 
			double Xstart,
			double Xend,
			double x, 
			double amount)
		{
			var cells = vector.Count;
			var dx = (Xend - Xstart) / (double)cells;
			var i = (int)((x - Xstart) / dx);

			// lower boundary
			if (i <= 0)
				vector[0] += amount;

			// upper boundary
			else if (i >= (cells-1))
				vector[cells-1] += amount; 

			// in the middle, alias
			else
			{
				var gap = x - (Xstart + i * dx);
				var p = gap / dx;

				vector[i] += (1-p) * amount;
				vector[i+1] += p * amount;
			}
		}

		
		/// <summary>
		/// Adds the amount to the appropriate 2D cell with aliasing
		/// </summary>
		/// <param name='matrix'>
		/// Matrix to add into
		/// </param>
		/// <param name='rdomain'>
		/// Domain of rows
		/// </param>
		/// <param name='cdomain'>
		/// Domain of cols
		/// </param>
		/// <param name='rx'>
		/// Row ordinate
		/// </param>
		/// <param name='cx'>
		/// Column ordinate
		/// </param>
		/// <param name='amount'>
		/// Amount to be added
		/// </param>
		public static void AddAliased (
			Matrix<double> matrix, 
			Vector<double> rdomain, Vector<double> cdomain, 
			double rx, double cx, 
			double amount)
		{
			var ri = ArrayUtils.Find (rdomain, rx);
			var ci = ArrayUtils.Find (cdomain, cx);
			
			var r0 = 0;
			var r1 = 0;
			var c0 = 0;
			var c1 = 0;
			var rwidth = 0.0;
			var cwidth = 0.0;
			
			if (ri <= 0)
			{ 
				r0 = 0; r1 = 0; 
				rx = rdomain[0];
				rwidth = 1.0;
			}
			else if (ri >= (rdomain.Count-1))
			{ 
				r0 = r1 = rdomain.Count-1; 
				rx = rdomain[rdomain.Count-1];
				rwidth = 1.0;
			} 			
			else
			{ 
				r0 = ri; r1 = ri+1;
				rwidth = rdomain[r1] - rdomain[r0];
			}

			if (ci <= 0)
			{ 
				c0 = 0; c1 = 0;
				cx = cdomain[0];
				cwidth = 1.0;
			}
			else if (ci >= (cdomain.Count-1))
			{ 
				c0 = c1 = cdomain.Count-1; 
				cx = cdomain[cdomain.Count-1];
				cwidth = 1.0;
			} 			
			else
			{ 
				c0 = ci; c1 = ci+1; 
				cwidth = cdomain[c1] - cdomain[c0];				
			}
			
			// perform aliasing
			var rp = (rx - rdomain[r0]) / rwidth;
			var cp = (cx - cdomain[c0]) / cwidth;
			
			matrix[r0,c0] += (1-rp)*(1-cp) * amount;
			matrix[r0,c1] += (1-rp)*(cp) * amount;
			matrix[r1,c0] += (rp)*(1-cp) * amount;
			matrix[r1,c1] += (rp)*(cp) * amount;
		}
		
	}
}


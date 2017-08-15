 
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using bridge.math.matrix;

namespace bridge.common.collections
{
	/// <summary>
	/// Collection utilities
	/// </summary>
	public static class Collections
	{
		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a list
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static List<T> Collect<T,V> (IEnumerable<V> src, Func<V,T> f)
		{
			var list = new List<T>();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}

		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a list
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static HashSet<T> CollectSet<T,V> (IEnumerable<V> src, Func<V,T> f)
		{
			var list = new HashSet<T>();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}

		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a collection
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static C CollectInto<C,V,T> (IEnumerable<V> src, Func<V,T> f) where C : ICollection<T>, new()
		{
			var list = new C();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}
		

		/// <summary>
		/// Maps across enumerable, creating matrix, mapping function provides vector for each row
		/// </summary>
		/// <param name="src">Source.</param>
		/// <param name="mapping">Mapping.</param>
		public static Matrix<double> MapToMatrixByRow<V> (IEnumerable<V> src, Func<V,Vector<double>> mapping)
		{
			var tmpl = mapping (First (src));
			var nrows = Length (src);
			var ncols = tmpl.Count;

			IIndexByName index = null; 
			if (tmpl is IndexedVector)
				index = ((IndexedVector)tmpl).Indices;

			var mat = new IndexedMatrix (nrows, ncols, null, index);
			var ri = 0;
			foreach (var v in src)
			{
				var rvec = mapping (v);
				if (rvec.Count != ncols)
					throw new ArgumentException ("ncols does not match mapping function vector size");

				for (int ci = 0; ci < ncols; ci++)
					mat [ri, ci] = rvec [ci];

				ri++;
			}

			return mat;
		}
		

		/// <summary>
		/// Maps across enumerable, creating matrix, mapping function provides vector for each row
		/// </summary>
		/// <param name="src">Source.</param>
		/// <param name="mapping">Mapping.</param>
		public static Matrix<double> MapToMatrixByCol<V> (IEnumerable<V> src, Func<V,Vector<double>> mapping)
		{
			var tmpl = mapping (First (src));
			var ncols = Length (src);
			var nrows = tmpl.Count;

			IIndexByName index = null; 
			if (tmpl is IndexedVector)
				index = ((IndexedVector)tmpl).Indices;

			var mat = new IndexedMatrix (nrows, ncols, index, null);
			var ci = 0;
			foreach (var v in src)
			{
				var cvec = mapping (v);
				if (cvec.Count != nrows)
					throw new ArgumentException ("nrows does not match mapping function vector size");

				for (int ri = 0; ri < nrows; ri++)
					mat [ri, ci] = cvec [ri];

				ci++;
			}

			return mat;
		}

		
		/// <summary>
		/// Finds the first element of an enumeration matching predicate
		/// </summary>
		/// <returns>
		/// The matching value or null
		/// </returns>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='predicate'>
		/// Predicate.
		/// </param>
		public static V FindOne<V> (IEnumerable<V> src, Predicate<V> predicate)
		{
			foreach (var v in src)
				if (predicate (v)) return v;
			
			return default(V);
		}
		
		/// <summary>
		/// Gets the 1st element
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		public static V First<V> (IEnumerable<V> src)
		{
			foreach (var v in src)
				return v;
			
			return default(V);
		}
		
		/// <summary>
		/// Gets the ith element
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='ith'>
		/// index into pivots
		/// </param>
		public static V Ith<V> (IEnumerable<V> src, int ith)
		{
			foreach (var v in src)
			{
				if (--ith <= 0)
					return v;
			}

			return default(V);
		}


		/// <summary>
		/// Gets the length of the sequence
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		public static int Length<V> (IEnumerable<V> src)
		{
			var count = 0;
			foreach (var v in src)
				count++;

			return count;
		}

		
		/// <summary>
		/// Finds the all elements of an enumeration matching predicate
		/// </summary>
		/// <returns>
		/// The matching values
		/// </returns>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='predicate'>
		/// Predicate.
		/// </param>
		public static IList<V> FindAll<V> (IEnumerable<V> src, Predicate<V> predicate)
		{
			var list = new List<V>();
			foreach (var v in src)
				if (predicate (v)) list.Add(v);
			
			return list;
		}
		
		
		/// <summary>
		/// Maximum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static int Max<V> (IEnumerable<V> src, Func<V,int> f)
		{
			var Vmax = int.MinValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv > Vmax)
					Vmax = iv;
			}
			
			return Vmax;
		}
		
		
		/// <summary>
		/// Maximum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static double Max<V> (IEnumerable<V> src, Func<V,double> f)
		{
			var Vmax = double.MinValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv > Vmax)
					Vmax = iv;
			}
			
			return Vmax;
		}
		
		
		/// <summary>
		/// Minumum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static int Min<V> (IEnumerable<V> src, Func<V,int> f)
		{
			var Vmin = int.MaxValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv < Vmin)
					Vmin = iv;
			}
			
			return Vmin;
		}
		
		
		/// <summary>
		/// Minumum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static double Min<V> (IEnumerable<V> src, Func<V,double> f)
		{
			var Vmin = double.MaxValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv < Vmin)
					Vmin = iv;
			}
			
			return Vmin;
		}

	}
}


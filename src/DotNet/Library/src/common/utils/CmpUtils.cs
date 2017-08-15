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
using bridge.math.matrix;


namespace bridge.common.utils
{	
	/// <summary>
	/// Comparison utilities
	/// </summary>
	public class CmpUtils
	{
		// Min & Max functions
		
		
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static double Min (double a, double b, double c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
		
	
		/// <summary>
		/// Minimum the specified a, b, c and d.
		/// </summary>
		public static double Min (double a, double b, double c, double d)
		{
			if (a < b)
			{
				if (c > b || a < c)
					return (d < a) ? d : a;
				else
					return (d < c) ? d : c;
			}
			else
			{
				if (c > a || c > b)
					return (d < b) ? d : b;
				else
					return (d < c) ? d : c;
			}
		}

		
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (params double[] v)
		{
			double min = double.MaxValue;
			for (int i = 0 ; i < v.Length ; i++)
				min = Math.Min (min, v[i]);
			
			return min;
		}
		
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (Vector<double> v)
		{
			double min = double.MaxValue;
			for (int i = 0 ; i < v.Count ; i++)
				min = Math.Min (min, v[i]);

			return min;
		}

		/// <summary>
		/// Distance the specified a and b.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static Vector<double> Distance(Vector<double> a, Vector<double> b)
		{
			if (a.Count != b.Count)
				throw new Exception ("Can not compute distance with different size of vectos");

			IndexedVector retValue = new IndexedVector (b.Count);
			for (int i = 0; i < a.Count; ++i)
				retValue[i] =  a [i] - b [i];
			return retValue;
		}

	
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (double[] v, int Istart, int Iend)
		{
			double min = double.MaxValue;
			for (int i = Istart ; i <= Iend ; i++)
				min = Math.Min (min, v[i]);
			
			return min;
		}

		
	
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static long Min (long a, long b, long c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
	
	
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static int Min (int a, int b, int c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
		
	
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(params double[] v)
		{
			double max = -double.MaxValue;
			for (int i = 0 ; i < v.Length ; i++)
				max = Math.Max (max, v[i]);
			
			return max;
		}
		
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(Vector<double> v)
		{
			double max = -double.MaxValue;
			for (int i = 0 ; i < v.Count ; i++)
				max = Math.Max (max, v[i]);

			return max;
		}

	
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(double[] v, int Istart, int Iend)
		{
			double max = -double.MaxValue;
			for (int i = Istart ; i <= Iend ; i++)
				max = Math.Max (max, v[i]);
			
			return max;
		}
		
		
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		/// <param name='a'>
		public static double Max (double a, double b, double c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
	
	
		/// <summary>
		/// Max the specified a, b, c and d.
		/// </summary>
		public static double Max (double a, double b, double c, double d)
		{
			if (a > b)
			{
				if (c < b || a > c)
					return (d > a) ? d : a;
				else
					return (d < c) ? d : c;
			}
			else
			{
				if (c < a || c < b)
					return (d > b) ? d : b;
				else
					return (d > c) ? d : c;
			}
		}
	
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		/// <param name='a'>
		public static long Max (long a, long b, long c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
	
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		public static int Max (int a, int b, int c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
		
		
		// Constraints
			
			
		/// <summary>
		/// 	Determine an size lower than amount: min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Lower (double amount, double min, double incr)
		{
			if (Math.Abs(amount) <= min)
				return 0;
		
			double absamount = Math.Abs (amount);
			
			double n = (int)((absamount - min) / incr);
			double namount = min + n * incr;
			
			if (namount < absamount)
				return amount < 0 ?  -namount : namount;
			if (n > 0)
				return amount < 0 ? -(min + (n-1) * incr) : (min + (n-1) * incr);
			else
				return 0;
		}
		
	
		/// <summary>
		/// 	Determine an size higher than amount: min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Higher (double amount, double min, double incr)
		{
			double absamount = Math.Abs (amount);
			
			double n = (int)((absamount - min) / incr);
			double namount = min + (n+1) * incr;
			
			if (namount >= min)
				return namount;
			else
				return 0;
		}
	
		
		/// <summary>
		/// 	Determine nearest amount to min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		/// <param name='threshold'>
		/// 	rounding threshold as percentage of sizeincr before jumping to next incr.
		/// </param>
		public static double Nearest (double amount, double min, double incr, double threshold)
		{
			return CmpUtils.Nearest (amount, min, incr, double.MaxValue, threshold);
		}
		
	
		/// <summary>
		/// 	Determine nearest amount to min + n * incr.
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Nearest (double amount, double min, double incr)
		{
			return CmpUtils.Nearest (amount, min, incr, double.MaxValue, 0.5);
		}
		
		
	
		/// <summary>
		/// 	Determine nearest amount as a magnitude to min + n * incr, signed result
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	minimum value (as a positive #).
		/// </param>
		/// <param name='incr'>
		/// 	increment (as a positive #).
		/// </param>
		/// <param name='max'>
		/// 	maximum value (as a positive #).
		/// </param>
		/// <param name='threshold'>
		/// 	rounding threshold as percentage of sizeincr before jumping to next incr.
		/// </param>
		public static double Nearest (double amount, double min, double incr, double max, double threshold)
		{
			if (incr == 0.0)
				return amount;
				
			if (Math.Abs(amount) <= (min * threshold))
				return 0;
		
			double namount = Math.Abs (amount) - min;
			if (namount <= 0)
				return amount < 0 ? -min : min;
			
			double n = (int)( (namount + incr *(1-threshold)) / incr);
			double abs = Math.Min (min + n * incr, max);
			
			return amount < 0 ? -abs : abs; 
		}
	
	
	
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static double Constrain (double x, double min, double max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}
	
		
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static int Constrain (int x, int min, int max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}

		
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static long Constrain (long x, long min, long max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}
		
		
		// Comparisons
		
	
		/// <summary>
		/// Determine whether values are equivalent (don't differ by more than epsilon)
		/// </summary>
		public static bool EQ (double a, double b, double epsilon = 1e-14)
		{
			return Math.Abs (a - b) <= epsilon;
		}


		/// <summary>
		/// Determines if is zero (abs(a) < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsZero (double a, double epsilon = 1e-14)
		{
			return Math.Abs (a) <= epsilon;
		}
		
		/// <summary>
		/// Determines if is less-than or equal to zero (a < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsLEZero (double a, double epsilon = 1e-14)
		{
			return a < epsilon;
		}
		
		/// <summary>
		/// Determines if is greater-than or equal to zero (a < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsGEZero (double a, double epsilon = 1e-14)
		{
			return a > -epsilon;
		}


		/// <summary>
		/// Not Zero if Abs(a) > specified epsilon
		/// </summary>
		/// <returns><c>true</c>, if zero was noted, <c>false</c> otherwise.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool NotZero (double a, double epsilon = 1e-14)
		{
			return Math.Abs (a) >= epsilon;
		}
	
		/// <summary>
		/// Determine relative value (-1, 0, 1) for LT, EQ, GT
		/// </summary>
		public static int Compare (double a, double b, double epsilon = 1e-14)
		{
			if (Math.Abs (a - b) <= epsilon)
				return 0;
			else
				return (a < b) ? -1 : 1;
		}
		
			
	
		/// <summary>
		/// Determine whether a > b, by more than epsilon
		/// </summary>
		public static bool GT (double a, double b, double epsilon = 1e-14)
		{
			return (a - b) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a < b, by more than epsilon
		/// </summary>
		public static bool LT (double a, double b, double epsilon = 1e-14)
		{
			return (b - a) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a >= b, by more than epsilon
		/// </summary>
		public static bool GE (double a, double b, double epsilon = 1e-14)
		{
			return EQ (a,b, epsilon) ? true : (a - b) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a <= b, by more than epsilon
		/// </summary>
		public static bool LE (double a, double b, double epsilon = 1e-14)
		{
			return EQ (a,b, epsilon) ? true : (b - a) > epsilon;
		}
			
	}
}

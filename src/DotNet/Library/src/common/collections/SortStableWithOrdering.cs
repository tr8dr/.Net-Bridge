//  
//  	Authors
//  		Jonathan Shore
//  
//  	Copyright:
//  		2012 Systematic Trading LLC 
//  		2002 Systematic Trading LLC
//  
//  		This software is only to be used for the purpose for which
//  		it has been provided.  No part of it is to be reproduced,
//  		disassembled, transmitted, stored in a retrieval system nor
//  		translated in any human or computer language in any way or
//  		for any other purposes whatsoever without the prior written
//  		consent of Systematic Trading LLC
// 
//  
// 
using System;
using System.Diagnostics;

namespace com.stg.common.collections
{
	/// <summary>
	/// This is a stable sort (using merge sort) that also tracks the sort order index
	/// of the resulting sorted values.
	/// <p/>
	/// This is not an in-place sort and requires auxilliary storage for both the values and indices.   Hence
 	/// is presented as a class rather than a function.
 	/// <p/>
 	/// Also note, that because of retained and working state, this class is not thread-safe, if shared by multiple
 	/// threads.  Should be instantiated as thread-local if it is intended to be shared across threads
	/// </summary>
	public class SortStableWithOrdering<V>
	{
		public SortStableWithOrdering (int maxsize, Comparison<V> cmp)
		{
			_tmp_data = new V[maxsize];
			_tmp_indices = new int[maxsize];
			_cmp = cmp;
		}
		
		public SortStableWithOrdering (Comparison<V> cmp)
			: this (256, cmp)
		{
		}
		
		// Functions
		
		
		/// <summary>
		/// Sort the specified data, with data in original form and ordering presenting the sort order
		/// </summary>
		/// <param name='data'>
		/// Data to be sorted
		/// </param>
		/// <param name='ordering'>
		/// Ordering is an array of int with the same dimension as the data to be sorted
		/// </param>
		public void SortOrder (
			V[] data, 
			int[] ordering, 
			int Istart = 0, 
			int Iend = -1)
		{
			if (Iend < 0)
				Iend = data.Length - 1;

			var len = (Iend - Istart + 1);
			Debug.Assert (len == ordering.Length, "ordering must be of the same length as the data to be sorted");
			
			// adjust for size if necessary
			if (data.Length > _tmp_data.Length)
			{
				_tmp_data = new V[len];
				_tmp_indices = new int[len];
			}

			// initial ordering
			for (int i = 0 ; i < len ; i++)
				ordering[i] = Istart + i;

			MergeSortWithOrder (data, ordering, 0, len-1);
		}


		/// <summary>
		/// Sort the specified data
		/// </summary>
		/// <param name='data'>
		/// Data to be sorted
		/// </param>
		public void Sort (
			V[] data, 
			int Istart = 0, 
			int Iend = -1)
		{
			if (Iend < 0)
				Iend = data.Length - 1;

			var len = (Iend - Istart + 1);

			// adjust for size if necessary
			if (len > _tmp_data.Length)
				_tmp_data = new V[len];

			MergeSort (data, Istart, Iend);
		}

		
		#region MergeSort without order-index tracking
		
		
		private void MergeSort (V[] data, int left, int right)
		{
			// if single element (or none), nothing to do
			if (right <= left)
				return;
			
			// create 2 sorted streams: [left, mid] and [mid+1, right]
			var mid = (left + right) / 2;
			MergeSort (data, left, mid);
			MergeSort (data, mid+1, right);
			
			// now merge the 2 sorted streams of [left, mid] and [mid+1, right]
			Merge (data, left, mid+1, right);
		}
		
		
		private void Merge (V[] data, int left, int division, int right)
		{
			var Lptr = left;
			var Rptr = division;
			var Tptr = left;
			
			// select from each sorted sequence
			while (Lptr < division && Rptr <= right)
			{
				var cmp = _cmp (data[Lptr], data[Rptr]);
				if (cmp <= 0)
				{
					_tmp_data[Tptr++] = data[Lptr++];
				}
				else
				{
					_tmp_data[Tptr++] = data[Rptr++];
				}
			}
			
			// cleanup on residual left over in left sequence (if any)
			while (Lptr < division)
			{
				_tmp_data[Tptr++] = data[Lptr++];
			}
			
			// cleanup on residual left over in right sequence (if any)
			while (Rptr <= right)
			{
				_tmp_data[Tptr++] = data[Rptr++];
			}
			
			// now copy back in from tmp arrays into the destination
			Array.Copy (_tmp_data, left, data, left, (right - left + 1));
		}


		#endregion

		#region MergeSort with order-index tracking


		private void MergeSortWithOrder (V[] data, int[] ordering, int left, int right)
		{
			// if single element (or none), nothing to do
			if (right <= left)
				return;

			// create 2 sorted streams: [left, mid] and [mid+1, right]
			var mid = (left + right) / 2;
			MergeSortWithOrder (data, ordering, left, mid);
			MergeSortWithOrder (data, ordering, mid+1, right);

			// now merge the 2 sorted streams of [left, mid] and [mid+1, right]
			MergeWithOrder (data, ordering, left, mid+1, right);
		}


		private void MergeWithOrder (V[] data, int[] ordering, int left, int division, int right)
		{
			var Lptr = left;
			var Rptr = division;
			var Tptr = left;

			// select from each sorted sequence
			while (Lptr < division && Rptr <= right)
			{
				var I_Lptr = ordering [Lptr];
				var I_Rptr = ordering [Rptr];

				var cmp = _cmp (data[I_Lptr], data[I_Rptr]);
				if (cmp <= 0)
				{
					_tmp_indices[Tptr++] = ordering[Lptr++];
				}
				else
				{
					_tmp_indices[Tptr++] = ordering[Rptr++];
				}
			}

			// cleanup on residual left over in left sequence (if any)
			while (Lptr < division)
			{
				_tmp_indices[Tptr++] = ordering[Lptr++];
			}

			// cleanup on residual left over in right sequence (if any)
			while (Rptr <= right)
			{
				_tmp_indices[Tptr++] = ordering[Rptr++];
			}

			// now copy back in from tmp arrays into the destination
			Array.Copy (_tmp_indices, left, ordering, left, (right - left + 1));
		}


		#endregion

		
		// Variables
		
		private V[]				_tmp_data;
		private int[]			_tmp_indices;
		private Comparison<V>	_cmp;
	}
}


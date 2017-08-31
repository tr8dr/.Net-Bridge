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
using System.Threading;


namespace bridge.common.utils
{
	public class AtomicUtils
	{
		/// <summary>
		/// Determines if the ith bit is set atomically
		/// </summary>
		/// <returns><c>true</c> if is bit set; otherwise, <c>false</c>.</returns>
		/// <param name="bits">Bits.</param>
		/// <param name="ith">Ith.</param>
		public static bool GetBit (ref int bits, int ith)
		{
			var mask = 1 << ith;
			if ((Interlocked.Add (ref bits, 0) & mask) == mask)
				return true;
			else
				return false;
		}


		/// <summary>
		/// Sets the ith bit atomically
		/// </summary>
		/// <param name="bits">Bits.</param>
		/// <param name="ith">Ith.</param>
		/// <param name="val">If set to <c>true</c> value.</param>
		public static void SetBit (ref int bits, int ith, bool val = true)
		{
			var mask = 1 << ith;
			if (val)
			{
				while (true)
				{
					var prior = bits;
					var next = prior | mask;
					if (Interlocked.CompareExchange (ref bits, next, prior) == prior)
						return;
				}
			}
			else
			{
				while (true)
				{
					var prior = bits;
					var next = prior & ~mask;
					if (Interlocked.CompareExchange (ref bits, next, prior) == prior)
						return;
				}
			}
		}



		/// <summary>
		/// Sets a value atomically
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="value">Value to set to</param>
		public static void Set (ref int target, int value)
		{
			while (true)
			{
				var prior = target;
				if (Interlocked.CompareExchange (ref target, value, prior) == prior)
					return;
			}
		}

	}
}


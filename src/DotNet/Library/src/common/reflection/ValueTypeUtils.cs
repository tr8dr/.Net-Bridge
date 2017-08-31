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
using System.Collections.Generic;
using bridge.math.matrix;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;

namespace bridge.common.reflection
{
	public enum VType
		{ Short, Int, Long, Float, Double, Byte, Bool, Char, String, Enum, Vector, Matrix, List, Period, Null, Other }
		

	/// <summary>
	/// Fast mapping of value type class IDs to enum
	/// </summary>
	public class ValueTypeUtils
	{		
		/// <summary>
		/// Determine the value type of an object (if a value)
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public static VType TypeOf (object obj)
		{
			VType type = VType.Other;
			if (obj == null)
				return VType.Null;

			Type otype = obj.GetType();
			if (_map.TryGetValue (otype, out type))
				return type;
			if (otype.IsEnum)
				return VType.Enum;

			// test to see if list type or not
			Type list = typeof(List<int>).GetGenericTypeDefinition ();

			Type gtype1 = otype.IsGenericType ? otype.GetGenericTypeDefinition () : otype;
			if (gtype1 == list)
				return VType.List;

			Type btype = BaseTypeOf (otype);
			Type gtype2 = btype.IsGenericType ? btype.GetGenericTypeDefinition () : btype;
			if (gtype2 == list)
				return VType.List;
			else
				return VType.Other;
		}

		
		/// <summary>
		/// Determine the value type of an object (if a value)
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public static VType TypeOf (Type type)
		{
			VType vtype = VType.Other;
			if (type == null)
				return VType.Null;

			if (_map.TryGetValue (type, out vtype))
				return vtype;
			if (type.IsEnum)
				return VType.Enum;

			// test to see if list type or not
			Type list = typeof(List<int>).GetGenericTypeDefinition ();

			Type gtype1 = type.IsGenericType ? type.GetGenericTypeDefinition () : type;
			if (gtype1 == list)
				return VType.List;

			Type btype = BaseTypeOf (type);
			Type gtype2 = btype.IsGenericType ? btype.GetGenericTypeDefinition () : btype;
			if (gtype2 == list)
				return VType.List;
			else
				return VType.Other;
		}
		
		
		/// <summary>
		/// Convert the specified arg to given type (if possible)
		/// </summary>
		/// <param name='arg'>
		/// Argument.
		/// </param>
		/// <param name='totype'>
		/// Type to convert to (if possible)
		/// </param>
		public static object Convert (object arg, VType totype, Type actual)
		{
			VType fromtype = TypeOf (arg);		
			switch (totype)
			{
				case VType.Short:
					return ConformShort (arg, fromtype);
				case VType.Int:
					return ConformInt (arg, fromtype);
				case VType.Long:
					return ConformLong (arg, fromtype);
				case VType.Float:
					return ConformFloat (arg, fromtype);
				case VType.Double:
					return ConformDouble (arg, fromtype);
				case VType.Bool:
					return ConformBool (arg, fromtype);
				case VType.Byte:
					return ConformByte (arg, fromtype);
				case VType.Char:
					return ConformChar (arg, fromtype);
				case VType.Enum:
					return ConformEnum (arg, fromtype, actual);
				case VType.String:
					return arg.ToString();
				default:
					throw new ArgumentException ("cannot convert to type: " + totype);
			}			
		}
			
		
		
		// Implementation
		
		
		/// <summary>
		/// Transform arg to short if possible
		/// </summary>
		private static object ConformShort (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return arg;
				case VType.Int:
					return (short)((int)arg);
				case VType.Long:
					return (short)((long)arg);
				case VType.Float:
					return (short)((float)arg);
				case VType.Double:
					return (short)((double)arg);
				case VType.String:
					return short.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to short");					
			}
		}

		
		/// <summary>
		/// Transform arg to int if possible
		/// </summary>
		private static object ConformInt (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (int)((short)arg);
				case VType.Int:
					return ((int)arg);
				case VType.Long:
					return (int)((long)arg);
				case VType.Float:
					return (int)((float)arg);
				case VType.Double:
					return (int)((double)arg);
				case VType.String:
					return int.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to int");					
			}
		}

		
		/// <summary>
		/// Transform arg to enum if possible
		/// </summary>VType
		private static object ConformEnum (object arg, VType atype, Type actual)
		{
			switch (atype)
			{
				case VType.Short:
					return Enum.GetValues(actual).GetValue ((int)((short)arg));
				case VType.Int:
					return Enum.GetValues(actual).GetValue (((int)arg));
				case VType.Long:
					return Enum.GetValues(actual).GetValue ((int)((long)arg));
				case VType.String:
					return Enum.Parse (actual, (string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to enum");					
			}
		}

		
		/// <summary>
		/// Transform arg to long if possible
		/// </summary>
		private static object ConformLong (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (long)((short)arg);
				case VType.Int:
					return (long)((int)arg);
				case VType.Long:
					return (long)((long)arg);
				case VType.Float:
					return (long)((float)arg);
				case VType.Double:
					return (long)((double)arg);
				case VType.String:
					return long.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to long");					
			}
		}

		
		/// <summary>
		/// Transform arg to float if possible
		/// </summary>
		private static object ConformFloat (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (float)((short)arg);
				case VType.Int:
					return (float)((int)arg);
				case VType.Long:
					return (float)((long)arg);
				case VType.Float:
					return (float)((float)arg);
				case VType.Double:
					return (float)((double)arg);
				case VType.String:
					return float.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to float");					
			}
		}

		
		/// <summary>
		/// Transform arg to double if possible
		/// </summary>
		private static object ConformDouble (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (double)((short)arg);
				case VType.Int:
					return (double)((int)arg);
				case VType.Long:
					return (double)((long)arg);
				case VType.Float:
					return (double)((float)arg);
				case VType.Double:
					return (double)((double)arg);
				case VType.String:
					return double.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to double");					
			}
		}

		
		/// <summary>
		/// Transform arg to bool if possible
		/// </summary>
		private static object ConformBool (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return ((short)arg) != 0;
				case VType.Int:
					return ((int)arg) != 0;
				case VType.Long:
					return ((long)arg) != 0;
				case VType.Float:
					return ((float)arg) != 0.0;
				case VType.Double:
					return ((double)arg) != 0.0;
				case VType.String:
					return bool.Parse ((string)arg);
				case VType.Byte:
					return ((byte)arg) != 0;
				case VType.Char:
					return ((char)arg) != 0;
				case VType.Bool:
					return ((bool)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}

		
		/// <summary>
		/// Transform arg to byte if possible
		/// </summary>
		private static object ConformByte (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (byte)((short)arg);
				case VType.Int:
					return (byte)((int)arg);
				case VType.Long:
					return (byte)((long)arg);
				case VType.Float:
					return (byte)((float)arg);
				case VType.Double:
					return (byte)((double)arg);
				case VType.String:
					return (byte)int.Parse ((string)arg);
				case VType.Byte:
					return ((byte)arg);
				case VType.Char:
					return (byte)((char)arg);
				case VType.Bool:
					return (byte)(((bool)arg) ? 1 : 0);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}

		
		/// <summary>
		/// Transform arg to byte if possible
		/// </summary>
		private static object ConformChar (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (char)((short)arg);
				case VType.Int:
					return (char)((int)arg);
				case VType.Long:
					return (char)((long)arg);
				case VType.Float:
					return (char)((float)arg);
				case VType.Double:
					return (char)((double)arg);
				case VType.String:
					return (char)int.Parse ((string)arg);
				case VType.Byte:
					return (char)((byte)arg);
				case VType.Char:
					return (char)((char)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}


		private static Type BaseTypeOf (Type type)
		{
			if (type.BaseType != null)
				return type.BaseType;
			else
				return type;
		}

		
		static ValueTypeUtils ()
		{
			_map[typeof(short)] = VType.Short;
			_map[typeof(Int16)] = VType.Short;

			_map[typeof(int)] = VType.Int;
			_map[typeof(Int32)] = VType.Int;

			_map[typeof(long)] = VType.Long;
			_map[typeof(Int64)] = VType.Long;

			_map[typeof(float)] = VType.Float;
			_map[typeof(Single)] = VType.Float;

			_map[typeof(double)] = VType.Double;
			_map[typeof(Double)] = VType.Double;

			_map[typeof(byte)] = VType.Byte;
			_map[typeof(Byte)] = VType.Byte;

			_map[typeof(bool)] = VType.Bool;
			_map[typeof(Boolean)] = VType.Bool;

			_map[typeof(char)] = VType.Char;
			_map[typeof(Char)] = VType.Char;

			_map[typeof(IndexedVector)] = VType.Vector;
			_map[typeof(DenseVector)] = VType.Vector;
			_map[typeof(Vector<double>)] = VType.Vector;
			_map[typeof(SubviewVector)] = VType.Vector;

			_map[typeof(IndexedMatrix)] = VType.Matrix;
			_map[typeof(DenseMatrix)] = VType.Matrix;
			_map[typeof(Matrix<double>)] = VType.Matrix;

			_map[typeof(string)] = VType.String;

			_map[typeof(List<int>)] = VType.List;
			_map[typeof(List<double>)] = VType.List;
			_map[typeof(List<string>)] = VType.List;
		}
		
		// Variables
		
		static Dictionary<Type,VType>	_map = new Dictionary<Type, VType>();
	}
}


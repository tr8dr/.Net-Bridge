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
using bridge.common.io;
using System.Collections.Generic;
using bridge.math.matrix;
using bridge.server.data;
using bridge.server.ctrl;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace bridge.server
{
	/// <summary>
	/// CLR message base class
	/// </summary>
	public class CLRMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.core.CLRMessage"/> class.
		/// </summary>
		/// <param name="type">Message type.</param>
		public CLRMessage (byte type)
		{
			_type = type;
		}


		// Properties

		/// <summary>
		/// Message type
		/// </summary>
		public byte MessageType
			{ get { return _type; } }


		// Serialization

		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public virtual void Serialize (IBinaryWriter cout)
		{
			cout.WriteUInt16 (Magic);
			cout.WriteByte (_type);
		}

		/// <summary>
		/// Deserialize the message (assumes type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public virtual void Deserialize (IBinaryReader cin)
		{
		}


		// IO


		/// <summary>
		/// Read the next message from the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static CLRMessage Read (IBinaryReader stream)
		{
			var magic = stream.ReadUInt16();
			if (magic != Magic)
				throw new ArgumentException ("encountered message with wrong magic #, protocol wrong");

			var type = (byte)stream.ReadByte();
			var msg = Create (type);

			msg.Deserialize (stream);
			return msg;
		}


		/// <summary>
		/// Read a value off of the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static object ReadValue (IBinaryReader stream)
		{
			var obj = DeserializeValue (stream);
			if (obj is Exception)
				throw (Exception)obj;
			else
				return obj;
		}

		
		/// <summary>
		/// Write a message to the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static void Write (IBinaryWriter stream, CLRMessage msg)
		{
			msg.Serialize (stream);
			stream.Flush();
		}


		/// <summary>
		/// Write a value to the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static void WriteValue (IBinaryWriter stream, object value)
		{
			SerializeValue (stream, value);
			stream.Flush();
		}


		#region Message Creation


		/// <summary>
		/// Create the specified type.
		/// </summary>
		/// <param name="type">Type.</param>
		internal static CLRMessage Create (byte type)
		{
			switch (type)
			{
				case TypeNull:
					return new CLRNullMessage ();
				case TypeBool:
					return new CLRBoolMessage ();
				case TypeByte:
					return new CLRByteMessage ();
				case TypeInt32:
					return new CLRInt32Message ();
				case TypeInt64:
					return new CLRInt64Message ();
				case TypeReal64:
					return new CLRReal64Message ();
				case TypeString:
					return new CLRStringMessage ();
				case TypeObject:
					return new CLRObjectMessage ();

				case TypeBoolArray:
					return new CLRBoolArrayMessage ();
				case TypeByteArray:
					return new CLRByteArrayMessage ();
				case TypeInt32Array:
					return new CLRInt32ArrayMessage ();
				case TypeInt64Array:
					return new CLRInt64ArrayMessage ();
				case TypeReal64Array:
					return new CLRReal64ArrayMessage ();
				case TypeStringArray:
					return new CLRStringArrayMessage ();
				case TypeObjectArray:
					return new CLRObjectArrayMessage ();
				
				case TypeVector:
					return new CLRVectorMessage ();
				case TypeMatrix:
					return new CLRMatrixMessage ();
				case TypeException:
					return new CLRExceptionMessage ();

				case TypeCreate:
					return new CLRCreateMessage ();
				case TypeCallStaticMethod:
					return new CLRCallStaticMethodMessage ();
				case TypeCallMethod:
					return new CLRCallMethodMessage ();
				case TypeGetProperty:
					return new CLRGetPropertyMessage ();
				case TypeGetIndexedProperty:
					return new CLRGetIndexedPropertyMessage ();
				case TypeGetIndexed:
					return new CLRGetIndexedMessage ();
				case TypeSetProperty:
					return new CLRSetPropertyMessage ();
				case TypeGetStaticProperty:
					return new CLRGetStaticPropertyMessage ();
				case TypeSetStaticProperty:
					return new CLRSetStaticPropertyMessage ();
				case TypeProtect:
					return new CLRProtectMessage ();
				case TypeRelease:
					return new CLRReleaseMessage ();

				case TypeTemplateReq:
					return new CLRTemplateReqMessage ();
				case TypeTemplateReply:
					return new CLRTemplateReplyMessage ();

				default:
					throw new ArgumentException ("encountered unknow CLR message type, bad protocol: " + (int)type);
			}
		}


		/// <summary>
		/// Get the type associated with a value
		/// </summary>
		/// <param name="value">Value.</param>
		internal static byte TypeOf (object value)
		{
			if (value == null)
				return TypeNull;

			if (value is Exception)
				return TypeException;

			byte ctype = 0;
			var klass = value.GetType();
			if (_typemap.TryGetValue (klass, out ctype))
				return ctype;
			if (klass.IsArray)
				return TypeObjectArray;
			else
				return TypeObject;
		}


		/// <summary>
		/// Serlializes a value.
		/// </summary>
		/// <param name="cout">Cout.</param>
		/// <param name="val">Value.</param>
		internal static void SerializeValue (IBinaryWriter cout, object val)
		{
			CLRMessage msg = null;
			switch (TypeOf (val))
			{
				case TypeNull:
					msg = new CLRNullMessage ();
					break;

				case TypeBool:
					msg = new CLRBoolMessage (Convert.ToBoolean (val));
					break;

				case TypeByte:
					msg = new CLRByteMessage (Convert.ToByte (val));
					break;

				case TypeInt32:
					msg = new CLRInt32Message (Convert.ToInt32 (val));
					break;

				case TypeInt64:
					msg = new CLRInt64Message (Convert.ToInt64 (val));
					break;

				case TypeReal64:
					msg = new CLRReal64Message (Convert.ToDouble (val));
					break;

				case TypeString:
					msg = new CLRStringMessage ((string)val);
					break;

				case TypeObject:
					msg = new CLRObjectMessage (val);
					break;

				case TypeBoolArray:
					msg = new CLRBoolArrayMessage ((bool[])val);
					break;

				case TypeByteArray:
					msg = new CLRByteArrayMessage ((byte[])val);
					break;

				case TypeInt32Array:
					msg = new CLRInt32ArrayMessage ((int[])val);
					break;

				case TypeInt64Array:
					msg = new CLRInt64ArrayMessage ((long[])val);
					break;

				case TypeReal64Array:
					msg = new CLRReal64ArrayMessage ((double[])val);
					break;

				case TypeStringArray:
					msg = new CLRStringArrayMessage ((string[])val);
					break;

				case TypeObjectArray:
					msg = new CLRObjectArrayMessage ((object[])val);
					break;

				case TypeVector:
					msg = new CLRVectorMessage ((Vector<double>)val);
					break;

				case TypeMatrix:
					msg = new CLRMatrixMessage ((Matrix<double>)val);
					break;

				case TypeException:
					msg = new CLRExceptionMessage (val);
					break;
					
				default:
					throw new ArgumentException ("do not know how to serialize: " + val.GetType());
			}

			msg.Serialize (cout);
		}


		/// <summary>
		/// Serlializes a value.
		/// </summary>
		/// <param name="cout">Cout.</param>
		/// <param name="val">Value.</param>
		internal static object DeserializeValue (IBinaryReader cin)
		{
			var msg = Read (cin);
			switch (msg.MessageType)
			{
				case TypeNull:
					return null;

				case TypeBool:
					return ((CLRBoolMessage)msg).Value;

				case TypeByte:
					return ((CLRByteMessage)msg).Value;

				case TypeInt32:
					return ((CLRInt32Message)msg).Value;

				case TypeInt64:
					return ((CLRInt64Message)msg).Value;

				case TypeReal64:
					return ((CLRReal64Message)msg).Value;

				case TypeString:
					return ((CLRStringMessage)msg).Value;

				case TypeObject:
					return ((CLRObjectMessage)msg).ToObject();

				case TypeBoolArray:
					return ((CLRBoolArrayMessage)msg).Value;

				case TypeByteArray:
					return ((CLRByteArrayMessage)msg).Value;

				case TypeInt32Array:
					return ((CLRInt32ArrayMessage)msg).Value;

				case TypeInt64Array:
					return ((CLRInt64ArrayMessage)msg).Value;

				case TypeReal64Array:
					return ((CLRReal64ArrayMessage)msg).Value;

				case TypeStringArray:
					return ((CLRStringArrayMessage)msg).Value;

				case TypeObjectArray:
					return ((CLRObjectArrayMessage)msg).Value;

				case TypeVector:
					return ((CLRVectorMessage)msg).Value;

				case TypeMatrix:
					return ((CLRMatrixMessage)msg).Value;

				case TypeException:
					return ((CLRExceptionMessage)msg).ToException();

				default:
					throw new ArgumentException ("do not know how to deserialize: " + msg.GetType());
			}
		}


		#endregion 

		#region Message Types

		public const ushort			Magic						= 0xd00d;

		public const byte			TypeNull					= 0;
		public const byte			TypeBool					= 1;
		public const byte			TypeByte					= 2;
		public const byte			TypeInt32					= 5;
		public const byte			TypeInt64					= 6;
		public const byte			TypeReal64					= 7;
		public const byte			TypeString					= 8;
		public const byte			TypeObject					= 9;

		public const byte			TypeVector					= 21;
		public const byte			TypeMatrix					= 22;
		public const byte			TypeException				= 23;

		public const byte			TypeBoolArray				= 101;
		public const byte			TypeByteArray				= 102;
		public const byte			TypeInt32Array				= 105;
		public const byte			TypeInt64Array				= 106;
		public const byte			TypeReal64Array				= 107;
		public const byte			TypeStringArray				= 108;
		public const byte			TypeObjectArray				= 109;

		public const byte			TypeCreate					= 201;
		public const byte			TypeCallStaticMethod		= 202;
		public const byte			TypeCallMethod				= 203;
		public const byte			TypeGetProperty				= 204;
		public const byte			TypeGetIndexedProperty		= 205;
		public const byte			TypeGetIndexed				= 206;
		public const byte			TypeSetProperty				= 207;
		public const byte			TypeGetStaticProperty		= 208;
		public const byte			TypeSetStaticProperty		= 209;
		public const byte			TypeProtect					= 210;
		public const byte			TypeRelease					= 211;

		public const byte			TypeTemplateReq				= 212;
		public const byte			TypeTemplateReply			= 213;

		#endregion

		#region Static Initializer


		static CLRMessage ()
		{
			_typemap[typeof(bool)] = TypeBool;
			_typemap[typeof(byte)] = TypeByte;
			_typemap[typeof(short)] = TypeInt32;
			_typemap[typeof(int)] = TypeInt32;
			_typemap[typeof(long)] = TypeInt64;
			_typemap[typeof(float)] = TypeReal64;
			_typemap[typeof(double)] = TypeReal64;
			_typemap[typeof(string)] = TypeString;
			_typemap[typeof(object)] = TypeObject;

			_typemap[typeof(bool[])] = TypeBoolArray;
			_typemap[typeof(byte[])] = TypeByteArray;
			_typemap[typeof(int[])] = TypeInt32Array;
			_typemap[typeof(long[])] = TypeInt64Array;
			_typemap[typeof(float[])] = TypeReal64Array;
			_typemap[typeof(double[])] = TypeReal64Array;
			_typemap[typeof(string[])] = TypeStringArray;
			_typemap[typeof(object[])] = TypeObjectArray;

			_typemap[typeof(IndexedVector)] = TypeVector;
			_typemap[typeof(SubviewVector)] = TypeVector;
			_typemap[typeof(DenseVector)] = TypeVector;
			
			_typemap[typeof(IndexedMatrix)] = TypeMatrix;
			_typemap[typeof(DenseMatrix)] = TypeMatrix;
		}

		#endregion

		// Variables

		protected byte					_type;
		static Dictionary<Type,byte>	_typemap = new Dictionary<Type,byte>();
	}
}


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
using System.IO;
using System.Text;


namespace bridge.common.io
{
	/// <summary>
	/// Reads binary data swapping endianness
	/// </summary>
	public class SwapEndianConverter : IBinaryConversions
	{
		// Operations
		

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16(byte[] buffer, int offset)
		{
			_int16.b2 = buffer[offset+0];
			_int16.b1 = buffer[offset+1];
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32(byte[] buffer, int offset)
		{
			_int32.b4 = buffer[offset+0];
			_int32.b3 = buffer[offset+1];
			_int32.b2 = buffer[offset+2];
			_int32.b1 = buffer[offset+3];
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64(byte[] buffer, int offset)
		{
			_int64.b8 = buffer[offset+0];
			_int64.b7 = buffer[offset+1];
			_int64.b6 = buffer[offset+2];
			_int64.b5 = buffer[offset+3];
			_int64.b4 = buffer[offset+4];
			_int64.b3 = buffer[offset+5];
			_int64.b2 = buffer[offset+6];
			_int64.b1 = buffer[offset+7];
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16(byte[] buffer, int offset)
		{
			_int16.b2 = buffer[offset+0];
			_int16.b1 = buffer[offset+1];
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32(byte[] buffer, int offset)
		{
			_int32.b4 = buffer[offset+0];
			_int32.b3 = buffer[offset+1];
			_int32.b2 = buffer[offset+2];
			_int32.b1 = buffer[offset+3];
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64(byte[] buffer, int offset)
		{
			_int64.b8 = buffer[offset+0];
			_int64.b7 = buffer[offset+1];
			_int64.b6 = buffer[offset+2];
			_int64.b5 = buffer[offset+3];
			_int64.b4 = buffer[offset+4];
			_int64.b3 = buffer[offset+5];
			_int64.b2 = buffer[offset+6];
			_int64.b1 = buffer[offset+7];
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble(byte[] buffer, int offset)
		{
			_float64.b8 = buffer[offset+0];
			_float64.b7 = buffer[offset+1];
			_float64.b6 = buffer[offset+2];
			_float64.b5 = buffer[offset+3];
			_float64.b4 = buffer[offset+4];
			_float64.b3 = buffer[offset+5];
			_float64.b2 = buffer[offset+6];
			_float64.b1 = buffer[offset+7];
			
			return _float64.ToDouble();
		}


		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteInt16(byte[] buffer, int offset, short v)
		{
			_int16.i = (ushort)v;
			buffer[offset+1] = _int16.b1;
			buffer[offset+0] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteInt32(byte[] buffer, int offset, int v)
		{
			_int32.i = (uint)v;
			buffer[offset+3] = _int32.b1;
			buffer[offset+2] = _int32.b2;
			buffer[offset+1] = _int32.b3;
			buffer[offset+0] = _int32.b4;
		}

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteInt64(byte[] buffer, int offset, long v)
		{
			_int64.l = (ulong)v;
			buffer[offset+7] = _int64.b1;
			buffer[offset+6] = _int64.b2;
			buffer[offset+5] = _int64.b3;
			buffer[offset+4] = _int64.b4;
			buffer[offset+3] = _int64.b5;
			buffer[offset+2] = _int64.b6;
			buffer[offset+1] = _int64.b7;
			buffer[offset+0] = _int64.b8;
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteUInt16(byte[] buffer, int offset, ushort v)
		{
			_int16.i = v;
			buffer[offset+1] = _int16.b1;
			buffer[offset+0] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteUInt32(byte[] buffer, int offset, uint v)
		{
			_int32.i = v;
			buffer[offset+3] = _int32.b1;
			buffer[offset+2] = _int32.b2;
			buffer[offset+1] = _int32.b3;
			buffer[offset+0] = _int32.b4;
		}
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public void WriteUInt64(byte[] buffer, int offset, ulong v)
		{
			_int64.l = v;
			buffer[offset+7] = _int64.b1;
			buffer[offset+6] = _int64.b2;
			buffer[offset+5] = _int64.b3;
			buffer[offset+4] = _int64.b4;
			buffer[offset+3] = _int64.b5;
			buffer[offset+2] = _int64.b6;
			buffer[offset+1] = _int64.b7;
			buffer[offset+0] = _int64.b8;			
		}
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteDouble(byte[] buffer, int offset, double v)
		{
			_float64.d = v;
			buffer[offset+7] = _float64.b1;
			buffer[offset+6] = _float64.b2;
			buffer[offset+5] = _float64.b3;
			buffer[offset+4] = _float64.b4;
			buffer[offset+3] = _float64.b5;
			buffer[offset+2] = _float64.b6;
			buffer[offset+1] = _float64.b7;
			buffer[offset+0] = _float64.b8;		
		}
		
		
		// Variables
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}
	
	
	/// <summary>
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianConverter : IBinaryConversions
	{
		// Operations
		

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16(byte[] buffer, int offset)
		{
			_int16.b1 = buffer[offset+0];
			_int16.b2 = buffer[offset+1];
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32(byte[] buffer, int offset)
		{
			_int32.b1 = buffer[offset+0];
			_int32.b2 = buffer[offset+1];
			_int32.b3 = buffer[offset+2];
			_int32.b4 = buffer[offset+3];
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64(byte[] buffer, int offset)
		{
			_int64.b1 = buffer[offset+0];
			_int64.b2 = buffer[offset+1];
			_int64.b3 = buffer[offset+2];
			_int64.b4 = buffer[offset+3];
			_int64.b5 = buffer[offset+4];
			_int64.b6 = buffer[offset+5];
			_int64.b7 = buffer[offset+6];
			_int64.b8 = buffer[offset+7];
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16(byte[] buffer, int offset)
		{
			_int16.b1 = buffer[offset+0];
			_int16.b2 = buffer[offset+1];
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32(byte[] buffer, int offset)
		{
			_int32.b1 = buffer[offset+0];
			_int32.b2 = buffer[offset+1];
			_int32.b3 = buffer[offset+2];
			_int32.b4 = buffer[offset+3];
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64(byte[] buffer, int offset)
		{
			_int64.b1 = buffer[offset+0];
			_int64.b2 = buffer[offset+1];
			_int64.b3 = buffer[offset+2];
			_int64.b4 = buffer[offset+3];
			_int64.b5 = buffer[offset+4];
			_int64.b6 = buffer[offset+5];
			_int64.b7 = buffer[offset+6];
			_int64.b8 = buffer[offset+7];
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble(byte[] buffer, int offset)
		{
			_float64.b1 = buffer[offset+0];
			_float64.b2 = buffer[offset+1];
			_float64.b3 = buffer[offset+2];
			_float64.b4 = buffer[offset+3];
			_float64.b5 = buffer[offset+4];
			_float64.b6 = buffer[offset+5];
			_float64.b7 = buffer[offset+6];
			_float64.b8 = buffer[offset+7];
			
			return _float64.ToDouble();
		}


		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteInt16(byte[] buffer, int offset, short v)
		{
			_int16.i = (ushort)v;
			buffer[offset+0] = _int16.b1;
			buffer[offset+1] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteInt32(byte[] buffer, int offset, int v)
		{
			_int32.i = (uint)v;
			buffer[offset+0] = _int32.b1;
			buffer[offset+1] = _int32.b2;
			buffer[offset+2] = _int32.b3;
			buffer[offset+3] = _int32.b4;
		}

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteInt64(byte[] buffer, int offset, long v)
		{
			_int64.l = (ulong)v;
			buffer[offset+0] = _int64.b1;
			buffer[offset+1] = _int64.b2;
			buffer[offset+2] = _int64.b3;
			buffer[offset+3] = _int64.b4;
			buffer[offset+4] = _int64.b5;
			buffer[offset+5] = _int64.b6;
			buffer[offset+6] = _int64.b7;
			buffer[offset+7] = _int64.b8;
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteUInt16(byte[] buffer, int offset, ushort v)
		{
			_int16.i = v;
			buffer[offset+0] = _int16.b1;
			buffer[offset+1] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteUInt32(byte[] buffer, int offset, uint v)
		{
			_int32.i = v;
			buffer[offset+0] = _int32.b1;
			buffer[offset+1] = _int32.b2;
			buffer[offset+2] = _int32.b3;
			buffer[offset+3] = _int32.b4;
		}
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public void WriteUInt64(byte[] buffer, int offset, ulong v)
		{
			_int64.l = v;
			buffer[offset+0] = _int64.b1;
			buffer[offset+1] = _int64.b2;
			buffer[offset+2] = _int64.b3;
			buffer[offset+3] = _int64.b4;
			buffer[offset+4] = _int64.b5;
			buffer[offset+5] = _int64.b6;
			buffer[offset+6] = _int64.b7;
			buffer[offset+7] = _int64.b8;			
		}
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteDouble(byte[] buffer, int offset, double v)
		{
			_float64.d = v;
			buffer[offset+0] = _float64.b1;
			buffer[offset+1] = _float64.b2;
			buffer[offset+2] = _float64.b3;
			buffer[offset+3] = _float64.b4;
			buffer[offset+4] = _float64.b5;
			buffer[offset+5] = _float64.b6;
			buffer[offset+6] = _float64.b7;
			buffer[offset+7] = _float64.b8;		
		}
		
		
		// Variables
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}

}

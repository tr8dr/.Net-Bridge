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
	///  Writes binary data, swapping endianness
	/// </summary>
	public class SwapEndianWriter : IBinaryWriter
	{
		public SwapEndianWriter (Stream stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt16 (short value)
		{
			_int16.i = unchecked((ushort)value);
			_stream.WriteByte(_int16.b2);
			_stream.WriteByte(_int16.b1);
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt32 (int value)
		{
			_int32.i = unchecked((uint)value);
			_stream.WriteByte(_int32.b4);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b1);
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt64 (long value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b8);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b1);
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt16 (ushort value)
		{
			_int16.i = value;
			_stream.WriteByte(_int16.b2);
			_stream.WriteByte(_int16.b1);
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt32 (uint value)
		{
			_int32.i = value;
			_stream.WriteByte(_int32.b4);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b1);
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt64 (ulong value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b8);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b1);
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteDouble (double value)
		{
			_float64.d = value;
			_stream.WriteByte(_float64.b8);
			_stream.WriteByte(_float64.b7);
			_stream.WriteByte(_float64.b6);
			_stream.WriteByte(_float64.b5);
			_stream.WriteByte(_float64.b4);
			_stream.WriteByte(_float64.b3);
			_stream.WriteByte(_float64.b2);
			_stream.WriteByte(_float64.b1);
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			value = value ?? "";
			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}
		
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));

			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
		
	}
	
	
	/// <summary>
	///  Writes binary data, preserving endianness
	/// </summary>
	public class SameEndianWriter : IBinaryWriter
	{
		public SameEndianWriter (Stream stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt16 (short value)
		{
			_int16.i = unchecked((ushort)value);
			_stream.WriteByte(_int16.b1);
			_stream.WriteByte(_int16.b2);
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt32 (int value)
		{
			_int32.i = unchecked((uint)value);
			_stream.WriteByte(_int32.b1);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b4);
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt64 (long value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b1);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b8);
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt16 (ushort value)
		{
			_int16.i = value;
			_stream.WriteByte(_int16.b1);
			_stream.WriteByte(_int16.b2);
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt32 (uint value)
		{
			_int32.i = value;
			_stream.WriteByte(_int32.b1);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b4);
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt64 (ulong value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b1);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b8);
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteDouble (double value)
		{
			_float64.d = value;
			_stream.WriteByte(_float64.b1);
			_stream.WriteByte(_float64.b2);
			_stream.WriteByte(_float64.b3);
			_stream.WriteByte(_float64.b4);
			_stream.WriteByte(_float64.b5);
			_stream.WriteByte(_float64.b6);
			_stream.WriteByte(_float64.b7);
			_stream.WriteByte(_float64.b8);
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			if (value == null)
				throw new ArgumentNullException("value");

			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}


		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			
			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));
			
			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
		
	}

	
	
	
	/// <summary>
	///  Writes binary data, preserving endianness
	/// </summary>
	public class SameEndianBufferedWriter : IBinaryWriter
	{
		public SameEndianBufferedWriter (BufferedRandomAccessFile stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt16 (short value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					short* pshort = (short*)(pbuf + offset);
					*pshort = value;
				}
			}
			else
			{
				_int16.i = unchecked((ushort)value);
				_stream.WriteByte(_int16.b1);
				_stream.WriteByte(_int16.b2);
			}
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt32 (int value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					int* pint = (int*)(pbuf + offset);
					*pint = value;
				}
			}
			else
			{
				_int32.i = unchecked((uint)value);
				_stream.WriteByte(_int32.b1);
				_stream.WriteByte(_int32.b2);
				_stream.WriteByte(_int32.b3);
				_stream.WriteByte(_int32.b4);
			}
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt64 (long value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					long* plong = (long*)(pbuf + offset);
					*plong = value;
				}
			}
			else
			{
				_int64.l = unchecked((ulong)value);
				_stream.WriteByte(_int64.b1);
				_stream.WriteByte(_int64.b2);
				_stream.WriteByte(_int64.b3);
				_stream.WriteByte(_int64.b4);
				_stream.WriteByte(_int64.b5);
				_stream.WriteByte(_int64.b6);
				_stream.WriteByte(_int64.b7);
				_stream.WriteByte(_int64.b8);
			}
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt16 (ushort value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					ushort* pshort = (ushort*)(pbuf + offset);
					*pshort = value;
				}
			}
			else
			{
				_int16.i = value;
				_stream.WriteByte(_int16.b1);
				_stream.WriteByte(_int16.b2);
			}
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt32 (uint value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					uint* pint = (uint*)(pbuf + offset);
					*pint = value;
				}
			}
			else
			{
				_int32.i = value;
				_stream.WriteByte(_int32.b1);
				_stream.WriteByte(_int32.b2);
				_stream.WriteByte(_int32.b3);
				_stream.WriteByte(_int32.b4);
			}
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt64 (ulong value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					ulong* plong = (ulong*)(pbuf + offset);
					*plong = value;
				}
			}
			else
			{
				_int64.l = value;
				_stream.WriteByte(_int64.b1);
				_stream.WriteByte(_int64.b2);
				_stream.WriteByte(_int64.b3);
				_stream.WriteByte(_int64.b4);
				_stream.WriteByte(_int64.b5);
				_stream.WriteByte(_int64.b6);
				_stream.WriteByte(_int64.b7);
				_stream.WriteByte(_int64.b8);
			}
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteDouble (double value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					double* pdbl = (double*)(pbuf + offset);
					*pdbl = value;
				}
			}
			else
			{
				_float64.d = value;
				_stream.WriteByte(_float64.b1);
				_stream.WriteByte(_float64.b2);
				_stream.WriteByte(_float64.b3);
				_stream.WriteByte(_float64.b4);
				_stream.WriteByte(_float64.b5);
				_stream.WriteByte(_float64.b6);
				_stream.WriteByte(_float64.b7);
				_stream.WriteByte(_float64.b8);
			}
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			if (value == null)
				throw new ArgumentNullException("value");

			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}


		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			
			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));
			
			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private BufferedRandomAccessFile	_stream;
		
		private Int16Union					_int16 = new Int16Union(0);
		private Int32Union					_int32 = new Int32Union(0);
		private Long64Union					_int64 = new Long64Union(0);
		private Float64Union				_float64 = new Float64Union(0);
		
	}
	
}


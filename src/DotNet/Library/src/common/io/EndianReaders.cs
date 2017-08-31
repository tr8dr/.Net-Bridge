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
using System.IO;
using System.Text;


namespace bridge.common.io
{
	/// <summary>
	/// Reads binary data swapping endianness
	/// </summary>
	public class SwapEndianReader : IBinaryReader
	{
		public SwapEndianReader (Stream stream)
		{
			_stream = stream;
		}


		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
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
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16()
		{
			_int16.b2 = InternalReadByte();
			_int16.b1 = InternalReadByte();
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32()
		{
			_int32.b4 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b1 = InternalReadByte();
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64()
		{
			_int64.b8 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b1 = InternalReadByte();
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16()
		{
			_int16.b2 = InternalReadByte();
			_int16.b1 = InternalReadByte();
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32()
		{
			_int32.b4 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b1 = InternalReadByte();
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64()
		{
			_int64.b8 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b1 = InternalReadByte();
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble()
		{
			_float64.b8 = InternalReadByte();
			_float64.b7 = InternalReadByte();
			_float64.b6 = InternalReadByte();
			_float64.b5 = InternalReadByte();
			_float64.b4 = InternalReadByte();
			_float64.b3 = InternalReadByte();
			_float64.b2 = InternalReadByte();
			_float64.b1 = InternalReadByte();
			
			return _float64.ToDouble();
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);

			return encoding.GetString(data);
		}

		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			byte[] data = new byte[size];
			ReadInternal (data, size);

			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);

			return encoding.GetString (data, 0, len);
		}

		
		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
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
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianReader : IBinaryReader
	{
		public SameEndianReader (Stream stream)
		{
			_stream = stream;
		}

		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
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
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16()
		{
			_int16.b1 = InternalReadByte();
			_int16.b2 = InternalReadByte();
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32()
		{
			_int32.b1 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b4 = InternalReadByte();
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64()
		{
			_int64.b1 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b8 = InternalReadByte();
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16()
		{
			_int16.b1 = InternalReadByte();
			_int16.b2 = InternalReadByte();
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32()
		{
			_int32.b1 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b4 = InternalReadByte();
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64()
		{
			_int64.b1 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b8 = InternalReadByte();
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble()
		{
			_float64.b1 = InternalReadByte();
			_float64.b2 = InternalReadByte();
			_float64.b3 = InternalReadByte();
			_float64.b4 = InternalReadByte();
			_float64.b5 = InternalReadByte();
			_float64.b6 = InternalReadByte();
			_float64.b7 = InternalReadByte();
			_float64.b8 = InternalReadByte();
			
			return _float64.ToDouble();
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);
			
			encoding = encoding ?? Encoding.ASCII;
			return encoding.GetString(data);
		}

		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			byte[] data = new byte[size];
			ReadInternal (data, size);
			
			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);
			
			return encoding.GetString (data, 0, len);
		}


		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
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
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianBufferedReader : IBinaryReader
	{
		public SameEndianBufferedReader (BufferedRandomAccessFile stream)
		{
			_stream = stream;
		}

		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
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
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		unsafe public short ReadInt16()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					short* pshort = (short*)(pbuf + offset);
					return *pshort;
				}
			}
			else
			{
				_int16.b1 = InternalReadByte();
				_int16.b2 = InternalReadByte();
				return _int16.ToShort();
			}
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		unsafe public int ReadInt32()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					int* pint = (int*)(pbuf + offset);
					return *pint;
				}
			}
			else
			{
				_int32.b1 = InternalReadByte();
				_int32.b2 = InternalReadByte();
				_int32.b3 = InternalReadByte();
				_int32.b4 = InternalReadByte();
				
				return _int32.ToInt();
			}
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		unsafe public long ReadInt64()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					long* plong = (long*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_int64.b1 = InternalReadByte();
				_int64.b2 = InternalReadByte();
				_int64.b3 = InternalReadByte();
				_int64.b4 = InternalReadByte();
				_int64.b5 = InternalReadByte();
				_int64.b6 = InternalReadByte();
				_int64.b7 = InternalReadByte();
				_int64.b8 = InternalReadByte();
				
				return _int64.ToLong();
			}
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		unsafe public ushort ReadUInt16()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					ushort* pshort = (ushort*)(pbuf + offset);
					return *pshort;
				}
			}
			else
			{
				_int16.b1 = InternalReadByte();
				_int16.b2 = InternalReadByte();
			
				return _int16.ToUShort();
			}
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		unsafe public uint ReadUInt32()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					uint* pint = (uint*)(pbuf + offset);
					return *pint;
				}
			}
			else
			{
				_int32.b1 = InternalReadByte();
				_int32.b2 = InternalReadByte();
				_int32.b3 = InternalReadByte();
				_int32.b4 = InternalReadByte();
			
				return _int32.ToUInt();
			}
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		unsafe public ulong ReadUInt64()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					ulong* plong = (ulong*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_int64.b1 = InternalReadByte();
				_int64.b2 = InternalReadByte();
				_int64.b3 = InternalReadByte();
				_int64.b4 = InternalReadByte();
				_int64.b5 = InternalReadByte();
				_int64.b6 = InternalReadByte();
				_int64.b7 = InternalReadByte();
				_int64.b8 = InternalReadByte();
				
				return _int64.ToULong();
			}
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		unsafe public double ReadDouble()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					double* plong = (double*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_float64.b1 = InternalReadByte();
				_float64.b2 = InternalReadByte();
				_float64.b3 = InternalReadByte();
				_float64.b4 = InternalReadByte();
				_float64.b5 = InternalReadByte();
				_float64.b6 = InternalReadByte();
				_float64.b7 = InternalReadByte();
				_float64.b8 = InternalReadByte();
				
				return _float64.ToDouble();
			}
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);
			return encoding.GetString(data);
		}
		
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			byte[] data = new byte[size];
			ReadInternal (data, size);
			
			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);
			
			return encoding.GetString (data, 0, len);
		}

		
		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
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


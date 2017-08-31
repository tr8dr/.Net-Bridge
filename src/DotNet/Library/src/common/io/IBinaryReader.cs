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
	/// Interface for binary stream reader 
	/// </summary>
	public interface IBinaryReader : IDisposable
	{
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		void 					Close();

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		void 					Seek (int offset, SeekOrigin origin);
		
		/// <summary>
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read or -1 if EOF seen</returns>
		int 					ReadByte();

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		bool 					ReadBoolean();

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		short 					ReadInt16();

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		int 					ReadInt32();	

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		long 					ReadInt64();

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		ushort 					ReadUInt16();

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		uint 					ReadUInt32();		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		ulong 					ReadUInt64();
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		double 					ReadDouble();
		
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
		int 					Read (byte[] buffer, int index, int count);

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		string 					ReadString (Encoding encoding = null);
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		string 					ReadStringField (int size, Encoding encoding = null);

	}
}


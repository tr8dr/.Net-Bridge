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
	/// Interface for binary stream writer 
	/// </summary>
	public interface IBinaryWriter : IDisposable
	{
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		void 					Close();
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		void 					Flush();

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		void 					Seek (int offset, SeekOrigin origin);
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteBool (bool value);

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteInt16 (short value);

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void					WriteInt32 (int value);

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteInt64 (long value);

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt16 (ushort value);

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt32 (uint value);
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt64 (ulong value);
		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteDouble (double value);

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteByte (byte value);

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUByte (sbyte value);
		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		void 					Write (byte[] value, int offset, int count);
		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		void 					WriteString (string value, Encoding encoding = null);

		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		void 					WriteStringField (string value, int size, Encoding encoding = null);

	}
}


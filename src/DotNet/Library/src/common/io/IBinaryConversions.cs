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
	/// Interface for converting endian bytes into proper form 
	/// </summary>
	public interface IBinaryConversions
	{
		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		short 					ReadInt16(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		int 					ReadInt32(byte[] buffer, int offset);	

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		long 					ReadInt64(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		ushort 					ReadUInt16(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		uint 					ReadUInt32(byte[] buffer, int offset);		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		ulong 					ReadUInt64(byte[] buffer, int offset);
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		double 					ReadDouble(byte[] buffer, int offset);		

		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		void 					WriteInt16(byte[] buffer, int offset, short v);

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		void 					WriteInt32(byte[] buffer, int offset, int v);	

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		void 					WriteInt64(byte[] buffer, int offset, long v);

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		void 					WriteUInt16(byte[] buffer, int offset, ushort v);

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		void 					WriteUInt32(byte[] buffer, int offset, uint v);		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		void 					WriteUInt64(byte[] buffer, int offset, ulong v);
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		void 					WriteDouble(byte[] buffer, int offset, double v);		
	}
}


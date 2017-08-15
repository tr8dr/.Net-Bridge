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
using System.Linq;

namespace bridge.common.io
{
	/// <summary>
	/// A stream that concatenates two or more streams
	/// </summary>
	public class ConcatenatedStream : Stream
	{
		public ConcatenatedStream (params Stream[] underlier)
		{
			StreamList = underlier;
			_pos = 0L;
			_Icurrent = 0;
			_len = StreamList.Sum (v => v.Length - v.Position);
		}


		// Properties

		public Stream[] StreamList
			{ get; private set; }

		public int Available
		{ get { return (int)(_pos - _len); } }

		public override bool CanRead
			{ get { return true; } }

		public override bool CanWrite
			{ get { return false; } }

		public override bool CanSeek
			{ get { return false; } }

		public override long Length
			{ get { return _len; } }

		public override long Position
		{ 
			get { return _pos; } 
			set { throw new Exception ("position cannot be set"); } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			for (int i = 0; i < StreamList.Length; i++)
				StreamList [i].Close ();
		}


		/// <summary>
		/// Read into the specified buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			var read = 0;

			while (count > 0 && _Icurrent < StreamList.Length)
			{
				var done = StreamList [_Icurrent].Read (buffer, offset, count);
				if (done == 0)
					_Icurrent++; 
					
				_pos += done;
				read += done;
				offset += done;
				count -= done;
			}

			return read;
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			if (_Icurrent == StreamList.Length)
				return -1;

			while (_Icurrent < StreamList.Length)
			{
				var c = StreamList [_Icurrent].ReadByte ();
				if (c >= 0)
					return c;
				else
					_Icurrent++;
			}

			return -1;
		}


		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Seek to the specified offset and origin.
		/// </summary>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			throw new NotImplementedException ("SetLength operation only for writeable streams");
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			throw new NotImplementedException ("WriteByte operation only for writeable streams");
		}


		/// <summary>
		/// Writes the buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ("Write operation only for writeable streams");
		}


		// Variables

		private long		_len;
		private long		_pos;
		private int			_Icurrent;
	}
}


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


namespace bridge.common.io
{
	/// <summary>
	/// Buffered write-only stream with indication of how many bytes are available in buffer.
	/// </summary>
	public class BufferedWriteStream : Stream
	{
		public BufferedWriteStream (Stream underlier, int buffersize = 4096)
		{
			Underlier = underlier;
			_buffer = new byte[buffersize];
		}


		// Properties

		public Stream Underlier
			{ get; private set; }

		public override bool CanRead
			{ get { return false; } }

		public override bool CanWrite
			{ get { return true; } }

		public override bool CanSeek
			{ get { return Underlier.CanSeek; } }

		public override long Length
			{ get { return Underlier.Length; } }

		public override long Position
		{ 
			get { return Underlier.Position + _pos; } 
			set { Underlier.Position = value;  _pos = 0; } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			Flush ();
			Underlier.Close();
			_pos = 0;
			_buffer = null;
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
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}

		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			if (_pos == 0)
				return;

			Underlier.Write (_buffer, 0, _pos);
			_pos = 0;
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
			var pos = Underlier.Seek (offset, origin);
			_pos = 0;

			return pos;
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			Underlier.SetLength (value);
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			_buffer[_pos++] = value;

			if (_pos == _buffer.Length)
				Flush ();
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
			while (count > 0)
			{
				var write = Math.Min (count, _buffer.Length - _pos);
				Array.Copy (buffer, offset, _buffer, _pos, write);

				_pos += write;
				count -= write;
				offset += write;

				if (_pos == _buffer.Length)
					Flush ();
			}
		}


		// Variables

		private byte[]		_buffer;
		private int			_pos = 0;
	}
}


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
using bridge.common.utils;

namespace bridge.common.io
{
	/// <summary>
	/// Buffered read/write stream, where read and write are separate and do not intersect
	/// </summary>
	public class BufferedDuplexStream : Stream
	{
		public BufferedDuplexStream (Stream underlier)
		{
			Underlier = underlier;
		}


		// Properties

		public Stream Underlier
			{ get; private set; }

		public int Available
			{ get { return _rsize - _rpos; } }

		public override bool CanRead
			{ get { return true; } }

		public override bool CanWrite
			{ get { return true; } }

		public override bool CanSeek
			{ get { return false; } }

		public override long Length
			{ get { return Underlier.Length; } }

		public override long Position
		{ 
			get { throw new ArgumentException ("cannot support seeking on duplex stream"); } 
			set { throw new ArgumentException ("cannot support seeking on duplex stream"); } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			Flush ();
			Underlier.Close();
			_rpos = 0;
			_rsize = 0;
			_wpos = 0;
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
			Refill ();
			var done = Math.Min(_rsize - _rpos, count);
			Array.Copy (_rbuffer, _rpos, buffer, offset, done);
			
			_rpos += done;
			return done;
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			Refill();
			
			if (_rpos < _rsize)
				return _rbuffer[_rpos++];
			else
				return -1;
		}


		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			if (_wpos == 0)
				return;

			Underlier.Write (_wbuffer, 0, _wpos);
			Underlier.Flush ();
			_wpos = 0;
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
			throw new ArgumentException ("cannot support seeking on duplex stream");
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			throw new ArgumentException ("cannot support seeking on duplex stream");
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			switch (_wbuffer.Length - _wpos)
			{
				case 0:
					Flush ();
					_wbuffer [_wpos++] = value;
					break;

				case 1:
					_wbuffer [_wpos++] = value;
					Flush ();
					break;

				default:
					_wbuffer [_wpos++] = value;
					break;
			}
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
			if (count < 0)
				throw new ArgumentException ("Write: attempted to write a buffer with length < 0");

			while (count > 0)
			{
				var write = CmpUtils.Constrain (count, 0, _wbuffer.Length - _wpos);

				// flush if cannot write more to the buffer
				if (write == 0)
				{
					Flush ();
					continue;
				}

				Array.Copy (buffer, offset, _wbuffer, _wpos, write);

				_wpos += write;
				count -= write;
				offset += write;
			}

			if (_wpos == _wbuffer.Length)
				Flush ();
		}


		#region Implementation
		
		private void Refill ()
		{
			if (_rpos < _rsize)
				return;
			
			_rpos = 0;
			_rsize = 0;

			_rsize = Underlier.Read (_rbuffer, 0, _rbuffer.Length);
		}


		#endregion

		// Variables

		private byte[]		_rbuffer = new byte[4096];
		private byte[]		_wbuffer = new byte[4096];
		private int			_rpos = 0;
		private int			_wpos = 0;
		private int			_rsize = 0;
	}
}


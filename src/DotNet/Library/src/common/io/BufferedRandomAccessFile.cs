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
using bridge.common.collections;
using bridge.common.utils;
using bridge.common.system;
using System.Threading;
using bridge.common.serialization;
using System.Diagnostics;

namespace bridge.common.io
{
	/// <summary>
	/// Buffered implementation against a random access stream
	/// </summary>
	public sealed class BufferedRandomAccessFile : FileStream, IDisposable
	{
		public enum State
			{ Uninitialized, Empty, HasData, Dirty };
			
		public enum Op
			{ Read, Write, ReadWrite };

		public BufferedRandomAccessFile (string file, int blocksize = 8192, int maxsize = 4*8192, FileAccess mode = FileAccess.ReadWrite) 
			: base (file, FileMode.OpenOrCreate, mode)
		{
			_filename = file;
			_blocksize = blocksize;
			
			var nblocks = (int)(maxsize / blocksize);
			_cache = new LRUCache<int,Buffer>(x => blocksize, maxsize);
			_pool = new STObjectPool<Buffer> (nblocks + 1, () => new Buffer(this, blocksize));
			_Flen = base.Length;
		}
		
		
		~BufferedRandomAccessFile ()
		{
			Close();
		}
		
		
		// Properties
		
		
		public long ReadPosition
			{ get { return _rpos; } }
		
		public long WritePosition
			{ get { return _wpos; } }
		
		public override long Position
		{
			get
			{
				throw new Exception ("Position: not supported, refer to ReadPosition or WritePosition");
			}
			set
			{
				throw new Exception ("Position: not supported, refer to ReadPosition or WritePosition");
			}
		}
		
				
		// Functions
		

		/// <summary>
		/// Flush write buffers
		/// </summary>
		public override void Flush ()
		{
			if (_writing != null)
				_writing.Flush();
		}
			
		
		/// <summary>
		/// Close the file
		/// </summary>
		public override void Close ()
		{
			Flush ();
			base.Close ();
		}
		
		
		/// <summary>
		/// Current length of file
		/// </summary>
		public override long Length
		{
			get { return _Flen; }
		}
		
		
		/// <summary>
		/// Sets new file length.
		/// </summary>
		/// <param name='newLength'>
		/// New file length
		/// </param>
		public override void SetLength (long newLength)
		{
			base.SetLength (newLength);
		
			_rpos = Math.Min (_rpos, newLength);
			_wpos = Math.Min (_wpos, newLength);
			_Flen = newLength;
		}
	
				
		/// <summary>
		/// Provides a buffered extent of desired size if available contiguously for reading.
		/// The # of byte read MUST correspond to the nbytes parameter, as the stream will advance
		/// by the indicated amount.
		/// </summary>
		/// <returns>
		/// true if the extent found
		/// </returns>
		/// <param name='buffer'>
		/// the buffer holding the extent (output param)
		/// </param>
		/// <param name='offset'>
		/// offset in buffer (output param)
		/// </param>
		/// <param name='nbytes'>
		/// number of bytes wants to locate
		/// </param>
		public bool ReadExtent (out byte[] buffer, out long offset, int nbytes)
		{
			Buffer buf = GetReadBufferFor (_rpos);
			if ((buf.len - buf.rpos) >= nbytes)
			{
				buffer = buf.data;
				offset = buf.rpos;
				buf.rpos += nbytes;
				_rpos += nbytes;
				return true;
			} 
			else
			{
				buffer = null;
				offset = 0;
				return false;
			}
		}
	
				
		/// <summary>
		/// Provides a buffered extent of desired size if available contiguously for writing
		/// The # of bytes written MUST correspond to the nbytes parameter, as the stream will advance
		/// by the indicated amount.
		/// </summary>
		/// <returns>
		/// true if the extent found
		/// </returns>
		/// <param name='buffer'>
		/// the buffer holding the extent (output param)
		/// </param>
		/// <param name='offset'>
		/// offset in buffer (output param)
		/// </param>
		/// <param name='nbytes'>
		/// number of bytes wants to locate
		/// </param>
		public bool WriteExtent (out byte[] buffer, out long offset, int nbytes)
		{
			Buffer buf = GetWriteBufferFor (_wpos);
			if ((buf.data.Length - buf.wpos) >= nbytes)
			{
				buffer = buf.data;
				offset = buf.wpos;
				buf.wpos += nbytes;
				buf.len = Math.Max (buf.wpos, buf.len);
				_wpos += nbytes;
				_Flen = Math.Max (_wpos, _Flen);
				return true;
			} 
			else
			{
				buffer = null;
				offset = 0;
				return false;
			}
		}
		
				
		/// <summary>
		/// Reads a byte from buffer
		/// </summary>
		/// <returns>
		/// The byte.
		/// </returns>
		public override int ReadByte() 
		{
			if (_rpos < _Flen)
			{
				Buffer buf = GetReadBufferFor (_rpos++);
				return (int)buf.data[buf.rpos] & 0xff;
			} else
				return -1;
		} 

		
		
		/// <summary>
		/// Reads a block of bytes from the stream and writes the data in a given buffer. (Overrides Stream.Read(Byte[], Int32, Int32).)
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='off'>
		/// Offet
		/// </param>
		/// <param name='len'>
		/// Length.
		/// </param>
		public override int Read (byte[] buffer, int off, int len) 
		{
			int read = 0;
			
			while (read < len && _rpos < _Flen)
			{
				Buffer buf = GetReadBufferFor (_rpos);
				
				int size = Math.Min (len - read, buf.len - buf.rpos);
				System.Buffer.BlockCopy (buf.data, buf.rpos, buffer, off, size);
				
				_rpos += size;
				read += size;
			}
			
			return (read > 0) ? read : -1;
		}
		
						
		/// <summary>
		/// Reads a block of bytes from the stream and writes the data in a given buffer. (Overrides Stream.Read(Byte[], Int32, Int32).)
		/// </summary>
		public override long Seek (long pos, SeekOrigin origin) 
		{
			throw new IOException ("Must use seek() variant that indicates reader or writer");
		}
		
		
		/// <summary>
		/// Seek to the specified position for reading or writing
		/// </summary>
		/// <param name='op'>
		/// Op.
		/// </param>
		/// <param name='pos'>
		/// Position.
		/// </param>
		public void Seek (Op op, long pos) 
		{
			switch (op)
			{
				case Op.Read:
					_rpos = pos;
					break;
				case Op.Write:
					_wpos = pos;
					break;
				default:
					_rpos = _wpos = pos;
					break;
			}
		}
		
		
		/// <summary>
		/// Writes the specified buffer, given offset and len.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='off'>
		/// Offset.
		/// </param>
		/// <param name='len'>
		/// Length.
		/// </param>
		public override void Write(byte[] buffer, int off, int len) 
		{
			int written = 0;
			Buffer buf = null;
			
			while (written < len)
			{
				buf = GetWriteBufferFor (_wpos); 
				
				int amount = Math.Min (len, _blocksize - buf.wpos);
				System.Buffer.BlockCopy (buffer, off, buf.data, buf.wpos, amount);
				
				_wpos += amount;
				written += amount;
				_Flen = Math.Max (_wpos + amount, _Flen);
				
				buf.len = buf.wpos + amount;
			}
		}
				
		
		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='b'>
		/// Byte to be written
		/// </param>
		public override void WriteByte (byte b) 
		{
			Buffer buf = GetWriteBufferFor (_wpos++);
			buf.data[buf.wpos] = b;
			buf.len = Math.Max (buf.wpos+1, buf.len);
			
			_Flen = Math.Max (_wpos, _Flen);
		}
	
		
		
		// Implementation
			
		
		private Buffer GetBufferFor (long pos)
		{
			lock (_cache)
			{
				var newpage = (int)(pos / _blocksize);	
				Buffer buf = _cache[newpage];
			
				// may need to create or get from pool
				if (buf == null)
				{
					buf = _pool.Alloc ();
					Debug.Assert (buf.state == State.Uninitialized);
				}
		
				switch (buf.state)
				{
					case State.Dirty:
					case State.HasData:
						return buf;
					
					default:
						long Fbase = newpage * (long)_blocksize;
						buf.len = (int)Math.Min((long)(_Flen - Fbase), _blocksize);
						buf.page = newpage;
						
						ReadBlock (Fbase, buf.data, buf.len);
						
						buf.state = State.HasData;
						_cache[newpage] = buf;
						return buf;
				}
			}
		}
		
		
		private Buffer GetReadBufferFor (long pos)
		{
			int Bpos = (int)(pos % _blocksize);
			int newpage = (int)(pos / _blocksize);
			
			if (_reading == null || _reading.page != newpage)
				_reading = GetBufferFor (pos);
	
			_reading.rpos = Bpos;		
			return _reading;
		}
	
		
		
		private Buffer GetWriteBufferFor (long pos)
		{
			int Bpos = (int)(pos % _blocksize);
			int newpage = (int)(pos / _blocksize);
	
			if (_writing != null && _writing.page == newpage)
			{
				_writing.wpos = Bpos;
				_writing.state = State.Dirty;
				return _writing;
			}
			
			if (_writing != null && _writing.state == State.Dirty)
				Flush ();
			
			_writing = GetBufferFor (pos);
			_writing.wpos = Bpos;
			_writing.state = State.Dirty;
			
			return _writing;
		}
	
	
		
		/**
		 * Read in a block from a position in file
		 */
		private void ReadBlock (long position, byte[] buffer, int len)
		{
			lock (_cache)
			{
				base.Seek(position, SeekOrigin.Begin);
				
				int read = 0;
				while (read < len)
				{
					int n = base.Read (buffer, read, len - read);
					if (n >= 0)
						read += n;
					else
						throw new IOException ("hit EOF prematurely");
				}
			}
		}
		
		
		/**
		 * Write block to specific position
		 */
		private void WriteBlock (long position, byte[] buffer, int len)
		{
			lock (_cache)
			{
				base.Seek (position, SeekOrigin.Begin);		
				base.Write (buffer, 0, len);
			}
		}
		
		
		// Classes
		
		
		internal class Buffer : IDisposable
		{
			public Buffer (BufferedRandomAccessFile file, int size)
			{ 
				this.file = file;
				this.data = new byte[size];
				this.state = State.Uninitialized;
			}
			
			
			public Buffer ()
				{ }
			
			
			// Data

			public byte[]						data;
			public long							page;
	
			public int							rpos;
			public int							wpos;
			public int							len;
			public State						state;
			public BufferedRandomAccessFile		file;
			

			// Methods
			
			
			public void Dispose ()
			{
				if (state == State.Dirty)
					Flush ();
				if (state == State.Uninitialized)
					return;

				state = State.Uninitialized;
				page = -1;
				file._pool.Free(this);
			}
			
			
			public void Flush ()
			{
				if (state != State.Dirty)
					return;
				
				long pos = page * (long)file._blocksize;
	
				if (_log.IsDebugEnabled)
					_log.Info ("flushing block " + page + ", on: " + file._filename + ", range: [" + pos + ", " + (pos+len) + "]");
				
				file.WriteBlock (pos, data, len);		
				
				state = State.HasData;
			}
		}
		
		
		
		// Variables
		
		private string						_filename;
		private STObjectPool<Buffer>		_pool;				// pool of blocks to use
		private LRUCache<int,Buffer>		_cache;				// cache of blocks
		
		private Buffer						_reading;			// current read buffer
		private Buffer						_writing;			// current write buffer
		
		private int							_blocksize;
		private long						_rpos = 0L;			// current read position in file
		private long						_wpos = 0L;			// current write position in file
		private long						_Flen = -1;			// length of file	
		
		static Logger						_log = Logger.Get("IO");
	}
}


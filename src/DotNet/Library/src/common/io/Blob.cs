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
using System.Text;
using System.Collections.Generic;


namespace bridge.common.io
{
	/// <summary>
	/// Efficient byte buffer (convertible to string also)
	/// </summary>
	public class Blob
	{
		public Blob (int size)
		{
			_current = new Region (size, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);	
		}
		
		public Blob (byte[] data)
		{
			_current = new Region(data, data.Length, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);
		}
		
		
		public Blob (string sdata)
		{
			var data = TextEncoding.GetBytes(sdata);
			
			_current = new Region(data, data.Length, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);
		}

		public Blob ()
			: this (0) {}
		
		
		// Properties
		
		
		/// <summary>
		/// Gets list of regions.
		/// </summary>
		/// <value>
		/// The regions.
		/// </value>
		public IList<Region> Regions
			{ get { return _list; } }
		
		
		/// <summary>
		/// Gets current length or adjustes length upwards or downwards as indicated in setting
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public int Length
		{ 
			get 
			{ 
				return _alen + _current.Used; 
			}
			set
			{
				int clen = _alen + _current.Used;
				int delta = clen - value;
			
				// simple cases
				int used = _current.Used;
				if (delta < 0 && used >= -delta)
					_current.Used = (used + delta);
				else if (delta >= 0 && _current.Span >= delta)
					_current.Used = (used + delta);
			
				// case where need to trim buffers
				clen = 0;
				for (int i = 0 ; i < _list.Count ; i++)
				{
					Region region = (Region)_list[i];
					int olen = clen;
					clen += region.Used;
			
					if (clen >= value)
					{
						region.Used = (value - olen);
						for (int j = clen-1 ; j > value ; j--)
							_list.RemoveAt (j);
					}
				}
			
				// case where need to expand
				if (clen < value)
				{
					Region region = Acquire (Math.Max (BufSize, value - clen));
					region.Used = (value - clen);
				}
			}
		}
		
		
		/// <summary>
		/// Gets or sets the ith position in the buffer.
		/// </summary>
		/// <param name='ith'>
		/// index
		/// </param>
		public byte this [int ith]
		{
			get 
			{
				Region region = GetRegionForIndex (ith);
				return region[ith - region.StartingIndex];
			}
			set 
			{
				Region region = GetRegionForIndex (ith);
				region[ith - region.StartingIndex] = value;
			}
		}
		
		
		/// <summary>
		/// Get as byte array.
		/// </summary>
		public byte[] AsBytes
		{
			get
			{
				// special case where buffer is exactly the right size, etc
				if (_list.Count == 1 && _current.Span == 0)
					return _current.Bytes;
		
				// otherwise copy to one large array
				byte[] buffer = new byte[Length];
				int offset = 0;
		
				for (int i = 0 ; i < _list.Count ; i++)
				{
					Region region = (Region)_list[i];
					int used = region.Used;
					byte[] rbuffer = region.Bytes;
					
					System.Buffer.BlockCopy (rbuffer, 0, buffer, offset, used);
						
					offset += used;
				}
		
				// adjust buffer
				_list.Clear ();
				_alen = 0;
				_current = new Region (buffer, buffer.Length, 0);
				_list.Add (_current);
		
				return buffer;
			}
		}
		
		
		/// <summary>
		/// Gets the buffer as string.
		/// </summary>
		/// <value>
		/// The string.
		/// </value>
		public string AsString
			{ get { return TextEncoding.GetString(AsBytes); } }
	
		
		/// <summary>
		/// Provide blob as byte array
		/// </summary>
		public static implicit operator byte[] (Blob v)
			{ return v != null ? v.AsBytes : null; }

		
		/// <summary>
		/// Provide blob as string
		/// </summary>
		public static implicit operator string (Blob v)
			{ return v != null ? v.AsString : null; }
		
		/// <summary>
		/// Return as string
		/// </summary>
		public override string ToString()
		{
			return AsString;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Append ASCII string to buffer
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void Append (string text)
		{
			byte[] bytes = TextEncoding.GetBytes (text);
			Append (bytes, 0, bytes.Length);
		}
		
		
		/**
		 * Append buffer to current binary buffer (or create new one)
		 * 
		 * @param buffer	buffer to be appended (copied)
		 * @param offset	offset in buffer
		 * @param len		length of region to be copied
		 */
		public void Append (byte[] buffer, int offset, int len)
		{
			Region region = _current;
	
			while (len > 0)
			{
				// replenish buffer if necessary
				if (region.Span == 0)
					region = Acquire (Math.Max (BufSize, len));
	
				// determine span
				int cspan = Math.Min (region.Span, len);
	
				// copy into buffer
				byte[] rbytes = region.Bytes;
				int roffset = region.Offset;
				System.Buffer.BlockCopy (buffer, offset, rbytes, roffset, cspan);
					
				region.Used = (region.Used + cspan);
				len -= cspan;
			}
		}
		
		
		/**
		 * Append byte to buffer
		 * 
		 * @param c	byte to be appended
		 */
		public void Append (byte c)
		{
			if (_current.Span == 0)
				Acquire (BufSize);
	
			
			int used = _current.Used;
			byte[] rbuffer = _current.Bytes;
			
			rbuffer [used] = c;
			_current.Used = (used+1);
		}

		
		
		/**
		 * Acquire buffer to write into (may be less than the requested though not 0 if "atleast" is false
		 * otherwise will provide a buffer of at least the size requested)
		 * <p>
		 * "Used" property MUST be adjusted after acquisition!
		 * 
		 * @param size		size guideline for acquired buffer
		 * @param atleast	indicate whether size returned must be minimally the requested
		 * @return			the region for the sub-buffer
		 */
		public Region Acquire (int size, bool atleast)
		{
			int cspan = _current.Span;
	
			if (cspan >= size)
				return _current;
			if (cspan > 0 && cspan >= (size /4) && !atleast)
				return _current;
				
			_alen += _current.Used;
			_current = new Region (size, 0);
			_list.Add (_current);
			return _current;
		}
	
	
		
		/**
		 * Acquire buffer to write into (may be less than the requested though not 0 if "atleast" is false
		 * otherwise will provide a buffer of at least the size requested)
		 * <p>
		 * "Used" property MUST be adjusted after acquisition!
		 * 
		 * @param size		size guideline for acquired buffer
		 * @return			the region for the sub-buffer
		 */
		public Region Acquire (int size)
		{
			return Acquire (size, true);
		}
		
		
		/**
		 * Find region for given index
		 *
		 * @param ith		index
		 * @return			region for this index
		 */
		public Region GetRegionForIndex (int ith)
		{
			int pos = 0;
			for (int i = 0 ; i < _list.Count ; i++)
			{
				Region region = _list[i];
				if (pos+ith < region.Used) return region;
				pos += region.Used;
			}
			
			return null;
		}
		
		
		// Classes
		
		public class Region
		{
			public Region (int size, int startingindex)
			{
				_startingindex = startingindex;
				_used = 0;
				if (size > 0)
					_buffer = new byte [size];
				else
					_buffer = null;
			}
	
			public Region (byte[] buffer, int used, int startingindex)
			{
				_startingindex = startingindex;
				_used = used;
				_buffer = buffer;
			}
	
			public Region ()
				: this (0,0) { }
	
			
			/// <summary>
			/// Gets the buffer region bytes.
			/// </summary>
			/// <value>
			/// The bytes.
			/// </value>
			public byte[] Bytes
			{
				get 
				{
					if (_buffer != null)
						return _buffer;
					else
						return _buffer = new byte [BufSize];
				}
			}
	
			
			/// <summary>
			/// Gets the index of region.
			/// </summary>
			/// <value>
			/// The index of the starting.
			/// </value>
			public int StartingIndex
				{ get { return _startingindex; } }
			
			
			/// <summary>
			/// offset for write position into buffer
			/// </summary>
			/// <value>
			/// The offset.
			/// </value>
			public int Offset
				{ get { return _used; } }
			
			
			/// <summary>
			/// Gets or sets the value at the ith position in buffer region
			/// </summary>
			/// <param name='ith'>
			/// Ith.
			/// </param>
			public byte this[int ith]
				{ get { return _buffer[ith + _startingindex]; } set { _buffer[ith + _startingindex] = value; } }
	
			
			/// <summary>
			/// Span of bytes remaining (free space in buffer)
			/// </summary>
			/// <value>
			/// The span.
			/// </value>
			public int Span
				{ get { return Bytes.Length - _used - _startingindex; } }
	
	
			/// <summary>
			/// get the amount of the region currently used
			/// </summary>
			/// <value>
			/// The used.
			/// </value>
			public int Used
				{ get { return _used; } set { _used = value; } }
					
	
			// variables
	
			private byte[]		_buffer;
			private int			_used;
			private int			_startingindex;	
		}
		
		
		// Constants
		
		private readonly static ASCIIEncoding 	TextEncoding 	= new ASCIIEncoding();
		private readonly static int				BufSize 		= 4192;
		
		// Variables
		
		private IList<Region>	_list;
		private Region			_current;
		private int				_alen;
	}
}


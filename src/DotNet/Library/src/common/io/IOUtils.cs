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

using bridge.common.utils;
using System.Collections;
using System.Collections.Generic;


namespace bridge.common.io
{
	/// <summary>
	/// IO related functions
	/// </summary>
	public class IOUtils
	{
		// reading functions
			
		
		/**
		 * read a line from stream, delimited by \n
		 * 
		 * @param stream		stream to read from
		 * @return				string for line read in or null if EOF
		 * @throws 			IOException
		 */
		public static string ReadLine (Stream stream)
		{
			StringBuilder line = new StringBuilder (1024);
	
			while (true)
			{
				int c = stream.ReadByte();
				if (c == -1)
					return (line.Length > 0) ? line.ToString() : null;
				if (c == '\n')
					return line.ToString();
				else
					line.Append ((char)c);
			}
		}


		/// <summary>
		/// Read until end of Json block
		/// </summary>
		/// <returns>The line.</returns>
		/// <param name="stream">Stream.</param>
		public static string ReadJSonBlock (Stream stream)
		{
			StringBuilder block = new StringBuilder (4096);
			const char NOTQUOTED = (char)0;

			var stack = new Stack<char> ();
			var quoted = NOTQUOTED;
			var raw = 0;

			do
			{
				raw = stream.ReadByte();
				if (raw < 0)
					return block.ToString();

				var c = (char)raw;
				block.Append (c);

				switch (c)
				{
					case '[':
					case '{':
						if (quoted != NOTQUOTED)
							break;
						stack.Push (c);
						break;
					case '"':
					case '\'':
						if (quoted == c)
							quoted = NOTQUOTED;
						else if (quoted == NOTQUOTED)
							quoted = c;
						break;
					case ']':
						if (quoted == NOTQUOTED)
						{
							var top = stack.Pop();
							if (top != '[')
								throw new ArgumentException ("encountered invalid JSON in stream: " + block);
						}
						break;
					case '}':
						if (quoted == NOTQUOTED)
						{
							var top = stack.Pop();
							if (top != '{')
								throw new ArgumentException ("encountered invalid JSON in stream: " + block);
						}
						break;
				}
			}
			while (stack.Count > 0 || raw <= 32);

			return block.ToString ();
		}
				
		
		/**
		 * read a line from stream, reading repeatedly until not empty, delimited by \n
		 * 
		 * @param stream		stream to read from
		 * @return				string for line read in or null if EOF
		 * @throws 			IOException
		 */
		public static string ReadNonEmptyLine (Stream stream)
		{
			while (true)
			{
				// get next line
				string line = ReadLine (stream);
	
				// end of stream?
				if (line == null)
					return null;
	
				// if has non-whitespace, return
				if (!StringUtils.IsBlank(line))
					return line;
			}
		}
		
		
		/**
		 * read exactly N bytes from the given stream
		 * 
		 * @param stream		stream to read from
		 * @param buffer		buffer to place result into
		 * @param n				number of bytes to read
		 * @return				buffer of n bytes or null if failed
		 * @throws 			IOException
		 */
		public static Blob ReadExact (Stream stream, int n, Blob buffer = null)
		{
			if (buffer == null)
				buffer = new Blob();
			
			while (n > 0)
			{
				// acquire buffer for read
				var region = buffer.Acquire (n, false);
	
				// read into buffer
				int cn = stream.Read (region.Bytes, region.Offset, Math.Min (n, region.Span));
				// error, reached EOS prematurely
				if (cn == 0) 
					return null;
					
				// otherwise proceed
				n -= cn;
				region.Used += cn;
			}	
	
			return buffer;
	
		}
		

		/// <summary>
		/// read exactly N bytes from the given stream, or returns null
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		/// <param name='n'>
		/// N.
		/// </param>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public static byte[] ReadExact (Stream stream, int n, byte[] buffer)
		{
			var offset = 0;
			while (n > 0)
			{
				// read into buffer
				int cn = stream.Read (buffer, offset, n);
				// error, reached EOS prematurely
				if (cn == 0) 
					return null;
					
				n -= cn;
				offset += cn;
			}	
	
			return buffer;
		}
	
	
		
		/**
		 * read stream to end
		 * 
		 * @param stream		stream to read from
		 * @return				buffer containing stream content
		 * @throws 			IOException
		 */
		public static Blob ReadToEnd (Stream stream)
		{
			Blob buffer = new Blob ();
	
			while (true)
			{
				// acquire buffer for read
				var region = buffer.Acquire (4096, false);
	
				int n = stream.Read (region.Bytes, region.Offset, region.Span);
				if (n < 0)
				{
				    stream.Close();
					return buffer;
				} else
					region.Used += n;
			}
	
		}
		
			
		
		/**
		 * Read stream until specified terminator encountered
		 * 
		 * @param stream		stream to read from
		 * @param terminator	terminator
		 * @return				buffer spanning until terminator
		 */
		public static Blob ReadUntil (Stream stream, string terminator)
		{
			Blob buffer = new Blob();
	
			// position in terminator
			int tpos = 0;
			int tlen = terminator.Length;
	
			while (true)
			{
				int c = stream.ReadByte ();
	
				// if premature EOF return null
				if (c == -1)
					return null;
	
				// keep track of where we are terminator-wise
				tpos = (c == terminator[tpos]) ? tpos+1 : 0;
			
				// if terminator reached, return content (minus terminator)
				if (tpos == tlen)
				{ 
					buffer.Length = (buffer.Length - (tlen-1)); 
					return buffer; 
				}
	
				buffer.Append ((byte)c);
			}
	
		}
			
		
		
		/**
		 * Copy complete input stream to output stream.  Input stream is closed (possibly) after
		 * reading completes, output stream remains open.
		 * 
		 * @param istream		input stream to read to completion
		 * @param ostream		output stream
		 */
		public static void Copy (Stream istream, Stream ostream, bool close = true)
		{
			byte[] rbuf = new byte[4096];
			
			while (true)
			{
				int n = istream.Read (rbuf, 0, 4096);
				if (n <= 0)
					{ if (close) istream.Close(); return; }
				else
					ostream.Write (rbuf, 0, n);
			}		
		}
	
		
		
		/**
		 * Copy specified # of bytes from input stream to output stream.  
		 * 
		 * @param istream		input stream to read to completion
		 * @param ostream		output stream
		 * @param len			amount to copy
		 */
		public static void Copy (Stream istream, Stream ostream, int len)
		{
			byte[] rbuf = new byte[4096];
			
			while (len > 0)
			{
				int amount = Math.Min (len, 4096);
				int n = istream.Read (rbuf, 0, amount);
				ostream.Write (rbuf, 0, n);
				len -= n;
			}		
		}
		
		
		
		// writing functions
		
		
		/**
		 * write an ASCII string to stream
		 * 
		 * @param stream	stream to write to
		 * @param string	string to write
		 */
		public static void Write (Stream stream, string s)
		{
			for (int i = 0 ; i < s.Length ; i++)
				stream.WriteByte ((byte)s[i]);
		}
	
		
		/**
		 * Write buffer to stream
		 * 
		 * @param stream	stream to write to
		 * @param buffer	buffer to write
		 */
		public static void write (Stream stream, Blob buffer)
		{
			foreach (var region in buffer.Regions)
			{
				stream.Write (region.Bytes, 0, region.Used);	
			}
		}
		
		
	}
}


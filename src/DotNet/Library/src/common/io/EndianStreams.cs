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
	/// Provides network normalized stream reader & writer given endian-ness of architecture
	/// </summary>
	public class EndianStreams
	{
		public enum Endian
			{ Little, Big, Network }
		

				
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		public static IBinaryConversions ConversionsFor (Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return new SameEndianConverter ();
			else
				return new SwapEndianConverter ();			
		}

		
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		public static IBinaryReader ReaderFor (Stream stream, Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return (stream is BufferedRandomAccessFile) ? 
					(IBinaryReader)new SameEndianBufferedReader ((BufferedRandomAccessFile)stream) :
					(IBinaryReader)new SameEndianReader (stream);
			else
				return new SwapEndianReader (stream);			
		}

		
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		public static IBinaryWriter WriterFor (Stream stream, Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return (stream is BufferedRandomAccessFile) ? 
					(IBinaryWriter)new SameEndianBufferedWriter ((BufferedRandomAccessFile)stream) :
					(IBinaryWriter)new SameEndianWriter (stream);
			else
				return new SwapEndianWriter (stream);			
		}

		
		// Implementation

		
		/// <summary>
		/// Determine what our architecture is
		/// </summary>
		private static Endian LocalEndian
		{
			get
			{
				Int32Union u = new Int32Union (1);
				if (u.b1 == 1)
					return Endian.Little;
				else
					return Endian.Big;
			}
		}
			
	}	
}


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
using System.Runtime.InteropServices;

namespace bridge.common.io
{
	/// <summary>
	/// Structure for converting int16
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Int16Union
	{
		public Int16Union (short v)
		{
			b1 = b2 = 0;
			i = unchecked((ushort)v);
		}

		public Int16Union (ushort v)
		{
			b1 = b2 = 0;
			i = v;
		}

		public Int16Union (byte[] buffer, int offset = 0)
		{
			i = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp = b1;
			b1 = b2;
			b2 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public short ToShort ()
		{
			return unchecked((short)i);
		}
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public ushort ToUShort ()
		{
			return i;
		}
		
		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public ushort 	i;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
	}

	
	/// <summary>
	/// Structure for converting int32
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Int32Union
	{
		public Int32Union (int v)
		{
			b1 = b2 = b3 = b4 = 0;
			i = unchecked((uint)v);
		}

		public Int32Union (uint v)
		{
			b1 = b2 = b3 = b4 = 0;
			i = v;
		}

		public Int32Union (byte[] buffer, int offset = 0)
		{
			i = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp = b1;
			b1 = b4;
			b4 = tmp;
			
			tmp = b2;
			b2 = b3;
			b3 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			buffer[2] = b3;
			buffer[3] = b4;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public int ToInt ()
		{
			return unchecked ((int)i);
		}

				
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public uint ToUInt ()
		{
			return i;
		}

		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public uint 	i;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
	}
	
	
	/// <summary>
	/// Structure for converting long
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Long64Union
	{
		public Long64Union (ulong v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			l = v;
		}

		public Long64Union (long v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			l = unchecked((ulong)v);
		}

		public Long64Union (byte[] buffer, int offset = 0)
		{
			l = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
			b5 = buffer[offset + 4];
			b6 = buffer[offset + 5];
			b7 = buffer[offset + 6];
			b8 = buffer[offset + 7];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp;
			
			tmp = b1; b1 = b8; b8 = tmp;			
			tmp = b2; b2 = b7; b7 = tmp;
			tmp = b3; b3 = b6; b6 = tmp;
			tmp = b4; b4 = b5; b5 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer, int offset)
		{
			buffer[offset + 0] = b1;
			buffer[offset + 1] = b2;
			buffer[offset + 2] = b3;
			buffer[offset + 3] = b4;
			buffer[offset + 4] = b5;
			buffer[offset + 5] = b6;
			buffer[offset + 6] = b7;
			buffer[offset + 7] = b8;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to ulong (in local form presumably)
		/// </summary>
		public ulong ToULong ()
		{
			return l;
		}

		/// <summary>
		/// Convert to long (in local form presumably)
		/// </summary>
		public long ToLong ()
		{
			return unchecked((long)l);
		}

		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public ulong 	l;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
		/// <summary>
		/// byte #5
		/// </summary>
		[FieldOffset(4)]
		public byte 	b5;
		/// <summary>
		/// byte #6
		/// </summary>
		[FieldOffset(5)]
		public byte 	b6;
		/// <summary>
		/// byte #7
		/// </summary>
		[FieldOffset(6)]
		public byte 	b7;
		/// <summary>
		/// byte #8
		/// </summary>
		[FieldOffset(7)]
		public byte 	b8;
	}

	
	/// <summary>
	/// Structure for converting doubles
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Float64Union
	{
		public Float64Union (double v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			d = v;
		}

		public Float64Union (byte[] buffer, int offset = 0)
		{
			d = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
			b5 = buffer[offset + 4];
			b6 = buffer[offset + 5];
			b7 = buffer[offset + 6];
			b8 = buffer[offset + 7];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp;
			
			tmp = b1; b1 = b8; b8 = tmp;			
			tmp = b2; b2 = b7; b7 = tmp;
			tmp = b3; b3 = b6; b6 = tmp;
			tmp = b4; b4 = b5; b5 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			buffer[2] = b3;
			buffer[3] = b4;
			buffer[4] = b5;
			buffer[5] = b6;
			buffer[6] = b7;
			buffer[7] = b8;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public double ToDouble ()
		{
			return d;
		}
		
		
		// Data
		
		/// <summary>
		/// float64 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public double 	d;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
		/// <summary>
		/// byte #5
		/// </summary>
		[FieldOffset(4)]
		public byte 	b5;
		/// <summary>
		/// byte #6
		/// </summary>
		[FieldOffset(5)]
		public byte 	b6;
		/// <summary>
		/// byte #7
		/// </summary>
		[FieldOffset(6)]
		public byte 	b7;
		/// <summary>
		/// byte #8
		/// </summary>
		[FieldOffset(7)]
		public byte 	b8;
	}

}


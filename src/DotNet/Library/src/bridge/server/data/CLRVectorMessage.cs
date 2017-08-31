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
using bridge.common.io;
using MathNet.Numerics.LinearAlgebra;
using bridge.math.matrix;
using System.Text;


namespace bridge.server.data
{
	/// <summary>
	/// CLR vector message.
	/// </summary>
	public class CLRVectorMessage : CLRMessage
	{
		public CLRVectorMessage ()
			: base (TypeVector)
		{
		}

		public CLRVectorMessage (Vector<double> vector)
			: base (TypeVector)
		{
			Value = vector;
		}


		// Properties

		public Vector<double> Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			var indices = MatrixUtils.IndicesOf (Value);
			if (indices != null)
			{
				var namelist = indices.NameList;
				cout.WriteInt32 (namelist.Length);
				for (int i = 0 ; i < namelist.Length ; i++)
					cout.WriteString (namelist[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);

			cout.WriteInt32 (Value.Count);
			for (int i = 0 ; i < Value.Count ; i++)
				cout.WriteDouble (Value[i]);
		}


		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			var ridxlen = cin.ReadInt32();
			IndexByName<string> rindex = null;

			if (ridxlen > 0)
			{
				rindex = new IndexByName<string> ();
				for (int i = 0 ; i < ridxlen ; i++)
					rindex.Add (cin.ReadString());
			}

			var count = cin.ReadInt32();
			Value = new IndexedVector (count, rindex);

			for (int i = 0 ; i < count ; i++)
				Value[i] = cin.ReadDouble();
		}
	}
}


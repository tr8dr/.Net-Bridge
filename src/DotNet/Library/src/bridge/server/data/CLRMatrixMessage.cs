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
	/// CLR matrix message.
	/// </summary>
	public class CLRMatrixMessage : CLRMessage
	{
		public CLRMatrixMessage ()
			: base (TypeMatrix)
		{
		}

		public CLRMatrixMessage (Matrix<double> matrix)
			: base (TypeMatrix)
		{
			Value = matrix;
		}


		// Properties

		public Matrix<double> Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			var rindices = MatrixUtils.RowIndicesOf (Value);
			var cindices = MatrixUtils.ColIndicesOf (Value);

			if (rindices != null)
			{
				var indices = rindices.NameList;
				cout.WriteInt32 (indices.Length);
				for (int i = 0 ; i < indices.Length ; i++)
					cout.WriteString (indices[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);
			
			if (cindices != null)
			{
				var indices = cindices.NameList;
				cout.WriteInt32 (indices.Length);
				for (int i = 0 ; i < indices.Length ; i++)
					cout.WriteString (indices[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);

			cout.WriteInt32 (Value.RowCount);
			cout.WriteInt32 (Value.ColumnCount);

			for (int ci = 0 ; ci < Value.ColumnCount ; ci++)
			{
				for (int ri = 0 ; ri < Value.RowCount ; ri++)
				{
					cout.WriteDouble (Value[ri,ci]);
				}
			}

		}


		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			IndexByName<string> rindex = null;
			IndexByName<string> cindex = null;

			var ridxlen = cin.ReadInt32();
			if (ridxlen > 0)
			{
				rindex = new IndexByName<string> ();
				for (int i = 0 ; i < ridxlen ; i++)
					rindex.Add (cin.ReadString());
			}
			
			var cidxlen = cin.ReadInt32();
			if (cidxlen > 0)
			{
				cindex = new IndexByName<string> ();
				for (int i = 0 ; i < cidxlen ; i++)
					cindex.Add (cin.ReadString());
			}

			var rows = cin.ReadInt32();
			var cols = cin.ReadInt32();
			Value = new IndexedMatrix (rows, cols, rindex, cindex);

			for (int ci = 0 ; ci < Value.ColumnCount ; ci++)
			{
				for (int ri = 0 ; ri < Value.RowCount ; ri++)
				{
					Value[ri,ci] = cin.ReadDouble();
				}
			}
		}
	}
}


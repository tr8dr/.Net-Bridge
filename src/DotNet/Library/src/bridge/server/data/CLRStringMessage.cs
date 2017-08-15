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
using bridge.common.io;
using System.Text;


namespace bridge.server.data
{
	/// <summary>
	/// CLR string message.
	/// </summary>
	public class CLRStringMessage : CLRMessage
	{
		public CLRStringMessage ()
			: base (TypeString)
		{
		}

		public CLRStringMessage (string value)
			: base (TypeString)
		{
			Value = value;
		}


		// Properties

		public string Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteString (Value, Encoding.ASCII);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadString();
		}

	}
}


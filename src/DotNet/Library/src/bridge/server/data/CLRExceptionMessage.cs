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
	/// CLR exception message.
	/// </summary>
	public class CLRExceptionMessage : CLRMessage
	{
		public CLRExceptionMessage ()
			: base (TypeException)
		{
		}

		public CLRExceptionMessage (object exception)
			: base (TypeException)
		{
			Message = exception.ToString();
		}


		// Properties

		public string Message
			{ get; private set; }


		// Serialization

		/// <summary>
		/// Provide as an exception
		/// </summary>
		/// <returns>The exception.</returns>
		public Exception ToException ()
		{
			return new Exception (Message);
		}

		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteString (Message, Encoding.ASCII);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Message = cin.ReadString();
		}

	}
}


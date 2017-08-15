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


namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR GetProperty message.
	/// </summary>
	public class CLRGetPropertyMessage : CLRMessage
	{
		public CLRGetPropertyMessage ()
			: base (TypeGetProperty)
		{
		}

		public CLRGetPropertyMessage (object obj, string property)
			: base (TypeGetProperty)
		{
			Obj = obj;
			PropertyName = property;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public string PropertyName
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteString (PropertyName);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			PropertyName = cin.ReadString();
		}

	}
}


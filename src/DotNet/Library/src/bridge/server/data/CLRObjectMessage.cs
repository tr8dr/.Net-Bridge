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


namespace bridge.server.data
{
	/// <summary>
	/// CLR object message.
	/// </summary>
	public class CLRObjectMessage : CLRMessage
	{
		public CLRObjectMessage ()
			: base (TypeObject)
		{
		}

		public CLRObjectMessage (CLRObjectProxy proxy)
			: base (TypeObject)
		{
			ObjectId = proxy.ObjectId;
			ClassName = proxy.ClassName;
		}
		
		public CLRObjectMessage (object obj)
			: base (TypeObject)
		{
			var proxy = CLRObjectProxy.ProxyFor (obj);
			ObjectId = proxy.ObjectId;
			ClassName = proxy.ClassName;
		}


		// Properties

		public int ObjectId
			{ get; private set; }

		public string ClassName
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (ObjectId);
			cout.WriteBool (true);
			cout.WriteString (ClassName);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			ObjectId = cin.ReadInt32();
			if (cin.ReadBoolean())
				ClassName = cin.ReadString ();
		}


		/// <summary>
		/// Converts to local object
		/// </summary>
		/// <returns>The object.</returns>
		public object ToObject ()
		{
			object obj = null;
			if (CLRObjectProxy.TryFindObject (ObjectId, out obj))
				return obj;
			else
				return new CLRObjectProxy (ObjectId);
		}

	}
}


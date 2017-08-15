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
using System.Linq;
using bridge.common.io;
using System.Reflection;


namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Call Method message.
	/// </summary>
	public class CLRTemplateReplyMessage : CLRMessage
	{
		public CLRTemplateReplyMessage ()
			: base (TypeTemplateReply)
		{
		}

		public CLRTemplateReplyMessage (Type type)
			: base (TypeTemplateReply)
		{
			var props = type.GetProperties (BindingFlags.Public | BindingFlags.Instance).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			var methods = type.GetMethods (BindingFlags.Public | BindingFlags.Instance).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			var classmethods = type.GetMethods (BindingFlags.Public | BindingFlags.Static).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			PropertyList = props.ToArray ();
			MethodList = methods.ToArray ();
			ClassMethodList = classmethods.ToArray ();
		}


		// Properties

		public string[] PropertyList
			{ get; set; }

		public string[] MethodList
			{ get; set; }

		public string[] ClassMethodList
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			cout.WriteInt32 (PropertyList.Length);
			foreach (var str in PropertyList)
				cout.WriteString(str);

			cout.WriteInt32 (MethodList.Length);
			foreach (var str in MethodList)
				cout.WriteString(str);

			cout.WriteInt32 (ClassMethodList.Length);
			foreach (var str in ClassMethodList)
				cout.WriteString(str);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			var plen = cin.ReadInt32();
			var plist = new string[plen];
			for (int i = 0; i < plen; i++)
				plist [i] = cin.ReadString ();

			var mlen = cin.ReadInt32();
			var mlist = new string[mlen];
			for (int i = 0; i < mlen; i++)
				mlist [i] = cin.ReadString ();

			var clen = cin.ReadInt32();
			var clist = new string[clen];
			for (int i = 0; i < clen; i++)
				clist [i] = cin.ReadString ();

			PropertyList = plist;
			MethodList = mlist;
			ClassMethodList = clist;
		}
	}
}


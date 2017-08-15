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


namespace bridge.common.parsing.json
{
	public enum JsonTokenType
		{ NONE, NULL, STRING, INT, FLOAT, BOOLEAN, WHITESPACE, KEY, ARRAY_START, ARRAY_END, OBJECT_START, OBJECT_END, SEPARATOR }


	/// <summary>
	/// Json token.
	/// </summary>
	public class JsonToken
	{

		public JsonToken (JsonTokenType type, object payload = null)
		{
			_type = type;
			_payload = payload;
		}


		// Properties


		public JsonTokenType Type
			{ get { return _type; } }

		public object Payload
			{ get { return _payload; }  }

		public string AsString
			{ get { return (string)this; } }

		public long AsLong
			{ get { return (long)this; } }

		public int AsInt
			{ get { return (int)this; } }

		public bool AsBool
			{ get { return (bool)this; } }

		public double AsDouble
			{ get { return (double)this; } }


		// Conversions


		public static implicit operator int (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return int.Parse((string)payload);
			if (payload is int)
				return (int)payload;
			if (payload is decimal)
				return (int)payload;
			else
				throw new Exception ("could not convert payload to int");
		}

		public static implicit operator long (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return long.Parse((string)payload);
			if (payload is int)
				return (long)payload;
			if (payload is long)
				return (long)payload;
			else
				throw new Exception ("could not convert payload to long");
		}

		public static implicit operator double (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return double.Parse((string)payload);
			if (payload is double)
				return (double)payload;
			if (payload is int)
				return (double)payload;
			if (payload is decimal)
				return (double)payload;
			else
				throw new Exception ("could not convert payload to double");
		}

		public static implicit operator bool (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return bool.Parse((string)payload);
			if (payload is bool)
				return (bool)payload;
			else
				throw new Exception ("could not convert payload to bool");
		}

		public static implicit operator string (JsonToken token)
		{
			object payload = token._payload;
			switch (token.Type)
			{
				case JsonTokenType.BOOLEAN:
					return (bool)payload == true ? "true" : "false";
				case JsonTokenType.NULL:
					return "null";
				default:
					return payload.ToString();
			}
		}


		// Meta

		public override string ToString ()
			{ return _type + ":" + _payload.ToString(); }




		// Variables

		private JsonTokenType 	_type;
		private object 			_payload;
	}
}


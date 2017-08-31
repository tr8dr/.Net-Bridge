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

using bridge.common.parsing;
using bridge.common.utils;


namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Date token.
	/// </summary>
	public struct DateToken
	{
		public enum TType
			{ NONE, ALPHA, NUMERIC, DASH, COLON, COMMA, SLASH, T, DOT, Z, WHITESPACE }

		public DateToken (TType type, object payload = null)
		{
			_type = type;
			_payload = payload;
		}
	
		
		// Properties
		
		
		public TType Type
			{ get { return _type; } }
	
		public object Payload
			{ get { return _payload; }  }
	

		// Conversions
		
		
		public static implicit operator int (DateToken token)
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
		
		public static implicit operator long (DateToken token)
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
		
		public static implicit operator double (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return double.Parse((string)payload);
			if (payload is double)
				return (double)payload;
			if (payload is decimal)
				return (double)payload;
			else
				throw new Exception ("could not convert payload to double");
		}
		
		public static implicit operator bool (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return bool.Parse((string)payload);
			if (payload is bool)
				return (bool)payload;
			else
				throw new Exception ("could not convert payload to bool");
		}
		
		public static implicit operator string (DateToken token)
		{
			object payload = token._payload;
			return payload.ToString();
		}
		
		
		// Meta
		
		public override string ToString ()
			{ return _type + ":" + _payload.ToString(); }
		
		
		// Variables
		
		private TType		_type;
		private object		_payload;
	}
}


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
using System.Text;
using System.Collections;
using bridge.common.time;


namespace bridge.common.utils
{	
	/// <summary>
	/// Class provides mapping between string form and many of the primitive types
	/// </summary>
	public struct Any
	{
		public Any (string v)
		{
			_sval = v; 
		}
		
		
		// Conversions
		
		public static implicit operator string(Any v)
			{ return v._sval; }
		
		public static implicit operator char(Any v)
			{ return v._sval[0]; }
		
		public static implicit operator int(Any v)
			{ return int.Parse(v._sval); }
		
		public static implicit operator double(Any v)
			{ return double.Parse(v._sval); }
		
		public static implicit operator bool(Any v)
			{ return bool.Parse(v._sval); }
		
		public static implicit operator long(Any v)
			{ return long.Parse(v._sval); }
		
		public static implicit operator ZDateTime(Any v)
			{ return new ZDateTime(v._sval, ZTimeZone.Local); }
		
		
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public int Or (int def)
		{
			string v = _sval;
			if (v != null)
				return int.Parse(v);
			else
				return def;
		}
		
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public long Or (long def)
		{
			string v = _sval;
			if (v != null)
				return long.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public double Or (double def)
		{
			string v = _sval;
			if (v != null)
				return double.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public bool Or (bool def)
		{
			string v = _sval;
			if (v != null)
				return bool.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public string Or (string def)
		{
			string v = _sval;
			if (v != null)
				return v;
			else
				return def;
		}

		// Predicates
		
		public bool IsNull
			{ get { return _sval == null; } }
		
		
		// Meta
		
		
		public override string ToString ()
		{
			return _sval;
		}
		
		
		// Variables
		
		private string		_sval;
	}
}

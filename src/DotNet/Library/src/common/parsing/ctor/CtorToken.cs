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


namespace bridge.common.parsing.ctor
{
	/// <summary>
	/// Ctor token.
	/// </summary>
	public class CtorToken : Token<CtorToken.TType>
	{
		public enum TType
			{ NONE, IDENTIFIER, STRING, NUMERIC, SEPARATOR, ARRAY_OPEN, ARRAY_CLOSE, OPEN_PAREN, CLOSE_PAREN }
		
		public CtorToken (TType type, object info = null)
			: base (type, info) {}			
	}
}


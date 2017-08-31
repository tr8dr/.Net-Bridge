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
using System.Collections.Generic;

namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Date lexer
	/// </summary>
	public class DateLexer
	{		
		// Functions
		
		
		/// <summary>
		/// Create token stream of dates
		/// </summary>
		/// <param name='str'>
		/// date as string.
		/// </param>
		public List<DateToken> Parse (string str)
		{
			_tokens.Clear();
			Lex (str);
			return _tokens;
		}
		
		
		// Implementation
		
		
		private void Lex (string str)
		{
			var len = str.Length;
			int wordstart = 0;
			
			var type = DateToken.TType.NONE;
			
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				switch (c)
				{
					case '-':
						Close (type, str.Substring (wordstart, i - wordstart));
						Close (DateToken.TType.DASH, "-");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
					case ',':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.COMMA, ",");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
					case '.':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.DOT, "-");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
						
					case ':':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.COLON, ":");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
						
					case '/':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.SLASH, "/");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
	
					case '\n':
					case '\r':
					case '\t':
					case ' ':
						if (type != DateToken.TType.WHITESPACE)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.WHITESPACE;
							wordstart = i;
						}
						break;
	
					
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						if (type != DateToken.TType.NUMERIC)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.NUMERIC;
							wordstart = i;
						}
						break;
						
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
						if (type != DateToken.TType.ALPHA)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.ALPHA;
							wordstart = i;
						}
						break;
				}
			}
			
			Close (type, str.Substring (wordstart));
		}
		
		
		
		private void Close (DateToken.TType type, string v)
		{
			if (type == DateToken.TType.NONE)
				return;
	
			if (v == "T")
				_tokens.Add (new DateToken (DateToken.TType.T, v));
			else if (v == "Z")
				_tokens.Add (new DateToken (DateToken.TType.Z, v));
			else
				_tokens.Add (new DateToken (type, v));
		}
		
		
		// Variables
		
		private List<DateToken>		_tokens = new List<DateToken>();
	}
}


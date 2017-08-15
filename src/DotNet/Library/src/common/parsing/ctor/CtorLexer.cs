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
using System.Collections.Generic;

using bridge.common.parsing;
using bridge.common.utils;


namespace bridge.common.parsing.ctor
{
	/// <summary>
	/// Ctor lexer
	/// </summary>
	public class CtorLexer
	{
		public CtorLexer (string expr)
		{
			Lex (expr);
		}
		
		
		// Properties
		
		
		public IList<CtorToken> Tokens
			{ get { return _tokens; } }
		
		
		// Implementation
		
		
		private void Lex (string str)
		{
			int len = str.Length;
			
			int wordstart = 0;
			
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				switch (c)
				{
					case '\'':
						wordstart = ++i;
						while (i < len && str[i] != '\'') i++;
						Add (CtorToken.TType.STRING, str.Substring(wordstart, i - wordstart));
						break;
	
					case '"':
						wordstart = ++i;
						while (i < len && str[i] != '"') i++;
						Add (CtorToken.TType.STRING, str.Substring(wordstart, i - wordstart));
						break;
	
					case ',':
						Add (CtorToken.TType.SEPARATOR, ",");
						break;
	
					case '(':
						Add (CtorToken.TType.OPEN_PAREN, "(");
						break;
	
					case ')':
						Add (CtorToken.TType.CLOSE_PAREN, ")");
						break;
	
					case '.':
					case '-':
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
						wordstart = i;
						while (i < len && StringUtils.IsNumeric(str[i])) i++; 
						
						string snum = str.Substring(wordstart, i - wordstart);
						if (snum.IndexOf('.') >= 0 || snum.IndexOf('e') >= 0)
							Add (CtorToken.TType.NUMERIC, double.Parse(snum));
						else
							Add (CtorToken.TType.NUMERIC, int.Parse(snum));
						
						i--;
						break;
						
					case '[':
						Add (CtorToken.TType.ARRAY_OPEN, "[");
						break;
						
					case ']':
						Add (CtorToken.TType.ARRAY_CLOSE, "]");
						break;
	
	
					case '\n':
					case '\r':
					case '\t':
					case ' ':
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
						wordstart = i;
						while (i < len && (c = str[i]) > ' ' && (char.IsDigit(c) || char.IsLetterOrDigit(c) || c == '.' || c == '+' || c =='$')) i++;
						Add (CtorToken.TType.IDENTIFIER, str.Substring(wordstart, i - wordstart));
						i--;
						break;
						
					default:
						throw new Exception ("failed to parse constructor: " + str + " at " + i);
				}
			}
		}
		
		
		
		private void Add (CtorToken.TType type, object value = null)
		{
			_tokens.Add (new CtorToken (type, value));
		}
		
		
		// Variables
		
		private IList<CtorToken>		_tokens = new List<CtorToken>();
	}
}


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
using System.Text;


namespace bridge.common.parsing.json
{
	/// <summary>
	/// Json lexer.
	/// </summary>
	public class JsonLexer
	{
		/// <summary>
		/// Create token stream from JSON
		/// </summary>
		/// <param name='str'>
		/// date as string.
		/// </param>
		public IEnumerable<JsonToken> Parse (string str)
		{
			var len = str.Length;

			Tuple<JsonToken,int> current;

			var i = 0;
			while (i < len)
			{
				char c = str [i];
				switch (c)
				{
					case '"':
						current = ProcessQuote (str, i);
						i = current.Item2;
						yield return current.Item1;
						break;

					case 't':
					case 'f':
					case 'T':
					case 'F':
						current = ProcessBool (str, i);
						i = current.Item2;
						yield return current.Item1;
						break;

					case 'n':
						current = ProcessNull (str, i);
						i = current.Item2;
						yield return current.Item1;
						break;

					case '-':
					case '.':
					case 'e':
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
						current = ProcessNumeric (str, i);
						i = current.Item2;
						yield return current.Item1;
						break;

					case '{':
						i++;
						yield return new JsonToken (JsonTokenType.OBJECT_START, "{");
						break;

					case '}':
						i++;
						yield return new JsonToken (JsonTokenType.OBJECT_END, "}");
						break;

					case '[':
						i++;
						yield return new JsonToken (JsonTokenType.ARRAY_START, "[");
						break;

					case ']':
						i++;
						yield return new JsonToken (JsonTokenType.ARRAY_END, "]");
						break;

					case '\n':
						i++;
						yield return new JsonToken (JsonTokenType.WHITESPACE, "\n");
						break;

					case ' ':
						yield return new JsonToken (JsonTokenType.WHITESPACE, " ");
						i++;
						break;

					case '\t':
						yield return new JsonToken (JsonTokenType.WHITESPACE, "\t");
						i++;

						break;

					case ',':
						yield return new JsonToken (JsonTokenType.SEPARATOR, ",");
						i++;
						break;

					case '\r':
						yield return new JsonToken (JsonTokenType.WHITESPACE, "\r");
						i++;
						break;

					default:
						var context = str.Substring (Math.Max (0, i - 64), i - Math.Max (0, i - 64) + 1);
						throw new ArgumentException ("json: unrecognized character: " + c + ", context: " + context);
				}
			}
		}


		#region Implementation


		private Tuple<JsonToken,int> ProcessQuote (string str, int pos)
		{
			var escaped = false;
			var len = str.Length;

			var buffer = new StringBuilder ();
			var epos = ++pos;

			while (epos < len)
			{
				var c = str [epos];
				switch (c)
				{
					case '\\':
						if (escaped)
						{
							buffer.Append ('\\');
							escaped = false;
						} 
						else
						{
							buffer.Append ('\\');
							escaped = true;
						}
						epos++;
						break;

					case '"':
						if (escaped)
						{
							buffer.Append ('\"');
							epos++;
							escaped = false;
						}
						else
						{
							epos++;
							while (epos < len && str [epos] <= ' ')
								epos++;
							if (epos < len && str [epos] == ':')
							{
								var tok = new JsonToken (JsonTokenType.KEY, buffer.ToString ());
								return Tuple.Create (tok, epos + 1);
							}
							else
							{
								var tok = new JsonToken (JsonTokenType.STRING, buffer.ToString ());
								return Tuple.Create (tok, epos);
							}
						}
						break;

					default:
						buffer.Append (c);
						epos++;
						escaped = false;
						break;
				}
			}

			throw new ArgumentException ("could not parse json, mising string terminator: " + str.Substring(pos,len-pos));
		}
			

		private Tuple<JsonToken,int> ProcessBool (string str, int pos)
		{
			var len = str.Length;

			var epos = pos;
			while (epos < len)
			{
				var c = str [epos];
				switch (c)
				{
					case 'f':
					case 'a':
					case 'l':
					case 's':
					case 'e':
					case 't':
					case 'r':
					case 'u':
						epos++;
						break;

					case 'F':
					case 'A':
					case 'L':
					case 'S':
					case 'E':
					case 'T':
					case 'R':
					case 'U':
						epos++;
						break;

					case ' ':
					case ',':
					case ']':
					case '}':
					case '\n':
					case '\r':
						var v = Boolean.Parse (str.Substring (pos, epos - pos).ToLower());
						var tok = new JsonToken (JsonTokenType.BOOLEAN, v);
						return Tuple.Create (tok, epos);

					default:
						throw new ArgumentException ("json: bool with improper termination: " + str.Substring (pos, epos - pos+1) + ", char: " + c);
				}
			}

			throw new ArgumentException ("json: could not parse boolean, not complete: " + str.Substring (pos, epos - pos));
		}


		private Tuple<JsonToken,int> ProcessNull (string str, int pos)
		{
			var len = str.Length;

			var epos = pos;
			while (epos < len)
			{
				var c = str [epos];
				switch (c)
				{
					case 'n':
					case 'u':
					case 'l':
						epos++;
						break;

					case ' ':
					case ',':
					case ']':
					case '}':
					case '\n':
					case '\r':
						var v = str.Substring (pos, epos - pos);
						var tok = new JsonToken (JsonTokenType.NULL, v);

						if (v == "null")
							return Tuple.Create (tok, epos);
						else
							throw new ArgumentException ("json: unknown token: " + v);

					default:
						throw new ArgumentException ("json: null with improper termination: " + str.Substring (pos, epos - pos+1) + ", char: " + c);
				}
			}

			throw new ArgumentException ("json: could not parse boolean, not complete: " + str.Substring (pos, epos - pos));
		}
			

		private Tuple<JsonToken,int> ProcessNumeric (string str, int pos)
		{
			var len = str.Length;
			var isfloat = false;

			var epos = pos;
			while (epos < len)
			{
				var c = str [epos];
				switch (c)
				{
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
						epos++;
						break;

					case '.':
					case 'e':
						isfloat = true;
						epos++;
						break;

					case ' ':
					case ',':
					case ']':
					case '}':
					case '\n':
					case '\r':
						if (isfloat)
						{
							var v = Double.Parse (str.Substring (pos, epos - pos));
							var tok = new JsonToken (JsonTokenType.FLOAT, v);
							return Tuple.Create (tok, epos);
						}
						else
						{
							var v = Int32.Parse (str.Substring (pos, epos - pos));
							var tok = new JsonToken (JsonTokenType.INT, v);
							return Tuple.Create (tok, epos);
						}

					default:
						throw new ArgumentException ("json: number with improper termination: " + str.Substring (pos, epos - pos+1) + ", char: " + c);

				}
			}

			throw new ArgumentException ("json: expression terminated early: " + str.Substring (pos, epos - pos));
		}


		#endregion
	}
}


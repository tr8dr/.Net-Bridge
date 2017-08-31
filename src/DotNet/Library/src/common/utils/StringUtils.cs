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
using System.Collections.Generic;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;


namespace bridge.common.utils
{	
	/// <summary>
	/// Collection of string manipulation utilities
	/// </summary>
	public class StringUtils
	{	
		/// <summary>
		/// Determine whether string starts with given substring from offset
		/// </summary>
		/// <param name="fullstr">
		/// The string to inspect
		/// </param>
		/// <param name="offset">
		/// Starting offset into the string
		/// </param>
		/// <param name="sub">
		/// Substring to find within the main string (fullstr)
		/// </param>
		/// <returns>
		/// true if matches, false otherwise
		/// </returns>
		public static bool StartsWith (string fullstr, int offset, string sub)
		{
			int len = sub.Length;
			if ((fullstr.Length - len - offset) < len)
				return false;
			
			for (int i = 0 ; i < len ; i++)
				if (fullstr[i + offset] != sub[i]) return false;
			
			return true;
		}
		
		
		/// <summary>
		/// 	returns the first or second argument depending on which of the arguments is non-null (or empty)
		/// </summary>
		/// <param name="a">
		/// 	first string
		/// </param>
		/// <param name="b">
		/// 	second string
		/// </param>
		/// <returns>
		/// 	first of two strings which is non-null or empty
		/// </returns>
		public static string Or (string a, string b)
		{
			if (a == null)
				return b;
			if (a.Length == 0)
				return b;
			else
				return a;
		}
		
		/// <summary>
		/// 	returns the first or second argument depending on which of the arguments is non-null (or empty)
		/// </summary>
		/// <param name="a">
		/// 	first string
		/// </param>
		/// <param name="b">
		/// 	second string
		/// </param>
		/// <returns>
		/// 	first of two strings which is non-null or empty
		/// </returns>
		public static string Or (params string[] v)
		{
			for (int i = 0 ; i < v.Length ; i++)
			{
				if (v[i] != null && v[i].Length > 0)
					return v[i];
			}
			
			return null;
		}


		/// <summary>
		/// 	returns the first or second argument (translated to int value) depending on which of the
		/// 	arguments is non-null (or empty)
		/// </summary>
		/// <param name="a">
		/// 	first integer as string
		/// </param>
		/// <param name="b">
		/// 	default integer value
		/// </param>
		/// <returns>
		/// 	first of two value which is non-null or empty as integer
		/// </returns>
		public static int Or (string a, int b)
		{
			if (a == null)
				return b;
			if (a.Length == 0)
				return b;
			else
				return int.Parse(a);
		}

		

		/// <summary>
		/// 	returns the first or second argument (translated to double value) depending on which of the
		/// 	arguments is non-null (or empty)
		/// </summary>
		/// <param name="a">
		/// 	first double as string
		/// </param>
		/// <param name="b">
		/// 	default double value
		/// </param>
		/// <returns>
		/// 	first of two value which is non-null or empty as double
		/// </returns>
		public static double Or (string a, double b)
		{
			if (a == null)
				return b;
			if (a.Length == 0)
				return b;
			else
				return double.Parse(a);
		}

		
		/// <summary>
		/// Pretty prints a variety of values
		/// </summary>
		/// <returns>The print.</returns>
		/// <param name="result">Result.</param>
		public static string PrettyPrint (object result)
		{
			if (result == null)
			{
				return "null";
			}
			
			else if (result is Array)
			{
				var output = new StringBuilder();
				Array a = (Array) result;
				
				output.Append ("{ ");
				int top = a.GetUpperBound (0);
				for (int i = a.GetLowerBound (0); i <= top; i++)
				{
					output.Append (PrettyPrint (a.GetValue (i)));
					if (i != top)
						output.Append (", ");
				}
				output.Append (" }");
				return output.ToString();
			} 

			else if (result is Vector<double>)
			{
				var output = new StringBuilder();
				Vector<double> v = (Vector<double>) result;

				output.Append ("[");
				for (int i = 0; i < v.Count; i++)
				{
					if (i > 0)
						output.Append (", ");

					output.Append (v[i]);
				}
				output.Append ("]");
				return output.ToString();
			} 

			else if (result is bool)
			{
				if ((bool) result)
					return "true";
				else
					return "false";
			} 
			
			else if (result is string)
			{
				return String.Format ("\"{0}\"", EscapeString ((string)result));
			} 
			
			else if (result is IDictionary)
			{
				var o = new StringBuilder();
				IDictionary dict = (IDictionary) result;
				int top = dict.Count, count = 0;
				
				o.Append ("{");
				foreach (DictionaryEntry entry in dict)
				{
					count++;
					o.Append ("{ ");
					o.Append (PrettyPrint (entry.Key));
					o.Append (", ");
					o.Append (PrettyPrint (entry.Value));
					if (count != top)
						o.Append (" }, ");
					else
						o.Append (" }");
				}
				o.Append ("}");
				return o.ToString();
			} 
			
			else if (WorksAsEnumerable (result)) 
			{
				var o = new StringBuilder();
				int i = 0;
				o.Append ("{ ");
				foreach (object item in (IEnumerable) result) 
				{
					if (i++ != 0)
						o.Append (", ");
					
					o.Append (PrettyPrint (item));
				}
				o.Append (" }");
				return o.ToString ();
			} 
			
			else if (result is char) 
			{
				return EscapeChar ((char) result);
			} 
			
			else 
			{
				return result.ToString ();
			}
		}
		


		/// <summary>
		/// 	Trim whitespace from either side of a string (but handle nulls)
		/// </summary>
		/// <param name="s">
		/// 	string to trim
		/// </param>
		/// <returns>
		/// 	string minus whitespace
		/// </returns>
		public static string Trim (string s)
		{
			return s != null ? s.Trim() : "";
		}


		/// <summary>
		/// 	Adds quotes around string if none already exist (quote char is ")
		/// </summary>
		/// <param name="v">
		/// 	string to quote
		/// </param>
		/// <returns>
		/// 	quoted string
		/// </returns>
		public static string Quote (string v)
		{
			if (v[0] == '"')
				return v;
			else
				return "\"" + v + "\"";
		}


		/// <summary>
		/// 	Adds quotes around string if none already exist (quote char is ')
		/// </summary>
		/// <param name="v">
		/// 	string to quote
		/// </param>
		/// <returns>
		/// 	quoted string
		/// </returns>
		public static string QuoteSingle (string v)
		{
			if (v[0] == '\'')
				return v;
			else
				return "\'" + v + "\'";
		}

	
		/// <summary>
		/// 	remove quotes from string if any (quote char is ")
		/// </summary>
		/// <param name="v">
		/// 	string to unquote
		/// </param>
		/// <returns>
		/// 	unquoted string
		/// </returns>
		public static string Unquote (string v)
		{
			if (v == null || v.Length == 0 || v[0] != '"')
				return v;
			else
				return v.Substring (1, v.Length-2);
		}


		/// <summary>
		/// 	returns the ith field of length "cnt" fields, delimited by specified delimiter
		/// 	<p>
		/// 	Ex. field ("USD.CM_DEPTH.stgLHBONDS.US5YTA=D4", 1, '.', 1) returns "CM_DEPTH"
		/// </summary>
		/// <param name="s">
		/// 	string to parse
		/// </param>
		/// <param name="idx">
		/// 	nth field to start with
		/// </param>
		/// <param name="delim">
		/// 	delimiter between fields
		/// </param>
		/// <param name="cnt">
		/// 	number of fields to take from index
		/// </param>
		/// <returns>
		/// 	field or fields
		/// </returns>
		public static string Field (string s, int idx, char delim, int cnt = 1)
		{
			int si = 0;
			int ei = 0;
			int ln = cnt > 0 ? s.Length : 0;
	
			for (ei= 0; ei < ln; ++ei)
			{
				if (s[ei] != delim)
					continue;
				
				if (idx-- > 0) 
					si = ei+1;
				else if (-idx == cnt) 
					return s.Substring (si, ei-si);
			}
	
			return (idx <= 0) ? s.Substring (si, ei-si) : "";
		}

		
		/// <summary>
		/// 	returns the ith field of length "cnt" fields from end of string (in reverse), delimited by specified delimiter
		/// </summary>
		/// <param name="s">
		/// 	string to parse
		/// </param>
		/// <param name="idx">
		/// 	nth field to start with
		/// </param>
		/// <param name="delim">
		/// 	delimiter between fields
		/// </param>
		/// <param name="cnt">
		/// 	number of fields to take from index
		/// </param>
		/// <returns>
		/// 	field or fields
		/// </returns>
		public static string RField (string s, int idx, char delim, int cnt = 1)
		{
			int si = 0;
			int ei = s.Length;
			int len = s.Length;
	
			for (si = len-1; si >= 0; si--)
			{
				if (s[si] != delim)
					continue;
					
				if (idx-- > 0) 
					ei = si;
				else if (-idx == cnt) 
					return s.Substring (si+1, ei-(si+1));
			}
	
			return (idx <= 0) ? s.Substring(0, ei) : "";
		}


		/// <summary>
		/// return string from start to ith field to the end of the string (trimming left part)
		/// </summary>
		/// <param name='s'>
		/// 	string to parse
		/// </param>
		/// <param name='idx'>
		/// 	index of field
		/// </param>
		/// <param name='delim'>
		/// 	delimiter string
		/// </param>
		public static string LTrimField (string s, int idx, string delim)
		{
			int len = s.Length;
			int si = 0;
			int ei = s.IndexOf (delim);
	
			// mark the begining of the section or sections
			for (; idx-- != 0 && ei < len && ei != -1; )
				ei = s.IndexOf(delim, (si = ei+delim.Length));
	
			return s.Substring (si);
		}
		
		
		/// <summary>
		/// 	return string from start to ith field to the end of the string (trimming left part)
		/// </summary>
		/// <param name='s'>
		/// 	string to parse
		/// </param>
		/// <param name='idx'>
		/// 	index of field
		/// </param>
		/// <param name='delim'>
		/// 	delimiter char
		/// </param>
		public static string LTrimField (string s, int idx, char delim)
		{
			int len = s.Length;
			int si = 0;
			int ei = s.IndexOf (delim);
	
			// mark the begining of the section or sections
			for (; idx-- != 0 && ei < len && ei != -1;)
				ei = s.IndexOf(delim, (si = ei+1));
	
			return s.Substring(si);
		}


		/// <summary>
		/// 	return string from start to ith field from the end of the string (trimming right part)
		/// 	<p>
		/// 	Ex. rtrimfield ("TBOND:USD:BOND:5Y:A:BTEC", 1,':') returns "TBOND:USD:BOND:5Y:A"	
		/// </summary>
		/// <param name='s'>
		/// 	string to parse
		/// </param>
		/// <param name='idx'>
		/// 	index
		/// </param>
		/// <param name='delim'>
		/// 	delimiter
		/// </param>
		public static string RTrimField (string s, int idx, char delim)
		{
			int len = s.Length;
			int ei = len;
					
			// mark the begining of the section or sections
			for (int i = 0 ; ei >= 0 && i < idx ; i++)
				ei = s.LastIndexOf (delim, ei-1);
	
			if (ei >= 0)
				return s.Substring(0, ei);
			else
				return "";
		}

	
		/// <summary>
		/// return string from start to ith field from the end of the string (trimming right part)
		/// </summary>
		public static string RTrimField (string s, int idx, string delim)
		{
			int ei = s.LastIndexOf(delim);
	
			// mark the begining of the section or sections
			for (int i = 0 ; ei >= 0 && i < idx ; i++)
				ei = s.LastIndexOf (delim, ei);
	
			if (ei >= 0)
				return s.Substring(0, ei);
			else
				return "";
		}


		/// <summary>
		/// 	Is the string the numeric.
		/// </summary>
		/// <returns>
		/// 	true if numeric
		/// </returns>
		/// <param name='v'>
		/// 	string to test
		/// </param>
		public static bool IsNumeric (string v)
		{
			if (v == null)
				return false;
			
			int i = v.Length;
			
			while (i > 0)
			{
				char c = v[--i];
				switch (c)
				{
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
					case '-':
					case '+':
					case 'e':
					case '.':
						break;
					default:
						return false;
				}
			}
	
			return (v.Length > 0);
		}
	
	
		/// <summary>
		/// 	is string liekly regex?
		/// </summary>
		public static bool IsRegex (string v)
		{
			for (int i = 0 ; i < v.Length; i++)
			{
				char c = v[i];
				if (c == '.' || c == '-' || c == '+' || c == '[' || c == '*' || c == '?')
					return true;
			}
	
			return false;
		}
	
		/// <summary>
		/// Determines whether string is likely to be numeric
		/// </summary>
		public static bool IsNumeric (char v)
		{
			switch (v)
			{
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
				case '-':
				case '+':
				case 'e':
				case '.':
					return true;
				default:
					return false;
			}
		}


		/// <summary>
		/// Determines whether string looks to be a hexidecimal sequence
		/// </summary>
		public static bool IsHexDigit (char v)
		{
			return char.IsDigit(v) || (v >= 'a' && v <= 'f') || (v >= 'A' && v <= 'F');
		}

	
		/// <summary>
		/// Determines whether string is empty or filled with whitespace
		/// </summary>
		public static bool IsBlank (string s)
		{
			if (s == null)
				return true;
	
			int len = s.Length;
			if (len == 0)
				return true;
	
			bool empty = true;
			for (int i = 0 ; empty && i < len ; i++)
				empty = char.IsWhiteSpace(s[i]);
				
			return empty;
		}
	

		/// <summary>
		/// 	Provides the numeric part of a string, skipping white space and punctuation.
		/// </summary>
		/// <returns>
		/// 	numeric portion of string
		/// </returns>
		/// <param name='v'>
		/// 	string
		/// </param>
		public static string NumericPart (string v)
		{
			var Istart = 0;
			while (Istart < v.Length && !IsNumeric (v[Istart])) Istart++;

			var Iend = Istart+1;
			while (Iend < v.Length && IsNumeric (v[Iend])) Iend++;

			if (Istart == v.Length)
				throw new ArgumentException ("NumericPart: given string did not contain a #");
			else
				return v.Substring(Istart, Iend-Istart);
		}


		/// <summary>
		/// capitalize words in string (if delimited).  We decide that any non-alphabetic character
		/// acts as a delimiter.  Therefore, the next alphabetic character after a delimiter will
		/// be capitalized, the remainder in lower-case.
		/// </summary>
		public static string Capitalize (string s)
		{
			StringBuilder build = new StringBuilder(s.Length);
				
			bool newword = true;
			for (int i = 0 ; i < s.Length ; i++)
			{
				char c = s[i];
				if (c <= '@')
					{ newword = true; build.Append(c); }
				else if (newword)
					{ newword = false; build.Append (char.ToUpper(c)); }
				else
					build.Append (char.ToLower(c));
			}
			return build.ToString();
		}

		
		/// <summary>
		/// 	replaces all occurrences of "from" substring to "to" substring in given string
		/// </summary>
		/// <param name='str'>
		/// 	string in which to execute replacements
		/// </param>
		/// <param name='from'>
		/// 	sub-string to replace
		/// </param>
		/// <param name='to'>
		/// 	sub-string to map to
		/// </param>
		public static string Replace (string str, string from, string to)
		{
			var len = str.Length;
			var flen = from.Length;
			
			int next = -1;
			int pos = 0;
			
			StringBuilder nstr = new StringBuilder (len);
			
			while ((next = str.IndexOf (from, pos)) >= 0)
			{
				// copy region up to match
				for (int i = pos ; i < next ; i++)
					nstr.Append(str[i]);
					
				// copy in replacement
				nstr.Append (to);
				
				// position for next search
				pos = next + flen;
			}
			
			// copy remainder of string
			for (int i = pos ; i < len ; i++)
				nstr.Append(str[i]);
	
			return nstr.ToString();
		}
	
	
	
		/// <summary>
		/// 	replaces all occurrences of "from" char to "to" char in given string
		/// </summary>
		/// <param name='str'>
		/// 	string in which to execute replacements
		/// </param>
		/// <param name='from'>
		/// 	char to replace
		/// </param>
		/// <param name='to'>
		/// 	char to map to
		/// </param>
		public static string Replace (string str, char from, char to)
		{
			var len = str.Length;
			var flen = 1;
			
			int next = -1;
			int pos = 0;
			
			StringBuilder nstr = new StringBuilder (len);
			
			while ((next = str.IndexOf (from, pos)) > 0)
			{
				// copy region up to match
				for (int i = pos ; i < next ; i++)
					nstr.Append(str[i]);
					
				// copy in replacement
				nstr.Append (to);
				
				// position for next search
				pos = next + flen;
			}
		
			
			// copy remainder of string
			for (int i = pos ; i < len ; i++)
				nstr.Append(str[i]);
	
			return nstr.ToString();
		}

	
		/// <summary>
		/// 	inserts tabs, indenting the given, possibly multiline string
		/// </summary>
		/// <param name='str'>
		/// 	string to indent
		/// </param>
		/// <param name='nlevels'>
		/// 	number of levels to indent
		/// </param>
		public static string Indent (string str, int nlevels = 1)
		{
			var len = str.Length;
			
			StringBuilder nstr = new StringBuilder (len + nlevels * 10);
			
			// initial indent
			for (int i = 0 ; i < nlevels ; i++)
				nstr.Append ('\t');
			
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				nstr.Append (c);

				// indent after new line
				if (c == '\n' && (i+1) < len)
				{
					for (int j = 0 ; j < nlevels ; j++)
						nstr.Append ('\t');					
				}
			}
	
			return nstr.ToString();
		}


		/// <summary>
		/// 	removes K tabs from indented text
		/// </summary>
		/// <param name='str'>
		/// 	string to un-indent
		/// </param>
		/// <param name='nlevels'>
		/// 	number of levels to indent
		/// </param>
		public static string Undent (string str, int nlevels = 1)
		{
			var len = str.Length;

			StringBuilder nstr = new StringBuilder (len);

			var icount = 0;
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				if (c == '\t' && icount++ < nlevels)
					continue;

				nstr.Append (c);

				// reset indent count on new line
				if (c == '\n')
					icount = 0;
			}

			return nstr.ToString();
		}


		/// <summary>
		/// 	removes tabs up to some minimum observed tab level
		/// </summary>
		/// <param name='str'>
		/// 	string to un-indent
		/// </param>
		public static string AutoUndent (string str)
		{
			return Undent (str, IndentationLevel (str));
		}


		/// <summary>
		/// 	determines common indentation level
		/// </summary>
		/// <param name='str'>
		/// 	string to un-indent
		/// </param>
		public static int IndentationLevel (string str)
		{
			var len = str.Length;

			var minlevels = int.MaxValue;

			var tabcount = 0;
			var codecount = 0;
			for (int i = 0; i < len; i++)
			{
				switch (str [i])
				{
					case '\r':
					case '\n':
						if (codecount > 0)
							minlevels = Math.Min (minlevels, tabcount);

						codecount = 0;
						tabcount = 0;
						break;

					case ' ':
						break;

					case '\t':
						if (codecount == 0)
							tabcount++;
						break;

					default:
						codecount++;
						break;
				}
			}

			if (codecount > 0)
				minlevels = Math.Min (minlevels, tabcount);

			return minlevels;
		}

		
	
		/// <summary>
		/// 	Split a string at delimiter boundaries, possibly skipping empty fields if indicated
		/// </summary>
		/// <param name='s'>
		/// 	String to split
		/// </param>
		/// <param name='delimiter'>
		/// 	Delimiter
		/// </param>
		/// <param name='skipempty'>
		/// 	Indicate whether should skip empty delimited fields
		/// </param>
		public static List<string> Split (string s, char delimiter, bool skipempty = true, bool trim = false)
		{
			var elements = new List<string>();
	
			if (s == null)
				return elements;
	
			int len = s.Length;
			int start = 0;
			int end = 0;
			string section;
			
			while (start < len)
			{
				end = s.IndexOf (delimiter, start);
				if (end == -1)
					section = s.Substring (start);
				else
					section = s.Substring (start, end-start);
				
				start = end >= 0 ? end+1 : len;
				
				if (skipempty && IsBlank (section))
					continue;

				if (trim)
					section = section.Trim ();
				
				elements.Add (section);
			}
			
			return elements;
		}
	
	
		/// <summary>
		/// 	Join elements as string with separator
		/// </summary>
		/// <param name='elements'>
		/// 	Element list or collection
		/// </param>
		/// <param name='separator'>
		/// 	Separator string
		/// </param>
		public static string Join<T> (IEnumerable<T> elements, string separator)
		{
			StringBuilder buf = new StringBuilder();
			
			int i = 0;
			foreach (T obj in elements)
			{
				if (i > 0) buf.Append(separator);
				buf.Append(obj.ToString());
				i++;
			}
			
			return buf.ToString();
		}


		/// <summary>
		/// 	Join elements as string with separator
		/// </summary>
		/// <param name='elements'>
		/// 	Element list or collection
		/// </param>
		/// <param name='separator'>
		/// 	Separator string
		/// </param>
		public static string Join<T> (List<T> elements, int Istart, int Iend, string separator)
		{
			StringBuilder buf = new StringBuilder();

			for (int i = Istart ; i <= Iend ; i++)
			{
				if (i > Istart) buf.Append(separator);
				buf.Append(elements[i].ToString());
			}

			return buf.ToString();
		}
	
		
		/// <summary>
		/// 	Join elements as string with separator
		/// </summary>
		/// <param name='elements'>
		/// 	Element list or collection
		/// </param>
		/// <param name='separator'>
		/// 	Separator string
		/// </param>
		public static string Join (string[] elements, string separator)
		{
			var n = elements.Length;
			StringBuilder buf = new StringBuilder();
			for (int i = 0 ; i < n ; i++)
			{
				if (i > 0) buf.Append(separator);
				buf.Append(elements[i]);
			}
			
			return buf.ToString();
		}

		
		/// <summary>
		/// 	Provide a unique int for a string based on the notion that the string contains only alphabetic characters
		/// </summary>
		/// <param name='str'>
		/// 	String to hash
		/// </param>
		/// <param name='offset'>
		///		Offset in string
		/// </param>
		/// <param name='len'>
		/// 	Length of substring to hash
		/// </param>
		/// <param name='ignorecase'>
		/// 	If true will ignore case
		/// </param>
		public static int Unique (string str, int offset, int len, bool ignorecase = false)
		{
			int multiplier = 1;
			int hash = 0;
			
			if (ignorecase)
			{
				for (int i = 0 ; i < len ; i++)
				{
					char c = char.ToUpper(str[i+offset]);
					hash += (c - 'A') * multiplier;
					multiplier *= 26;	
				}
			} 
			else 
			{
				for (int i = 0 ; i < len ; i++)
				{
					char c = str[i+offset];
					if (c >= 'a')
						hash += (c - 'a' + 26) * multiplier;
					else
						hash += (c - 'A') * multiplier;
						
					multiplier *= 52;	
				}
			}
			
			return hash;
		}
			
	
		/// <summary>
		/// Strips the CR/LF's from a string.
		/// </summary>
		public static string StripCRLF (string s)
		{
			StringBuilder news = new StringBuilder(s.Length);
			
			for (int i = 0 ; i < s.Length ; i++)
			{
				char c = s[i];
				if (c == '\n')
					continue;
				if (c == '\r')
					continue;
				
				news.Append(c);
			}
			
			return news.ToString();
		}


        /// <summary>
        /// Create a comma delimited list from collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">the collection to be rendered into a string</param>
        public static string ToString<T>(IEnumerable<T> collection)
        {
            StringBuilder s = new StringBuilder();
            int i = 0;
			
			s .Append("[");
            foreach (T v in collection)
            {
                if (i++ > 0) s.Append(",");
                s.Append(v.ToString());
            }
			
			s.Append("]");
            return s.ToString();
        }

		
		/// <summary>
		/// Escapes a SQL string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="s">S.</param>
		public static string EscapeSqlString (string s)
		{
			StringBuilder str = new StringBuilder (s.Length + 64);

			for (int i = 0 ; i < s.Length ; i++)
			{
				var c = s [i];
				switch (c)
				{
					case '\'':
						case '\"':
						case '\\':
						case '%':
						case '_':
						str.Append ('\\');
						str.Append (c);
						break;

						case '\r':
						str.Append ("\\r");
						break;

						case '\n':
						str.Append ("\\n");
						break;

						case '\t':
						str.Append ("\\t");
						break;

						default:
						str.Append (c);
						break;
				}
			}

			return str.ToString ();
		}


		#region Implementation

		/// <summary>
		/// Escapes the char.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="c">C.</param>
		private static string EscapeChar (char c)
		{
			if (c == '\'')
				return "'\\''";
			
			if (c > 32)
				return "'" + c + "'";
			
			switch (c)
			{
				case '\a':
					return "'\\a'";
					
				case '\b':
					return "'\\b'";
					
				case '\n':
					return "'\\n'";
					
				case '\v':
					return "'\\v'";
					
				case '\r':
					return "'\\r'";
					
				case '\f':
					return "'\\f'";
					
				case '\t':
					return "'\\t";
					
				default:
					return string.Format ("'\\x{0:x}", (int) c);
			}
		}


		/// <summary>
		/// Escapes the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="s">S.</param>
		private static string EscapeString (string s)
		{
			return s.Replace ("\"", "\\\"");
		}


		
		// Some types (System.Json.JsonPrimitive) implement
		// IEnumerator and yet, throw an exception when we
		// try to use them, helper function to check for that
		// condition
		private static bool WorksAsEnumerable (object obj)
		{
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				try 
				{
					enumerable.GetEnumerator ();
					return true;
				} 
				catch 
				{
					// nothing, we return false below
				}
			}
			return false;
		}


		#endregion
	}
}



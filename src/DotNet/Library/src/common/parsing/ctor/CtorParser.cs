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

using bridge.common.parsing;
using bridge.common.utils;


namespace bridge.common.parsing.ctor
{
	/// <summary>
	/// Parse a constructor
	/// <pre>
	/// 	Foo(12.3, "this is a test", 3, [1,2,3,4,5])
	/// </pre>
	/// The following argument types are recognized
	/// <ul>
	/// 	<li>strings:	"abcbd" or 'abcd'</li>
	/// 	<li>numeric:	1.234, 4</li>
	/// 	<li>arrays:		[1,2,3,4] or [1.4,3.6,...]</li>
	/// </ul>
	/// Furthermore, integers vs floats are distinguished as to whether they have a decimal point or not.
	/// An array takes on the type of the first value in the array.
	/// </summary>
	public class CtorParser
	{
		public CtorParser (string ctor)
		{
			_ctor = ctor;
			CtorLexer lexer = new CtorLexer(ctor);
			
			IList<CtorToken> tokens = lexer.Tokens;
			int len = tokens.Count;
	
			if (tokens[0].Type != CtorToken.TType.IDENTIFIER)
				throw new Exception ("ctor: missing constructor class name");
			
			// get token name
			_klass = (string)tokens[0];
			
			if (tokens[1].Type != CtorToken.TType.OPEN_PAREN)
				throw new Exception ("ctor: missing open paren");
			
			if (tokens[tokens.Count-1].Type != CtorToken.TType.CLOSE_PAREN)
				throw new Exception ("ctor: missing close paren");
			
			// get arguments
			IList<object> args = Args (tokens, 2, len -2);
			_args = Post (args);
		}
		
		
		
		// Properties
		
		
		public string Class
			{ get { return _klass; } }
		
		public object[] Arguments
			{ get { return _args; } }
		
		public string Constructor
			{ get { return _ctor; } }
		
	
		
		// Implementation
		
		
		private IList<object> Args (IList<CtorToken> tokens, int Istart, int Iend)
		{
			var nargs = new List<object>();
			
			for (int i = Istart ; i <= Iend ; i++)
			{
				CtorToken tok = tokens[i];
				switch (tok.Type)
				{
					case CtorToken.TType.SEPARATOR:
						break;
						
					case CtorToken.TType.ARRAY_OPEN:
						int Astart = i+1;
						int Aend = Astart;
						
						while (Aend < Iend && tokens[Aend].Type != CtorToken.TType.ARRAY_CLOSE) Aend++;					
						nargs.Add (Args (tokens, Astart, Aend-1));
						i = Aend-1;
						break;
					
					case CtorToken.TType.NUMERIC:
					case CtorToken.TType.STRING:
					case CtorToken.TType.IDENTIFIER:
						nargs.Add(tok.Payload);
						break;
				}
			}
			
			return nargs;
		}
		
		
		private object[] Post (IList<object> args)
		{
			var nargs = new object[args.Count];
			for (int i = 0 ; i < args.Count ; i++)
			{
				object elem = args[i];
				if (elem is IList<object>)
				{
					var list = (IList<object>)elem;
					if (list[0] is int)
						nargs[i] = ToIntArray(list);
					else if (list[0] is double)
						nargs[i] = ToDoubleArray(list);
					else if (list[0] is string)
						nargs[i] = ToStringArray(list);
					else
						nargs[i] = list;
				} else
					nargs[i] = elem;
			}
			
			return nargs;
		}
		
		
		private int[] ToIntArray (IList<object> list)
		{
			int len = list.Count;
			int[] array = new int[len];
			
			for (int i = 0 ; i < len ; i++)
				array[i] = ((int)list[i]);
			
			return array;
		}
		
		
		private double[] ToDoubleArray (IList<object> list)
		{
			int len = list.Count;
			double[] array = new double[len];
			
			for (int i = 0 ; i < len ; i++)
				array[i] = ((double)list[i]);
			
			return array;
		}
		
		
		private string[] ToStringArray (IList<object> list)
		{
			int len = list.Count;
			string[] array = new string[len];
			
			for (int i = 0 ; i < len ; i++)
				array[i] = ((string)list[i]);
			
			return array;
		}
		
		
		
		
		
		// Variables
		
		private string		_ctor;
		private string		_klass;
		private object[]	_args;
	}
}


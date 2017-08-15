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
using System.Reflection;

using bridge.common.parsing.ctor;


namespace bridge.common.reflection
{
	/// <summary>
	/// Object creation functions
	/// </summary>
	public static class Creator
	{
		/// <summary>
		/// Create new object instance for type, given args
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static object NewInstanceByType (Type type, params object[] args)
		{
			if (args != null && args.Length > 0)
			{
				ConstructorInfo ctor = FindMatchingCtor (type, args);
				if (ctor == null)
					throw new ArgumentException ("could not find constructor for given arguments");
				
				try
					{ return ctor.Invoke (args); }
				catch
					{ }
				
				ReflectUtils.ConformArguments (ctor.GetParameters(), args);
				return ctor.Invoke (args);
			} else
				return Activator.CreateInstance (type);
		}
		
		
		
		/// <summary>
		/// Create new object instance for type, given args
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='typename'>
		/// Type name
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static object NewInstanceByName (string typename, params object[] args)
		{
			Type type = ReflectUtils.FindType (typename);
			if (type == null)
				throw new ArgumentException ("could not find type: " + typename);
			
			return NewInstance (type, args);
		}

		
		/// <summary>
		/// Create new object instance for type, given args
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static object NewInstance (Type type, params object[] args)
		{
			return NewInstanceByType (type, args);
		}
		
		
		/// <summary>
		/// Create new object instance for type, given args
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='typename'>
		/// Type name
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static object NewInstance (string typename, params object[] args)
		{
			return NewInstanceByName (typename, args);
		}

				
		/// <summary>
		/// Create instance of class specified by string classname(a,b,c)
		/// </summary>
		/// <param name='ctorInvocation'>
		/// constructor spec (ie  FooClass(1.3,4.45,test)
		/// </param>
		public static object NewByCtor (string ctorInvocation)
		{
			CtorParser parser = new CtorParser(ctorInvocation);
			
			object[] args = parser.Arguments;
			Type type = ReflectUtils.FindType (parser.Class);

			return NewInstance (type, args);
		}

		
		/// <summary>
		/// Create instance of class specified by string classname(a,b,c)
		/// </summary>
		/// <param name='ctorInvocation'>
		/// constructor spec (ie  FooClass(1.3,4.45,test)
		/// </param>
		public static object NewByCtor (string namespc, string ctorInvocation)
		{
			CtorParser parser = new CtorParser(ctorInvocation);
			
			object[] args = parser.Arguments;
			
			Type type = null;
			if (parser.Class.IndexOf ('.') > 0)
				type = ReflectUtils.FindType (parser.Class);
			else
				type = ReflectUtils.FindType (namespc + "." + parser.Class);

			return NewInstance (type, args);
		}
		
		
		
		// Implementation
		
		
		/// <summary>
		/// Finds the ctor that best matches the argument set
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		private static ConstructorInfo FindMatchingCtor (Type type, object[] args)
		{
			ConstructorInfo best = null;
			int bestscore = int.MinValue;
			
			foreach (var ctor in type.GetConstructors())
			{
				var paramlist = ctor.GetParameters();
				if (paramlist.Length != args.Length)
					continue;
				
				var score = ReflectUtils.ScoreParameters (paramlist, args);
				if (score == 0)
					return ctor;
				if (score > bestscore)
					{ best = ctor; bestscore = score; }
			}
			
			return best;
		}
		
	}
}


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

using System.Reflection;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using bridge.common.utils;
using bridge.common.time;
using bridge.common.collections;
using System.IO;
using bridge.common.serialization;


namespace bridge.common.reflection
{
	/// <summary>
	/// Reflection utils
	/// </summary>
	public static class ReflectUtils
	{
		static ReflectUtils ()
		{
			_system_assemblies.Add ("FSharp.Core", "FSharp.Core, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
		}

		/// <summary>
		/// Finds type by name, where name need not be fully qualified with namespace and assembly
		/// </summary>
		/// <returns>
		/// The type or null if not found
		/// </returns>
		/// <param name='name'>
		/// Type name, such as "Foo" or "bridge.models.frob.Foo"
		/// </param>
		public static Type FindType (string name)
		{
			Type type = null;
			if (_types.TryGetValue(name, out type))
				return type;

			// replace + in type with /
			var typename = name;
			if (typename.IndexOf ('+') >= 0)
				typename = StringUtils.Replace (typename, '+', '/');

			if ((type = FindInAssemblies (typename)) != null)
			{
				_types[name] = type;
				return type;
			}

			try
			{
				type = Type.GetType (typename);
				_types[name] = type;
				return type;
			}
			catch (Exception)
			{
				return null;
			}

		}


		/// <summary>
		/// Checks the type (used to force a type load)
		/// </summary>
		/// <param name="type">Type.</param>
		public static void CheckType (Type type)
		{
			if (type.GetMember ("ToString") == null)
				throw new ArgumentException ("type not instantiated properly: " + type);
		}


		/// <summary>
		/// Determines object if is instance of generic type the specified obj type.
		/// </summary>
		/// <returns><c>true</c> if is instance of generic type the specified obj type; otherwise, <c>false</c>.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="type">Type.</param>
		public static bool IsInstanceOfGenericType (object obj, Type type)
		{
			var otype = obj.GetType ();

			if (!otype.IsGenericType)
				return false;
			else
				return otype.GetGenericTypeDefinition () == type;
		}

		
		/// <summary>
		/// Finds type by name, where name need not be fully qualified with namespace and assembly
		/// </summary>
		/// <returns>
		/// The type or null if not found
		/// </returns>
		/// <param name='name'>
		/// Type name, such as "Foo" or "bridge.models.frob.Foo"
		/// </param>
		public static IntPtr FindTypeByHandle (string name)
		{
			Type type = FindType (name);
			if (type == null)
				throw new Exception ("could not find type: " + name);
			
			return type.TypeHandle.Value;
		}
		
		
		/// <summary>
		/// Finds type by name, where name need not be fully qualified with namespace and assembly
		/// 
		/// This version searches a specific assembly and its dependencies, but does not cache as the assembly may
		/// have been dynamically loaded.  Hence, calls to this function may be expensive, so should only be done
		/// infrequently.
		/// </summary>
		/// <returns>
		/// The type or null if not found
		/// </returns>
		/// <param name='assembly'>
		/// assembly
		/// </param>
		/// <param name='name'>
		/// Type name, such as "Foo" or "bridge.models.frob.Foo"
		/// </param>
		public static Type FindType (Assembly assembly, string name)
		{
			Type type = null;
			if ((type = FindInAssembly(assembly, name)) != null)
				return type;
			
			foreach (var sub in assembly.GetReferencedAssemblies())
			{
                Assembly ass = Assembly.Load(sub);
				if ((type = FindInAssembly(ass, name)) != null)
					return type;		
			}
			
			return null;
		}
		
		
		/// <summary>
		/// Finds type by name, where name need not be fully qualified with namespace and assembly
		/// 
		/// This version searches a specific assembly and its dependencies, but does not cache as the assembly may
		/// have been dynamically loaded.  Hence, calls to this function may be expensive, so should only be done
		/// infrequently.
		/// </summary>
		/// <returns>
		/// The type or null if not found
		/// </returns>
		/// <param name='assembly'>
		/// assembly
		/// </param>
		/// <param name='name'>
		/// Type name, such as "Foo" or "bridge.models.frob.Foo"
		/// </param>
		public static IntPtr FindTypeByHandle (Assembly assembly, string name)
		{
			Type type = FindType (assembly, name);
			if (type == null)
				throw new Exception ("could not find type: " + name);
			
			return type.TypeHandle.Value;
		}


        /// <summary>
        /// Add given assembly to the list of those to be searched with reflection
        /// </summary>
        /// <param name="assembly">Assembly to be added.</param>
        public static void Register (Assembly assembly)
        {
            _supplemental.Add (assembly);
        }
		
		
		/// <summary>
		/// Attempts to locate the named assembly within the current app domain or tries
		/// to load from path in filesystem.
		/// </summary>
		/// <returns>
		/// The assembly.
		/// </returns>
		/// <param name='name'>
		/// Assembly name as in "bridge.indicators"
		/// </param>
		public static Assembly FindAssembly (string name)
		{
			if (name == null)
				throw new ArgumentException ("tried to load an assembly with NULL name");

			if (name.EndsWith(".dll") || name.EndsWith(".exe"))
				name = StringUtils.RTrimField (name, 1, '.');

			// if is a system assembly, load from fully qualitied name
			if (_system_assemblies.ContainsKey (name))
				return Assembly.Load (_system_assemblies [name]);
			
			// first search loaded assemblies
			AppDomain domain = AppDomain.CurrentDomain;
			Assembly assembly = Collections.FindOne (
				domain.GetAssemblies(), (a) => string.Compare(name, a.GetName().Name, true) == 0);
			if (assembly != null)
				return assembly;
			
			// now try to find in filesystem
			var path = StringUtils.Or (
				ResourceLoader.Find ("lib/" + name + ".dll"),
				ResourceLoader.Find (name + ".dll"));

			// try to find the assembly without the full rooted name 
			if (path == null && Path.IsPathRooted (name))
				return FindAssembly (Path.GetFileName (name));

			if (path == null)
				throw new ArgumentException ("could not find assembly: " + name);
			
			var pdir = Directory.GetParent(path);
			var pcwd = Environment.CurrentDirectory;
			
			Environment.CurrentDirectory = pdir.FullName;
			
			try
				{ assembly = Assembly.LoadFrom (path); }
			finally
				{ Environment.CurrentDirectory = pcwd; }
			
			return assembly;
		}
		
		
		/// <summary>
		/// Calls the named static method.
		/// </summary>
		/// <returns>
		/// The return of static method call
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='parameters'>
		/// Parameters.
		/// </param>
		public static object CallStaticMethod (Type type, string method, params object[] parameters)
		{
			MethodInfo imethod = FindMatchingMethod (type, method, parameters);
			if (imethod != null)
				return CallMethod (null, imethod, parameters);
			else
				throw new ArgumentException ("could not find method '" + method + "' in " + type + ", args: " + StringUtils.ToString(parameters));
		}

		
		
		/// <summary>
		/// Calls the named static method.
		/// </summary>
		/// <returns>
		/// The return of static method call
		/// </returns>
		/// <param name='classname'>
		/// class name.
		/// </param>
		/// <param name='method'>
		/// Method name.
		/// </param>
		/// <param name='parameters'>
		/// Parameters.
		/// </param>
		public static object CallStaticMethodByName (string classname, string method, params object[] parameters)
		{
			Type type = FindType (classname);
			if (type == null)
				throw new Exception ("CallStaticMethod: could not find specified type: " + classname);
			
			MethodInfo imethod = FindMatchingMethod (type, method, parameters);
			if (method == null)
				throw new Exception ("CallStaticMethod: could not find matching method: " + method);

			return CallMethod (null, imethod, parameters);
		}
		
		
		/// <summary>
		/// Calls the named method, finding the one with the best match
		/// </summary>
		/// <returns>
		/// The return of static method call
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='parameters'>
		/// Parameters.
		/// </param>
		public static object CallMethod (object obj, string name, params object[] parameters)
		{
			var type = obj.GetType();
			MethodInfo method = FindMatchingMethod (type, name, parameters);
			
			if (method == null)
				throw new ArgumentException ("cannot find matching method: " + name + ", within: " + type + ", requested with " + parameters.Length + " params");

			return CallMethod (obj, method, parameters);
		}

		
		/// <summary>
		/// Gets the named property
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		public static object GetProperty (object obj, string name)
		{
			var index = name.IndexOf ('[');
			if (index < 0)
			{
				var type = obj.GetType();
				PropertyInfo prop = type.GetProperty (name);
				
				if (prop == null)
					throw new ArgumentException ("cannot find matching property: " + name + ", whithin: " + type);

				return prop.GetValue (obj, null);
			}
			else
			{
				var ith = int.Parse (name.Substring(index+1, name.Length - index - 2));
				var propname = name.Substring (0, index);

				return GetIndexedProperty (obj, propname, ith);
			}
		}
		
		
		/// <summary>
		/// Gets the ith element of collection at named property
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ith'>
		/// Index.
		/// </param>
		public static object GetIndexedProperty (object obj, string name, int ith)
		{
			var type = obj.GetType();
			PropertyInfo prop = type.GetProperty (name);
			
			if (prop == null)
				throw new ArgumentException ("cannot find matching property: " + name + ", whithin: " + type);

			// get the collection at the property
			var collection = prop.GetValue (obj, null);
				
			// get the name of the this[] indexer
			var attrs = collection.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true);
			var indexname = ((DefaultMemberAttribute)attrs[0]).MemberName;
					
			// get the property info for it
			var indexer = collection.GetType().GetProperty (indexname);
				
			return indexer.GetValue (collection, new object[] {ith});
		}
		
		
		/// <summary>
		/// Gets the ith element of collection
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='ith'>
		/// Index.
		/// </param>
		public static object GetIndexed (object obj, int ith)
		{
			var type = obj.GetType();

			// get the name of the this[] indexer
			var attrs = obj.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true);
			var indexname = ((DefaultMemberAttribute)attrs[0]).MemberName;
			
			// get the property info for it
			var indexer = obj.GetType().GetProperty (indexname);
			
			return indexer.GetValue (obj, new object[] {ith});
		}

		
		/// <summary>
		/// Sets the named property
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param val='value'>
		/// property value
		/// </param>
		public static void SetProperty (object obj, string name, object val)
		{
			var type = obj.GetType();
			PropertyInfo prop = type.GetProperty (name);
			
			if (prop == null)
				throw new ArgumentException ("cannot find matching property: " + name + ", whithin: " + type);

			prop.SetValue (obj, val, null);
		}

		
		/// <summary>
		/// Gets the named property
		/// </summary>
		/// <param name='classname'>
		/// class name
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		public static object GetStaticProperty (string classname, string name)
		{
			var type = FindType (classname);
			PropertyInfo prop = type.GetProperty (name);
			
			if (prop == null)
				throw new ArgumentException ("cannot find matching property: " + name + ", whithin: " + classname);

			return prop.GetValue (null, null);
		}

		
		/// <summary>
		/// Sets the named property
		/// </summary>
		/// <param name='classname'>
		/// class name
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param val='value'>
		/// property value
		/// </param>
		public static void SetStaticProperty (string classname, string name, object val)
		{
			var type = FindType (classname);
			PropertyInfo prop = type.GetProperty (name);
			
			if (prop == null)
				throw new ArgumentException ("cannot find matching property: " + name + ", whithin: " + classname);

			prop.SetValue (null, val, null);
		}
		
		
		/// <summary>
		/// Finds the matching or closest method
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static MethodInfo FindMatchingMethod (Type type, string name, object[] args)
		{
			MethodInfo best = null;
			int bestscore = int.MinValue;
			
			foreach (var method in type.GetMethods())
			{
				var paramlist = method.GetParameters();
				if (paramlist.Length != args.Length)
					continue;
				
				if (method.Name != name)
					continue;
				
				var score = ReflectUtils.ScoreParameters (paramlist, args);
				if (score > bestscore)
					{ best = method; bestscore = score; }
			}
			
			return best;
		}


		
		/// <summary>
		/// Find delegate associated with event by name
		/// </summary>
		/// <param name='obj'>
		/// Object containing event
		/// </param>
		/// <param name='eventname'>
		/// Event name.
		/// </param>
		public static Delegate GetEventDelegate (object obj, string eventname)
		{
			Type type = obj.GetType();
			
			while (type != null)
			{ 
				FieldInfo[] fields = type.GetFields (BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
				foreach (FieldInfo field in fields)
       			{
         			if (field.Name == eventname)
						return field.GetValue(obj) as Delegate;
				}
				
				type = type.BaseType;
			}
	
			return null;
       	}
		
		
		/// <summary>
		/// Finds all methods on a type with matching attribute
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='attribute'>
		/// Attribute type
		/// </param>
		public static IList<MethodInfo> FindMethodsWithAttribute (Type type, Type attribute)
		{
			var list = new List<MethodInfo> ();
			foreach (var method in type.GetMethods())
			{
				var attr = FindAttribute (method, attribute);
				if (attr != null)
					list.Add (method);
			}
			
			return list;
		}

		
				
		/// <summary>
		/// Finds all types within an assembly sporting given attribute
		/// </summary>
		/// <param name='assembly'>
		/// Assembly to search.
		/// </param>
		/// <param name='attribute'>
		/// Attribute type
		/// </param>
		public static IList<Type> FindTypesWithAttribute (Assembly assembly, Type attribute)
		{
			var list = new List<Type> ();
			foreach (var type in assembly.GetTypes())
			{
				var attr = FindAttribute (type, attribute);
				if (attr != null)
					list.Add (type);
			}
			
			return list;
		}


		/// <summary>
		/// Finds the properties matching selector
		/// </summary>
		/// <returns>The properties.</returns>
		/// <param name="type">Type.</param>
		/// <param name="selector">Selector.</param>
		public static IList<PropertyInfo> FindProperties (Type type, Func<PropertyInfo, bool> selector)
		{
			var proplist = new List<PropertyInfo> ();
			foreach (var prop in type.GetProperties ())
			{
				if (selector (prop))
					proplist.Add (prop);
			}

			return proplist;
		}
		
		
		/// <summary>
		/// Convert arguments to parameter types where they mismatch
		/// </summary>
		/// <param name='ctor'>
		/// Ctor.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static void ConformArguments (ParameterInfo[] paramlist, object[] args)
		{
			int i = -1;
			foreach (var parm in paramlist)
			{
				i++;
				object arg = args[i];
				Type pclass = parm.ParameterType;
				Type aclass = arg != null ? arg.GetType() : null;
				
				var ptype = ValueTypeUtils.TypeOf (pclass);
				var atype = ValueTypeUtils.TypeOf (aclass);
				
				if ((atype == ptype && atype != VType.Other) || pclass == aclass || pclass.IsAssignableFrom (aclass))
					continue;

				if (arg == null && pclass.IsValueType)
					throw new ArgumentException ("could not find matching constructor");
				
				if (arg == null)
					continue;
				
				// convert simple value types to conform
				if (ptype != VType.Other)
				{
					args [i] = ValueTypeUtils.Convert (arg, ptype, pclass);
				}

				// special handling for delegates
				else if (DelegateGenerator.IsDelegate (pclass))
				{
					args [i] = DelegateGenerator.ConvertToDelegate (pclass, args [i]);
				}

				// special case for ZDateTime, allowing long specifier
				else if (pclass == typeof(ZDateTime))
				{
					if (aclass == typeof (long))
						args[i] = new ZDateTime((long)arg, ZTimeZone.NewYork);
					else if (aclass == typeof(string))
						args[i] = new ZDateTime((string)arg);
					else
						throw new ArgumentException ("unknown argument pairing");
				}

				// see if we can instantiate a persitable class
				else if (atype == VType.String && pclass.IsSubclassOf(typeof(IPersist<string>)))
				{
					var nobj = (IPersist<string>)Activator.CreateInstance(pclass);
					nobj.State = (string)arg;
					args[i] = nobj;
				}
				
				// the parameter type is an array, but our value is a single value of the same type, convert
				else if (pclass.IsArray && pclass.GetElementType() == aclass)
				{
					var narray = Array.CreateInstance (pclass.GetElementType(), 1);
					narray.SetValue (arg, 0);
					args[i] = narray;
				}

				else if (typeof(Vector<double>).IsAssignableFrom (pclass))
				{
					if (aclass == typeof(double))
					{
						var vec = new DenseVector (1);
						vec[0] = ((Double)args[i]);
						args[i] = vec;
					}
					if (aclass == typeof(int))
					{
						var vec = new DenseVector (1);
						vec[0] = (double)((Int32)args[i]);
						args[i] = vec;
					}
				}
				
				// otherwise try to create a new instance from string
				else if (atype == VType.String)
				{
					string sval = (string)arg;
					try
					{
						var method = pclass.GetMethod ("Parse");
						if (method != null)
						{
							args[i] = method.Invoke (null, new object[] { sval });
							continue;
						}
					}
					catch
						{ }
					
					try
					{
						args[i] = Activator.CreateInstance(pclass, sval);
					}
					catch
						{ throw new ArgumentException ("could not coerce type " + aclass + " to " + pclass); }
				}
			}
		}


		
		#region Implementaton


		private static object CallMethod (object obj, MethodInfo method, params object[] parameters)
		{
			try
			{ 
				return method.Invoke (obj, parameters); 
			}
			catch (TargetInvocationException te)
			{ 
				throw te.InnerException;
			}
			catch (MethodAccessException me)
			{
				throw new Exception ("method " + method.Name + " is not a public method", me);
			}
			catch
			{ }

			try
			{
				ConformArguments (method.GetParameters(), parameters);
				return method.Invoke (obj, parameters);
			}
			catch (TargetInvocationException te)
			{ 
				throw te.InnerException;
			}
		}


		private static Type FindInAssemblies (string stype)
		{
            foreach (Assembly a in _supplemental)
			{
				try
				{
					Type type = FindInAssembly(a, stype);
					if (type != null)
						return type;
				}
				catch (Exception e)
				{
					_log.Warn("could not load assembly: " + a + ", why: " + e.Message);
				}
			}

            AppDomain dom = AppDomain.CurrentDomain;
			foreach (Assembly a in dom.GetAssemblies())
			{
				try
				{
					Type type = FindInAssembly (a, stype);
					if (type != null)
						return type;
				}
				catch (Exception e)
				{
					_log.Warn ("could not load assembly: " + a + ", why: " + e.Message);
				}
			}

			return null;
		}


		private static Type FindInAssembly (Assembly assembly, string stype)
		{
			stype = "." + StringUtils.Replace (stype, '/', '.');

			foreach (Type type in assembly.GetTypes())
			{
				var fullname = "." + StringUtils.Replace (type.FullName, '+', '.');

				if (fullname.EndsWith(stype))
					return type;
			}

			return null;
		}


		
		private static object FindAttribute (MethodInfo method, Type attrib)
		{
			var attrs = method.GetCustomAttributes(attrib, true);
            return attrs.Length > 0 ? attrs[0] : null;
        }

		
		private static object FindAttribute (Type type, Type attrib)
		{
			var attrs = type.GetCustomAttributes(attrib, true);
			return attrs.Length > 0 ? attrs[0] : null;
		}
		

				
		
		/// <summary>
		/// Scores the parameters for a given method or ctor signature relative to arguments provided
		/// </summary>
		/// <param name='paramlist'>
		/// List of method parameters
		/// </param>
		/// <param name='args'>
		/// Arguments supplied
		/// </param>
		internal static int ScoreParameters (ParameterInfo[] paramlist, object[] args)
		{
			var score = 0;
			
			int i = -1;
			foreach (var parm in paramlist)
			{
				i++;
				var eklass = parm.ParameterType;
				var aklass = args[i] != null ? args[i].GetType() : null;
				
				if (eklass == aklass || eklass.IsAssignableFrom (aklass) || DelegateGenerator.IsAssignableFrom (eklass, aklass))
					score += 200;
				else
					score += ArgumentPenalty (eklass, aklass);
			}
			
			return score;
		}
		
		
		/// <summary>
		/// Determine penalty for mismatched arguments (- is penalty and + is a reward)
		/// </summary>
		/// <returns>
		/// The penalty.
		/// </returns>
		/// <param name='paramclass'>
		/// parameter class.
		/// </param>
		/// <param name='argclass'>
		/// class of given argument.
		/// </param>
		internal static int ArgumentPenalty (Type paramclass, Type argclass)
		{
			if (typeof(Vector<double>).IsAssignableFrom (paramclass))
			{
				if (typeof(Vector<double>).IsAssignableFrom (argclass))
					return 100;
				if (argclass == typeof(double) || argclass == typeof(int))
					return 50;
				else
					return -100;
			}

			if (paramclass == typeof(string) && argclass == null)
				return 100;
			if (!paramclass.IsValueType && argclass == null)
				return 100;
			if (paramclass.IsArray && paramclass.GetElementType () == argclass)
				return 10;
			if (!paramclass.IsValueType)
				return -100;
			
			var paramtype = ValueTypeUtils.TypeOf (paramclass);
			var argtype = ValueTypeUtils.TypeOf (argclass);
			
			if (paramtype == argtype && paramtype != VType.Other)
				return 100;
			
			switch (paramtype)
			{
				case VType.Short:
					switch (argtype)
					{
						case VType.Int:
						case VType.Long:
							return 50;
						case VType.Float:
						case VType.Double:
							return 20;
						default:
							return -50;
					}
					
				case VType.Int:
					switch (argtype)
					{
						case VType.Long:
							return 50;
						case VType.Short:
						case VType.Float:
						case VType.Double:
							return 75;
						default:
							return -50;
					}
					
				case VType.Long:
					switch (argtype)
					{
						case VType.Short:
						case VType.Int:
						case VType.Float:
						case VType.Double:
							return 50;
						default:
							return -50;
					}
					
				case VType.Float:
					switch (argtype)
					{
						case VType.Float:
						case VType.Double:
							return 100;
						case VType.Long:
							return 20;
						case VType.Short:
						case VType.Int:
							return 50;
						default:
							return -50;
					}
					
				case VType.Double:
					switch (argtype)
					{
						case VType.Int:
						case VType.Short:
						case VType.Long:
							return 50;
						case VType.Float:
						case VType.Double:
							return 100;
						default:
							return -50;
					}
					
				case VType.Bool:
					switch (argtype)
					{
						case VType.Long:
						case VType.Float:
						case VType.Double:
							return 20;
						case VType.Int:
						case VType.Short:
						case VType.Byte:
						case VType.Char:
							return 50;
						default:
							return -50;
							
				case VType.Enum:
					switch (argtype)
					{
						case VType.Int:
							return 50;
						case VType.String:
							return 100;
						default:
							return -50;
					}
				}
			}
			
			if (paramclass == typeof(ZDateTime))
			{
				if (argclass == typeof(string))
					return 20;
				if (argclass == typeof(long))
					return 50;
				else 
					return -50;
			}

			return -100;
		}

		#endregion
		
		// Variables

		static Dictionary<string,string>	_system_assemblies = new Dictionary<string,string> ();
        static List<Assembly>               _supplemental = new List<Assembly>();
		static IDictionary<string,Type>		_types = new Dictionary<string,Type>();
		static Logger						_log = Logger.Get ("REFLECT");
	}
}


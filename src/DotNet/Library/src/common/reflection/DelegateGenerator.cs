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


namespace bridge.common.reflection
{
	public class DelegateGenerator
	{
		/// <summary>
		/// Determines if is delegate
		/// </summary>
		/// <returns><c>true</c> if is delegate the specified type; otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		public static bool IsDelegate (Type type)
		{
			return typeof(Delegate).IsAssignableFrom (type.BaseType);
		}


		/// <summary>
		/// Determines whether the argument type is a delegate and then 
		/// </summary>
		/// <returns><c>true</c> if is assignable from the specified delegatetype srcobj; otherwise, <c>false</c>.</returns>
		/// <param name="delegatetype">Delegatetype.</param>
		/// <param name="srcobj">Srcobj.</param>
		public static bool IsAssignableFrom (Type delegatetype, Type srcobj)
		{
			if (!IsDelegate (delegatetype))
				return false;
			if (srcobj == null)
				return true;

			var mdelegate = delegatetype.GetMethod ("Invoke");
			var matching = FindMatchingDelegateMethod (mdelegate, srcobj);

			return matching != null;
		}


		/// <summary>
		/// Converts to delegate: finds a method signature on object matching delegate
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="obj">Object.</param>
		public static object ConvertToDelegate (Type target, object obj)
		{
			if (obj == null)
				return null;

			var mdelegate = target.GetMethod ("Invoke");
			var objtype = obj.GetType ();

			var matching = FindMatchingDelegateMethod (mdelegate, objtype);
			if (matching == null)
				throw new ArgumentException ("could not find matching delegate method in: " + objtype + " for delegate: " + target);

			return Delegate.CreateDelegate(target, obj, matching);
		}


		#region Implementation




		/// <summary>
		/// Finds the matching delegate method.
		/// </summary>
		/// <returns>The matching delegate method or null.</returns>
		/// <param name="delegate_method">Delegate method info.</param>
		/// <param name="srctype">Src object type</param>
		private static MethodInfo FindMatchingDelegateMethod (MethodInfo delegate_method, Type srctype)
		{
			var target_return = delegate_method.ReturnType;
			var target_params = delegate_method.GetParameters ();

			foreach (var method in srctype.GetMethods ()) 
			{
				if (method.ReturnType != target_return)
					continue;

				if (!method.IsPublic)
					continue;

				var args = method.GetParameters ();
				if (args.Length != target_params.Length)
					continue;

				var matching = true;
				for (int i = 0; i < args.Length && matching; i++)
					matching = args [i].ParameterType == target_params [i].ParameterType;

				if (matching)
					return method;
			}

			return null;
		}


		#endregion
	}
}


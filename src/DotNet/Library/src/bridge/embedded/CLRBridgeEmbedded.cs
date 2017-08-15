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
using bridge.common.reflection;
using bridge;


namespace bridge.embedded
{
	/// <summary>
	/// Access to CLR types: provided for both local embedded access
	/// </summary>
	public class CLRBridgeEmbedded : ICLRBridge
	{
		/// <summary>
		/// Creates a new object of given classname
		/// </summary>
		/// <param name="classname">Classname.</param>
		/// <param name="parameters">Parameters.</param>
		public object Create (string classname, params object[] parameters)
		{
			return Creator.NewInstanceByName (classname, parameters);
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
		public object CallStaticMethodByName (string classname, string method, params object[] parameters)
		{
			return ReflectUtils.CallStaticMethodByName (classname, method, parameters);
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
		public object CallMethod (object obj, string name, params object[] parameters)
		{
			return ReflectUtils.CallMethod (obj, name, parameters);
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
		public object GetProperty (object obj, string name)
		{
			return ReflectUtils.GetProperty (obj, name);
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
		public object GetIndexedProperty (object obj, string name, int ith)
		{
			return ReflectUtils.GetIndexedProperty (obj, name, ith);
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
		public object GetIndexed (object obj, int ith)
		{
			return ReflectUtils.GetIndexed (obj, ith);
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
		public void SetProperty (object obj, string name, object val)
		{
			ReflectUtils.SetProperty (obj, name, val);
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
		public object GetStaticProperty (string classname, string name)
		{
			return ReflectUtils.GetStaticProperty (classname, name);
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
		public void SetStaticProperty (string classname, string name, object val)
		{
			ReflectUtils.SetStaticProperty (classname, name, val);
		}


		/// <summary>
		/// Protects the given object from GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		public void Protect (object obj)
		{
		}
		
		
		/// <summary>
		/// Releases object for GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		public void Release (object obj)
		{
		}

	}
}


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


namespace bridge
{
	/// <summary>
	/// Access to CLR types: provided for both local and remote access
	/// </summary>
	public interface ICLRBridge
	{
		/// <summary>
		/// Creates a new object of given classname
		/// </summary>
		/// <param name="classname">Classname.</param>
		/// <param name="parameters">Parameters.</param>
		object						Create (string classname, params object[] parameters);


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
		object 						CallStaticMethodByName (string classname, string method, params object[] parameters);
		
		
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
		object 						CallMethod (object obj, string name, params object[] parameters);
		
		
		/// <summary>
		/// Gets the named property
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		object 						GetProperty (object obj, string name);
		
		
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
		object 						GetIndexedProperty (object obj, string name, int ith);
		
		
		/// <summary>
		/// Gets the ith element of collection
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='ith'>
		/// Index.
		/// </param>
		object 						GetIndexed (object obj, int ith);
		
		
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
		void 						SetProperty (object obj, string name, object val);
		
		
		/// <summary>
		/// Gets the named property
		/// </summary>
		/// <param name='classname'>
		/// class name
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		object 						GetStaticProperty (string classname, string name);
		
		
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
		void 						SetStaticProperty (string classname, string name, object val);


		/// <summary>
		/// Protects the given object from GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		void						Protect (object obj);
		
		
		/// <summary>
		/// Releases object for GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		void						Release (object obj);

	}
}


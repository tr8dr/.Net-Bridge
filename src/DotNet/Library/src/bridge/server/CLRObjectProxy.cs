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
using System.Threading;


namespace bridge.server
{
	/// <summary>
	/// CLR object proxy.
	/// </summary>
	public class CLRObjectProxy
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.server.CLRObjectProxy"/> class.
		/// </summary>
		/// <param name="oid">Oid.</param>
		public CLRObjectProxy (int oid)
		{
			_objectId = oid;
		}

		// Properties

		public int ObjectId
			{ get { return _objectId; } }

		public string ClassName
			{ get { return Find(_objectId, false).GetType().ToString(); } }


		// Class Methods

		/// <summary>
		/// Create a proxy for the given object (and record the mapping)
		/// </summary>
		/// <param name="obj">Object.</param>
		public static CLRObjectProxy ProxyFor (object obj)
		{
			var iproxy = obj as CLRObjectProxy;
			if (iproxy != null)
				return iproxy;

			int objectid = 0;
			if (_cache_oi.TryGetValue (obj, out objectid))
				return new CLRObjectProxy (objectid);
				
			var proxy = new CLRObjectProxy (Interlocked.Increment (ref _idgenerator));
			_cache_io [proxy.ObjectId] = obj;
			_cache_oi [obj] = proxy.ObjectId;
			return proxy;
		}

		
		/// <summary>
		/// Create a proxy for the given object (and record the mapping)
		/// </summary>
		/// <param name="obj">Object.</param>
		public static int ProxyIdFor (object obj)
		{
			var iproxy = obj as CLRObjectProxy;
			if (iproxy != null)
				return iproxy.ObjectId;
			
			int objectid = 0;
			if (_cache_oi.TryGetValue (obj, out objectid))
				return objectid;
			
			var proxy = new CLRObjectProxy (Interlocked.Increment (ref _idgenerator));
			_cache_io [proxy.ObjectId] = obj;
			_cache_oi [obj] = proxy.ObjectId;
			return proxy.ObjectId;
		}


		/// <summary>
		/// Release the specified proxy.
		/// </summary>
		/// <param name="proxy">Proxy.</param>
		public static void Release (CLRObjectProxy proxy)
		{
			Release (proxy.ObjectId);
		}
		
		
		/// <summary>
		/// Release the specified proxy by id.
		/// </summary>
		/// <param name="proxyId">Proxy ID.</param>
		public static void Release (int proxyId)
		{
			object obj = null;
			if (_cache_io.TryGetValue (proxyId, out obj))
			{
				_cache_io.Remove (proxyId);
				_cache_oi.Remove (obj);
			}
		}

		
		/// <summary>
		/// Find object by proxy
		/// </summary>
		/// <param name="proxy">Proxy.</param>
		/// <param name="obj">Mapped object.</param>
		public static bool TryFindObject (CLRObjectProxy proxy, out object obj)
		{
			return (_cache_io.TryGetValue (proxy.ObjectId, out obj));
		}
		
		
		/// <summary>
		/// Find object by proxy
		/// </summary>
		/// <param name="proxyid">Proxy.</param>
		/// <param name="obj">Mapped object.</param>
		public static bool TryFindObject (int proxyid, out object obj)
		{
			return (_cache_io.TryGetValue (proxyid, out obj));
		}

		
		/// <summary>
		/// Find object for proxy ID
		/// </summary>
		/// <param name="proxyid">Proxy.</param>
		public static object Find (int proxyid, bool proxyok = true)
		{
			object obj = null;
			if (_cache_io.TryGetValue (proxyid, out obj))
				return obj;
			if (proxyok)
				return new CLRObjectProxy (proxyid);
			else
				throw new ArgumentException ("cannot find object associated with proxy: " + proxyid);
		}


		#region Meta

		public override string ToString ()
		{
			return string.Format ("[CLRObjectProxy: ObjectId={0}]", ObjectId);
		}

		public override int GetHashCode ()
		{
			return ObjectId;
		}

		#endregion

		// Variables

		private int						_objectId;
		static Dictionary<int,object>	_cache_io = new Dictionary<int, object>();
		static Dictionary<object,int>	_cache_oi = new Dictionary<object, int>();
		static int						_idgenerator = 0;
	}
}


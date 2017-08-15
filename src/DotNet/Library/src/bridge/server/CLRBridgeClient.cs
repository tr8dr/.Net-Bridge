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
using System.Net.Sockets;
using System.Threading;
using bridge.common.io;
using System.IO;
using bridge.common.utils;
using bridge.server.ctrl;
using bridge.server.data;


namespace bridge.server
{
	/// <summary>
	/// Client side of CLR-server bridge
	/// </summary>
	public class CLRBridgeClient : ICLRBridge
	{
		public CLRBridgeClient (int port)
		{
			Url = new Uri ("svc://127.0.0.1:" + port + "/");
			AttemptConnection (1);
		}

		public CLRBridgeClient (string url)
		{
			Url = new Uri(url);
			AttemptConnection (1);
		}


		// Properties

		public Uri Url
			{ get; private set; }


		// Operations


		/// <summary>
		/// Creates a new object of given classname
		/// </summary>
		/// <param name="classname">Classname.</param>
		/// <param name="parameters">Parameters.</param>
		public object Create (string classname, params object[] parameters)
		{
			// send request
			var req = new CLRCreateMessage (classname, parameters);
			CLRMessage.Write (_cout, req);

			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRCallStaticMethodMessage (classname, method, parameters);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRCallMethodMessage (obj, name, parameters);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRGetPropertyMessage (obj, name);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRGetIndexedPropertyMessage (obj, name, ith);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRGetIndexedMessage (obj, ith);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRSetPropertyMessage (obj, name, val);
			CLRMessage.Write (_cout, req);
			
			// get response (to make sure is not an exception)
			CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRGetStaticPropertyMessage (classname, name);
			CLRMessage.Write (_cout, req);
			
			// get response
			return CLRMessage.ReadValue (_cin);
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
			// send request
			var req = new CLRSetStaticPropertyMessage (classname, name, val);
			CLRMessage.Write (_cout, req);
			
			// get response (to make sure is not an exception)
			CLRMessage.ReadValue (_cin);
		}

		
		/// <summary>
		/// Protects the given object from GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		public void Protect (object obj)
		{
			// send request
			var req = new CLRProtectMessage (obj);
			CLRMessage.Write (_cout, req);
		}
		
		
		/// <summary>
		/// Releases object for GCing
		/// </summary>
		/// <param name="obj">Object.</param>
		public void Release (object obj)
		{
			// send request
			var req = new CLRReleaseMessage (obj);
			CLRMessage.Write (_cout, req);
		}


		#region Implementation
		
		
		/// <summary>
		/// Attempts the connection with retry
		/// </summary>
		/// <param name='retries'>
		/// Retries.
		/// </param>
		/// <param name='timeout'>
		/// Timeout in seconds
		/// </param>
		private void AttemptConnection (int retries = 10, int timeout = 5)
		{
			Exception error = null;
			for (int i = 0; i <= retries ; i++)
			{
				if (i > 0)
				{
					Console.Error.WriteLine ("clr: failed to connect to clr server, will retry in " + timeout + " secs, url: " + Url); 
					Thread.Sleep (timeout * 1000);
				}
				
				try 
				{
					_client = new TcpClient ();
					_client.Connect(Url.Host, Url.Port);
					_client.NoDelay = true;
					_stream = new BufferedDuplexStream (_client.GetStream());
					_cin = EndianStreams.ReaderFor (_stream, EndianStreams.Endian.Little);
					_cout = EndianStreams.WriterFor (_stream, EndianStreams.Endian.Little);
					return;
				}
				catch (Exception e)
				{
					_client = null;
					error = e;
				}
			}

			throw error;
		}


		#endregion


		// Variables

		private TcpClient			_client;
		private Stream				_stream;
		private IBinaryReader		_cin;
		private IBinaryWriter		_cout;

		static Logger				_log = Logger.Get ("CLR");
	}
}


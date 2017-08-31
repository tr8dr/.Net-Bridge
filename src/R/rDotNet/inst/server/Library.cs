// -------------------------------------------
// global using directives
// -------------------------------------------
using GFSpinLock = bridge.common.system.SpinLock;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Generic; 
using System.Collections;
using System.Diagnostics;
using System.Diagnostics; 
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System;
using System; 
using bridge.common.collections;
using bridge.common.io;
using bridge.common.parsing.ctor;
using bridge.common.parsing.dates;
using bridge.common.parsing;
using bridge.common.reflection;
using bridge.common.serialization;
using bridge.common.system;
using bridge.common.time;
using bridge.common.utils;
using bridge.embedded;
using bridge.math.matrix;
using bridge.server.ctrl;
using bridge.server.data;
using bridge;
// -------------------------------------------
// File: ../DotNet/Library/src/bridge/embedded/CLRBridgeEmbedded.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/ICLRBridge.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/CLRBridgeClient.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/CLRBridgeServer.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server
{
    /// <summary>
    /// Provides the server end of the CLR protocol
    /// </summary>
    public class CLRBridgeServer
    {
		public CLRBridgeServer (Uri url)
			: this (url.Port)
		{
		}

		public CLRBridgeServer(int port)
        {
			_port = port;
		}


		// Functions


		/// <summary>
		/// Start the server
		/// </summary>
		/// <param name="step">If set to <c>true</c> will allow stepping and not start a thread.</param>
		public void Start (bool blocking = false)
		{
			if (_server_socket != null)
				return;

            if (!SetupListener())
                return;

			if (!blocking)
			{
				var worker = new Thread(_ => Service());
				worker.Start();
			}
			else
			{
				Service ();
			}
		}



		#region Server


		/// <summary>
		/// Setup server.
		/// </summary>
		private bool SetupListener ()
		{
			_log.Info("starting execution server listener on port: " + _port);
			var mask = new IPEndPoint(IPAddress.Any, _port);
			_server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _server_socket.Bind(mask);
                _server_socket.Listen(10);
                return true;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    _log.Warn("another CLR server already running, exiting");
                    return false;
                }
                else
                    throw e;
            }
		}


        /// <summary>
        /// Handle incoming clients
        /// </summary>
        private void Service()
        {
            while (true)
            {
                var client_socket = _server_socket.Accept();

                _log.Info("execution: received new client from: " + client_socket.RemoteEndPoint);
				client_socket.NoDelay = true;
				var stream = new BufferedDuplexStream(new NetworkStream(client_socket));
                var client = new CLRBridgeServerClient (stream, client_socket.RemoteEndPoint);

                client.Start();
            }
        }


		#endregion

        // Variables

		private int				_port;
        private Socket			_server_socket;

        static Logger			_log = Logger.Get("CLR");
    }
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/CLRBridgeServerClient.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server
{
	/// <summary>
	/// CLR bridge server client handler.
	/// </summary>
	public class CLRBridgeServerClient
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.server.CLRBridgeServerClient"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public CLRBridgeServerClient (Stream stream, EndPoint endpoint)
		{
			_stream = stream;
			_endpoint = endpoint;

			_cin = EndianStreams.ReaderFor (stream, EndianStreams.Endian.Little);
			_cout = EndianStreams.WriterFor (stream, EndianStreams.Endian.Little);
		}


		// Functions


		/// <summary>
		/// Start servicing client
		/// </summary>
		public void Start ()
		{
			_servicer = new Thread (_ => 
			{
				try
				{
					Service();
				}
				catch (IOException e)
				{
					_log.Info ("CLR bridge client closed: " + _endpoint);
				}
				catch (Exception f)
				{
					_log.Info ("CLR bridge client closed: " + _endpoint + ", with exception: " + f.ToString());
				}
			});

			_servicer.Start();
		}


		#region Service


		/// <summary>
		/// Service loop.
		/// </summary>
		private void Service ()
		{
			CLRMessage msg = null;
			try
			{
				while ((msg = CLRMessage.Read (_cin)) != null)
				{
					switch (msg.MessageType)
					{
						case CLRMessage.TypeCreate:
							HandleCreate (msg as CLRCreateMessage);
							break;

						case CLRMessage.TypeCallStaticMethod:
							HandleCallStaticMethod (msg as CLRCallStaticMethodMessage);
							break;

						case CLRMessage.TypeCallMethod:
							HandleCallMethod (msg as CLRCallMethodMessage);
							break;

						case CLRMessage.TypeGetProperty:
							HandleGetProperty (msg as CLRGetPropertyMessage);
							break;
							
						case CLRMessage.TypeSetProperty:
							HandleSetProperty (msg as CLRSetPropertyMessage);
							break;
							
						case CLRMessage.TypeGetStaticProperty:
							HandleGetStaticProperty (msg as CLRGetStaticPropertyMessage);
							break;
							
						case CLRMessage.TypeSetStaticProperty:
							HandleSetStaticProperty (msg as CLRSetStaticPropertyMessage);
							break;
							
						case CLRMessage.TypeGetIndexedProperty:
							HandleGetIndexedProperty (msg as CLRGetIndexedPropertyMessage);
							break;
							
						case CLRMessage.TypeGetIndexed:
							HandleGetIndexed (msg as CLRGetIndexedMessage);
							break;
							
						case CLRMessage.TypeProtect:
							HandleProtect (msg as CLRProtectMessage);
							break;
							
						case CLRMessage.TypeRelease:
							HandleRelease (msg as CLRReleaseMessage);
							break;

						case CLRMessage.TypeTemplateReq:
							HandleTemplate (msg as CLRTemplateReqMessage);
							break;

						default:
							throw new ArgumentException ("unknown request message: " + msg);
					}
				}
			}
			catch (Exception e)
			{
				_log.Warn ("receipt of messsage failed: " + e.ToString () + ", stack: " + e.StackTrace);
			}
		}


		#endregion

		#region Behaviors


		/// <summary>
		/// Creates a new object of given classname
		/// </summary>
		/// <param name="req">Request.</param>
		private void HandleCreate (CLRCreateMessage req)
		{
			try
			{
				var obj = _api.Create (req.ClassName, req.Parameters);
				CLRMessage.WriteValue (_cout, obj);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}


		/// <summary>
		/// Handles the template request
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleTemplate (CLRTemplateReqMessage req)
		{
			try
			{
				var type = ReflectUtils.FindType (req.ClassName);
				CLRMessage.Write (_cout, new CLRTemplateReplyMessage (type));
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}

		
		/// <summary>
		/// Calls a static method.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleCallStaticMethod (CLRCallStaticMethodMessage req)
		{
			try
			{
				var obj = _api.CallStaticMethodByName (req.ClassName, req.MethodName, req.Parameters);
				CLRMessage.WriteValue (_cout, obj);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		
		
		/// <summary>
		/// Calls a method.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleCallMethod (CLRCallMethodMessage req)
		{
			try
			{
				// get object
				var obj = ToLocalObject (req.Obj);
				// invoke
				var result = _api.CallMethod (obj, req.MethodName, req.Parameters);
				CLRMessage.WriteValue (_cout, result);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		

		/// <summary>
		/// Gets property on object.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleGetProperty (CLRGetPropertyMessage req)
		{
			try
			{
				// get object
				var obj = ToLocalObject (req.Obj);
				// invoke
				var result = _api.GetProperty (obj, req.PropertyName);
				CLRMessage.WriteValue (_cout, result);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		
		
		/// <summary>
		/// Sets property on object.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleSetProperty (CLRSetPropertyMessage req)
		{
			try
			{
				// get object
				var obj = ToLocalObject (req.Obj);
				// invoke
				_api.SetProperty (obj, req.PropertyName, req.Value);
				CLRMessage.WriteValue (_cout, null);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		
		
		/// <summary>
		/// Gets static property on class.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleGetStaticProperty (CLRGetStaticPropertyMessage req)
		{
			try
			{
				// invoke
				var result = _api.GetStaticProperty (req.ClassName, req.PropertyName);
				CLRMessage.WriteValue (_cout, result);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		
		
		/// <summary>
		/// Sets static property on class.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleSetStaticProperty (CLRSetStaticPropertyMessage req)
		{
			try
			{
				// invoke
				_api.SetProperty (req.ClassName, req.PropertyName, req.Value);
				CLRMessage.WriteValue (_cout, null);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}


		/// <summary>
		/// Gets the indexed property.
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleGetIndexedProperty (CLRGetIndexedPropertyMessage req)
		{
			try
			{
				// get object
				var obj = ToLocalObject (req.Obj);
				// invoke
				var result = _api.GetIndexedProperty (obj, req.PropertyName, req.Index);
				CLRMessage.WriteValue (_cout, result);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}
		

		/// <summary>
		/// Gets the indexed value on an object
		/// </summary>
		/// <param name="req">Req.</param>
		private void HandleGetIndexed (CLRGetIndexedMessage req)
		{
			try
			{
				// get object
				var obj = ToLocalObject (req.Obj);
				// invoke
				var result = _api.GetIndexed (obj, req.Index);
				CLRMessage.WriteValue (_cout, result);
			}
			catch (TargetInvocationException te)
			{
				CLRMessage.WriteValue (_cout, te.GetBaseException());
			}
			catch (Exception e)
			{
				CLRMessage.WriteValue (_cout, e);
			}
		}

		
		/// <summary>
		/// Protects the given object from GCing
		/// </summary>
		/// <param name="req">Request.</param>
		private void HandleProtect (CLRProtectMessage req)
		{
		}
		
		
		/// <summary>
		/// Releases object for GCing
		/// </summary>
		/// <param name="req">Request.</param>
		private void HandleRelease (CLRReleaseMessage req)
		{
			CLRObjectProxy.Release (req.ObjectId);
		}


		#endregion

		#region Miscellaneous


		/// <summary>
		/// Test object to see whether local or not
		/// </summary>
		/// <returns>The local object.</returns>
		/// <param name="obj">Object.</param>
		private object ToLocalObject (object obj)
		{
			// make sure is not a proxy on the server side
			var proxy = obj as CLRObjectProxy;
			if (proxy == null)
				return obj;
			else
				throw new ArgumentException ("could not find object associated with proxy: " + proxy.ObjectId);
		}


		#endregion


		// Variables

		private Stream				_stream;
		private EndPoint			_endpoint;

		private IBinaryReader		_cin;
		private IBinaryWriter		_cout;

		private Thread				_servicer;
		private CLRBridgeEmbedded	_api = new CLRBridgeEmbedded();

		static Logger				_log = Logger.Get ("CLR");
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/CLRMessage.cs
// -------------------------------------------
﻿// 
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


namespace bridge.server
{
	/// <summary>
	/// CLR message base class
	/// </summary>
	public class CLRMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.core.CLRMessage"/> class.
		/// </summary>
		/// <param name="type">Message type.</param>
		public CLRMessage (byte type)
		{
			_type = type;
		}


		// Properties

		/// <summary>
		/// Message type
		/// </summary>
		public byte MessageType
			{ get { return _type; } }


		// Serialization

		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public virtual void Serialize (IBinaryWriter cout)
		{
			cout.WriteUInt16 (Magic);
			cout.WriteByte (_type);
		}

		/// <summary>
		/// Deserialize the message (assumes type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public virtual void Deserialize (IBinaryReader cin)
		{
		}


		// IO


		/// <summary>
		/// Read the next message from the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static CLRMessage Read (IBinaryReader stream)
		{
			var magic = stream.ReadUInt16();
			if (magic != Magic)
				throw new ArgumentException ("encountered message with wrong magic #, protocol wrong");

			var type = (byte)stream.ReadByte();
			var msg = Create (type);

			msg.Deserialize (stream);
			return msg;
		}


		/// <summary>
		/// Read a value off of the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static object ReadValue (IBinaryReader stream)
		{
			var obj = DeserializeValue (stream);
			if (obj is Exception)
				throw (Exception)obj;
			else
				return obj;
		}

		
		/// <summary>
		/// Write a message to the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static void Write (IBinaryWriter stream, CLRMessage msg)
		{
			msg.Serialize (stream);
			stream.Flush();
		}


		/// <summary>
		/// Write a value to the stream
		/// </summary>
		/// <param name="stream">Stream.</param>
		public static void WriteValue (IBinaryWriter stream, object value)
		{
			SerializeValue (stream, value);
			stream.Flush();
		}


		#region Message Creation


		/// <summary>
		/// Create the specified type.
		/// </summary>
		/// <param name="type">Type.</param>
		internal static CLRMessage Create (byte type)
		{
			switch (type)
			{
				case TypeNull:
					return new CLRNullMessage ();
				case TypeBool:
					return new CLRBoolMessage ();
				case TypeByte:
					return new CLRByteMessage ();
				case TypeInt32:
					return new CLRInt32Message ();
				case TypeInt64:
					return new CLRInt64Message ();
				case TypeReal64:
					return new CLRReal64Message ();
				case TypeString:
					return new CLRStringMessage ();
				case TypeObject:
					return new CLRObjectMessage ();

				case TypeBoolArray:
					return new CLRBoolArrayMessage ();
				case TypeByteArray:
					return new CLRByteArrayMessage ();
				case TypeInt32Array:
					return new CLRInt32ArrayMessage ();
				case TypeInt64Array:
					return new CLRInt64ArrayMessage ();
				case TypeReal64Array:
					return new CLRReal64ArrayMessage ();
				case TypeStringArray:
					return new CLRStringArrayMessage ();
				case TypeObjectArray:
					return new CLRObjectArrayMessage ();
				
				case TypeVector:
					return new CLRVectorMessage ();
				case TypeMatrix:
					return new CLRMatrixMessage ();
				case TypeException:
					return new CLRExceptionMessage ();

				case TypeCreate:
					return new CLRCreateMessage ();
				case TypeCallStaticMethod:
					return new CLRCallStaticMethodMessage ();
				case TypeCallMethod:
					return new CLRCallMethodMessage ();
				case TypeGetProperty:
					return new CLRGetPropertyMessage ();
				case TypeGetIndexedProperty:
					return new CLRGetIndexedPropertyMessage ();
				case TypeGetIndexed:
					return new CLRGetIndexedMessage ();
				case TypeSetProperty:
					return new CLRSetPropertyMessage ();
				case TypeGetStaticProperty:
					return new CLRGetStaticPropertyMessage ();
				case TypeSetStaticProperty:
					return new CLRSetStaticPropertyMessage ();
				case TypeProtect:
					return new CLRProtectMessage ();
				case TypeRelease:
					return new CLRReleaseMessage ();

				case TypeTemplateReq:
					return new CLRTemplateReqMessage ();
				case TypeTemplateReply:
					return new CLRTemplateReplyMessage ();

				default:
					throw new ArgumentException ("encountered unknow CLR message type, bad protocol: " + (int)type);
			}
		}


		/// <summary>
		/// Get the type associated with a value
		/// </summary>
		/// <param name="value">Value.</param>
		internal static byte TypeOf (object value)
		{
			if (value == null)
				return TypeNull;

			if (value is Exception)
				return TypeException;

			byte ctype = 0;
			var klass = value.GetType();
			if (_typemap.TryGetValue (klass, out ctype))
				return ctype;
			if (klass.IsArray)
				return TypeObjectArray;
			else
				return TypeObject;
		}


		/// <summary>
		/// Serlializes a value.
		/// </summary>
		/// <param name="cout">Cout.</param>
		/// <param name="val">Value.</param>
		internal static void SerializeValue (IBinaryWriter cout, object val)
		{
			CLRMessage msg = null;
			switch (TypeOf (val))
			{
				case TypeNull:
					msg = new CLRNullMessage ();
					break;

				case TypeBool:
					msg = new CLRBoolMessage (Convert.ToBoolean (val));
					break;

				case TypeByte:
					msg = new CLRByteMessage (Convert.ToByte (val));
					break;

				case TypeInt32:
					msg = new CLRInt32Message (Convert.ToInt32 (val));
					break;

				case TypeInt64:
					msg = new CLRInt64Message (Convert.ToInt64 (val));
					break;

				case TypeReal64:
					msg = new CLRReal64Message (Convert.ToDouble (val));
					break;

				case TypeString:
					msg = new CLRStringMessage ((string)val);
					break;

				case TypeObject:
					msg = new CLRObjectMessage (val);
					break;

				case TypeBoolArray:
					msg = new CLRBoolArrayMessage ((bool[])val);
					break;

				case TypeByteArray:
					msg = new CLRByteArrayMessage ((byte[])val);
					break;

				case TypeInt32Array:
					msg = new CLRInt32ArrayMessage ((int[])val);
					break;

				case TypeInt64Array:
					msg = new CLRInt64ArrayMessage ((long[])val);
					break;

				case TypeReal64Array:
					msg = new CLRReal64ArrayMessage ((double[])val);
					break;

				case TypeStringArray:
					msg = new CLRStringArrayMessage ((string[])val);
					break;

				case TypeObjectArray:
					msg = new CLRObjectArrayMessage ((object[])val);
					break;

				case TypeVector:
					msg = new CLRVectorMessage ((Vector<double>)val);
					break;

				case TypeMatrix:
					msg = new CLRMatrixMessage ((Matrix<double>)val);
					break;

				case TypeException:
					msg = new CLRExceptionMessage (val);
					break;
					
				default:
					throw new ArgumentException ("do not know how to serialize: " + val.GetType());
			}

			msg.Serialize (cout);
		}


		/// <summary>
		/// Serlializes a value.
		/// </summary>
		/// <param name="cout">Cout.</param>
		/// <param name="val">Value.</param>
		internal static object DeserializeValue (IBinaryReader cin)
		{
			var msg = Read (cin);
			switch (msg.MessageType)
			{
				case TypeNull:
					return null;

				case TypeBool:
					return ((CLRBoolMessage)msg).Value;

				case TypeByte:
					return ((CLRByteMessage)msg).Value;

				case TypeInt32:
					return ((CLRInt32Message)msg).Value;

				case TypeInt64:
					return ((CLRInt64Message)msg).Value;

				case TypeReal64:
					return ((CLRReal64Message)msg).Value;

				case TypeString:
					return ((CLRStringMessage)msg).Value;

				case TypeObject:
					return ((CLRObjectMessage)msg).ToObject();

				case TypeBoolArray:
					return ((CLRBoolArrayMessage)msg).Value;

				case TypeByteArray:
					return ((CLRByteArrayMessage)msg).Value;

				case TypeInt32Array:
					return ((CLRInt32ArrayMessage)msg).Value;

				case TypeInt64Array:
					return ((CLRInt64ArrayMessage)msg).Value;

				case TypeReal64Array:
					return ((CLRReal64ArrayMessage)msg).Value;

				case TypeStringArray:
					return ((CLRStringArrayMessage)msg).Value;

				case TypeObjectArray:
					return ((CLRObjectArrayMessage)msg).Value;

				case TypeVector:
					return ((CLRVectorMessage)msg).Value;

				case TypeMatrix:
					return ((CLRMatrixMessage)msg).Value;

				case TypeException:
					return ((CLRExceptionMessage)msg).ToException();

				default:
					throw new ArgumentException ("do not know how to deserialize: " + msg.GetType());
			}
		}


		#endregion 

		#region Message Types

		public const ushort			Magic						= 0xd00d;

		public const byte			TypeNull					= 0;
		public const byte			TypeBool					= 1;
		public const byte			TypeByte					= 2;
		public const byte			TypeInt32					= 5;
		public const byte			TypeInt64					= 6;
		public const byte			TypeReal64					= 7;
		public const byte			TypeString					= 8;
		public const byte			TypeObject					= 9;

		public const byte			TypeVector					= 21;
		public const byte			TypeMatrix					= 22;
		public const byte			TypeException				= 23;

		public const byte			TypeBoolArray				= 101;
		public const byte			TypeByteArray				= 102;
		public const byte			TypeInt32Array				= 105;
		public const byte			TypeInt64Array				= 106;
		public const byte			TypeReal64Array				= 107;
		public const byte			TypeStringArray				= 108;
		public const byte			TypeObjectArray				= 109;

		public const byte			TypeCreate					= 201;
		public const byte			TypeCallStaticMethod		= 202;
		public const byte			TypeCallMethod				= 203;
		public const byte			TypeGetProperty				= 204;
		public const byte			TypeGetIndexedProperty		= 205;
		public const byte			TypeGetIndexed				= 206;
		public const byte			TypeSetProperty				= 207;
		public const byte			TypeGetStaticProperty		= 208;
		public const byte			TypeSetStaticProperty		= 209;
		public const byte			TypeProtect					= 210;
		public const byte			TypeRelease					= 211;

		public const byte			TypeTemplateReq				= 212;
		public const byte			TypeTemplateReply			= 213;

		#endregion

		#region Static Initializer


		static CLRMessage ()
		{
			_typemap[typeof(bool)] = TypeBool;
			_typemap[typeof(byte)] = TypeByte;
			_typemap[typeof(short)] = TypeInt32;
			_typemap[typeof(int)] = TypeInt32;
			_typemap[typeof(long)] = TypeInt64;
			_typemap[typeof(float)] = TypeReal64;
			_typemap[typeof(double)] = TypeReal64;
			_typemap[typeof(string)] = TypeString;
			_typemap[typeof(object)] = TypeObject;

			_typemap[typeof(bool[])] = TypeBoolArray;
			_typemap[typeof(byte[])] = TypeByteArray;
			_typemap[typeof(int[])] = TypeInt32Array;
			_typemap[typeof(long[])] = TypeInt64Array;
			_typemap[typeof(float[])] = TypeReal64Array;
			_typemap[typeof(double[])] = TypeReal64Array;
			_typemap[typeof(string[])] = TypeStringArray;
			_typemap[typeof(object[])] = TypeObjectArray;

			_typemap[typeof(IndexedVector)] = TypeVector;
			_typemap[typeof(SubviewVector)] = TypeVector;
			_typemap[typeof(DenseVector)] = TypeVector;
			
			_typemap[typeof(IndexedMatrix)] = TypeMatrix;
			_typemap[typeof(DenseMatrix)] = TypeMatrix;
		}

		#endregion

		// Variables

		protected byte					_type;
		static Dictionary<Type,byte>	_typemap = new Dictionary<Type,byte>();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/CLRObjectProxy.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRCallMethodMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Call Method message.
	/// </summary>
	public class CLRCallMethodMessage : CLRMessage
	{
		public CLRCallMethodMessage ()
			: base (TypeCallMethod)
		{
		}

		public CLRCallMethodMessage (object obj, string method, params object[] args)
			: base (TypeCallMethod)
		{
			Obj = obj;
			MethodName = method;
			Parameters = args;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public string MethodName
			{ get; private set; }

		public object[] Parameters
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteString (MethodName);

			// arguments
			cout.WriteUInt16 ((ushort)Parameters.Length);
			for (int i = 0 ; i < Parameters.Length ; i++)
				CLRMessage.SerializeValue (cout, Parameters[i]);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			MethodName = cin.ReadString();

			// arguments
			var len = (int)cin.ReadUInt16 ();
			Parameters = new object[len];

			for (int i = 0 ; i < len ; i++)
				Parameters[i] = CLRMessage.DeserializeValue (cin);
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRCallStaticMethodMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Call Static Method message.
	/// </summary>
	public class CLRCallStaticMethodMessage : CLRMessage
	{
		public CLRCallStaticMethodMessage ()
			: base (TypeCallStaticMethod)
		{
		}

		public CLRCallStaticMethodMessage (string classname, string method, params object[] args)
			: base (TypeCallStaticMethod)
		{
			ClassName = classname;
			MethodName = method;
			Parameters = args;
		}


		// Properties

		public string ClassName
			{ get; private set; }

		public string MethodName
			{ get; private set; }

		public object[] Parameters
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteString (ClassName);
			cout.WriteString (MethodName);

			// arguments
			cout.WriteUInt16 ((ushort)Parameters.Length);
			for (int i = 0 ; i < Parameters.Length ; i++)
				CLRMessage.SerializeValue (cout, Parameters[i]);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			ClassName = cin.ReadString();
			MethodName = cin.ReadString();

			// arguments
			var len = (int)cin.ReadUInt16 ();
			Parameters = new object[len];

			for (int i = 0 ; i < len ; i++)
				Parameters[i] = CLRMessage.DeserializeValue (cin);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRCreateMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Create Object message.
	/// </summary>
	public class CLRCreateMessage : CLRMessage
	{
		public CLRCreateMessage ()
			: base (TypeCreate)
		{
		}

		public CLRCreateMessage (string classname, params object[] args)
			: base (TypeCreate)
		{
			ClassName = classname;
			Parameters = args;
		}


		// Properties

		public string ClassName
			{ get; private set; }

		public object[] Parameters
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class name
			cout.WriteString (ClassName);

			// arguments
			cout.WriteUInt16 ((ushort)Parameters.Length);
			for (int i = 0 ; i < Parameters.Length ; i++)
				CLRMessage.SerializeValue (cout, Parameters[i]);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// create class name
			ClassName = cin.ReadString();

			// arguments
			var len = (int)cin.ReadUInt16 ();
			Parameters = new object[len];

			for (int i = 0 ; i < len ; i++)
				Parameters[i] = CLRMessage.DeserializeValue (cin);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRGetIndexedMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR GetIndexed message.
	/// </summary>
	public class CLRGetIndexedMessage : CLRMessage
	{
		public CLRGetIndexedMessage ()
			: base (TypeGetIndexed)
		{
		}

		public CLRGetIndexedMessage (object obj, int index)
			: base (TypeGetIndexed)
		{
			Obj = obj;
			Index = index;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public int Index
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteInt32 (Index);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			Index = cin.ReadInt32();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRGetIndexedPropertyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR GetIndexedProperty message.
	/// </summary>
	public class CLRGetIndexedPropertyMessage : CLRMessage
	{
		public CLRGetIndexedPropertyMessage ()
			: base (TypeGetIndexedProperty)
		{
		}

		public CLRGetIndexedPropertyMessage (object obj, string property, int index)
			: base (TypeGetIndexedProperty)
		{
			Obj = obj;
			PropertyName = property;
			Index = index;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public string PropertyName
			{ get; private set; }
		
		public int Index
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteString (PropertyName);
			cout.WriteInt32 (Index);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			PropertyName = cin.ReadString();
			Index = cin.ReadInt32();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRGetPropertyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR GetProperty message.
	/// </summary>
	public class CLRGetPropertyMessage : CLRMessage
	{
		public CLRGetPropertyMessage ()
			: base (TypeGetProperty)
		{
		}

		public CLRGetPropertyMessage (object obj, string property)
			: base (TypeGetProperty)
		{
			Obj = obj;
			PropertyName = property;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public string PropertyName
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteString (PropertyName);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			PropertyName = cin.ReadString();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRGetStaticPropertyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR GetStaticProperty message.
	/// </summary>
	public class CLRGetStaticPropertyMessage : CLRMessage
	{
		public CLRGetStaticPropertyMessage ()
			: base (TypeGetStaticProperty)
		{
		}

		public CLRGetStaticPropertyMessage (string classname, string property)
			: base (TypeGetStaticProperty)
		{
			ClassName = classname;
			PropertyName = property;
		}


		// Properties

		public string ClassName
			{ get; private set; }

		public string PropertyName
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteString (ClassName);
			cout.WriteString (PropertyName);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			ClassName = cin.ReadString();
			PropertyName = cin.ReadString();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRProtectMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Protect message.
	/// </summary>
	public class CLRProtectMessage : CLRMessage
	{
		public CLRProtectMessage ()
			: base (TypeProtect)
		{
		}

		public CLRProtectMessage (object obj)
			: base (TypeProtect)
		{
			ObjectId = CLRObjectProxy.ProxyIdFor (obj);
		}


		// Properties

		public int ObjectId
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (ObjectId);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			ObjectId = cin.ReadInt32();
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRReleaseMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Release message.
	/// </summary>
	public class CLRReleaseMessage : CLRMessage
	{
		public CLRReleaseMessage ()
			: base (TypeRelease)
		{
		}

		public CLRReleaseMessage (object obj)
			: base (TypeRelease)
		{
			ObjectId = CLRObjectProxy.ProxyIdFor (obj);
		}


		// Properties

		public int ObjectId
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (ObjectId);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			ObjectId = cin.ReadInt32();
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRSetPropertyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR SetProperty message.
	/// </summary>
	public class CLRSetPropertyMessage : CLRMessage
	{
		public CLRSetPropertyMessage ()
			: base (TypeSetProperty)
		{
		}

		public CLRSetPropertyMessage (object obj, string property, object value)
			: base (TypeSetProperty)
		{
			Obj = obj;
			PropertyName = property;
			Value = value;
		}


		// Properties

		public object Obj
			{ get; private set; }

		public string PropertyName
			{ get; private set; }

		public object Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteInt32 (CLRObjectProxy.ProxyIdFor (Obj));
			cout.WriteString (PropertyName);
			CLRMessage.SerializeValue (cout, Value);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			Obj = CLRObjectProxy.Find (cin.ReadInt32(), proxyok: true);
			PropertyName = cin.ReadString();
			Value = CLRMessage.DeserializeValue (cin);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRSetStaticPropertyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR SetStaticProperty message.
	/// </summary>
	public class CLRSetStaticPropertyMessage : CLRMessage
	{
		public CLRSetStaticPropertyMessage ()
			: base (TypeSetStaticProperty)
		{
		}

		public CLRSetStaticPropertyMessage (string classname, string property, object value)
			: base (TypeSetStaticProperty)
		{
			ClassName = classname;
			PropertyName = property;
			Value = value;
		}


		// Properties

		public string ClassName
			{ get; private set; }

		public string PropertyName
			{ get; private set; }

		public object Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			// class & method names
			cout.WriteString (ClassName);
			cout.WriteString (PropertyName);
			CLRMessage.SerializeValue (cout, Value);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			// class & method names
			ClassName = cin.ReadString();
			PropertyName = cin.ReadString();
			Value = CLRMessage.DeserializeValue (cin);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRTemplateReplyMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Call Method message.
	/// </summary>
	public class CLRTemplateReplyMessage : CLRMessage
	{
		public CLRTemplateReplyMessage ()
			: base (TypeTemplateReply)
		{
		}

		public CLRTemplateReplyMessage (Type type)
			: base (TypeTemplateReply)
		{
			var props = type.GetProperties (BindingFlags.Public | BindingFlags.Instance).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			var methods = type.GetMethods (BindingFlags.Public | BindingFlags.Instance).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			var classmethods = type.GetMethods (BindingFlags.Public | BindingFlags.Static).
				Where (m => !m.IsSpecialName).
				Select (x => x.Name).
				Distinct ();

			PropertyList = props.ToArray ();
			MethodList = methods.ToArray ();
			ClassMethodList = classmethods.ToArray ();
		}


		// Properties

		public string[] PropertyList
			{ get; set; }

		public string[] MethodList
			{ get; set; }

		public string[] ClassMethodList
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			cout.WriteInt32 (PropertyList.Length);
			foreach (var str in PropertyList)
				cout.WriteString(str);

			cout.WriteInt32 (MethodList.Length);
			foreach (var str in MethodList)
				cout.WriteString(str);

			cout.WriteInt32 (ClassMethodList.Length);
			foreach (var str in ClassMethodList)
				cout.WriteString(str);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			var plen = cin.ReadInt32();
			var plist = new string[plen];
			for (int i = 0; i < plen; i++)
				plist [i] = cin.ReadString ();

			var mlen = cin.ReadInt32();
			var mlist = new string[mlen];
			for (int i = 0; i < mlen; i++)
				mlist [i] = cin.ReadString ();

			var clen = cin.ReadInt32();
			var clist = new string[clen];
			for (int i = 0; i < clen; i++)
				clist [i] = cin.ReadString ();

			PropertyList = plist;
			MethodList = mlist;
			ClassMethodList = clist;
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/ctrl/CLRTemplateReqMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.ctrl
{
	/// <summary>
	/// CLR Call Method message.
	/// </summary>
	public class CLRTemplateReqMessage : CLRMessage
	{
		public CLRTemplateReqMessage ()
			: base (TypeTemplateReq)
		{
		}

		public CLRTemplateReqMessage (string classname)
			: base (TypeTemplateReq)
		{
			ClassName = classname;
		}


		// Properties

		public string ClassName
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			cout.WriteString (ClassName);
		}

		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			ClassName = cin.ReadString();
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRBoolArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR bool array message.
	/// </summary>
	public class CLRBoolArrayMessage : CLRMessage
	{
		public CLRBoolArrayMessage ()
			: base (TypeBoolArray)
		{
		}

		public CLRBoolArrayMessage (bool[] value, int len = -1)
			: base (TypeBoolArray)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public bool[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteByte ((byte)(Value[i] ? 1 : 0));
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new bool[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = cin.ReadByte() != 0;
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRBoolMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR bool message.
	/// </summary>
	public class CLRBoolMessage : CLRMessage
	{
		public CLRBoolMessage ()
			: base (TypeBool)
		{
		}

		public CLRBoolMessage (bool value)
			: base (TypeBool)
		{
			Value = value;
		}


		// Properties

		public bool Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteByte ((byte)(Value ? 1 : 0));
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadByte() != 0;
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRByteArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR byte array message.
	/// </summary>
	public class CLRByteArrayMessage : CLRMessage
	{
		public CLRByteArrayMessage ()
			: base (TypeByteArray)
		{
		}

		public CLRByteArrayMessage (byte[] value, int len = -1)
			: base (TypeByteArray)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public byte[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteByte (Value[i]);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new byte[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = (byte)cin.ReadByte();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRByteMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR byte message.
	/// </summary>
	public class CLRByteMessage : CLRMessage
	{
		public CLRByteMessage ()
			: base (TypeByte)
		{
		}

		public CLRByteMessage (byte value)
			: base (TypeByte)
		{
			Value = value;
		}


		// Properties

		public byte Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteByte (Value);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = (byte)cin.ReadByte();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRExceptionMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR exception message.
	/// </summary>
	public class CLRExceptionMessage : CLRMessage
	{
		public CLRExceptionMessage ()
			: base (TypeException)
		{
		}

		public CLRExceptionMessage (object exception)
			: base (TypeException)
		{
			Message = exception.ToString();
		}


		// Properties

		public string Message
			{ get; private set; }


		// Serialization

		/// <summary>
		/// Provide as an exception
		/// </summary>
		/// <returns>The exception.</returns>
		public Exception ToException ()
		{
			return new Exception (Message);
		}

		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteString (Message, Encoding.ASCII);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Message = cin.ReadString();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRInt32ArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR int32 array message.
	/// </summary>
	public class CLRInt32ArrayMessage : CLRMessage
	{
		public CLRInt32ArrayMessage ()
			: base (TypeInt32Array)
		{
		}

		public CLRInt32ArrayMessage (int[] value, int len = -1)
			: base (TypeInt32Array)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public int[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteInt32 (Value[i]);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new int[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = cin.ReadInt32();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRInt32Message.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR int32 message.
	/// </summary>
	public class CLRInt32Message : CLRMessage
	{
		public CLRInt32Message ()
			: base (TypeInt32)
		{
		}

		public CLRInt32Message (int value)
			: base (TypeInt32)
		{
			Value = value;
		}


		// Properties

		public int Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Value);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadInt32();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRInt64ArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR int64 array message.
	/// </summary>
	public class CLRInt64ArrayMessage : CLRMessage
	{
		public CLRInt64ArrayMessage ()
			: base (TypeInt64Array)
		{
		}

		public CLRInt64ArrayMessage (long[] value, int len = -1)
			: base (TypeInt64Array)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public long[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteInt64 (Value[i]);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new long[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = cin.ReadInt64();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRInt64Message.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR int64 message.
	/// </summary>
	public class CLRInt64Message : CLRMessage
	{
		public CLRInt64Message ()
			: base (TypeInt64)
		{
		}

		public CLRInt64Message (long value)
			: base (TypeInt64)
		{
			Value = value;
		}


		// Properties

		public long Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt64 (Value);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadInt64();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRMatrixMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR matrix message.
	/// </summary>
	public class CLRMatrixMessage : CLRMessage
	{
		public CLRMatrixMessage ()
			: base (TypeMatrix)
		{
		}

		public CLRMatrixMessage (Matrix<double> matrix)
			: base (TypeMatrix)
		{
			Value = matrix;
		}


		// Properties

		public Matrix<double> Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			var rindices = MatrixUtils.RowIndicesOf (Value);
			var cindices = MatrixUtils.ColIndicesOf (Value);

			if (rindices != null)
			{
				var indices = rindices.NameList;
				cout.WriteInt32 (indices.Length);
				for (int i = 0 ; i < indices.Length ; i++)
					cout.WriteString (indices[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);
			
			if (cindices != null)
			{
				var indices = cindices.NameList;
				cout.WriteInt32 (indices.Length);
				for (int i = 0 ; i < indices.Length ; i++)
					cout.WriteString (indices[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);

			cout.WriteInt32 (Value.RowCount);
			cout.WriteInt32 (Value.ColumnCount);

			for (int ci = 0 ; ci < Value.ColumnCount ; ci++)
			{
				for (int ri = 0 ; ri < Value.RowCount ; ri++)
				{
					cout.WriteDouble (Value[ri,ci]);
				}
			}

		}


		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			IndexByName<string> rindex = null;
			IndexByName<string> cindex = null;

			var ridxlen = cin.ReadInt32();
			if (ridxlen > 0)
			{
				rindex = new IndexByName<string> ();
				for (int i = 0 ; i < ridxlen ; i++)
					rindex.Add (cin.ReadString());
			}
			
			var cidxlen = cin.ReadInt32();
			if (cidxlen > 0)
			{
				cindex = new IndexByName<string> ();
				for (int i = 0 ; i < cidxlen ; i++)
					cindex.Add (cin.ReadString());
			}

			var rows = cin.ReadInt32();
			var cols = cin.ReadInt32();
			Value = new IndexedMatrix (rows, cols, rindex, cindex);

			for (int ci = 0 ; ci < Value.ColumnCount ; ci++)
			{
				for (int ri = 0 ; ri < Value.RowCount ; ri++)
				{
					Value[ri,ci] = cin.ReadDouble();
				}
			}
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRNullMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR null-value message.
	/// </summary>
	public class CLRNullMessage : CLRMessage
	{
		public CLRNullMessage ()
			: base (TypeNull)
		{
		}


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRObjectArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR object array message.
	/// 
	/// Object array can handle values of any type, including primitive values
	/// </summary>
	public class CLRObjectArrayMessage : CLRMessage
	{
		public CLRObjectArrayMessage ()
			: base (TypeObjectArray)
		{
		}

		public CLRObjectArrayMessage (object[] value, int len = -1)
			: base (TypeObjectArray)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public object[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
			{
				var obj = Value[i];
				CLRMessage.SerializeValue (cout, obj);
			}
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new object[Length];
			 
			for (int i = 0 ; i < Length ; i++)
			{
				Value[i] = CLRMessage.DeserializeValue (cin);
			}
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRObjectMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR object message.
	/// </summary>
	public class CLRObjectMessage : CLRMessage
	{
		public CLRObjectMessage ()
			: base (TypeObject)
		{
		}

		public CLRObjectMessage (CLRObjectProxy proxy)
			: base (TypeObject)
		{
			ObjectId = proxy.ObjectId;
			ClassName = proxy.ClassName;
		}
		
		public CLRObjectMessage (object obj)
			: base (TypeObject)
		{
			var proxy = CLRObjectProxy.ProxyFor (obj);
			ObjectId = proxy.ObjectId;
			ClassName = proxy.ClassName;
		}


		// Properties

		public int ObjectId
			{ get; private set; }

		public string ClassName
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (ObjectId);
			cout.WriteBool (true);
			cout.WriteString (ClassName);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			ObjectId = cin.ReadInt32();
			if (cin.ReadBoolean())
				ClassName = cin.ReadString ();
		}


		/// <summary>
		/// Converts to local object
		/// </summary>
		/// <returns>The object.</returns>
		public object ToObject ()
		{
			object obj = null;
			if (CLRObjectProxy.TryFindObject (ObjectId, out obj))
				return obj;
			else
				return new CLRObjectProxy (ObjectId);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRReal64ArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR real64 array message.
	/// </summary>
	public class CLRReal64ArrayMessage : CLRMessage
	{
		public CLRReal64ArrayMessage ()
			: base (TypeReal64Array)
		{
		}

		public CLRReal64ArrayMessage (double[] value, int len = -1)
			: base (TypeReal64Array)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public double[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteDouble (Value[i]);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new double[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = cin.ReadDouble();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRReal64Message.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR real 64 message.
	/// </summary>
	public class CLRReal64Message : CLRMessage
	{
		public CLRReal64Message ()
			: base (TypeReal64)
		{
		}

		public CLRReal64Message (double value)
			: base (TypeReal64)
		{
			Value = value;
		}


		// Properties

		public double Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteDouble (Value);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadDouble();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRStringArrayMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR string array message.
	/// </summary>
	public class CLRStringArrayMessage : CLRMessage
	{
		public CLRStringArrayMessage ()
			: base (TypeStringArray)
		{
		}

		public CLRStringArrayMessage (string[] value, int len = -1)
			: base (TypeStringArray)
		{
			Value = value;
			Length = len >= 0 ? len : Value.Length;
		}


		// Properties

		public string[] Value
			{ get; private set; }

		public int Length
			{ get; set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteInt32 (Length);

			for (int i = 0 ; i < Length ; i++)
				cout.WriteString (Value[i], Encoding.ASCII);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Length = cin.ReadInt32();
			Value = new string[Length];
			 
			for (int i = 0 ; i < Length ; i++)
				Value[i] = cin.ReadString();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRStringMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR string message.
	/// </summary>
	public class CLRStringMessage : CLRMessage
	{
		public CLRStringMessage ()
			: base (TypeString)
		{
		}

		public CLRStringMessage (string value)
			: base (TypeString)
		{
			Value = value;
		}


		// Properties

		public string Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);
			cout.WriteString (Value, Encoding.ASCII);
		}
		
		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			Value = cin.ReadString();
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/bridge/server/data/CLRVectorMessage.cs
// -------------------------------------------
﻿// 
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



namespace bridge.server.data
{
	/// <summary>
	/// CLR vector message.
	/// </summary>
	public class CLRVectorMessage : CLRMessage
	{
		public CLRVectorMessage ()
			: base (TypeVector)
		{
		}

		public CLRVectorMessage (Vector<double> vector)
			: base (TypeVector)
		{
			Value = vector;
		}


		// Properties

		public Vector<double> Value
			{ get; private set; }


		// Serialization
		
		/// <summary>
		/// Serialize the message.
		/// </summary>
		/// <param name="cout">Cout.</param>
		public override void Serialize (IBinaryWriter cout)
		{
			base.Serialize (cout);

			var indices = MatrixUtils.IndicesOf (Value);
			if (indices != null)
			{
				var namelist = indices.NameList;
				cout.WriteInt32 (namelist.Length);
				for (int i = 0 ; i < namelist.Length ; i++)
					cout.WriteString (namelist[i], Encoding.ASCII);
			} else
				cout.WriteInt32 (0);

			cout.WriteInt32 (Value.Count);
			for (int i = 0 ; i < Value.Count ; i++)
				cout.WriteDouble (Value[i]);
		}


		/// <summary>
		/// Deserialize the message (assumes magic & type already read in)
		/// </summary>
		/// <param name="cin">Cin.</param>
		public override void Deserialize (IBinaryReader cin)
		{
			var ridxlen = cin.ReadInt32();
			IndexByName<string> rindex = null;

			if (ridxlen > 0)
			{
				rindex = new IndexByName<string> ();
				for (int i = 0 ; i < ridxlen ; i++)
					rindex.Add (cin.ReadString());
			}

			var count = cin.ReadInt32();
			Value = new IndexedVector (count, rindex);

			for (int i = 0 ; i < count ; i++)
				Value[i] = cin.ReadDouble();
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/BlockingQueue.cs
// -------------------------------------------
﻿// 
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





namespace src.common.collections
{
	/// <summary>
	/// Blocking queue.
	/// </summary>
	public class BlockingQueue<T>
	{
		public BlockingQueue (int size)
		{
			_queue = new Queue<T> (size);
		}

		public BlockingQueue ()
		{
			_queue = new Queue<T> ();
		}


		// Properties

		public int Count
			{ get { return _queue.Count; } }


		// Operations

		/// <summary>
		/// Enqueue the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Enqueue (T item)
		{
			lock (_queue)
			{
				_queue.Enqueue (item);

				if (_queue.Count == 1)
					Monitor.PulseAll (_queue);
			}
		}


		/// <summary>
		/// Dequeue next item or wait until becomes available
		/// </summary>
		public T Dequeue ()
		{
			lock (_queue)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait (_queue);
				}

				return _queue.Dequeue ();
			}
		}
		

		/// <summary>
		/// Peek at next item or wait until becomes available
		/// </summary>
		public T Peek ()
		{
			lock (_queue)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait (_queue);
				}

				return _queue.Peek ();
			}
		}

		
		/// <summary>
		/// Dequeue next item if available, otherwise return false
		/// </summary>
		public bool TryDequeue (out T value)
		{
			lock (_queue)
			{
				if (_queue.Count > 0)
				{
					value = _queue.Dequeue ();
					return true;
				}
				else
				{
					value = default(T);
					return false;
				}
			}
		}


		// Variables

		private readonly Queue<T>	_queue;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/Collections.cs
// -------------------------------------------
﻿ 

namespace bridge.common.collections
{
	/// <summary>
	/// Collection utilities
	/// </summary>
	public static class Collections
	{
		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a list
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static List<T> Collect<T,V> (IEnumerable<V> src, Func<V,T> f)
		{
			var list = new List<T>();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}

		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a list
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static HashSet<T> CollectSet<T,V> (IEnumerable<V> src, Func<V,T> f)
		{
			var list = new HashSet<T>();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}

		/// <summary>
		/// Collect from the specified src, applying a function for insertion into a collection
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		/// <typeparam name='T'>
		/// The list element type
		/// </typeparam>
		/// <typeparam name='V'>
		/// The source element type
		/// </typeparam>
		public static C CollectInto<C,V,T> (IEnumerable<V> src, Func<V,T> f) where C : ICollection<T>, new()
		{
			var list = new C();
			foreach (var v in src)
				list.Add (f(v));
			
			return list;
		}
		

		/// <summary>
		/// Maps across enumerable, creating matrix, mapping function provides vector for each row
		/// </summary>
		/// <param name="src">Source.</param>
		/// <param name="mapping">Mapping.</param>
		public static Matrix<double> MapToMatrixByRow<V> (IEnumerable<V> src, Func<V,Vector<double>> mapping)
		{
			var tmpl = mapping (First (src));
			var nrows = Length (src);
			var ncols = tmpl.Count;

			IIndexByName index = null; 
			if (tmpl is IndexedVector)
				index = ((IndexedVector)tmpl).Indices;

			var mat = new IndexedMatrix (nrows, ncols, null, index);
			var ri = 0;
			foreach (var v in src)
			{
				var rvec = mapping (v);
				if (rvec.Count != ncols)
					throw new ArgumentException ("ncols does not match mapping function vector size");

				for (int ci = 0; ci < ncols; ci++)
					mat [ri, ci] = rvec [ci];

				ri++;
			}

			return mat;
		}
		

		/// <summary>
		/// Maps across enumerable, creating matrix, mapping function provides vector for each row
		/// </summary>
		/// <param name="src">Source.</param>
		/// <param name="mapping">Mapping.</param>
		public static Matrix<double> MapToMatrixByCol<V> (IEnumerable<V> src, Func<V,Vector<double>> mapping)
		{
			var tmpl = mapping (First (src));
			var ncols = Length (src);
			var nrows = tmpl.Count;

			IIndexByName index = null; 
			if (tmpl is IndexedVector)
				index = ((IndexedVector)tmpl).Indices;

			var mat = new IndexedMatrix (nrows, ncols, index, null);
			var ci = 0;
			foreach (var v in src)
			{
				var cvec = mapping (v);
				if (cvec.Count != nrows)
					throw new ArgumentException ("nrows does not match mapping function vector size");

				for (int ri = 0; ri < nrows; ri++)
					mat [ri, ci] = cvec [ri];

				ci++;
			}

			return mat;
		}

		
		/// <summary>
		/// Finds the first element of an enumeration matching predicate
		/// </summary>
		/// <returns>
		/// The matching value or null
		/// </returns>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='predicate'>
		/// Predicate.
		/// </param>
		public static V FindOne<V> (IEnumerable<V> src, Predicate<V> predicate)
		{
			foreach (var v in src)
				if (predicate (v)) return v;
			
			return default(V);
		}
		
		/// <summary>
		/// Gets the 1st element
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		public static V First<V> (IEnumerable<V> src)
		{
			foreach (var v in src)
				return v;
			
			return default(V);
		}
		
		/// <summary>
		/// Gets the ith element
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='ith'>
		/// index into pivots
		/// </param>
		public static V Ith<V> (IEnumerable<V> src, int ith)
		{
			foreach (var v in src)
			{
				if (--ith <= 0)
					return v;
			}

			return default(V);
		}


		/// <summary>
		/// Gets the length of the sequence
		/// </summary>
		/// <param name='src'>
		/// Source collection
		/// </param>
		public static int Length<V> (IEnumerable<V> src)
		{
			var count = 0;
			foreach (var v in src)
				count++;

			return count;
		}

		
		/// <summary>
		/// Finds the all elements of an enumeration matching predicate
		/// </summary>
		/// <returns>
		/// The matching values
		/// </returns>
		/// <param name='src'>
		/// Source collection
		/// </param>
		/// <param name='predicate'>
		/// Predicate.
		/// </param>
		public static IList<V> FindAll<V> (IEnumerable<V> src, Predicate<V> predicate)
		{
			var list = new List<V>();
			foreach (var v in src)
				if (predicate (v)) list.Add(v);
			
			return list;
		}
		
		
		/// <summary>
		/// Maximum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static int Max<V> (IEnumerable<V> src, Func<V,int> f)
		{
			var Vmax = int.MinValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv > Vmax)
					Vmax = iv;
			}
			
			return Vmax;
		}
		
		
		/// <summary>
		/// Maximum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static double Max<V> (IEnumerable<V> src, Func<V,double> f)
		{
			var Vmax = double.MinValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv > Vmax)
					Vmax = iv;
			}
			
			return Vmax;
		}
		
		
		/// <summary>
		/// Minumum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static int Min<V> (IEnumerable<V> src, Func<V,int> f)
		{
			var Vmin = int.MaxValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv < Vmin)
					Vmin = iv;
			}
			
			return Vmin;
		}
		
		
		/// <summary>
		/// Minumum across specified src, applying a function
		/// </summary>
		/// <param name='src'>
		/// Source sequence
		/// </param>
		/// <param name='f'>
		/// Function to apply to each element, emitting the value to be collected into a list
		/// </param>
		public static double Min<V> (IEnumerable<V> src, Func<V,double> f)
		{
			var Vmin = double.MaxValue;
			foreach (var v in src)
			{
				var iv = f(v);
				if (iv < Vmin)
					Vmin = iv;
			}
			
			return Vmin;
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/Comparators.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Comparison delegate, returns {-1, 0, 1} to indicate <, = , > relationships
	/// </summary>
	public delegate int Compare<in T> (T a, T b);
	
	
	/// <summary>
	/// Compare delegate implementations
	/// </summary>
	public static class Comparisons
	{
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (int a, int b)
		{
			return a - b;
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (int a, int b)
		{
			return b - a;
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (long a, long b)
		{
			if (a > b)
				return 1;
			if (b < a)
				return -1;
			else
				return 0;
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (long a, long b)
		{
			if (a > b)
				return -1;
			if (b < a)
				return 1;
			else
				return 0;
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompare (string a, string b)
		{
			return a.CompareTo(b);
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompare (string a, string b)
		{
			return b.CompareTo(a);
		}
		
		
		/// <summary>
		/// Compare in (normal) ascending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int AscendingCompareIgnoreCase (string a, string b)
		{
			return string.Compare (a, b, StringComparison.OrdinalIgnoreCase);
		}
		
		
		/// <summary>
		/// Compare in (reverse) descending order
		/// </summary>
		/// <param name='a'>
		/// A.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		public static int DescendingCompareIgnoreCase (string a, string b)
		{
			return string.Compare (b, a, StringComparison.OrdinalIgnoreCase);
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/LinkedNode.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Node for a list or queue
	/// </summary>
	public class LinkedNode<Node>
	{
		public Node Prior
			{ get; set; }
		
		public Node Next
			{ get; set; }
	}		
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/LinkedQueue.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Queue that uses exposed nodes, allowing for lower-level manipulation
	/// </summary>
	public class LinkedQueue<Node> : IEnumerable<Node> where Node : LinkedNode<Node>, new()
	{		
		// Properties
		
		public Node Front
			{ get { return _front; } }
		
		public Node Back
			{ get { return _back; } }
		
		public int Count
			{ get { return _count; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Append value node to list
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual Node Append (Node node)
		{
			if (_back != null)
			{
				node.Prior = _back;
				_back.Next = node;
				_back = node;
			}
			else
			{
				_back = node;
				_front = node;
			}
			
			_count++;
			return node;
		}
		
		
		/// <summary>
		/// Append new node to list and return ref
		/// </summary>
		public virtual Node Append ()
		{
			return Append (NodePool<Node>.Alloc());
		}
		
		
		/// <summary>
		/// Prepend the specified node.
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual Node Prepend (Node node)
		{
			if (_front != null)
			{
				node.Next = _front;
				_front.Prior = node;
				_front = node;
			}
			else
			{
				_back = node;
				_front = node;
			}
			
			_count++;
			return node;
		}


		/// <summary>
		/// Move given node to front of queue
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual void MoveToFront (Node node)
		{
			if (_front == node)
				return;
			if (_front == null)
				throw new ArgumentException ("cannot move a node in the queue when does not belong to the queue");

			if (node == _back)
				_back = node.Prior;
			if (node.Prior != null)
				node.Prior.Next = node.Next;
			if (node.Next != null)
				node.Next.Prior = node.Prior;

			node.Next = _front;
			_front.Prior = node;
			_front = node;
		}


		/// <summary>
		/// Move given node to back of queue
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual void MoveToBack (Node node)
		{
			if (_back == node)
				return;
			if (_back == null)
				throw new ArgumentException ("cannot move a node in the queue when does not belong to the queue");

			if (node == _front)
				_front = node.Next;
			if (node.Prior != null)
				node.Prior.Next = node.Next;
			if (node.Next != null)
				node.Next.Prior = node.Prior;

			node.Next = _front;
			_front.Prior = node;
			_front = node;
		}

		
		/// <summary>
		/// Prepend the a new node and return ref
		/// </summary>
		public virtual Node Prepend ()
		{
			return Prepend (NodePool<Node>.Alloc());
		}	
		
		
		/// <summary>
		/// insert node after given
		/// </summary>
		/// <param name='prior'>
		/// Node to insert after
		/// </param>
		/// <param name='newnode'>
		/// New node to insert
		/// </param>
		public virtual Node Insert (Node prior, Node newnode)
		{
			var next = prior.Next;
			
			newnode.Next = next;
			newnode.Prior = prior;
			prior.Next = newnode;
	
			if (_back == prior)
				_back = newnode;
			
			if (next != null)
				next.Prior = newnode;
			
			_count++;
			return newnode;
		}
		
		
		/// <summary>
		/// insert new node after given prior node, returning ref to new node
		/// </summary>
		/// <param name='prior'>
		/// Node to insert after
		/// </param>
		public virtual Node Insert (Node prior)
		{
			return Insert (prior, NodePool<Node>.Alloc());
		}
		
		
		/// <summary>
		/// Remove the specified node 
		/// <p/>
		/// Note that the node is invalidated after this call, in fact may be allocated to a new use
		/// </summary>
		/// <param name='node'>
		/// node to be removed
		/// </param>
		public virtual bool Remove (Node node)
		{
			bool removed = false;
			if (node == _front)
				{ _front = node.Next; removed = true; }
			if (node == _back)
				{ _back = node.Prior; removed = true; }
			
			if (node.Prior != null)
				{ node.Prior.Next = node.Next; removed = true; }
			if (node.Next != null)
				{ node.Next.Prior = node.Prior; removed = true; }
			
			node.Prior = null;
			node.Next = null;
			
			if (removed)
			{
				_count--;
				NodePool<Node>.Free (node);
			}
			
			return removed;
		}
				
		
		/// <summary>
		/// Clear list
		/// <p/>
		/// Note that all nodes are invalidated after this call, in fact may be allocated to a new use
		/// </summary>
		public virtual void Clear ()
		{
			for (Node node = _front ; node != null ; )
			{
				var next = node.Next;
				NodePool<Node>.Free (node);					
				node = next;
			}
			
			_front = null;
			_back = null;
			_count = 0;
		}
	
		
		// Meta
		
		
		public System.Collections.IEnumerator GetEnumerator ()
		{
			for (Node  node = _front ; node != null ; node = node.Next)
				yield return node;
		}
		
		IEnumerator<Node> IEnumerable<Node>.GetEnumerator ()
		{
			for (Node node = _front ; node != null ; node = node.Next)
				yield return node;
		}

		public override string ToString ()
		{
			StringBuilder s = new StringBuilder(1024);
			s.Append("[");
			
			int i = 0;
			for (Node node = _front ; node != null ; node = node.Next)
			{
				if (i++ > 0)
					s.Append(", ");
				
				s.Append(node.ToString());
			}
			
			return s.ToString();			 
		}
		
		// Variables
		
		private Node 		_front;
		private Node 		_back;
		private int			_count;
	}
	
	
	
	/// <summary>
	/// OrderInfo pool allocator
	/// </summary>
	public static class NodePool<Node> where Node : LinkedNode<Node>, new()
	{
		/// <summary>
		/// Create an node
		/// </summary>
		public static Node Alloc ()
		{
			var allocator = _global.Value;
			var node = allocator.Alloc ();
			return node;
		}
		
		
		/// <summary>
		/// Release a node
		/// </summary>
		/// <param name='node'>
		/// node to be released.
		/// </param>
		public static void Free (Node node)
		{
			node.Prior = null;
			node.Next = null;	
			
			var allocator = _global.Value;
			allocator.Free (node);
		}

				
		// Variables
		
		static ThreadLocal<STObjectPool<Node>>
			_global = new ThreadLocal<STObjectPool<Node>> (
				() => new STObjectPool<Node>(1024, () => new Node()));
	}
	
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/LRUCache.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Resource usage delegate.  Measures memory, instance, or other resources
	/// </summary>
	public delegate long ResourceUsage<T> (T obj); 
	
	/// <summary>
	/// Resource usage delegate.  Measures memory, instance, or other resources
	/// </summary>
	public delegate void Disposal<T> (T obj); 


	/// <summary>
	/// LRU cache that uses approximate resource usage to decide when to retire elements
	/// </summary>
	public class LRUCache<K,T> : IEnumerable<KeyValuePair<K,T>>
	{
		/// <summary>
		/// Create LRU cache
		/// </summary>
		/// <param name='measure'>
		/// Resource measurement delegate applied to each element
		/// </param>
		/// <param name='maxresource'>
		/// Maximum resource utilization allowed, after which will trim back on an LRU basis
		/// </param>
		public LRUCache (
			ResourceUsage<T> measure, 
			long maxresource,
			Disposal<T> disposal = null)
		{
			_measure = measure;
			_maxresource = maxresource;
			_disposal = disposal;
			_mru = new LinkedQueue<Pair<K,T>> ();;
		}
		
		
		// Properties
		
		public long CurrentUtilization
			{ get { return _curresource; } }
		
		public long MaxUtilization
			{ get { return _maxresource; } set { _maxresource = value; } }

		
		// Accessors
		
		
		/// <summary>
		/// Access element in cache by key (retrieving or setting)
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public T this[K key]
		{
			get 
			{
				Pair<K,T> node = null;
				if (!_cache.TryGetValue(key, out node))
					return default(T); 
						
				Touch(node);
				return node.Obj;
			}
			set 
			{
				Pair<K,T> node = null;
				if (_cache.TryGetValue(key, out node))
				{
					node.Obj = value;
					Touch(node);
				}
				else
				{
					New (key, value);
					Prune();
				}
			}
		}
		
		
		/// <summary>
		/// Remove object associated with specific key
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public bool Remove (K key)
		{
			Pair<K,T> node = null;
			if (!_cache.TryGetValue(key, out node))
				return false; 

			var obj = node.Obj;
			_cache.Remove(key);
			_mru.Remove (node);
			
			if (_disposal != null)
				_disposal(obj);
			
			return true;
		}
		
		
		/// <summary>
		/// Clear the cache
		/// </summary>
		public void Clear ()
		{
			foreach (Pair<K,T> node in _mru)
			{
				if (_disposal != null)
					_disposal (node.Obj);
			}

			// clear
			_cache.Clear();
			_mru .Clear();
			_curresource = 0;
		}
		
		
		// Meta

		
		public IEnumerator<KeyValuePair<K,T>> GetEnumerator ()
		{
			foreach (Pair<K,T> node in _mru)
				yield return new KeyValuePair<K, T> (node.Key, node.Obj);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (Pair<K,T> node in _mru)
				yield return new KeyValuePair<K, T> (node.Key, node.Obj);
		}
		
		
		#region Implementation

		
		private void Touch (Pair<K,T> node)
		{				
			_mru.MoveToFront (node);
		}
		
		
		private Pair<K,T> New (K key, T obj)
		{
			var node = new Pair<K,T> { Key = key, Obj = obj };

			_mru.Prepend (node);
			_cache [key] = node;

			_curresource += _measure(obj);
			return node;
		}
		
		
		/// <summary>
		/// Remove objects in LRU order until resource utilization satisfied, however preserving at least one entry
		/// </summary>
		private void Prune ()
		{
			if (_curresource <= _maxresource || _cache.Count == 0)
				return;

			var node = _mru.Back;
			while (node != null && _curresource > _maxresource)
			{
				var pnode = node.Prior;
				var obj = node.Obj;
				var key = node.Key;

				// adjust resource utilization down
				_curresource -= _measure(obj);

				// remove from map
				_cache.Remove (key);
				// remove node
				_mru.Remove (node);

				// dispose
				if (_disposal != null)
					_disposal (obj);

				// setup for next							
				node = pnode;
			}

			if (node == null)
				_curresource = 0;
		}


		#endregion

		
		// Variables
		
		private ResourceUsage<T>			_measure;
		private long						_maxresource;
		private long						_curresource;
		private Disposal<T> 				_disposal;
		
		private IDictionary<K,Pair<K,T>>	_cache = new Dictionary<K,Pair<K,T>>();
		private LinkedQueue<Pair<K,T>>		_mru;
	}
	
	
	/// <summary>
	/// LRU linked list node
	/// </summary>
	sealed class Pair<K,T> : LinkedNode<Pair<K,T>>
	{
		public K		Key;
		public T		Obj;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/PriorityQueue.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Priority queue, implemented as a binary heap.  Provides the following:
	/// <list>
	/// 	<item>enqueue		[enqueues an element such that an element can be later dequeued in priority order]</item>
	/// 	<item>dequeue		[dequeues element from heap with the highest priority]</item>
	/// 	<item>remove		[removes an element out of order]</item>
	/// </list>
	/// <p/>
	/// A delegate providing priorities for object put on the queue must be provided in construction.  
	/// Priorities must be invariant, or if variant must be removed and re-inserted when priority changes.
	/// <p/>
	/// Higher #s are considered to be higher priority and can be positive or negative in value.
	/// </summary>
	public class PriorityQueue<T> where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.common.collections.PriorityQueue`1"/> class.
		/// </summary>
		/// <param name="priority">Priority function</param>
		public PriorityQueue (Func<T,double> priority)
		{
			_queue = new T[512];
			_priority = priority;
			_count = 0;
		}
		
		
		// Properties
		
		public int Count
			{ get { return _count; } }


		// Functions
		
		
		/// <summary>
		/// Enqueue prioritized object
		/// <p/>
		/// Item will be retrieved with dequeue not in FIFO order rather priority
		/// order.  Highest priority objects will be dequeued sooner.
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Enqueue (T item)
		{
			_count++;
			if (_count >= _queue.Length)
				Grow ();
			
			_queue[_count-1] = item;
			SettleUpward (item, _count);
		}
		
		
		/// <summary>
		/// Dequeue highest priority item from queue
		/// <p/>
		/// Item is removed from the queue such that the next highest priority item will
		/// be retrieved in the next call to Dequeue().
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when one tries to dequeue on an empty queue
		/// </exception>
		public T Dequeue ()
		{
			if (_count == 0)
				throw new ArgumentException ("Dequeue: queue empty");
	
			// get leafmost node & adjust queue size
			T top = _queue[0];
			T leafy = _queue[--_count];
			_queue[0] = leafy;
			_queue[_count] = null;

			// adjust placement in queue
			if (_count > 1)
				SettleDownward (leafy, 1);
	
			return top;			
		}
		
		
		/// <summary>
		/// Dequeue highest priority item from queue
		/// <p/>
		/// Item is removed from the queue such that the next highest priority item will
		/// be retrieved in the next call to Dequeue().
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when one tries to dequeue on an empty queue
		/// </exception>
		public bool TryDequeue (out T item)
		{
			if (_count == 0)
			{
				item = default(T);
				return false;
			}
	
			// get leafmost node & adjust queue size
			T top = _queue[0];
			T leafy = _queue[--_count];
			_queue[0] = leafy;
			_queue[_count] = null;
	
			// adjust placement in queue
			if (_count > 1)
				SettleDownward (leafy, 1);
	
			item = top;
			return true;
		}
		
		
		/// <summary>
		/// Peek at top of queue
		/// </summary>
		/// <exception cref='ArgumentException'>
		/// Is thrown when called on empty queue
		/// </exception>
		public T Peek ()
		{
			if (_count > 0)
				return _queue[0];
			else
				throw new ArgumentException ("Dequeue: queue empty");
		}
		
		
		/// <summary>
		/// Peek at top of queue
		/// </summary>
		public bool TryPeek (out T item)
		{
			if (_count > 0)
			{
				item = _queue[0];
				return true;
			} 
			else
			{
				item = null;
				return false;
			}
		}
		
		
		/// <summary>
		/// Clear the queue
		/// </summary>
		public void Clear ()
		{
			for (int i = 0 ; i < _count ; i++)
				_queue[i] = null;
			
			_count = 0;
		}
		
		
		/// <summary>
		/// Remove an item matching given
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public bool Remove (T item)
		{
			int pos = FindIndexOf (item);
			if (pos < 0)
				return false;
				
			// get leafmost node & adjust queue size
			T leafy = _queue[--_count];
			_queue[pos] = leafy;
			_queue[_count] = null;
			
			// if item removed is leaf, nothing to do
			if (pos == _count)
				return true;;
	
			// otherwise, settle leaf into deleted spot and adjust into heap, in [1..n] space
			if (_count > 0)
				Settle (leafy, pos+1);
			
			return true;
		}
		
		
		// Implementation
		
		
		/// <summary>
		/// Resettle object in queue (either upwards or downwards)
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// place to start settling from, in [1..n] space.
		/// </param>
		private void Settle (T obj, int start)
		{
			// get parent
			T parent = (start > 1) ? 
				_queue[start/2] : null;
	
			// get priorities
			var Tpriority = _priority (obj);
			var Ppriority = (parent != null) ? _priority(parent) : double.MaxValue;
	
			if (Tpriority > Ppriority)
				SettleUpward (obj, start);
			else
				SettleDownward (obj, start);
		}
	
	
		/// <summary>
		/// Resettle object in queue downwards; assumes object is smaller than parent at starting position
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// start place to start settling from, in [1..n] space.
		/// </param>
		private void SettleDownward (T obj, int start)
		{
			// get size
			var size = _count;
			int i = start;
	
			var Tpriority = _priority(obj);
		
			// find position downwards
			while (i < size)
			{
				int ci = i*2;
	
				// get children
				T childA = (size >= ci) ? 
					_queue[ci-1] : default(T);
				T childB = (size > ci) ? 
					_queue[ci+1-1] : default(T);
	
				// get priorities
				var Apriority = (childA != null) ? _priority(childA) : double.MinValue;
				var Bpriority = (childB != null) ? _priority(childB) : double.MinValue;
	
				// determine whether current node a proper resting place
				if (Tpriority >= Apriority && Tpriority >= Bpriority)
					{ _queue[i-1] = obj; return; }
	
				// otherwise must swap down with the largest child
				if (Apriority >= Bpriority)
					{ _queue[i-1] = childA;  i = ci; }
				else
					{ _queue[i-1] = childB;  i = ci+1; }
			}
	
			// at right-most leaf
			_queue[i-1] = obj; 
		}
		
		
		/// <summary>
		/// Resettle object in queue upwards; assumes children are smaller than
		/// node at start position
		/// </summary>
		/// <param name='obj'>
		/// object to settle in queue.
		/// </param>
		/// <param name='start'>
		/// place to start settling from, in [1..n] space.
		/// </param>
		private void SettleUpward (T obj, int start)
		{
			// place in at starting position
			_queue[start-1] = obj;
	
			// find proper place for object by moving through heap (upwards towards root)
			for (int i = start ; i > 1 ; i = i / 2)
			{
				// get priority of parent
				T parent = _queue[i/2-1];
				T child = _queue[i-1];
					
				// if parent lower-priority, swap
				if (_priority(child) > _priority(parent))
				{ 
					_queue[i-1] = parent; 
					_queue[i/2-1] = child; 
				} else
					return;
			}
		}
		
		
		private void Grow ()
		{
			
			var nlen = Math.Min (_count * 2, _count + 1024);
			T[] nqueue = new T[nlen];
			Array.Copy (_queue, 0, nqueue, 0, _queue.Length);
			_queue = nqueue;
		}
		
		
		private int FindIndexOf (T obj)
		{
			for (int i = 0 ; i < _count ; i++)
			{
				if (_queue[i].Equals (obj))
					return i;
			}
			
			return -1;
		}
		
		
		// Variables
		
		private T[]				_queue;
		private int				_count;
		private Func<T,double>	_priority;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/SortedList.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.collections
{
	/// <summary>
	/// Sorted list
	/// </summary>
	public class SortedList<V> : IList<V>
	{
		public SortedList (Comparison<V> cmp, int capacity)
		{
			_cmp = cmp;
			_list = new List<V> (capacity);
		}

		
		public SortedList (Comparison<V> cmp)
		{
			_cmp = cmp;
			_list = new List<V> ();
		}
		
		
		// Properties
		
		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		public bool IsReadOnly
			{ get { return false; } } 
		
		/// <summary>
		/// Gets the # of elements in the list
		/// </summary>
		public int Count
			{ get { return _list.Count; } }
		
		
		/// <summary>
		/// Gets the ith element in the list
		/// </summary>
		/// <param name='ith'>
		/// Ith.
		/// </param>
		public V this[int ith]
		{ 
			get 
			{ 
				return _list[ith]; 
			} 
			set 
			{ 
				throw new ArgumentException ("cannot support setting list elements by index, as is a sorted list"); 
			}
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Add the specified item (in sorted order)
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Add (V item)
		{
			if (_list.Count > 0)
			{
				var idx = FindLE (item);
				_list.Insert (idx+1, item);
			} 
			else
			{
				_list.Add (item);
			}
		}
		
		
		/// <summary>
		/// Adds the elements of the specified collection to the end of the list.
		/// </summary>
		/// <param name='collection'>
		/// The collection whose elements are added to the end of the list.
		/// </param>
		public void AddRange (IEnumerable<V> collection)
		{
			_list.AddRange (collection);
			_list.Sort (_cmp);
		}
		
		
		/// <summary>
		/// Clear the list
		/// </summary>
		public void Clear ()
		{
			_list.Clear();
		}
		
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name='item'>
		/// item to determine containment for
		/// </param>
		public bool Contains (V item)
		{
			return _list.Contains(item);
		}
		
		
		/// <summary>
		/// Returns the index of given item
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public int IndexOf (V item)
		{
			return _list.IndexOf (item);
		}
		
		
		/// <summary>
		/// Insert the specified item at index.
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void Insert (int index, V item)
		{
			_list.Insert (index, item);
			_list.Sort (_cmp);
		}
		
		
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name='item'>
		/// item to be removed
		/// </param>
		public bool Remove (V item)
		{
			return _list.Remove (item);
		}
		
		
		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		public void RemoveAt (int index)
		{
			_list.RemoveAt (index);
		}
		
		
		/// <summary>
		/// Gets the sublist starting from index of given length
		/// </summary>
		/// <returns>
		/// The range.
		/// </returns>
		/// <param name='start'>
		/// Start.
		/// </param>
		/// <param name='length'>
		/// Length.
		/// </param>
		public SortedList<V> GetRange (int start, int length)
		{
			var nlist = new SortedList<V> (_cmp, length);
			for (int i = 0 ; i < length ; i++)
				nlist._list[i] = _list[i + start];
			
			return nlist;
		}
		
		
		/// <summary>
		/// Removes the given range
		/// </summary>
		/// <param name='start'>
		/// Start index
		/// </param>
		/// <param name='length'>
		/// Length of range to be removed
		/// </param>
		public void RemoveRange (int start, int length)
		{
			_list.RemoveRange (start, length);
		}
		
		
		// Meta
		
		
		public void CopyTo (V[] array, int arrayIndex)
		{
			_list.CopyTo (array, arrayIndex);
		}
		
		
		public override bool Equals (object obj)
		{
			return _list.Equals (obj);
		}
		
		
		public System.Collections.IEnumerator GetEnumerator ()
		{
			return _list.GetEnumerator();
		}
		
		IEnumerator<V> IEnumerable<V>.GetEnumerator ()
		{
			return _list.GetEnumerator();
		}
		
		public override int GetHashCode ()
		{
			return _list.GetHashCode ();
		}
		
		
		// Implementation
		
		
		
		/// <summary>
		/// Finds the max(v[index]) <= element
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		private int FindLE (V item)
		{
			var Istart = 0;
			var Iend = Count-1;
			var Iguess = 0;
			var cmp =0;
			
			while (Istart < Iend)
			{
				Iguess = Istart + (Iend - Istart) / 2;
				V v = _list[Iguess];
				
				cmp = _cmp (item, v);
				if (cmp == 0)
					return Iguess;
				
				if (Iguess == Istart)
					return Iguess;
				
				if (cmp > 0)
					Istart = Iguess; 
				else
					Iend = Iguess;			
			}
			
			if (cmp > 0)
				return Iguess;
			else
				return -1;
			
		}
		
		
		// Variables
		
		private Comparison<V>		_cmp;
		private List<V>				_list;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/collections/SortStableWithOrdering.cs
// -------------------------------------------
//  
//  	Authors
//  		Jonathan Shore
//  
//  	Copyright:
//  		2012 Systematic Trading LLC 
//  		2002 Systematic Trading LLC
//  
//  		This software is only to be used for the purpose for which
//  		it has been provided.  No part of it is to be reproduced,
//  		disassembled, transmitted, stored in a retrieval system nor
//  		translated in any human or computer language in any way or
//  		for any other purposes whatsoever without the prior written
//  		consent of Systematic Trading LLC
// 
//  
// 

namespace com.stg.common.collections
{
	/// <summary>
	/// This is a stable sort (using merge sort) that also tracks the sort order index
	/// of the resulting sorted values.
	/// <p/>
	/// This is not an in-place sort and requires auxilliary storage for both the values and indices.   Hence
 	/// is presented as a class rather than a function.
 	/// <p/>
 	/// Also note, that because of retained and working state, this class is not thread-safe, if shared by multiple
 	/// threads.  Should be instantiated as thread-local if it is intended to be shared across threads
	/// </summary>
	public class SortStableWithOrdering<V>
	{
		public SortStableWithOrdering (int maxsize, Comparison<V> cmp)
		{
			_tmp_data = new V[maxsize];
			_tmp_indices = new int[maxsize];
			_cmp = cmp;
		}
		
		public SortStableWithOrdering (Comparison<V> cmp)
			: this (256, cmp)
		{
		}
		
		// Functions
		
		
		/// <summary>
		/// Sort the specified data, with data in original form and ordering presenting the sort order
		/// </summary>
		/// <param name='data'>
		/// Data to be sorted
		/// </param>
		/// <param name='ordering'>
		/// Ordering is an array of int with the same dimension as the data to be sorted
		/// </param>
		public void SortOrder (
			V[] data, 
			int[] ordering, 
			int Istart = 0, 
			int Iend = -1)
		{
			if (Iend < 0)
				Iend = data.Length - 1;

			var len = (Iend - Istart + 1);
			Debug.Assert (len == ordering.Length, "ordering must be of the same length as the data to be sorted");
			
			// adjust for size if necessary
			if (data.Length > _tmp_data.Length)
			{
				_tmp_data = new V[len];
				_tmp_indices = new int[len];
			}

			// initial ordering
			for (int i = 0 ; i < len ; i++)
				ordering[i] = Istart + i;

			MergeSortWithOrder (data, ordering, 0, len-1);
		}


		/// <summary>
		/// Sort the specified data
		/// </summary>
		/// <param name='data'>
		/// Data to be sorted
		/// </param>
		public void Sort (
			V[] data, 
			int Istart = 0, 
			int Iend = -1)
		{
			if (Iend < 0)
				Iend = data.Length - 1;

			var len = (Iend - Istart + 1);

			// adjust for size if necessary
			if (len > _tmp_data.Length)
				_tmp_data = new V[len];

			MergeSort (data, Istart, Iend);
		}

		
		#region MergeSort without order-index tracking
		
		
		private void MergeSort (V[] data, int left, int right)
		{
			// if single element (or none), nothing to do
			if (right <= left)
				return;
			
			// create 2 sorted streams: [left, mid] and [mid+1, right]
			var mid = (left + right) / 2;
			MergeSort (data, left, mid);
			MergeSort (data, mid+1, right);
			
			// now merge the 2 sorted streams of [left, mid] and [mid+1, right]
			Merge (data, left, mid+1, right);
		}
		
		
		private void Merge (V[] data, int left, int division, int right)
		{
			var Lptr = left;
			var Rptr = division;
			var Tptr = left;
			
			// select from each sorted sequence
			while (Lptr < division && Rptr <= right)
			{
				var cmp = _cmp (data[Lptr], data[Rptr]);
				if (cmp <= 0)
				{
					_tmp_data[Tptr++] = data[Lptr++];
				}
				else
				{
					_tmp_data[Tptr++] = data[Rptr++];
				}
			}
			
			// cleanup on residual left over in left sequence (if any)
			while (Lptr < division)
			{
				_tmp_data[Tptr++] = data[Lptr++];
			}
			
			// cleanup on residual left over in right sequence (if any)
			while (Rptr <= right)
			{
				_tmp_data[Tptr++] = data[Rptr++];
			}
			
			// now copy back in from tmp arrays into the destination
			Array.Copy (_tmp_data, left, data, left, (right - left + 1));
		}


		#endregion

		#region MergeSort with order-index tracking


		private void MergeSortWithOrder (V[] data, int[] ordering, int left, int right)
		{
			// if single element (or none), nothing to do
			if (right <= left)
				return;

			// create 2 sorted streams: [left, mid] and [mid+1, right]
			var mid = (left + right) / 2;
			MergeSortWithOrder (data, ordering, left, mid);
			MergeSortWithOrder (data, ordering, mid+1, right);

			// now merge the 2 sorted streams of [left, mid] and [mid+1, right]
			MergeWithOrder (data, ordering, left, mid+1, right);
		}


		private void MergeWithOrder (V[] data, int[] ordering, int left, int division, int right)
		{
			var Lptr = left;
			var Rptr = division;
			var Tptr = left;

			// select from each sorted sequence
			while (Lptr < division && Rptr <= right)
			{
				var I_Lptr = ordering [Lptr];
				var I_Rptr = ordering [Rptr];

				var cmp = _cmp (data[I_Lptr], data[I_Rptr]);
				if (cmp <= 0)
				{
					_tmp_indices[Tptr++] = ordering[Lptr++];
				}
				else
				{
					_tmp_indices[Tptr++] = ordering[Rptr++];
				}
			}

			// cleanup on residual left over in left sequence (if any)
			while (Lptr < division)
			{
				_tmp_indices[Tptr++] = ordering[Lptr++];
			}

			// cleanup on residual left over in right sequence (if any)
			while (Rptr <= right)
			{
				_tmp_indices[Tptr++] = ordering[Rptr++];
			}

			// now copy back in from tmp arrays into the destination
			Array.Copy (_tmp_indices, left, ordering, left, (right - left + 1));
		}


		#endregion

		
		// Variables
		
		private V[]				_tmp_data;
		private int[]			_tmp_indices;
		private Comparison<V>	_cmp;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/BitConversions.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.io
{
	/// <summary>
	/// Structure for converting int16
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Int16Union
	{
		public Int16Union (short v)
		{
			b1 = b2 = 0;
			i = unchecked((ushort)v);
		}

		public Int16Union (ushort v)
		{
			b1 = b2 = 0;
			i = v;
		}

		public Int16Union (byte[] buffer, int offset = 0)
		{
			i = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp = b1;
			b1 = b2;
			b2 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public short ToShort ()
		{
			return unchecked((short)i);
		}
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public ushort ToUShort ()
		{
			return i;
		}
		
		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public ushort 	i;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
	}

	
	/// <summary>
	/// Structure for converting int32
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Int32Union
	{
		public Int32Union (int v)
		{
			b1 = b2 = b3 = b4 = 0;
			i = unchecked((uint)v);
		}

		public Int32Union (uint v)
		{
			b1 = b2 = b3 = b4 = 0;
			i = v;
		}

		public Int32Union (byte[] buffer, int offset = 0)
		{
			i = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp = b1;
			b1 = b4;
			b4 = tmp;
			
			tmp = b2;
			b2 = b3;
			b3 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			buffer[2] = b3;
			buffer[3] = b4;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public int ToInt ()
		{
			return unchecked ((int)i);
		}

				
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public uint ToUInt ()
		{
			return i;
		}

		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public uint 	i;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
	}
	
	
	/// <summary>
	/// Structure for converting long
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Long64Union
	{
		public Long64Union (ulong v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			l = v;
		}

		public Long64Union (long v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			l = unchecked((ulong)v);
		}

		public Long64Union (byte[] buffer, int offset = 0)
		{
			l = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
			b5 = buffer[offset + 4];
			b6 = buffer[offset + 5];
			b7 = buffer[offset + 6];
			b8 = buffer[offset + 7];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp;
			
			tmp = b1; b1 = b8; b8 = tmp;			
			tmp = b2; b2 = b7; b7 = tmp;
			tmp = b3; b3 = b6; b6 = tmp;
			tmp = b4; b4 = b5; b5 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer, int offset)
		{
			buffer[offset + 0] = b1;
			buffer[offset + 1] = b2;
			buffer[offset + 2] = b3;
			buffer[offset + 3] = b4;
			buffer[offset + 4] = b5;
			buffer[offset + 5] = b6;
			buffer[offset + 6] = b7;
			buffer[offset + 7] = b8;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to ulong (in local form presumably)
		/// </summary>
		public ulong ToULong ()
		{
			return l;
		}

		/// <summary>
		/// Convert to long (in local form presumably)
		/// </summary>
		public long ToLong ()
		{
			return unchecked((long)l);
		}

		
		// Data
		
		/// <summary>
		/// Int32 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public ulong 	l;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
		/// <summary>
		/// byte #5
		/// </summary>
		[FieldOffset(4)]
		public byte 	b5;
		/// <summary>
		/// byte #6
		/// </summary>
		[FieldOffset(5)]
		public byte 	b6;
		/// <summary>
		/// byte #7
		/// </summary>
		[FieldOffset(6)]
		public byte 	b7;
		/// <summary>
		/// byte #8
		/// </summary>
		[FieldOffset(7)]
		public byte 	b8;
	}

	
	/// <summary>
	/// Structure for converting doubles
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct Float64Union
	{
		public Float64Union (double v)
		{
			b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = 0;
			d = v;
		}

		public Float64Union (byte[] buffer, int offset = 0)
		{
			d = 0;
			b1 = buffer[offset + 0];
			b2 = buffer[offset + 1];
			b3 = buffer[offset + 2];
			b4 = buffer[offset + 3];
			b5 = buffer[offset + 4];
			b6 = buffer[offset + 5];
			b7 = buffer[offset + 6];
			b8 = buffer[offset + 7];
		}

		
		/// <summary>
		/// Convert to network form or back (depending on 
		/// </summary>
		public void Flip ()
		{
			byte tmp;
			
			tmp = b1; b1 = b8; b8 = tmp;			
			tmp = b2; b2 = b7; b7 = tmp;
			tmp = b3; b3 = b6; b6 = tmp;
			tmp = b4; b4 = b5; b5 = tmp;
		}
		
		
		/// <summary>
		/// Convert int in network or local form to bytes
		/// </summary>
		/// <returns>
		public byte[] ToBytes (byte[] buffer)
		{
			buffer[0] = b1;
			buffer[1] = b2;
			buffer[2] = b3;
			buffer[3] = b4;
			buffer[4] = b5;
			buffer[5] = b6;
			buffer[6] = b7;
			buffer[7] = b8;
			return buffer;
		}
		
		
		/// <summary>
		/// Convert to int (in local form presumably)
		/// </summary>
		public double ToDouble ()
		{
			return d;
		}
		
		
		// Data
		
		/// <summary>
		/// float64 version of the value.
		/// </summary>
		[FieldOffset(0)]
		public double 	d;

		/// <summary>
		/// byte #1
		/// </summary>
		[FieldOffset(0)]
		public byte 	b1;
		/// <summary>
		/// byte #2
		/// </summary>
		[FieldOffset(1)]
		public byte 	b2;
		/// <summary>
		/// byte #3
		/// </summary>
		[FieldOffset(2)]
		public byte 	b3;
		/// <summary>
		/// byte #4
		/// </summary>
		[FieldOffset(3)]
		public byte 	b4;
		/// <summary>
		/// byte #5
		/// </summary>
		[FieldOffset(4)]
		public byte 	b5;
		/// <summary>
		/// byte #6
		/// </summary>
		[FieldOffset(5)]
		public byte 	b6;
		/// <summary>
		/// byte #7
		/// </summary>
		[FieldOffset(6)]
		public byte 	b7;
		/// <summary>
		/// byte #8
		/// </summary>
		[FieldOffset(7)]
		public byte 	b8;
	}

}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/Blob.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.io
{
	/// <summary>
	/// Efficient byte buffer (convertible to string also)
	/// </summary>
	public class Blob
	{
		public Blob (int size)
		{
			_current = new Region (size, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);	
		}
		
		public Blob (byte[] data)
		{
			_current = new Region(data, data.Length, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);
		}
		
		
		public Blob (string sdata)
		{
			var data = TextEncoding.GetBytes(sdata);
			
			_current = new Region(data, data.Length, 0);
			_alen = 0;
			_list = new List<Region> ();
			_list.Add (_current);
		}

		public Blob ()
			: this (0) {}
		
		
		// Properties
		
		
		/// <summary>
		/// Gets list of regions.
		/// </summary>
		/// <value>
		/// The regions.
		/// </value>
		public IList<Region> Regions
			{ get { return _list; } }
		
		
		/// <summary>
		/// Gets current length or adjustes length upwards or downwards as indicated in setting
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public int Length
		{ 
			get 
			{ 
				return _alen + _current.Used; 
			}
			set
			{
				int clen = _alen + _current.Used;
				int delta = clen - value;
			
				// simple cases
				int used = _current.Used;
				if (delta < 0 && used >= -delta)
					_current.Used = (used + delta);
				else if (delta >= 0 && _current.Span >= delta)
					_current.Used = (used + delta);
			
				// case where need to trim buffers
				clen = 0;
				for (int i = 0 ; i < _list.Count ; i++)
				{
					Region region = (Region)_list[i];
					int olen = clen;
					clen += region.Used;
			
					if (clen >= value)
					{
						region.Used = (value - olen);
						for (int j = clen-1 ; j > value ; j--)
							_list.RemoveAt (j);
					}
				}
			
				// case where need to expand
				if (clen < value)
				{
					Region region = Acquire (Math.Max (BufSize, value - clen));
					region.Used = (value - clen);
				}
			}
		}
		
		
		/// <summary>
		/// Gets or sets the ith position in the buffer.
		/// </summary>
		/// <param name='ith'>
		/// index
		/// </param>
		public byte this [int ith]
		{
			get 
			{
				Region region = GetRegionForIndex (ith);
				return region[ith - region.StartingIndex];
			}
			set 
			{
				Region region = GetRegionForIndex (ith);
				region[ith - region.StartingIndex] = value;
			}
		}
		
		
		/// <summary>
		/// Get as byte array.
		/// </summary>
		public byte[] AsBytes
		{
			get
			{
				// special case where buffer is exactly the right size, etc
				if (_list.Count == 1 && _current.Span == 0)
					return _current.Bytes;
		
				// otherwise copy to one large array
				byte[] buffer = new byte[Length];
				int offset = 0;
		
				for (int i = 0 ; i < _list.Count ; i++)
				{
					Region region = (Region)_list[i];
					int used = region.Used;
					byte[] rbuffer = region.Bytes;
					
					System.Buffer.BlockCopy (rbuffer, 0, buffer, offset, used);
						
					offset += used;
				}
		
				// adjust buffer
				_list.Clear ();
				_alen = 0;
				_current = new Region (buffer, buffer.Length, 0);
				_list.Add (_current);
		
				return buffer;
			}
		}
		
		
		/// <summary>
		/// Gets the buffer as string.
		/// </summary>
		/// <value>
		/// The string.
		/// </value>
		public string AsString
			{ get { return TextEncoding.GetString(AsBytes); } }
	
		
		/// <summary>
		/// Provide blob as byte array
		/// </summary>
		public static implicit operator byte[] (Blob v)
			{ return v != null ? v.AsBytes : null; }

		
		/// <summary>
		/// Provide blob as string
		/// </summary>
		public static implicit operator string (Blob v)
			{ return v != null ? v.AsString : null; }
		
		/// <summary>
		/// Return as string
		/// </summary>
		public override string ToString()
		{
			return AsString;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Append ASCII string to buffer
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void Append (string text)
		{
			byte[] bytes = TextEncoding.GetBytes (text);
			Append (bytes, 0, bytes.Length);
		}
		
		
		/**
		 * Append buffer to current binary buffer (or create new one)
		 * 
		 * @param buffer	buffer to be appended (copied)
		 * @param offset	offset in buffer
		 * @param len		length of region to be copied
		 */
		public void Append (byte[] buffer, int offset, int len)
		{
			Region region = _current;
	
			while (len > 0)
			{
				// replenish buffer if necessary
				if (region.Span == 0)
					region = Acquire (Math.Max (BufSize, len));
	
				// determine span
				int cspan = Math.Min (region.Span, len);
	
				// copy into buffer
				byte[] rbytes = region.Bytes;
				int roffset = region.Offset;
				System.Buffer.BlockCopy (buffer, offset, rbytes, roffset, cspan);
					
				region.Used = (region.Used + cspan);
				len -= cspan;
			}
		}
		
		
		/**
		 * Append byte to buffer
		 * 
		 * @param c	byte to be appended
		 */
		public void Append (byte c)
		{
			if (_current.Span == 0)
				Acquire (BufSize);
	
			
			int used = _current.Used;
			byte[] rbuffer = _current.Bytes;
			
			rbuffer [used] = c;
			_current.Used = (used+1);
		}

		
		
		/**
		 * Acquire buffer to write into (may be less than the requested though not 0 if "atleast" is false
		 * otherwise will provide a buffer of at least the size requested)
		 * <p>
		 * "Used" property MUST be adjusted after acquisition!
		 * 
		 * @param size		size guideline for acquired buffer
		 * @param atleast	indicate whether size returned must be minimally the requested
		 * @return			the region for the sub-buffer
		 */
		public Region Acquire (int size, bool atleast)
		{
			int cspan = _current.Span;
	
			if (cspan >= size)
				return _current;
			if (cspan > 0 && cspan >= (size /4) && !atleast)
				return _current;
				
			_alen += _current.Used;
			_current = new Region (size, 0);
			_list.Add (_current);
			return _current;
		}
	
	
		
		/**
		 * Acquire buffer to write into (may be less than the requested though not 0 if "atleast" is false
		 * otherwise will provide a buffer of at least the size requested)
		 * <p>
		 * "Used" property MUST be adjusted after acquisition!
		 * 
		 * @param size		size guideline for acquired buffer
		 * @return			the region for the sub-buffer
		 */
		public Region Acquire (int size)
		{
			return Acquire (size, true);
		}
		
		
		/**
		 * Find region for given index
		 *
		 * @param ith		index
		 * @return			region for this index
		 */
		public Region GetRegionForIndex (int ith)
		{
			int pos = 0;
			for (int i = 0 ; i < _list.Count ; i++)
			{
				Region region = _list[i];
				if (pos+ith < region.Used) return region;
				pos += region.Used;
			}
			
			return null;
		}
		
		
		// Classes
		
		public class Region
		{
			public Region (int size, int startingindex)
			{
				_startingindex = startingindex;
				_used = 0;
				if (size > 0)
					_buffer = new byte [size];
				else
					_buffer = null;
			}
	
			public Region (byte[] buffer, int used, int startingindex)
			{
				_startingindex = startingindex;
				_used = used;
				_buffer = buffer;
			}
	
			public Region ()
				: this (0,0) { }
	
			
			/// <summary>
			/// Gets the buffer region bytes.
			/// </summary>
			/// <value>
			/// The bytes.
			/// </value>
			public byte[] Bytes
			{
				get 
				{
					if (_buffer != null)
						return _buffer;
					else
						return _buffer = new byte [BufSize];
				}
			}
	
			
			/// <summary>
			/// Gets the index of region.
			/// </summary>
			/// <value>
			/// The index of the starting.
			/// </value>
			public int StartingIndex
				{ get { return _startingindex; } }
			
			
			/// <summary>
			/// offset for write position into buffer
			/// </summary>
			/// <value>
			/// The offset.
			/// </value>
			public int Offset
				{ get { return _used; } }
			
			
			/// <summary>
			/// Gets or sets the value at the ith position in buffer region
			/// </summary>
			/// <param name='ith'>
			/// Ith.
			/// </param>
			public byte this[int ith]
				{ get { return _buffer[ith + _startingindex]; } set { _buffer[ith + _startingindex] = value; } }
	
			
			/// <summary>
			/// Span of bytes remaining (free space in buffer)
			/// </summary>
			/// <value>
			/// The span.
			/// </value>
			public int Span
				{ get { return Bytes.Length - _used - _startingindex; } }
	
	
			/// <summary>
			/// get the amount of the region currently used
			/// </summary>
			/// <value>
			/// The used.
			/// </value>
			public int Used
				{ get { return _used; } set { _used = value; } }
					
	
			// variables
	
			private byte[]		_buffer;
			private int			_used;
			private int			_startingindex;	
		}
		
		
		// Constants
		
		private readonly static ASCIIEncoding 	TextEncoding 	= new ASCIIEncoding();
		private readonly static int				BufSize 		= 4192;
		
		// Variables
		
		private IList<Region>	_list;
		private Region			_current;
		private int				_alen;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/BufferedDuplexStream.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.io
{
	/// <summary>
	/// Buffered read/write stream, where read and write are separate and do not intersect
	/// </summary>
	public class BufferedDuplexStream : Stream
	{
		public BufferedDuplexStream (Stream underlier)
		{
			Underlier = underlier;
		}


		// Properties

		public Stream Underlier
			{ get; private set; }

		public int Available
			{ get { return _rsize - _rpos; } }

		public override bool CanRead
			{ get { return true; } }

		public override bool CanWrite
			{ get { return true; } }

		public override bool CanSeek
			{ get { return false; } }

		public override long Length
			{ get { return Underlier.Length; } }

		public override long Position
		{ 
			get { throw new ArgumentException ("cannot support seeking on duplex stream"); } 
			set { throw new ArgumentException ("cannot support seeking on duplex stream"); } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			Flush ();
			Underlier.Close();
			_rpos = 0;
			_rsize = 0;
			_wpos = 0;
		}


		/// <summary>
		/// Read into the specified buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			Refill ();
			var done = Math.Min(_rsize - _rpos, count);
			Array.Copy (_rbuffer, _rpos, buffer, offset, done);
			
			_rpos += done;
			return done;
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			Refill();
			
			if (_rpos < _rsize)
				return _rbuffer[_rpos++];
			else
				return -1;
		}


		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			if (_wpos == 0)
				return;

			Underlier.Write (_wbuffer, 0, _wpos);
			Underlier.Flush ();
			_wpos = 0;
		}


		/// <summary>
		/// Seek to the specified offset and origin.
		/// </summary>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new ArgumentException ("cannot support seeking on duplex stream");
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			throw new ArgumentException ("cannot support seeking on duplex stream");
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			switch (_wbuffer.Length - _wpos)
			{
				case 0:
					Flush ();
					_wbuffer [_wpos++] = value;
					break;

				case 1:
					_wbuffer [_wpos++] = value;
					Flush ();
					break;

				default:
					_wbuffer [_wpos++] = value;
					break;
			}
		}


		/// <summary>
		/// Writes the buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			if (count < 0)
				throw new ArgumentException ("Write: attempted to write a buffer with length < 0");

			while (count > 0)
			{
				var write = CmpUtils.Constrain (count, 0, _wbuffer.Length - _wpos);

				// flush if cannot write more to the buffer
				if (write == 0)
				{
					Flush ();
					continue;
				}

				Array.Copy (buffer, offset, _wbuffer, _wpos, write);

				_wpos += write;
				count -= write;
				offset += write;
			}

			if (_wpos == _wbuffer.Length)
				Flush ();
		}


		#region Implementation
		
		private void Refill ()
		{
			if (_rpos < _rsize)
				return;
			
			_rpos = 0;
			_rsize = 0;

			_rsize = Underlier.Read (_rbuffer, 0, _rbuffer.Length);
		}


		#endregion

		// Variables

		private byte[]		_rbuffer = new byte[4096];
		private byte[]		_wbuffer = new byte[4096];
		private int			_rpos = 0;
		private int			_wpos = 0;
		private int			_rsize = 0;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/BufferedRandomAccessFile.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.io
{
	/// <summary>
	/// Buffered implementation against a random access stream
	/// </summary>
	public sealed class BufferedRandomAccessFile : FileStream, IDisposable
	{
		public enum State
			{ Uninitialized, Empty, HasData, Dirty };
			
		public enum Op
			{ Read, Write, ReadWrite };

		public BufferedRandomAccessFile (string file, int blocksize = 8192, int maxsize = 4*8192, FileAccess mode = FileAccess.ReadWrite) 
			: base (file, FileMode.OpenOrCreate, mode)
		{
			_filename = file;
			_blocksize = blocksize;
			
			var nblocks = (int)(maxsize / blocksize);
			_cache = new LRUCache<int,Buffer>(x => blocksize, maxsize);
			_pool = new STObjectPool<Buffer> (nblocks + 1, () => new Buffer(this, blocksize));
			_Flen = base.Length;
		}
		
		
		~BufferedRandomAccessFile ()
		{
			Close();
		}
		
		
		// Properties
		
		
		public long ReadPosition
			{ get { return _rpos; } }
		
		public long WritePosition
			{ get { return _wpos; } }
		
		public override long Position
		{
			get
			{
				throw new Exception ("Position: not supported, refer to ReadPosition or WritePosition");
			}
			set
			{
				throw new Exception ("Position: not supported, refer to ReadPosition or WritePosition");
			}
		}
		
				
		// Functions
		

		/// <summary>
		/// Flush write buffers
		/// </summary>
		public override void Flush ()
		{
			if (_writing != null)
				_writing.Flush();
		}
			
		
		/// <summary>
		/// Close the file
		/// </summary>
		public override void Close ()
		{
			Flush ();
			base.Close ();
		}
		
		
		/// <summary>
		/// Current length of file
		/// </summary>
		public override long Length
		{
			get { return _Flen; }
		}
		
		
		/// <summary>
		/// Sets new file length.
		/// </summary>
		/// <param name='newLength'>
		/// New file length
		/// </param>
		public override void SetLength (long newLength)
		{
			base.SetLength (newLength);
		
			_rpos = Math.Min (_rpos, newLength);
			_wpos = Math.Min (_wpos, newLength);
			_Flen = newLength;
		}
	
				
		/// <summary>
		/// Provides a buffered extent of desired size if available contiguously for reading.
		/// The # of byte read MUST correspond to the nbytes parameter, as the stream will advance
		/// by the indicated amount.
		/// </summary>
		/// <returns>
		/// true if the extent found
		/// </returns>
		/// <param name='buffer'>
		/// the buffer holding the extent (output param)
		/// </param>
		/// <param name='offset'>
		/// offset in buffer (output param)
		/// </param>
		/// <param name='nbytes'>
		/// number of bytes wants to locate
		/// </param>
		public bool ReadExtent (out byte[] buffer, out long offset, int nbytes)
		{
			Buffer buf = GetReadBufferFor (_rpos);
			if ((buf.len - buf.rpos) >= nbytes)
			{
				buffer = buf.data;
				offset = buf.rpos;
				buf.rpos += nbytes;
				_rpos += nbytes;
				return true;
			} 
			else
			{
				buffer = null;
				offset = 0;
				return false;
			}
		}
	
				
		/// <summary>
		/// Provides a buffered extent of desired size if available contiguously for writing
		/// The # of bytes written MUST correspond to the nbytes parameter, as the stream will advance
		/// by the indicated amount.
		/// </summary>
		/// <returns>
		/// true if the extent found
		/// </returns>
		/// <param name='buffer'>
		/// the buffer holding the extent (output param)
		/// </param>
		/// <param name='offset'>
		/// offset in buffer (output param)
		/// </param>
		/// <param name='nbytes'>
		/// number of bytes wants to locate
		/// </param>
		public bool WriteExtent (out byte[] buffer, out long offset, int nbytes)
		{
			Buffer buf = GetWriteBufferFor (_wpos);
			if ((buf.data.Length - buf.wpos) >= nbytes)
			{
				buffer = buf.data;
				offset = buf.wpos;
				buf.wpos += nbytes;
				buf.len = Math.Max (buf.wpos, buf.len);
				_wpos += nbytes;
				_Flen = Math.Max (_wpos, _Flen);
				return true;
			} 
			else
			{
				buffer = null;
				offset = 0;
				return false;
			}
		}
		
				
		/// <summary>
		/// Reads a byte from buffer
		/// </summary>
		/// <returns>
		/// The byte.
		/// </returns>
		public override int ReadByte() 
		{
			if (_rpos < _Flen)
			{
				Buffer buf = GetReadBufferFor (_rpos++);
				return (int)buf.data[buf.rpos] & 0xff;
			} else
				return -1;
		} 

		
		
		/// <summary>
		/// Reads a block of bytes from the stream and writes the data in a given buffer. (Overrides Stream.Read(Byte[], Int32, Int32).)
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='off'>
		/// Offet
		/// </param>
		/// <param name='len'>
		/// Length.
		/// </param>
		public override int Read (byte[] buffer, int off, int len) 
		{
			int read = 0;
			
			while (read < len && _rpos < _Flen)
			{
				Buffer buf = GetReadBufferFor (_rpos);
				
				int size = Math.Min (len - read, buf.len - buf.rpos);
				System.Buffer.BlockCopy (buf.data, buf.rpos, buffer, off, size);
				
				_rpos += size;
				read += size;
			}
			
			return (read > 0) ? read : -1;
		}
		
						
		/// <summary>
		/// Reads a block of bytes from the stream and writes the data in a given buffer. (Overrides Stream.Read(Byte[], Int32, Int32).)
		/// </summary>
		public override long Seek (long pos, SeekOrigin origin) 
		{
			throw new IOException ("Must use seek() variant that indicates reader or writer");
		}
		
		
		/// <summary>
		/// Seek to the specified position for reading or writing
		/// </summary>
		/// <param name='op'>
		/// Op.
		/// </param>
		/// <param name='pos'>
		/// Position.
		/// </param>
		public void Seek (Op op, long pos) 
		{
			switch (op)
			{
				case Op.Read:
					_rpos = pos;
					break;
				case Op.Write:
					_wpos = pos;
					break;
				default:
					_rpos = _wpos = pos;
					break;
			}
		}
		
		
		/// <summary>
		/// Writes the specified buffer, given offset and len.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='off'>
		/// Offset.
		/// </param>
		/// <param name='len'>
		/// Length.
		/// </param>
		public override void Write(byte[] buffer, int off, int len) 
		{
			int written = 0;
			Buffer buf = null;
			
			while (written < len)
			{
				buf = GetWriteBufferFor (_wpos); 
				
				int amount = Math.Min (len, _blocksize - buf.wpos);
				System.Buffer.BlockCopy (buffer, off, buf.data, buf.wpos, amount);
				
				_wpos += amount;
				written += amount;
				_Flen = Math.Max (_wpos + amount, _Flen);
				
				buf.len = buf.wpos + amount;
			}
		}
				
		
		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='b'>
		/// Byte to be written
		/// </param>
		public override void WriteByte (byte b) 
		{
			Buffer buf = GetWriteBufferFor (_wpos++);
			buf.data[buf.wpos] = b;
			buf.len = Math.Max (buf.wpos+1, buf.len);
			
			_Flen = Math.Max (_wpos, _Flen);
		}
	
		
		
		// Implementation
			
		
		private Buffer GetBufferFor (long pos)
		{
			lock (_cache)
			{
				var newpage = (int)(pos / _blocksize);	
				Buffer buf = _cache[newpage];
			
				// may need to create or get from pool
				if (buf == null)
				{
					buf = _pool.Alloc ();
					Debug.Assert (buf.state == State.Uninitialized);
				}
		
				switch (buf.state)
				{
					case State.Dirty:
					case State.HasData:
						return buf;
					
					default:
						long Fbase = newpage * (long)_blocksize;
						buf.len = (int)Math.Min((long)(_Flen - Fbase), _blocksize);
						buf.page = newpage;
						
						ReadBlock (Fbase, buf.data, buf.len);
						
						buf.state = State.HasData;
						_cache[newpage] = buf;
						return buf;
				}
			}
		}
		
		
		private Buffer GetReadBufferFor (long pos)
		{
			int Bpos = (int)(pos % _blocksize);
			int newpage = (int)(pos / _blocksize);
			
			if (_reading == null || _reading.page != newpage)
				_reading = GetBufferFor (pos);
	
			_reading.rpos = Bpos;		
			return _reading;
		}
	
		
		
		private Buffer GetWriteBufferFor (long pos)
		{
			int Bpos = (int)(pos % _blocksize);
			int newpage = (int)(pos / _blocksize);
	
			if (_writing != null && _writing.page == newpage)
			{
				_writing.wpos = Bpos;
				_writing.state = State.Dirty;
				return _writing;
			}
			
			if (_writing != null && _writing.state == State.Dirty)
				Flush ();
			
			_writing = GetBufferFor (pos);
			_writing.wpos = Bpos;
			_writing.state = State.Dirty;
			
			return _writing;
		}
	
	
		
		/**
		 * Read in a block from a position in file
		 */
		private void ReadBlock (long position, byte[] buffer, int len)
		{
			lock (_cache)
			{
				base.Seek(position, SeekOrigin.Begin);
				
				int read = 0;
				while (read < len)
				{
					int n = base.Read (buffer, read, len - read);
					if (n >= 0)
						read += n;
					else
						throw new IOException ("hit EOF prematurely");
				}
			}
		}
		
		
		/**
		 * Write block to specific position
		 */
		private void WriteBlock (long position, byte[] buffer, int len)
		{
			lock (_cache)
			{
				base.Seek (position, SeekOrigin.Begin);		
				base.Write (buffer, 0, len);
			}
		}
		
		
		// Classes
		
		
		internal class Buffer : IDisposable
		{
			public Buffer (BufferedRandomAccessFile file, int size)
			{ 
				this.file = file;
				this.data = new byte[size];
				this.state = State.Uninitialized;
			}
			
			
			public Buffer ()
				{ }
			
			
			// Data

			public byte[]						data;
			public long							page;
	
			public int							rpos;
			public int							wpos;
			public int							len;
			public State						state;
			public BufferedRandomAccessFile		file;
			

			// Methods
			
			
			public void Dispose ()
			{
				if (state == State.Dirty)
					Flush ();
				if (state == State.Uninitialized)
					return;

				state = State.Uninitialized;
				page = -1;
				file._pool.Free(this);
			}
			
			
			public void Flush ()
			{
				if (state != State.Dirty)
					return;
				
				long pos = page * (long)file._blocksize;
	
				if (_log.IsDebugEnabled)
					_log.Info ("flushing block " + page + ", on: " + file._filename + ", range: [" + pos + ", " + (pos+len) + "]");
				
				file.WriteBlock (pos, data, len);		
				
				state = State.HasData;
			}
		}
		
		
		
		// Variables
		
		private string						_filename;
		private STObjectPool<Buffer>		_pool;				// pool of blocks to use
		private LRUCache<int,Buffer>		_cache;				// cache of blocks
		
		private Buffer						_reading;			// current read buffer
		private Buffer						_writing;			// current write buffer
		
		private int							_blocksize;
		private long						_rpos = 0L;			// current read position in file
		private long						_wpos = 0L;			// current write position in file
		private long						_Flen = -1;			// length of file	
		
		static Logger						_log = Logger.Get("IO");
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/BufferedReadStream.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.io
{
	/// <summary>
	/// Buffered read-only stream with indication of how many bytes are available in buffer.
	/// </summary>
	public class BufferedReadStream : Stream
	{
		public BufferedReadStream (
			Stream underlier, 
			int buffersize = 8192)
		{
			Underlier = underlier;
			_buffer = new byte[buffersize];
		}


		// Properties

		public Stream Underlier
			{ get; private set; }

		public int Available
			{ get { return _size - _pos; } }

		public override bool CanRead
			{ get { return true; } }

		public override bool CanWrite
			{ get { return false; } }

		public override bool CanSeek
			{ get { return Underlier.CanSeek; } }

		public override long Length
			{ get { return Underlier.Length; } }

		public override long Position
		{ 
			get { return Underlier.Position - _size + _pos; } 
			set { Underlier.Position = value;  _pos = 0;  _size = 0; } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			Underlier.Close();
			_size = 0;
			_pos = 0;
			_buffer = null;
		}


		/// <summary>
		/// Read into the specified buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			Refill ();
			var done = Math.Min(_size - _pos, count);
			if (done > 0)
				Array.Copy (_buffer, _pos, buffer, offset, done);

			_pos += done;
			return done;
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			Refill();

			if (_pos < _size)
				return _buffer[_pos++];
			else
				return -1;
		}

		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Seek to the specified offset and origin.
		/// </summary>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		public override long Seek (long offset, SeekOrigin origin)
		{
			var pos = Underlier.Seek (offset, origin);
			_pos = 0;
			_size = 0;

			return pos;
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			throw new NotImplementedException ("SetLength operation only for writeable streams");
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			throw new NotImplementedException ("WriteByte operation only for writeable streams");
		}


		/// <summary>
		/// Writes the buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ("Write operation only for writeable streams");
		}


		#region Implementation

		private void Refill ()
		{
			if (_pos < _size)
				return;

			_pos = 0;
			_size = Math.Max (0, Underlier.Read (_buffer, 0, _buffer.Length));
		}

		#endregion

		// Variables

		private byte[]		_buffer;
		private int			_pos = 0;
		private int			_size = 0;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/BufferedWriteStream.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	/// <summary>
	/// Buffered write-only stream with indication of how many bytes are available in buffer.
	/// </summary>
	public class BufferedWriteStream : Stream
	{
		public BufferedWriteStream (Stream underlier, int buffersize = 4096)
		{
			Underlier = underlier;
			_buffer = new byte[buffersize];
		}


		// Properties

		public Stream Underlier
			{ get; private set; }

		public override bool CanRead
			{ get { return false; } }

		public override bool CanWrite
			{ get { return true; } }

		public override bool CanSeek
			{ get { return Underlier.CanSeek; } }

		public override long Length
			{ get { return Underlier.Length; } }

		public override long Position
		{ 
			get { return Underlier.Position + _pos; } 
			set { Underlier.Position = value;  _pos = 0; } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			Flush ();
			Underlier.Close();
			_pos = 0;
			_buffer = null;
		}


		/// <summary>
		/// Read into the specified buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}

		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			if (_pos == 0)
				return;

			Underlier.Write (_buffer, 0, _pos);
			_pos = 0;
		}


		/// <summary>
		/// Seek to the specified offset and origin.
		/// </summary>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		public override long Seek (long offset, SeekOrigin origin)
		{
			var pos = Underlier.Seek (offset, origin);
			_pos = 0;

			return pos;
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			Underlier.SetLength (value);
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			_buffer[_pos++] = value;

			if (_pos == _buffer.Length)
				Flush ();
		}


		/// <summary>
		/// Writes the buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				var write = Math.Min (count, _buffer.Length - _pos);
				Array.Copy (buffer, offset, _buffer, _pos, write);

				_pos += write;
				count -= write;
				offset += write;

				if (_pos == _buffer.Length)
					Flush ();
			}
		}


		// Variables

		private byte[]		_buffer;
		private int			_pos = 0;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/ConcatedStream.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.io
{
	/// <summary>
	/// A stream that concatenates two or more streams
	/// </summary>
	public class ConcatenatedStream : Stream
	{
		public ConcatenatedStream (params Stream[] underlier)
		{
			StreamList = underlier;
			_pos = 0L;
			_Icurrent = 0;
			_len = StreamList.Sum (v => v.Length - v.Position);
		}


		// Properties

		public Stream[] StreamList
			{ get; private set; }

		public int Available
		{ get { return (int)(_pos - _len); } }

		public override bool CanRead
			{ get { return true; } }

		public override bool CanWrite
			{ get { return false; } }

		public override bool CanSeek
			{ get { return false; } }

		public override long Length
			{ get { return _len; } }

		public override long Position
		{ 
			get { return _pos; } 
			set { throw new Exception ("position cannot be set"); } 
		}


		// Functions

		/// <summary>
		/// Close the stream
		/// </summary>
		public override void Close ()
		{
			for (int i = 0; i < StreamList.Length; i++)
				StreamList [i].Close ();
		}


		/// <summary>
		/// Read into the specified buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			var read = 0;

			while (count > 0 && _Icurrent < StreamList.Length)
			{
				var done = StreamList [_Icurrent].Read (buffer, offset, count);
				if (done == 0)
					_Icurrent++; 
					
				_pos += done;
				read += done;
				offset += done;
				count -= done;
			}

			return read;
		}


		/// <summary>
		/// Reads a byte.
		/// </summary>
		public override int ReadByte ()
		{
			if (_Icurrent == StreamList.Length)
				return -1;

			while (_Icurrent < StreamList.Length)
			{
				var c = StreamList [_Icurrent].ReadByte ();
				if (c >= 0)
					return c;
				else
					_Icurrent++;
			}

			return -1;
		}


		/// <summary>
		/// Flush stream
		/// </summary>
		public override void Flush ()
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Seek to the specified offset and origin.
		/// </summary>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotImplementedException ("flush operation only for writeable streams");
		}


		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name='value'>
		/// length of file.
		/// </param>
		public override void SetLength (long value)
		{
			throw new NotImplementedException ("SetLength operation only for writeable streams");
		}


		/// <summary>
		/// Writes one byte.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void WriteByte (byte value)
		{
			throw new NotImplementedException ("WriteByte operation only for writeable streams");
		}


		/// <summary>
		/// Writes the buffer, offset and count.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='count'>
		/// Count.
		/// </param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ("Write operation only for writeable streams");
		}


		// Variables

		private long		_len;
		private long		_pos;
		private int			_Icurrent;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/EndianConversions.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	/// <summary>
	/// Reads binary data swapping endianness
	/// </summary>
	public class SwapEndianConverter : IBinaryConversions
	{
		// Operations
		

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16(byte[] buffer, int offset)
		{
			_int16.b2 = buffer[offset+0];
			_int16.b1 = buffer[offset+1];
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32(byte[] buffer, int offset)
		{
			_int32.b4 = buffer[offset+0];
			_int32.b3 = buffer[offset+1];
			_int32.b2 = buffer[offset+2];
			_int32.b1 = buffer[offset+3];
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64(byte[] buffer, int offset)
		{
			_int64.b8 = buffer[offset+0];
			_int64.b7 = buffer[offset+1];
			_int64.b6 = buffer[offset+2];
			_int64.b5 = buffer[offset+3];
			_int64.b4 = buffer[offset+4];
			_int64.b3 = buffer[offset+5];
			_int64.b2 = buffer[offset+6];
			_int64.b1 = buffer[offset+7];
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16(byte[] buffer, int offset)
		{
			_int16.b2 = buffer[offset+0];
			_int16.b1 = buffer[offset+1];
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32(byte[] buffer, int offset)
		{
			_int32.b4 = buffer[offset+0];
			_int32.b3 = buffer[offset+1];
			_int32.b2 = buffer[offset+2];
			_int32.b1 = buffer[offset+3];
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64(byte[] buffer, int offset)
		{
			_int64.b8 = buffer[offset+0];
			_int64.b7 = buffer[offset+1];
			_int64.b6 = buffer[offset+2];
			_int64.b5 = buffer[offset+3];
			_int64.b4 = buffer[offset+4];
			_int64.b3 = buffer[offset+5];
			_int64.b2 = buffer[offset+6];
			_int64.b1 = buffer[offset+7];
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble(byte[] buffer, int offset)
		{
			_float64.b8 = buffer[offset+0];
			_float64.b7 = buffer[offset+1];
			_float64.b6 = buffer[offset+2];
			_float64.b5 = buffer[offset+3];
			_float64.b4 = buffer[offset+4];
			_float64.b3 = buffer[offset+5];
			_float64.b2 = buffer[offset+6];
			_float64.b1 = buffer[offset+7];
			
			return _float64.ToDouble();
		}


		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteInt16(byte[] buffer, int offset, short v)
		{
			_int16.i = (ushort)v;
			buffer[offset+1] = _int16.b1;
			buffer[offset+0] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteInt32(byte[] buffer, int offset, int v)
		{
			_int32.i = (uint)v;
			buffer[offset+3] = _int32.b1;
			buffer[offset+2] = _int32.b2;
			buffer[offset+1] = _int32.b3;
			buffer[offset+0] = _int32.b4;
		}

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteInt64(byte[] buffer, int offset, long v)
		{
			_int64.l = (ulong)v;
			buffer[offset+7] = _int64.b1;
			buffer[offset+6] = _int64.b2;
			buffer[offset+5] = _int64.b3;
			buffer[offset+4] = _int64.b4;
			buffer[offset+3] = _int64.b5;
			buffer[offset+2] = _int64.b6;
			buffer[offset+1] = _int64.b7;
			buffer[offset+0] = _int64.b8;
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteUInt16(byte[] buffer, int offset, ushort v)
		{
			_int16.i = v;
			buffer[offset+1] = _int16.b1;
			buffer[offset+0] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteUInt32(byte[] buffer, int offset, uint v)
		{
			_int32.i = v;
			buffer[offset+3] = _int32.b1;
			buffer[offset+2] = _int32.b2;
			buffer[offset+1] = _int32.b3;
			buffer[offset+0] = _int32.b4;
		}
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public void WriteUInt64(byte[] buffer, int offset, ulong v)
		{
			_int64.l = v;
			buffer[offset+7] = _int64.b1;
			buffer[offset+6] = _int64.b2;
			buffer[offset+5] = _int64.b3;
			buffer[offset+4] = _int64.b4;
			buffer[offset+3] = _int64.b5;
			buffer[offset+2] = _int64.b6;
			buffer[offset+1] = _int64.b7;
			buffer[offset+0] = _int64.b8;			
		}
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteDouble(byte[] buffer, int offset, double v)
		{
			_float64.d = v;
			buffer[offset+7] = _float64.b1;
			buffer[offset+6] = _float64.b2;
			buffer[offset+5] = _float64.b3;
			buffer[offset+4] = _float64.b4;
			buffer[offset+3] = _float64.b5;
			buffer[offset+2] = _float64.b6;
			buffer[offset+1] = _float64.b7;
			buffer[offset+0] = _float64.b8;		
		}
		
		
		// Variables
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}
	
	
	/// <summary>
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianConverter : IBinaryConversions
	{
		// Operations
		

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16(byte[] buffer, int offset)
		{
			_int16.b1 = buffer[offset+0];
			_int16.b2 = buffer[offset+1];
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32(byte[] buffer, int offset)
		{
			_int32.b1 = buffer[offset+0];
			_int32.b2 = buffer[offset+1];
			_int32.b3 = buffer[offset+2];
			_int32.b4 = buffer[offset+3];
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64(byte[] buffer, int offset)
		{
			_int64.b1 = buffer[offset+0];
			_int64.b2 = buffer[offset+1];
			_int64.b3 = buffer[offset+2];
			_int64.b4 = buffer[offset+3];
			_int64.b5 = buffer[offset+4];
			_int64.b6 = buffer[offset+5];
			_int64.b7 = buffer[offset+6];
			_int64.b8 = buffer[offset+7];
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16(byte[] buffer, int offset)
		{
			_int16.b1 = buffer[offset+0];
			_int16.b2 = buffer[offset+1];
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32(byte[] buffer, int offset)
		{
			_int32.b1 = buffer[offset+0];
			_int32.b2 = buffer[offset+1];
			_int32.b3 = buffer[offset+2];
			_int32.b4 = buffer[offset+3];
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64(byte[] buffer, int offset)
		{
			_int64.b1 = buffer[offset+0];
			_int64.b2 = buffer[offset+1];
			_int64.b3 = buffer[offset+2];
			_int64.b4 = buffer[offset+3];
			_int64.b5 = buffer[offset+4];
			_int64.b6 = buffer[offset+5];
			_int64.b7 = buffer[offset+6];
			_int64.b8 = buffer[offset+7];
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble(byte[] buffer, int offset)
		{
			_float64.b1 = buffer[offset+0];
			_float64.b2 = buffer[offset+1];
			_float64.b3 = buffer[offset+2];
			_float64.b4 = buffer[offset+3];
			_float64.b5 = buffer[offset+4];
			_float64.b6 = buffer[offset+5];
			_float64.b7 = buffer[offset+6];
			_float64.b8 = buffer[offset+7];
			
			return _float64.ToDouble();
		}


		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteInt16(byte[] buffer, int offset, short v)
		{
			_int16.i = (ushort)v;
			buffer[offset+0] = _int16.b1;
			buffer[offset+1] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteInt32(byte[] buffer, int offset, int v)
		{
			_int32.i = (uint)v;
			buffer[offset+0] = _int32.b1;
			buffer[offset+1] = _int32.b2;
			buffer[offset+2] = _int32.b3;
			buffer[offset+3] = _int32.b4;
		}

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteInt64(byte[] buffer, int offset, long v)
		{
			_int64.l = (ulong)v;
			buffer[offset+0] = _int64.b1;
			buffer[offset+1] = _int64.b2;
			buffer[offset+2] = _int64.b3;
			buffer[offset+3] = _int64.b4;
			buffer[offset+4] = _int64.b5;
			buffer[offset+5] = _int64.b6;
			buffer[offset+6] = _int64.b7;
			buffer[offset+7] = _int64.b8;
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		public void WriteUInt16(byte[] buffer, int offset, ushort v)
		{
			_int16.i = v;
			buffer[offset+0] = _int16.b1;
			buffer[offset+1] = _int16.b2;
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		public void WriteUInt32(byte[] buffer, int offset, uint v)
		{
			_int32.i = v;
			buffer[offset+0] = _int32.b1;
			buffer[offset+1] = _int32.b2;
			buffer[offset+2] = _int32.b3;
			buffer[offset+3] = _int32.b4;
		}
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public void WriteUInt64(byte[] buffer, int offset, ulong v)
		{
			_int64.l = v;
			buffer[offset+0] = _int64.b1;
			buffer[offset+1] = _int64.b2;
			buffer[offset+2] = _int64.b3;
			buffer[offset+3] = _int64.b4;
			buffer[offset+4] = _int64.b5;
			buffer[offset+5] = _int64.b6;
			buffer[offset+6] = _int64.b7;
			buffer[offset+7] = _int64.b8;			
		}
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		public void WriteDouble(byte[] buffer, int offset, double v)
		{
			_float64.d = v;
			buffer[offset+0] = _float64.b1;
			buffer[offset+1] = _float64.b2;
			buffer[offset+2] = _float64.b3;
			buffer[offset+3] = _float64.b4;
			buffer[offset+4] = _float64.b5;
			buffer[offset+5] = _float64.b6;
			buffer[offset+6] = _float64.b7;
			buffer[offset+7] = _float64.b8;		
		}
		
		
		// Variables
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}

}
// -------------------------------------------
// File: ../DotNet/Library/src/common/io/EndianReaders.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	/// <summary>
	/// Reads binary data swapping endianness
	/// </summary>
	public class SwapEndianReader : IBinaryReader
	{
		public SwapEndianReader (Stream stream)
		{
			_stream = stream;
		}


		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
		}
		

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16()
		{
			_int16.b2 = InternalReadByte();
			_int16.b1 = InternalReadByte();
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32()
		{
			_int32.b4 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b1 = InternalReadByte();
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64()
		{
			_int64.b8 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b1 = InternalReadByte();
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16()
		{
			_int16.b2 = InternalReadByte();
			_int16.b1 = InternalReadByte();
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32()
		{
			_int32.b4 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b1 = InternalReadByte();
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64()
		{
			_int64.b8 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b1 = InternalReadByte();
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble()
		{
			_float64.b8 = InternalReadByte();
			_float64.b7 = InternalReadByte();
			_float64.b6 = InternalReadByte();
			_float64.b5 = InternalReadByte();
			_float64.b4 = InternalReadByte();
			_float64.b3 = InternalReadByte();
			_float64.b2 = InternalReadByte();
			_float64.b1 = InternalReadByte();
			
			return _float64.ToDouble();
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);

			return encoding.GetString(data);
		}

		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			byte[] data = new byte[size];
			ReadInternal (data, size);

			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);

			return encoding.GetString (data, 0, len);
		}

		
		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
			catch
				{ }
		}
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}
	
	
	/// <summary>
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianReader : IBinaryReader
	{
		public SameEndianReader (Stream stream)
		{
			_stream = stream;
		}

		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
		}
		

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		public short ReadInt16()
		{
			_int16.b1 = InternalReadByte();
			_int16.b2 = InternalReadByte();
			
			return _int16.ToShort();
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		public int ReadInt32()
		{
			_int32.b1 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b4 = InternalReadByte();
			
			return _int32.ToInt();
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		public long ReadInt64()
		{
			_int64.b1 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b8 = InternalReadByte();
			
			return _int64.ToLong();
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		public ushort ReadUInt16()
		{
			_int16.b1 = InternalReadByte();
			_int16.b2 = InternalReadByte();
			
			return _int16.ToUShort();
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		public uint ReadUInt32()
		{
			_int32.b1 = InternalReadByte();
			_int32.b2 = InternalReadByte();
			_int32.b3 = InternalReadByte();
			_int32.b4 = InternalReadByte();
			
			return _int32.ToUInt();
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		public ulong ReadUInt64()
		{
			_int64.b1 = InternalReadByte();
			_int64.b2 = InternalReadByte();
			_int64.b3 = InternalReadByte();
			_int64.b4 = InternalReadByte();
			_int64.b5 = InternalReadByte();
			_int64.b6 = InternalReadByte();
			_int64.b7 = InternalReadByte();
			_int64.b8 = InternalReadByte();
			
			return _int64.ToULong();
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		public double ReadDouble()
		{
			_float64.b1 = InternalReadByte();
			_float64.b2 = InternalReadByte();
			_float64.b3 = InternalReadByte();
			_float64.b4 = InternalReadByte();
			_float64.b5 = InternalReadByte();
			_float64.b6 = InternalReadByte();
			_float64.b7 = InternalReadByte();
			_float64.b8 = InternalReadByte();
			
			return _float64.ToDouble();
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);
			
			encoding = encoding ?? Encoding.ASCII;
			return encoding.GetString(data);
		}

		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			byte[] data = new byte[size];
			ReadInternal (data, size);
			
			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);
			
			return encoding.GetString (data, 0, len);
		}


		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
			catch
				{ }
		}
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
	}	

	
	
	/// <summary>
	/// Reads binary data, preserving endianness 
	/// </summary>
	public class SameEndianBufferedReader : IBinaryReader
	{
		public SameEndianBufferedReader (BufferedRandomAccessFile stream)
		{
			_stream = stream;
		}

		// Operations
		
		
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		public void Close()
		{
			_stream.Close();
		}
		

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read</returns>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		public bool ReadBoolean()
		{
			return InternalReadByte() != 0;
		}

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		unsafe public short ReadInt16()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					short* pshort = (short*)(pbuf + offset);
					return *pshort;
				}
			}
			else
			{
				_int16.b1 = InternalReadByte();
				_int16.b2 = InternalReadByte();
				return _int16.ToShort();
			}
		}

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		unsafe public int ReadInt32()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					int* pint = (int*)(pbuf + offset);
					return *pint;
				}
			}
			else
			{
				_int32.b1 = InternalReadByte();
				_int32.b2 = InternalReadByte();
				_int32.b3 = InternalReadByte();
				_int32.b4 = InternalReadByte();
				
				return _int32.ToInt();
			}
		}
		

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		unsafe public long ReadInt64()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					long* plong = (long*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_int64.b1 = InternalReadByte();
				_int64.b2 = InternalReadByte();
				_int64.b3 = InternalReadByte();
				_int64.b4 = InternalReadByte();
				_int64.b5 = InternalReadByte();
				_int64.b6 = InternalReadByte();
				_int64.b7 = InternalReadByte();
				_int64.b8 = InternalReadByte();
				
				return _int64.ToLong();
			}
		}

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		unsafe public ushort ReadUInt16()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					ushort* pshort = (ushort*)(pbuf + offset);
					return *pshort;
				}
			}
			else
			{
				_int16.b1 = InternalReadByte();
				_int16.b2 = InternalReadByte();
			
				return _int16.ToUShort();
			}
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		unsafe public uint ReadUInt32()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					uint* pint = (uint*)(pbuf + offset);
					return *pint;
				}
			}
			else
			{
				_int32.b1 = InternalReadByte();
				_int32.b2 = InternalReadByte();
				_int32.b3 = InternalReadByte();
				_int32.b4 = InternalReadByte();
			
				return _int32.ToUInt();
			}
		}
		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		unsafe public ulong ReadUInt64()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					ulong* plong = (ulong*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_int64.b1 = InternalReadByte();
				_int64.b2 = InternalReadByte();
				_int64.b3 = InternalReadByte();
				_int64.b4 = InternalReadByte();
				_int64.b5 = InternalReadByte();
				_int64.b6 = InternalReadByte();
				_int64.b7 = InternalReadByte();
				_int64.b8 = InternalReadByte();
				
				return _int64.ToULong();
			}
		}
		
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		unsafe public double ReadDouble()
		{
			byte[] buffer;
			long offset;
			
			if (_stream.ReadExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					double* plong = (double*)(pbuf + offset);
					return *plong;
				}
			}
			else
			{
				_float64.b1 = InternalReadByte();
				_float64.b2 = InternalReadByte();
				_float64.b3 = InternalReadByte();
				_float64.b4 = InternalReadByte();
				_float64.b5 = InternalReadByte();
				_float64.b6 = InternalReadByte();
				_float64.b7 = InternalReadByte();
				_float64.b8 = InternalReadByte();
				
				return _float64.ToDouble();
			}
		}
				
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		public int Read (byte[] buffer, int index, int count)
		{
			if (buffer==null)
				throw new ArgumentNullException("buffer");
			
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count < 0)
				throw new ArgumentOutOfRangeException("index");
			
			if (count+index > buffer.Length)
				throw new ArgumentException ("Not enough space in buffer for specified number of bytes starting at specified index");
			
			int read=0;
			while (count > 0)
			{
				int block = _stream.Read(buffer, index, count);
				if (block==0)
					return read;
				
				index += block;
				read += block;
				count -= block;
			}
			
			return read;
		}

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		public string ReadString (Encoding encoding = null)
		{
			int bytesToRead = ReadInt32();

			byte[] data = new byte[bytesToRead];
			ReadInternal (data, bytesToRead);
			return encoding.GetString(data);
		}
		
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		public string ReadStringField (int size, Encoding encoding = null)
		{
			byte[] data = new byte[size];
			ReadInternal (data, size);
			
			// find null terminator
			var len = 0;
			for (len = 0 ; len < size && data[len] > 0 ; len++);
			
			return encoding.GetString (data, 0, len);
		}

		
		// Implementation
		

		
		/// <summary>
		/// Reads the given number of bytes from the stream, throwing an exception
		/// if they can't all be read.
		/// </summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		void ReadInternal (byte[] data, int size)
		{
			int index = 0;
			
			while (index < size)
			{
				int read = _stream.Read(data, index, size-index);
				
				if (read==0)
					throw new EndOfStreamException (String.Format("End of stream reached with {0} byte{1} left to read.", size-index, size-index==1 ? "s" : ""));
				
				index += read;
			}
		}
		
		
		
		/// <summary>
		/// Read a byte from stream, checking to see whether read failed
		/// </summary>
		private byte InternalReadByte ()
		{
			int v = _stream.ReadByte();
			
			if (v >= 0)
				return (byte)v;
			else
				throw new EndOfStreamException ("End of stream reached with 1 byte left to read");
		}

		
		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
				{ _stream.Dispose(); }
			catch
				{ }
		}
		
		
		// Variables
		
		private BufferedRandomAccessFile	_stream;
		private Int16Union					_int16 = new Int16Union(0);
		private Int32Union					_int32 = new Int32Union(0);
		private Long64Union					_int64 = new Long64Union(0);
		private Float64Union				_float64 = new Float64Union(0);
	}	
	
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/EndianStreams.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	/// <summary>
	/// Provides network normalized stream reader & writer given endian-ness of architecture
	/// </summary>
	public class EndianStreams
	{
		public enum Endian
			{ Little, Big, Network }
		

				
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		public static IBinaryConversions ConversionsFor (Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return new SameEndianConverter ();
			else
				return new SwapEndianConverter ();			
		}

		
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		public static IBinaryReader ReaderFor (Stream stream, Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return (stream is BufferedRandomAccessFile) ? 
					(IBinaryReader)new SameEndianBufferedReader ((BufferedRandomAccessFile)stream) :
					(IBinaryReader)new SameEndianReader (stream);
			else
				return new SwapEndianReader (stream);			
		}

		
		/// <summary>
		/// Creates reader that converts from network-normalized form to local.
		/// To make this efficient the provided stream must be buffered.
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		public static IBinaryWriter WriterFor (Stream stream, Endian endian = Endian.Network)
		{
			if (endian == Endian.Network)
				endian = Endian.Big;
			
			Endian local = LocalEndian;
			if (local == endian)
				return (stream is BufferedRandomAccessFile) ? 
					(IBinaryWriter)new SameEndianBufferedWriter ((BufferedRandomAccessFile)stream) :
					(IBinaryWriter)new SameEndianWriter (stream);
			else
				return new SwapEndianWriter (stream);			
		}

		
		// Implementation

		
		/// <summary>
		/// Determine what our architecture is
		/// </summary>
		private static Endian LocalEndian
		{
			get
			{
				Int32Union u = new Int32Union (1);
				if (u.b1 == 1)
					return Endian.Little;
				else
					return Endian.Big;
			}
		}
			
	}	
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/EndianWriters.cs
// -------------------------------------------
﻿// 
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
 


namespace bridge.common.io
{
	/// <summary>
	///  Writes binary data, swapping endianness
	/// </summary>
	public class SwapEndianWriter : IBinaryWriter
	{
		public SwapEndianWriter (Stream stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt16 (short value)
		{
			_int16.i = unchecked((ushort)value);
			_stream.WriteByte(_int16.b2);
			_stream.WriteByte(_int16.b1);
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt32 (int value)
		{
			_int32.i = unchecked((uint)value);
			_stream.WriteByte(_int32.b4);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b1);
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt64 (long value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b8);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b1);
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt16 (ushort value)
		{
			_int16.i = value;
			_stream.WriteByte(_int16.b2);
			_stream.WriteByte(_int16.b1);
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt32 (uint value)
		{
			_int32.i = value;
			_stream.WriteByte(_int32.b4);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b1);
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt64 (ulong value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b8);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b1);
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteDouble (double value)
		{
			_float64.d = value;
			_stream.WriteByte(_float64.b8);
			_stream.WriteByte(_float64.b7);
			_stream.WriteByte(_float64.b6);
			_stream.WriteByte(_float64.b5);
			_stream.WriteByte(_float64.b4);
			_stream.WriteByte(_float64.b3);
			_stream.WriteByte(_float64.b2);
			_stream.WriteByte(_float64.b1);
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			value = value ?? "";
			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}
		
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;

			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));

			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
		
	}
	
	
	/// <summary>
	///  Writes binary data, preserving endianness
	/// </summary>
	public class SameEndianWriter : IBinaryWriter
	{
		public SameEndianWriter (Stream stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt16 (short value)
		{
			_int16.i = unchecked((ushort)value);
			_stream.WriteByte(_int16.b1);
			_stream.WriteByte(_int16.b2);
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt32 (int value)
		{
			_int32.i = unchecked((uint)value);
			_stream.WriteByte(_int32.b1);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b4);
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteInt64 (long value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b1);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b8);
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt16 (ushort value)
		{
			_int16.i = value;
			_stream.WriteByte(_int16.b1);
			_stream.WriteByte(_int16.b2);
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt32 (uint value)
		{
			_int32.i = value;
			_stream.WriteByte(_int32.b1);
			_stream.WriteByte(_int32.b2);
			_stream.WriteByte(_int32.b3);
			_stream.WriteByte(_int32.b4);
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUInt64 (ulong value)
		{
			_int64.l = unchecked((ulong)value);
			_stream.WriteByte(_int64.b1);
			_stream.WriteByte(_int64.b2);
			_stream.WriteByte(_int64.b3);
			_stream.WriteByte(_int64.b4);
			_stream.WriteByte(_int64.b5);
			_stream.WriteByte(_int64.b6);
			_stream.WriteByte(_int64.b7);
			_stream.WriteByte(_int64.b8);
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteDouble (double value)
		{
			_float64.d = value;
			_stream.WriteByte(_float64.b1);
			_stream.WriteByte(_float64.b2);
			_stream.WriteByte(_float64.b3);
			_stream.WriteByte(_float64.b4);
			_stream.WriteByte(_float64.b5);
			_stream.WriteByte(_float64.b6);
			_stream.WriteByte(_float64.b7);
			_stream.WriteByte(_float64.b8);
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			if (value == null)
				throw new ArgumentNullException("value");

			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}


		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			
			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));
			
			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private Stream			_stream;
		
		private Int16Union		_int16 = new Int16Union(0);
		private Int32Union		_int32 = new Int32Union(0);
		private Long64Union		_int64 = new Long64Union(0);
		private Float64Union	_float64 = new Float64Union(0);
		
	}

	
	
	
	/// <summary>
	///  Writes binary data, preserving endianness
	/// </summary>
	public class SameEndianBufferedWriter : IBinaryWriter
	{
		public SameEndianBufferedWriter (BufferedRandomAccessFile stream)
		{
			_stream = stream;
		}
		
		
		// Operations
		
		
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		public void Close()
		{
			_stream.Flush();
			_stream.Close();
		}
		
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_stream.Flush();
		}

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		public void Seek (int offset, SeekOrigin origin)
		{
			_stream.Seek (offset, origin);
		}
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteBool (bool value)
		{
			_stream.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt16 (short value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					short* pshort = (short*)(pbuf + offset);
					*pshort = value;
				}
			}
			else
			{
				_int16.i = unchecked((ushort)value);
				_stream.WriteByte(_int16.b1);
				_stream.WriteByte(_int16.b2);
			}
		}

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt32 (int value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					int* pint = (int*)(pbuf + offset);
					*pint = value;
				}
			}
			else
			{
				_int32.i = unchecked((uint)value);
				_stream.WriteByte(_int32.b1);
				_stream.WriteByte(_int32.b2);
				_stream.WriteByte(_int32.b3);
				_stream.WriteByte(_int32.b4);
			}
		}

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteInt64 (long value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					long* plong = (long*)(pbuf + offset);
					*plong = value;
				}
			}
			else
			{
				_int64.l = unchecked((ulong)value);
				_stream.WriteByte(_int64.b1);
				_stream.WriteByte(_int64.b2);
				_stream.WriteByte(_int64.b3);
				_stream.WriteByte(_int64.b4);
				_stream.WriteByte(_int64.b5);
				_stream.WriteByte(_int64.b6);
				_stream.WriteByte(_int64.b7);
				_stream.WriteByte(_int64.b8);
			}
		}

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt16 (ushort value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 2))
			{
				fixed (byte* pbuf = buffer)
				{
					ushort* pshort = (ushort*)(pbuf + offset);
					*pshort = value;
				}
			}
			else
			{
				_int16.i = value;
				_stream.WriteByte(_int16.b1);
				_stream.WriteByte(_int16.b2);
			}
		}

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt32 (uint value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 4))
			{
				fixed (byte* pbuf = buffer)
				{
					uint* pint = (uint*)(pbuf + offset);
					*pint = value;
				}
			}
			else
			{
				_int32.i = value;
				_stream.WriteByte(_int32.b1);
				_stream.WriteByte(_int32.b2);
				_stream.WriteByte(_int32.b3);
				_stream.WriteByte(_int32.b4);
			}
		}
		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteUInt64 (ulong value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					ulong* plong = (ulong*)(pbuf + offset);
					*plong = value;
				}
			}
			else
			{
				_int64.l = value;
				_stream.WriteByte(_int64.b1);
				_stream.WriteByte(_int64.b2);
				_stream.WriteByte(_int64.b3);
				_stream.WriteByte(_int64.b4);
				_stream.WriteByte(_int64.b5);
				_stream.WriteByte(_int64.b6);
				_stream.WriteByte(_int64.b7);
				_stream.WriteByte(_int64.b8);
			}
		}

		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		unsafe public void WriteDouble (double value)
		{
			byte[] buffer;
			long offset;
			
			if (_stream.WriteExtent (out buffer, out offset, 8))
			{
				fixed (byte* pbuf = buffer)
				{
					double* pdbl = (double*)(pbuf + offset);
					*pdbl = value;
				}
			}
			else
			{
				_float64.d = value;
				_stream.WriteByte(_float64.b1);
				_stream.WriteByte(_float64.b2);
				_stream.WriteByte(_float64.b3);
				_stream.WriteByte(_float64.b4);
				_stream.WriteByte(_float64.b5);
				_stream.WriteByte(_float64.b6);
				_stream.WriteByte(_float64.b7);
				_stream.WriteByte(_float64.b8);
			}
		}

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteByte (byte value)
		{
			_stream.WriteByte (value);
		}

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		public void WriteUByte (sbyte value)
		{
			_stream.WriteByte (unchecked((byte)value));
		}

		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		public void Write (byte[] value, int offset, int count)
		{
			_stream.Write(value, offset, count);
		}

		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		public void WriteString (string value, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			if (value == null)
				throw new ArgumentNullException("value");

			byte[] data = encoding.GetBytes(value);
			WriteUInt32 ((uint)data.Length);
			_stream.Write(data, 0, data.Length);
		}


		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		public void WriteStringField (string value, int size, Encoding encoding = null)
		{
			value = value ?? "";
			if (encoding == null)
				encoding = ASCIIEncoding.ASCII;
			
			// write string
			byte[] data = encoding.GetBytes(value);
			_stream.Write(data, 0, Math.Min (data.Length, size));
			
			// pad field with nulls
			for (int i = data.Length ; i < size ; i++)
				_stream.WriteByte(0);
		}

		
		// Implementation
		

		/// <summary>
		/// Disposes of the underlying stream.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Flush();
				Close();
			}
			catch
				{ }
		}
		
		
		
		// Variables
		
		private BufferedRandomAccessFile	_stream;
		
		private Int16Union					_int16 = new Int16Union(0);
		private Int32Union					_int32 = new Int32Union(0);
		private Long64Union					_int64 = new Long64Union(0);
		private Float64Union				_float64 = new Float64Union(0);
		
	}
	
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/IBinaryConversions.cs
// -------------------------------------------
﻿// 
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
 


namespace bridge.common.io
{
	/// <summary>
	/// Interface for converting endian bytes into proper form 
	/// </summary>
	public interface IBinaryConversions
	{
		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		short 					ReadInt16(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		int 					ReadInt32(byte[] buffer, int offset);	

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		long 					ReadInt64(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		ushort 					ReadUInt16(byte[] buffer, int offset);

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		uint 					ReadUInt32(byte[] buffer, int offset);		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		ulong 					ReadUInt64(byte[] buffer, int offset);
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		double 					ReadDouble(byte[] buffer, int offset);		

		/// <summary>
		/// Writes a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		void 					WriteInt16(byte[] buffer, int offset, short v);

		/// <summary>
		/// Writes a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		void 					WriteInt32(byte[] buffer, int offset, int v);	

		/// <summary>
		/// Writes a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		void 					WriteInt64(byte[] buffer, int offset, long v);

		/// <summary>
		/// Writes a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		void 					WriteUInt16(byte[] buffer, int offset, ushort v);

		/// <summary>
		/// Writes a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		void 					WriteUInt32(byte[] buffer, int offset, uint v);		
		
		/// <summary>
		/// Writes a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		void 					WriteUInt64(byte[] buffer, int offset, ulong v);
		
		/// <summary>
		/// Writes a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		void 					WriteDouble(byte[] buffer, int offset, double v);		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/IBinaryReader.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	/// <summary>
	/// Interface for binary stream reader 
	/// </summary>
	public interface IBinaryReader : IDisposable
	{
		/// <summary>
		/// Closes the reader, including the underlying stream..
		/// </summary>
		void 					Close();

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		void 					Seek (int offset, SeekOrigin origin);
		
		/// <summary>
		/// Reads a single byte from the stream.
		/// </summary>
		/// <returns>The byte read or -1 if EOF seen</returns>
		int 					ReadByte();

		/// <summary>
		/// Reads a boolean from the stream. 1 byte is read.
		/// </summary>
		/// <returns>The boolean read</returns>
		bool 					ReadBoolean();

		/// <summary>
		/// Reads a 16-bit signed integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit integer read</returns>
		short 					ReadInt16();

		/// <summary>
		/// Reads a 32-bit signed integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit integer read</returns>
		int 					ReadInt32();	

		/// <summary>
		/// Reads a 64-bit signed integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit integer read</returns>
		long 					ReadInt64();

		/// <summary>
		/// Reads a 16-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 2 bytes are read.
		/// </summary>
		/// <returns>The 16-bit unsigned integer read</returns>
		ushort 					ReadUInt16();

		/// <summary>
		/// Reads a 32-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 4 bytes are read.
		/// </summary>
		/// <returns>The 32-bit unsigned integer read</returns>
		uint 					ReadUInt32();		
		
		/// <summary>
		/// Reads a 64-bit unsigned integer from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The 64-bit unsigned integer read</returns>
		ulong 					ReadUInt64();
		
		/// <summary>
		/// Reads a double-precision floating-point value from the stream, using the bit converter
		/// for this reader. 8 bytes are read.
		/// </summary>
		/// <returns>The floating point value read</returns>
		double 					ReadDouble();
		
		/// <summary>
		/// Reads the specified number of bytes into the given buffer, starting at
		/// the given index.
		/// </summary>
		/// <param name="buffer">The buffer to copy data into</param>
		/// <param name="index">The first index to copy data into</param>
		/// <param name="count">The number of bytes to read</param>
		/// <returns>The number of bytes actually read. This will only be less than
		/// the requested number of bytes if the end of the stream is reached.
		/// </returns>
		int 					Read (byte[] buffer, int index, int count);

		/// <summary>
		/// Reads a length-prefixed string from the stream, using the encoding for this reader.
		/// A 7-bit encoded integer is first read, which specifies the number of bytes 
		/// to read from the stream. These bytes are then converted into a string with
		/// the encoding for this reader.
		/// </summary>
		/// <returns>The string read from the stream.</returns>
		string 					ReadString (Encoding encoding = null);
		
		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="size">Field size.</param>
		/// <param name="encoding">Encoding.</param>
		string 					ReadStringField (int size, Encoding encoding = null);

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/IBinaryWriter.cs
// -------------------------------------------
﻿// 
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
 

namespace bridge.common.io
{
	/// <summary>
	/// Interface for binary stream writer 
	/// </summary>
	public interface IBinaryWriter : IDisposable
	{
		/// <summary>
		/// Closes the writer, including the underlying stream.
		/// </summary>
		void 					Close();
		
		/// <summary>
		/// Flushes the underlying stream.
		/// </summary>
		void 					Flush();

		/// <summary>
		/// Seeks within the stream.
		/// </summary>
		/// <param name="offset">Offset to seek to.</param>
		/// <param name="origin">Origin of seek operation.</param>
		void 					Seek (int offset, SeekOrigin origin);
		

		/// <summary>
		/// Writes a boolean value to the stream. 1 byte is written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteBool (bool value);

		/// <summary>
		/// Writes a 16-bit signed integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteInt16 (short value);

		/// <summary>
		/// Writes a 32-bit signed integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void					WriteInt32 (int value);

		/// <summary>
		/// Writes a 64-bit signed integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteInt64 (long value);

		/// <summary>
		/// Writes a 16-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 2 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt16 (ushort value);

		/// <summary>
		/// Writes a 32-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 4 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt32 (uint value);
		
		/// <summary>
		/// Writes a 64-bit unsigned integer to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUInt64 (ulong value);
		
		/// <summary>
		/// Writes a double-precision floating-point value to the stream, using the bit converter
		/// for this writer. 8 bytes are written.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteDouble (double value);

		/// <summary>
		/// Writes a signed byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteByte (byte value);

		/// <summary>
		/// Writes an unsigned byte to the stream.
		/// </summary>
		/// <param name="value">The value to write</param>
		void 					WriteUByte (sbyte value);
		
		/// <summary>
		/// Writes a portion of an array of bytes to the stream.
		/// </summary>
		/// <param name="value">An array containing the bytes to write</param>
		/// <param name="offset">The index of the first byte to write within the array</param>
		/// <param name="count">The number of bytes to write</param>
		void 					Write (byte[] value, int offset, int count);
		
		/// <summary>
		/// Writes a string to the stream, using the encoding for this writer.
		/// </summary>
		/// <param name="value">The value to write. Must not be null.</param>
		/// <exception cref="ArgumentNullException">value is null</exception>
		void 					WriteString (string value, Encoding encoding = null);

		/// <summary>
		/// Reads a string in a fixed size field (null terminated)
		/// </summary>
		/// <returns>The string</returns>
		/// <param name="value">String to write.</param>
		/// <param name="size">Size of field.</param>
		/// <param name="encoding">Encoding.</param>
		void 					WriteStringField (string value, int size, Encoding encoding = null);

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/IOUtils.cs
// -------------------------------------------
﻿// 
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





namespace bridge.common.io
{
	/// <summary>
	/// IO related functions
	/// </summary>
	public class IOUtils
	{
		// reading functions
			
		
		/**
		 * read a line from stream, delimited by \n
		 * 
		 * @param stream		stream to read from
		 * @return				string for line read in or null if EOF
		 * @throws 			IOException
		 */
		public static string ReadLine (Stream stream)
		{
			StringBuilder line = new StringBuilder (1024);
	
			while (true)
			{
				int c = stream.ReadByte();
				if (c == -1)
					return (line.Length > 0) ? line.ToString() : null;
				if (c == '\n')
					return line.ToString();
				else
					line.Append ((char)c);
			}
		}


		/// <summary>
		/// Read until end of Json block
		/// </summary>
		/// <returns>The line.</returns>
		/// <param name="stream">Stream.</param>
		public static string ReadJSonBlock (Stream stream)
		{
			StringBuilder block = new StringBuilder (4096);
			const char NOTQUOTED = (char)0;

			var stack = new Stack<char> ();
			var quoted = NOTQUOTED;
			var raw = 0;

			do
			{
				raw = stream.ReadByte();
				if (raw < 0)
					return block.ToString();

				var c = (char)raw;
				block.Append (c);

				switch (c)
				{
					case '[':
					case '{':
						if (quoted != NOTQUOTED)
							break;
						stack.Push (c);
						break;
					case '"':
					case '\'':
						if (quoted == c)
							quoted = NOTQUOTED;
						else if (quoted == NOTQUOTED)
							quoted = c;
						break;
					case ']':
						if (quoted == NOTQUOTED)
						{
							var top = stack.Pop();
							if (top != '[')
								throw new ArgumentException ("encountered invalid JSON in stream: " + block);
						}
						break;
					case '}':
						if (quoted == NOTQUOTED)
						{
							var top = stack.Pop();
							if (top != '{')
								throw new ArgumentException ("encountered invalid JSON in stream: " + block);
						}
						break;
				}
			}
			while (stack.Count > 0 || raw <= 32);

			return block.ToString ();
		}
				
		
		/**
		 * read a line from stream, reading repeatedly until not empty, delimited by \n
		 * 
		 * @param stream		stream to read from
		 * @return				string for line read in or null if EOF
		 * @throws 			IOException
		 */
		public static string ReadNonEmptyLine (Stream stream)
		{
			while (true)
			{
				// get next line
				string line = ReadLine (stream);
	
				// end of stream?
				if (line == null)
					return null;
	
				// if has non-whitespace, return
				if (!StringUtils.IsBlank(line))
					return line;
			}
		}
		
		
		/**
		 * read exactly N bytes from the given stream
		 * 
		 * @param stream		stream to read from
		 * @param buffer		buffer to place result into
		 * @param n				number of bytes to read
		 * @return				buffer of n bytes or null if failed
		 * @throws 			IOException
		 */
		public static Blob ReadExact (Stream stream, int n, Blob buffer = null)
		{
			if (buffer == null)
				buffer = new Blob();
			
			while (n > 0)
			{
				// acquire buffer for read
				var region = buffer.Acquire (n, false);
	
				// read into buffer
				int cn = stream.Read (region.Bytes, region.Offset, Math.Min (n, region.Span));
				// error, reached EOS prematurely
				if (cn == 0) 
					return null;
					
				// otherwise proceed
				n -= cn;
				region.Used += cn;
			}	
	
			return buffer;
	
		}
		

		/// <summary>
		/// read exactly N bytes from the given stream, or returns null
		/// </summary>
		/// <param name='stream'>
		/// Stream.
		/// </param>
		/// <param name='n'>
		/// N.
		/// </param>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public static byte[] ReadExact (Stream stream, int n, byte[] buffer)
		{
			var offset = 0;
			while (n > 0)
			{
				// read into buffer
				int cn = stream.Read (buffer, offset, n);
				// error, reached EOS prematurely
				if (cn == 0) 
					return null;
					
				n -= cn;
				offset += cn;
			}	
	
			return buffer;
		}
	
	
		
		/**
		 * read stream to end
		 * 
		 * @param stream		stream to read from
		 * @return				buffer containing stream content
		 * @throws 			IOException
		 */
		public static Blob ReadToEnd (Stream stream)
		{
			Blob buffer = new Blob ();
	
			while (true)
			{
				// acquire buffer for read
				var region = buffer.Acquire (4096, false);
	
				int n = stream.Read (region.Bytes, region.Offset, region.Span);
				if (n < 0)
				{
				    stream.Close();
					return buffer;
				} else
					region.Used += n;
			}
	
		}
		
			
		
		/**
		 * Read stream until specified terminator encountered
		 * 
		 * @param stream		stream to read from
		 * @param terminator	terminator
		 * @return				buffer spanning until terminator
		 */
		public static Blob ReadUntil (Stream stream, string terminator)
		{
			Blob buffer = new Blob();
	
			// position in terminator
			int tpos = 0;
			int tlen = terminator.Length;
	
			while (true)
			{
				int c = stream.ReadByte ();
	
				// if premature EOF return null
				if (c == -1)
					return null;
	
				// keep track of where we are terminator-wise
				tpos = (c == terminator[tpos]) ? tpos+1 : 0;
			
				// if terminator reached, return content (minus terminator)
				if (tpos == tlen)
				{ 
					buffer.Length = (buffer.Length - (tlen-1)); 
					return buffer; 
				}
	
				buffer.Append ((byte)c);
			}
	
		}
			
		
		
		/**
		 * Copy complete input stream to output stream.  Input stream is closed (possibly) after
		 * reading completes, output stream remains open.
		 * 
		 * @param istream		input stream to read to completion
		 * @param ostream		output stream
		 */
		public static void Copy (Stream istream, Stream ostream, bool close = true)
		{
			byte[] rbuf = new byte[4096];
			
			while (true)
			{
				int n = istream.Read (rbuf, 0, 4096);
				if (n <= 0)
					{ if (close) istream.Close(); return; }
				else
					ostream.Write (rbuf, 0, n);
			}		
		}
	
		
		
		/**
		 * Copy specified # of bytes from input stream to output stream.  
		 * 
		 * @param istream		input stream to read to completion
		 * @param ostream		output stream
		 * @param len			amount to copy
		 */
		public static void Copy (Stream istream, Stream ostream, int len)
		{
			byte[] rbuf = new byte[4096];
			
			while (len > 0)
			{
				int amount = Math.Min (len, 4096);
				int n = istream.Read (rbuf, 0, amount);
				ostream.Write (rbuf, 0, n);
				len -= n;
			}		
		}
		
		
		
		// writing functions
		
		
		/**
		 * write an ASCII string to stream
		 * 
		 * @param stream	stream to write to
		 * @param string	string to write
		 */
		public static void Write (Stream stream, string s)
		{
			for (int i = 0 ; i < s.Length ; i++)
				stream.WriteByte ((byte)s[i]);
		}
	
		
		/**
		 * Write buffer to stream
		 * 
		 * @param stream	stream to write to
		 * @param buffer	buffer to write
		 */
		public static void write (Stream stream, Blob buffer)
		{
			foreach (var region in buffer.Regions)
			{
				stream.Write (region.Bytes, 0, region.Used);	
			}
		}
		
		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/io/NetUtils.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.io
{
	public class NetUtils
	{

		/// <summary>
		/// Determines whether the given hostname or address is this host
		/// </summary>
		/// <param name='host'>
		/// host to test
		/// </param>
		public static bool IsHostLocal (string host)
		{
			var localhost = Dns.GetHostName();
			if (host == localhost)
				return true;

			var local = AddressesOf (localhost);
			var test = AddressesOf (host);
			for (int li = 0 ; li < local.Length ; li++)
			{
				for (int ti = 0 ; ti < test.Length ; ti++)
				{
					if (local[li].Equals (test[ti]))
						return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Determine the address of the address as string or hostname
		/// </summary>
		/// <param name='host'>
		/// Address or host name
		/// </param>
		public static IPAddress[] AddressesOf (string host)
		{
			try
			{
				return Dns.GetHostAddresses (host);
			}
			catch (Exception)
			{
				return new IPAddress[0];
			}
		}


		/// <summary>
		/// Convert a hostname based URL to an address
		/// </summary>
		/// <returns>The address based.</returns>
		/// <param name="url">URL.</param>
		public static Uri ToAddressBased (Uri url)
		{
			IPAddress[] addrs = null;

			switch (url.HostNameType)
			{
				case UriHostNameType.IPv4:
				case UriHostNameType.IPv6:
					return url;

				case UriHostNameType.Dns:
					if (url.Host == "localhost")
						addrs = Dns.GetHostAddresses (Dns.GetHostName());
					else
						addrs = Dns.GetHostAddresses (url.Host);

					return new Uri(url.Scheme + "://" + addrs [0] + ":" + url.Port + url.PathAndQuery);

				default:
					return url;
			}
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/ColumnViewVector.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Column View vector is a read-only view on a matrix column
	/// </summary>
	public class ColumnViewVector : Vector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.SubVector"/> class.
		/// </summary>
		/// <param name='underlier'>
		/// Underlying parent vector
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='n'>
		/// Length of sub vector or defaults to remaining length from offset
		/// </param>
		public ColumnViewVector (Matrix<double> underlier, int col)
			: base (new SubviewStorage (underlier, col))
		{
			_data = (SubviewStorage)Storage;
		}

		// Properties

		public int Column
			{ get { return ((SubviewStorage)Storage).Column; } }

		public Matrix<double> Underlier
			{ get { return ((SubviewStorage)Storage).Underlier; } }


		#region Storage

		/// <summary>
		/// Storage for subview
		/// </summary>
		private class SubviewStorage : VectorStorage<double>
		{
			public SubviewStorage (Matrix<double> underlier, int col)
				: base (underlier.RowCount)
			{
				Underlier = underlier;
				Column = col;
			}

			// properties

			/// <summary>
			/// The underlier.
			/// </summary>
			public Matrix<double> Underlier;

			/// <summary>
			/// The offset.
			/// </summary>
			public int Column;
		
			/// <summary>
			/// True if the vector storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }


			// Functions

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int index)
			{
				return Underlier [index, Column];
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="value">The value to set the element to. </param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int index, double value)
			{
				Underlier [index, Column] = value;
			}
		}

		#endregion

		// Variables

		private SubviewStorage		_data;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/EvaluatedVector.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Sub vector is a read-only view on a sub-range of a parent vector
	/// </summary>
	public class EvaluatedVector : Vector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.SubVector"/> class.
		/// </summary>
		/// <param name='underlier'>
		/// Underlying parent vector
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='n'>
		/// Length of sub vector or defaults to remaining length from offset
		/// </param>
		public EvaluatedVector (int size, Func<int,double> filler)
			: base (new EvaluatedStorage (size, filler))
		{
		}
		
		
		#region Implementation

		private class EvaluatedStorage : VectorStorage<double>
		{
			public EvaluatedStorage (int size, Func<int,double> filler)
				: base (size)
			{
				_filler = filler;
			}

			// Properties

			/// <summary>
			/// True if the vector storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }


			// Functions

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int index)
			{
				return _filler (index);
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="value">The value to set the element to. </param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int index, double value)
			{
				throw new AccessViolationException ("cannot write to an evaluated vector");
			}


			// Variables

			private Func<int,double> _filler;
		}


		#endregion
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/IIndexByName.cs
// -------------------------------------------
﻿// 
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



namespace bridge.math.matrix
{
	/// <summary>
	/// List of names used as indices for rows or columns
	/// <para>
	/// Keeps track of position of elements in list so that can directly look into a matrix structure
	/// </para>
	/// </summary>
	public interface IIndexByName
	{		
		/// <summary>
		/// Given a name determines the position in matrix or other structure
		/// </summary>
		/// <value>
		/// The ordering.
		/// </value>
		Dictionary<string,int> 				Ordering		{ get; }
		
		/// <summary>
		/// Gets the name list as string[]
		/// </summary>
		string[]							NameList		{ get; }

		/// <summary>
		/// Gets the count.
		/// </summary>
		int									Count			{ get; }
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/IndexByName.cs
// -------------------------------------------
﻿// 
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



namespace bridge.math.matrix
{
	/// <summary>
	/// List of names used as indices for rows or columns
	/// <para>
	/// Keeps track of position of elements in list so that can directly look into a matrix structure
	/// </para>
	/// </summary>
	public class IndexByName<T> : List<T>, IIndexByName
	{
		public IndexByName (params T[] names)
		{
			int i = 0;
			foreach (T item in names)
			{
				Add(item);
				_ordering[item.ToString()] = i++;
			}
		}
		
		public IndexByName (IEnumerable<T> c)
		{
			int i = 0;
			foreach (T item in c)
			{
				Add(item);
				_ordering[item.ToString()] = i++;
			}
		}
		
		
		// Properties
		
		
		/// <summary>
		/// Given a name determines the position in matrix or other structure
		/// </summary>
		/// <value>
		/// The ordering.
		/// </value>
		public Dictionary<string,int> Ordering 
			{ get { return _ordering;} }
		
		
		/// <summary>
		/// Gets the name list as string[]
		/// </summary>
		public string[] NameList
		{ 
			get 
			{ 
				string[] list = new string[Count];
				for (int i = 0 ; i < Count ; i++)
					list[i] = this[i].ToString();
				
				return list;
			} 
		}

		
		// Operations
		
		
		/// <summary>
		/// Add the specified name.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public new void Add (T name)
		{
			base.Add (name);
			
			var key = name.ToString();
			_ordering[key] = Count-1;
		}

		
		/// <summary>
		/// Removes name at index, shifting list
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		public new void RemoveAt (int index)
		{
			base.RemoveAt(index);
			_ordering.Clear();
			
			int i = 0;
			foreach (var o in this)
				_ordering[o.ToString()] = i++; 
		}
		
		
		/// <summary>
		/// Clear list
		/// </summary>
		public new void Clear ()
		{
			base.Clear();
			_ordering.Clear();
		}
		
		
		/// <summary>
		/// Index of the name
		/// </summary>
		/// <param name='item'>
		/// name
		/// </param>
		public new int IndexOf (T name)
		{
			return _ordering[name.ToString()];
		}


		/// <summary>
		/// Index of the name
		/// </summary>
		/// <param name='item'>
		/// name
		/// </param>
		public bool TryIndexOf (T name, out int idx)
		{
			return _ordering.TryGetValue (name.ToString(), out idx);
		}

		
		/// <summary>
		/// Insert the name at specified index
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		public new void Insert (int index, T name)
		{
			base.Insert(index, name);
			_ordering.Clear();
			
			int i = 0;
			foreach (var o in this)
				_ordering[o.ToString()] = i++; 
		}
		
		/// <summary>
		/// Converts index to string[]
		/// </summary>
		/// <returns>
		/// The new array containing a copy of the list's elements.
		/// </returns>
		public new string[] ToArray()
		{
			string[] list = new string[Count];
			for (int i = 0 ; i < Count ; i++)
				list[i] = this[i].ToString ();
			
			return list;
		}
		
		
		// Variables
		
		private Dictionary<string,int>	_ordering = new Dictionary<string,int>();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/IndexedMatrix.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Matrix with named indices & expansion
	/// </summary>
	public class IndexedMatrix : DenseMatrix
	{
		/// <summary>
		/// Matrix with no size (and no data allocation)
		/// <p/>
		/// Should only be used in the context of serialization or internal functions
		/// </summary>
		public IndexedMatrix ()
			: this (0,0)
		{
		}
			
				
		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			int rows, int cols, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)
			
			: base (rows, cols)
		{
			_rowindices = rownames;
			_colindices = colnames;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.IndexedMatrix"/> class.
		/// </summary>
		/// <param name="src">Source matrix.</param>
		/// <param name="rownames">Row names.</param>
		/// <param name="colnames">Col names.</param>
		public IndexedMatrix (
			Matrix<double> src, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)

			: base (src.RowCount, src.ColumnCount)
		{
			_rowindices = rownames;
			_colindices = colnames;
			src.CopyTo (this);
		}


		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			double[] data,
			int rows, int cols, 
			IIndexByName rownames = null, 
			IIndexByName colnames = null)

			: base (rows, cols, data)
		{
			_rowindices = rownames;
			_colindices = colnames;
		}


		/// <summary>
		/// Create matrix with named indices and also allow matrix to grow # of columns (if Initialize.Alloc rather than Initialize.Size)
		/// </summary>
		/// <param name='rows'>
		/// Rows.
		/// </param>
		/// <param name='cols'>
		/// Cols.
		/// </param>
		/// <param name='rownames'>
		/// Rownames.
		/// </param>
		/// <param name='colnames'>
		/// Colnames.
		/// </param>
		/// <param name='init'>
		/// Indicate whether rows/cols represent total size (Initialize.Size) or whether cols represents a allocation (Initialize.Alloc)
		/// </param>
		public IndexedMatrix (
			double[] data,
			int rows, int cols, 
			string[] rownames = null, 
			string[] colnames = null)

			: base (rows, cols, data)
		{
			_rowindices = rownames != null ? new IndexByName<string> (rownames) : null;
			_colindices = colnames != null ? new IndexByName<string> (colnames) : null;
		}

		
		// Properties
		
		
		/// <summary>
		/// Gets the named row indices for this matrix (may be null if not given)
		/// </summary>
		public IIndexByName RowIndices
			{ get { return _rowindices; } }
		
		/// <summary>
		/// Gets the named col indices for this matrix (may be null if not given)
		/// </summary>
		public IIndexByName ColIndices
			{ get { return _colindices; } }
		
		/// <summary>
		/// Gets the row names as string[]
		/// </summary>
		public string[] RowNames
			{ get { return _rowindices != null ? _rowindices.NameList : null; } }
		
		/// <summary>
		/// Gets the col names as string[]
		/// </summary>
		public string[] ColNames
			{ get { return _colindices != null ? _colindices.NameList : null; } }
		
		
		/// <summary>
		/// Gets the underlying data for this vector
		/// </summary>
		public double[] Data
			{ get { return ((DenseColumnMajorMatrixStorage<double>)Storage).Data; } }
		
		
		// Functions
				
		
		/// <summary>
		/// Gets or sets the element given by row,col name on the matrix
		/// </summary>
		/// <param name='row'>
		/// Row index name
		/// </param>
		/// <param name='col'>
		/// Col index name
		/// </param>
		public virtual double this [string row, string col]
		{
			get 
			{ 
				int ridx = _rowindices.Ordering[row];
				int cidx = _colindices.Ordering[col];
				return base[ridx, cidx]; 
			}
			set 
			{ 
				int ridx = _rowindices.Ordering[row];
				int cidx = _colindices.Ordering[col];
				base[ridx, cidx] = value; 
			}
		}
		
		
		/// <summary>
		/// Sets all values in the matrix to given value
		/// </summary>
		/// <param name='newv'>
		/// New value to set
		/// </param>
		public void SetAll (double newv)
		{
			var n = RowCount*ColumnCount;
			var data = Data;

			for (int i = 0 ; i < n ; i++)
				data[i] = newv;
		}


		/// <summary>
		/// Rolls the rows in the matrix up or down, removing the K rows and filling with the lower (upper).   The
		/// remaining rows at the end of the shift have undefined values and should be overwritten.
		/// </summary>
		/// <param name="shift">If shift is negative, shifts rows up, and positive, shifts down</param>
		public void ShiftRows (int shift)
		{
			var ncol = ColumnCount;
			var nrow = RowCount;

			var data = Data;
			if (shift < 0)
			{
				for (int ci = 0; ci < ncol; ci++)
				{
					Array.Copy (data, ci * ncol - shift, data, ci * ncol, (nrow + shift));
				}
			}
			else
			{
				for (int ci = 0; ci < ncol; ci++)
				{
					Array.Copy (data, ci * ncol, data, ci * ncol + shift, (nrow - shift));
				}
			}
		}

					
		
		// Variables

		protected IIndexByName		
			_rowindices;
		protected IIndexByName		
			_colindices;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/IndexedVector.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Dense vector with index
	/// </summary>
	public sealed class IndexedVector : DenseVector
	{
		
		/// <summary>
		/// Initializes a new vector with given size
		/// </summary>
		/// <param name='size'>
		/// length of vector
		/// </param>
		/// <param name='names'>
		/// Optional name-based index
		/// </param>
		public IndexedVector (int size, IIndexByName names = null)
			: base (size)
		{
			_index = names;
		}


		/// <summary>
		/// Initializes a new vector with data array
        /// </summary>
        /// <param name="data">raw vector in form of array</param>
		/// <param name='names'>Optional name-based index</param>
		public IndexedVector(double[] data, IIndexByName names = null)
			: base(data)
		{
			_index = names;
		}


		/// <summary>
		/// Initializes a new vector with data array
		/// </param>
		/// <param name='names'>
		/// Optional name-based index
		/// </param>
		public IndexedVector(double[] data, string[] names)
			: base(data)
		{
			if (names != null)
				_index = new IndexByName<string>(names);
			else
				_index = null;
		}

		
		// Properties
		

        /// <summary>
        /// Gets the vector as an array
        /// </summary>
        public double[] Data
            { get { return base.AsArray(); } }

		
		/// <summary>
		/// Gets / Sets the named index for this vector (may be null if not provided)
		/// </summary>
		public IIndexByName Indices
			{ get { return _index; } set { _index = value; } }
		
		/// <summary>
		/// Gets the row names as string[]
		/// </summary>
		public string[] Names
			{ get { return _index != null ? _index.NameList : null; } }
		
		
		// Operations
					
		
		/// <summary>
		/// Gets or sets the element given by name on the vector
		/// </summary>
		/// <param name='name'>
		/// Row index name
		/// </param>
		public double this [string name]
		{
			get 
			{ 
				int idx = _index.Ordering[name];
				return base.At(idx); 
			}
			set 
			{ 
				int idx = _index.Ordering[name];
				base.At(idx, value); 
			}
		}


        // Variables

        private IIndexByName _index;
	}

}


// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/MatrixOps.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Various matrix operations
	/// </summary>
	public static class MatrixOps
	{
		/// <summary>
		/// Scale columns of transposed of matrix by vector entries C = A^T Diag(b)
		/// </summary>
		/// <param name='A'>
		/// 	matrix A
		/// </param>
		/// <param name='b'>
		/// 	diagonal vector b
		/// </param>
		/// <returns>
		/// 	new matrix C = A^T Diag(b)
		/// </returns>
		public static Matrix<double> MultMtD (Matrix<double> A, Vector<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, A.RowCount);
			
			var n = A.RowCount;
			var m = A.ColumnCount;
			
			Debug.Assert (n == b.Count, "mismatch between number of rows in A and diagonal vector");
			
			for (int ri = 0 ; ri < n ; ri++)
			{
				var scalar = b[ri];				
				for (int ci = 0 ; ci < m ; ci++)
				{
					mout[ci,ri]= A[ri,ci] * scalar;
				}
			}
			
			return mout;
		}
		
		
		/// <summary>
		/// Multiply M'M with a scale
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultSMtM (Matrix<double> A, double S = 1.0, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, A.ColumnCount);

			var nrow = A.RowCount;
			var ncol = A.ColumnCount;

			for (int ri = 0 ; ri < ncol ; ri++)
			{
				for (int ci = 0 ; ci < ncol ; ci++)
				{
					double sum = 0;
					for (int i = 0 ; i < nrow ; i++)
						sum += A[i,ri] * A[i,ci];
					
					mout [ri,ci] = sum * S;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply M' D M, where D is a diagonalized vector
		/// </summary>
		/// <param name='M'>matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name='mout'>Output matrix.</param>
		public static Matrix<double> MultMtDM (Matrix<double> M, Vector<double> D, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (M.ColumnCount, M.ColumnCount);

			var nrow = M.RowCount;
			var ncol = M.ColumnCount;
			for (int ri = 0; ri < ncol; ri++) 
			{
				for (int ci = 0; ci < ncol; ci++) 
				{
					double sum = 0;
					for (int i = 0; i < nrow; i++)
						sum += M [i, ri] * D[i] * M [i, ci];

					mout [ri, ci] = sum;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply X' D Y, where D is a diagonalized vector
		/// </summary>
		/// <param name='X'>X matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name="Y">Y matrix</param>
		/// <param name='mout'>Output matrix.</param>
		public static Matrix<double> MultXtDY (Matrix<double> X, Vector<double> D, Matrix<double> Y, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (X.ColumnCount, Y.ColumnCount);

			var nrow = X.RowCount;
			var ncol1 = X.ColumnCount;
			var ncol2 = Y.ColumnCount;

			for (int ri = 0; ri < ncol1; ri++) 
			{
				for (int ci = 0; ci < ncol2; ci++) 
				{
					double sum = 0;
					for (int i = 0; i < nrow; i++)
						sum += X [i, ri] * D [i] * Y [i, ci];

					mout [ri, ci] = sum;
				}
			}

			return mout;
		}


		/// <summary>
		/// Multiply X' D y, where D is a diagonalized vector
		/// </summary>
		/// <param name='M'>matrix</param>
		/// <param name="D">diagonal vector</param>
		/// <param name="y">Y vector</param>
		/// <param name='mout'>Output matrix.</param>
		public static Vector<double> MultXtDy (Matrix<double> X, Vector<double> D, Vector<double> y, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (X.ColumnCount);

			var nrow = X.RowCount;
			var ncol = X.ColumnCount;

			for (int ri = 0; ri < ncol; ri++) 
			{
				double sum = 0;
				for (int i = 0; i < nrow; i++)
					sum += X [i, ri] * D [i] * y [i];

				vout [ri] = sum;
			}

			return vout;
		}
		
		
		/// <summary>
		/// Multiply A'B, where A & B are separate matrices, but 
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMtM (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.ColumnCount, B.ColumnCount);
			
			A.TransposeThisAndMultiply (B, mout);
			return mout;
		}
		
		/// <summary>
		/// Multiply AB', where A & B are separate matrices, but 
		/// </summary>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMMt (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.RowCount, B.RowCount);
			
			A.TransposeAndMultiply (B, mout);
			return mout;
		}
		
		/// <summary>
		/// Multiply two matrices
		/// </summary>
		/// <returns>
		/// O = A * B
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='B'>
		/// Matrix B.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> MultMM (Matrix<double> A, Matrix<double> B, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (A.RowCount, B.ColumnCount);
			
			A.Multiply (B, mout);
			return mout;
		}
		
		
		/// <summary>
		/// Multiply matrix with vector v = A b
		/// </summary>
		/// <returns>
		/// v = A * b
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> MultMV (Matrix<double> A, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (A.RowCount);
			
			A.Multiply (b, vout);
			return vout;
		}
		
		/// <summary>
		/// Multiply matrix transpose with vector v = A'b
		/// </summary>
		/// <returns>
		/// v = A' * b
		/// </returns>
		/// <param name='A'>
		/// Matrix A
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> MultMtV (Matrix<double> A, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (A.ColumnCount);
			
			A.TransposeThisAndMultiply (b, vout);
			return vout;
		}
		
			
		/// <summary>
		/// Add two vectors
		/// </summary>
		/// <returns>
		/// v = a + b
		/// </returns>
		/// <param name='a'>
		/// vector a
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> AddVV (Vector<double> a, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (a.Count);
			
			var n = a.Count;
			
			for (int i = 0 ; i < n ; i++)
				vout[i] = a[i] + b[i];
			
			return vout;
		}

		/// <summary>
		/// Dot product of 2 vectors
		/// </summary>
		/// <returns>The product.</returns>
		/// <param name="a">The 1st vectpr.</param>
		/// <param name="b">The 2nd vector.</param>
		public static double DotProduct (Vector<double> a, Vector<double> b)
		{
			var n = a.Count;
			var cum = 0.0;
			for (int i = 0 ; i < n ; i++)
				cum += a[i] * b[i];

			return cum;
		}

			
		/// <summary>
		/// Add two matrix
		/// </summary>
		/// <returns>
		/// m = a + S b
		/// </returns>
		/// <param name='a'>
		/// matrix a
		/// </param>
		/// <param name='b'>
		/// matrix b.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> AddMM (Matrix<double> a, Matrix<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (a.RowCount, a.ColumnCount);
			
			var nrows = a.RowCount;
			var ncols = a.ColumnCount;
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					mout[ri,ci] = a[ri,ci] + b[ri,ci];
				}
			}

			return mout;
		}
		
			
		/// <summary>
		/// Subtract two matrices
		/// </summary>
		/// <returns>
		/// m = a - b
		/// </returns>
		/// <param name='a'>
		/// matrix a
		/// </param>
		/// <param name='b'>
		/// matrix b.
		/// </param>
		/// <param name='mout'>
		/// Output matrix.
		/// </param>
		public static Matrix<double> SubMM (Matrix<double> a, Matrix<double> b, Matrix<double> mout = null)
		{
			if (mout == null)
				mout = new DenseMatrix (a.RowCount, a.ColumnCount);
			
			var nrows = a.RowCount;
			var ncols = a.ColumnCount;
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					mout[ri,ci] = a[ri,ci] - b[ri,ci];
				}
			}

			return mout;
		}
		
			
		/// <summary>
		/// Subtract two vectors a-b
		/// </summary>
		/// <returns>
		/// v = a - b
		/// </returns>
		/// <param name='a'>
		/// vector a
		/// </param>
		/// <param name='b'>
		/// vector b.
		/// </param>
		/// <param name='vout'>
		/// Output vector.
		/// </param>
		public static Vector<double> SubVV (Vector<double> a, Vector<double> b, Vector<double> vout = null)
		{
			if (vout == null)
				vout = new DenseVector (a.Count);
			
			var n = a.Count;
			
			for (int i = 0 ; i < n ; i++)
				vout[i] = a[i] - b[i];
			
			return vout;
		}

		
		/// <summary>
		/// Solve X x = b, for x
		/// </summary>
		/// <param name='A'>
		/// matrix A
		/// </param>
		/// <param name='b'>
		/// vector b
		/// </param>
		/// <param name='x'>
		/// output vector x
		/// </param>
		public static Vector<double> Solve (Matrix<double> A, Vector<double> b, Vector<double> x = null)
		{
			return MultMV (A.Inverse(), b, x);
		}


		/// <summary>
		/// Diagonal matrix from vector
		/// </summary>
		/// <param name="diag">Diagonal values.</param>
		public static Matrix<double> Diag (Vector<double> diag)
		{
			var n = diag.Count;
			var M = new DenseMatrix (n,n);

			for (int i = 0; i < n; i++)
				M [i, i] = diag [i];

			return M;
		}


		/// <summary>
		/// Identity matrix
		/// </summary>
		/// <param name="n">N.</param>
		public static Matrix<double> Identity (int n)
		{
			var M = new DenseMatrix (n,n);

			for (int i = 0; i < n; i++)
				M [i, i] = 1.0;

			return M;
		}


		/// <summary>
		/// Raise matrix to given power
		/// </summary>
		/// <param name="A">square matrix to be raised to power</param>
		/// <param name="pow">power to raise to.</param>
		public static Matrix<double> Pow (Matrix<double> A, double pow)
		{
			var svd = A.Svd ();
			var W = svd.W;
			var U = svd.U;

			var D = new DenseVector (W.RowCount);

			for (int i = 0; i < D.Count; i++)
				D [i] = Math.Pow (W [i, i], pow);

			return U * MatrixOps.Diag(D) * U.Transpose ();
		}


		/// <summary>
		/// Sets all values in vector to given
		/// </summary>
		/// <param name="vec">Vector.</param>
		/// <param name="v">Value to set to.</param>
		public static void SetAll (Vector<double> vec, double v)
		{
			for (int i = 0; i < vec.Count; i++)
				vec [i] = v;
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/MatrixUtils.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	public class MatrixUtils
	{
		public enum InterpStyle
			{ Linear, Flat }

		/// <summary>
		/// Gets the underlying matrix data
		/// </summary>
		/// <param name='m'>
		/// M.
		/// </param>
		public static double[] DataOf (Matrix<double> matrix)
		{
			IndexedMatrix imat = matrix as IndexedMatrix;
			if (imat != null)
				return imat.Data;
			
			DenseMatrix dmat = matrix as DenseMatrix;
			if (dmat != null)
				return dmat.Values;
			else
				throw new ArgumentException ("cannot get underlying data for matrix of type: " + matrix.GetType());
		}
		

		/// <summary>
		/// Get the data for a vector
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="v">V.</param>
		/// <param name="copyif">If set to <c>true</c> will copy the data if inaccessible.</param>
		public static double[] DataOf (Vector<double> v, bool copyif = true)
		{
			var va = v as IndexedVector;
			if (va != null)
				return va.Data;
			
			var vb = v as SubviewVector;
			if (vb != null)
				return vb.Data;
			
			var vc = v as DenseVector;
			if (vc != null)
				return vc.Values;

			if (!copyif)
				throw new ArgumentException ("cannot access the data for the given vector, without copying");

			double[] data = new double[v.Count];
			for (int i = 0 ; i < v.Count; i++)
				data[i] = v[i];
			
			return data;
		}
		
		
		/// <summary>
		/// Get offset within raw data for start of vector
		/// </summary>
		public static int DataOffsetOf (Vector<double> v)
		{
			var va = v as SubviewVector;
			if (va != null)
				return va.DataOffset;
			else
				return 0;
		}


		/// <summary>
		/// Set all values of a vector to constant
		/// </summary>
		/// <param name="v">Vector.</param>
		/// <param name="constant">value to set in</param>
		public static void SetAll (Vector<double> v, double constant)
		{
			var len = v.Count;
			for (int i = 0; i < len; i++)
				v[i] = constant;
		}


		/// <summary>
		/// Indices of this vector (as string[] or null)
		/// </summary>
		/// <param name="vec">Vec.</param>
		public static IIndexByName IndicesOf (Vector<double> vec)
		{
			var ivec = vec as IndexedVector;
			if (ivec != null)
				return ivec.Indices != null ? ivec.Indices : null;
			else
				return null;
		}
		
		
		/// <summary>
		/// Row Indices of this matrix (as string[] or null)
		/// </summary>
		/// <param name="mat">Matrix.</param>
		public static IIndexByName RowIndicesOf (Matrix<double> mat)
		{
			var imat = mat as IndexedMatrix;
			if (imat != null)
				return imat.RowIndices != null ? imat.RowIndices : null;
			else
				return null;
		}
		
		
		/// <summary>
		/// Column Indices of this matrix (as string[] or null)
		/// </summary>
		/// <param name="mat">Matrix.</param>
		public static IIndexByName ColIndicesOf (Matrix<double> mat)
		{
			var imat = mat as IndexedMatrix;
			if (imat != null)
				return imat.ColIndices != null ? imat.ColIndices : null;
			else
				return null;
		}


		/// <summary>
		/// Converts the given matrix to an indexed matrix
		/// </summary>
		/// <param name='m'>
		/// M.
		/// </param>
		public static IndexedMatrix ToIndexedMatrix (Matrix<double> matrix)
		{
			IndexedMatrix imat = matrix as IndexedMatrix;
			if (imat != null)
				return imat;

			IIndexByName index = null;
			DenseMatrix dmat = matrix as DenseMatrix;
			if (dmat != null)
				return new IndexedMatrix (dmat.Values, dmat.RowCount, dmat.ColumnCount, index, index);

			imat = new IndexedMatrix (matrix.RowCount, matrix.ColumnCount, index, index);
			for (int ri = 0 ; ri < matrix.RowCount ; ri++)
			{
				for (int ci = 0 ; ci < matrix.ColumnCount ; ci++)
					imat[ri,ci] = matrix[ri,ci];
			}

			return imat;
		}

		
		/// <summary>
		/// Convert vectors that are either not dense or not indexed to dense
		/// </summary>
		public static Vector<double> ToDenseVector (Vector<double> v)
		{
			var va = v as IndexedVector;
			if (va != null)
				return va;
			
			var vc = v as DenseVector;
			if (vc != null)
				return vc;
			
			var len = v.Count;
			DenseVector newv = new DenseVector (len);
			
			for (int i = 0 ; i < len ; i++)
				newv[i] = v[i];
			
			return newv;
		}


		/// <summary>
		/// Combine vectors as columns to create new matrix
		/// </summary>
		/// <param name="cvecs">Column vector list</param>
		public static Matrix<double> CBind (Vector<double>[] cvecs)
		{
			var v1 = cvecs [0];
			IIndexByName idx = (v1 is IndexedVector) ? ((IndexedVector)v1).Indices : null;

			var mat = new IndexedMatrix (v1.Count, cvecs.Length, idx, null);
			for (int ci = 0 ; ci < cvecs.Length ; ci++)
			{
				var v = cvecs [ci];
				for (int ri = 0; ri < v1.Count; ri++)
					mat [ri, ci] = v [ri];
			}

			return mat;
		}

		
		/// <summary>
		/// Random matrix
		/// </summary>
		/// <returns>
		/// The matrix.
		/// </returns>
		/// <param name='nrows'>
		/// Nrows.
		/// </param>
		/// <param name='ncols'>
		/// Ncols.
		/// </param>
		public static DenseMatrix RandomMatrix (int nrows, int ncols)
		{
			DenseMatrix m = new DenseMatrix (nrows, ncols);
			Random rand = new Random ();
			
			for (int ri = 0 ; ri < nrows ; ri++)
			{
				for (int ci = 0 ; ci < ncols ; ci++)
				{
					m[ri,ci] = rand.NextDouble();
				}
			}
			
			return m;
		}
		
		
		/// <summary>
		/// Determines whether indexed matrix contains a seried of bars, based on column headers
		/// </summary>
		public static bool IsBars (IndexedMatrix m)
		{
			IIndexByName colnames = m.ColIndices;
			if (colnames == null)
				return false;
			
			var ordering = colnames.Ordering;
			if (ordering.ContainsKey ("close") || ordering.ContainsKey("Close"))
				return true;
			else
				return false;
		}
		
		
		/// <summary>
		/// Determines whether indexed matrix contains a seried of quotes, based on column headers
		/// </summary>
		public static bool IsQuotes (IndexedMatrix m)
		{
			IIndexByName colnames = m.ColIndices;
			if (colnames == null)
				return false;
			
			var ordering = colnames.Ordering;
			if (ordering.ContainsKey ("bid") || ordering.ContainsKey("Bid"))
				return true;
			else
				return false;
		}


		/// <summary>
		/// Fills in missing values, interpolating in a flat or linear fashion
		/// </summary>
		/// <param name="vec">Vector to fill.</param>
		/// <param name="style">Interpolation style.</param>
		public static void FillNA (Vector<double> vec, InterpStyle style)
		{
			var nrows = vec.Count;

			// find 1st non-NA
			var iend = 0;
			while (iend < nrows && Double.IsNaN(vec[iend])) iend++;

			// fill in prior NA stub
			for (int ri = 0 ; ri < iend; ri++)
				vec [ri] = vec [iend];

			var istart = 0;
			while (iend < nrows) 
			{
				istart = iend + 1;
				while (istart < nrows && !Double.IsNaN(vec[istart])) istart++;
				iend = istart;
				while (iend < nrows && Double.IsNaN(vec[iend])) iend++;

				var Vs = vec[istart-1];
				var Ve = (iend < nrows && style == InterpStyle.Linear) ? vec[iend] : Vs;

				var dpdt = (Ve-Vs) / (iend-istart+1);
				for (int i = istart ; i < iend ; i++)
				{
					Vs += dpdt;
					vec[i] = Vs;
				}
			}
		}
		
		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/SubviewMatrix.cs
// -------------------------------------------
﻿// 
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



namespace bridge.math.matrix
{
	/// <summary>
	/// View onto a matrix
	/// </summary>
	public class SubviewMatrix : Matrix
	{
		public SubviewMatrix (Matrix<double> underlier, int rowoffset, int coloffset, int nrows, int ncols)
			: base (new SubviewStorage (underlier, rowoffset, coloffset, nrows, ncols))
		{
			_data = (SubviewStorage)Storage;
		}


		/// <summary>
		/// Gets the underlying data for this matrix
		/// </summary>
		public double[] Data
			{ get { return MatrixUtils.DataOf (_data.Underlier); } }


		#region Storage

		/// <summary>
		/// Storage for subview
		/// </summary>
		private class SubviewStorage : MatrixStorage<double>
		{
			public SubviewStorage (Matrix<double> underlier, int rowoffset, int coloffset, int nrows, int ncols)
				: base (nrows, ncols)
			{
				Underlier = underlier;
				RowOffset = rowoffset;
				ColOffset = coloffset;
			}

			// properties

			/// <summary>
			/// The underlier.
			/// </summary>
			public Matrix<double> Underlier;

			/// <summary>
			/// The row offset.
			/// </summary>
			public int RowOffset;

			/// <summary>
			/// The column offset.
			/// </summary>
			public int ColOffset;

			/// <summary>
			/// True if the matrix storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }

			/// <summary>
			/// True if all fields of this matrix can be set to any value.
			///  False if some fields are fixed, like on a diagonal matrix.
			/// </summary>
			/// <value><c>true</c> if this instance is fully mutable; otherwise, <c>false</c>.</value>
			public override bool IsFullyMutable
				{ get { return true; } }


			// Functions

			/// <summary>
			/// True if the specified field can be set to any value.
			///  False if the field is fixed, like an off-diagonal field on a diagonal matrix.
			/// </summary>
			/// <returns><c>true</c> if this instance is mutable at the specified row column; otherwise, <c>false</c>.</returns>
			/// <param name="row">Row.</param>
			/// <param name="column">Column.</param>
			public override bool IsMutableAt (int row, int column)
			{
				return true;
			}

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="row">The row of the element.</param>
			/// <param name="column">The column of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int ri, int ci)
			{
				return Underlier [RowOffset + ri, ColOffset + ci];
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="row">The row of the element.</param>
			/// <param name="column">The column of the element.</param>
			/// <param name="value">The value to set the element to.</param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int ri, int ci, double value)
			{
				Underlier [RowOffset + ri, ColOffset + ci] = value;
			}
		}

		#endregion


		// Variables

		private SubviewStorage _data;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/matrix/SubviewVector.cs
// -------------------------------------------
﻿// 
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


namespace bridge.math.matrix
{
	/// <summary>
	/// Sub vector is a read-only view on a sub-range of a parent vector
	/// </summary>
	public class SubviewVector : Vector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.math.matrix.SubVector"/> class.
		/// </summary>
		/// <param name='underlier'>
		/// Underlying parent vector
		/// </param>
		/// <param name='offset'>
		/// Offset.
		/// </param>
		/// <param name='n'>
		/// Length of sub vector or defaults to remaining length from offset
		/// </param>
		public SubviewVector (Vector<double> underlier, int offset, int n = -1)
			: base (new SubviewStorage (underlier, offset, n))
		{
			_data = (SubviewStorage)Storage;
		}
		
		// Properties
		
		
		/// <summary>
		/// Gets the underlying data for this vector
		/// </summary>
		public double[] Data
			{ get { return MatrixUtils.DataOf (_data.Underlier); } }
		
		/// <summary>
		/// Gets the offset of the start of this subvector in the raw data
		/// </summary>
		public int DataOffset
			{ get { return MatrixUtils.DataOffsetOf (_data.Underlier) + _data.Offset; } }
		
		/// <summary>
		/// The offset
		/// </summary>
		public int Offset
			{ get { return _data.Offset; } set { _data.Offset = value; } }

		/// <summary>
		/// The length of the window
		/// </summary>
		public int Length
			{ get { return _data.Length; } }


		#region Storage

		/// <summary>
		/// Storage for subview
		/// </summary>
		private class SubviewStorage : VectorStorage<double>
		{
			public SubviewStorage (Vector<double> underlier, int offset, int len)
				: base (len >= 0 ? len : underlier.Count - offset)
			{
				Underlier = underlier;
				Offset = offset;
			}

			// properties

			/// <summary>
			/// The underlier.
			/// </summary>
			public Vector<double> Underlier;

			/// <summary>
			/// The offset.
			/// </summary>
			public int Offset;
		
			/// <summary>
			/// True if the vector storage format is dense.
			/// </summary>
			public override bool IsDense 
				{ get { return true; } }


			// Functions

			/// <summary>
			/// Retrieves the requested element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <returns>The requested element.</returns>
			/// <remarks>Not range-checked.</remarks>
			public override double At(int index)
			{
				return Underlier [Offset + index];
			}

			/// <summary>
			/// Sets the element without range checking.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="value">The value to set the element to. </param>
			/// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
			public override void At(int index, double value)
			{
				Underlier [Offset + index] = value;
			}
		}

		#endregion

		// Variables

		private SubviewStorage		_data;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/ctor/CtorLexer.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.parsing.ctor
{
	/// <summary>
	/// Ctor lexer
	/// </summary>
	public class CtorLexer
	{
		public CtorLexer (string expr)
		{
			Lex (expr);
		}
		
		
		// Properties
		
		
		public IList<CtorToken> Tokens
			{ get { return _tokens; } }
		
		
		// Implementation
		
		
		private void Lex (string str)
		{
			int len = str.Length;
			
			int wordstart = 0;
			
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				switch (c)
				{
					case '\'':
						wordstart = ++i;
						while (i < len && str[i] != '\'') i++;
						Add (CtorToken.TType.STRING, str.Substring(wordstart, i - wordstart));
						break;
	
					case '"':
						wordstart = ++i;
						while (i < len && str[i] != '"') i++;
						Add (CtorToken.TType.STRING, str.Substring(wordstart, i - wordstart));
						break;
	
					case ',':
						Add (CtorToken.TType.SEPARATOR, ",");
						break;
	
					case '(':
						Add (CtorToken.TType.OPEN_PAREN, "(");
						break;
	
					case ')':
						Add (CtorToken.TType.CLOSE_PAREN, ")");
						break;
	
					case '.':
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
						wordstart = i;
						while (i < len && StringUtils.IsNumeric(str[i])) i++; 
						
						string snum = str.Substring(wordstart, i - wordstart);
						if (snum.IndexOf('.') >= 0 || snum.IndexOf('e') >= 0)
							Add (CtorToken.TType.NUMERIC, double.Parse(snum));
						else
							Add (CtorToken.TType.NUMERIC, int.Parse(snum));
						
						i--;
						break;
						
					case '[':
						Add (CtorToken.TType.ARRAY_OPEN, "[");
						break;
						
					case ']':
						Add (CtorToken.TType.ARRAY_CLOSE, "]");
						break;
	
	
					case '\n':
					case '\r':
					case '\t':
					case ' ':
						break;
						
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
						wordstart = i;
						while (i < len && (c = str[i]) > ' ' && (char.IsDigit(c) || char.IsLetterOrDigit(c) || c == '.' || c == '+' || c =='$')) i++;
						Add (CtorToken.TType.IDENTIFIER, str.Substring(wordstart, i - wordstart));
						i--;
						break;
						
					default:
						throw new Exception ("failed to parse constructor: " + str + " at " + i);
				}
			}
		}
		
		
		
		private void Add (CtorToken.TType type, object value = null)
		{
			_tokens.Add (new CtorToken (type, value));
		}
		
		
		// Variables
		
		private IList<CtorToken>		_tokens = new List<CtorToken>();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/ctor/CtorParser.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/ctor/CtorToken.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.parsing.ctor
{
	/// <summary>
	/// Ctor token.
	/// </summary>
	public class CtorToken : Token<CtorToken.TType>
	{
		public enum TType
			{ NONE, IDENTIFIER, STRING, NUMERIC, SEPARATOR, ARRAY_OPEN, ARRAY_CLOSE, OPEN_PAREN, CLOSE_PAREN }
		
		public CtorToken (TType type, object info = null)
			: base (type, info) {}			
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/dates/DateLexer.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Date lexer
	/// </summary>
	public class DateLexer
	{		
		// Functions
		
		
		/// <summary>
		/// Create token stream of dates
		/// </summary>
		/// <param name='str'>
		/// date as string.
		/// </param>
		public List<DateToken> Parse (string str)
		{
			_tokens.Clear();
			Lex (str);
			return _tokens;
		}
		
		
		// Implementation
		
		
		private void Lex (string str)
		{
			var len = str.Length;
			int wordstart = 0;
			
			var type = DateToken.TType.NONE;
			
			for (int i = 0 ; i < len ; i++)
			{
				char c = str[i];
				switch (c)
				{
					case '-':
						Close (type, str.Substring (wordstart, i - wordstart));
						Close (DateToken.TType.DASH, "-");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
					case ',':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.COMMA, ",");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
					case '.':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.DOT, "-");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
						
					case ':':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.COLON, ":");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
						
					case '/':
						Close (type, str.Substring (wordstart, i-wordstart));
						Close (DateToken.TType.SLASH, "/");
						wordstart = i+1;
						type = DateToken.TType.NONE;
						break;
	
	
					case '\n':
					case '\r':
					case '\t':
					case ' ':
						if (type != DateToken.TType.WHITESPACE)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.WHITESPACE;
							wordstart = i;
						}
						break;
	
					
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
						if (type != DateToken.TType.NUMERIC)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.NUMERIC;
							wordstart = i;
						}
						break;
						
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
						if (type != DateToken.TType.ALPHA)
						{
							Close (type, str.Substring (wordstart, i-wordstart));
							type = DateToken.TType.ALPHA;
							wordstart = i;
						}
						break;
				}
			}
			
			Close (type, str.Substring (wordstart));
		}
		
		
		
		private void Close (DateToken.TType type, string v)
		{
			if (type == DateToken.TType.NONE)
				return;
	
			if (v == "T")
				_tokens.Add (new DateToken (DateToken.TType.T, v));
			else if (v == "Z")
				_tokens.Add (new DateToken (DateToken.TType.Z, v));
			else
				_tokens.Add (new DateToken (type, v));
		}
		
		
		// Variables
		
		private List<DateToken>		_tokens = new List<DateToken>();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/dates/DateParser.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Parses Dates / Times
	/// <p>
	/// The date formats are as follows:
	/// <ul>
	/// 	<li>MMMYY			- ie MAR07</li>
	/// 	<li>MMMYYYY			- ie MAR2007</li>
	/// 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
	/// 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
	/// 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
	/// 	<li>YYYYMMDD		- ie 20071103</li>
	/// 	<li>MM/dd/YY		- ie 12/01/07</li>
	/// 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
	/// 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
	/// 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
	/// </ul>
	/// The dates can be followed by a space or 'T' separator and can have the following formats:
	/// <ul>
	/// 	<li>HH:mm:ss		- ie 23:10:00</li>
	/// 	<li>HH:mm:ss:SSS	- ie 23:10:00:100</li>
	/// 	<li>HH:mm:ss.SSS	- ie 23:10:00:100</li>
	/// </ul>
	/// Each of these formats can have a Z (for GMT) or timezone (EST) at the end of the 
	/// time string.
	/// </summary>
	public class DateParser
	{
		public enum Convention
			{ American, British }

		public DateParser (Convention convention = Convention.American)
		{
			_convention = convention;
			_lexer = new DateLexer ();
		}

			   
		static DateParser()
	    {
			_months = new Dictionary<string,int> (StringComparer.OrdinalIgnoreCase);
	    	_months["JAN"] = 1;
	    	_months["FEB"] = 2;
	    	_months["MAR"] = 3;
	    	_months["APR"] = 4;
	    	_months["MAY"] = 5;
	    	_months["JUN"] = 6;
	    	_months["JUL"] = 7;
	    	_months["AUG"] = 8;
	    	_months["SEP"] = 9;
	    	_months["OCT"] = 10;
	    	_months["NOV"] = 11;
	    	_months["DEC"] = 12;
	    	
	    	_months["SUN"] = -1;
	    	_months["MON"] = -2;
	    	_months["TUE"] = -3;
	    	_months["WED"] = -4;
	    	_months["THU"] = -5;
	    	_months["FRI"] = -6;
	    	_months["SAT"] = -7;
			
			_parser = new ThreadLocal<DateParser>(() => new DateParser());
	    }
			
		
		// Properties
		
		
		public static DateParser DefaultParser
			{ get { return _parser.Value; } }
				

		
		// Functions
		

		/// <summary>
		/// Parse date/time string
		/// </summary>
		/// <param name='sdate'>
		/// date.
		/// </param>
		/// <param name='default_zone'>
		/// default time zone
		/// </param>
		public ZDateTime Parse (string sdate, ZTimeZone default_zone)
		{
			List<DateToken> tokens = _lexer.Parse (sdate);
			
			return ParseDateTime (sdate, tokens, default_zone);
		}
		
		
		// Classes
		
		
		/**
		 * Date information
		 */
		private struct DateInfo 
		{
			public ZTimeZone		Zone;
			public int				DayOfMonth;
			public int				Month;
			public int 				Year;
			public int				Hours;
			public int				Minutes;
			public int				Seconds;
			public int				Milliseconds;
		}
		
		
		// Implementation
		
		
		private ZDateTime ParseDateTime (string sdate, List<DateToken> tokens, ZTimeZone default_zone)
		{
			var len = tokens.Count;
			if (len < 1)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
	
			// special-case, may be a timestamp
			var first = tokens[0];
			if (len == 1 && first.Type == DateToken.TType.NUMERIC)
			{
				long stamp = (long)first;

				// determine whether YYYYMMDD date or whether timestamp
				if (stamp > 19000000 && stamp < 90000000)
					return new ZDateTime ((int)(stamp / 10000), (int)(stamp / 100) % 100, (int)stamp % 100, 0, 0, 0, 0, default_zone); 
				else
					return new ZDateTime(stamp, default_zone);
			}
			
			// otherwise straight date-time
			else
			{
				DateInfo idate = new DateInfo();
				int Itime = ParseDate (sdate, ref idate, tokens);
				ParseTime (sdate, ref idate, tokens, Itime);
				
				if (idate.Zone == null)
					idate.Zone = default_zone;
				
				return CreateDateTime (ref idate);
			}		
		}
		
		
		private ZDateTime ParseJustTime (string sdate, List<DateToken> tokens, ZTimeZone zone)
		{
			DateInfo idate = new DateInfo();
			ParseTime (sdate, ref idate, tokens, 0);
			
			return CreateTime (ref idate, zone);
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMMYY			- ie MAR07</li>
		 * 	<li>MMMYYYY			- ie MAR2007</li>
		 * 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
		 * 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
		 * 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
		 * 	<li>MM/dd/YY		- ie 12/01/07</li>
		 * 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
		 * 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
		 * 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
		 * </ul>
		 */
		private int ParseDate (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			switch (tokens[0].Type)
			{
				case DateToken.TType.ALPHA:
					int mod = ToMonthOrDay ((string)tokens[0]);
					if (mod > 0)
						return ParseDateMMMYY (sdate, ref info, tokens);
					else
						return ParseDateHTTP (sdate, ref info, tokens);
						
				case DateToken.TType.NUMERIC:
					return ParseDateNN (sdate, ref info, tokens);
				
				default:
					throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
			}
		}
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMMYY			- ie MAR07</li>
		 * 	<li>MMMYYYY			- ie MAR2007</li>
		 * </ul>
		 */
		private int ParseDateMMMYY (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			info.Month = ToMonthOrDay ((string)tokens[0]);
			
			switch (tokens[1].Type)
			{
				case DateToken.TType.NUMERIC:
					info.Year = ToYear ((int)tokens[1]);
					return 2;
	
				case DateToken.TType.WHITESPACE:
				case DateToken.TType.DASH:
					return ParseDateMMMDDYYYY (sdate, ref info, tokens);
					
				default:
					throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
			}
			
		}
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMM DD, YYYY	- Jan 31, 2011</li>
		 * </ul>
		 */
		private int ParseDateMMMDDYYYY (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			info.DayOfMonth = ToNumeric(sdate, tokens[2]);
			
			// locate year
			for (int i = 3 ; i < tokens.Count ; i++)
			{
				DateToken token = tokens[i];
				switch (token.Type)
				{
					case DateToken.TType.NUMERIC:
						info.Year = ToYear ((int)token);
						return i+1;
						
					case DateToken.TType.COMMA:
					case DateToken.TType.DASH:
					case DateToken.TType.WHITESPACE:
					case DateToken.TType.SLASH:
						break;
						
					default:
						throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
				}		
			}
			
			throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>DAY, dd MMM YYYY	- ie Fri, 25 Jul 2008 10:38:41 GMT</li>
		 * </ul>
		 */
		private int ParseDateHTTP (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			int Inow = 0;
			DateToken T2 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T3 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T4 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			
			// dd-MMM-yy case
			if (T3.Type == DateToken.TType.ALPHA)
			{
				info.Month = ToMonthOrDay ((string)T3);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Year = ToYear (sdate, T4);
				return Inow+1;
			}
			else
			{
				info.Month = Bound (sdate, ToNumeric (sdate, T3), 1, 12);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Year = ToYear (sdate, T4);
				return Inow+1;			
			}
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>YYYYMMDD		- ie 20080401</li>
		 * 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
		 * 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
		 * 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
		 * 	<li>MM/dd/YY		- ie 12/01/07</li>
		 * 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
		 * 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
		 * 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
		 * </ul>
		 */
		private int ParseDateNN (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			var len = tokens.Count;
	
			int Inow = 0;
			DateToken T1 = tokens[0];
			
			// YYYYMMDD case
			if (len == 1)
			{
				int yyyymmdd = (int)T1;
				info.Year = Bound (sdate, yyyymmdd / 10000, 1950, 9999);
				info.Month = Bound (sdate, (yyyymmdd / 100) % 100, 1, 12);
				info.DayOfMonth = Bound (sdate, yyyymmdd % 100, 1, 31);
				return 1;
			}
			
			DateToken T2 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T3 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			
			// dd-MMM-yy case
			if (T2.Type == DateToken.TType.ALPHA)
			{
				info.Month = ToMonthOrDay ((string)T2);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T1), 1, 31);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
			
			//  yyyy-mm-dd case
			else if ((int)T1 > 1000)
			{
				info.Year = ToYear (sdate, T1);
				info.Month = Bound (sdate, ToNumeric (sdate, T2), 1, 12);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T3), 1, 31);			
				return Inow+1;
			}
			// mm/dd/yy case
			else if (_convention == Convention.American)
			{
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Month = Bound (sdate, ToNumeric (sdate, T1), 1, 12);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
			// dd/mm/yy case
			else
			{
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T1), 1, 31);
				info.Month = Bound (sdate, ToNumeric (sdate, T2), 1, 12);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
		}
	
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>HH:mm:ss		- ie 23:10:00</li>
		 * 	<li>HH:mm:ss:SSS	- ie 23:10:00:100</li>
		 * 	<li>HH:mm:ss.SSS	- ie 23:10:00:100</li>
		 * </ul>
		 * Each of these formats can have a Z (for GMT) or timezone (EST) at the end of the 
		 * time string.
		 */
		private void ParseTime (string sdate, ref DateInfo info, List<DateToken> tokens, int Istart)
		{
			var len = tokens.Count;
			
			if (Istart == len)
				return;
			
			DateToken separator = tokens[Istart];
			DateToken last = tokens[len-1];
			DateToken plast = tokens[len-2];
	
			if (separator.Type != DateToken.TType.WHITESPACE && separator.Type != DateToken.TType.T)
				throw new ArgumentException ("failed to parse date, as unknown time form: " + sdate);
	
			
			int cap = 0;
			
			if (last.Type == DateToken.TType.NUMERIC)
				cap = len;
			else if (plast.Type == DateToken.TType.NUMERIC)
				cap = len - 1;
			else
				cap = len - 2;
			
			int Inow = Istart + 1;
			
			// get hours
			if (Inow < cap)
				info.Hours = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 23);
			
			// get minutes
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
				info.Minutes = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 59);
						
			// get seconds
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
				info.Seconds = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 59);
			
			// get milli seconds
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
			{
				var fraction = Bound (sdate, ToNumeric (sdate, tokens [Inow]), 0, 999999999);
				if (fraction <= 999)
					info.Milliseconds = fraction;
				else if (fraction <= 999999)
					info.Milliseconds = fraction / 1000;
				else
					info.Milliseconds = fraction / 1000000;
			}
			// get timezone
			info.Zone = GetTimezone (last);
		}
	
		
		
		// Implementation: Aux Functions
	
		
		/**
		 * Convert string month to numeric
		 */
		private int ToMonthOrDay (string smonth)
		{
			int month = 0;
			if (_months.TryGetValue(smonth, out month))
				return month;
			else
				throw new ArgumentException ("failed to parse date, invalid month or day: " + smonth);
		}
	
		
		/**
		 * convert numeric token
		 */
		private int ToNumeric (string sdate, DateToken tok)
		{
			if (tok.Type != DateToken.TType.NUMERIC)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
			else
				return (int)tok;
		}
	
		
		/**
		 * Normalize year
		 */
		private int ToYear (string sdate, DateToken tok)
		{
			if (tok.Type != DateToken.TType.NUMERIC)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
			else
				return ToYear ((int)tok);
		}
	
		
		/**
		 * Normalize year
		 */
		private int ToYear (int yy)
		{
			if (yy > 1000)
				return yy;
			if (yy > 50)
				return 1900 + yy;
			else
				return 2000 + yy;
		}
		
		
		/**
		 * skip separators in token stream
		 */
		private int SkipSeparator (List<DateToken> tokens, int pos)
		{
			var len = tokens.Count;
			for (int i = pos ; i < len ; i++)
			{
				switch (tokens[i].Type)
				{
					case DateToken.TType.T:
					case DateToken.TType.WHITESPACE:
					case DateToken.TType.COLON:
					case DateToken.TType.COMMA:
					case DateToken.TType.DOT:
					case DateToken.TType.SLASH:
					case DateToken.TType.DASH:
						continue;
					
					default:
						return i;
				}
			}
			
			return len;
		}
	
		
		/**
		 * Do bounds checking
		 */
		private int Bound (string sdate, int value, int min, int max)
		{
			if (value < min || value > max)
				throw new ArgumentException ("invalid date, field out of bounds: " + sdate + ", field: " + value);
			
			return value;
		}
						
		
		/**
		 * Determine timezone for this date
		 */
		private ZTimeZone GetTimezone (DateToken tok)
		{
			switch (tok.Type)
			{
				case DateToken.TType.Z:
					return ZTimeZone.GMT;
					
				case DateToken.TType.ALPHA:
					return new ZTimeZone ((string)tok);
					
				default:
					return default(ZTimeZone);
			}
		}
		
		
		/**
		 * Create date/time from given info
		 */
		private ZDateTime CreateDateTime (ref DateInfo info)
		{
			return new ZDateTime (info.Year, info.Month, Math.Max(info.DayOfMonth, 1), info.Hours, info.Minutes, info.Seconds, info.Milliseconds, info.Zone);
		}
			
	
		/**
		 * Create time from given info, relative to current date
		 */
		private ZDateTime CreateTime (ref DateInfo info, ZTimeZone zone)
		{
			ZDateTime now = ZDateTime.TimeNowFor (zone);
			return new ZDateTime (now.Year, now.Month, Math.Max(now.Day, 1), info.Hours, info.Minutes, info.Seconds, info.Milliseconds, info.Zone);
		}
		
		
		// Static initialization
		
		static IDictionary<string,int> _months;
		
		// Variables

		private Convention					_convention;
		private DateLexer					_lexer;
		static ThreadLocal<DateParser>		_parser;
	}


	/// <summary>
	/// British date parser.
	/// </summary>
	public class BritishDateParser : DateParser
	{
		public static DateParser INSTANCE = new BritishDateParser();

		public BritishDateParser ()
			: base (Convention.British)
		{
		}
	}


	/// <summary>
	/// American date parser.
	/// </summary>
	public class AmericanDateParser : DateParser
	{
		public static DateParser INSTANCE = new AmericanDateParser();

		public AmericanDateParser ()
			: base (Convention.American)
		{
		}
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/dates/DateToken.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Date token.
	/// </summary>
	public struct DateToken
	{
		public enum TType
			{ NONE, ALPHA, NUMERIC, DASH, COLON, COMMA, SLASH, T, DOT, Z, WHITESPACE }

		public DateToken (TType type, object payload = null)
		{
			_type = type;
			_payload = payload;
		}
	
		
		// Properties
		
		
		public TType Type
			{ get { return _type; } }
	
		public object Payload
			{ get { return _payload; }  }
	

		// Conversions
		
		
		public static implicit operator int (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return int.Parse((string)payload);
			if (payload is int)
				return (int)payload;
			if (payload is decimal)
				return (int)payload;
			else
				throw new Exception ("could not convert payload to int");
		}
		
		public static implicit operator long (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return long.Parse((string)payload);
			if (payload is int)
				return (long)payload;
			if (payload is long)
				return (long)payload;
			else
				throw new Exception ("could not convert payload to long");
		}
		
		public static implicit operator double (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return double.Parse((string)payload);
			if (payload is double)
				return (double)payload;
			if (payload is decimal)
				return (double)payload;
			else
				throw new Exception ("could not convert payload to double");
		}
		
		public static implicit operator bool (DateToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return bool.Parse((string)payload);
			if (payload is bool)
				return (bool)payload;
			else
				throw new Exception ("could not convert payload to bool");
		}
		
		public static implicit operator string (DateToken token)
		{
			object payload = token._payload;
			return payload.ToString();
		}
		
		
		// Meta
		
		public override string ToString ()
			{ return _type + ":" + _payload.ToString(); }
		
		
		// Variables
		
		private TType		_type;
		private object		_payload;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/json/JsonLexer.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/json/JsonToken.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.parsing.json
{
	public enum JsonTokenType
		{ NONE, NULL, STRING, INT, FLOAT, BOOLEAN, WHITESPACE, KEY, ARRAY_START, ARRAY_END, OBJECT_START, OBJECT_END, SEPARATOR }


	/// <summary>
	/// Json token.
	/// </summary>
	public class JsonToken
	{

		public JsonToken (JsonTokenType type, object payload = null)
		{
			_type = type;
			_payload = payload;
		}


		// Properties


		public JsonTokenType Type
			{ get { return _type; } }

		public object Payload
			{ get { return _payload; }  }

		public string AsString
			{ get { return (string)this; } }

		public long AsLong
			{ get { return (long)this; } }

		public int AsInt
			{ get { return (int)this; } }

		public bool AsBool
			{ get { return (bool)this; } }

		public double AsDouble
			{ get { return (double)this; } }


		// Conversions


		public static implicit operator int (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return int.Parse((string)payload);
			if (payload is int)
				return (int)payload;
			if (payload is decimal)
				return (int)payload;
			else
				throw new Exception ("could not convert payload to int");
		}

		public static implicit operator long (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return long.Parse((string)payload);
			if (payload is int)
				return (long)payload;
			if (payload is long)
				return (long)payload;
			else
				throw new Exception ("could not convert payload to long");
		}

		public static implicit operator double (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return double.Parse((string)payload);
			if (payload is double)
				return (double)payload;
			if (payload is int)
				return (double)payload;
			if (payload is decimal)
				return (double)payload;
			else
				throw new Exception ("could not convert payload to double");
		}

		public static implicit operator bool (JsonToken token)
		{
			object payload = token._payload;
			if (payload is string)
				return bool.Parse((string)payload);
			if (payload is bool)
				return (bool)payload;
			else
				throw new Exception ("could not convert payload to bool");
		}

		public static implicit operator string (JsonToken token)
		{
			object payload = token._payload;
			switch (token.Type)
			{
				case JsonTokenType.BOOLEAN:
					return (bool)payload == true ? "true" : "false";
				case JsonTokenType.NULL:
					return "null";
				default:
					return payload.ToString();
			}
		}


		// Meta

		public override string ToString ()
			{ return _type + ":" + _payload.ToString(); }




		// Variables

		private JsonTokenType 	_type;
		private object 			_payload;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/parsing/Token.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.parsing
{
	/// <summary>
	/// Lexer token
	/// </summary>
	public class Token<EnumType>
	{
		public Token (EnumType type, object payload = null)
		{
			_type = type;
			_payload = payload;
		}
	
		
		// Properties
		
		
		public EnumType Type
			{ get { return _type; } }
	
		public object Payload
			{ get { return _payload; }  }
	

		// Conversions
		
		
		public static implicit operator int (Token<EnumType> token)
		{
			object payload = token._payload;
			if (payload is int)
				return (int)payload;
			if (payload is string)
				return int.Parse((string)payload);
			if (payload is decimal)
				return (int)payload;
			else
				throw new Exception ("could not convert payload to int");
		}
		
		public static implicit operator long (Token<EnumType> token)
		{
			object payload = token._payload;
			if (payload is int)
				return (long)payload;
			if (payload is long)
				return (long)payload;
			if (payload is string)
				return long.Parse((string)payload);
			else
				throw new Exception ("could not convert payload to long");
		}
		
		public static implicit operator double (Token<EnumType> token)
		{
			object payload = token._payload;
			if (payload is double)
				return (double)payload;
			if (payload is string)
				return double.Parse((string)payload);
			if (payload is decimal)
				return (double)payload;
			else
				throw new Exception ("could not convert payload to double");
		}
		
		public static implicit operator bool (Token<EnumType> token)
		{
			object payload = token._payload;
			if (payload is bool)
				return (bool)payload;
			if (payload is string)
				return bool.Parse((string)payload);
			else
				throw new Exception ("could not convert payload to bool");
		}
		
		public static implicit operator string (Token<EnumType> token)
		{
			object payload = token._payload;
			return payload.ToString();
		}
		
		
		// Meta
		
		public override string ToString ()
			{ return _type + ":" + _payload.ToString(); }
		
		
		// Variables
		
		private EnumType	_type;
		private object		_payload;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/reflection/Creator.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/common/reflection/DelegateGenerator.cs
// -------------------------------------------
﻿// 
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

// -------------------------------------------
// File: ../DotNet/Library/src/common/reflection/ReflectUtils.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.reflection
{
    /// <summary>
    /// Reflection utils
    /// </summary>
    public static class ReflectUtils
    {
        static ReflectUtils()
        {
            _system_assemblies.Add("FSharp.Core", "FSharp.Core, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

			var domain = AppDomain.CurrentDomain;
			domain.AssemblyResolve += new ResolveEventHandler(AssemblyLoader);
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
        public static Type FindType(string name)
        {
            Type type = null;
            if (_types.TryGetValue(name, out type))
                return type;

            // replace + in type with /
            var typename = name;
            if (typename.IndexOf('+') >= 0)
                typename = StringUtils.Replace(typename, '+', '/');

            if ((type = FindInAssemblies(typename)) != null)
            {
                _types[name] = type;
                return type;
            }

            try
            {
                type = Type.GetType(typename);
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
        public static void CheckType(Type type)
        {
            if (type.GetMember("ToString") == null)
                throw new ArgumentException("type not instantiated properly: " + type);
        }


        /// <summary>
        /// Determines object if is instance of generic type the specified obj type.
        /// </summary>
        /// <returns><c>true</c> if is instance of generic type the specified obj type; otherwise, <c>false</c>.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="type">Type.</param>
        public static bool IsInstanceOfGenericType(object obj, Type type)
        {
            var otype = obj.GetType();

            if (!otype.IsGenericType)
                return false;
            else
                return otype.GetGenericTypeDefinition() == type;
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
        public static IntPtr FindTypeByHandle(string name)
        {
            Type type = FindType(name);
            if (type == null)
                throw new Exception("could not find type: " + name);

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
        public static Type FindType(Assembly assembly, string name)
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
        public static IntPtr FindTypeByHandle(Assembly assembly, string name)
        {
            Type type = FindType(assembly, name);
            if (type == null)
                throw new Exception("could not find type: " + name);

            return type.TypeHandle.Value;
        }


        /// <summary>
        /// Add given assembly to the list of those to be searched with reflection
        /// </summary>
        /// <param name="assembly">Assembly to be added.</param>
        public static void Register(Assembly assembly)
        {
            var path = FileUtils.DirOf(assembly.Location);
            _assembly_dirs.Add(path);
            _supplemental.Add(assembly);
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
        public static Assembly FindAssembly(string name)
        {
            if (name == null)
                throw new ArgumentException("tried to load an assembly with NULL name");

            if (name.EndsWith(".dll") || name.EndsWith(".exe"))
                name = StringUtils.RTrimField(name, 1, '.');

            // if is a system assembly, load from fully qualitied name
            if (_system_assemblies.ContainsKey(name))
                return Assembly.Load(_system_assemblies[name]);

            // first search loaded assemblies
            AppDomain domain = AppDomain.CurrentDomain;
            Assembly assembly = Collections.FindOne(
                domain.GetAssemblies(), (a) => string.Compare(name, a.GetName().Name, true) == 0);
            if (assembly != null)
                return assembly;

            // now try to find in filesystem
            var path = StringUtils.Or(
                ResourceLoader.Find("lib/" + name + ".dll"),
                ResourceLoader.Find(name + ".dll"));

            // try to find the assembly without the full rooted name 
            if (path == null && Path.IsPathRooted(name))
                return FindAssembly(Path.GetFileName(name));

            if (path == null)
                throw new ArgumentException("could not find assembly: " + name);

            var pdir = Directory.GetParent(path);
            var pcwd = Environment.CurrentDirectory;

            Environment.CurrentDirectory = pdir.FullName;

            try
            { assembly = Assembly.LoadFrom(path); }
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
        public static object CallStaticMethod(Type type, string method, params object[] parameters)
        {
            MethodInfo imethod = FindMatchingMethod(type, method, parameters);
            if (imethod != null)
                return CallMethod(null, imethod, parameters);
            else
                throw new ArgumentException("could not find method '" + method + "' in " + type + ", args: " + StringUtils.ToString(parameters));
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
        public static object CallStaticMethodByName(string classname, string method, params object[] parameters)
        {
            Type type = FindType(classname);
            if (type == null)
                throw new Exception("CallStaticMethod: could not find specified type: " + classname);

            MethodInfo imethod = FindMatchingMethod(type, method, parameters);
            if (method == null)
                throw new Exception("CallStaticMethod: could not find matching method: " + method);

            return CallMethod(null, imethod, parameters);
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
        public static object CallMethod(object obj, string name, params object[] parameters)
        {
            var type = obj.GetType();
            MethodInfo method = FindMatchingMethod(type, name, parameters);

            if (method == null)
                throw new ArgumentException("cannot find matching method: " + name + ", within: " + type + ", requested with " + parameters.Length + " params");

            return CallMethod(obj, method, parameters);
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
        public static object GetProperty(object obj, string name)
        {
            var index = name.IndexOf('[');
            if (index < 0)
            {
                var type = obj.GetType();
                PropertyInfo prop = type.GetProperty(name);

                if (prop == null)
                    throw new ArgumentException("cannot find matching property: " + name + ", whithin: " + type);

                return prop.GetValue(obj, null);
            }
            else
            {
                var ith = int.Parse(name.Substring(index + 1, name.Length - index - 2));
                var propname = name.Substring(0, index);

                return GetIndexedProperty(obj, propname, ith);
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
        public static object GetIndexedProperty(object obj, string name, int ith)
        {
            var type = obj.GetType();
            PropertyInfo prop = type.GetProperty(name);

            if (prop == null)
                throw new ArgumentException("cannot find matching property: " + name + ", whithin: " + type);

            // get the collection at the property
            var collection = prop.GetValue(obj, null);

            // get the name of the this[] indexer
            var attrs = collection.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true);
            var indexname = ((DefaultMemberAttribute)attrs[0]).MemberName;

            // get the property info for it
            var indexer = collection.GetType().GetProperty(indexname);

            return indexer.GetValue(collection, new object[] { ith });
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
        public static object GetIndexed(object obj, int ith)
        {
            var type = obj.GetType();

            // get the name of the this[] indexer
            var attrs = obj.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true);
            var indexname = ((DefaultMemberAttribute)attrs[0]).MemberName;

            // get the property info for it
            var indexer = obj.GetType().GetProperty(indexname);

            return indexer.GetValue(obj, new object[] { ith });
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
        public static void SetProperty(object obj, string name, object val)
        {
            var type = obj.GetType();
            PropertyInfo prop = type.GetProperty(name);

            if (prop == null)
                throw new ArgumentException("cannot find matching property: " + name + ", whithin: " + type);

            prop.SetValue(obj, val, null);
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
        public static object GetStaticProperty(string classname, string name)
        {
            var type = FindType(classname);
            PropertyInfo prop = type.GetProperty(name);

            if (prop == null)
                throw new ArgumentException("cannot find matching property: " + name + ", whithin: " + classname);

            return prop.GetValue(null, null);
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
        public static void SetStaticProperty(string classname, string name, object val)
        {
            var type = FindType(classname);
            PropertyInfo prop = type.GetProperty(name);

            if (prop == null)
                throw new ArgumentException("cannot find matching property: " + name + ", whithin: " + classname);

            prop.SetValue(null, val, null);
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
        public static MethodInfo FindMatchingMethod(Type type, string name, object[] args)
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

                var score = ReflectUtils.ScoreParameters(paramlist, args);
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
        public static Delegate GetEventDelegate(object obj, string eventname)
        {
            Type type = obj.GetType();

            while (type != null)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
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
        public static IList<MethodInfo> FindMethodsWithAttribute(Type type, Type attribute)
        {
            var list = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
            {
                var attr = FindAttribute(method, attribute);
                if (attr != null)
                    list.Add(method);
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
        public static IList<Type> FindTypesWithAttribute(Assembly assembly, Type attribute)
        {
            var list = new List<Type>();
            foreach (var type in assembly.GetTypes())
            {
                var attr = FindAttribute(type, attribute);
                if (attr != null)
                    list.Add(type);
            }

            return list;
        }


        /// <summary>
        /// Finds the properties matching selector
        /// </summary>
        /// <returns>The properties.</returns>
        /// <param name="type">Type.</param>
        /// <param name="selector">Selector.</param>
        public static IList<PropertyInfo> FindProperties(Type type, Func<PropertyInfo, bool> selector)
        {
            var proplist = new List<PropertyInfo>();
            foreach (var prop in type.GetProperties())
            {
                if (selector(prop))
                    proplist.Add(prop);
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
        public static void ConformArguments(ParameterInfo[] paramlist, object[] args)
        {
            int i = -1;
            foreach (var parm in paramlist)
            {
                i++;
                object arg = args[i];
                Type pclass = parm.ParameterType;
                Type aclass = arg != null ? arg.GetType() : null;

                var ptype = ValueTypeUtils.TypeOf(pclass);
                var atype = ValueTypeUtils.TypeOf(aclass);

                if ((atype == ptype && atype != VType.Other) || pclass == aclass || pclass.IsAssignableFrom(aclass))
                    continue;

                if (arg == null && pclass.IsValueType)
                    throw new ArgumentException("could not find matching constructor");

                if (arg == null)
                    continue;

                // convert simple value types to conform
                if (ptype != VType.Other)
                {
                    args[i] = ValueTypeUtils.Convert(arg, ptype, pclass);
                }

                // special handling for delegates
                else if (DelegateGenerator.IsDelegate(pclass))
                {
                    args[i] = DelegateGenerator.ConvertToDelegate(pclass, args[i]);
                }

                // special case for ZDateTime, allowing long specifier
                else if (pclass == typeof(ZDateTime))
                {
                    if (aclass == typeof(long))
                        args[i] = new ZDateTime((long)arg, ZTimeZone.NewYork);
                    else if (aclass == typeof(string))
                        args[i] = new ZDateTime((string)arg);
                    else
                        throw new ArgumentException("unknown argument pairing");
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
                    var narray = Array.CreateInstance(pclass.GetElementType(), 1);
                    narray.SetValue(arg, 0);
                    args[i] = narray;
                }

		else if (pclass == typeof(double []))
		{
		    if (typeof (Vector<double>).IsAssignableFrom (aclass)) {
			var vec = (Vector<double>)args [i];
			var data = MatrixUtils.DataOf (vec);
			args [i] = data;
		    } else if (aclass != typeof (double []))
			throw new ArgumentException ("unknown argument pairing: " + aclass + " -> double[]");
		}

                else if (typeof(Vector<double>).IsAssignableFrom(pclass))
                {
                    if (aclass == typeof(double))
                    {
                        var vec = new DenseVector(1);
                        vec[0] = ((Double)args[i]);
                        args[i] = vec;
                    }
                    if (aclass == typeof(int))
                    {
                        var vec = new DenseVector(1);
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
                        var method = pclass.GetMethod("Parse");
                        if (method != null)
                        {
                            args[i] = method.Invoke(null, new object[] { sval });
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
                    { throw new ArgumentException("could not coerce type " + aclass + " to " + pclass); }
                }
            }
        }



        #region Implementaton


        private static object CallMethod(object obj, MethodInfo method, params object[] parameters)
        {
            try
            {
                return method.Invoke(obj, parameters);
            }
            catch (TargetInvocationException te)
            {
                throw te.InnerException;
            }
            catch (MethodAccessException me)
            {
                throw new Exception("method " + method.Name + " is not a public method", me);
            }
            catch
            { }

            try
            {
                ConformArguments(method.GetParameters(), parameters);
                return method.Invoke(obj, parameters);
            }
            catch (TargetInvocationException te)
            {
                throw te.InnerException;
            }
        }


        private static Type FindInAssemblies(string stype)
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
                    _log.Warn("could not load assembly: " + a + ", why: " + e.ToString());
                }
            }

            AppDomain dom = AppDomain.CurrentDomain;
            foreach (Assembly a in dom.GetAssemblies())
            {
                try
                {
                    Type type = FindInAssembly(a, stype);
                    if (type != null)
                        return type;
                }
                catch (Exception e)
                {
                    _log.Warn("could not load assembly: " + a + ", why: " + e.ToString());
                }
            }

            return null;
        }


        private static Type FindInAssembly(Assembly assembly, string stype)
        {
            stype = "." + StringUtils.Replace(stype, '/', '.');

            foreach (Type type in assembly.GetTypes())
            {
                var fullname = "." + StringUtils.Replace(type.FullName, '+', '.');

                if (fullname.EndsWith(stype))
                    return type;
            }

            return null;
        }



        private static object FindAttribute(MethodInfo method, Type attrib)
        {
            var attrs = method.GetCustomAttributes(attrib, true);
            return attrs.Length > 0 ? attrs[0] : null;
        }


        private static object FindAttribute(Type type, Type attrib)
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
        internal static int ScoreParameters(ParameterInfo[] paramlist, object[] args)
        {
            var score = 0;

            int i = -1;
            foreach (var parm in paramlist)
            {
                i++;
                var eklass = parm.ParameterType;
                var aklass = args[i] != null ? args[i].GetType() : null;

		if (aklass == typeof (IndexedVector) && eklass.IsAssignableFrom (typeof (double [])))
		    score += 200;
                else if (eklass == aklass || eklass.IsAssignableFrom(aklass) || DelegateGenerator.IsAssignableFrom(eklass, aklass))
                    score += 200;
                else
                    score += ArgumentPenalty(eklass, aklass);
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
        internal static int ArgumentPenalty(Type paramclass, Type argclass)
        {
            if (typeof(Vector<double>).IsAssignableFrom(paramclass))
            {
                if (typeof(Vector<double>).IsAssignableFrom(argclass))
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
            if (paramclass.IsArray && paramclass.GetElementType() == argclass)
                return 10;
            if (!paramclass.IsValueType)
                return -100;

            var paramtype = ValueTypeUtils.TypeOf(paramclass);
            var argtype = ValueTypeUtils.TypeOf(argclass);

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


        /// <summary>
        /// Assembly load event handler
        /// </summary>
        /// <returns>The assembly or none</returns>
        /// <param name="source">Source object.</param>
        /// <param name="e">request.</param>
        static Assembly AssemblyLoader(object source, ResolveEventArgs e)
        {
            _log.Debug("resolving assembly: " + e.Name);

            var assemblyname = StringUtils.Field(e.Name, 0, ',');
            foreach (var dir in _assembly_dirs)
            {
                var path = dir + Path.DirectorySeparatorChar + assemblyname + ".dll";
                if (File.Exists(path))
                    return Assembly.LoadFrom(path);
            }

            return null;
        }

		#endregion
		
		// Variables

		static Dictionary<string,string>	_system_assemblies = new Dictionary<string,string> ();
        static List<Assembly>               _supplemental = new List<Assembly>();
        static List<string>                 _assembly_dirs = new List<string>();
		static IDictionary<string,Type>		_types = new Dictionary<string,Type>();
		static Logger						_log = Logger.Get ("REFLECT");
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/reflection/ValueTypeUtils.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.reflection
{
	public enum VType
		{ Short, Int, Long, Float, Double, Byte, Bool, Char, String, Enum, Vector, Matrix, List, Period, Null, Other }
		

	/// <summary>
	/// Fast mapping of value type class IDs to enum
	/// </summary>
	public class ValueTypeUtils
	{		
		/// <summary>
		/// Determine the value type of an object (if a value)
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public static VType TypeOf (object obj)
		{
			VType type = VType.Other;
			if (obj == null)
				return VType.Null;

			Type otype = obj.GetType();
			if (_map.TryGetValue (otype, out type))
				return type;
			if (otype.IsEnum)
				return VType.Enum;

			// test to see if list type or not
			Type list = typeof(List<int>).GetGenericTypeDefinition ();

			Type gtype1 = otype.IsGenericType ? otype.GetGenericTypeDefinition () : otype;
			if (gtype1 == list)
				return VType.List;

			Type btype = BaseTypeOf (otype);
			Type gtype2 = btype.IsGenericType ? btype.GetGenericTypeDefinition () : btype;
			if (gtype2 == list)
				return VType.List;
			else
				return VType.Other;
		}

		
		/// <summary>
		/// Determine the value type of an object (if a value)
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public static VType TypeOf (Type type)
		{
			VType vtype = VType.Other;
			if (type == null)
				return VType.Null;

			if (_map.TryGetValue (type, out vtype))
				return vtype;
			if (type.IsEnum)
				return VType.Enum;

			// test to see if list type or not
			Type list = typeof(List<int>).GetGenericTypeDefinition ();

			Type gtype1 = type.IsGenericType ? type.GetGenericTypeDefinition () : type;
			if (gtype1 == list)
				return VType.List;

			Type btype = BaseTypeOf (type);
			Type gtype2 = btype.IsGenericType ? btype.GetGenericTypeDefinition () : btype;
			if (gtype2 == list)
				return VType.List;
			else
				return VType.Other;
		}
		
		
		/// <summary>
		/// Convert the specified arg to given type (if possible)
		/// </summary>
		/// <param name='arg'>
		/// Argument.
		/// </param>
		/// <param name='totype'>
		/// Type to convert to (if possible)
		/// </param>
		public static object Convert (object arg, VType totype, Type actual)
		{
			VType fromtype = TypeOf (arg);		
			switch (totype)
			{
				case VType.Short:
					return ConformShort (arg, fromtype);
				case VType.Int:
					return ConformInt (arg, fromtype);
				case VType.Long:
					return ConformLong (arg, fromtype);
				case VType.Float:
					return ConformFloat (arg, fromtype);
				case VType.Double:
					return ConformDouble (arg, fromtype);
				case VType.Bool:
					return ConformBool (arg, fromtype);
				case VType.Byte:
					return ConformByte (arg, fromtype);
				case VType.Char:
					return ConformChar (arg, fromtype);
				case VType.Enum:
					return ConformEnum (arg, fromtype, actual);
				case VType.String:
					return arg.ToString();
				default:
					throw new ArgumentException ("cannot convert to type: " + totype);
			}			
		}
			
		
		
		// Implementation
		
		
		/// <summary>
		/// Transform arg to short if possible
		/// </summary>
		private static object ConformShort (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return arg;
				case VType.Int:
					return (short)((int)arg);
				case VType.Long:
					return (short)((long)arg);
				case VType.Float:
					return (short)((float)arg);
				case VType.Double:
					return (short)((double)arg);
				case VType.String:
					return short.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to short");					
			}
		}

		
		/// <summary>
		/// Transform arg to int if possible
		/// </summary>
		private static object ConformInt (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (int)((short)arg);
				case VType.Int:
					return ((int)arg);
				case VType.Long:
					return (int)((long)arg);
				case VType.Float:
					return (int)((float)arg);
				case VType.Double:
					return (int)((double)arg);
				case VType.String:
					return int.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to int");					
			}
		}

		
		/// <summary>
		/// Transform arg to enum if possible
		/// </summary>VType
		private static object ConformEnum (object arg, VType atype, Type actual)
		{
			switch (atype)
			{
				case VType.Short:
					return Enum.GetValues(actual).GetValue ((int)((short)arg));
				case VType.Int:
					return Enum.GetValues(actual).GetValue (((int)arg));
				case VType.Long:
					return Enum.GetValues(actual).GetValue ((int)((long)arg));
				case VType.String:
					return Enum.Parse (actual, (string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to enum");					
			}
		}

		
		/// <summary>
		/// Transform arg to long if possible
		/// </summary>
		private static object ConformLong (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (long)((short)arg);
				case VType.Int:
					return (long)((int)arg);
				case VType.Long:
					return (long)((long)arg);
				case VType.Float:
					return (long)((float)arg);
				case VType.Double:
					return (long)((double)arg);
				case VType.String:
					return long.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to long");					
			}
		}

		
		/// <summary>
		/// Transform arg to float if possible
		/// </summary>
		private static object ConformFloat (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (float)((short)arg);
				case VType.Int:
					return (float)((int)arg);
				case VType.Long:
					return (float)((long)arg);
				case VType.Float:
					return (float)((float)arg);
				case VType.Double:
					return (float)((double)arg);
				case VType.String:
					return float.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to float");					
			}
		}

		
		/// <summary>
		/// Transform arg to double if possible
		/// </summary>
		private static object ConformDouble (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (double)((short)arg);
				case VType.Int:
					return (double)((int)arg);
				case VType.Long:
					return (double)((long)arg);
				case VType.Float:
					return (double)((float)arg);
				case VType.Double:
					return (double)((double)arg);
				case VType.String:
					return double.Parse ((string)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to double");					
			}
		}

		
		/// <summary>
		/// Transform arg to bool if possible
		/// </summary>
		private static object ConformBool (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return ((short)arg) != 0;
				case VType.Int:
					return ((int)arg) != 0;
				case VType.Long:
					return ((long)arg) != 0;
				case VType.Float:
					return ((float)arg) != 0.0;
				case VType.Double:
					return ((double)arg) != 0.0;
				case VType.String:
					return bool.Parse ((string)arg);
				case VType.Byte:
					return ((byte)arg) != 0;
				case VType.Char:
					return ((char)arg) != 0;
				case VType.Bool:
					return ((bool)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}

		
		/// <summary>
		/// Transform arg to byte if possible
		/// </summary>
		private static object ConformByte (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (byte)((short)arg);
				case VType.Int:
					return (byte)((int)arg);
				case VType.Long:
					return (byte)((long)arg);
				case VType.Float:
					return (byte)((float)arg);
				case VType.Double:
					return (byte)((double)arg);
				case VType.String:
					return (byte)int.Parse ((string)arg);
				case VType.Byte:
					return ((byte)arg);
				case VType.Char:
					return (byte)((char)arg);
				case VType.Bool:
					return (byte)(((bool)arg) ? 1 : 0);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}

		
		/// <summary>
		/// Transform arg to byte if possible
		/// </summary>
		private static object ConformChar (object arg, VType atype)
		{
			switch (atype)
			{
				case VType.Short:
					return (char)((short)arg);
				case VType.Int:
					return (char)((int)arg);
				case VType.Long:
					return (char)((long)arg);
				case VType.Float:
					return (char)((float)arg);
				case VType.Double:
					return (char)((double)arg);
				case VType.String:
					return (char)int.Parse ((string)arg);
				case VType.Byte:
					return (char)((byte)arg);
				case VType.Char:
					return (char)((char)arg);
				default:
					throw new ArgumentException ("could not coerce type " + atype + " to bool");					
			}
		}


		private static Type BaseTypeOf (Type type)
		{
			if (type.BaseType != null)
				return type.BaseType;
			else
				return type;
		}

		
		static ValueTypeUtils ()
		{
			_map[typeof(short)] = VType.Short;
			_map[typeof(Int16)] = VType.Short;

			_map[typeof(int)] = VType.Int;
			_map[typeof(Int32)] = VType.Int;

			_map[typeof(long)] = VType.Long;
			_map[typeof(Int64)] = VType.Long;

			_map[typeof(float)] = VType.Float;
			_map[typeof(Single)] = VType.Float;

			_map[typeof(double)] = VType.Double;
			_map[typeof(Double)] = VType.Double;

			_map[typeof(byte)] = VType.Byte;
			_map[typeof(Byte)] = VType.Byte;

			_map[typeof(bool)] = VType.Bool;
			_map[typeof(Boolean)] = VType.Bool;

			_map[typeof(char)] = VType.Char;
			_map[typeof(Char)] = VType.Char;

			_map[typeof(IndexedVector)] = VType.Vector;
			_map[typeof(DenseVector)] = VType.Vector;
			_map[typeof(Vector<double>)] = VType.Vector;
			_map[typeof(SubviewVector)] = VType.Vector;

			_map[typeof(IndexedMatrix)] = VType.Matrix;
			_map[typeof(DenseMatrix)] = VType.Matrix;
			_map[typeof(Matrix<double>)] = VType.Matrix;

			_map[typeof(string)] = VType.String;

			_map[typeof(List<int>)] = VType.List;
			_map[typeof(List<double>)] = VType.List;
			_map[typeof(List<string>)] = VType.List;
		}
		
		// Variables
		
		static Dictionary<Type,VType>	_map = new Dictionary<Type, VType>();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/serialization/IPersist.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.serialization
{
	/// <summary>
	/// Interface for classes that implement persistance / serialization into some hierarchical tagged format
	/// </summary>
	public interface IPersist<Format>
	{
		
		/// <summary>
		/// Gets and sets the state into the object into the appropriate format
		/// </summary>
		Format				State			{ get; set; }
	}

	/// <summary>
	/// Interface for classes that implement persistance / serialization into some hierarchical tagged format
	/// </summary>
	public interface IPersistRO<Format>
	{

		/// <summary>
		/// Gets the state of the object in required format
		/// </summary>
		Format				State			{ get; }
	}
			
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/ExclusiveLock.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system
{
	/// <summary>
	/// Exclusive lock for a resource
	/// </summary>
	public class ExclusiveLock : ILock
	{		
		/// <summary>
		/// Obtain exclusive lock
		/// </summary>
		public void Lock ()
		{
			Monitor.Enter (this);
		}
		
		/// <summary>
		/// Exit exclusive lock
		/// </summary>
		public void Unlock ()
		{
			Monitor.Exit (this);
		}
		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			if (timeout > 0)
				return Monitor.TryEnter (this, timeout);
			else
				return Monitor.TryEnter (this);
		}	
		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/ILock.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system
{
	/// <summary>
	/// Common lock functions
	/// </summary>
	public interface ILock
	{
		/// <summary>
		/// Lock the resource
		/// </summary>
		void					Lock ();
		
		
		/// <summary>
		/// UnLock the resource
		/// </summary>
		void					Unlock ();
		
		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		bool					TryLock (int timeout = 0);
		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/ObjectPool.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.system
{


	/// <summary>
	/// Single threaded pool for objects reuse: this is not thread-safe, hence either should be thread-local
	/// <p/>
	/// Subclasses should add a Alloc with arguments, refering to Alloc() to get unparameterized object
	/// </summary>
	public sealed class STObjectPool<T> where T : new()
	{
		public STObjectPool (int maxsize, Func<T> creator = null)
		{
			if (creator != null)
				_creator = creator;
			else
				_creator = () => new T();
			
			_pool = new T[maxsize];
			_in = 0;
			_out = 0;
		}
		
		
		// Properties
		
		public int PoolSize
			{ get { return _pool.Length; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Get / Create an allocation of class instance
		/// </summary>
		public T Alloc ()
		{
			if (_in == _out)
				return _creator();

			var next = (_out + 1) % _pool.Length;
			var obj = _pool[_out];
			_out = next;

			return obj;
		}
		

		/// <summary>
		/// Free the specified obj.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public void Free (T obj)
		{
			if (obj == null)
				return;

			var next = (_in + 1) % _pool.Length;

			// if pool full return
			if (next == _out)
				return;

			// add to queue
			_pool[_in] = obj;
			_in = next;
		}


		// Variables
		
		readonly Func<T>	_creator;
		readonly T[]		_pool;
		int					_in;
		int					_out;
	}


	/// <summary>
	/// Pool objects for reuse: this lock-free implementation taken from Mono source with modifications
	/// <p/>
	/// Subclasses should add a Alloc with arguments, refering to Alloc() to get unparameterized object
	/// </summary>
	public sealed class MTObjectPool<T> where T : new()
	{
		public MTObjectPool (int maxsize, Func<T> creator = null)
		{
			if (creator != null)
				_creator = creator;
			else
				_creator = () => new T();
			
			_pool = new T[maxsize];
			_index_add = 0L;
			_index_remove = 0L;
		}
		
		
		// Properties
		
		public int PoolSize
			{ get { return _pool.Length; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Get / Create an allocation of class instance
		/// </summary>
		public T Alloc ()
		{
			if (_index_remove == _index_add)
				return _creator();
			if ((_index_add & ~SignalBit) - 1L == _index_remove)
				return _creator();
			
			long i;
			T result;
			int tries = 3;
			
			do 
			{
				i = _index_remove;
				// check to see whether queue exhausted (index_add % size)
				if ((_index_add & ~SignalBit) - 1L == i || tries == 0)
					return _creator();
				
				result = _pool[(int)(i % _pool.Length)];				
			} 
			while (Interlocked.CompareExchange (ref _index_remove, i + 1, i) != i && --tries > 0);
			
			return result;
		}
		
		
		/// <summary>
		/// Free the specified obj.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public void Free (T obj)
		{
			if (obj == null || (_index_add - _index_remove) >= _pool.Length - 1)
				return;
			
			long idx;
			int outer_tries = 3;
			do 
			{
				int inner_tries = 10;
				do
				{
					idx = _index_add;
				}
				while ((idx & SignalBit) > 0L && --inner_tries > 0);
				
				if ((idx - _index_remove) >= (_pool.Length - 1))
					return;
				
			} 
			while (Interlocked.CompareExchange (ref _index_add, idx + 1 + SignalBit, idx) != idx && --outer_tries > 0);
			
			_pool[(int)(idx % _pool.Length)] = obj;
			_index_add = _index_add - SignalBit;			
		}
		
		
		// Constants
		
		const long			SignalBit = 0x0800000000000000;
		
		// Variables
		
		readonly Func<T>	_creator;
		readonly T[]		_pool;
		long				_index_add;
		long				_index_remove;
	}

}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/ReadWriteLock.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system
{
	/// <summary>
	/// Read write lock allowing single exclusive writer and multiple readers
	/// </summary>
	public class ReadWriteLock
	{
		public ReadWriteLock (LockRecursionPolicy policy = LockRecursionPolicy.SupportsRecursion)
		{
			_lock = new ReaderWriterLockSlim (policy);
			_rlock = new ReaderLock (this);
			_wlock = new WriterLock (this);
		}
		
		
		/// <summary>
		/// Reader lock that can be locked / unlocked by a particular reader thread
		/// </summary>
		public ILock ReadLock
			{ get { return _rlock; } }

		/// <summary>
		/// Writer lock that can be locked / unlocked by a particular writer thread
		/// </summary>
		public ILock WriteLock
			{ get { return _wlock; } }
		

        // Operations

		/// <summary>
		/// Wait for next write (as seen by the release of a write lock");
		/// </summary>
        public void WaitForNextWrite()
        {
			if (_written == null)
				_written = _lock;

			lock (_written)
			{
				Monitor.Wait (_written);
			}
        }
		

		// classes
		
		
		private class ReaderLock : ILock
		{
			public ReaderLock (ReadWriteLock src)
			{
				_lock = src;
			}
			
			public void Lock ()
				{ _lock._lock.EnterReadLock (); }
			
			public void Unlock ()
				{ _lock._lock.ExitReadLock (); }
			
			public bool TryLock (int timeout = 0)
				{ return _lock._lock.TryEnterReadLock (timeout); }
				
			
			// Variables
			
			private ReadWriteLock	_lock; 
		}

				
		
		private class WriterLock : ILock
		{
			public WriterLock (ReadWriteLock src)
			{
				_lock = src;
			}
			
			public void Lock ()
				{ _lock._lock.EnterWriteLock(); }
			
			public void Unlock ()
			{ 
				_lock._lock.ExitWriteLock();
				if (_lock._written != null)
				{
					lock (_lock._written) Monitor.PulseAll (_lock._written);
				}
			}
			
			public bool TryLock (int timeout = 0)
			{ 
				return _lock._lock.TryEnterWriteLock (timeout);
			}
				
			
			// Variables
			
			private ReadWriteLock	_lock; 
		}

		
		// Variables
		
		private ReaderWriterLockSlim	_lock;
		
		private ReaderLock				_rlock;		
		private WriterLock				_wlock;
        private object                  _written;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/ReentrantFairLockByMonitor.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system 
{ 
	/// <summary>
	/// Fair lock with reentrancy
	/// 
	/// Uses monitors to affect the locking, hence not the fastest locking solution
	/// </summary>
	public sealed class ReentrantFairLockByMonitor : ILock
	{ 

		// Properties

		public bool IsOwner 
			{ get { return _owner == Thread.CurrentThread.ManagedThreadId; } }

		public int Depth
			{ get { return _depth; } }


		// Functions


		/// <summary>
		/// Enter the lock
		/// </summary>
		public void Lock () 
		{ 
			var tid = Thread.CurrentThread.ManagedThreadId;
			lock (_lock)
			{
				// quick check for reentrant exit
				if (_owner == tid)
					{ _depth++; return; }

				// enqueue our thread ID for servicing
				_queue.Enqueue (tid);

				// wait until we can grab the lock
				while (_queue.Peek() != tid)
					Monitor.Wait (_lock);

				_owner = tid;
				_depth++;
			}
		} 


		/// <summary>
		/// Exit the lock
		/// </summary>
		public void Unlock () 
		{ 
			var tid = Thread.CurrentThread.ManagedThreadId;

			lock (_lock)
			{
				if (--_depth > 0)
					return;

				if (_queue.Dequeue() != tid)
					throw new ArgumentException ("thread not owning lock attempted to unlock lock");

				_owner = int.MinValue;
				if (_queue.Count > 0)
					Monitor.PulseAll (_lock);
			}
		} 

		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			throw new NotImplementedException ("try-lock not implemented");
		}	
		

		// Variables

		volatile int			_depth = 0;
		volatile int			_owner = int.MinValue;

		private Queue<int>		_queue = new Queue<int>();
		private object			_lock = new object();
	} 
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/SpinLock.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system
{
	/// <summary>
	/// Highly efficient lock for high-frequency locked-sections with thread-reentry (unlike
	/// the .NET SpinLock implementation).
	/// </summary>
	public class SpinLock : ILock
	{	
		// Properties

		/// <summary>
		/// Returns owner of lock (note that this may be out-of-date)
		/// </summary>
		public int Owner
			{ get { return _depth == 0 ? 0 : _owner; } }

		/// <summary>
		/// Determines whether lock is open for this thread (again, may be dated)
		/// </summary>
		public bool IsOpen
			{ get { return _depth == 0 ? true : _owner == Thread.CurrentThread.ManagedThreadId; } }
		
		/// <summary>
		/// Determines lock depth
		/// </summary>
		public int Depth
			{ get { return _depth; } }


		// Operations


		/// <summary>
		/// Obtain exclusive lock
		/// 
		/// We use a staggered spin time to reduce the contention for the atomic variables
		/// </summary>
		public void Lock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;

			// spin is a structure, so should be cheap to instantiate (BTW)
			SpinWait spin = new SpinWait();

			while (true)
			{
				// enter the guard (which doubles as the recursion depth)
				var depth = Interlocked.Increment (ref _depth);

				// if depth == 1 then we have ownership
				if (depth == 1)
				{
					_owner = tid;
					return;
				}

				// if depth > 1 and owner is us, then we are good
				else if (_owner == tid)
					return;

				// decrement depth since we did not acquire
				Interlocked.Decrement (ref _depth);

				// spinning part, busy-wait for a while
				spin.SpinOnce ();
			}

		}
		
		/// <summary>
		/// Exit exclusive lock
		/// </summary>
		public void Unlock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;
			if (tid != _owner)
				throw new Exception ("attempted to unlock lock not owned by thread: " + tid);

			var depth = Interlocked.Decrement (ref _depth);
			if (depth == 0)
				Thread.Yield();
		}

		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			var tid = Thread.CurrentThread.ManagedThreadId;

			// spin is a structure, so should be cheap to instantiate (BTW)
			SpinWait spin = new SpinWait();
			var Tstart = timeout > 0L ? SystemUtils.ClockMillis : 0L;

			while (true)
			{
				// enter the guard (which doubles as the recursion depth)
				var depth = Interlocked.Increment (ref _depth);

				// if depth == 1 then we have ownership
				if (depth == 1)
				{
					_owner = tid;
					return true;
				}
				
				// if depth > 1 and owner is us, then we are good
				else if (_owner == tid)
					return true;

				// decrement depth since we did not acquire
				Interlocked.Decrement (ref _depth);

				if (timeout == 0)
					return false;
				if ((SystemUtils.ClockMillis - Tstart) >= timeout)
					return false;
				
				// spinning part, busy-wait for a while
				spin.SpinOnce ();
			}
		}	


		// Meta

		public override string ToString ()
		{
			return string.Format ("[SpinLock: Depth={0}, Owner={1}, IsOpen={2}]", _depth, _owner, IsOpen);
		}


		// Constants

		static readonly int		NoOwner = 0x7fffffff;
		
		// Variables

		private int				_depth = 0;
		volatile int			_owner = NoOwner;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/system/TicketLock.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.system
{
	using MonoTicketLock = System.Threading.SpinLock;

	/// <summary>
	/// Highly efficient reentrant lock with no context switching (busy-waits) and with 
	/// FIFO thread selection.
	/// 
	/// This implementation is adapted from the mono SpinLock, but with reentrant adaptations
	/// </summary>
	public class TicketLock : ILock
	{	
		// Properties

		/// <summary>
		/// Returns owner of lock (note that this may be out-of-date)
		/// </summary>
		public int Owner
			{ get { return _depth == 0 ? 0 : _owner; } }

		/// <summary>
		/// Determines whether lock is open for this thread (again, may be dated)
		/// </summary>
		public bool IsOpen
			{ get { return _depth == 0 ? true : _owner == Thread.CurrentThread.ManagedThreadId; } }
		
		/// <summary>
		/// Determines lock depth
		/// </summary>
		public int Depth
			{ get { return _depth; } }


		// Operations


		/// <summary>
		/// Obtain exclusive lock
		/// 
		/// We use a staggered spin time to reduce the contention for the atomic variables
		/// </summary>
		public void Lock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;

			// enter the guard (which doubles as the recursion depth)
			var depth = Interlocked.Increment (ref _depth);

			// if depth == 1 then we may have ownership, but need to see if we were next (otherwise block)
			if (depth == 1)
				FairEnter (tid);

			// if depth > 1 and owner is us, then we are good
			else if (_owner == tid)
				return;

			// need to wait for our turn
			else
				FairEnter (tid);
		}


		/// <summary>
		/// Exit exclusive lock
		/// </summary>
		public void Unlock ()
		{
			var tid = Thread.CurrentThread.ManagedThreadId;
			if (tid != _owner)
				throw new Exception ("attempted to unlock lock not owned by thread: " + tid);

			var depth = Interlocked.Decrement (ref _depth);
			if (depth == 0)
			{
				_owner = int.MinValue;
				FairExit ();
			}
		}

		
		/// <summary>
		/// Tries to lock within the specified time period (returning true if acquired, false otherwise)
		/// </summary>
		/// <param name='timeout'>
		/// Timeout in milliseconds or 0 if no timeout
		/// </param>
		public bool TryLock (int timeout = 0)
		{
			var tid = Thread.CurrentThread.ManagedThreadId;
			
			// enter the guard (which doubles as the recursion depth)
			var depth = Interlocked.Increment (ref _depth);
			
			// if depth == 1 then we may have ownership, but need to see if we were next (otherwise block)
			if (depth == 1)
				return FairEnter (tid, timeout);
			
			// if depth > 1 and owner is us, then we are good
			else if (_owner == tid)
				return true;
			
			// need to wait for our turn
			else
				return FairEnter (tid, timeout);
		}	


		// Meta

		public override string ToString ()
		{
			return string.Format ("[TicketLock: Depth={0}, Owner={1}, IsOpen={2}]", _depth, _owner, IsOpen);
		}


		#region Ticket-Based Entry


		/// <summary>
		/// Wait until it is our turn to lock
		/// </summary>
		private void FairEnter (int tid)
		{
			Interlocked.Decrement (ref _depth);

			var taken = false;
			while (!taken)
				_ticketlock.Enter (ref taken);

			_owner = tid;
		}


		/// <summary>
		/// Wait until it is our turn to lock or until # of ms elapsed
		/// </summary>
		private bool FairEnter (int tid, int ms)
		{
			Interlocked.Decrement (ref _depth);

			var taken = false;
			_ticketlock.TryEnter (ms, ref taken);

			if (taken)
				_owner = tid;

			return taken;
		}


		/// <summary>
		/// Exit the lock
		/// </summary>
		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.Success)]
		private void FairExit ()
		{
			_ticketlock.Exit (true);
		}


		#endregion


		// Variables

		private int					_depth = 0;
		private int					_owner;
		private MonoTicketLock		_ticketlock = new MonoTicketLock ();
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/time/Clock.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.time
{	
	/// <summary>
	/// Time & clock related functions
	/// </summary>
	public class Clock
	{

		/// <summary>
		/// Gets or overrides current time.  Time is presented in UTC milliseconds since Jan 1 1970.
		/// </summary>
		/// <value>
		/// The clock now.
		/// </value>
		public static long Now
		{
			get
			{
				if (_Toverride > 0)
					return _Toverride;
				else
					return SystemUtils.ClockMillis;
			}
			set
				{ _Toverride = value; }
		}
		

		/// <summary>
		/// Provide current clock in local date/time
		/// </summary>
		public static ZDateTime Local
		{
			get 
			{
				return new ZDateTime (Now, ZTimeZone.Local);
			}
		}


		/// <summary>
		/// Provide current clock in UTC date/time
		/// </summary>
		public static ZDateTime UTC
		{
			get 
			{
				return new ZDateTime (Now, ZTimeZone.UTC);
			}
		}


		/// <summary>
		/// Determines whether is realtime or not.  It requires an observation period of
		/// 5 seconds before it decided whether is realtime or not.
		/// </summary>
		/// <returns>-1 if not realtime, 0 if uncertain, 1 if realtime</returns>
		/// <param name="Tnow">given time.</param>
		public static int IsRealTime (long Tnow)
		{
			if (_realtime != 0)
				return _realtime;

			if (_Tobservation_real == 0L)
			{
				_Tobservation_real = SystemUtils.ClockMillis;
				_Tobservation_given = Tnow;
			}

			var dt_given = Tnow - _Tobservation_given; 
			if (dt_given < 5000L)
				return 0;

			var dt_real = SystemUtils.ClockMillis - _Tobservation_real;
			var speedup = (double)dt_given / (double)dt_real;

			if (speedup >= 1.5)
				return _realtime = -1;
			else
				return _realtime = 1;
		}
		
		
		// Variables

		[ThreadStatic] static long			_Toverride;
		[ThreadStatic] static long			_Tobservation_given;
		[ThreadStatic] static long			_Tobservation_real;
		[ThreadStatic] static int			_realtime = 0;
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZDateTime.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.time
{
	/// <summary>
	/// DateTime with time zone.  The missing paired timezone is a major oversight in .NET
	/// </summary>
	public struct ZDateTime
	{	
		/// <summary>
		/// Create a zoned datetime, converting the local time into the equivalent time of given zone
		/// </summary>
		/// <param name='local'>
		/// Local time
		/// </param>
		/// <param name='zone'>
		/// Zone to convert to
		/// </param>
		public ZDateTime (DateTime local, ZTimeZone zone)
	    {
			_parts = 0L;
			switch (local.Kind)
			{
				case DateTimeKind.Local:
					DateTime utc = local.ToUniversalTime();
					_utc = (utc.Ticks - 621355968000000000L) / 10000L; 
		        	_zone = zone;
					break;			
					
				case DateTimeKind.Utc:
					_utc = (local.Ticks - 621355968000000000L) / 10000L; 
	        		_zone = zone;
					break;

				default:
					throw new ArgumentException ("cannot handled unspecified datetimes");
			}
	    }
		
		
		/// <summary>
		/// Create a date in given timezone given UTC clock value
		/// </summary>
		/// <param name='clock'>
		/// # of ms since Jan 1 1970
		/// </param>
		/// <param name='zone'>
		/// Zone to present in
		/// </param>
		public ZDateTime (long clock, ZTimeZone zone)
	    {
			_utc = clock;
	        _zone = zone;
			_parts = 0L;
	    }

		
		
		/// <summary>
		/// Specify a time in the given time zone
		/// </summary>
		/// <param name='year'>
		/// Year.
		/// </param>
		/// <param name='month'>
		/// Month.
		/// </param>
		/// <param name='day'>
		/// Day.
		/// </param>
		/// <param name='hr'>
		/// Hr.
		/// </param>
		/// <param name='mins'>
		/// Mins.
		/// </param>
		/// <param name='secs'>
		/// Secs.
		/// </param>
		/// <param name='ms'>
		/// Ms.
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (int year, int month, int day, int hr, int mins, int secs, int ms, ZTimeZone zone)
	    {
			DateTime baseutc = new DateTime (year, month, Math.Max(day,1), hr, mins, secs, ms, DateTimeKind.Utc);
			DateTime utc = (baseutc - zone.BaseUtcOffset);
			
			var dst = zone.GetDSTInfoFor (utc);
			if (zone.IsDaylightSavingTime (utc))
				_utc = (utc.Ticks - 621355968000000000L) / 10000L - dst.Offset;
			else
				_utc = (utc.Ticks - 621355968000000000L) / 10000L;
				
	        _zone = zone;
			_parts = 
				((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27) |
				((ulong)hr << 22) | ((ulong)mins << 16) | ((ulong)secs << 10) | (ulong)ms;
	    }
		
		
		/// <summary>
		/// Specify a time in the given time zone
		/// </summary>
		/// <param name='year'>
		/// Year.
		/// </param>
		/// <param name='month'>
		/// Month.
		/// </param>
		/// <param name='day'>
		/// Day.
		/// </param>
		/// <param name='hr'>
		/// Hr.
		/// </param>
		/// <param name='mins'>
		/// Mins.
		/// </param>
		/// <param name='secs'>
		/// Secs.
		/// </param>
		/// <param name='ms'>
		/// Ms.
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (int year, int month, int day, int hr, int mins, int secs, int ms, string zone)
			: this (year, month, day, hr, mins, secs, ms, new ZTimeZone(zone))
	    {
	    }
		
		
		/// <summary>
		/// Create date from string & zone spec
		/// </summary>
		/// <param name='date'>
		/// Date as string in some default known format
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime (string date, ZTimeZone zone = null)
	    {
			if (zone == null)
				zone = DefaultZoneFor (date);
			
			ZDateTime parsed = Parse (date, zone);

			_utc = parsed.Clock; 
	        _zone = parsed.TimeZone;
			_parts = parsed._parts;
	    }
		
		// Properties
		
	    public DateTime UTC 
			{ get { return new DateTime(_utc * 10000L + 621355968000000000L, DateTimeKind.Utc); } }
		
		public long Ticks
			{ get { return _utc * 10000L + 621355968000000000L; } }
		
		public DateTime Adjusted 
			{ get { return _zone.Convert (_utc); } }
	
	    public ZTimeZone TimeZone 
			{ get { return _zone; } }
		
		public long Clock
			{ get { return _utc; } }
		
		public int Year
			{ get { Convert(); return (int)(_parts >> 36); } }
		
		public int Month
			{ get { Convert(); return (int)(_parts >> 32) & 0x0000000f; } }
		
		public int Day
			{ get { Convert(); return (int)(_parts >> 27) & 0x0000001f; } }
		
		public int Hour
			{ get { Convert(); return (int)(_parts >> 22) & 0x0000001f; } }
		
		public int Minute
			{ get { Convert(); return (int)(_parts >> 16) & 0x0000003f; } }
		
		public int Second
			{ get { Convert(); return (int)(_parts >> 10) & 0x0000003f; } }
		
		public int Millisecond
			{ get { Convert(); return (int)(_parts & (long)0x000003ff); } }
		
		public ZTime Time
		{ 
			get 
			{ 
				var adj = Adjusted;
				return new ZTime (adj.Hour, adj.Minute, adj.Second, adj.Millisecond); 
			} 
		}
		
		
		public static ZDateTime Now
			{ get { return new ZDateTime (SystemUtils.ClockMillis, ZTimeZone.UTC); } }


		public bool IsJustDate
			{ get { return Hour == 0 && Minute == 0 && Second == 0 && Millisecond == 0; } }

		
		// Operations
		
		
		/// <summary>
		/// Time now in appropriate timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZDateTime TimeNowFor (ZTimeZone zone)
		{
			if (zone == ZTimeZone.GMT)
				return Now;
			else
				return new ZDateTime (SystemUtils.ClockMillis, zone);	
		}


		/// <summary>
		/// Convert time to the specified zone.
		/// </summary>
		/// <param name="zone">Zone.</param>
		public ZDateTime Convert (ZTimeZone zone)
		{
			if (zone == TimeZone)
				return this;
			else
				return new ZDateTime (Clock, zone);	
		}

		
		/// <summary>
		/// Add the specified hrs, mins, secs and ms.
		/// </summary>
		/// <param name='days'>
		/// Days to add.
		/// </param>
		/// <param name='hrs'>
		/// Hrs to add.
		/// </param>
		/// <param name='mins'>
		/// Mins to add.
		/// </param>
		/// <param name='secs'>
		/// Secs to add.
		/// </param>
		/// <param name='ms'>
		/// Milliseconds to add.
		/// </param>
		public ZDateTime Add (int days, int hrs, int mins, int secs, int ms)
		{
			TimeSpan span = new TimeSpan (days, hrs, mins, secs, ms);			
			return new ZDateTime (_utc + span.Ticks / 10000L, _zone);
		}
		
		
		/// <summary>
		/// Add the specified delta.
		/// </summary>
		/// <param name='delta'>
		/// Delta.
		/// </param>
		public ZDateTime Add (TimeSpan delta)
		{
			return new ZDateTime (_utc + delta.Ticks / 10000L, _zone);
		}
		
		
		// Meta
		
		
		public override int GetHashCode ()
		{
			return (int)_utc + _zone.GetHashCode();
		}
		
		public override bool Equals (object obj)
		{
			ZDateTime o = (ZDateTime)obj;
			return _utc == o._utc && _zone == o._zone;
		}
		
		public static implicit operator long (ZDateTime time)
			{ return time.Clock; }

		public static implicit operator ZDateTime (long time)
			{ return new ZDateTime (time, ZTimeZone.UTC); }

		
		public static bool operator== (ZDateTime a, ZDateTime b)
			{ return a._utc == b._utc; }

		public static bool operator!= (ZDateTime a, ZDateTime b)
			{ return a._utc != b._utc; }

		public static bool operator< (ZDateTime a, ZDateTime b)
			{ return a._utc < b._utc; }
		
		public static bool operator> (ZDateTime a, ZDateTime b)
			{ return a._utc > b._utc; }
		
		public static bool operator>= (ZDateTime a, ZDateTime b)
			{ return a._utc >= b._utc; }
		
		public static bool operator<= (ZDateTime a, ZDateTime b)
			{ return a._utc <= b._utc; }

		
		public override string ToString ()
		{
			return string.Format ("{0} {1}", Adjusted, _zone);
		}
		
		
		
		/// <summary>
		/// provide date as xml gmt form (2007-01-04T23:11:02.340Z).
		/// </summary>
		public string ToXmlDateTime ()
		{
			return string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}.{6:000}Z", Year, Month, Day, Hour, Minute, Second, Millisecond);
		}
		
		
		/// <summary>
		/// provide date as xml gmt form (2007-01-04T23:11:02.340Z).
		/// </summary>
		public static string ToXmlDateTime (long time)
		{
			var ztime = new ZDateTime (time, ZTimeZone.UTC);
			return ztime.ToXmlDateTime ();
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340).
		/// </summary>
		public string ToExcelDateTime ()
		{
			return string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:000}", Year, Month, Day, Hour, Minute, Second, Millisecond);
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340)..
		/// </summary>
		public static string ToExcelDateTime (long time)
		{
			var ztime = new ZDateTime (time, ZTimeZone.UTC);
			return ztime.ToExcelDateTime ();
		}

		/// <summary>
		/// provide date as excel gmt form (2007-01-04 23:11:02.340).
		/// </summary>
		public string ToExcelDateTime (ZTimeZone zone)
		{
			var ztime = new ZDateTime (Clock, zone);
			return ztime.ToExcelDateTime();
		}

		
		/// <summary>
		/// Parse date from date & zone spec string
		/// </summary>
		/// <param name='date'>
		/// Date as string in some default known format
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZDateTime Parse (string date, string zone)
	    {
			if (date == null || zone == null)
				throw new ArgumentException ("date string or timezone is null");

			ZTimeZone czone = ZTimeZone.Find (zone);
			return new ZDateTime (date, czone);
	    }


		/// <summary>
		/// Minimum of 2 times
		/// </summary>
		/// <param name="a">The 1st time.</param>
		/// <param name="b">The 2nd time.</param>
		public static ZDateTime Min (ZDateTime a, ZDateTime b)
		{
			if (a.Clock < b.Clock)
				return a;
			else
				return b;
		}


		/// <summary>
		/// Max of 2 times
		/// </summary>
		/// <param name="a">The 1st time.</param>
		/// <param name="b">The 2nd time.</param>
		public static ZDateTime Max (ZDateTime a, ZDateTime b)
		{
			if (a.Clock > b.Clock)
				return a;
			else
				return b;
		}


		// Implementation


		private static ZTimeZone DefaultZoneFor (string date)
		{
			if (date[date.Length-1] == 'Z')
				return ZTimeZone.UTC;
			else
				return ZTimeZone.Local;
		}
		
		
		private static ZDateTime Parse (string date, ZTimeZone zone)
		{
			DateParser parser = DateParser.DefaultParser;
			return parser.Parse (date, zone);
		}
		
		
		private void Convert ()
		{
			if (_parts > 0)
				return;
			
			var generator = _zone.Generator;
			var info = generator.DateAndTimeFor (_utc);
			_parts = info.Encoded;
		}
				
				
		// Variables
		
		private long			_utc;
		private ulong			_parts;
	    private ZTimeZone		_zone;
	}

}
// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZDateTimeGenerator.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.time
{
				
	/// <summary>
	/// date/time info generator
	/// </summary>
	public class ZDateTimeInfoGenerator
	{
		public ZDateTimeInfoGenerator (ZTimeZone zone)
		{
			_zone = zone;
			_zone_offset = (long)zone.BaseUtcOffset.TotalMilliseconds;
		}
		
		
		// Functions
	
	
		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZDateTimeInfo DateAndTimeFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var timepart = (int)(adjclock % (24L*3600L*1000L));
			
			var hour = timepart / (3600*1000);
			var min = (timepart % (3600*1000)) / 60000;
			var sec = (timepart % 60000) / 1000;
			var ms = timepart % 1000;
			
			// determine new date if clock does not fall within day range
			var netclock = adjclock * 10000L + 621355968000000000L;
			if (netclock < _day_start || netclock > _day_end)
 				NewYMD (netclock);
				
			return new ZDateTimeInfo (_year, _month, _day, hour, min, sec, ms);
		}



		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZDateTimeInfo DateFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			var netclock = adjclock * 10000L + 621355968000000000L;
			
			// determine new date if clock does not fall within day range
			if (netclock < _day_start || netclock > _day_end)
 				NewYMD (netclock);
				
			return new ZDateTimeInfo (_year, _month, _day);
		}

		
		
		/// <summary>
		/// Date / time information for clock
		/// </summary>
		public ZTime TimeFor (long clock)
		{
			// if entered a new year need to recompute DST
			if (clock > _year_end || clock < _year_start)
				NewDST (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var netclock = adjclock * 10000L + 621355968000000000L;
			var timepart = (int)((netclock % TicksPerDay) / 10000L);
			
			var hour = timepart / (3600*1000);
			var min = (timepart % (3600*1000)) / 60000;
			var sec = (timepart % 60000) / 1000;
			var ms = timepart % 1000;
				
			return new ZTime (hour,min,sec,ms);
		}

		

		// Implementation: DST
		
		
		private void NewYMD (long netclock)
		{
			// n = number of days since 1/1/0001
            int n = (int)(netclock / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            int y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            int y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            int y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            int y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
            _year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
            
            // n = day number within year
            n -= y1 * DaysPerYear;

			// Leap year calculation looks different from IsLeapYear since y1, y4, and y100 are relative to year 1, not year 0
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear? DaysToMonth366: DaysToMonth365;
            
			// All months have less than 32 days, so n >> 5 is a good conservative estimate for the month
            var month = n >> 5 + 1;
            // m = 1-based month number
            while (n >= days[month]) month++;
			_month = month;
			
            // Return 1-based day-of-month
            _day = n - days[month - 1] + 1;
			
			// setup day window
			_day_start = (netclock / TicksPerDay) * TicksPerDay;
			_day_end = (_day_start + TicksPerDay - 1);
		}
		
		
		private void NewDST (DateTime time)
		{
			var rule = GetApplicableRule (_zone, time);
			var year = time.Year;
			var utcoffset = _zone.Underlier.BaseUtcOffset;
			
			if (rule != null)
			{
				var Rstart = rule.DaylightTransitionStart;
				var Rend = rule.DaylightTransitionEnd;
				
				DateTime DST_start = TransitionPoint (
					Rstart, year);
				DateTime DST_end = TransitionPoint (
					Rend, 
					year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
				
				DST_start -= utcoffset;
				DST_end -= (utcoffset + rule.DaylightDelta);
	
				_DST_start = (DST_start.Ticks - 621355968000000000L) / 10000L;
				_DST_end = (DST_end.Ticks - 621355968000000000L) / 10000L;
				_DST_offset = (long)rule.DaylightDelta.TotalMilliseconds;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_year_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_year_end = (yend.Ticks - 621355968000000000L) / 10000L;
			}
			else
			{
				_DST_start = long.MaxValue;
				_DST_end = 0;
				_DST_offset = 0;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_year_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_year_end = (yend.Ticks - 621355968000000000L) / 10000L;

			}
		}
		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (ZTimeZone ourzone, DateTime time)
		{
			TimeZoneInfo zone = ourzone.Underlier;
			if (zone == TimeZoneInfo.Utc)
				return null;
			
			var Tclock = time.Ticks + zone.BaseUtcOffset.Ticks;
			
			// use local to avoid another thread changing reference during test
			var srule = _rule;
			if (srule != null && IsAppropriateRule (srule, Tclock))
				return srule;
			
			var rulelist = zone.GetAdjustmentRules();
			foreach (var rule in rulelist)
			{
				if (IsAppropriateRule (rule, Tclock))
					return _rule = rule;
			}
			
			return null;
		}

		
		private bool IsAppropriateRule (TimeZoneInfo.AdjustmentRule rule, long Tclock)
		{
			if (rule.DateStart.Ticks > Tclock)
				return false;
			if (rule.DateEnd.Ticks >= Tclock)
				return true;
			else
				return false;
		}
		
		
		private DateTime TransitionPoint (TimeZoneInfo.TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
				return new DateTime (year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;

			DayOfWeek first = (new DateTime (year, transition.Month, 1)).DayOfWeek;
			DayOfWeek target = transition.DayOfWeek;

			// locate first dayofweek 
			int dayadjust = (first > target) ? (7 - (int)first + 1) : ((int)target - (int)first + 1);
			// roll to the nth
			int day = dayadjust + (transition.Week - 1) * 7;
			if (day >  DateTime.DaysInMonth (year, transition.Month))
				day -= 7;
			
			return new DateTime (year, transition.Month, day) + transition.TimeOfDay.TimeOfDay;
		}
		
		
		
		// Constants
		
		private const int 				dp400 = 146097;
		private const int				dp100 = 36524;
		private const int				dp4 = 1461;

		// Number of 100ns ticks per time unit
        private const long 				TicksPerMillisecond = 10000;
        private const long 				TicksPerSecond = TicksPerMillisecond * 1000;
        private const long 				TicksPerMinute = TicksPerSecond * 60;
        private const long 				TicksPerHour = TicksPerMinute * 60;
        private const long 				TicksPerDay = TicksPerHour * 24;
    
        // Number of milliseconds per time unit
        private const int 				MillisPerSecond = 1000;
        private const int 				MillisPerMinute = MillisPerSecond * 60;
        private const int 				MillisPerHour = MillisPerMinute * 60;
        private const int 				MillisPerDay = MillisPerHour * 24;
    
        // Number of days in a non-leap year
        private const int 				DaysPerYear = 365;
        // Number of days in 4 years
        private const int 				DaysPer4Years = DaysPerYear * 4 + 1;
        // Number of days in 100 years
        private const int 				DaysPer100Years = DaysPer4Years * 25 - 1;
        // Number of days in 400 years
        private const int 				DaysPer400Years = DaysPer100Years * 4 + 1;
    
        // Number of days from 1/1/0001 to 12/31/1600
        private const int 				DaysTo1601 = DaysPer400Years * 4;
        // Number of days from 1/1/0001 to 12/30/1899
        private const int 				DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
        // Number of days from 1/1/0001 to 12/31/9999
        private const int 				DaysTo10000 = DaysPer400Years * 25 - 366;
    
        private const long 				FileTimeOffset = DaysTo1601 * TicksPerDay;
        private const long 				DoubleDateOffset = DaysTo1899 * TicksPerDay;

		// The minimum OA date is 0100/01/01 & the maximum OA date is 9999/12/31
        private const long 				OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;
        // All OA dates must be greater than (not >=) OADateMinAsDouble
        private const double 			OADateMinAsDouble = -657435.0;
        // All OA dates must be less than (not <=) OADateMaxAsDouble
        private const double 			OADateMaxAsDouble = 2958466.0;
    
        private static readonly int[] DaysToMonth365 = 
			{ 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly int[] DaysToMonth366 = 
			{ 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

		
		// Variables
		
		private TimeZoneInfo.AdjustmentRule 	_rule; 
		private ZTimeZone						_zone;
		private long							_zone_offset;

		private long							_year_start;
		private long							_year_end;
		private long							_day_start;
		private long							_day_end;
		
		private long							_DST_start;
		private long							_DST_end;
		private long							_DST_offset;
		
		private int								_year;
		private int								_month;
		private int								_day;
	}

}

// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZDateTimeInfo.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.time
{
	/// <summary>
	/// Quick calculation of time of day given a clock timestamp and timezone
	/// </summary>
	public struct ZDateTimeInfo
	{
		public ZDateTimeInfo (int year, int month, int day, int hr, int min, int sec, int ms = 0)
		{
			_parts =
				((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27) |
				((ulong)hr << 22) | ((ulong)min << 16) | ((ulong)sec << 10) | (ulong)ms;
		}

		public ZDateTimeInfo (int year, int month, int day)
		{
			_parts = ((ulong)year << 36) | ((ulong)month << 32) | ((ulong)day << 27);
		}

		
		// Properties
		
		public int Year
			{ get { return (int)((_parts >> 36) & 0x000007ffL); } }
		
		public int Month
			{ get { return (int)((_parts >> 32) & 0x0000000fL); } }
		
		public int Day
			{ get { return (int)((_parts >> 27) & 0x0000001fL); } }
		
		public int Hour
			{ get { return (int)((_parts >> 22) & 0x0000001fL); } }
		
		public int Minute
			{ get { return (int)((_parts >> 16) & 0x0000003fL); } }
		
		public int Second
			{ get { return (int)((_parts >> 10) & 0x0000003fL); } }
		
		public int Millisecond
			{ get { return (int)(_parts & 0x000003ffL); } }
		
		public ulong Encoded
			{ get { return _parts; } }
		
		// Variables
		
		private ulong	_parts;
	}
	
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZDateTimeRange.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.time
{
	/// <summary>
	/// Specific date/time range.
	/// </summary>
	public class ZDateTimeRange
	{
		public ZDateTimeRange (ZDateTime start, ZDateTime end)
		{
			_start = start;
			_end = end;
		}
		
		public ZDateTimeRange (string start, string end, ZTimeZone zone)
		{
			_start = new ZDateTime (start, zone);
			_end = new ZDateTime (end, zone);
		}
		
		// Properties
		
		public ZDateTime Start
			{ get { return _start; } }

		public ZDateTime End
			{ get { return _end; } }

		
		// Operation
		
		
		/// <summary>
		/// Determine whether given time within the specified time range.
		/// </summary>
		public bool Within (DateTime time)
		{
			long clock = (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
			return clock >= _start.Clock && clock <= _end.Clock;
		}

		/// <summary>
		/// Determine whether given time within the specified time range.
		/// </summary>
		public bool Within (ZDateTime time)
		{
			long clock = time.Clock;
			return clock >= _start.Clock && clock <= _end.Clock;
		}

		
		/// <summary>
		/// Determine whether given clock time within the specified time range.   Note that the clock
		/// must be in UTC ms since Jan 1 1970 (this is not the default in .NET, so requires conversion).
		/// </summary>
		public bool Within (long clock)
		{
			return clock >= _start.Clock && clock <= _end.Clock;
		}
		
		
		/// <summary>
		/// Determine whether given time range intersects with this one
		/// </summary>
		/// <param name='Tstart'>
		/// start time
		/// </param>
		/// <param name='Tend'>
		/// end time
		/// </param>
		public bool Intersects (long Tstart, long Tend)
		{
			if (Tstart <= _start.Clock && Tend >= _start.Clock)
				return true;
			if (Tstart > _start.Clock && Tstart < _end.Clock)
				return true;
			else
				return false;
		}

		
		public override string ToString ()
		{
			return "[" + _start + ", " + _end + "]"; 
		}
		
		
		// Variables
		
		private ZDateTime		_start;
		private ZDateTime		_end;
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZDaylightSavings.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.time
{
	/// <summary>
	/// Daylight savings information for a particular date / timezone
	/// </summary>
	public struct ZDaylightSavings
	{
		public ZDaylightSavings (long DST_start, long DST_end, long DST_offset)
		{
			_DST_start = DST_start;
			_DST_end = DST_end;
			_DST_offset = DST_offset;
		}

		public ZDaylightSavings (DateTime DST_start, DateTime DST_end, long DST_offset)
		{
			_DST_start = (DST_start.Ticks - 621355968000000000L) / 10000L;
			_DST_end = (DST_end.Ticks - 621355968000000000L) / 10000L;
			_DST_offset = DST_offset;
		}
		
		
		// Properties
		
		/// <summary>
		/// Start of DST in UTC time (in ms since Jan 1, 1970)
		/// </summary>
		public long Start
			{ get { return _DST_start; } }
		
		/// <summary>
		/// End of DST in UTC time (in ms since Jan 1, 1970)
		/// </summary>
		public long End
			{ get { return _DST_end; } }
		
		/// <summary>
		/// Daylight savings time offset in milliseconds
		/// </summary>
		public long Offset
			{ get { return _DST_offset; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Determines whether the given utc time is in daylight savings time or not
		/// </summary>
		/// <returns>
		/// <c>true</c> if should apply daylight savings, false otherwise
		/// </returns>
		/// <param name='utc'>
		/// UTC time
		/// </param>
		public bool IsInDaylightSavings (DateTime utc)
		{
			var clock = (utc.Ticks - 621355968000000000L) / 10000L;
			return (clock >= Start && clock <= End);
		}
		
		/// <summary>
		/// Determines whether the given utc time is in daylight savings time or not
		/// </summary>
		/// <returns>
		/// <c>true</c> if should apply daylight savings, false otherwise
		/// </returns>
		/// <param name='utc'>
		/// UTC time in milliseconds since Jan 1 1970
		/// </param>
		public bool IsInDaylightSavings (long utc)
		{
			return (utc >= Start && utc <= End);
		}
		
		
		// Variables
		
		private long		_DST_start;
		private long		_DST_end;
		private long		_DST_offset;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZTime.cs
// -------------------------------------------
﻿// 
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





namespace bridge.common.time
{
	/// <summary>
	/// Represents a time (absent of date), to millisecond granularity
	/// <ul>
	/// 	<li>13:30</li>
	/// 	<li>13:30:10:400</li>
	/// </ul>
	/// </summary>
	[Serializable]
	public struct ZTime
	{
		public readonly static ZTime		TIME_SOD = new ZTime (0,0,0,0);
		public readonly static ZTime		TIME_EOD = new ZTime (23,59,59,999);
		
		
		// Ctors
		
		
		/// <summary>
		/// Create time from string
		/// </summary>
		/// <param name='time'>
		/// time in HH:MM:SS:mmm format (or abbreviated)
		/// </param>
		public ZTime (string time)
		{
			string hr = StringUtils.Or (StringUtils.Field (time, 0, ':'), "0");
			string min = StringUtils.Or (StringUtils.Field (time, 1, ':'), "0");
			string sec = StringUtils.Or (StringUtils.Field (time, 2, ':'), "0");
			string ms = StringUtils.Or (StringUtils.Field (time, 3, ':'), "0");
			
			_time = 
				int.Parse(hr) * 3600 * 1000 +
				int.Parse(min) * 60 * 1000 +
				int.Parse(sec) * 1000 +
				int.Parse(ms);
		}
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="bridge.common.time.ZTime"/> struct.
		/// </summary>
		/// <param name='hr'>
		/// Hour
		/// </param>
		/// <param name='min'>
		/// Minutes
		/// </param>
		/// <param name='sec'>
		/// Seconds.
		/// </param>
		/// <param name='ms'>
		/// Milliseconds.
		/// </param>
		public ZTime (int hr, int min, int sec = 0, int ms = 0)
		{
			_time = hr * 3600 * 1000 + min * 60*1000 + sec * 1000 + ms;
		}
		
		
		// Properties
		
		
		public int Hour
			{ get { return _time / (3600 * 1000); } }
		
		public int Minute
			{ get { return (_time / (60*1000)) % 60; } }
		
		public int Second
			{ get { return (_time / 1000) % 60; } }
				
		public int MilliSecond
			{ get { return _time % 1000; } }
		
		public long UntilEndOfDay
			{ get { return 24L*3600000L - (long)_time; } }
		
		public long FromStartOfDay
			{ get { return _time; } }
		
		
		// Operations
		
				
		/// <summary>
		/// Convert to date/time (within current day, may be before current time)
		/// </summary>
		/// <returns>
		/// The date/time set according to this time spec.
		/// </returns>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZDateTime ToTime (long Tnow = 0L, ZTimeZone zone = null)
		{
			if (zone == null)
				zone = ZTimeZone.Local;
			
			if (Tnow == 0L)
				Tnow = Clock.Now;

			ZDateTime now = new ZDateTime (Tnow, zone);
			return new ZDateTime (now.Year, now.Month, now.Day, Hour, Minute, Second, MilliSecond, zone);
		}
	
		
		/// <summary>
		/// Convert to date/time (within current day if time after current time, otherwise following day)
		/// </summary>
		public ZDateTime ToNextTime (long Tnow = 0L, ZTimeZone zone = null)
		{
			if (zone == null)
				zone = ZTimeZone.Local;

			if (Tnow == 0L)
				Tnow = Clock.Now;
			
			var Tproj = ToTime(Tnow, zone);
			if (Tproj.Clock >= Tnow)
				return Tproj;
			else
				return Tproj.Add (1, 0, 0, 0, 0);
		}
		
		/// <summary>
		/// Provide offset in milliseconds from other time (if this time is > other will be +, otherwise -)
		/// </summary>
		/// <returns>
		/// Millisecond difference
		/// </returns>
		/// <param name='other'>
		/// Other time
		/// </param>
		public long OffsetFrom (ZTime other)
		{
			return Difference (this, other);
		}
		
		
		/// <summary>
		/// Provide the difference Ta and Tb in milliseconds (Ta - Tb).
		/// </summary>
		/// <param name='Ta'>
		/// Ta.
		/// </param>
		/// <param name='Tb'>
		/// Tb.
		/// </param>
		public static long Difference (ZTime Ta, ZTime Tb)
		{
			return Ta._time - Tb._time;
		}
		
		
		// Class Methods
		
	
		
		/// <summary>
		/// Get Time component of given date
		/// </summary>
		/// <param name='date'>
		/// date (date/time)
		/// </param>
		public static ZTime TimeOf (ZDateTime date)
		{
			return new ZTime (date.Hour, date.Minute, date.Second, date.Millisecond);
		}
	
		
		/// <summary>
		/// Get Time component of given date
		/// </summary>
		/// <param name='clock'>
		/// Clock (UTC ms since Jan 1 1970).
		/// </param>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public static ZTime TimeOf (long clock, ZTimeZone zone = null)
		{
			zone = zone ?? ZTimeZone.Local;
			return TimeOf (new ZDateTime (clock, zone));
		}
		
		
		
		// Meta
		
		public override int GetHashCode ()
		{
			return _time;
		}
		
		public override bool Equals (object obj)
		{
			ZTime o = (ZTime)obj;
			return _time == o._time;
		}

		public static implicit operator ZTime (string s)
			{ return new ZTime (s); }
		
		public static bool operator== (ZTime a, ZTime b)
			{ return a._time == b._time; }
		
		public static bool operator!= (ZTime a, ZTime b)
			{ return a._time != b._time; }
		
		public static bool operator< (ZTime a, ZTime b)
			{ return a._time < b._time; }
		
		public static bool operator> (ZTime a, ZTime b)
			{ return a._time > b._time; }
		
		public static bool operator<= (ZTime a, ZTime b)
			{ return a._time <= b._time; }
		
		public static bool operator>= (ZTime a, ZTime b)
			{ return a._time >= b._time; }
		
		public static int operator- (ZTime a, ZTime b)
			{ return a._time - b._time; }
		
		
		public override string ToString ()
			{ return "" + Hour + ":" + Minute + ":" + Second + ":" + MilliSecond; }
		
		
		// Variable
		
		private int			_time;
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZTimeOfDay.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.time
{
	/// <summary>
	/// Quick calculation of time of day given a clock timestamp and timezone
	/// </summary>
	public class ZTimeOfDay
	{
		/// <summary>
		/// Create time zone from .NET timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZTimeOfDay (ZTimeZone zone)
		{
			_zone = zone;
			_zone_offset = (long)zone.BaseUtcOffset.TotalMilliseconds;
		}
		
		
		
		// Functions
		
		
		/// <summary>
		/// Determines whether given UTC time is in daylight savings or not (for this zone)
		/// </summary>
		/// <returns>
		/// <c>true</c> if clock is in daylight savings; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='clock'>
		/// time in UTC ms since Jan 1 1970
		/// </param>
		public ZTime timeOf (long clock)
		{			
			if (clock > _good_end || clock < _good_start)
				NewTime (new DateTime(clock * 10000L + 621355968000000000L, DateTimeKind.Utc));
			
			var dstadj = (clock < _DST_start || clock > _DST_end) ? 0 : _DST_offset;
			var adjclock = clock + _zone_offset + dstadj; 
			
			var netclock = adjclock * 10000L + 621355968000000000L;
			var hour = (int)((netclock  % TicksPerDay) / TicksPerHour);
			var min = (int)((netclock % TicksPerHour) / TicksPerMinute);
			var sec = (int)((netclock % TicksPerMinute) / TicksPerSecond);
			var ms = (int)((netclock % TicksPerSecond) / TicksPerMillisecond);
			
			return new ZTime (hour, min, sec, ms);
		}
		
		
		// Implementation: DST
		
		
		private void NewTime (DateTime time)
		{
			var rule = GetApplicableRule (_zone.Composed, time);
			var year = time.Year;
			var utcoffset = _zone.BaseUtcOffset;
			
			if (rule != null)
			{
				var Rstart = rule.DaylightTransitionStart;
				var Rend = rule.DaylightTransitionEnd;
				
				DateTime DST_start = TransitionPoint (
					Rstart, year);
				DateTime DST_end = TransitionPoint (
					Rend, 
					year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
				
				DST_start -= utcoffset;
				DST_end -= (utcoffset + rule.DaylightDelta);
	
				_DST_start = (DST_start.Ticks - 621355968000000000L) / 10000L;
				_DST_end = (DST_end.Ticks - 621355968000000000L) / 10000L;
				_DST_offset = (long)rule.DaylightDelta.TotalMilliseconds;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_good_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_good_end = (yend.Ticks - 621355968000000000L) / 10000L;
			}
			else
			{
				_DST_start = long.MaxValue;
				_DST_end = 0;
				_DST_offset = 0;
				
				DateTime ystart = new DateTime(year, 1, 1, 0,0,0,0, DateTimeKind.Utc) - utcoffset;
				DateTime yend = new DateTime(year, 12, 31, 23,59,59,999, DateTimeKind.Utc) - utcoffset;
				
				_good_start = (ystart.Ticks - 621355968000000000L) / 10000L;
				_good_end = (yend.Ticks - 621355968000000000L) / 10000L;

			}
		}
		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (TimeZoneInfo zone, DateTime time)
		{
			var Tclock = time.Ticks + zone.BaseUtcOffset.Ticks;
			
			// use local to avoid another thread changing reference during test
			var srule = _rule;
			if (srule != null && IsAppropriateRule (srule, Tclock))
				return srule;
			
			var rulelist = zone.GetAdjustmentRules();
			foreach (var rule in rulelist)
			{
				if (IsAppropriateRule (rule, Tclock))
					return _rule = rule;
			}
			
			return null;
		}

		
		private bool IsAppropriateRule (TimeZoneInfo.AdjustmentRule rule, long Tclock)
		{
			if (rule.DateStart.Ticks > Tclock)
				return false;
			if (rule.DateEnd.Ticks >= Tclock)
				return true;
			else
				return false;
		}
		
		
		private DateTime TransitionPoint (TimeZoneInfo.TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
				return new DateTime (year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;

			DayOfWeek first = (new DateTime (year, transition.Month, 1)).DayOfWeek;
			DayOfWeek target = transition.DayOfWeek;

			// locate first dayofweek 
			int dayadjust = (first > target) ? (7 - (int)first + 1) : ((int)target - (int)first + 1);
			// roll to the nth
			int day = dayadjust + (transition.Week - 1) * 7;
			if (day >  DateTime.DaysInMonth (year, transition.Month))
				day -= 7;
			
			return new DateTime (year, transition.Month, day) + transition.TimeOfDay.TimeOfDay;
		}
		
		
		private static int GetYear (long netclock)
		{
			int totaldays = (int)(netclock / TicksPerDay);
			int num400 = (totaldays / dp400);
			totaldays -=  num400 * dp400;
		
			// leap year adjustment
			int num100 = (totaldays / dp100);
			if (num100 == 4)
				num100 = 3;
			
			totaldays -= (num100 * dp100);

			int num4 = totaldays / dp4;
			totaldays -= (num4 * dp4);

			int numyears = totaldays / 365 ;
			
			// another leap adjustment
			if (numyears == 4)
				numyears = 3;
			
			return num400*400 + num100*100 + num4*4 + numyears + 1;
		}
		
		
		// Constants
		
		private const int 			dp400 = 146097;
		private const int			dp100 = 36524;
		private const int			dp4 = 1461;
		
		private const long 			TicksPerDay = 864000000000L;
		private const long 			TicksPerHour = 36000000000L;
		private const long 			TicksPerMillisecond = 10000L;
		private const long 			TicksPerMinute = 600000000L;
		private const long 			TicksPerSecond = 10000000L;	
		
		
		// Variables
		
		private TimeZoneInfo.AdjustmentRule 	_rule; 
		private ZTimeZone						_zone;
		private long							_zone_offset;

		private long							_good_start;
		private long							_good_end;
		
		private long							_DST_start;
		private long							_DST_end;
		private long							_DST_offset;
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZTimeRange.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.time
{
	/// <summary>
	/// Represents a time range
	/// </summary>
	[Serializable]
	public class ZTimeRange
	{
		
		/// <summary>
		/// Create interval spanning start to end times
		/// </summary>
		/// <param name='start'>
		/// Start time.
		/// </param>
		/// <param name='end'>
		/// End time.
		/// </param>
		public ZTimeRange (ZTime start, ZTime end)
		{
			_start = start;
			_end = end;
			Debug.Assert (end.MilliSecond >= start.MilliSecond);
		}
		
		
		/// <summary>
		/// Create interval, parsing from string form:
		/// <p>
		/// Form can be one of:
		/// <ul>
		/// 	<li>[<time>, <time>]</li>
		/// 	<li><time>,<time></li>
		/// </ul>
		/// </summary>
		/// <param name='interval'>
		/// Interval.
		/// </param>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
		public ZTimeRange (string interval)
		{
			int isplit = interval.IndexOf (',');
			if (isplit == -1)
				throw new Exception ("time interval not in proper format: " + interval);
			
			string sstart = interval.Substring (0, isplit).Trim ();
			string send = interval.Substring (isplit+1).Trim();
			
			if (sstart[0] == '[')
				sstart = sstart.Substring (1);
			if (send[send.Length-1] == ']')
				send = send.Substring (0, send.Length-1);
			
			_start = new ZTime (sstart);
			_end = new ZTime (send);
			Debug.Assert (_end.MilliSecond >= _start.MilliSecond);
		}
		
		
		// Properties
		

		public ZTime StartTime
			{ get { return _start; } }
		
		public ZTime EndTime
			{ get { return _end; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Determine if time for given date within time-interval
		/// </summary>
		/// <param name='date'>
		/// date from which time is extracted (in local timezone)
		/// </param>
		public bool Within (ZDateTime date)
			{ return Within (ZTime.TimeOf(date)); }
	
		
		/// <summary>
		/// Determine if time for given clock time within time-interval
		/// </summary>
		/// <param name='clock'>
		/// clock stamp from which time is extracted (in local timezone)
		/// </param>
		public bool Within (long clock)
			{ return Within (ZTime.TimeOf(clock)); }
	
		
		/// <summary>
		/// Determine if time within time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool Within (ZTime time)
			{ return _start <= time && _end >= time; }
		

		/// <summary>
		/// Determine if time before time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool Before (ZTime time)
			{ return _start > time; }
		
		
		/// <summary>
		/// etermine if time after time-interval
		/// </summary>
		/// <param name='time'>
		/// time to consider
		/// </param>
		public bool After (ZTime time)
			{ return _end < time; }
	
		
		// Meta
		
		
		public override string ToString()
			{ return "[" + _start + ", " + _end + "]"; }

		public static implicit operator ZTimeRange (string range)
			{ return new ZTimeRange (range); }
		
		
		// Variables
		
		private ZTime	_start;
		private ZTime	_end;
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/time/ZTimeZone.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.time
{
	/// <summary>
	/// TimeZone functionality & timezone conversion.  The .NET implementation is too cumbersome and slow
	/// </summary>
	public class ZTimeZone
	{
		public readonly static ZTimeZone		GMT = new ZTimeZone (TimeZoneInfo.Utc);
		public readonly static ZTimeZone		UTC = new ZTimeZone (TimeZoneInfo.Utc);
		public readonly static ZTimeZone		Local = new ZTimeZone ("Local");
		
		public readonly static ZTimeZone		NewYork = new ZTimeZone ("America/New_York");
		public readonly static ZTimeZone		London = new ZTimeZone ("Europe/London");
		public readonly static ZTimeZone		Tokyo = new ZTimeZone ("Asia/Tokyo");
		public readonly static ZTimeZone		Auckland = new ZTimeZone ("Pacific/Auckland");

		
		/// <summary>
		/// Create time zone from .NET timezone
		/// </summary>
		/// <param name='zone'>
		/// Zone.
		/// </param>
		public ZTimeZone (TimeZoneInfo zone)
		{
			Composed = zone;
			Underlier = GetUnderlyingZone (zone);

			_generator = new ThreadLocal<ZDateTimeInfoGenerator> (() => new ZDateTimeInfoGenerator(this));
		}
		
		
		/// <summary>
		/// Create time zone from name
		/// </summary>
		/// <param name='zonename'>
		/// name of timezone
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Is thrown when an zone name is invalid
		/// </exception>
		public ZTimeZone (string zonename)
		{
			Composed = FindInternal (zonename);
			Underlier = GetUnderlyingZone (Composed);

			if (Composed == null)
				throw new ArgumentException ("unknown timezone: " + zonename);

			_generator = new ThreadLocal<ZDateTimeInfoGenerator> (() => new ZDateTimeInfoGenerator(this));
		}
		
		
		// Properties
		
		public TimeZoneInfo Composed
			{ get; private set; }

		public TimeZoneInfo Underlier
			{ get; private set; }
		
		public TimeSpan BaseUtcOffset
			{ get { return Composed.BaseUtcOffset; } }
		
		public string Id
			{ get { return Composed.Id; } }
		
		public ZDateTimeInfoGenerator Generator
			{ get { return _generator.Value; } }
		
		
		// Functions
		
		
		/// <summary>
		/// Determines whether given UTC time is in daylight savings or not (for this zone)
		/// </summary>
		/// <returns>
		/// <c>true</c> if clock is in daylight savings; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='clock'>
		/// time in UTC ms since Jan 1 1970
		/// </param>
		public bool IsDaylightSavingTime (long clock)
		{
			var Nclock = clock * 10000L + 621355968000000000L;
			return IsDaylightSavingTime (new DateTime (Nclock, DateTimeKind.Utc));			
		}

		
		/// <summary>
		/// Determines whether given UTC time is in daylight savings or not (for this zone)
		/// </summary>
		/// <returns>
		/// <c>true</c> if clock is in daylight savings; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='clock'>
		/// datetime in UTC form
		/// </param>
		public bool IsDaylightSavingTime (DateTime clock)
		{
			var rule = GetApplicableRule (clock);
			return IsDaylightSavingTime (rule, clock);
		}
		
		
		/// <summary>
		/// Gets the DST info for given UTC time
		/// </summary>
		/// <param name='clock'>
		/// UTC time in milliseconds since Jan 1, 1970
		/// </param>
		public ZDaylightSavings GetDSTInfoFor (long clock)
		{
			var Nclock = clock * 10000L + 621355968000000000L;
			return GetDSTInfoFor (new DateTime (Nclock, DateTimeKind.Utc));			
		}
		
		
		/// <summary>
		/// Gets the DST info for given UTC time
		/// </summary>
		/// <param name='clock'>
		/// UTC time
		/// </param>
		public ZDaylightSavings GetDSTInfoFor (DateTime clock)
		{
			var dstrule = GetApplicableRule (clock);
			return GetDSTInfoFor (dstrule, clock);
		}
		
		
		/// <summary>
		/// Convert the specified utc Datetime to a shifted UTC Datetime yielding Hour, Minute, etc. according to the time
		/// in this time zone
		/// </summary>
		/// <param name='utc'>
		/// UTC date/time
		/// </param>
		public DateTime Convert (DateTime utc)
		{
			if (utc.Kind != DateTimeKind.Utc)
				throw new Exception ("provided datetime must be in UTC form");
			
			var dstrule = GetApplicableRule (utc);
			if (IsDaylightSavingTime (dstrule, utc))
				return utc + BaseUtcOffset + dstrule.DaylightDelta;
			else
				return utc + BaseUtcOffset;
		}
		
		
		/// <summary>
		/// Convert the specified utc Datetime to a shifted UTC Datetime yielding Hour, Minute, etc. according to the time
		/// in this time zone
		/// </summary>
		/// <param name='clock'>
		/// UTC date/time in ms since Jan 1, 1970
		/// </param>
		public DateTime Convert (long clock)
		{
			DateTime utc = new DateTime (clock * 10000L + 621355968000000000L, DateTimeKind.Utc);
			var dstrule = GetApplicableRule (utc);
			if (IsDaylightSavingTime (dstrule, utc))
				return utc + BaseUtcOffset + dstrule.DaylightDelta;
			else
				return utc + BaseUtcOffset;
		}
		
		
		/// <summary>
		/// Find zone associated with name
		/// </summary>
		/// <param name='zonename'>
		/// name of time zone
		/// </param>
		public static ZTimeZone Find (string zonename)
		{
			return new ZTimeZone (FindInternal (zonename));
		}
		
		
		// Implementation: Zone Find


		/// <summary>
		/// Gets the underlying zone (for instance if GMT+5, would return GMT)
		/// </summary>
		/// <param name='composed'>
		/// Composed.
		/// </param>
		private static TimeZoneInfo GetUnderlyingZone (TimeZoneInfo composed)
		{
			var id = composed.DisplayName;

			var split = id.IndexOfAny(new char[] {'-','+'});
			if (split < 0)
				return composed;

            var smain = id[0] != '(' ? id.Substring(0, split) : id.Substring(1, split-1);
			return FindZone (smain);
		}

		
		/// <summary>
		/// Locate timezone by name or allow offset specification, such as "GMT+2", "GMT-5", "UTC+1"
		/// </summary>
		/// <param name='id'>
		/// Timezone name or name + offset
		/// </param>
		private static TimeZoneInfo FindInternal (string id)
		{
			int split = id.IndexOfAny(new char[] {'-','+'});
			if (split < 0)
			{
				return FindZone (id);
			}
			else
			{
				string smain = id.Substring(0, split);
				int ioffset = int.Parse(id.Substring(split+1)); 
				
				TimeZoneInfo main = FindZone (smain);
				TimeSpan offset = main.BaseUtcOffset.Add (new TimeSpan(ioffset, 0, 0));
				
				return TimeZoneInfo.CreateCustomTimeZone (id, offset, id, id);
			}
		}
		
		
		/// <summary>
		/// Finds the zone by ID.
		/// </summary>
		private static TimeZoneInfo FindZone (string zone)
		{
			if (_zones == null)
				Initialize ();
			
			TimeZoneInfo info = null;
			if (_zones.TryGetValue(zone, out info))
				return info;
			
			try
			{	
				return TimeZoneInfo.FindSystemTimeZoneById (zone);
			}
			catch
			{
				throw new Exception ("timezone " + zone + " not found, entries: " + StringUtils.ToString(_zones.Keys));
			}
		}
		
		
		/// <summary>
		/// Finds the timezone matching one of the known aliases (unfortunately this differs by platform)
		/// </summary>
		private static TimeZoneInfo FindAny (params string[] aliases)
		{
			foreach (var alias in aliases)
			{
				try
					{ return TimeZoneInfo.FindSystemTimeZoneById (alias); }
				catch
					{ }
			}
			
			throw new Exception ("could not find any time zones by alias: " + aliases[0] + ", ...");
		}
		
		
		// Meta
		
		
		public override string ToString ()
		{
			 return Composed.ToString();
		}
		
		
		// Implementation: DST

		
		private bool IsDaylightSavingTime (TimeZoneInfo.AdjustmentRule rule, DateTime time)
		{
			if (rule == null)
				return false;
					
			var Rstart = rule.DaylightTransitionStart;
			var Rend = rule.DaylightTransitionEnd;
			
			DateTime DST_start = TransitionPoint (
				Rstart, time.Year);
			DateTime DST_end = TransitionPoint (
				Rend, 
				time.Year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
			
			DST_start -= Underlier.BaseUtcOffset;
			DST_end -= (Underlier.BaseUtcOffset + rule.DaylightDelta);

			return (time >= DST_start && time < DST_end);			
		}

		
		private ZDaylightSavings GetDSTInfoFor (TimeZoneInfo.AdjustmentRule rule, DateTime time)
		{
			if (rule != null)
			{
				var Rstart = rule.DaylightTransitionStart;
				var Rend = rule.DaylightTransitionEnd;
			
				DateTime DST_start = TransitionPoint (
					Rstart, time.Year);
				DateTime DST_end = TransitionPoint (
					Rend, 
					time.Year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
			
				DST_start -= Underlier.BaseUtcOffset;
				DST_end -= (Underlier.BaseUtcOffset + rule.DaylightDelta);
				
				return new ZDaylightSavings (DST_start, DST_end, (long)rule.DaylightDelta.TotalMilliseconds);
			}
			else
			{
				return new ZDaylightSavings (DateTime.MaxValue, DateTime.MinValue, 0L);
			}
		}

		
		
		private TimeZoneInfo.AdjustmentRule GetApplicableRule (DateTime time)
		{
			var zone = Underlier;
			var Tclock = time.Ticks + zone.BaseUtcOffset.Ticks;
			
			// use local to avoid another thread changing reference during test
			var srule = _rule;
			if (srule != null && IsAppropriateRule (srule, Tclock))
				return srule;
			
			var rulelist = zone.GetAdjustmentRules();
			foreach (var rule in rulelist)
			{
				if (IsAppropriateRule (rule, Tclock))
					return _rule = rule;
			}
			
			return null;
		}

		
		private static bool IsAppropriateRule (TimeZoneInfo.AdjustmentRule rule, long Tclock)
		{
			if (rule.DateStart.Ticks > Tclock)
				return false;
			if (rule.DateEnd.Ticks >= Tclock)
				return true;
			else
				return false;
		}
		
		
		private static DateTime TransitionPoint (TimeZoneInfo.TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
				return new DateTime (year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;

			DayOfWeek first = (new DateTime (year, transition.Month, 1)).DayOfWeek;
			DayOfWeek target = transition.DayOfWeek;

			// locate first dayofweek 
			int dayadjust = (first > target) ? (7 - (int)first + 1) : ((int)target - (int)first + 1);
			// roll to the nth
			int day = dayadjust + (transition.Week - 1) * 7;
			if (day >  DateTime.DaysInMonth (year, transition.Month))
				day -= 7;
			
			return new DateTime (year, transition.Month, day) + transition.TimeOfDay.TimeOfDay;
		}
		
		
		// Initialization
		
		
		private static void Install (TimeZoneInfo zone, params string[] aliases)
		{
			foreach (string alias in aliases)
				_zones[alias] = zone;
        }
		
		
		private static void Initialize ()
		{
            lock (typeof(ZTimeZone))
            {
                if (_zones == null)
                    _zones = new Dictionary<string, TimeZoneInfo>(StringComparer.OrdinalIgnoreCase);
				
				// special hack for windows because local timezone lookup broken (waiting for mono folks)
				if (SystemUtils.IsWindows)
					Install (FindAny("America/New_York", "Eastern Standard Time"), "Local");
				else
					Install (TimeZoneInfo.Local, "Local");
				
                Install(
					FindAny("America/New_York", "Eastern Standard Time"), 
					"America/New_York", "America/New York", "EST", "GMT-5");
                Install(
					FindAny("Asia/Tokyo", "Tokyo Standard Time"), 
					"Asia/Tokyo", "JST", "GMT+9");
                Install(
					FindAny("Europe/London", "GMT Standard Time"), 
					"Europe/London", "GMT");
				Install(
					FindAny("Asia/Seoul","Korea Standard Time"), "Asia/Seoul");
				Install(
					FindAny("Asia/Bangkok", "SE Asia Standard Time"), 
					"Asia/Jakarta", "Asia/Bangkok", "Asia/Saigon");
				Install(
					FindAny("Europe/Moscow", "Russian Standard Time"), 
					"Europe/Moscow");
				Install(
					FindAny("America/Sao_Paulo", "E. South America Standard Time"), "America/Sao_Paulo");
				Install(
					FindAny("Asia/Shanghai", "China Standard Time"), 
					"Asia/Beijing", "Asia/Shanghai", "Asia/Hong_Kong");
				Install(
					FindAny("Asia/Singapore","Singapore Standard Time"), 
					"Asia/Singapore", "Asia/Brunei", "Asia/Manila", "Asia/Kuala_Lumpur");
				Install(
					FindAny("Australia/Perth","W. Australia Standard Time"), 
					"Australia/Perth");
				Install(
					FindAny("Asia/Taipei","Taipei Standard Time"), 
					"Asia/Taipei");
				Install(
					FindAny("Australia/Sydney","AUS Eastern Standard Time"), 
					"Australia/Sydney");
				Install(
					FindAny("Europe/Berlin","W. Europe Standard Time"), 
					"Europe/Frankfurt", "Europe/Berlin");
				Install(
					FindAny("Europe/Prague","Central Europe Standard Time"), 
					"Europe/Budapest", "Europe/Prague", "Europe/Belgrade");
				Install(
					FindAny("Europe/Athens","GTB Standard Time"), 
					"Europe/Athens", "Europe/Bucharest");
				Install(
					FindAny("Africa/Johannesburg","South Africa Standard Time"), 
					"Africa/Johannesburg");
				Install(
					FindAny("Europe/Istanbul","Turkey Standard Time"), 
					"Europe/Istanbul");
				Install(
					FindAny("Europe/Paris","Romance Standard Time"), 
					"Europe/Paris", "Europe/Brussels", "Europe/Copenhagen");
				Install(
					FindAny("Europe/Warsaw","Central European Standard Time"), 
					"Europe/Warsaw");
				Install(
					FindAny("Asia/Jerusalem","Israel Standard Time"), 
					"Asia/Tel_Aviv", "Asia/Jerusalem");
				Install(
					FindAny("Asia/Riyadh","Arab Standard Time"), 
					"Asia/Riyadh", "Asia/Qatar", "Asia/Kuwait", "Asia/Bahrain", "Asia/Aden");
				Install(
					FindAny("Pacific/Auckland","New Zealand Standard Time"), 
					"Pacific/Auckland");
				Install(
					FindAny("America/Buenos_Aires","Argentina Standard Time"), 
					"America/Buenos_Aires");
				Install(
					FindAny("America/Lima","SA Pacific Standard Time"), 
					"America/Bogota", "America/Lima", "America/Cayman", "America/Jamaica");
				Install(
					FindAny("America/Caracas","Venezuela Standard Time"), 
					"America/Caracas");
				Install(
					FindAny("Asia/Manila","Pacific SA Standard Time"), 
					"Asia/Santiago", "Asia/Manila");
				Install(
					FindAny("Asia/Kolkata","India Standard Time"), 
					"Asia/Mumbai","Asia/Kolkata");
				Install(
					FindAny("Asia/Almaty","Central Asia Standard Time"), 
					"Asia/Almaty");

                Install(TimeZoneInfo.Utc, "GMT", "UTC");
            }
		}

				
		// Variables
		
		static IDictionary<string,TimeZoneInfo>		_zones;
		TimeZoneInfo.AdjustmentRule 				_rule; 
		ThreadLocal<ZDateTimeInfoGenerator> 		_generator;
	}
			

}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/Any.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.utils
{	
	/// <summary>
	/// Class provides mapping between string form and many of the primitive types
	/// </summary>
	public struct Any
	{
		public Any (string v)
		{
			_sval = v; 
		}
		
		
		// Conversions
		
		public static implicit operator string(Any v)
			{ return v._sval; }
		
		public static implicit operator char(Any v)
			{ return v._sval[0]; }
		
		public static implicit operator int(Any v)
			{ return int.Parse(v._sval); }
		
		public static implicit operator double(Any v)
			{ return double.Parse(v._sval); }
		
		public static implicit operator bool(Any v)
			{ return bool.Parse(v._sval); }
		
		public static implicit operator long(Any v)
			{ return long.Parse(v._sval); }
		
		public static implicit operator ZDateTime(Any v)
			{ return new ZDateTime(v._sval, ZTimeZone.Local); }
		
		
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public int Or (int def)
		{
			string v = _sval;
			if (v != null)
				return int.Parse(v);
			else
				return def;
		}
		
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public long Or (long def)
		{
			string v = _sval;
			if (v != null)
				return long.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public double Or (double def)
		{
			string v = _sval;
			if (v != null)
				return double.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public bool Or (bool def)
		{
			string v = _sval;
			if (v != null)
				return bool.Parse(v);
			else
				return def;
		}

				
		/// <summary>
		/// Provide requested value or default (if requested value not present)
		/// </summary>
		public string Or (string def)
		{
			string v = _sval;
			if (v != null)
				return v;
			else
				return def;
		}

		// Predicates
		
		public bool IsNull
			{ get { return _sval == null; } }
		
		
		// Meta
		
		
		public override string ToString ()
		{
			return _sval;
		}
		
		
		// Variables
		
		private string		_sval;
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/ArgumentParser.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.utils
{	
	/// <summary>
	/// command-line parser (understands unix and dos conventions).  In particular:
	/// <ul>
	///		<li>/argname:value</li>
	///		<li>-argname value</li>
	///		<li>--argname=value</li>
	/// </ul>
	/// If the value is comma "," separated will assume is a list and parse accordingly.  If
	/// multiple instances of a switch occur, will aggregate into a list likewise.
	/// </summary>
	public class ArgumentParser
	{
		/// <summary>
		/// provide a help function
		/// </summary>
		public delegate void HelpDelegate ();


		/// <summary>
		/// create parser
		/// </summary>
		public ArgumentParser (string[] args, HelpDelegate help)
		{
			_pending = args;
			_help = help;

			Register ("help", false, false, "shows command help");
		}


		/// <summary>
		/// create parser
		/// </summary>
		public ArgumentParser (string[] args)
			: this (args, null)
		{
		}


		/// <summary>
		/// indicate arguments to parse up-front
		/// </summary>
		/// <param name="name">name of argument</param>
		/// <param name="arguments">indicate whether argument has a trailing value</param>
		/// <param name="mandatory">optional or mandatory</param>
		public void Register (string name, bool arguments, bool mandatory, string description)
		{
			_templates [name] = new Template (name, arguments, mandatory, description);
		}


		// accessors

		/// <summary>
		/// get argument corresponding to name
		/// </summary>
		public Argument this [string argname]
		{
			get 
			{ 
				Parse();
				Argument val = null;
				if (_switches.TryGetValue (argname, out val))
					return val;
				else
					return new Argument (argname);
			}
			set 
			{ 
				_switches[argname] = value; 
			}
		}
		
		
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public int Or (string name, int def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public double Or (string name, double def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public string Or (string name, string def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public bool Or (string name, bool def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public ZDateTime Or (string name, ZDateTime def)
		{
			if (Contains (name))
				return new ZDateTime (this[name].Value);
			else
				return def;
		}

		/// <summary>
		/// get remaining non-switch arguments
		/// </summary>
		public IList<Any> Remainder
			{ get { Parse(); return _remainder; } }
		

		/// <summary>
		/// contains arg?
		/// </summary>
		public bool Contains (string argname)
			{ Parse(); return _switches.ContainsKey (argname); }


		/// <summary>
		/// parse arguments
		/// </summary>
		public void Parse ()
		{
			if (_pending != null)
			{
				Parse(_pending);
				Check ();
				_pending = null;
			}
		}


		
		/// <summary>
		/// help
		/// </summary>
		public void Help ()
		{ 
			if (_help != null) 
				_help ();
			else
				HelpInfo ();
		}


		// Implementation


		/// <summary>
		/// parse argument stream
		/// </summary>
		private void Parse (string[] args)
		{
			// parse arguments
			for (int i = 0 ; i < args.Length ; i++)
			{
				// get next argument name (possibly null)
				Argument arg = Parse (args, ref i);

				if (arg != null)
					Add (arg);
				else
					_remainder.Add (new Any(args[i]));
			}
		}


		/// <summary>
		/// check arguments to see if they conform
		/// </summary>
		private void Check ()
		{
			foreach (Template entry in _templates.Values)
			{
				// check to see if arg
                var arg = _switches.ContainsKey(entry.Name) ? _switches[entry.Name] : null;

				if (arg == null && entry.Mandatory)
					ArgumentError ("could not find mandatory argument: " + entry.Name);
				if (arg == null && !entry.Mandatory)
					continue;
				else if (arg.Type == Argument.ValueType.None && entry.HasValue)
					ArgumentError ("argument: " + entry.Name + " does not have value");
			}

            if (_switches.ContainsKey("help"))
				{ Help(); System.Environment.Exit (1); }
		}
		

		/// <summary>
		/// an argument
		/// </summary>
		private Argument Parse (string[] args, ref int i)
		{
			string first = args[i];
			
			// unix style argument "--argname=value"
			if (first.StartsWith("--")) 
			{
				string name = StringUtils.Field (first.Substring(2), 0, '=');
				if (!_templates.ContainsKey (name))
					ArgumentError ("unknown switch: " + name);
				
				IList<Any> vallist = ParseValue (_templates[name], StringUtils.LTrimField (first.Substring(2), 1, '='));
				return new Argument (name, vallist);
			}
			
			// unix style argument "-argname value"
			else if (first.StartsWith("-"))
			{
				string name = first.Substring(1);
				if (!_templates.ContainsKey (name))
					ArgumentError ("unknown switch: " + name);
				
				IList<Any> vallist = ParseValue (_templates[name], (args.Length > (i+1)) ? args[i+1] : null);
				i += (vallist.Count == 0) ? 0 : 1;
				return new Argument (name, vallist);
			}

			else
				return null;
		}



		/// <summary>
		/// parse value, returning single value or array
		/// </summary>
		private IList<Any> ParseValue (Template arginfo, string val)
		{
			// determine if supposed to have argument
			bool hasvalue = arginfo.HasValue;

			// if not supposed to have value, return
			if (!hasvalue || val == null)
				return new Any[] {};

			// otherwise process value
			string[] vallist = val.Split (new char[] {','});
			
			IList<Any> list = new List<Any>();
			foreach (string single in vallist)
			{
				string nsingle = single.Trim ();
				if (nsingle.Length == 0)
					continue;
				else
					list.Add (new Any (nsingle));
			}

			return list;
		}



		/// <summary>
		/// add argument
		/// </summary>
		private void Add (Argument arg)
		{
            if (_switches.ContainsKey(arg.Name))
				((Argument)_switches[arg.Name]).Append (arg.ValueList);
			else
				_switches[arg.Name] = arg;
		}


		/// <summary>
		/// display error
		/// </summary>
		private void ArgumentError (string msg)
		{
			Console.WriteLine ("{0}: {1}\n", Application(), msg);
			Help();
			System.Environment.Exit (1);
		}


		/// <summary>
		/// display error
		/// </summary>
		private string Application ()
		{
			string fullname = StringUtils.Field (System.Environment.CommandLine, 0, ' ');
			string[] parts = fullname.Split (new char[] {'/','\\'});
			return StringUtils.Field (parts	[parts.Length-1], 0, '.');
		}


		/// <summary>
		/// help info
		/// </summary>
		private void HelpInfo ()
		{
			Console.WriteLine ("{0}: has the following parameters:\n", Application());
			int len = 0;
			foreach (var tmpl in _templates.Values)
			{
				len = (int)Math.Max (len, tmpl.Name.Length);
			}
	
			foreach (Template tmpl in _templates.Values)
			{
				// check to see if arg
				if (tmpl.HasValue)
					Console.WriteLine ("   -{0}\t: {1} ({2})", StringField (tmpl.Name + " <value>",len+10), tmpl.Description, tmpl.Mandatory ? "required" : "optional");
				else
					Console.WriteLine ("   -{0}\t: {1} ({2})", StringField (tmpl.Name,len+10), tmpl.Description, tmpl.Mandatory ? "required" : "optional");
			}
		}


		/// <summary>
		/// format string into field length (left aligned)
		/// </summary>
		private string StringField (string s, int len)
		{
			StringBuilder build = new StringBuilder (len);
			build.Append (s);
			for (int i = 0 ; i < (len - s.Length) ; i++) build.Append (' ');
			return build.ToString();
		}
		
		
		// Classes
		
		
		/// <summary>
		/// Parameter template
		/// </summary>
		private struct Template
		{
			public string		Name;
			public bool			HasValue;
			public bool			Mandatory;
			public string		Description;
			
			
			public Template (string name, bool hasval, bool mandatory, string description)
			{
				Name = name;
				HasValue = hasval;
				Mandatory = mandatory;
				Description = description;
			}
		}
		

		// variables

		private string[]						_pending;
		private Dictionary<string,Argument>		_switches	= new Dictionary<string, Argument>();
		private IList<Any>						_remainder	= new List<Any>();
		private Dictionary<string,Template>		_templates	= new Dictionary<string, Template>();
		private HelpDelegate					_help;
	}
	
	
	
	/// <summary>
	/// command-line argument, containing
	/// <ul>
	///		<li>switch</li>
	///		<li>argument type</li>
	///		<li>value or array of values</li>
	/// </ul>
	/// </summary>
	public class Argument
	{
		/// <summary>
		/// type of value
		/// </summary>
		public enum ValueType
			{ None, Single, List };


		internal Argument (string name)
		{
			_name = name;
			_valuelist = new List<Any> ();
		}

		internal Argument (string name, Any svalue)
		{
			_name = name;
			_valuelist = new List<Any> ();
			_valuelist.Add (svalue);
		}

		internal Argument (string name, IList<Any> valuelist)
		{
			_name = name;
			_valuelist = new List<Any> (valuelist);
		}

		internal Argument (string name, object val)
		{
			_name = name;
			_valuelist = new List<Any> ();
			Append (val);
		}



		/// <summary>
		/// type of value
		/// </summary>
		public ValueType Type
			{ get { return _valuelist.Count > 1 ? ValueType.List :  ValueType.Single; } }

		/// <summary>
		/// name
		/// </summary>
		public string Name
			{ get { return _name; } }

		/// <summary>
		/// value (only valid for single values)
		/// </summary>
		public Any Value
			{ get { return (Any)_valuelist[0]; } }

		/// <summary>
		/// value list (only valid for single values)
		/// </summary>
		public IList<Any> ValueList
			{ get { return _valuelist; } }


		/// <summary>
		/// append another value
		/// </summary>
		public void Append (Any v)
			{ _valuelist.Add (v); }

		/// <summary>
		/// append another value list
		/// </summary>
		public void Append (ICollection<Any> list)
		{
			foreach (Any v in list) _valuelist.Add (v);
		}

		/// <summary>
		/// append another value list
		/// </summary>
		public void Append (object v)
		{
			if (v.GetType().IsArray)
				Append ((ICollection<Any>)v);
			else
				Append ((Any)v);
		}


		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public int Or (int def)
		{
			if (_valuelist.Count > 0)
				return (int)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public double Or (double def)
		{
			if (_valuelist.Count > 0)
				return (double)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public string Or (string def)
		{
			if (_valuelist.Count > 0)
				return (string)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public bool Or (bool def)
		{
			if (_valuelist.Count > 0)
				return (bool)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public ZDateTime Or (ZDateTime def)
		{
			if (_valuelist.Count > 0)
				return new ZDateTime ((string)Value);
			else
				return def;
		}

		
		// Comversions
		
		
		public static implicit operator string (Argument arg)
			{ return (string)arg.Value; }
		
		public static implicit operator double (Argument arg)
			{ return (double)arg.Value; }
		
		public static implicit operator bool (Argument arg)
			{ return (bool)arg.Value; }
		
		public static implicit operator int (Argument arg)
			{ return (int)arg.Value; }

		public override string ToString ()
			{ return (string)Value; }

		// variables

		string			_name;
		IList<Any>		_valuelist;
	}
	
	
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/ArrayUtils.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.utils
{
	/// <summary>
	/// Misc Array utils.
	/// </summary>
	public static class ArrayUtils
	{
		/// <summary>
		/// Fill the vector with given value
		/// </summary>
		/// <param name='vec'>
		/// Vector
		/// </param>
		/// <param name='val'>
		/// Value.
		/// </param>
		public static void Fill<T> (T[] vec, T val)
		{
			for (int i = 0 ; i < vec.Length ; i++)
				vec[i] = val;
		}


		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		public static int Find (double[] xvec, double x, int Istart = 0, int Iend = -1)
		{
			if (Iend < 0)
				Iend = xvec.Length - 1;
			
			var Xend = xvec[Iend];
			var Xstart = xvec[Istart];
			
			var m = (double)(Iend-Istart+1) / (Xend - Xstart);
			var Iguess = Istart + (int)(m * (x - Xstart));
			return Find (xvec, x, Istart, Iend, Iguess);
		}

		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		public static int Find (Vector<double> xvec, double x, int Istart = 0, int Iend = -1)
		{
			if (Iend < 0)
				Iend = xvec.Count - 1;
			
			var Xend = xvec[Iend];
			var Xstart = xvec[Istart];
			
			var m = (double)(Iend-Istart+1) / (Xend - Xstart);
			var Iguess = Istart + (int)(m * (x - Xstart));
			return Find (xvec, x, Istart, Iend, Iguess);
		}
		
		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		/// <param name='Iguess'>
		/// initial guess.
		/// </param>
		/// <returns>
		/// index of x with max(x) <= target x or -1 if cannot find
		/// </returns>
		public static int Find (double[] xvec, double x, int Istart, int Iend, int Iguess)
		{
			int Ilastguess = -1;
			
			while (Istart <= Iend)
			{
				if (Iguess < Istart)
					return Ilastguess;
				if (Iguess > Iend)
					return Iend;
				
				double Xguess = xvec[Iguess];
				
				if (Xguess == x)
					return Iguess;
	
				if (Xguess > x)
				{
					Iend = Iguess-1;
					if (Iend < Istart)
						return Ilastguess;
					if (Iend == Istart)
						return Iend;
				} 
				else
				{ 
					Istart = Iguess+1; 
					Ilastguess = Iguess; 
				}
				
				if (Istart >= Iend)
					return Ilastguess;
							
				double Xend = xvec[Iend];
				double Xstart = xvec[Istart];
				
				double m = (double)(Iend-Istart+1) / (double)(Xend - Xstart);
				Iguess = Istart + (int)((x - Xstart) * m);
			}
			
			if (Ilastguess != -1)
				return Ilastguess;
			if (xvec[Istart] <= x)
				return Istart;
			else
				return -1;
		}

		
		/// <summary>
		/// Find index of max(x) <= target x
		/// </summary>
		/// <param name='xvec'>
		/// vector of x values.
		/// </param>
		/// <param name='x'>
		/// target x.
		/// </param>
		/// <param name='Istart'>
		/// start index to start search from.
		/// </param>
		/// <param name='Iend'>
		/// end index for search (inclusive).
		/// </param>
		/// <param name='Iguess'>
		/// initial guess.
		/// </param>
		/// <returns>
		/// index of x with max(x) <= target x or -1 if cannot find
		/// </returns>
		public static int Find (Vector<double> xvec, double x, int Istart, int Iend, int Iguess)
		{
			int Ilastguess = -1;
			
			while (Istart <= Iend)
			{
				if (Iguess < Istart)
					return Ilastguess;
				if (Iguess > Iend)
					return Iend;
				
				double Xguess = xvec[Iguess];
				
				if (Xguess == x)
					return Iguess;
	
				if (Xguess > x)
				{
					Iend = Iguess-1;
					if (Iend < Istart)
						return Ilastguess;
					if (Iend == Istart)
						return Iend;
				} 
				else
				{ 
					Istart = Iguess+1; 
					Ilastguess = Iguess; 
				}
				
				if (Istart >= Iend)
					return Ilastguess;
							
				double Xend = xvec[Iend];
				double Xstart = xvec[Istart];
				
				double m = (double)(Iend-Istart+1) / (double)(Xend - Xstart);
				Iguess = Istart + (int)((x - Xstart) * m);
			}
			
			if (Ilastguess != -1)
				return Ilastguess;
			if (xvec[Istart] <= x)
				return Istart;
			else
				return -1;
		}
			
		
		/// <summary>
		/// Adds the amount to the appropriate 1D cell with aliasing
		/// </summary>
		/// <param name='vector'>
		/// Vector
		/// </param>
		/// <param name='domain'>
		/// Index of vector domain
		/// </param>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='amount'>
		/// Amount to be added at vector[X]
		/// </param>
		public static void AddAliased (Vector<double> vector, Vector<double> domain, double x, double amount)
		{
			var i = ArrayUtils.Find (domain, x);
			
			// lower boundary
			if (i <= 0)
				vector[0] += amount;
			
			// upper boundary
			else if (i >= (domain.Count-1))
				vector[domain.Count-1] += amount; 
			
			// in the middle, alias
			else
			{
				var width = domain[i+1] - domain[i];
				var dx = x - domain[i];
				var p = dx / width;
				
				vector[i] += (1-p) * amount;
				vector[i+1] += p * amount;
			}
		}
		

		/// <summary>
		/// Adds the amount to the appropriate 1D cell with aliasing
		/// </summary>
		/// <param name='vector'>
		/// Vector
		/// </param>
		/// <param name='domain'>
		/// Index of vector domain
		/// </param>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='amount'>
		/// Amount to be added at vector[X]
		/// </param>
		public static void AddAliased (
			Vector<double> vector, 
			double Xstart,
			double Xend,
			double x, 
			double amount)
		{
			var cells = vector.Count;
			var dx = (Xend - Xstart) / (double)cells;
			var i = (int)((x - Xstart) / dx);

			// lower boundary
			if (i <= 0)
				vector[0] += amount;

			// upper boundary
			else if (i >= (cells-1))
				vector[cells-1] += amount; 

			// in the middle, alias
			else
			{
				var gap = x - (Xstart + i * dx);
				var p = gap / dx;

				vector[i] += (1-p) * amount;
				vector[i+1] += p * amount;
			}
		}

		
		/// <summary>
		/// Adds the amount to the appropriate 2D cell with aliasing
		/// </summary>
		/// <param name='matrix'>
		/// Matrix to add into
		/// </param>
		/// <param name='rdomain'>
		/// Domain of rows
		/// </param>
		/// <param name='cdomain'>
		/// Domain of cols
		/// </param>
		/// <param name='rx'>
		/// Row ordinate
		/// </param>
		/// <param name='cx'>
		/// Column ordinate
		/// </param>
		/// <param name='amount'>
		/// Amount to be added
		/// </param>
		public static void AddAliased (
			Matrix<double> matrix, 
			Vector<double> rdomain, Vector<double> cdomain, 
			double rx, double cx, 
			double amount)
		{
			var ri = ArrayUtils.Find (rdomain, rx);
			var ci = ArrayUtils.Find (cdomain, cx);
			
			var r0 = 0;
			var r1 = 0;
			var c0 = 0;
			var c1 = 0;
			var rwidth = 0.0;
			var cwidth = 0.0;
			
			if (ri <= 0)
			{ 
				r0 = 0; r1 = 0; 
				rx = rdomain[0];
				rwidth = 1.0;
			}
			else if (ri >= (rdomain.Count-1))
			{ 
				r0 = r1 = rdomain.Count-1; 
				rx = rdomain[rdomain.Count-1];
				rwidth = 1.0;
			} 			
			else
			{ 
				r0 = ri; r1 = ri+1;
				rwidth = rdomain[r1] - rdomain[r0];
			}

			if (ci <= 0)
			{ 
				c0 = 0; c1 = 0;
				cx = cdomain[0];
				cwidth = 1.0;
			}
			else if (ci >= (cdomain.Count-1))
			{ 
				c0 = c1 = cdomain.Count-1; 
				cx = cdomain[cdomain.Count-1];
				cwidth = 1.0;
			} 			
			else
			{ 
				c0 = ci; c1 = ci+1; 
				cwidth = cdomain[c1] - cdomain[c0];				
			}
			
			// perform aliasing
			var rp = (rx - rdomain[r0]) / rwidth;
			var cp = (cx - cdomain[c0]) / cwidth;
			
			matrix[r0,c0] += (1-rp)*(1-cp) * amount;
			matrix[r0,c1] += (1-rp)*(cp) * amount;
			matrix[r1,c0] += (rp)*(1-cp) * amount;
			matrix[r1,c1] += (rp)*(cp) * amount;
		}
		
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/AtomicUtils.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.utils
{
	public class AtomicUtils
	{
		/// <summary>
		/// Determines if the ith bit is set atomically
		/// </summary>
		/// <returns><c>true</c> if is bit set; otherwise, <c>false</c>.</returns>
		/// <param name="bits">Bits.</param>
		/// <param name="ith">Ith.</param>
		public static bool GetBit (ref int bits, int ith)
		{
			var mask = 1 << ith;
			if ((Interlocked.Add (ref bits, 0) & mask) == mask)
				return true;
			else
				return false;
		}


		/// <summary>
		/// Sets the ith bit atomically
		/// </summary>
		/// <param name="bits">Bits.</param>
		/// <param name="ith">Ith.</param>
		/// <param name="val">If set to <c>true</c> value.</param>
		public static void SetBit (ref int bits, int ith, bool val = true)
		{
			var mask = 1 << ith;
			if (val)
			{
				while (true)
				{
					var prior = bits;
					var next = prior | mask;
					if (Interlocked.CompareExchange (ref bits, next, prior) == prior)
						return;
				}
			}
			else
			{
				while (true)
				{
					var prior = bits;
					var next = prior & ~mask;
					if (Interlocked.CompareExchange (ref bits, next, prior) == prior)
						return;
				}
			}
		}



		/// <summary>
		/// Sets a value atomically
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="value">Value to set to</param>
		public static void Set (ref int target, int value)
		{
			while (true)
			{
				var prior = target;
				if (Interlocked.CompareExchange (ref target, value, prior) == prior)
					return;
			}
		}

	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/CmpUtils.cs
// -------------------------------------------
﻿// 
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



namespace bridge.common.utils
{	
	/// <summary>
	/// Comparison utilities
	/// </summary>
	public class CmpUtils
	{
		// Min & Max functions
		
		
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static double Min (double a, double b, double c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
		
	
		/// <summary>
		/// Minimum the specified a, b, c and d.
		/// </summary>
		public static double Min (double a, double b, double c, double d)
		{
			if (a < b)
			{
				if (c > b || a < c)
					return (d < a) ? d : a;
				else
					return (d < c) ? d : c;
			}
			else
			{
				if (c > a || c > b)
					return (d < b) ? d : b;
				else
					return (d < c) ? d : c;
			}
		}

		
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (params double[] v)
		{
			double min = double.MaxValue;
			for (int i = 0 ; i < v.Length ; i++)
				min = Math.Min (min, v[i]);
			
			return min;
		}
		
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (Vector<double> v)
		{
			double min = double.MaxValue;
			for (int i = 0 ; i < v.Count ; i++)
				min = Math.Min (min, v[i]);

			return min;
		}

		/// <summary>
		/// Distance the specified a and b.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static Vector<double> Distance(Vector<double> a, Vector<double> b)
		{
			if (a.Count != b.Count)
				throw new Exception ("Can not compute distance with different size of vectos");

			IndexedVector retValue = new IndexedVector (b.Count);
			for (int i = 0; i < a.Count; ++i)
				retValue[i] =  a [i] - b [i];
			return retValue;
		}

	
		/// <summary>
		/// min of a vector of doubles
		/// </summary>
		public static double Min (double[] v, int Istart, int Iend)
		{
			double min = double.MaxValue;
			for (int i = Istart ; i <= Iend ; i++)
				min = Math.Min (min, v[i]);
			
			return min;
		}

		
	
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static long Min (long a, long b, long c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
	
	
		/// <summary>
		/// Minimum the specified a, b and c.
		/// </summary>
		public static int Min (int a, int b, int c)
		{
			if (a < b)
				return (c < a) ? c : a;
			else
				return (c < b) ? c : b;
		}
		
	
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(params double[] v)
		{
			double max = -double.MaxValue;
			for (int i = 0 ; i < v.Length ; i++)
				max = Math.Max (max, v[i]);
			
			return max;
		}
		
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(Vector<double> v)
		{
			double max = -double.MaxValue;
			for (int i = 0 ; i < v.Count ; i++)
				max = Math.Max (max, v[i]);

			return max;
		}

	
		/// <summary>
		/// max of a vector of doubles
		/// </summary>
		public static double Max(double[] v, int Istart, int Iend)
		{
			double max = -double.MaxValue;
			for (int i = Istart ; i <= Iend ; i++)
				max = Math.Max (max, v[i]);
			
			return max;
		}
		
		
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		/// <param name='a'>
		public static double Max (double a, double b, double c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
	
	
		/// <summary>
		/// Max the specified a, b, c and d.
		/// </summary>
		public static double Max (double a, double b, double c, double d)
		{
			if (a > b)
			{
				if (c < b || a > c)
					return (d > a) ? d : a;
				else
					return (d < c) ? d : c;
			}
			else
			{
				if (c < a || c < b)
					return (d > b) ? d : b;
				else
					return (d > c) ? d : c;
			}
		}
	
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		/// <param name='a'>
		public static long Max (long a, long b, long c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
	
	
		/// <summary>
		/// Max the specified a, b and c.
		/// </summary>
		public static int Max (int a, int b, int c)
		{
			if (a > b)
				return (c > a) ? c : a;
			else
				return (c > b) ? c : b;
		}
		
		
		// Constraints
			
			
		/// <summary>
		/// 	Determine an size lower than amount: min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Lower (double amount, double min, double incr)
		{
			if (Math.Abs(amount) <= min)
				return 0;
		
			double absamount = Math.Abs (amount);
			
			double n = (int)((absamount - min) / incr);
			double namount = min + n * incr;
			
			if (namount < absamount)
				return amount < 0 ?  -namount : namount;
			if (n > 0)
				return amount < 0 ? -(min + (n-1) * incr) : (min + (n-1) * incr);
			else
				return 0;
		}
		
	
		/// <summary>
		/// 	Determine an size higher than amount: min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Higher (double amount, double min, double incr)
		{
			double absamount = Math.Abs (amount);
			
			double n = (int)((absamount - min) / incr);
			double namount = min + (n+1) * incr;
			
			if (namount >= min)
				return namount;
			else
				return 0;
		}
	
		
		/// <summary>
		/// 	Determine nearest amount to min + n * incr
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		/// <param name='threshold'>
		/// 	rounding threshold as percentage of sizeincr before jumping to next incr.
		/// </param>
		public static double Nearest (double amount, double min, double incr, double threshold)
		{
			return CmpUtils.Nearest (amount, min, incr, double.MaxValue, threshold);
		}
		
	
		/// <summary>
		/// 	Determine nearest amount to min + n * incr.
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	Minimum value.
		/// </param>
		/// <param name='incr'>
		/// 	Increment.
		/// </param>
		public static double Nearest (double amount, double min, double incr)
		{
			return CmpUtils.Nearest (amount, min, incr, double.MaxValue, 0.5);
		}
		
		
	
		/// <summary>
		/// 	Determine nearest amount as a magnitude to min + n * incr, signed result
		/// </summary>
		/// <param name='amount'>
		/// 	Amount.
		/// </param>
		/// <param name='min'>
		/// 	minimum value (as a positive #).
		/// </param>
		/// <param name='incr'>
		/// 	increment (as a positive #).
		/// </param>
		/// <param name='max'>
		/// 	maximum value (as a positive #).
		/// </param>
		/// <param name='threshold'>
		/// 	rounding threshold as percentage of sizeincr before jumping to next incr.
		/// </param>
		public static double Nearest (double amount, double min, double incr, double max, double threshold)
		{
			if (incr == 0.0)
				return amount;
				
			if (Math.Abs(amount) <= (min * threshold))
				return 0;
		
			double namount = Math.Abs (amount) - min;
			if (namount <= 0)
				return amount < 0 ? -min : min;
			
			double n = (int)( (namount + incr *(1-threshold)) / incr);
			double abs = Math.Min (min + n * incr, max);
			
			return amount < 0 ? -abs : abs; 
		}
	
	
	
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static double Constrain (double x, double min, double max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}
	
		
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static int Constrain (int x, int min, int max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}

		
		/// <summary>
		/// 	Constrain value to be within min/max range.  If exceeds, set to min or max
		/// </summary>
		/// <param name='x'>
		/// 	X.
		/// </param>
		/// <param name='min'>
		/// 	Minimum.
		/// </param>
		/// <param name='max'>
		/// 	Maximum.
		/// </param>
		public static long Constrain (long x, long min, long max)
		{
			if (x < min)
				return min;
			if (x > max)
				return max;
			else
				return x;
		}
		
		
		// Comparisons
		
	
		/// <summary>
		/// Determine whether values are equivalent (don't differ by more than epsilon)
		/// </summary>
		public static bool EQ (double a, double b, double epsilon = 1e-14)
		{
			return Math.Abs (a - b) <= epsilon;
		}


		/// <summary>
		/// Determines if is zero (abs(a) < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsZero (double a, double epsilon = 1e-14)
		{
			return Math.Abs (a) <= epsilon;
		}
		
		/// <summary>
		/// Determines if is less-than or equal to zero (a < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsLEZero (double a, double epsilon = 1e-14)
		{
			return a < epsilon;
		}
		
		/// <summary>
		/// Determines if is greater-than or equal to zero (a < the specified a epsilon).
		/// </summary>
		/// <returns><c>true</c> if is zero the specified a epsilon; otherwise, <c>false</c>.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool IsGEZero (double a, double epsilon = 1e-14)
		{
			return a > -epsilon;
		}


		/// <summary>
		/// Not Zero if Abs(a) > specified epsilon
		/// </summary>
		/// <returns><c>true</c>, if zero was noted, <c>false</c> otherwise.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="epsilon">Epsilon.</param>
		public static bool NotZero (double a, double epsilon = 1e-14)
		{
			return Math.Abs (a) >= epsilon;
		}
	
		/// <summary>
		/// Determine relative value (-1, 0, 1) for LT, EQ, GT
		/// </summary>
		public static int Compare (double a, double b, double epsilon = 1e-14)
		{
			if (Math.Abs (a - b) <= epsilon)
				return 0;
			else
				return (a < b) ? -1 : 1;
		}
		
			
	
		/// <summary>
		/// Determine whether a > b, by more than epsilon
		/// </summary>
		public static bool GT (double a, double b, double epsilon = 1e-14)
		{
			return (a - b) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a < b, by more than epsilon
		/// </summary>
		public static bool LT (double a, double b, double epsilon = 1e-14)
		{
			return (b - a) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a >= b, by more than epsilon
		/// </summary>
		public static bool GE (double a, double b, double epsilon = 1e-14)
		{
			return EQ (a,b, epsilon) ? true : (a - b) > epsilon;
		}
		
	
		/// <summary>
		/// Determine whether a <= b, by more than epsilon
		/// </summary>
		public static bool LE (double a, double b, double epsilon = 1e-14)
		{
			return EQ (a,b, epsilon) ? true : (b - a) > epsilon;
		}
			
	}
}
// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/FileUtils.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.io
{
	/// <summary>
	/// Various file utilities
	/// </summary>
	public class FileUtils
	{
		// Constants

		public static readonly uint		Mode_rwx = Convert.ToUInt32 ("0777", 8);
		public static readonly uint		Mode_rw = Convert.ToUInt32 ("0666", 8);

		public readonly static int		Recursive 	= 1;
	    public readonly static int		Single		= 0;
	    

		/// <summary>
		/// Length of the file
		/// </summary>
		/// <param name="path">Path.</param>
		public static long LengthOf (string path)
		{
			return (new FileInfo(path)).Length;
		}


		/// <summary>
		/// Return directory component of file path.
		/// </summary>
		/// <returns>directory component.</returns>
		/// <param name="path">Path.</param>
	    public static string DirOf (string path)
	    {
			return Path.GetDirectoryName (path);
	    }

			    
		/// <summary>
		/// Return file component of file path
		/// </summary>
		/// <returns>
		/// The of.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name="remove_extension">
		/// Indicates whether file extension should be removed
		/// </param>
	    public static string FileOf (string path, bool remove_extension = false)
	    {
			var filename = Path.GetFileName (path);
			if (remove_extension)
				return StringUtils.Field (filename, 0, '.');
			else
				return filename;
	    }
		

		/// <summary>
		/// Find file in given directory path matching regular expression
		/// </summary>
		/// <returns>The in directory.</returns>
		/// <param name="dir">Dir.</param>
		/// <param name="matching">Matching.</param>
		public static List<string> FilesInDirectory (string dir, string matching = null)
		{
			var files = new List<string> ();
			var rex = new Regex (matching ?? ".+");

			foreach (var filename in Directory.GetFiles (dir))
			{
				if (rex.IsMatch (filename))
					files.Add (filename);
			}

			return files;
		}


		/// <summary>
		/// Find files with extension.
		/// </summary>
		/// <returns>The with extension.</returns>
		/// <param name="dir">Dir.</param>
		/// <param name="extension">Extension.</param>
		public static List<string> FilesWithExtension (string dir, string extension)
		{
			var files = new List<string> ();

			foreach (var filename in Directory.GetFiles (dir))
			{
				if (filename.EndsWith(extension))
					files.Add (filename);
			}

			return files;
		}



		/// <summary>
		/// Provide N levels of the tail of the path
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='n'>
		/// N.
		/// </param>
		/// <param name='separator'>
		/// Separator.
		/// </param>
		public static string TailOf (string path, int n, char separator = '/')
		{
			int Istart = path.Length;
			while (n-- > 0 && Istart > 0)
				Istart = path.LastIndexOf (separator, Istart-1);

			if (Istart < 0)
				return path;
			else
				return path.Substring (Istart+1);
		}

	    
	    /**
	     * Creates a temporary filename guaranteed to be unique
	     *
	     * @param prefix	tmp file prefix
	     * @return			unique temporary file name
	     */
	    public static string TmpFile (String prefix)
	    {
	    	String tmpdir = Path.DirectorySeparatorChar + "tmp";
	    	long timestamp = SystemUtils.Ticks;
	    	
	    	return tmpdir + Path.DirectorySeparatorChar + prefix + "-" + timestamp;
	    }
	    
		
		/// <summary>
		/// Gets the home directory
		/// </summary>
		public static string HomeDir
		{
			get 
			{
				var platform = Environment.OSVersion.Platform;
				return (platform == PlatformID.Unix || platform == PlatformID.MacOSX) ? 
					Environment.GetEnvironmentVariable("HOME") : 
    				Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
			}
		}
		
		
		/// <summary>
		/// Determines whether the given directory can be written to (i.e. create a new file)
		/// </summary>
		/// <param name='dir'>
		/// directory path
		/// </param>
		public static bool IsDirectoryWritable (string dir)
		{
			var info = new DirectoryInfo (dir);
			if (!info.Exists)
				return false;
			
			try
			{
				var testfile = dir + Path.DirectorySeparatorChar + ".dummy";
				WriteFile (testfile, "test");
				DeleteFile (testfile);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}


		/// <summary>
		/// Chmod the specified path and mask.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="mask">Mask.</param>
		public static void Chmod (string path, uint mask)
		{
			try
			{
				InitializePosix ();
				if (_Mchmod != null)
					_Mchmod.Invoke (null, new object[] { path, mask });
			}
			catch (Exception e)
			{
				_log.Warn ("failed to chmod file: " + path);
			}
		}
	
		
		/// <summary>
		/// Determines whether the file is likely on a local filesystem (tests latency
		/// </summary>
		/// <param name='filename'>
		/// ilenane
		/// </param>
		public static bool IsLocalFile (string filename)
		{
			FileInfo info = new FileInfo (filename);
			var dir = info.Directory.FullName;
			
			lock (_local)
			{
				var local = false;
				if (_local.TryGetValue(dir, out local))
					return local;
			}
			
			var timing = TimeFileAccess (filename);
			
			lock (_local)
			{
				if (timing.Milliseconds > 50)
					return _local[dir] = false;
				else
					return _local[dir] = true;
			}
		}
		
	
	    /**
	     * Convert ~ to home dir
	     *
	     * @param dir		directory with home alias (possibly)
	     * @return			expanded director path
	     */
	    public static string NormalizeDir (String dir)
	    {
	    	if (dir.StartsWith("~/"))
	    		return HomeDir + dir.Substring(1);
	    	else
	    		return dir;
	    }

		
        /// <summary>
        /// Get the nth ancestor of the given directory
        /// </summary>
        /// <param name="adir"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static string DirAncestorOf(DirectoryInfo adir, int up)
        {
            for (int i = 0; i < up; i++)
                adir = adir.Parent;

            return adir.FullName;
        }

		
	    /// <summary>
	    /// Copy file
	    /// </summary>
	    /// <param name='file'>
	    /// File.
	    /// </param>
	    /// <param name='basefile'>
	    /// Basefile.
	    /// </param>
	    public static void Copy (string srcfile, string dstfile)
	    {
	    	Stream fin = CreateInputStream (srcfile);
	    	Stream fout = CreateOutputStream (dstfile);
	    	
    		IOUtils.Copy (fin, fout);
    		fin.Close();
    		fout.Close ();
	    }


		/// <summary>
		/// Determine # of lines in file (that contain non-whitespace)
		/// </summary>
		/// <returns>The # of lines in file.</returns>
		/// <param name="file">Filename.</param>
		/// <param name="nonwhitespace">If true, only counts lines that contain text.</param>
		public static int LinesInFile (string file, bool nonwhitespace = true)
		{
			var fin = CreateInputStream (file);
			var buffer = new byte [8192];

			int nrows = 0;
			bool solidline = false;

			while (true) {
				// read into buffer
				int n = fin.Read (buffer, 0, 8192);
				// error, reached EOS prematurely
				if (n <= 0) {
					fin.Close ();
					return solidline ? nrows + 1 : nrows;
				}

				// scan for \n
				for (int i = 0; i < n; i++) {
					switch (buffer [i]) {
					case 10: // LN
						if (solidline || !nonwhitespace) nrows++;
						solidline = false;
						break;
					case 0:
					case 9:  // tab
					case 11: // VT
					case 12: // FF
					case 13: // CR
					case 32: // space
						break;
					default:
						solidline = true;
						break;
					}
				}
			}
		}

	    
		/// <summary>
		/// Decompress the specified file and basefile.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		/// <param name='basefile'>
		/// Basefile.
		/// </param>
	    public static void Decompress (string file, string basefile = null)
	    {
			var defile_dst = (basefile != null) ? basefile : StringUtils.RTrimField (file, 1, '.');
			var defile_tmp = DirOf (defile_dst) + Path.DirectorySeparatorChar + "." + FileOf (defile_dst);

	    	Stream fin = CreateInputStream (file);
	    	Stream fout = CreateOutputStream (defile_tmp);
	    	
	    	try
	    	{
	    		IOUtils.Copy (fin, fout);
	    		fin.Close();
	    		fout.Close ();
	    	}
	    	catch
	    	{
	    		FileUtils.DeleteFile (defile_tmp);
	    		throw;
	    	}

			if (File.Exists (defile_dst))
				File.Delete (defile_dst);

			File.Move (defile_tmp, defile_dst);
	    }
		
	    
		/// <summary>
		/// Decompress the specified file and basefile, with a file lock.  We poll the file
		/// being decompressed to see that progress is occurring, otherwise delete the
		/// decompressed file and redo.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		/// <param name='basefile'>
		/// Basefile.
		/// </param>
		/// <param name='Tpolling'>
		/// Polling period for progress in milliseconds
		/// </param>
	    public static string DecompressWithLock (string file, string basefile = null, int Tpolling = 5000)
	    {
			var defile_dst = (basefile != null) ? basefile : StringUtils.RTrimField (file, 1, '.');
			var defile_tmp = DirOf (defile_dst) + Path.DirectorySeparatorChar + "." + FileOf (defile_dst);
	    	
	    	if (!File.Exists (defile_tmp))
			{
				Decompress (file, defile_dst);
				return defile_dst;
			}

			var info = new FileInfo (defile_tmp);
			var psize = info.Length;

			_log.Info ("decompress: waiting for another process to finish decompressing: " + file);
			while (File.Exists (defile_tmp))
			{
				Thread.Sleep (Tpolling);
				var nsize = info.Length;

				if (nsize == psize)
				{
					_log.Info ("decompress: other process dead, will redo decompression: " + file);
					File.Delete (defile_tmp);
					Decompress (file, defile_dst);
					return defile_dst;
				}
			}

			return defile_dst;
	    }
	
	        
	    
	    /**
	     * Compress file
	     *
	     * @param file			filename to be compressed
	     */
	    public static void Compress (string basefile)
	    {
	    	Stream fin = CreateInputStream (basefile);
	    	Stream fout = CreateOutputStream (basefile + ".gz");
	    	
	    	try
	    	{
	    		IOUtils.Copy (fin, fout);
	    		fin.Close();
	    		fout.Close ();
	    	}
	    	catch
	    	{
	    		FileUtils.DeleteFile (basefile + ".gz");
	    		throw;
	    	}
	    }
	    
	    
	    /**
	     * Get last modified timestamp for a given filename
	     * 
	     * @param filename		file path
	     * @return				date stamp of last modification
	     */
	    public static DateTime GetLastModified (String filename)
	    {
	    	FileInfo file = new FileInfo (filename);
	    	return file.LastWriteTime;
	    }
	    
	    
	    /**
	     * Create output stream, possibly compressed if compression extension used
	     *
	     * @param filename		filename
	     * @return				output stream
	     */
		public static Stream CreateOutputStream (string filename, int compresslevel = 5, int buffersize = 8*4096)
	    {
	    	filename = NormalizeDir (filename);
	
	    	var fout = new BufferedWriteStream (
				new FileStream (filename, FileMode.Create),
				buffersize);
	    	
	    	if (filename.EndsWith (".gz"))
	    		return new GZipStream(fout, CompressionMode.Compress);
	    	else
	    		return fout;
	    }
	    
	    
	    
	    /**
	     * Create input stream, possibly compressed if compression extension used
	     *
	     * @param filename		filename
	     * @return				input stream
	     */
	    public static Stream CreateInputStream (string filename, int buffersize = 8*4096)
	    {
	    	filename = NormalizeDir (filename);
	    	
	    	var fin = new FileStream (filename, FileMode.Open, FileAccess.Read, FileShare.Read, 8*8192, FileOptions.SequentialScan);
	    	
	    	if (filename.EndsWith (".gz"))
	    		return new BufferedReadStream (new GZipStream(fin, CompressionMode.Decompress));
	    	else
	    		return fin;
	    }
	    
		
		/**
		 * Read file to completion, returning as buffer
		 * 
		 * @param filename		path to filename
		 * @return				buffer containing file
		 * @throws 			IOException
		 */
		public static Blob ReadFile (string filename)
		{
	    	filename = NormalizeDir (filename);
			Blob buffer = new Blob();
			var stream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			
			while (true)
			{
				// acquire buffer for read
				Blob.Region region = buffer.Acquire (4096, false);
	
				int n = stream.Read (region.Bytes, region.Offset, region.Span);
				if (n <= 0)
					return buffer;
				else
					region.Used += n;
			}
		}
		
		
		/**
		 * Write content to file
		 * 
		 * @param filename		name of file
		 * @param content		content to be written
		 */
		public static void WriteFile (string filename, string content)
		{
	    	filename = NormalizeDir (filename);
			var stream = new BufferedWriteStream (new FileStream (filename,FileMode.Create));
		    
		    IOUtils.Write (stream, content);
		    stream.Close ();
		}
		
	
		/// <summary>
		/// Deletes the directory recursively.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void DeleteDirectory (string path)
		{
			path = NormalizeDir (path);
			string[] files = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs)
			{
				DeleteDirectory (dir);
			}

			Directory.Delete(path, false);
		}


		/// <summary>
		/// Deletes the directory recursively.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void DeleteFile (string path)
		{
			path = NormalizeDir (path);

			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
	
			
		
		/**
		 * move file or dir
		 */
		public static void Move (string frompath, string topath)
		{
			File.Move (frompath, topath);			
		}
	
		
		
		/**
		 * Create directory and any intervening missing directories in path
		 * 
		 * @param path		directory path
		 */
		public static void MkDirs (string path)
		{
			path = NormalizeDir(path);
	        Directory.CreateDirectory (path);
			Chmod (path, Mode_rwx);
		}
		
		
		
		/**
		 * Determine if given file/path exists
		 * 
		 * @param path		path/file to check
		 */
		public static bool Exists (string path)
		{
		    FileInfo file = new FileInfo (path);
		    return file.Exists;
		}
		
		
		/// <summary>
		/// Find the nth ancestor of given directory
		/// </summary>
		/// <param name='dir'>
		/// Directory path
		/// </param>
		/// <param name='nth'>
		/// Nth.
		/// </param>
		public static string AncestorOf (string dir, int nth)
		{
			DirectoryInfo idir = new DirectoryInfo (dir);
			for (int i = 0 ; i < nth ; i++)
				idir = idir.Parent;
			
			return idir.FullName;
		}
		
		
		/// <summary>
		/// Finds the given directory by name in descendent paths from the base path
		/// </summary>
		/// <returns>
		/// A sequence of directories matching name
		/// </returns>
		/// <param name='basedir'>
		/// Base directory
		/// </param>
		/// <param name='dirname'>
		/// Directory name
		/// </param>
		public static IEnumerable<string> FindDir (string basedir, string dirname, int maxdepth = int.MaxValue)
		{
			foreach (var subdir in SubDirsOf (basedir))
			{
				var name = subdir.Name;
				var path = subdir.FullName;
				if (name != dirname)
				{
					foreach (var result in FindDir (path, dirname, maxdepth-1)) 
						yield return result;
				} else
					yield return path;
			}
		}

		
		#region Implementation


        /// <summary>
        /// Provide an enumeration of subdirs
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static IEnumerable<DirectoryInfo> SubDirsOf(string path)
        {
            DirectoryInfo[] dirs = null;
            try
            {
			    DirectoryInfo bdir = new DirectoryInfo (path);
			    dirs = bdir.GetDirectories();
            }
            catch (Exception)
            {
            }

            if (dirs != null)
            {
                foreach (var dir in dirs)
                    yield return dir;
            }
        }

		
		private static TimeSpan TimeFileAccess (string filename)
		{
			var buffer = new byte[4096];
			var random = new Random ();
			
			
			Stopwatch watch = new Stopwatch();
			watch.Start();
			
			var fileinfo = new FileInfo (filename);
			var filelen = fileinfo.Length;
			var stream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			
			for (int i = 0 ; i < 8 ; i++)
			{
				var Istart = (long)(random.NextDouble() * Math.Max(filelen - 4096, 0));
				stream.Seek (Istart, SeekOrigin.Begin);
				stream.Read (buffer, 0, buffer.Length);
			}
			
			stream.Close();
			
			watch.Stop ();
			return watch.Elapsed;
		}


		private static void InitializePosix ()
		{
			if (_syscall == typeof(string))
				return;
			else if (_syscall != null)
				return;

			SystemUtils.AddLibPath ("/opt/mono-3.0/lib");

			try
			{
				var assembly = Assembly.Load ("Mono.Posix, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
				_syscall = assembly.GetType ("Mono.Unix.Native.Syscall");
				_Mchmod = _syscall.GetMethod ("chmod");
			}
			catch (Exception)
			{
				_syscall = typeof(string);
				_log.Warn ("various posix functions not supported on this platform (chmod(), etc)");
			}
		}
	
		
		#endregion

		// Variables
		
		static Dictionary<string,bool>		_local = new Dictionary<string, bool>();
		static Type							_syscall;
		static MethodInfo					_Mchmod;

		static Logger						_log = Logger.Get ("DATA");
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/Logger.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.utils
{
	/// <summary>
	/// Logging facility
	/// </summary>
	public class Logger
	{	
		/// <summary>
		/// message severity
		/// </summary>
		public enum Severity
			{ None = 0, Severe = 1, Warn = 2, Info = 3, Debug = 5, Debug2 = 6, Debug3 = 7 };

		/// <summary>
		/// Log decorations
		/// </summary>
		[Flags] 
		public enum Show
			{ Severity = 1, Time = 2, Module = 4 }
		


		/// <summary>
		/// Creates a logging facility for some category of the application identified by string
		/// </summary>
		/// <param name='category'>
		/// Category assigned to this logger
		/// </param>
		/// <param name='severity'>
		/// what level of severity passes through for reporting
		/// </param>
		protected Logger (string category, Severity severity)
		{
			_category = category;
			_severity = severity;
		}
		
		
		// Properties
		
		/// <summary>
		/// Gets the logger category
		/// </summary>
		/// <value>
		/// The category.
		/// </value>
		public string Category
			{ get { return _category; } }
		
		
		/// <summary>
		/// Gets or sets the local verbosity level.
		/// </summary>
		/// <value>
		/// The level.
		/// </value>
		public Severity Level
			{ get { return _severity; } set { _severity = value; } }
		
		
		/// <summary>
		/// Gets or sets the master verbosity level.
		/// </summary>
		/// <value>
		/// The master level.
		/// </value>
		public static Severity MasterLevel
		{
			get 
				{ return _masterseverity; }
			set 
			{
				_masterseverity = value;
				foreach (var log in _loggers.Values)
					log.Level = value;
			}
		}
		
		
		/// <summary>
		/// Gets or sets the reporter.
		/// </summary>
		/// <value>
		/// The reporter.
		/// </value>
		public static IReporter Reporter
			{ get { return _sink; } set { _sink = value; } }
		
		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public static void SetMasterLevel (int level)
		{
			MasterLevel = (Severity)level;
		}
		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public void SetLocalAndMasterLevel (Severity level_local, Severity level_global)
		{
			MasterLevel = level_global;
			Level = level_local;
		}

		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public static void SetMasterLevel (Severity level)
		{
			MasterLevel = level;
		}
		
		
		// Predicates
		
		
		public bool IsInfoEnabled
			{ get { return _severity >= Severity.Info; } }

		public bool IsDebugEnabled
			{ get { return _severity >= Severity.Debug; } }
		
		public bool IsDebug2Enabled
			{ get { return _severity >= Severity.Debug2; } }
		
		public bool IsDebug3Enabled
			{ get { return _severity >= Severity.Debug3; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Get or create logger for category
		/// </summary>
		/// <param name='category'>
		/// Category.
		/// </param>
		public static Logger Get (string category)
		{
			// try to get existing logger
			Logger log = null;
			if (_loggers.TryGetValue(category, out log))
				return log;
			
			// otherwise create new one
			log = new Logger (category, _masterseverity);
			_loggers[category] = log;
			return log;
		}

		
		/// <summary>
		/// Report the specified msg if severity passes filter
		/// </summary>
		/// <param name='severity'>
		/// Severity fo msg
		/// </param>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Post (Severity severity, string msg)
		{
			if (severity <= _severity)
			{
				long Tnow = Clock.Now;
				_sink.Post (new Report (this, Tnow, severity, msg));
			}
		}
	
		
		/// <summary>
		/// Report the specified msg as fatal. 
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Fatal (string msg, bool exit = false)
		{
			Post (Severity.Severe, msg);
			if (exit)
				Environment.Exit (1);
		}
	
		
		/// <summary>
		/// Report the specified msg as warning
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Warn (string msg)
		{
			Post (Severity.Warn, msg);
		}
	
		
		/// <summary>
		/// Report the specified msg as info
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Info (string msg)
		{
			Post (Severity.Info, msg);
		}
	
		
		/// <summary>
		/// Report the specified msg as debug
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Debug (string msg)
		{
			Post (Severity.Debug, msg);
		}
		
		
		/// <summary>
		/// Report the specified msg as debug
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Debug2 (string msg)
		{
			Post (Severity.Debug2, msg);
		}

		
		/// <summary>
		/// Add and parse logging related arguments.
		/// </summary>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static void Parse (ArgumentParser args)
		{
			args.Register ("warn", false, false, "will show warnings and higher level messages");
			args.Register ("info", false, false, "will show information and higher level messages");
			args.Register ("debug", false, false, "will show debug and higher level messages");
			args.Register ("debug2", false, false, "will show debug2 and higher level messages");
			args.Register ("log:level", true, false, "sets the logging level between [0,7]");
			args.Register ("log:showall", false, false, "shows logging with all decorations");

			if (args.Contains("warn"))
				MasterLevel = Severity.Warn;
			if (args.Contains("log:showall"))
				Reporter.Decoration = (int)(Show.Time | Show.Module | Show.Severity);
			if (args.Contains("info"))
				MasterLevel = Severity.Info;
			if (args.Contains("debug"))
				MasterLevel = Severity.Debug;
			if (args.Contains("debug2"))
				MasterLevel = Severity.Debug2;
			if (args.Contains("log:level"))
				MasterLevel = (Severity)((int)args["log:level"]);
		}
		
		
		#region Classes
		
		
		/// <summary>
		/// Reporting implementation
		/// </summary>
		public interface IReporter
		{
			/// <summary>
			/// What to show
			/// </summary>
			/// <value>The show.</value>
			int						Decoration 		{ get; set; }

			/// <summary>
			/// Post the specified msg.
			/// </summary>
			/// <param name="msg">Message.</param>
			void					Post (Report msg);
		}
		
		
		/// <summary>
		/// Report structure
		/// </summary>
		public struct Report
		{
			public Logger			Source;
			public long				Time;
			public Severity			Level;
			public string			Message;
			
			
			public Report (Logger src, long time, Severity severity, string msg)
			{
				Source = src;
				Time = time;
				Level = severity;
				Message = msg;
			}
		}
		
		
		
		// Variables
		
		private string						_category;
		private Severity					_severity;
		
		static IReporter					_sink = new StderrReporter();
		static IDictionary<string,Logger>	_loggers = new Dictionary<string,Logger>();
		static Severity						_masterseverity = Severity.Warn;
	}


	#endregion

	#region Logger Reporters


	/// <summary>
	/// Reporter that writes to stderr
	/// </summary>
	public class StderrReporter : Logger.IReporter
	{
		public StderrReporter (int show = (int)(Logger.Show.Severity | Logger.Show.Time))
		{
			_show = show;
		}


		// Properties

		/// <summary>
		/// What to show
		/// </summary>
		/// <value>The show.</value>
		public int Decoration
			{ get { return _show; } set { _show = value; } }


		// Functions

		
		/// <summary>
		/// Post the msg with formatting to stderr
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Post (Logger.Report report)
		{
			StringBuilder s = new StringBuilder (report.Message.Length + 64);

			bool skip = false;
			if ((_show & (int)Logger.Show.Severity) != 0)
			{
				s.Append (SeverityOf(report.Level));
				skip = true;
			}
			
			if ((_show & (int)Logger.Show.Time) != 0)
			{
				if (skip) s.Append (' ');

				ZDateTime time = new ZDateTime(report.Time, ZTimeZone.Local);
				s.Append (string.Format(" [{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}]", 
					time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond));
				
				skip = true;
			}
			
			if ((_show & (int)Logger.Show.Module) != 0)
			{
				if (skip) s.Append (' ');

				s.Append ('[');
				s.Append (report.Source.Category);
				s.Append (']');
				
				skip = true;
			}
			
			if (skip)
				s.Append (": ");
			
			s.Append (report.Message);
			
			Console.Error.WriteLine (s);
		}
		
		
		// Implementation

		
		private static string SeverityOf (Logger.Severity level)
		{
			switch (level)
			{
			case Logger.Severity.Severe:
				return "FATAL";
			case Logger.Severity.Warn:
				return "WARN";
			case Logger.Severity.Info:
				return "INFO";
			default:
				return "DEBUG";
			}
		}

		// Variables
		
		private int		_show;
	}

	#endregion
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/ResourceLoader.cs
// -------------------------------------------
﻿// 
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




namespace bridge.common.utils
{	
	/// <summary>
	/// Load resource (indicated by path) from local filesystem
	/// </summary>
	public class ResourceLoader
	{
		static ResourceLoader()
		{
			// add one level down from execution directory as a basedir
			Uri url = new Uri (Assembly.GetAssembly(typeof(ResourceLoader)).CodeBase);
			string dllpath = Path.GetDirectoryName(url.AbsolutePath);
			
			string last = StringUtils.RField(dllpath, 0, Path.DirectorySeparatorChar,1);
			if (last == "Debug" || last == "Release")
				Paths.Add (StringUtils.RTrimField (dllpath, 2, Path.DirectorySeparatorChar));
			else
				Paths.Add (StringUtils.RTrimField (dllpath, 1, Path.DirectorySeparatorChar));
			
			// add elements in path
            var pathvar = (Environment.OSVersion.Platform == PlatformID.Win32NT) ? "Path" : "PATH";
            var PATH = Environment.GetEnvironmentVariable(pathvar);
			var elems = StringUtils.Split (PATH, Path.PathSeparator, true);
			
			foreach (string path in elems)
				Paths.Add (path);

			// add elements in library path
			var libpath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
			if (libpath != null)
			{
				var lelems = StringUtils.Split (libpath, Path.PathSeparator, true);
			
				foreach (string path in lelems)
					Paths.Add (path);
			}
			
			var installdir = FindInstallDir (Path.GetDirectoryName (typeof(ResourceLoader).Assembly.Location));
			if (installdir != null)
			{	
				// add install dir path
				Paths.Add (installdir);

				// add development paths
				var srcdir = installdir + Path.DirectorySeparatorChar + "src";
				if (Directory.Exists (srcdir))
				{	
					foreach (var path in FileUtils.FindDir (srcdir, "etc", 3))
						Paths.Add (FileUtils.AncestorOf(path, 1));
				}
			}
			
			Paths.Add (".");
		}

		
		// Operations
		
		
		/// <summary>
		/// Load the file at the specified relative path (can also be absolute path), returning as buffer
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='cache'>
		/// Cache.
		/// </param>
		public static Blob Load (string path, bool cache = false)
		{
			Blob resource = null;
			if (Resources.TryGetValue (path, out resource))
				return resource;
			
			// if is an absolute path, load
			if (Path.IsPathRooted (path))
				resource = LoadFromFilesystem (path);
			else
				resource = LoadAndFindInFilesystem (path);
			
			if (cache && resource != null)
				Resources[path] = resource;
			
			return resource;
		}
		
		
		/// <summary>
		/// find resource file in path
		/// </summary>
		/// <param name='path'>
		/// relative path to file
		/// </param>
		public static string Find (string path)
		{
			path = FileUtils.NormalizeDir (path);
			try
			{
				FileInfo ifile = new FileInfo (path);
				if (ifile.Exists)
					return path;
			}
			catch (Exception)
				{ }

			foreach (string rootdir in Paths)
			{
				try
				{
					FileInfo file = new FileInfo (rootdir + Path.DirectorySeparatorChar + path);
					if (!file.Exists)
						continue;
				
					return file.FullName;
				}
				catch (Exception)
					{ }
			}
			
			return null;
		}
		
		
		
		/// <summary>
		/// Finds directory within paths
		/// </summary>
		/// <param name='path'>
		/// relative sub-path
		/// </param>
		public static string FindDir (string path)
		{
            path = path.Replace('/', Path.DirectorySeparatorChar);
			foreach (string rootdir in Paths)
			{
                DirectoryInfo dir = new DirectoryInfo(rootdir + Path.DirectorySeparatorChar + path);
                if (dir.Exists)
				    return dir.FullName;
			}
			
			string cparent = Path.GetDirectoryName(Assembly.GetAssembly (typeof(ResourceLoader)).Location);
			
			DirectoryInfo pdir = null;
			DirectoryInfo cdir = new DirectoryInfo (cparent);
			
			while ((cdir = cdir.Parent) != pdir)
			{
				pdir = cdir;

                DirectoryInfo dir = new DirectoryInfo(cdir.FullName + Path.DirectorySeparatorChar + path);
                if (dir.Exists)
					return dir.FullName;
			}
			
			return null;
		}
	
		
		
		/// <summary>
		/// find resource parent directory containing file in classpath
		/// </summary>
		/// <returns>
		/// The dir containing given resource file
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static string FindDirWith (string path)
		{
			string full = Find (path);
			if (full != null)
				return Path.GetDirectoryName(full);
			else
				return null;
		}
		
		
		/// <summary>
		/// Add path to list that loader will scan through
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static void AddPath (string path)
		{
			Paths.Add (path);
		}
		
		
		
		// Implementation
		
		
		/**
		 * Cycle through classpath entries to locate a given file
		 * 
		 * @param path		path
		 */
		private static Blob LoadAndFindInFilesystem (string path)
		{
			foreach (string rootdir in Paths)
			{
				_log.Debug2 ("searching path: " + rootdir + ", for: " + path);
				string file = rootdir + Path.DirectorySeparatorChar + path;
				FileInfo info = new FileInfo (file);
				
				if (!info.Exists)
					continue;
				
				return LoadFromFilesystem (info.FullName);
			}
			
			return null;
		}
		
		
		/**
		 * Load file from path
		 * 
		 * @param path		path
		 */
		private static Blob LoadFromFilesystem (string path)
		{
			return FileUtils.ReadFile (path);
		}
		
		
		/// <summary>
		/// Finds the installation dir.
		/// </summary>
		private static string FindInstallDir (string basedir)
		{
			var installdir = Environment.GetEnvironmentVariable("GFLIBDIR");
			if (installdir != null)
				return installdir;
			
			DirectoryInfo pdir = null;
			DirectoryInfo cdir = new DirectoryInfo(basedir);
			while (pdir != cdir && cdir != null)
			{
				var cpath = cdir.FullName;
				if (IsRoot (cpath))
					return null;
				
				var finddir = cpath + Path.DirectorySeparatorChar + ".installdir";
				var info = new FileInfo (finddir);
				if (info.Exists)
					return cdir.FullName;
				
				pdir = cdir;
				cdir = cdir.Parent;
			}
			
			return null;
		}


		/// <summary>
		/// Determines if the path root is reached
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		private static bool IsRoot (string path)
		{
			if (!SystemUtils.IsWindows)
				return path == "/";

			if (path[1] == ':' && path[2] == '\\')
				return true;
			else
				return false;
		}

		
		// Variables

		static IList<string>				Paths = new List<string>();
		static IDictionary<string,Blob>		Resources = new Dictionary<string,Blob>();

		static Logger						_log = Logger.Get ("CONFIG");
	}
}

// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/StringUtils.cs
// -------------------------------------------
﻿// 
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


// -------------------------------------------
// File: ../DotNet/Library/src/common/utils/SystemUtils.cs
// -------------------------------------------
﻿// 
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


namespace bridge.common.utils
{
	/// <summary>
	/// Various system utilities
	/// </summary>
	public static class SystemUtils
	{
		static SystemUtils ()
		{
			var Tbasis = DateTime.UtcNow.Ticks;
			_clockoffset = Tbasis - Stopwatch.GetTimestamp();
			_epochoffset = _clockoffset - 621355968000000000L;
		}
		
		
		/// <summary>
		/// Idnicates whether is windows or unix
		/// </summary>
		/// <value>
		/// <c>true</c> if running on windows; otherwise, <c>false</c>.
		/// </value>
		public static bool IsWindows
			{ get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }

		
		/// <summary>
		/// Gets the path of the root temporary directory
		/// </summary>
		public static string TmpPath
		{
			get
			{
				if (IsWindows)
					return Path.GetTempPath();
				else
					return "/tmp";
			}
		}
		
		
		/// <summary>
		/// Adds to the system path
		/// </summary>
		/// <param name='path'>
		/// Path to be added
		/// </param>
        public static void AddPath (string path)
        {
            var pathvar = IsWindows ? "Path" : "PATH";
            var opath = Environment.GetEnvironmentVariable(pathvar);
            var npath = opath + Path.PathSeparator + path;
            Environment.SetEnvironmentVariable (pathvar, npath);
        }
		
		
		/// <summary>
		/// Adds to the library path
		/// </summary>
		/// <param name='path'>
		/// Path to be added
		/// </param>
        public static void AddLibPath (string path)
        {
            if (IsWindows)
			{
            	var opath = Environment.GetEnvironmentVariable("Path");
				if (opath.Contains (path))
					return;
				
            	var npath = opath + Path.PathSeparator + path;
            	Environment.SetEnvironmentVariable ("Path", npath);
			}
			else
			{
            	var opath_linux = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
				if (opath_linux == null || !opath_linux.Contains (path))
				{
					var npath = StringUtils.IsBlank(opath_linux) ? path : opath_linux + Path.PathSeparator + path;
            		Environment.SetEnvironmentVariable ("LD_LIBRARY_PATH", npath);
				}
            	var opath_osx = Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH");
				if (opath_osx == null || !opath_osx.Contains (path))
				{
					var npath = StringUtils.IsBlank(opath_osx) ? path : opath_osx + Path.PathSeparator + path;
            		Environment.SetEnvironmentVariable ("DYLD_LIBRARY_PATH", npath);
				}
			}
        }
		
		
		/// <summary>
		/// Gets the clock time since epoch (Jan 1 1970) in milliseconds
		/// </summary>
		public static long ClockMillis
		{
			get 
			{
				return (Stopwatch.GetTimestamp() + _epochoffset) / 10000L;
			}
		}
		
		
		/// <summary>
		/// Gets the clock time since epoch (Jan 1 1970) in milliseconds
		/// </summary>
		public static long ClockMicro
		{
			get 
			{
				return (Stopwatch.GetTimestamp() + _epochoffset) / 10L;
			}
		}

		
		/// <summary>
		/// Gets the clock time in 10ths of a microsecond on a .NET clock basis
		/// </summary>
		public static long Ticks
		{
			get 
			{
				return Stopwatch.GetTimestamp() + _clockoffset;
			}
		}
		
		
		// Variables
		
		static readonly long 	_clockoffset;
		static readonly long 	_epochoffset;
	}

}


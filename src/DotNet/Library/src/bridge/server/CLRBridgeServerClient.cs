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
using System.IO;
using bridge.common.io;
using bridge.common.utils;
using System.Threading;
using bridge.server.ctrl;
using System.Net;
using bridge.embedded;
using System.Reflection;
using bridge.common.reflection;


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


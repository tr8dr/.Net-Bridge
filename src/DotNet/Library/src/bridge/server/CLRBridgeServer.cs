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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using bridge.common.io;
using bridge.common.utils;


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

			SetupListener ();

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
		private void SetupListener ()
		{
			_log.Info("starting execution server listener on port: " + _port);
			var mask = new IPEndPoint(IPAddress.Any, _port);
			_server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _server_socket.Bind(mask);
                _server_socket.Listen(10);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    _log.Warn("another CLR server already running, exiting");
                    Environment.Exit(0);
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


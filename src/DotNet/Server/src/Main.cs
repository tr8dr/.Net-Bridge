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
using bridge.common.utils;
using bridge.server;
using bridge.common.reflection;
using System.Collections.Generic;


namespace CLRServer
{
    /// <summary>
    /// Runs the CLR server on a given port.
    /// 
    /// </summary>
	class Server
	{
        private static void LoadDlls (IList<Any> dlls)
        {
			_log.Info("loading and registering library assemblies");
            foreach (var arg in dlls)
            {
                var assemblyname = (string)arg;
                var assembly = ReflectUtils.FindAssembly(assemblyname);
                ReflectUtils.Register(assembly);
            }
		}

		public static void Main (string[] argv)
		{
			ArgumentParser args = new ArgumentParser (argv);
			args.Register ("url", true, false, "server URL");
			args.Register ("dll", true, false, "library to make visible on the CLR bridge");
			Logger.Parse (args);
			
			var url = new Uri (args.Or ("url", "svc://127.0.0.1:56789"));

            if (args.Contains("dll"))
                LoadDlls(args["dll"].ValueList);

			_log.Info ("starting CLR bridge server");
			var svr = new CLRBridgeServer (url);
			svr.Start (blocking: true);

            Environment.Exit(0);
		}


		static Logger		_log = Logger.Get("CLR");
	}
}

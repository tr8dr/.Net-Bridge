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
using System.Net;


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


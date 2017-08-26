//
// General:
//      This file is pary of .NET Bridge
//
// Copyright:
//      2010 Jonathan Shore
//	2017 Jonathan Shore and Contributors
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



#ifndef RTCP_CLIENT
#define RTCP_CLIENT

#include <cstdlib>
#include <string>

typedef unsigned char byte;


//
// Simple TCP stream client
//
class RTcpClient
{
  public:

    RTcpClient (const std::string& host, int port)
      : _hostname(host), _port(port), _sock(-1)
    {
      connect (host, port);
    }

    // determine if connected
    bool is_connected ();

    // read data into buffer 
    int read (byte* buffer, int bufferlen, int retries = 0);

    // write data 
    int write (const byte* buffer, int len, int retries = 0);

    // close socket
    void close ();

  private:

    // reconnect if connection was broken 
    void reconnect ();

    // connect  
    void connect (const std::string& host, int port);

  private:
      std::string  _hostname;
      int          _port;
      int          _sock;
};

#endif

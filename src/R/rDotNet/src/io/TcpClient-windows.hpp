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

// [[Rcpp::depends(BH)]]

#ifndef TCP_CLIENT
#define TCP_CLIENT

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")


using namespace std;

typedef char byte;


//
// Simple TCP stream client
//
class TcpClient
{
  public:

    TcpClient (const std::string& host, int port)
      : _host(host), _post(port), _sock(INVALID_SOCKET)
    {
        connect ();
    }

    // read data into buffer 
    int read (byte* buffer, int bufferlen, int retries = 0)
    {
        for (int i = 0 ; i <= retries ; i++)
	{
            reconnect();
	    int n = recv (_sock, buffer, bufferlen, 0);
	    if (n >= 0)
	        return n;
	    else
	        close();
	}

	return 0;
    }

    // write data 
    int write (const byte* buffer, int len, int retries = 0)
    {
        for (int i = 0 ; i <= retries ; i++)
	{
            reconnect();
	    int n = send (_sock, buffer, len);
	    if (n > 0)
	        return n;
	    else
	        close();
	}

	return 0;
    }

    // close socket
    void close ()
    {
        if (_sock == INVALID_SOCKET)
	    return;
	
        closesocket (_sock);
	WSACleanup();
    }
  

  private:

    // reconnect if connection was broken 
    void reconnect ()
    {
        if (_sock != INVALID_SOCKET)
	  return;

	connect();
    }


    // connect  
    void connect ()
    {
        struct addrinfo hints;
	WSADATA wsaData;

	// magic needed to initialize the winsock API (usual WIN32 stupid internals exposure)
        int err = WSAStartup(MAKEWORD(2,2), &wsaData);
	if (err != 0)
	    throw std::runtime_error("failed to initialize socket api");

	// setup type of connect
	ZeroMemory (&hints, sizeof(hints));
	hints.ai_family = AF_UNSPEC;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	// resolve the host
	struct addrinfo* hostlist;
	err  = getaddrinfo(_host.c_str(), _port, &hints, &hostlist);
	if (err != 0)
	    { WSACleanup(); throw std::runtime_error("failed to connect to DNS"); }

	// attempt to connect to each alternative for the host in turn
	for (struct addrinfo* host = hostlist; host != NULL ; host = host->ai_next)
	{
	    // create socket
	    _sock = socket(host->ai_family, host->ai_socktype, host->ai_protocol);
	    if (_sock == INVALID_SOCKET)
	        { WSACleanup(); throw std::runtime_error("unable to create socket"); }

	    // attempt to connect
	    err = connect (_sock, host->ai_addr, (int)host->ai_addrlen);
	    if (iResult == SOCKET_ERROR)
	    {
	        closesocket(_sock);
	        _sock = INVALID_SOCKET;
	    } else
	        break;
	}

	// free up host resolution & check for connection
	freeaddrinfo(host);
    }
  
  private:
      string       _host;
      int          _port;
      SOCKET       _sock;
};

#endif

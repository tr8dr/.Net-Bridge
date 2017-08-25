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


#include "io/TcpClient.hpp"
#include "OS.hpp"

#ifdef WINDOWS
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#endif

#include <stdlib.h>
#include <stdexcept>

using namespace std;

#ifdef WINDOWS

// read data into buffer 
int RTcpClient::read (byte* buffer, int bufferlen, int retries)
{
    char* wbuffer = (char*)((void*)buffer);
    for (int i = 0 ; i <= retries ; i++)
    {
        reconnect();
	int n = recv (_sock, wbuffer, bufferlen, 0);
	if (n >= 0)
	    return n;
	else
	    close();
    }

	return 0;
}

// write data 
int RTcpClient::write (const byte* buffer, int len, int retries)
{
    char* wbuffer = (char*)((void*)buffer);
    for (int i = 0 ; i <= retries ; i++)
    {
        reconnect();
	int n = send (_sock, wbuffer, len, 0);
	if (n > 0)
	    return n;
	else
	    close();
    }
    
    return 0;
}

// close socket
void RTcpClient::close ()
{
  if (_sock < 0)
        return;
	
    closesocket (_sock);
    _sock = -1;
    WSACleanup();
}
  

// reconnect if connection was broken 
void RTcpClient::reconnect ()
{
  if (_sock < 0)
      return;

  connect(_hostname, _port);
}


// connect  
void RTcpClient::connect (const std::string& host, int port)
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
    char portname[32];
    char* hostname = (char*)((void*)host.c_str());

    sprintf(portname, "%d", port);

    err  = getaddrinfo(hostname, portname, &hints, &hostlist);
    if (err != 0)
        { WSACleanup(); throw std::runtime_error("failed to connect to DNS"); }

    // attempt to connect to each alternative for the host in turn
    struct addrinfo* addr = NULL;
    for (addr = hostlist; addr != NULL ; addr = addr->ai_next)
    {
        // create socket
        SOCKET sock = socket(addr->ai_family, addr->ai_socktype, addr->ai_protocol);
	if (sock == INVALID_SOCKET)
	    { WSACleanup(); throw runtime_error("unable to create socket"); }
        else
  	    _sock = sock;

	// attempt to connect
	err = ::connect (_sock, addr->ai_addr, (int)addr->ai_addrlen);
	if (err == SOCKET_ERROR)
	{
	    closesocket(_sock);
	    _sock = -1;
	} else
	    break;
    }

    // free up host resolution & check for connection
    freeaddrinfo(hostlist);
}

#else


#endif

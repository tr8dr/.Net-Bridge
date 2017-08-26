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


#include "TcpClient.hpp"
#include "OS.hpp"

#ifdef WINDOWS
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#else
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h> 
#endif

#include <Rcpp.h>
#include <stdlib.h>
#include <stdexcept>
#include <iostream>
#include <sstream>

using namespace std;
using namespace Rcpp;


// read data into buffer 
int RTcpClient::read (byte* buffer, int bufferlen, int retries)
{
    for (int i = 0 ; i <= retries ; i++)
    {
        reconnect();
#ifdef WINDOWS
	int n = ::recv (_sock, (char*)((void*)wbuffer), bufferlen, 0);
#else
	int n = ::recv (_sock, (void*)buffer, bufferlen, 0);
#endif	
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
    for (int i = 0 ; i <= retries ; i++)
    {
        reconnect();
#ifdef WINDOWS
	int n = send (_sock, (char*)((void*)wbuffer), len, 0);
#else
	int n = ::write (_sock, (void*)buffer, len);
#endif
	if (n > 0)
	{
	    return n;
	} else
	    close();
    }
    
    return 0;
}

// close socket
void RTcpClient::close ()
{
    if (_sock < 0)
        return;

    throw std::runtime_error ("closing connection\n");
#ifdef WINDOWS
    closesocket (_sock);
    _sock = -1;
    WSACleanup();
#else
    ::close (_sock);
    _sock = -1;
#endif
}
  

// reconnect if connection was broken 
void RTcpClient::reconnect ()
{
    if (_sock >= 0)
        return;

    connect(_hostname, _port);
}


#ifdef WINDOWS

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

// connect  
void RTcpClient::connect (const std::string& host, int port)
{
    // create socket
    _sock = socket(AF_INET, SOCK_STREAM, 0);
    if (_sock < 0)
        throw runtime_error("unable to create socket");

    // lookup host address
    struct hostent* server = ::gethostbyname(host.c_str());
    if (server == NULL)
        throw runtime_error("unable to lookup or locate CLR host on DNS");

    // create address
    struct sockaddr_in addr;
    bzero((void *)&addr, sizeof(addr));
    addr.sin_family = AF_INET;
    bcopy((void *)server->h_addr, (void *)&addr.sin_addr.s_addr, server->h_length);
    addr.sin_port = htons(port);

    // create connection
    int err = ::connect (_sock, (struct sockaddr *)&addr, sizeof(addr));
    if (err < 0)
        close();
}

#endif

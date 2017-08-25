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

#include <boost/asio.hpp>

using boost::asio::ip::tcp;
using namespace boost::asio::ip;
using namespace boost::asio;
using namespace std;

typedef char byte;


//
// Simple TCP stream client
//
class TcpClient
{
  public:

    typedef boost::asio::io_service service_t;
    typedef tcp::endpoint endpoint_t;
    typedef tcp::socket socket_t;
    typedef boost::system::error_code error_t;

    TcpClient (const std::string& host, int port)
      : _service(), _endpoint(), _sock(_service)
    {
        init (host, port);
    }

    // read data into buffer 
    int read (byte* buffer, int bufferlen, int retries = 0)
    {
        for (int i = 0 ; i <= retries ; i++)
	{
            reconnect();
	    int n = _sock.read_some (boost::asio::buffer(buffer,bufferlen));
	    if (n >= 0)
	        return n;
	    else
	        _sock.close();
	}

	return 0;
    }

    // write data 
    int write (const byte* buffer, int len, int retries = 0)
    {
        for (int i = 0 ; i <= retries ; i++)
	{
            reconnect();
	    int n = boost::asio::write (_sock, boost::asio::buffer(buffer,len));
	    if (n > 0)
	        return n;
	    else
	        _sock.close();
	}

	return 0;
    }

    // close socket
    void close ()
    {
        _sock.close();
    }
  

  private:
  
    // reconnect if connection was broken 
    void reconnect ()
    {
        if (_sock.is_open())
	  return;
	
	error_t error = boost::asio::error::host_not_found;

	_sock.close();
	_sock.connect (_endpoint, error);

	if (error)
	    throw boost::system::system_error(error);
    }

    // initial connection & name resolution 
    void init (const std::string& host, int port)
    {
	error_t error = boost::asio::error::host_not_found;
	char s_port[32];
	sprintf(s_port, "%d", port);
	
	// get endpoints that match host / port
	tcp::resolver resolver(_service);
        tcp::resolver::query query (host, s_port);
	tcp::resolver::iterator endpoint_iterator = resolver.resolve(query);
	tcp::resolver::iterator end;

	// attempt to connect to each endpoint in turn
	while (error && endpoint_iterator != end) {
	    _sock.close();
	    _sock.connect (*endpoint_iterator, error);
	    _endpoint = *endpoint_iterator++;
	}

	if (error)
	    throw boost::system::system_error(error);
    }
  
  private:
      service_t    _service; 
      endpoint_t   _endpoint;
      socket_t     _sock;
};

#endif

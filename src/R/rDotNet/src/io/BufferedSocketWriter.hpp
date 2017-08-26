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

#ifndef BUFFERED_SOCKET_WRITER
#define BUFFERED_SOCKET_WRITER

#include <cstdlib>
#include <stdexcept>
#include <Rcpp.h>
#include "TcpClient.hpp"

using namespace std;
using namespace Rcpp;

struct WriteStreamTerminatedException : std::exception
{
  char const* what() const throw();
};

//
// Buffered stream writer for various types
//
class BufferedSocketWriter
{
  public:

    BufferedSocketWriter (RTcpClient* tcp, int buflen = 8192)
      : _sock(tcp), _buffer(NULL), _buflen(buflen), _len(0)
    {
        _buffer = new byte[buflen];
    }

    ~BufferedSocketWriter ()
    {
        delete[] _buffer;
    }


    // write a byte to the stream
    void write_byte (char b)
    {
        if ((_len+1) > _buflen)
	    flush();

	_buffer[_len++] = b;
    }

    // write int16 
    void write_int16 (short v)
    {
        if ((_len+2) > _buflen)
	    flush();

	const char* bytes = reinterpret_cast<char *>(&v);
	_buffer[_len++] = bytes[0];
	_buffer[_len++] = bytes[1];
    }

    // write int32 
    void write_int32 (int v)
    {
        if ((_len+4) > _buflen)
	    flush();

	const char* bytes = reinterpret_cast<char *>(&v);
	_buffer[_len++] = bytes[0];
	_buffer[_len++] = bytes[1];
	_buffer[_len++] = bytes[2];
	_buffer[_len++] = bytes[3];
    }

    // write int64 
    void write_int64 (long v)
    {
        if ((_len+8) > _buflen)
	    flush();

	const char* bytes = reinterpret_cast<char *>(&v);
	_buffer[_len++] = bytes[0];
	_buffer[_len++] = bytes[1];
	_buffer[_len++] = bytes[2];
	_buffer[_len++] = bytes[3];
	_buffer[_len++] = bytes[4];
	_buffer[_len++] = bytes[5];
	_buffer[_len++] = bytes[6];
	_buffer[_len++] = bytes[7];
    }

    // write float64 
    void write_float64 (double v)
    {
        if ((_len+8) > _buflen)
	    flush();

	const char* bytes = reinterpret_cast<char *>(&v);
	_buffer[_len++] = bytes[0];
	_buffer[_len++] = bytes[1];
	_buffer[_len++] = bytes[2];
	_buffer[_len++] = bytes[3];
	_buffer[_len++] = bytes[4];
	_buffer[_len++] = bytes[5];
	_buffer[_len++] = bytes[6];
	_buffer[_len++] = bytes[7];
    }

    // write string 
    void write_string (const std::string& v)
    {
        int len = v.length();
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	    write_byte(v[i]);  
    }

    // write string 
    void write_string (const char* v)
    {
        int len = strlen(v);
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	    write_byte(v[i]);  
    }

    // write bool vector 
    void write_bool_array (const LogicalVector& v)
    {
        int len = v.size();
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	  write_int32(v[i] ? (char)1 : (char)0);  
    }

    // write int32 vector 
    void write_int32_array (const IntegerVector& v)
    {
        int len = v.size();
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	    write_int32(v[i]);  
    }

    // write float64 vector 
    void write_float64_array (const NumericVector& v)
    {
        int len = v.size();
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	    write_float64(v[i]);  
    }

    // write string vector 
    void write_string_array (const CharacterVector& v)
    {
        int len = v.size();
        write_int32(len);

	for (int i = 0 ; i < len ; i++)
	    write_string(v[i]);  
    }
  
    // close stream
    void close ()
    {
       flush();
       _sock->close();
       _len = 0;
    }
  
    // flush stream
    void flush ()
    {
       int done = _sock->write(_buffer, _len);
       if (done < _len)
	   throw std::runtime_error("problem communicating with CLR, could not complete message");
       _len = 0;
    }

  
  private:
    RTcpClient* _sock; 
    byte*       _buffer;
    int         _buflen;
    int         _len;
};

#endif

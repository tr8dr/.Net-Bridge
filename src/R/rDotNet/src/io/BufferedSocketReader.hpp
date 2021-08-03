//
// General:
//      This file is part of .NET Bridge
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

#ifndef BUFFERED_SOCKET_READER
#define BUFFERED_SOCKET_READER

#include <cstdlib>
#include <Rcpp.h>
#include "TcpClient.hpp"

using namespace std;
using namespace Rcpp;

struct ReadStreamTerminatedException : std::exception
{
    char const* what() const throw()
    {
        return "BufferedSocketReader: failed to complete stream read";
    }
};

//
// Buffered stream reader for various types
//
class BufferedSocketReader
{
  public:

    BufferedSocketReader (RTcpClient* tcp, int buflen = 4*8192)
      : _sock(tcp), _buffer(NULL), _buflen(buflen), _pos(0), _len(0), _eof(false)
    {
        _buffer = new byte[buflen];
    }

    ~BufferedSocketReader ()
    {
        delete[] _buffer;
    }

    // determine whether at EOS
    bool isEOF()
    {
        if (_eof)
	    return true;
	else if (_pos < _len)
	    return true;
	else {
	    replenish(1);
	    _eof = _len == 0;
	    return _eof;
	}
    }

    // read a byte from the stream
    char read_byte ()
    {
        if (_pos == _len)
	    replenish(1);
	if (_len < 1)
	    throw ReadStreamTerminatedException();
	else
	  return _buffer[_pos++];
    }

    // read a UTF-8 string from the stream (this is not efficient, but works)
    std::string read_string ()
    {
        // read string length
        int len = read_int32();

	// read string text
	char* tmp = new char[len];
	for (int i = 0 ; i < len ; i++)
	    tmp[i] = read_byte();

	std::string newstr (tmp, len);
	delete[] tmp;
	return newstr;
    }

    // read int16 
    int16_t read_int16 ()
    {
        if ((_pos+2) > _len)
	    replenish(2);
	if (_len < 2)
	    throw ReadStreamTerminatedException();

	int16_t* bufint = reinterpret_cast<int16_t *>(_buffer + _pos);
	_pos += 2;
	return *bufint;
    }

    // read int32 
    int32_t read_int32 ()
    {
        if ((_pos+4) > _len)
	    replenish(4);
	if (_len < 4)
	    throw ReadStreamTerminatedException();

	int32_t* bufint = reinterpret_cast<int32_t *>(_buffer + _pos);
	_pos += 4;
	return *bufint;
    }

    // read int64 
    int64_t read_int64 ()
    {
        if ((_pos+8) > _len)
	    replenish(8);
	if (_len < 8)
	    throw ReadStreamTerminatedException();

	int64_t* bufint = reinterpret_cast<int64_t *>(_buffer + _pos);
	_pos += 8;
	return *bufint;
    }

    // read float64 
    double read_float64 ()
    {
        if ((_pos+8) > _len)
	    replenish(8);
	if (_len < 8)
	    throw ReadStreamTerminatedException();

	double* bufval = reinterpret_cast<double *>(_buffer + _pos);
	_pos += 8;
	return *bufval;
    }

    // read a boolean array
    LogicalVector* read_bool_array ()
    {
        // read array length
        int len = read_int32();

	// read values into vector
	LogicalVector* vec = new LogicalVector(len);
	for (int i = 0 ; i < len ; i++)
	    (*vec)[i] = read_byte() != (char)0;

	return vec;
    }

    // read a float64 array
    NumericVector* read_float64_array ()
    {
        // read array length
        int len = read_int32();

	// read values into vector
	NumericVector* vec = new NumericVector(len);
	for (int i = 0 ; i < len ; i++)
	  (*vec)[i] = read_float64();

	return vec;
    }

    // read a int32 array
    IntegerVector* read_int32_array ()
    {
        // read array length
        int len = read_int32();

	// read values into vector
	IntegerVector* vec = new IntegerVector(len);
	for (int i = 0 ; i < len ; i++)
	  (*vec)[i] = read_int32();

	return vec;
    }

    // read a string array
    CharacterVector* read_string_array ()
    {
        // read array length
        int len = read_int32();

	// read values into vector
	CharacterVector* vec = new CharacterVector(len);
	for (int i = 0 ; i < len ; i++)
	  (*vec)[i] = read_string();

	return vec;
    }

    void close ()
    {
       _sock->close();
    }


  private:

    void replenish (int n)
    {
        // move residual to start of buffer
        int residual = _len - _pos;
        memcpy(_buffer, _buffer+_pos, residual);
	_pos = 0;
	_len = residual;

	// read required amount or more, replenishing buffer
	int read = 1;
	int total = _len;
	while (total < n && read > 0)
	{
	    int amount = _buflen - _len;
	    int r = _sock->read (_buffer + _len, amount);
	    read = max(r, 0);
	    
	    _len += read;
	    total += read;
	}
    }
  
  private:
    RTcpClient* _sock; 
    byte*       _buffer;
    int         _buflen;
    int         _pos;
    int         _len;
    bool        _eof;
};

#endif

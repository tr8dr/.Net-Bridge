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

#ifndef CLR_API
#define CLR_API

#include <cstdlib>
#include "CLRFactory.hpp"
#include "CLRObjectRef.hpp"
#include "msgs/CLRMessage.hpp"
#include "io/TcpClient.hpp"
#include "io/BufferedSocketReader.hpp"
#include "io/BufferedSocketWriter.hpp"

using namespace std;


//
// CLR API
//
class CLRApi
{
  public:

    typedef SEXP CLRObject;

    CLRApi (const char* host = "localhost", int port = 56789, int retries = 4)
      : _host(host), _port(port), _retries(retries), _factory(new CLRFactory(this)), 
	_tcp(NULL), _sin(NULL), _sout(NULL) {}

    ~CLRApi()
    {
        if (_factory != NULL)
	    delete _factory;
        if (_sout != NULL)
	    { _sout->close(); delete _sout; }
        if (_sin != NULL)
	    { _sin->close(); delete _sin; }
        if (_tcp != NULL)
	    { _tcp->close(); delete _tcp; }
    }

    // message factory for this API 
    CLRFactory* factory()
    {
        return _factory;
    }
  
    // start connection with CLR
    void start();
    // stop / close connection with CLR, resetting for new connection
    void reset(bool restart = true);

    // create object
    RValue create (const std::string& classname, const List& argv);
    // call static method
    RValue callstatic (const std::string& classname, const std::string& method, const List& argv);
  
    // call method on object
    RValue call (CLRObject obj, const std::string& method, const List& argv);
    // get property value
    RValue get (CLRObject obj, const std::string& property);
    // set property value
    void set (CLRObject obj, const std::string& property, const RObject& value);
    // get indexed value
    RValue get_indexed (CLRObject obj, int ith);

    // release object
    void release (int objectId);
    // evaluate query against CLR
    CLRMessage* read ();

  protected:

    // evaluate query against CLR
    RValue query (CLRMessage* msg);
    // evaluate message on CLR
    void exec (CLRMessage* msg);

  private:
    std::string            _host;
    int                    _port;
    int                    _retries;
    CLRFactory*            _factory;
    TcpClient*             _tcp;
    BufferedSocketReader*  _sin;
    BufferedSocketWriter*  _sout;
};


#endif

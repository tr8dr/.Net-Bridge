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

#include <cstdlib>
#include "Common.hpp"
#include "CLRApi.hpp"
#include "CLRObjectRef.hpp"

#include "msgs/ctrl/CLRCreateObject.hpp"
#include "msgs/ctrl/CLRCallStatic.hpp"
#include "msgs/ctrl/CLRCallMethod.hpp"
#include "msgs/ctrl/CLRSetProperty.hpp"
#include "msgs/ctrl/CLRGetProperty.hpp"
#include "msgs/ctrl/CLRGetIndexed.hpp"
#include "msgs/ctrl/CLRRelease.hpp"

using namespace std;
using namespace Rcpp;


// get object ID from R object structure
static int objectRefFor (SEXP obj)
{
    RObject robj (obj);
    SEXP objid = robj.attr("ObjectId");
    
    if (!Rf_isNull(objid))
        return Rcpp::as<int>(objid);
    else
        throw std::runtime_error ("CLRObject: cannot find object handle");
}

// evaluate query against CLR
RValue CLRApi::query (CLRMessage* msg)
{
    // make sure API has been started 
    start();

    CLRMessage* rmsg = nullptr;
    try
    {
      // send query
      msg->serialize (*_sout);
      _sout->flush();

      // wait for response
      short magic = _sin->read_int16();
      if (magic != CLRMessage::Magic)
	throw std::runtime_error ("message magic # is wrong, garbled sequence");
    
      char mtype = _sin->read_byte();
      // create appropriate message container
      rmsg = _factory->messageById (mtype);
      // read message
      rmsg->deserialize (*_sin);
    }
    catch (std::exception& se)
    {
        reset(true);
	throw std::runtime_error(se.what());
    }
    catch (boost::exception& be)
    {
        reset(true);
	throw std::runtime_error("connection issue with CLR, resetting");
    }
    
    // return SEXP
    RValue v = rmsg->rvalue();
    delete rmsg;

    return v;
}

// read message
CLRMessage* CLRApi::read ()
{
    // wait for response
    short magic = _sin->read_int16();
    if (magic != CLRMessage::Magic)
        throw std::runtime_error ("message magic # is wrong, garbled sequence");
    
    char mtype = _sin->read_byte();
    // create appropriate message container
    CLRMessage* msg = _factory->messageById (mtype);
    // read message
    msg->deserialize (*_sin);
    
    return msg;
}


// execute on CLR
void CLRApi::exec (CLRMessage* msg)
{
    // make sure API has been started 
    start();
    
    try
    {
        // send query
        msg->serialize (*_sout);
        _sout->flush();
    }
    catch (std::exception& se)
    {
        reset(false);
	throw std::runtime_error(se.what());
    }
    catch (boost::exception& be)
    {
        reset(false);
	throw std::runtime_error("connection issue with CLR, resetting");
    }
}


// start connection with CLR
void CLRApi::start()
{
    if (_tcp != nullptr)
      return;
    
    for (int i = 0 ; i <= _retries; i++)
    {
        try
        {
	    _tcp = new TcpClient (_host, _port);
	    _sin = new BufferedSocketReader (_tcp);
	    _sout = new BufferedSocketWriter (_tcp);
        }
        catch (...)
        {
	    if (i < _retries)
	        sleep(2);
	    else
	        throw std::runtime_error("could not connect to CLR server");
        }
    }
}


// stop / close connection with CLR
void CLRApi::reset(bool restart)
{
    if (_tcp != nullptr)
        _tcp->close();
    
    if (_tcp != nullptr)
        delete _tcp;

    if (_sin != nullptr)
        delete _sin;
    
    if (_sout != nullptr)
        delete _sout;
    
    _tcp = NULL;
    _sin = NULL;
    _sout = NULL;
    
    if (restart)
        start();
}

// create object
RValue CLRApi::create (const std::string& classname, const List& argv)
{
    CLRCreateObject req (this, classname, argv);
    return query (&req);
}

// call static method
RValue CLRApi::callstatic (const std::string& classname, const std::string& method, const List& argv)
{
    CLRCallStatic req (this, classname, method, argv);
    return query (&req);
}
  
// call method on object
RValue CLRApi::call (CLRObject obj, const std::string& method, const List& argv)
{
    int objectId = objectRefFor (obj);
    CLRCallMethod req (this, objectId, method, argv);
    return query (&req);
}

// get property value
RValue CLRApi::get (CLRObject obj, const std::string& property)
{
    int objectId = objectRefFor (obj);
    CLRGetProperty req (this, objectId, property);
    return query (&req);
}

// get indexed value
RValue CLRApi::get_indexed (CLRObject obj, int ith)
{
    int objectId = objectRefFor (obj);
    CLRGetIndexed req (this, objectId, ith);
    return query (&req);
}

// set property value
void CLRApi::set (CLRObject obj, const std::string& property, const RObject& value)
{
    int objectId = objectRefFor (obj);
    CLRSetProperty req (this, objectId, property, value);
    query (&req);
}

// release object
void CLRApi::release (int objectId)
{
    CLRRelease req (this, objectId);
    exec (&req);
}

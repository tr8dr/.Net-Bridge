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


#ifndef CLR_OBJECT_ARRAY
#define CLR_OBJECT_ARRAY

#include <Rcpp.h>
#include <cstdlib>
#include "msgs/CLRValue.hpp"
#include "CLRApi.hpp"

using namespace std;


//
// Object array
//
class CLRObjectArray : public CLRValue<List>
{
  public:

    CLRObjectArray (CLRApi* api, List* value = nullptr)
      : CLRValue(CLRMessage::TypeObjectArray, api, value)
    {
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        assert (_value != NULL);
        CLRMessage::serialize (stream);

	int len = _value->size();
	CLRFactory* factory = _api->factory();
	
	stream.write_int32 (len);
	for (int i = 0 ; i < len ; i++)
	{
	    SEXP obj = (*_value)[i];
	    RObject robj (obj);
	    CLRMessage* msg = factory->messageByValue (robj);

	    msg->serialize(stream);
	    delete msg;
	}
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
        int len = stream.read_int32();
	_value = new List();

	for (int i = 0 ; i < len ; i++)
	{
	    CLRMessage* msg = _api->read();
	    SEXP value = msg->rvalue();
	    _value->push_back (value);

	    delete msg;
	}
    }
};

#endif

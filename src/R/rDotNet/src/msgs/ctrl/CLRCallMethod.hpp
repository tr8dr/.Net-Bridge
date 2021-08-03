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

#ifndef CLR_CALL_METHOD
#define CLR_CALL_METHOD

#include <cstdlib>
#include <Rcpp.h>
#include "CLRFactory.hpp"

using namespace std;


//
//  Call object method Message
//
class CLRCallMethod : public CLRMessage
{
  public:
  
    CLRCallMethod (CLRApi* api, int32_t objectId, const std::string& method, const List& argv)
      : CLRMessage(CLRMessage::TypeCallMethod, api), _objectId(objectId), _method(method), _argv(argv)
    {
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        CLRMessage::serialize (stream);
	stream.write_int32(_objectId);
	stream.write_string(_method);

	// argv
	int argc = _argv.size();
	stream.write_int16((int16_t)argc);

	CLRFactory* factory = _api->factory();
	for (int i = 0 ; i < argc ; i++)
	{
	    CLRMessage* msg = factory->messageByValue(_argv[i]);
	    msg->serialize(stream);
	    delete msg;
	}
    }

  
  protected:
    int32_t       _objectId;
    std::string   _method;
    List          _argv;
};

#endif

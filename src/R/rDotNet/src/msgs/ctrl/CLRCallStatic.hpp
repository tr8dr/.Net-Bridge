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

#ifndef CLR_CALL_STATIC
#define CLR_CALL_STATIC

#include <cstdlib>
#include <Rcpp.h>
#include "CLRFactory.hpp"

using namespace std;


//
//  Call static method Message
//
class CLRCallStatic : public CLRMessage
{
  public:
  
    CLRCallStatic (CLRApi* api, const std::string klass, const std::string& method, const List& argv)
      : CLRMessage(CLRMessage::TypeCallStaticMethod, api), _class(klass), _method(method), _argv(argv) { }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        CLRMessage::serialize (stream);
	stream.write_string(_class);
	stream.write_string(_method);

	// argv
	int argc = _argv.size();
	stream.write_int16((short)argc);

	CLRFactory* factory = _api->factory();
	for (int i = 0 ; i < argc ; i++)
	{
	    CLRMessage* msg = factory->messageByValue(_argv[i]);
	    msg->serialize(stream);
	    delete msg;
	}
    }

  
  protected:
    std::string   _class;
    std::string   _method;
    List          _argv;
};

#endif

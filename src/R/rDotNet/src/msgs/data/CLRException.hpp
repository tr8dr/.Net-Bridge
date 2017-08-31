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

#ifndef CLR_EXCEPTION
#define CLR_EXCEPTION

#include <cstdlib>
#include "msgs/CLRValue.hpp"

using namespace std;


//
// Exception value
//
class CLRException : public CLRValue<std::string>
{
  public:

    CLRException (CLRApi* api, std::string* value = nullptr)
      : CLRValue(CLRMessage::TypeException, api, value)
    {
    }

    // throw exception instead of returning text of exception 
    RValue rvalue()
    {
        throw std::runtime_error (*_value);
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        assert (_value != NULL);
        CLRMessage::serialize (stream);
	stream.write_string (*_value);
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
        _value = new std::string;
	*_value = stream.read_string();
    }
};

#endif

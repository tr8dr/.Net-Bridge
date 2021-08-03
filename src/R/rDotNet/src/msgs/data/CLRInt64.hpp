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


#ifndef CLR_INT64
#define CLR_INT64

#include <cstdlib>
#include "msgs/CLRValue.hpp"

using namespace std;


//
// Int64 value
//
class CLRInt64 : public CLRValue<int64_t>
{
  public:

    CLRInt64 (CLRApi* api, int64_t* value = nullptr)
      : CLRValue(CLRMessage::TypeInt64, api, value)
    {
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        assert (_value != NULL);
        CLRMessage::serialize (stream);
	stream.write_int64 (*_value);
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
        _value = new int64_t;
	*_value = stream.read_int64();
    }
};

#endif

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


#ifndef CLR_NULL
#define CLR_NULL

#include <cstdlib>
#include "msgs/CLRValue.hpp"

using namespace std;


//
// Null value
//
class CLRNull : public CLRValue<int>
{
  public:

    CLRNull (CLRApi* api)
      : CLRValue(CLRMessage::TypeNull, api)
    {
    }

    // R value associated with this message
    virtual RValue rvalue()
    {
        return R_NilValue;
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        CLRMessage::serialize (stream);
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
    }
};

#endif

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

#ifndef CLR_OBJECT
#define CLR_OBJECT

#include <Rcpp.h>
#include <cstdlib>
#include "Common.hpp"
#include "msgs/CLRMessage.hpp"

using namespace std;


//
// Object Reference (uses a list object to hold information about object)
//
class CLRObjectRef : public CLRMessage
{
  public:

    CLRObjectRef (CLRApi* api, SEXP obj = nullptr)
      : CLRMessage(CLRMessage::TypeObject, api), _object(obj) { }

    // R value associated with this message
    RValue rvalue();

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream);
  
    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream);

  protected:

    RValue  _object;
};

#endif

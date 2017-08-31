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

#ifndef CLR_GETPROP
#define CLR_GETPROP

#include <cstdlib>
#include <Rcpp.h>
#include "CLRFactory.hpp"

using namespace std;


//
//  Get Property Message
//
class CLRGetProperty : public CLRMessage
{
  public:
  
    CLRGetProperty (CLRApi* api, int objectId, const std::string& property)
      : CLRMessage(CLRMessage::TypeGetProperty, api), _objectId(objectId),
	_property(property) { }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        CLRMessage::serialize (stream);
	stream.write_int32(_objectId);
	stream.write_string(_property);
    }
  
  protected:
    int           _objectId;
    std::string   _property;
};

#endif

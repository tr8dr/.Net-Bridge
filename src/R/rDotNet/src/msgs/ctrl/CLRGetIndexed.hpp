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

#ifndef CLR_GET_INDEXED
#define CLR_GET_INDEXED

#include <cstdlib>
#include <Rcpp.h>
#include "CLRFactory.hpp"

using namespace std;


//
//  Get indexed value on object
//
class CLRGetIndexed : public CLRMessage
{
  public:
  
    CLRGetIndexed (CLRApi* api, int objectId, int ith)
      : CLRMessage(CLRMessage::TypeGetIndexed, api), _objectId(objectId),
	_ith(ith) { }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        CLRMessage::serialize (stream);
	stream.write_int32(_objectId);
	stream.write_int32(_ith);
    }
  
  protected:
    int      _objectId;
    int      _ith;
};

#endif

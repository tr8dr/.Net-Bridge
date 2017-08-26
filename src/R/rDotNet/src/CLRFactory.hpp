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

#ifndef CLR_FACTORY
#define CLR_FACTORY

#include <cstdlib>
#include "Common.hpp"
#include "msgs/CLRMessage.hpp"

using namespace std;

class CLRApi;

//
// CLR Factory
//
class CLRFactory
{
  public:

    CLRFactory (CLRApi* api)
      : _api(api) {}
  
    // create message based on incoming message ID 
    CLRMessage* messageById (char mtype);

    // create message based on R object type
    CLRMessage* messageByValue (const RObject& robj);

  private:
    CLRApi* _api;
};


#endif

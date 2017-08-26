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

#ifndef CLR_VALUE
#define CLR_VALUE

#include <cstdlib>
#include <Rcpp.h>
#include "msgs/CLRMessage.hpp"

using namespace std;


//
// CLR Message base class
//
template <class T> class CLRValue : public CLRMessage
{
  public:
  
    CLRValue (char mtype, CLRApi* api, T* value = nullptr)
      : CLRMessage(mtype, api), _value(value)
    {
    }

    ~CLRValue ()
    {
      if (_value != NULL) delete _value;
    }
  
    // R value associated with this message
    RValue rvalue()
    {
        if (_value != NULL)
	    return RValue (Rcpp::wrap (*_value));
	else
	    throw std::runtime_error ("CLRMessage: no value assigned to message");
    }
  
  protected:
    T*   _value;
};

#endif

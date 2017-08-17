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

#ifndef CLR_MESSAGE
#define CLR_MESSAGE

#include <cstdlib>
#include "io/BufferedSocketReader.hpp"
#include "io/BufferedSocketWriter.hpp"

using namespace std;

class CLRApi;

//
// CLR Message base class
//
class CLRMessage
{
  public:

    static const short Magic                 = 0xd00d;
  
  static const char TypeNull                 = (char)0;
    static const char TypeBool               = (char)1;
    static const char TypeByte               = (char)2;
    static const char TypeInt32              = (char)5;
    static const char TypeInt64              = (char)6;
    static const char TypeFloat64            = (char)7;
    static const char TypeString             = (char)8;
    static const char TypeObject             = (char)9;

    static const char TypeVector             = (char)21;
    static const char TypeMatrix             = (char)22;
    static const char TypeException          = (char)23;

    static const char TypeBoolArray          = (char)101;
    static const char TypeByteArray          = (char)102;
    static const char TypeInt32Array         = (char)105;
    static const char TypeInt64Array         = (char)106;
    static const char TypeFloat64Array       = (char)107;
    static const char TypeStringArray        = (char)108;
    static const char TypeObjectArray        = (char)109;
  
    static const char TypeCreate             = (char)201;
    static const char TypeCallStaticMethod   = (char)202;
    static const char TypeCallMethod         = (char)203;
    static const char TypeGetProperty        = (char)204;
    static const char TypeGetIndexedProperty = (char)205;
    static const char TypeGetIndexed         = (char)206;
    static const char TypeSetProperty        = (char)207;
    static const char TypeGetStaticProperty  = (char)208;
    static const char TypeSetStaticProperty  = (char)209;
    static const char TypeProtect            = (char)210;
    static const char TypeRelease            = (char)211;
    static const char TypeTemplateReq        = (char)212;
    static const char TypeTemplateReply      = (char)213;
  
    CLRMessage (char mtype, CLRApi* api)
      : _mtype(mtype), _api(api) { }

    virtual ~CLRMessage() { }

    // type of message
    char type()
    {
        return _mtype;
    }

    // R value associated with this message
    virtual RValue rvalue()
    {
        throw std::runtime_error("CLRMessage: no value associated with message");
    }

    // serialize object to stream
    virtual void serialize (BufferedSocketWriter& stream)
    {
        // magic sequence opener
        stream.write_int16 (CLRMessage::Magic);
	// type
	stream.write_byte (_mtype);
    }

    // deserialize object from stream
    virtual void deserialize (BufferedSocketReader& stream)
    {
    }
  
  protected:
    char    _mtype;
    CLRApi* _api;
};

#endif

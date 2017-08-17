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


#include <cstdlib>
#include "Common.hpp"
#include "msgs/CLRMessage.hpp"
#include "CLRObjectRef.hpp"
#include "CLRFactory.hpp"
#include "msgs/data/CLRBool.hpp"
#include "msgs/data/CLRBoolArray.hpp"
#include "msgs/data/CLRByte.hpp"
#include "msgs/data/CLRException.hpp"
#include "msgs/data/CLRFloat64.hpp"
#include "msgs/data/CLRFloat64Array.hpp"
#include "msgs/data/CLRInt32.hpp"
#include "msgs/data/CLRInt32Array.hpp"
#include "msgs/data/CLRInt64.hpp"
#include "msgs/data/CLRMatrix.hpp"
#include "msgs/data/CLRNull.hpp"
#include "msgs/data/CLRString.hpp"
#include "msgs/data/CLRStringArray.hpp"
#include "msgs/data/CLRVector.hpp"
#include "msgs/data/CLRObjectArray.hpp"

using namespace std;


//
//  CLR Message unknown exception
//
struct CLRUnknownMessageException : std::exception
{
  public:
    explicit CLRUnknownMessageException (char mtype)
      : _mtype (mtype) {}
  
    char const* what() const throw()
    {
      sprintf((char*)_msg, "CLRMessage: unknown message type: %d", (int)((unsigned char)_mtype));
        return _msg;
    }

  private:
    char     _mtype;
    char     _msg[64];
};



//
// create message based on incoming message ID
//
CLRMessage* CLRFactory::messageById (char mtype)
{
    switch (mtype)
    {
    case CLRMessage::TypeNull:
        return new CLRNull (_api);
    case CLRMessage::TypeBool:
        return new CLRBool (_api);
   case CLRMessage::TypeByte:
       return new CLRByte (_api);
    case CLRMessage::TypeInt32:
        return new CLRInt32 (_api);
    case CLRMessage::TypeInt64:
        return new CLRInt64 (_api);
    case CLRMessage::TypeFloat64:
        return new CLRFloat64 (_api);
    case CLRMessage::TypeString:
        return new CLRString (_api);
    case CLRMessage::TypeObject:
        return new CLRObjectRef (_api);
	   
    case CLRMessage::TypeVector:
        return new CLRVector (_api);
    case CLRMessage::TypeMatrix:
        return new CLRMatrix (_api);
    case CLRMessage::TypeException:
        return new CLRException (_api);

    case CLRMessage::TypeBoolArray:
        return new CLRBoolArray (_api);
    case CLRMessage::TypeByteArray:
        throw std::runtime_error ("CLRMessage: R does not support byte arrays");
    case CLRMessage::TypeInt32Array:
        return new CLRInt32Array (_api);
    case CLRMessage::TypeInt64Array:
        throw std::runtime_error ("CLRMessage: R does not support int64 arrays");
    case CLRMessage::TypeFloat64Array:
        return new CLRFloat64Array (_api);
    case CLRMessage::TypeStringArray:
        return new CLRStringArray (_api);
    case CLRMessage::TypeObjectArray:
      return new CLRObjectArray (_api);
	   
    case CLRMessage::TypeCallMethod:
        throw std::runtime_error ("CLRMessage: should never receive a CLRCallMethod msg");
	    
    default:
        throw CLRUnknownMessageException (mtype);
    }
}


//
//  create message for string(1) class
//
static CLRMessage* messageForString1 (CLRApi* api, const RObject& robj)
{
    std::string* v = new std::string (Rcpp::as<std::string>(robj));
    return new CLRString (api, v);
}

//
//  create message for string(*) class
//
static CLRMessage* messageForStrings (CLRApi* api, const RObject& robj)
{
    CharacterVector vec (robj.get__());
    if (vec.size() == 1) 
        return new CLRString (api, new std::string(vec[0]));
    else
        return new CLRStringArray(api, new CharacterVector(vec));
}

//
//  create message for bool(*) class
//
static CLRMessage* messageForLogicals (CLRApi* api, const RObject& robj)
{
    LogicalVector vec (robj.get__());
    if (vec.size() == 1)
    {
        bool* v = new bool;
	*v = vec[0];
        return new CLRBool (api, v);
    } else
        return new CLRBoolArray(api, new LogicalVector(vec));
}

//
//  create message for int(*) class
//
static CLRMessage* messageForIntegers (CLRApi* api, const RObject& robj)
{
    IntegerVector vec (robj.get__());
    if (vec.size() == 1)
    {
        int* v = new int;
	*v = vec[0];
        return new CLRInt32 (api, v);
    } else
        return new CLRInt32Array(api, new IntegerVector(vec));
}


//
//  create message for matrix
//
static CLRMessage* messageForMatrix (CLRApi* api, const RObject& robj)
{
    NumericMatrix* mat = new NumericMatrix(robj.get__());
    return new CLRMatrix(api, mat);
}


//
//  create message for double(*) class
//
static CLRMessage* messageForFloats (CLRApi* api, const RObject& robj)
{
    SEXP edim = robj.attr("dim");
    if (!Rf_isNull(edim))
        return messageForMatrix (api, robj);
    
    NumericVector vec (robj.get__());    
    if (vec.size() == 1)
    {
        double* v = new double;
	*v = vec[0];
        return new CLRFloat64 (api, v);
    } else
        return new CLRVector(api, new NumericVector(vec));
}


//
//  determine if object reference
//
static bool isObjectRef (CLRApi* api, const RObject& robj)
{
    SEXP objid = robj.attr("ObjectId");
    return !Rf_isNull(objid);
}

//
//  create message for object
//
static CLRMessage* messageForObject (CLRApi* api, const RObject& robj)
{
    return new CLRObjectRef(api, robj.get__());
}

//
//  create message for object list
//
static CLRMessage* messageForObjectList (CLRApi* api, const RObject& robj)
{
    return new CLRObjectArray(api, new List(robj.get__()));
}

//
// create message based on R object type
//
CLRMessage* CLRFactory::messageByValue (const RObject& robj)
{
    int stype = robj.sexp_type();
    switch (stype)
    {
    case NILSXP:
        return new CLRNull(_api);  
    case CHARSXP:
      return messageForString1 (_api, robj);
    case LGLSXP:
        return messageForLogicals (_api, robj);
    case INTSXP:
        return messageForIntegers (_api, robj);
    case REALSXP:
        return messageForFloats (_api, robj);
    case STRSXP:
        return messageForStrings (_api, robj);
    case SYMSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R symbol type");
    case LISTSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R dotted pairs type");
    case CLOSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R closure type");
    case ENVSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R environ type");
    case PROMSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R promise type");
    case LANGSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R language construct type");
    case SPECIALSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R special form type");
    case BUILTINSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R built-in special type");
    case CPLXSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R complex type");
    case DOTSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R dot-dot-dot type");
    case ANYSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R ANY type");
    case VECSXP:
        if (isObjectRef (_api, robj))
            return messageForObject(_api, robj);
	else
	    return messageForObjectList(_api, robj);
    case EXPRSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R expression type");
    case BCODESXP:
        throw std::runtime_error ("CLRMessage: cannot handle R bytecode type");
    case EXTPTRSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R external-pointer type");
    case WEAKREFSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R weak-reference type");
    case RAWSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R raw-bytes type");
    case S4SXP:
        throw std::runtime_error ("CLRMessage: cannot handle R S4 type");
    case FUNSXP:
        throw std::runtime_error ("CLRMessage: cannot handle R closure type");
    default:
        throw std::runtime_error ("CLRMessage: unknown R type");
    }
}

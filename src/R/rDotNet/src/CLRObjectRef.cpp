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

#include <Rcpp.h>
#include <cstdlib>
#include "CLRObjectRef.hpp"
#include "CLRApi.hpp"

#undef TRUE
#undef FALSE

using namespace std;

//
// GC Handle
//
struct CLRObjectGC
{
    CLRObjectGC(int objectId, CLRApi* api) : ObjectId(objectId), API(api) {}

    int      ObjectId;
    CLRApi*  API;
};


// release object handle
static void ObjectFinalizer (SEXP sptr)
{
     if (TYPEOF(sptr) != EXTPTRSXP)
         throw std::runtime_error ("bad finalizer pointer sent in rDotNet gc()");

     // retrieve .NET GC handle (not really a pointer)
     CLRObjectGC* xgc = (CLRObjectGC*)((void*)R_ExternalPtrAddr (sptr));

     // inform API that object done
     xgc->API->release (xgc->ObjectId);
     delete xgc;
}


// R value associated with this message
RValue CLRObjectRef::rvalue()
{
    return _object;
}

// serialize object to stream
void CLRObjectRef::serialize (BufferedSocketWriter& stream)
{
    CLRMessage::serialize (stream);

    List list(_object);
    SEXP eId = list.attr("ObjectId");
    if (Rf_isNull(eId))
        throw std::runtime_error ("CLRMessage: object reference missing object ID");
	
    stream.write_int32 (Rcpp::as<int>(eId));
    stream.write_byte(0);
}

// deserialize object from stream
void CLRObjectRef::deserialize (BufferedSocketReader& stream)
{
    List vobj;
    vobj.attr("class") = "rDotNet";

    // object ID
    int objectId = stream.read_int32();
    vobj.attr("ObjectId") = objectId;

    // class name
    if (stream.read_byte() != 0)
        vobj.attr("Classname") = stream.read_string();

    // setup garbage collection
    CLRObjectGC* gc = new CLRObjectGC(objectId, _api);
    SEXP xgc = PROTECT(R_MakeExternalPtr((void*)gc, R_NilValue, R_NilValue));
    vobj.attr("gc") = xgc;

    R_RegisterCFinalizerEx(xgc, ObjectFinalizer, Rboolean::TRUE);

    _object = RValue(Rcpp::wrap(vobj));
    UNPROTECT(1);
}


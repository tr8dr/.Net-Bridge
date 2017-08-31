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

// [[Rcpp::depends(BH)]]

#ifndef CLR_VECTOR
#define CLR_VECTOR

#include <Rcpp.h>
#include <cstdlib>
#include "msgs/CLRValue.hpp"

using namespace std;


//
// Vector value
//
class CLRVector : public CLRValue<Rcpp::NumericVector>
{
  public:

    CLRVector (CLRApi* api, Rcpp::NumericVector* value = nullptr)
      : CLRValue(CLRMessage::TypeVector, api, value)
    {
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        assert (_value != NULL);
        CLRMessage::serialize (stream);

	Rcpp::RObject rnames = _value->names();
	int len = _value->size();
	if (!rnames.isNULL())
	{
	    Rcpp::StringVector names (rnames.get__());
	    stream.write_int32(len);
	    for (int i = 0 ; i < len; i++)
	        stream.write_string(names[i]);

	    stream.write_int32(len);
	    for (int i = 0 ; i < len; i++)
	        stream.write_float64((*_value)[i]);	    
	}
	else
	{
	    stream.write_int32(0);
	    stream.write_int32(len);
	    for (int i = 0 ; i < len; i++)
	        stream.write_float64((*_value)[i]);	    
	}
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
        _value = new Rcpp::NumericVector();
	int ilen = stream.read_int32();
	if (ilen > 0)
	{
	    std::vector<std::string> index(ilen);
	    for (int i = 0 ; i < ilen ; i++)
	        index[i] = stream.read_string();

	    int vlen = stream.read_int32();
	    assert(ilen == vlen);

	    for (int i = 0 ; i < vlen ; i++)
	        _value->push_back(stream.read_float64(), index[i]);
	}
	else
	{
	    int vlen = stream.read_int32();
	    for (int i = 0 ; i < vlen ; i++)
	        _value->push_back(stream.read_float64());
	}
    }
};

#endif

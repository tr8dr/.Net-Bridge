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

#ifndef CLR_MATRIX
#define CLR_MATRIX

#include <Rcpp.h>
#include <cstdlib>
#include "msgs/CLRValue.hpp"

using namespace std;
using namespace Rcpp;


//
// Matrix value
//
class CLRMatrix : public CLRValue<Rcpp::NumericMatrix>
{
  public:

    CLRMatrix (CLRApi* api, Rcpp::NumericMatrix* value = nullptr)
      : CLRValue(CLRMessage::TypeMatrix, api, value)
    {
    }

    // serialize object to stream
    void serialize (BufferedSocketWriter& stream)
    {
        assert (_value != NULL);
        CLRMessage::serialize (stream);

	const NumericMatrix& mat = *_value;
	
	// get row and column names
	Rcpp::Function rownames("rownames");
	Rcpp::Function colnames("colnames");

	RObject rn = rownames(mat);
	RObject cn = colnames(mat);

	// output row names if existant
	if (!rn.isNULL())
	{
	    StringVector rnames(rn.get__());
	    stream.write_int32(rnames.size());
	    for (int i = 0; i < rnames.size(); i++)
	        stream.write_string(rnames[i]);
	} else
	    stream.write_int32(0);

	// output column names if existant
	if (!cn.isNULL())
	{
	    StringVector cnames(cn.get__());
	    stream.write_int32(cnames.size());
	    for (int i = 0; i < cnames.size(); i++)
	        stream.write_string(cnames[i]);
	} else
	    stream.write_int32(0);

	// write out dimensions
	int nrow = mat.nrow();
	int ncol = mat.ncol();
	stream.write_int32(nrow);
	stream.write_int32(ncol);

	// write data
        for (int ci = 0 ; ci < ncol ; ci++)
        {
            for (int ri = 0 ; ri < nrow ; ri++)
            {
	        stream.write_float64 (mat(ri,ci));
	    }
        }
    }

    // deserialize object from stream
    void deserialize (BufferedSocketReader& stream)
    {
        // read matrix indices
        CharacterVector rn;
        CharacterVector cn;
      
	int rnlen = stream.read_int32();
	if (rnlen > 0)
	{
	    for (int i = 0 ; i < rnlen ; i++)
	        rn.push_back (stream.read_string());
	}
	
	int cnlen = stream.read_int32();
	if (cnlen > 0)
	{
	    for (int i = 0 ; i < cnlen ; i++)
	        cn.push_back (stream.read_string());
	}

	// read matrix dimensions
	int nrow = stream.read_int32();
	int ncol = stream.read_int32();

	// create
	_value = new NumericMatrix (nrow, ncol);
	NumericMatrix& mat = *_value;

        for (int ci = 0 ; ci < ncol ; ci++)
        {
            for (int ri = 0 ; ri < nrow ; ri++)
            {
	        double v = stream.read_float64 ();
		mat(ri,ci) = v;
	    }
        }

	// assign matrix indices if they exist
	List dimnames = Rcpp::List::create(
	    rn.size() > 0 ? rn.get__() : R_NilValue,
	    cn.size() > 0 ? cn.get__() : R_NilValue);

	mat.attr("dimnames") = dimnames;
    }
};

#endif

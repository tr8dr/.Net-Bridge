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
#include "Common.hpp"
#include "CLRApi.hpp"

using namespace Rcpp;

static CLRApi* api = NULL;


// [[Rcpp::export]]
void cinit(const std::string& host, int port)
{
    api = new CLRApi (host.c_str(), port);
}


// [[Rcpp::export]]
SEXP cnew (const std::string& classname, const List& argv)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    return api->create (classname, argv);
}

// [[Rcpp::export]]
SEXP ccall_static (const std::string& classname, const std::string& method, const List& argv)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    return api->callstatic (classname, method, argv);
}

// [[Rcpp::export]]
SEXP ccall (SEXP obj, const std::string& method, const List& argv)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    return api->call (obj, method, argv);
}

// [[Rcpp::export]]
SEXP cget (SEXP obj, const std::string& property)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    return api->get (obj, property);
}

// [[Rcpp::export]]
void cset (SEXP obj, const std::string& property, const RObject& value)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    api->set (obj, property, value);
}


// [[Rcpp::export]]
SEXP cget_indexed (SEXP obj, int ith)
{
    if (api == NULL)
        cinit ("localhost", 56789);
	       
    return api->get_indexed (obj, ith);
}

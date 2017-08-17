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

#ifndef R_VALUE
#define R_VALUE

#include <Rcpp.h>


//
//  GC Protected embodiment of R value
//
class RValue
{
  public:
    RValue (SEXP e)
      : _e(e), _count(new int(1))
    {
        if (e)
	    PROTECT(e);
    }

    RValue ()
      : _e(nullptr), _count(nullptr) { }

    RValue (const RValue& o)
      : _e(o._e), _count(o._count)
    {
        (*_count)++;
    }

    ~RValue()
    {
        remove();
    }

    // return underlying value
    operator SEXP () const
    {
        return _e;
    }

    // assignment 
    RValue& operator= (const RValue& o)
    {
        if (this != &o)
	{
	    remove();
	    _count = o._count;
	    (*_count)++;
	    _e = o._e;
	}

	return *this;
    }

  private:
  
    void remove ()
    {
        if (_count == nullptr)
	    return;
        if (--(*_count) > 0)
	    return;
       
	delete _count;
	if (!_e)
	    return;
	
	UNPROTECT(1);
    }
  
  private:
    SEXP    _e;
    int*    _count;
};

#endif

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

#ifndef FILEUTILS
#define FILEUTILS

#include <cstdlib>#include <boost/asio.hpp>
using namespace std;


class FileUtils
{
    // expand a path with ~ 
    static std::string expand_user (const std::string& path)
    {
        if (path.size() > 0 && path[0] == '~')
	{
            char const* home = getenv("HOME");
	    std::stringstream ss;
	    ss << home << path.substr(1,path.size()-1);
	    return ss.str();
	} else
	    return path;
    } 
};

#endif

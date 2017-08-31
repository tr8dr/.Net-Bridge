#
# General:
#      This file is part of .NET Bridge
#
# Copyright:
#      2010 Jonathan Shore
#      2017 Jonathan Shore and Contributors
#
# License:
#      Licensed under the Apache License, Version 2.0 (the "License");
#      you may not use this file except in compliance with the License.
#      You may obtain a copy of the License at:
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
#      Unless required by applicable law or agreed to in writing, software
#      distributed under the License is distributed on an "AS IS" BASIS,
#      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#      See the License for the specific language governing permissions and
#      limitations under the License.
#

## do our special magic here.  May need to build the .NET server
.onAttach = function (libname, pkgname)
{
    packagedir <- path.package("rDotNet")
    server <- sprintf("%s/server/bin/Debug/CLRServer.exe", packagedir)

    ## check to see if was compiled
    if (file.exists(server))
        return()

    packageStartupMessage ("attempting to build CLR server, one time setup")
    if (Sys.which("nuget") == "")
    {
        warning ("could not find nuget in path; will not be able to use rDotNet unless corrected and rebuilt")
        return ()
    }
    
    if (Sys.which("msbuild") == "" && Sys.which("xbuild") == "")
    {
        warning ("could not find msbuild or xbuild in path; will not be able to use rDotNet unless corrected and rebuilt")
        return()
    }
    
    cwd <- getwd()
    setwd(sprintf("%s/server", packagedir))

    packageStartupMessage ("getting dependent packages")
    system2 ("nuget", "restore", wait=TRUE, stderr=TRUE, stdout=TRUE)
        
    packageStartupMessage ("building project")
    system2 (ifelse(Sys.which("msbuild") != "", "msbuild", "xbuild"), wait=TRUE, stderr=TRUE, stdout=TRUE)

    setwd(cwd)
}

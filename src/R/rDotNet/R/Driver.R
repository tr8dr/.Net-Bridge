#
# General:
#      This file is pary of .NET Bridge
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


## closure for initialization
.initialize <- (function ()
{
    initialized <- FALSE

    expand.dll <- function (dll)
    {
        fullpath <- path.expand(dll)
        if (file.exists(fullpath))
            fullpath
        else
            stop (sprintf("cannot find dll: %s", fullpath))
    }

    or <- function (a,b)
    {
        if (is.null(a) || a == "") b else a
    }

    
    function (host = "localhost", port = 56789, dll=NULL, server.args=NULL)
    {
        if (initialized)
            return()

        ## test to see whether there is a CLR process already running
        if (internal_ctest_connection (host, port))
        {
            message ("CLR server already running; terminate the CLRserver process if restarting with a different DLL is desired")
        }

        ## otherwise start server
        else
        {
            if (.Platform$OS.type != "windows")
            {
                paths <- c("/usr/bin/mono64","/usr/local/bin/mono64","/Library/Frameworks/Mono.framework/Commands/mono64")
                mono <- paths[sapply(paths, file.exists)][1]
                if (is.null(mono))
                    stop ("could not find mono64")
            }
        
            packagedir <- path.package("rDotNet")
            server <- sprintf("%s/server/CLRServer.exe", packagedir)
        
            dll.env <- or (Sys.getenv("RDOTNET_DLL"), Sys.getenv("rDotNet_DLL"))
            if (!is.null(dll))
                server.args <- c(server.args, "-dll", expand.dll(dll))
            else if (dll.env != "")
                server.args <- c(server.args, "-dll", expand.dll(dll.env))
            
            args <- (if (.Platform$OS.type == "windows")
                c("-url", sprintf("svc://%s:%d/", host, port), server.args)
            else
                c("--llvm", server, "-url", sprintf("svc://%s:%d/", host, port), server.args))

            exe <- (if (.Platform$OS.type == "windows")
                server
            else
                mono)

            system2 (exe, args, wait=FALSE, stderr=FALSE, stdout=FALSE)
        }
        
        internal_cinit(host, port)
        initialized <<- TRUE
    }
    
}) ()


## initialize CLR
.cinit <- function (host = "localhost", port = 56789, dll=NULL, server.args=NULL)
{
    .initialize (host, port, dll, server.args)
}


## create new object from class
.cnew <- function (classname, ...)
{
    .initialize()	       
    argv = list(...)
    internal_cnew(classname, argv)
}

## call static method on class
.cstatic <- function (classname, method, ...)
{
    .initialize()	       
    argv = list(...)
    internal_ccall_static(classname, method, argv)
}

## create object through string ctor
.ctor <- function (ctor)
{
    .initialize() 
    internal_ccall_static("bridge.common.reflection.Creator","NewByCtor", list(ctor))
}

## call method on object
.ccall <- function (obj, method, ...)
{
    argv = list(...)
    internal_ccall(obj, method, argv)
}

## get property value
.cget <- function (obj, property)
{
    internal_cget(obj, property)
}

## set property value
.cset <- function (obj, property, value)
{
    internal_cset(obj, property, value)
}


##  Method accessor for objects
`$.rDotNet` <- function (obj,fun)
{
    switch (fun,
       Get = function(property) internal_cget(obj, property),
       Set = function(property,value) internal_cset(obj, property, value),
       function (...) {
           v <- internal_ccall(obj, fun, list(...))
           if (is.null(v))
               invisible(v)
           else
               v
       }
    )
}

## indexer
`[.rDotNet` <- function (obj,ith)
{
    internal_cget_indexed(obj, ith)
}


## to string
print.rDotNet <- function (obj, ...)
{
    tostr <- internal_ccall(obj, "ToString", list())
    objId <- attr(obj,'ObjectId')
    klass <- attr(obj, 'Classname')
    cat (sprintf("<dotnet obj: %d, class: %s, value: \"%s\">\n", objId, klass, tostr))
}

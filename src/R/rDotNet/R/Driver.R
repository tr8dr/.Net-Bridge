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


#' Initialize R <-> .NET bridge
#'
#' The function either connects to an existing running CLR bridge process at the given host:port or
#' instantiates a new CLR server at the current host with the given port and server arguments.
#'
#' If the .NET libraries are to be changed, the CLRServer process and R should be restarted.  CLR references
#' in the R session are only valid for the current CLR server instance. 
#'
#' Note that \code{.cinit} should only be called once per R session.  Calling multiple times will invalidate
#' any objects created in between prior calls.
#'
#' @param host The host machine on which the CLR bridge server is running; generally this
#'   is the localhost, which is the default.
#' @param port The port on which the CLR bridge is listening (default: 56789)
#' @param dll The path to an optional library dll to be loaded by the server.  This dll would contain .NET classes
#'   and functions one wants to call from R.
#' @param server.args Optional parameters to the CLRServer process (CLRServer.exe -help to list the options).
#'
#' @examples
#' \dontrun{
#' # create .NET bridge server, loading personal library to be referenced in the R session
#' .cinit (dll="~/Dev/MyLibrary.dll")
#' }
.cinit <- function (host = "localhost", port = 56789, dll=NULL, server.args=NULL)
{
    packagedir <- path.package("rDotNet")
    server <- sprintf("%s/server/CLRServer.exe", packagedir)

    if (!is.null(dll))
        server.args <- c(server.args, "-dll", path.expand(dll))
    
    args <- (if (.Platform$OS.type == "windows")
            c("-url", sprintf("svc://%s:%d/", host, port, server, server.args))
        else
            c("-llvm", server, "-url", sprintf("svc://%s:%d/", host, port), server.args))

    exe <- (if (.Platform$OS.type == "windows")
            server
        else
            "mono")
    
    system2 (exe, args, wait=FALSE, stderr=F, stdout=F) 
    invisible(.Call("rDotNet_cinit", PACKAGE='rDotNet', host, port))
}


## create new object from class
.cnew <- function (classname, ...)
{
    .initialize()	       
    argv = list(...)
    rDotNet_cnew(classname, argv)
}

## call static method on class
.cstatic <- function (classname, method, ...)
{
    .initialize()	       
    argv = list(...)
    rDotNet_ccall_static(classname, method, argv)
}

## create object through string ctor
.ctor <- function (ctor)
{
    .initialize() 
    rDotNet_ccall_static("bridge.common.reflection.Creator","NewByCtor", list(ctor))
}

## call method on object
.ccall <- function (obj, method, ...)
{
    argv = list(...)
    rDotNet_ccall(obj, method, argv)
}

## get property value
.cget <- function (obj, property)
{
    rDotNet_cget(obj, property)
}

## set property value
.cset <- function (obj, property, value)
{
    rDotNet_cset(obj, property, value)
}


##  Method accessor for objects
`$.rDotNet` <- function (obj,fun)
{
    switch (fun,
       Get = function(property) rDotNet_cget(obj, property),
       Set = function(property,value) rDotNet_cset(obj, property, value),
       function (...) {
           v <- rDotNet_ccall(obj, fun, list(...))
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
    rDotNet_cget_indexed(obj, ith)
}


## to string
print.rDotNet <- function (obj, ...)
{
    tostr <- rDotNet_ccall(obj, "ToString", list())
    objId <- attr(obj,'ObjectId')
    klass <- attr(obj, 'Classname')
    cat (sprintf("<dotnet obj: %d, class: %s, value: \"%s\">\n", objId, klass, tostr))
}

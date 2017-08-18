\name{.cinit}
\alias{.cinit}
\title{Initialize R <-> .NET bridge}
\usage{
.cinit(host='localhost', port=56789, dll=NULL, server.args=NULL)
}
\arguments{
\item{host}{The host machine on which the CLR bridge server is running; generally this
is the localhost, which is the default.}

\item{port}{The port on which the CLR bridge is listening (default: 56789)}

\item{dll}{The path to an optional library dll to be loaded by the server.  This dll would contain .NET classes
and functions one wants to call from R.}

\item{server.args}{Optional parameters to the CLRServer process (CLRServer.exe -help to list the options).}
}
\description{
The function either connects to an existing running CLR bridge process at the given host:port or
instantiates a new CLR server at the current host with the given port and server arguments.
}
\details{
If the .NET libraries are to be changed, the CLRServer process and R should be restarted.  CLR references
in the R session are only valid for the current CLR server instance.

Instead of calling \code{.cinit(dll="~/mydll.dll")} explictly one can set an environment variable \code{Sys.setenv(rDotNet_DLL="~/mydll.dll")} and use \code{.cnew()} and other functions after loading the package as opposed to first calling \code{.cinit}.  One can also run the \code{CLRServer} from the command line or an IDE with the appropriate DLL.
}
\examples{
\dontrun{

## create .NET bridge server, loading personal library to be referenced in the R session
.cinit (dll="~/Dev/MyLibrary.dll")
obj <- .cnew("NormalDistribution1D", 0.0, 1.0)

## alternative without explicit initialization
Sys.setenv(rDotNet_DLL="~/Dev/models.dll")
...
obj <- .cnew("NormalDistribution1D", 0.0, 1.0)

}}
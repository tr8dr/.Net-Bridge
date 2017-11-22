\name{.cinit}
\alias{.cinit}
\title{Initialize R <-> .NET bridge}
\usage{
.cinit(host='localhost', port=56789, dlls=NULL, server.args=NULL)
}
\arguments{
\item{host}{The host machine on which the CLR bridge server is running; generally this
is the localhost, which is the default.}

\item{port}{The port on which the CLR bridge is listening (default: 56789)}

\item{dlls}{The path to one or more optional library dlls to be loaded by the server.  These dlls would contain .NET classes
and functions one wants to call from R.}

\item{server.args}{Optional parameters to the CLRServer process (CLRServer.exe -help to list the options).}
}
\description{
The function either connects to an existing running CLR bridge process at the given host:port or
instantiates a new CLR server on the local computer with the given port and server arguments.
}
\details{
If the .NET libraries are to be changed, the CLRServer process and R should be restarted.  CLR references
in the R session are only valid for the current CLR server instance.

Instead of calling \code{.cinit(dlls=c("~/mydll.dll", "~/myother.dll"))} explictly one can set an environment variable 
\code{Sys.setenv(rDotNet_DLL="~/mydll.dll;~/myother.dll")} and use \code{.cnew()} and other functions after loading 
the package as opposed to first calling \code{.cinit}.  One can also run the \code{CLRServer} from the command line or an IDE with the appropriate DLL.
}
\examples{
\dontrun{

## create .NET bridge server, loading personal library to be referenced in the R session
.cinit (dlls="~/Dev/MyLibrary.dll")
obj <- .cnew("NormalDistribution1D", 0.0, 1.0)

## alternative without explicit initialization (this variable can be set outside of R)
Sys.setenv(rDotNet_DLL="~/Dev/models.dll;~/Dev/otherstuff.dll")
#...
obj <- .cnew("NormalDistribution1D", 0.0, 1.0)

}}

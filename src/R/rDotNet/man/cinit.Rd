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

Note that \code{.cinit} should only be called once per R session.  Calling multiple times will invalidate
any objects created prior to the latest invocation of \code{.cinit}.
}
\examples{
\dontrun{
## create .NET bridge server, loading personal library to be referenced in the R session
.cinit (dll="~/Dev/MyLibrary.dll")
}

}

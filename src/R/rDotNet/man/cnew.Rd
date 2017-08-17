\name{.cnew}
\alias{.cnew}
\title{Create .NET object for class of given name and arguments}
\usage{
.cnew(classname, ...)
}
\arguments{
\item{classname}{The name of the .NET based class to be created.  Can either be a fully qualified name like "com.stg.models.DickeyFuller" or "DickeyFuller" if the classname is unique in the VM.}

\item{...}{a sequence of arguments to the constructor for this class.  The arguments can either be fundamental types, vectors, matrices, or other objects.  Fuzzy matching will be applied, allowing imperfectly matched argument types to be converted, as may be needed given R's more basic type system.}
}
\description{
A .NET object will be created and a reference / R object will be returned, providing access to all public methods and properties.
}
\details{
The type name to be created need not be fully qualified if it is unique in the VM.  Constructor arguments are matched with a fuzzy approach
so that the lack of type precision within R does not present a problem for strong typing on the .NET side.  Structurally equivalent types are casted or
ortherwise converted.

For example if a C# ctor signature was the following:
\code{AugmentedDickeyFuller (Vector<double> series, int lag, ADFType type)}
where ADFType is an enumeration.  A call to \code{.cnew("com.stg.math.statistics.AugmentedDickeyFuller", c(1,2,3,4), 3, 'Driftless')} would convert the R vector to a Vector<double> and \code{'Driftless'} to the enum value \code{ADFType.Driftless}.
}
\examples{
\dontrun{

series <- rnorm(100)

## create instance of an object, using fully qualified class name
obj <- .cnew ("com.stg.math.statistics.AugmentedDickeyFuller", series, 3, 'Driftless')

## or create without fully qualified name
obj <- .cnew ("AugmentedDickeyFuller", series, 3, 'Driftless')
}}

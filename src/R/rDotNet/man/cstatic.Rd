\name{.cstatic}
\alias{.cstatic}
\title{call a static method of a .NET class}
\usage{
.cstatic(classname, methodname, ...)
}
\arguments{
\item{classname}{The name of the .NET based class to be created.  Can either be a fully qualified name like "com.stg.models.DickeyFuller" or "DickeyFuller" if the classname is unique in the VM.}}
\item{methodname}{name of the static method to be called on type}
\item{...}{a sequence of arguments to the method.  The arguments can either be fundamental types, vectors, matrices, or other objects.  Fuzzy matching will be applied, allowing imperfectly matched argument types to be converted, as may be needed given R's more basic type system.}
}
\description{
This function calls a static method on a type.
}
\examples{
\dontrun{

## call MtM static method on MatrixUtils
matrix <- diag(c(1,2,3,4,5,6,7))
newmat <- .cstatic ("com.stg.math.MatrixUtils", "MtM", matrix)
}

}

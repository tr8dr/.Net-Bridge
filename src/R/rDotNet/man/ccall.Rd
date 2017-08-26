\name{.ccall}
\alias{.ccall}
\title{call a method on a .NET object}
\usage{
.ccall(obj, methodname, ...)
}
\arguments{
\item{obj}{An object previously created with \code{.cnew()}}
\item{methodname}{name of method to be called on object}
\item{...}{a sequence of arguments to the method.  The arguments can either be fundamental types, vectors, matrices, or other objects.  Fuzzy matching will be applied, allowing imperfectly matched argument types to be converted, as may be needed given R's more basic type system.}
}
\description{
This function calls a method on a previously created object.
}
\examples{
\dontrun{
series <- rnorm(100)

## create instance of an object, using fully qualified class name
obj <- .cnew ("com.stg.math.Model", 1.0, 3.5)

## call method F(3) on object
val <- .ccall (obj, "F", 3)

## call overload method F of 2 arguments F(3,4) on object
val <- .ccall (obj, "F", 3, 4)

## call method Estimate(series) on object
series <- rnorm(100)
val <- .ccall (obj, "Estimate", series)
}

}

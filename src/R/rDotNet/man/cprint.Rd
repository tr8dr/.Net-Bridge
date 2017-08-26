\name{print.rDotNet}
\alias{print.rDotNet}
\title{print .NET object}
\usage{
\method{print}{rDotNet}(x, ...)
}
\arguments{
\item{x}{An object previously created with \code{.cnew()}}
\item{...}{other arguments, not used}
}
\description{
This operator returns the object as a string
}
\examples{
\dontrun{

## create instance of an object, using fully qualified class name
obj <- .cnew ("com.stg.math.Model", 1.0, 3.5)

## print the object as a string
print (obj)

}

}

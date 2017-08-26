\name{[.rDotNet}
\alias{[.rDotNet}
\title{return ith element on a .NET object (such as an array, list, etc)}
\usage{
\method{[}{rDotNet}(obj,ith)
}
\arguments{
\item{obj}{An object previously created with \code{.cnew()}}
\item{ith}{numerical index into array, list, etc}
}
\description{
This operator returns the ith element of an an indexable object.
}
\examples{
\dontrun{

## create instance of an object, using fully qualified class name
obj <- .cnew ("com.stg.math.Model", 1.0, 3.5)

## get the 34th element from this indexable object
element <- obj[34]

}

}

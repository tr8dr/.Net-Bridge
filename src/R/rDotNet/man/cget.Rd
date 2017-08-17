\name{.cget}
\alias{.cget}
\title{Get a property value on a .NET object}
\usage{
.cget(obj, propertyname) or
obj$Get(propertyname)
}
\arguments{
\item{obj}{Previously created .NET object}
\item{propertyname}{name of the property to retrieve}
}
\description{
This function gets the value of a property on previously created .NET object.
}
\examples{
\dontrun{
obj <- .cnew ("com.stg.math.statistics.AugmentedDickeyFuller", series, 3, 'Driftless')
lag <- obj$Get("Lag")
}}

\name{.cset}
\alias{.cset}
\title{Set a property value on a .NET object}
\usage{
.sget(obj, propertyname, value) or
obj$Set(propertyname, value)
}
\arguments{
\item{obj}{Previously created .NET object}
\item{propertyname}{name of the property to set}
\item{value}{property value to set to}
}
\description{
This function sets the value of a property on previously created .NET object.
}
\examples{
\dontrun{
obj <- .cnew ("com.stg.models.FastMFT")
obj$Set("Alpha", 0.90)
obj$Set("Gamma", 1e-4)

gamma <- obj$get("Gamma")
}

}

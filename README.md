# .Net Bridge
.NET Bridge allows Python and R to access .NET libraries, with the .NET library running either locally or on a remote machine. From R or Python one can:

- create .NET objects
- call member functions
- call class functions (i.e. static members)
- access and set properties
- access indexing members

The following data types in arguments are supported:

- .NET objects
- integers (16, 32, 64) bit
- floats (32, 64) bit
- strings
- byte
- enums
- boolean
- arrays of: objects, integers, doubles, boolean, bytes, strings, etc
- vectors (with optional named index)
- matrices (with optional named row and column indices)

## The History
Some years ago, wrote .NET bridge as part of a much larger trading system codebase.  Given interest from others on the net, made an effort to extract the bridge and related classes from the much larger codebase.   While the .NET Bridge codebase is a factor of 100x smaller, there may yet be classes that could be removed, to make this even tighter.

## How It Works
The R or Python packages communicate with the .NET side through simple client / server interactions.  Your .NET libraries are loaded by a runner that provides a server-based API, giving full visibility into your library(ies). 

On first use from R or Python, the package will start the .NET bridge server (or alternatively connect to an existing server).  If the server is started from within VisualStudio, Xamarin Studio, or other tool, can be run in debug mode, so that you can debug your libraries as they are called from R or Python.

## Examples
Assuming the following (contrived) .NET classes:
```C#
namespace com.stg.dummy 
{
    class Point (double X, double Y);
    
    class Circle
    {
        Circle (double radius)
        {
            _radius = radius;
        }

        public double Radius 
            { get { return _radius; } }
            
        public Point[] PointsFor (double Ra, double Rb, double incr)
        {
            
        }
        
    }
}

```
Here is an example in R:
```R
## create object PriceProbability within library
model <- .cnew("com.stg.models.PriceProbability", '1M', 'Up', 0.01)

## call "F" method on .NET object
model$F (1.34)

## 
series <- model$Sample(0.0, 1.0)
```


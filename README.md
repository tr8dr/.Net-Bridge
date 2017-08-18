# .Net Bridge
.NET Bridge allows Python and R to access .NET libraries, with the .NET library running either locally or on a remote machine. From R or Python one can:

- create .NET objects
- call member functions
- call class functions (i.e. static members)
- access and set properties
- access indexing members

Two packages are provided, in addition to the .NET codebase:

- [rDotNet](https://github.com/tr8dr/.Net-Bridge/tree/master/src/R/rDotNet) (.NET interop for R)
- [pyDotNet](https://github.com/tr8dr/.Net-Bridge/tree/master/src/Python/pyDotNet) (.NET interop for Python)

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
Some years ago, wrote the .NET bridge as part of a much larger research and trading codebase.  Given interest from others on the net, made an effort to extract the bridge and related classes from the much larger codebase.   While the .NET Bridge codebase is a factor of 100x smaller, there may yet be classes that could be removed, to make this even tighter.

## How It Works
The R or Python packages communicate with the .NET side through simple client / server interactions.  Your .NET libraries are loaded by a runner that provides a server-based API, giving full visibility into your library(ies). 

On first use from R or Python, the package will start the .NET bridge server (or alternatively connect to an existing server).  If the server is started from within VisualStudio, Xamarin Studio, or other tool, can be run in debug mode, so that you can debug your libraries as they are called from R or Python.

When a method is first called the code looks for all methods in a class that may match based on name and number of arguments and then picks the method from that subset with the closest convertible signature.  The argument set need not be a perfect match in terms of types provided that the types can be reasonably converted.   For example strings will be converted to enum values if a given signature requires an enum, integers can be converted to floating point, double[] arrays can be applied to double[] or Vector<double>, etc.  These signatures are cached so that subsequent calls avoid scanning.

For example if a class has 2 overloaded public methods "F":

- ```public double F (double x, Vector<double> series)```
- ```public double F (Direction dir, Vector<double> series)```

where Direction is ```enum Direction { Up, Down }```.  If the object is called from R or python as:

```python
obj.F ('Up', [0.1, 0.2, 3.0, 3.1, 3.2])
```
the second method would be chosen given that 'Up' is convertible to ```Direction.Up``` and the numeric array is convertible to ```Vector<double>```.

## Initialization
The .NET bridge server (CLRServer.exe) does not contain your code by default.  We need to instruct the server to load a dll or dlls from your environment.  There are a number of ways to do this:

- run CLRServer.exe -dll <path to your dll> on the command line 
- run CLRServer.exe -dll <path to your dll> in your favorite IDE (particularly useful for debugging)
- run from within R or Python

From within R one has two options.  The first approach is to set an environment variable either within the R session or externally:
```R
## set environment variable
Sys.setenv(rDotNet_DLL="~/Dev/mymodels.dll")

## load package
require(rDotNet)

## create an object and call a method
obj <- .cnew ("NormalDistribution1D", 0.0, 1.0)
obj$F (0.1)
```

The second approch is to explicitly call the rDotNet initialization function:
```R
require(rDotNet)

## initialize
.cinit(dll="~/Dev/mymodels.dll")

## create an object and call a method
obj <- .cnew ("NormalDistribution1D", 0.0, 1.0)
obj$F (0.1)
```

In python, there is one approach, since the API usage pattern is different:
```python
clr = CLRApi (dll="~/Dev/mymodels.dll")

## create an object and call a method
obj <- clr.new ("NormalDistribution1D", 0.0, 1.0)
obj.F (0.1)

```


## Example
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
            { get { return _radius; } set { _radius = value; } }
        public double Area 
            { get { return Math.PI * _radius * _radius; } }
            
        ...
            
        // function returning list of objects
        public List<Point> PointsFor1 (int npoints)
        {
            var incr = 2.0 * Math.PI / (double)npoints;
            var list = new List<Point>[);
            
            for (int i = 0 ; i < npoints ; i++)
            {
                var theta = (double)i * incr;
                var x = _radius * Math.cos(theta);
                var y = _radius * Math.sin(theta);
                list.Add (new Point(x,y));
            }
            
            return list;
        }
        
        // function returning array of objects
        public Point[] PointsFor2 (int npoints)
        {
            return PointsFor(npoints).ToArray();
        }        
    }
}

```
The R api uses the $ syntax to reference members much like other R object approaches.  Here is how we could call the above from R:
```R
## create circle object
circle <- .cnew("com.stg.dummy.Circle", 10.0)

## get the list of points back
pointlist <- circle$PointsFor1 (100)

## dereference one of the point objects
point <- pointlist[2]

## or do it all in one go
point <- circle$PointsFor1 (100)[3]

## getting a property
circle$Get("Area")

## setting a property
circle$Set("Radius, 20)

```

The python API provides CLR as proxy objects almost indistinguishable from python objects.  One can interact with normal python syntax.  Here is how we could call the above from python:
```python
clr = CLRApi.get()

## create circle object
circle = clr.new("com.stg.dummy.Circle", 10.0)

## get the list of points back
pointlist = circle.PointsFor1 (100)

## dereference one of the point objects
point = pointlist[2]

## or do it all in one go
point = circle.PointsFor1 (100)[3]

## getting a property
circle.Area

## setting a property
circle.Radius = 20
```

# Installation
Please refer to the [Installation document](https://github.com/tr8dr/.Net-Bridge/tree/master/INSTALL.md).

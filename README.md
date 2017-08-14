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
Here is how we could call the above from R:
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

Here is how we could call the above from python:
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

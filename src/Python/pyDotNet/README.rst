
Seemless Python <-> .NET interop
================================

pyDotNet allows Python  to access .NET libraries, with the .NET library running either locally or on a remote machine. From Python one can:

* create .NET objects
* call member functions
* call class functions (i.e. static members)
* access and set properties
* access indexing members

The packages is part of a larger codebase providing extensions for
both R and python `.NET Bridge <https://github.com/tr8dr/.Net-Bridge/>`_.  The following data types in arguments are supported:

* .NET objects
* integers (16, 32, 64) bit
* floats (32, 64) bit
* strings
* byte
* enums
* boolean
* arrays of: objects, integers, doubles, boolean, bytes, strings, etc
* vectors (with optional named index)
* matrices (with optional named row and column indices)



The History
-----------

Some years ago, wrote the .NET bridge as part of a much larger research and trading codebase.  Given interest from others on the net, made an effort to extract the bridge and related classes from the much larger codebase.   While the .NET Bridge codebase is a factor of 100x smaller, there may yet be classes that could be removed, to make this even tighter.

How It Works
------------

The Python package communicate with the .NET side through simple client / server interactions.  Your .NET libraries are loaded by a runner ``CLRServer.exe`` that provides a TCP-based API, giving full visibility into your library(ies). 

On first use from Python, the package will start the .NET bridge server (or alternatively connect to an existing server).  If the server is started from within VisualStudio, Xamarin Studio, or other tool, can be run in debug mode, so that you can debug your libraries as they are called from  Python.

When a method is first called the code looks for all methods in a class that may match based on name and number of arguments and then picks the method from that subset with the closest convertible signature.  The argument set need not be a perfect match in terms of types provided that the types can be reasonably converted.   For example strings will be converted to enum values if a given signature requires an enum, integers can be converted to floating point, double[] arrays can be applied to ``double[]`` or ``Vector<double>``, etc.  These signatures are cached so that subsequent calls avoid scanning.

For example if a class has 2 overloaded public methods "F":

* ``public double F (double x, Vector<double> series)``
* ``public double F (Direction dir, Vector<double> series)``

where Direction is ``enum Direction { Up, Down }``.  If the object is called from python as:

.. code-block:: python
		
    obj.F ('Up', [0.1, 0.2, 3.0, 3.1, 3.2])
..

the second method would be chosen given that 'Up' is convertible to ``Direction.Up`` and the numeric array is convertible to ``Vector<double>``.

## Initialization
The .NET bridge server (CLRServer.exe) will not have access to your code unless you indicate a DLL to be loaded.  One needs to instruct the server to load a dll or dlls from your environment.  There are a number of ways to do this:

* run CLRServer.exe -dll <path to your dll> on the command line 
* run CLRServer.exe -dll <path to your dll> in your favorite IDE (particularly useful for debugging)
* run from within Python


Initializing the CLR with a DLL is done as follows:


.. code-block:: python
		
    ## initialization
    clr = CLRApi (dll="~/Dev/mymodels.dll")

    ## create an object and call a method
    obj <- clr.new ("NormalDistribution1D", 0.0, 1.0)
    obj.F (0.1)
..

Example
-------

Assuming the following (contrived) .NET classes were declared in
namespace **com.stg.dummy**:


.. code-block:: C#

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
..


The python API provides CLR as proxy objects almost indistinguishable from python objects.  One can interact with normal python syntax.  Here is how we could call the above from python:

.. code-block:: python
		
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
..


Installation
=============

In general one installs this package like any other python package.
However .NET should be present on the machine and in the path.  See
the OS specific installation instructions below.

Unix
----

Depending on how your system is setup, the above may require running
as sudo on unix.  One should also make sure you have the mono SDK installed
and **nuget** and **msbuild** in your path.   On OS X mono installs in:

*  ``/Library/Frameworks/Mono.framework/Commands``

and on Linux will depend on the package installer.  The  bin directory
where msbuild and nuget reside must be added to your path.  Before
running the package install, check that **nuget** and **msbuild**
can be run from the command line, then run the following:


.. code-block:: sh
		
    python setup.py install
..

or alternatively using pip. Depending on how your system is setup, the above may require
running under sudo. 


Windows
-------

Windows will have .NET installed by default.  However the various executables
needed for building may not be in your path.  Adjust your path
so that **msbuild** and associated compilers are visible (you can
adjust **Path** settings in the control panel).  The path to the various
 tools may be in the following directory or something similar:

*  ``c:\Windows\Microsoft.NET\Framework64\v4.0.30319``

If you do not have a command line version of nuget installed, you will need to install nuget
and place in your path.  Can find a command line version of nuget here:

*  https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

\Finally with all of the above installed and working, can run the installation as follows:

.. code-block:: sh
		
    python setup.py install
..

or alternatively using pip.




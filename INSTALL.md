# Preliminary Notes
The .NET Bridge project is fairly complicated in that it provides packages for 3 different language environments. 
Unfortunately each of these languages environments has a distinct build and packaging approach.  We may add a unix and 
potentially a windows specific script that combines these very different build approaches into a unified build.  
For now, depending on whether you want to use R or Python, there is a different installation approach for each.

Both the R and Python packages depend on the .NET Net.Bridge library and CLRServer which have been prebuilt in this 
distribution, sitting in **bin/**.  The Python package build copies the files in ```bin/Debug``` into the packages
during the install.
In the case of R, the **rDotNet/inst/server** directory contains a
condensed copy of the library and server source files, which are built
during installation.  

# Installing the R Package
In general one installs this package like any other R package.
However .NET should be present on the machine and in the path.  See
the OS specific installation instructions below.

## Unix
Depending on how your system is setup, the above may require running
as sudo on unix.  One should also make sure you have the mono SDK installed
and **nuget** and **msbuild** in your path.   On OS X mono installs in:

- /Library/Frameworks/Mono.framework/Commands

and on Linux will depend on the package installer.  The  bin directory
where msbuild and nuget reside must be added to your path.  Before
running the package install, check that **nuget** and **msbuild**
can be run from the command line, then run the following:

```sh
R CMD INSTALL rDotNet
```

## Windows
Windows will have .NET installed by default.  However the various executables
needed for building may not be in your path.  Adjust your path so that
 **msbuild** and associated compilers are visible (you can adjust
 **Path** settings in the control panel).  The path to the various
 tools may be in the following directory or something similar:

- c:\Windows\Microsoft.NET\Framework64\v4.0.30319

If you do not have a command line version of nuget installed, you will need to install nuget
and place in your path.  Can find a command line version of nuget here:

- https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

On windows you will also need to install the **Rtools** toolset for building R packages, available
on CRAN.  Finally with all of the above installed and working, can run the **R CMD INSTALL** as
indicated above.

Finally once all of the above is installed and in your path, run:

```sh
R CMD INSTALL rDotNet
```


# Installing the Python Package
```sh
cd src/Python/pyDotNet
python3 setup.py install
```

Depending on how your system is setup, the above may require running
as sudo on unix. 




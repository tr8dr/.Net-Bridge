# Preliminary Notes
The .NET Bridge project is fairly complicated in that it provides packages for 3 different language environments. 
Unfortunately each of these languages environments has a distinct build and packaging approach.  We may add a unix and 
potentially a windows specific script that combines these very different build approaches into a unified build.  
For now, depending on whether you want to use R or Python, there is a different installation approach for each.

Both the R and Python packages depend on the .NET Net.Bridge library and CLRServer which have been prebuilt in this 
distribution, sitting in **bin/**.  The Python package build copies the files in ```bin/Debug``` into the packages
during the install.
In the case of R, the **rDotNet/inst/server** directory contains a
copy of the build.  If you want to modify the .NET server code or
library you will want to copy into **rDotNet/inst/server** so that the
latest changes are reflected in the package.

# Installing the R Package
```sh
cd src/R
R CMD INSTALL rDotNet
```
## Unix
Depending on how your system is setup, the above may require running
as sudo on unix.  One should also make sure you have the mono SDK installed
and **nuget** and **msbuild** in your path.   On OS X mono installs in:

- /Library/Frameworks/Mono.framework/Commands

and on Linux will depend on the package installer.  The essential thing is to 
add the bin director(ies) where msbuild and nuget reside to your path. 

## Windows
Windows will have .NET installed by default.  However the various executables
needed for building will not be in your path by default.  Add the path to **msbuild**
and associated compilers to your **Path** variable in the control panel.  The path may 
be in the following directory or something similar:

- c:\Windows\Microsoft.NET\Framework64\v4.0.30319

If you do not have a command line version of nuget installed, you will need to install nuget
and place in your path.  Can find a command line version of nuget here:

- https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

On windows you will also need to install the **Rtools** toolset for building R packages, available
on CRAN.  Finally with all of the above installed and working, can run the **R CMD INSTALL** as
indicated above.


# Installing the Python Package
```sh
cd src/Python/pyDotNet
python3 setup.py install
```

Depending on how your system is setup, the above may require running
as sudo on unix. 




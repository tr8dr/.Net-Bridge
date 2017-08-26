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

# Installing the Python Package
```sh
cd src/Python/pyDotNet
python3 setup.py install
```

Depending on how your system is setup, the above may require running
as sudo on unix.   On windows one should install the Rtools package
available on CRAN and make sure that the Rtools commands are in your
**Path**. 




# Preliminary Notes
The .NET Bridge project is fairly complicated in that it provides packages for 3 different language environments. 
Unfortunately each of these languages environments has a distinct build and packaging approach.  We may add a unix and 
potentially a windows specific script that combines these very different build approaches into a unified build.  
For now, depending on whether you want to use R or Python, there is a different installation approach for each.

Both the R and Python packages depend on the .NET Net.Bridge library and CLRServer which have been prebuilt in this 
distribution, sitting in **bin/**.  Both R and Python copy the files in ```bin/Debug``` into their own packages
so that each can reference the .NET functionality from within the package.

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

Depending on how your system is setup, the above may require running as sudo on unix.




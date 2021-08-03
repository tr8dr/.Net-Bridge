# rDotNet 0.9.0
This is the initial public version of rDotNet.  This version has been successfully built and tested on:

- OS X 10.12.6
- Windows 10 Pro
- Ubuntu Linux 14.04

# rDotNet 0.9.1
This version fixes some build issues.

- Jari Karppinen noted that the build for linux was broken so submitted some patches.
- added unit tests to the tests directory (which will be skipped if .NET is not available)
- adjusted the TcpClient source to use a newer API, so will build on Solaris

# rDotNet 0.9.2
This version allows the .NET bridge to initialize with more than one
DLL either provideds as a vector of DLLs or a semicolon delimited list
of dlls as an environment variable.

- feature and suggested implementation provided by SimonRi1985

# rDotNet 0.9.3
This version fixes some C/C++ type issues in the build.  The code was
uncorrectly using "long", "int", and "short", assuming 64, 32, and 16
bits respectively.   Now uses int64_t, int32_t, and int16_t.

- bug fix requested by Tomas Kalibera.




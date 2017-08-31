
## do our special magic here.  May need to build the .NET server
.onAttach = function (libname, pkgname)
{
    packagedir <- path.package("rDotNet")
    server <- sprintf("%s/server/bin/Debug/CLRServer.exe", packagedir)

    ## check to see if was compiled
    if (file.exists(server))
        return()

    packageStartupMessage ("attempting to build CLR server, one time setup")
    if (Sys.which("nuget") == "")
    {
        warning ("could not find nuget in path; will not be able to use rDotNet unless corrected and rebuilt")
        return ()
    }
    
    if (Sys.which("msbuild") == "" && Sys.which("xbuild") == "")
    {
        warning ("could not find msbuild or xbuild in path; will not be able to use rDotNet unless corrected and rebuilt")
        return()
    }
    
    cwd <- getwd()
    setwd(sprintf("%s/server", packagedir))

    packageStartupMessage ("getting dependent packages")
    system2 ("nuget", "restore", wait=TRUE, stderr=TRUE, stdout=TRUE)
        
    packageStartupMessage ("building project")
    system2 (ifelse(Sys.which("msbuild") != "", "msbuild", "xbuild"), wait=TRUE, stderr=TRUE, stdout=TRUE)

    setwd(cwd)
}

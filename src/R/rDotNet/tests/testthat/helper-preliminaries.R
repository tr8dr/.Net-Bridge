
## determine whether the package was built with .NET CLR server support
is.net.installed <- function ()
{
    packagedir <- path.package("rDotNet")
    server <- sprintf("%s/server/bin/Debug/CLRServer.exe", packagedir)

    ## check to see if was compiled / exists
    file.exists(server)
}

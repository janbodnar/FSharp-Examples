# FTP client

FTP commands using the FluentFTP library  

## List directory

```F#
#r "nuget: FluentFTP"

open FluentFTP
open System

let ip = "45.33.2.79"
let username = "username"
let passwd = "s$cret"

let path = fsi.CommandLineArgs[1]

let launch() = 

    use con = new FtpClient(ip, username, passwd)
    con.Connect()

    let paths = con.GetNameListing(path)
    printfn "%A" paths

    paths |> Array.iter Console.WriteLine  

launch()
```

## Fetch file 

run `$ dotnet fsi fetch_file.fsx /web/dart/process/index.html`  

```F# 
#r "nuget: FluentFTP"

open FluentFTP
open System.IO

let ip = "45.33.2.79"
let username = "username"
let passwd = "s$cret"

let path = fsi.CommandLineArgs[1]
let baseName = Path.GetFileName(path)

let launch () =

    use con = new FtpClient(ip, username, passwd)
    con.Connect()

    let status =
        con.DownloadFile(
            $"{baseName}",
            path,
            FtpLocalExists.Overwrite,
            FtpVerify.Retry
        )
    
    match status with
    | FtpStatus.Success -> printfn "file fetched OK"
    | FtpStatus.Failed -> printfn "failed to fetch file"
    | _ -> printfn "skipped or unknown"


launch ()
```

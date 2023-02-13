# Parse command line arguments

Using Argu library
The `-h` `--help` is built-in  


```F#
#r "nuget: Argu, 6.1.1"

open Argu

type Arguments =
    | [<AltCommandLine("-p")>] Print of message: string

    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | Print msg -> "Print message"

let argv = fsi.CommandLineArgs[1..]
let parser = ArgumentParser.Create<Arguments>(programName = "altcmd.fsx")

// let usage = parser.PrintUsage()
// printf "%s" usage

let res = parser.ParseCommandLine argv

if res.Contains Print then
    printfn "%s" (res.GetResult(Print))
```

The *Print* argument is turned to `--print`.  
We have a Print argument; with AltCommandLine attribute we add an alternative argument.  

## Error handler 

```F#
#r "nuget: Argu, 6.1.1"

open Argu
open System

type Arguments =
    | [<AltCommandLine("-p")>] Print of message: string

    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | Print msg -> "Print message"

let argv = fsi.CommandLineArgs[1..]

let errHandler =
    ProcessExiter(
        colorizer =
            function
            | ErrorCode.HelpText -> None
            | _ -> Some ConsoleColor.Red
    )

let parser =
    ArgumentParser.Create<Arguments>(programName = "errhand.fsx", errorHandler = errHandler)

let res = parser.ParseCommandLine argv

if res.Contains Print then
    printfn "%s" (res.GetResult(Print))
```


# Parse command line arguments

Using Argu library
The `-h` `--help` is built-in  

## Alternative arg name


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

## Main command 

Main commands do not need a prefix. The *Mandatory* attribute creates a required argument.  

```F#
#r "nuget: Argu, 6.1.1"

open Argu
open System

type Arguments =
    | [<MainCommand; Mandatory>] Host of string

    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | Host _ -> "The host to connect to"

let argv = fsi.CommandLineArgs[1..]

let errHandler =
    ProcessExiter(
        colorizer =
            function
            | ErrorCode.HelpText -> None
            | _ -> Some ConsoleColor.Red
    )

let parser =
    ArgumentParser.Create<Arguments>(programName = "mainarg.fsx", errorHandler = errHandler)

let usage = parser.PrintUsage()
printf "%s" usage

let res = parser.ParseCommandLine argv
printfn "%A" res
printfn "%s" (res.GetResult Host)
```

## Multiple values for an argument 

The example's `--url` argument takes two values  

`fx multval.fsx --url webcode.me 80`

```F#
#r "nuget: Argu, 6.1.1"

open Argu
open System

type Arguments =
    | Url of host:string * port:int

    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | Url _ -> "The host and port to make a connection"

let argv = fsi.CommandLineArgs[1..]

let errHandler =
    ProcessExiter(
        colorizer =
            function
            | ErrorCode.HelpText -> None
            | _ -> Some ConsoleColor.Red
    )

let parser =
    ArgumentParser.Create<Arguments>(programName = "multval.fsx", errorHandler = errHandler)

let usage = parser.PrintUsage()
printf "%s" usage

let res = parser.ParseCommandLine argv
printfn "%A" res
printfn "%A" (res.GetResult(Url))
printfn "%s" (fst (res.GetResult(Url)))
printfn "%d" (snd (res.GetResult(Url)))
```

# FSharp-Examples


F# is a functional language for .NET. Highly influenced by OCalm, first appeared  
in 2005.  

`$ dotnet new console -lang F# -o Simple`  
`$ cd Simple`  
`$ dotnet run`  

Turn off telemetry  

`export DOTNET_CLI_TELEMETRY_OPTOUT=1`  

The current directory  
`__SOURCE_DIRECTORY__`  

Run F# scripts  

`$ dotnet fsi simple.fsx`  

Run F# script in preview version  
`$ dotnet fsi --langversion:preview simple.fsx`  


read files from command line arguments  
```
let args = fsi.CommandLineArgs.[1..] 

let words = Array.collect (fun f -> File.ReadAllLines(f)) args
printfn "%A" words
```

`dotnet fsi simple.fsx *.txt`  run script  

**This is the same**  
```
Array.filter (fun e -> (snd e) > 1)
Array.filter (fun (_, i) -> i >= 2)
Array.filter (snd >> ((<)1))
```

```
Seq.filter (fun (_, s) -> Seq.length s > 1)
Seq.filter (fun (_, s) -> (s |> Seq.length) > 1)
Seq.filter (fun v -> match v with (_, c) -> c > 1)
Seq.map snd |> Seq.filter (fun t -> Seq.length t > 1)
```

**Types**  

`int list` is a synonym for `list<int>`  
`'a` the tick character in a type name is used for a generic type  
`a'` the tick in a variable name is used to denote a symbol similar to a;
     borrowed from math, where ' is used for a derivative or a transposed matrix


merge lists  

```
let a = [1; 2; 3; 4]
let b = [4; 4; 5; 6]

let merged = a @ b |> List.distinct
printfn "%A" merged
```

get cmd args in script  
```
let args2 = fsi.CommandLineArgs |> Array.tail 

for arg in args2 do 
    Console.WriteLine(arg)
done
```

get cmd args in program/not script  
` printfn "env.cmdline: %A" <| Environment.GetCommandLineArgs() `

read and filter data  
```
 let fileName = __SOURCE_DIRECTORY__ + "/words.txt"

 let data = File.ReadAllLines(fileName)
 let filtered = data |> Array.filter (fun e -> e.StartsWith "c" || e.StartsWith "s") 

 Console.WriteLine(filtered)
```

**Printing** 

to print a single argument without formatting, use `Console.WriteLine`  
the print* functions are F# helpers  

```
let name = "John Doe"

printfn "%s" name
System.Console.WriteLine(name)
```


F# 5 introduced string interpolation  

```
let name = "John Doe"
let occupation = "gardener"

let msg = $"{name} is an {occupation}"
printfn $"{msg}"

printfn $"5 * 8 = {5 * 8}"

printfn $"{58:C}"
printfn $"{58:X}"
```


Algorithm to calculate powers; F# script with  
#time directive  

```
let rec power x n: bigint =
    if n = 0I then 1I
    else x * (power x (n-1I))

// more efficient algorithm
let rec power2 x n: bigint =
    if n = 0I then 1I
    // if n is even
    elif (n % 2I = 0I) then ((power2 x (n/2I)) * (power2 x (n/2I)))
     // if n is odd
    else x * (power2 x (n-1I))

#time
//printfn "%A" (power 1254I 29_000I)
printfn "%A" (power2 1254I 29_000I)
#time
```

Custom timing fun with StopWatch  

```
open System.Diagnostics
open System.Threading


let time f =

  let stopwatch = Stopwatch.StartNew()

  f()

  stopwatch.Stop()
  
  printfn "%O" stopwatch.Elapsed


time (fun () -> Thread.Sleep(2000))
```

Write to source directory  

```
open System.IO

let writeToFile () = 

    let data = "an old falcon"
    File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "file.txt"), data)

writeToFile()
```

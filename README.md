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

## Printing

to print a single argument without formatting, use `Console.WriteLine`  
the print* functions are F# helpers  

```
let name = "John Doe"

printfn "%s" name
System.Console.WriteLine(name)
```

## not is a function 

```
let words =
    [ "sky"
      "wind"
      "blue"
      "storm"
      "war"
      "cup"
      "lure"
      "smile" ]

let w1 = "war"

if words |> List.contains w1 then
    printfn "%s found" w1

let w2 = "cloud"

if not(words |> List.contains w2) then
    printfn "%s not found" w2

let w3 = "bin"

if not <| (words |> List.contains w3) then 
    printfn "%s not found" w3
```

## Discards

The _ character is a discard which is a placeholder for values that we 
do not need.  

```
let vals = (1, 2, 3, 4, 5)
let _, _, _, x, y = vals

printfn $"x: {x}; y: {y}"
```



## Classic loops

```
let vals = [ 1; 2; 3; 4; 5 ]

for e in vals do
    printfn "%d" e

for e in 1 .. vals.Length do
    printfn "%d" e

let mutable i = 0

while i < vals.Length do
    printfn "%d" vals.[i]
    i <- i + 1
```
Traditional imperative loops are available; however, functional approach is   
preferred.  

## Functional iteration

```
let vals = [ 1; 2; 3; 4; 5 ]
vals |> List.iter Console.WriteLine 
```
Iterating over elements with a built-in List.iter function  

```
let vals = [ 1; 2; 3; 4; 5 ]

let rec iterate vals =
    match vals with
    | h::t ->
        printfn $"{h}" 
        iterate t // do all over again with the rest of the list
    | [] -> ()

iterate vals
```

iterating over elements with a custom recursive function utilizing pattern  
matching  

## Concat list of strings

```
open System

let words = ["sky"; "cloud"; "cup"; "snow"; "water"; "war"; "rock"]
let output = words |> Seq.reduce (fun acc e -> (sprintf $"{acc}, {e}"))
printfn $"{output}"

let output2 = words |> List.reduce (fun acc e -> acc + Environment.NewLine + e)
printfn $"{output2}"

let output3 = String.concat ", " words
printfn $"{output3}"

let output4 = words |> String.concat ", "
printfn $"{output4}"
```


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

**FindAll and filter are similar**  

```
let found =
    Array.FindAll(words, (fun e -> e.StartsWith("w") && e.Length = 3))

let found =
    words |> Array.filter (fun e -> e.StartsWith("w") && e.Length = 3)
```

## Find first/last element

```
let first =
    words |> Array.find (fun e -> e.Length = 3)

let last =
    words |> Array.findBack (fun e -> e.Length = 3)
```

## Types

'T, 'U - generic type parameters  
^T, ^U - statically resolved type parameters  

`int list` is a synonym for `list<int>`  
`'a` the tick character in a type name is used for a generic type  
`a'` the tick in a variable name is used to denote a symbol similar to a;  
     borrowed from math, where ' is used for a derivative or a transposed matrix  

`int [,]` - two-dimensional array  
`int [,,]` - three-dimensional array  

```
let t1 = (1, 2, 3, 4) // int * int * int * int
let t2 = (4, "falcon", true) // int * string * bool
```
In tuples, the types for elements are separated with * character  


`let f (x:int): int = x + 1`  - f is a function; it takes an int as a parameter  
                                 and returns an integer  

```
// g : int * string -> unit
let g(x, s) = 
    printfn "%d %s" x s 
```

g  is a function which takes an integer and a string as a parameter; it  
returns unit (nothing); this function is said to have a side-effect  

```
// int -> string -> unit
let f1 x s = 
    printfn "%d %s" x s

// int * string -> unit
let f2(x,s) = 
    printfn "%d %s" x s
    
f1 3 "falcons"   
f2(3, "falcons")
```

 f1's arguments are curried, whereas f2's arguments are tupled  


## Merge lists

```
let a = [1; 2; 3; 4]
let b = [4; 4; 5; 6]

let merged = a @ b |> List.distinct
printfn "%A" merged
```

## Command line arguments

in script  
```
let args2 = fsi.CommandLineArgs |> Array.tail 

for arg in args2 do 
    Console.WriteLine(arg)
done
```

in program/not script  
` printfn "env.cmdline: %A" <| Environment.GetCommandLineArgs() `

read and filter data  
```
 let fileName = __SOURCE_DIRECTORY__ + "/words.txt"

 let data = File.ReadAllLines(fileName)
 let filtered = data |> Array.filter (fun e -> e.StartsWith "c" || e.StartsWith "s") 

 Console.WriteLine(filtered)
```

## StringBuilder

```
open System.Text

let builder = StringBuilder()

Printf.bprintf builder "There are %d " 3
Printf.bprintf builder "hawks in the sky"

printfn "%s" (builder.ToString())
```

```
open System.Text

let buf = StringBuilder()

buf.Append("There are " ) |> ignore
buf.Append("three ") |> ignore 
buf.Append("eagles in the sky") |> ignore

printfn $"%s{buf.ToString()}"
```

## String interpolation

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

## Active patterns

Active patterns define ad hoc union data structures for easy pattern matching

```
let (|Even|Odd|) n =
    if n % 2 = 0 then
        Even
    else
        Odd
```
is equal to  

```
type NumKind =
    | Even
    | Odd

let GetChoice n =
    if n % 2 = 0 then
        Even
    else
        Odd
```


```
open System

let (|Even|Odd|) n = if n % 2 = 0 then Even else Odd

let testNum n =
    match n with
    | Even -> sprintf "%i is even" n
    | Odd -> sprintf "%i is odd" n

[ 12; 11; 4; 5; 6; 7; 8; 9 ]
|> List.map (testNum)
|> List.iter Console.WriteLine
```



## calc pow

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

## Stopwatch

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

## Source directory

Write to source directory  

```
open System.IO

let writeToFile () = 

    let data = "an old falcon"
    File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "file.txt"), data)

writeToFile()
```


## Get n words from sentence

```
open System

[<EntryPoint>]
let main argv =

    let n = argv.[0] |> int
//    let n = 4

    let msg = "There are three falcons in the sky."
    let output = msg.Split() |> Array.take n |> String.concat " "

    Console.WriteLine output
    0
```

### Read nth line

```
open System
open System.IO

[<EntryPoint>]
let main args =

    let n = Int32.Parse(args.[1]) - 1
    use r = new StreamReader(args.[0])

    let lines =
        Seq.unfold
            (fun (reader: StreamReader) ->
                if (reader.EndOfStream) then
                    None
                else
                    Some(reader.ReadLine(), reader))
            r

    let line = Seq.item n lines // Seq.nth throws an ArgumentException, if not not enough lines available
    
    Console.WriteLine(line)
    0
```

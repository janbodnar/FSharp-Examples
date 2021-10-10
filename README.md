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

F# one-liner  

`echo 'printfn "Hello from Fsharp"' | dotnet fsi --quiet`  

Run F# script in preview version  
`$ dotnet fsi --langversion:preview simple.fsx`  

## Command line args


```
let args = fsi.CommandLineArgs.[1..] 

let words = Array.collect (fun f -> File.ReadAllLines(f)) args
printfn "%A" words
```

`dotnet fsi simple.fsx *.txt`  run script  

---

### Script

```
let args2 = fsi.CommandLineArgs |> Array.tail 

for arg in args2 do 
    Console.WriteLine(arg)
done
```

### In program, main

```
open System

[<EntryPoint>]
let main argv =

    argv |> Array.iter Console.WriteLine

    0 
```

### In program, withoud main 

` printfn "env.cmdline: %A" <| Environment.GetCommandLineArgs() `


## Read and filter data  
```
 let fileName = __SOURCE_DIRECTORY__ + "/words.txt"

 let data = File.ReadAllLines(fileName)
 let filtered = data |> Array.filter (fun e -> e.StartsWith "c" || e.StartsWith "s") 

 Console.WriteLine(filtered)
```

## Standard/reverse call

```
let res = List.map (fun x -> x * x) [1;2;3;4;5]
let res2 = [1;2;3;4;5] |> List.map (fun x -> x * x)
```

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

## Pipes

```
printfn "%A" ([1;2;3] |> fun e -> e @ [4;5;6]) // lambda
printfn "%A" ([1;2;3] |> (@) <| [4;5;6]) // point-free
printfn "%A" (([1;2;3], [4;5;6]) ||> (@)) // point-free

// function
let addLists l1 l2 = l1 @ l2
let res = addLists [1;2;3] [4;5;6]

printfn "%A" res
```

## Discards

The _ character is a discard which is a placeholder for values that we 
do not need.  

```
let vals = (1, 2, 3, 4, 5)
let _, _, _, x, y = vals

printfn $"x: {x}; y: {y}"
```

## pown is power function for integers  

```
open System

let vals = [| 2; 4; 6; 8 |]

let powered = vals |> Array.map (fun e -> 2 |> pown e)
printfn "%A" powered
```

## Convert array ints to strings

```
open System

let nums = [| 2; 4; 6; 8 |]

let output =
    nums
    |> Array.map (sprintf "%i")
    |> String.concat ","

Console.WriteLine(output)
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

---

```
let matches = rx.Matches(data)

let topTen =
    matches
    |> Seq.map (fun m -> m.Value)
    |> Seq.filter (dig.IsMatch >> not)
    ...
```

```
let matches = rx.Matches(data)

let topTen =
     seq {
         for m in matches do
             yield m.Value
     }
     |> Seq.filter (dig.IsMatch >> not)
```

---

```
Seq.sortByDescending snd
Seq.sortBy (fun e -> -(snd e))
```

```
Seq.filter (dig.IsMatch >> not)
Seq.filter (fun e -> e |> dig.IsMatch |> not)
Seq.filter (fun e -> dig.IsMatch(e) |> not)
Seq.filter (fun e -> not(dig.IsMatch(e)))
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
     borrowed from math, where ' is used for a derivative or a transposed matrix;  
     or this name is a close variation of another one, with the unquoted name  
     
```
let d5'0 = flip (/) 5
let d5'1 = (/) >< 5

let m5'0 = flip (%) 5
let m5'1 = (%) >< 5
```

The ticks in the names mean that they are close variants of each other  


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


## Get file names  

```
open System.IO

let files = Directory.GetFiles(".")

printfn "%A" files

let names = files |> Array.map Path.GetFileName 
printfn "%A" names
```



## Partition function

```
open System.IO

let files = Directory.GetFiles(".")

let data = files 
        |> Array.partition(fun e -> e.EndsWith("txt"))

let matched = fst data
printfn "%A" matched

let output = matched |> Array.map (fun e -> e.TrimStart('.', '/'))
printfn "%A" output
```

partition files into two groups: txt files and the rest  

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

## Point-free 

Point-free programming  is a paradigm that advocates the formulation of  
programs by means of function composition. In the point-free style programs  
avoid explicitly nominating function arguments (or "points"), deriving  
instead complex function definitions by means of applying higher-order  
combinators on simpler functions.  

```
let vals = [1 .. 15]

// point-free
let flip f = fun x y -> f y x
let filtered = vals |> List.filter ((=) 0 << flip (%) 2)

printfn "%A" filtered

// lambda
let filtered2 = vals |> List.filter (fun n -> n % 2 = 0)

printfn "%A" filtered2
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

## Filter users having above average salary 

```
open System

type User =
    { FirstName: string
      LastName: string
      Salary: int }

let users = [
    
        { FirstName="Robert"; LastName="Novak"; Salary=1770 };
        { FirstName="John"; LastName="Doe";  Salary=1230 };
        { FirstName="Lucy"; LastName="Novak";  Salary=670 };
        { FirstName="Ben"; LastName="Walter";  Salary=2050 };
        { FirstName="Robin"; LastName="Brown";  Salary=2300 };
        { FirstName="Amy"; LastName="Doe";  Salary=1250 };
        { FirstName="Joe"; LastName="Draker";  Salary=1190 };
        { FirstName="Janet"; LastName="Doe";  Salary=980 };
        { FirstName="Peter"; LastName="Novak";  Salary=990 };
        { FirstName="Albert"; LastName="Novak";  Salary=1930 }
]

let avg = users |> List.averageBy (fun user -> float user.Salary)
let users2 = users |> List.filter (fun user -> user.Salary > int avg)

users2 |> List.iter Console.WriteLine
```

## sort words by frequency  

```
open System
open System.IO
open System.Text.RegularExpressions

let fileName = "the-king-james-bible.txt"
let data = File.ReadAllText(fileName)

let dig = Regex(@"\d")
let rx = Regex("[a-z-A-Z']+")

let matches = rx.Matches(data)

let topTen =
    matches
    |> Seq.map (fun m -> m.Value)
    |> Seq.filter (dig.IsMatch >> not)
    |> Seq.countBy id
    |> Seq.sortByDescending snd
    |> Seq.take 10

topTen
|> Seq.iter (fun (e, n) -> Console.WriteLine($"{e}: {n}"))
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

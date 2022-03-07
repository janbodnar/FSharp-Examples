# Strings 

## Concat list of strings

```F#
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


## int to string

The `int` built-in function converts a string to an integer  

```f#
open System

let vals = ("2", 1, "4", 6, "11")

let a, b, c, d, e = vals
let sum = int a + b + int c + d + int e

Console.WriteLine(sum)
```

## Convert array ints to strings

```f#
open System

let nums = [| 2; 4; 6; 8 |]

let output =
    nums
    |> Array.map (sprintf "%i")
    |> String.concat ","

Console.WriteLine(output)
```

## Building/formatting strings

```f#
open System
open System.Text

let name = "John Doe"
let age = 33

let msg1 = name + " is " + string age + " years old"
printfn $"{msg1}"

let msg2 = sprintf "%s is %d years old" name age
printfn $"{msg2}"

let msg3 = $"{name} is {age} years old"
printfn $"{msg3}"

let msg4 = String.Format("{0} is {1} years old", name, age)
printfn $"{msg4}"

let builder = StringBuilder()
let msg5 = builder.AppendFormat("{0} is {1} years old", name, age)
printfn $"{msg5}"
```

## StringBuilder

```F#
open System.Text

let builder = StringBuilder()

Printf.bprintf builder "There are %d " 3
Printf.bprintf builder "hawks in the sky"

printfn "%s" (builder.ToString())
```

```F#
open System.Text

let buf = StringBuilder()

buf.Append("There are " ) |> ignore
buf.Append("three ") |> ignore 
buf.Append("eagles in the sky") |> ignore

printfn $"%s{buf.ToString()}"
```

## String interpolation

F# 5 introduced string interpolation  

```F#
let name = "John Doe"
let occupation = "gardener"

let msg = $"{name} is an {occupation}"
printfn $"{msg}"

printfn $"5 * 8 = {5 * 8}"

printfn $"{58:C}"
printfn $"{58:X}"
```
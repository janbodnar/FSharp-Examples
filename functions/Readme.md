# Functions

## Definitions

This is a value, not a function  
    
    let hello =  
        printfn "Hello there"


This is a function

    let hello () =
        printfn "Hello there"

The following function definitions are the same. The first  
uses the lambda expression; the second is the conventional function definition.  

    let add = fun x y -> x + y
    let add2 x y = x + y

## Pipe operator

```
open System

let square x = x * x
let toStr (x: int) = x.ToString()
let rev (x: string) = String(Array.rev (x.ToCharArray()))

// passing to functions
let result = rev (toStr (square 256))

Console.WriteLine(result)

// using temporary bindings
let step1 = square 256
let step2 = toStr step1
let step3 = rev step2
let result2 = step3

Console.WriteLine(result2)

// using pipe operator
let result3 = 256 |> square |> toStr |> rev

Console.WriteLine(result3)
```


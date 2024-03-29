# Functions

## Passing function values

Parenthesis are neede around functions to pass values  
to `printfn` function  

```f#
open System

let show () = "falcon"

Console.WriteLine(show ())

printfn "%s" (show ())
printfn "%s %s" (show ()) (show ())
printfn "%s" <| show ()
show () |> printfn "%s"
```

another example: `printfn "%s" (DateTime.Now.ToLongDateString())`

```F#
let contains =
    let words = set ["sky"; "drum"; "dog"; "tree"]
    fun person -> words.Contains(person)

let contains2 word =
    let words = set ["sky"; "drum"; "dog"; "tree"]
    words.Contains(word)
```

contains creates words on declaration and reuses it, whereas  
contains2 creates words everytime you call the function;  
contains is slightly faster.  

## Generic function

F# uses the `'T` for a generic type.   

```F#
let fn<'T> (a: 'T) (b: 'T) = printfn "%A %A" a b

fn "sky" "blue"
fn true false
fn 'a' 'b'
fn 12 23
fn 1.2 1.3
```

## Curried & tupled functions

There are two function calls in F#: curried and tupled.  

`let add a b = a + b`  - curried form  

`add 10 5` => `((add 10) 5)`  - function takes 10 and then returns a function which takes 5  


The compiled F# code doesn't produce methods in the curried form; therefore,  
we cannot use partial function application directly from C#.   
This is due to performance reasons; most of the times, we specify all   
arguments and this can be implemented more efficiently.  


```F#
open System 

// curried
let add x y = x + y 
add 5 <| 10 |> Console.WriteLine
(add 5) 10 |> Console.WriteLine
(add 5) <| 10 |> Console.WriteLine
add <| 5 <| 10 |> Console.WriteLine
add 5 10 |> Console.WriteLine
5 |> add <| 10 |> Console.WriteLine
Console.WriteLine(add 10 5)
Console.WriteLine((add 10) 5)
Console.WriteLine((add 10) <| 5)
Console.WriteLine((add <| 10 <| 5))

// tupled
let add'(x, y) = x + y

add'(10, 5) |> Console.WriteLine
add' <| (10, 5) |> Console.WriteLine
(10, 5) |> add' |> Console.WriteLine
Console.WriteLine(add'(10, 5))
Console.WriteLine(((10, 5) |> add'))
Console.WriteLine((add' <| (10, 5)))
```

In a curried form, we can pass values one by one; in tupled form,  
we pass only tuples -- pairs of values.  

## Definitions

This is a value, not a function  

```f#
let hello =  
    printfn "Hello there"
```

This is a function

```f#
let hello () =
    printfn "Hello there"
```


The following function definitions are the same. The first  
uses the lambda expression; the second is the conventional function definition.  

```f#
let add = fun x y -> x + y
let add2 x y = x + y
```

## Returning a function


```f#
open System
open System.Threading

let f =
    printfn "This runs at startup"
    (fun () -> printfn "%s" (DateTime.Now.ToLongTimeString()))

f()

Thread.Sleep(1000)
f()

Thread.Sleep(1000)
f()
```

## Flip fun

The flip function swaps the arguments of a function  
It can make the code more clear; unlike in Haskell, it is not  
a standard operator  

```F#
let res = [ 1 .. 100 ] |> List.filter ((>) 50)  // returns values < 50  
printfn "%A" res

let flip f = fun x y -> f y x

let res2 =
    [ 1 .. 100 ] |> List.filter (flip (>) 50) // returns values > 50  

printfn "%A" res2
```

## Pipe operator

```f#
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


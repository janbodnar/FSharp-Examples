# Match expressions

The match expression provides branching control that is based on  
the comparison of an expression with a set of patterns.  

```F#
let vals = [ 1; -3; 5; 6; 0; 4; -9; 11; 22; -7 ]

for wal in vals do

    match wal with
    | n when n < 0 -> printfn "%d is negative" n
    | n when n > 0 -> printfn "%d is positive" n
    | _ -> printfn "zero"
```    
Printing a message for each value of a list using `when` guards.  
With `_`  we create an exhaustive matching.  

---

```F#
let vals =
    [ [ 1; 2; 3 ]
      [ 1; 2 ]
      [ 3; 4 ]
      [ 8; 8 ]
      [ 0 ] ]

let twoels (sub: int list) =
    match sub with
    | [ x; y ] -> printfn "%A" [ x; y ]
    | _ -> ignore ()

for sub in vals do
    twoels sub
```
Printing all sublists with two-elements with pattern matching and  
for loop.  

```F#
let vals =
    [ [ 1; 2; 3 ]
      [ 1; 2 ]
      [ 3; 4 ]
      [ 6; 5; 3; 2; 4; 5 ]
      [ 8; 8 ]
      [ 0 ] ]

let rec twoels (data: int list list) : unit =
    match data with
    | h :: t when h.Length = 2 ->
        printfn "%A" h
        twoels t
    | h :: t when h.Length <> 2 -> twoels t
    | _ -> ignore ()

twoels vals
```

Printing 2-e sublists with recursive algorithms.  

---

```F#
open System 

let rand = new Random()

let days =
    [ "monday"
      "tuesday"
      "wednesday"
      "thursday"
      "friday"
      "saturday"
      "sunday" ]

let d = days[rand.Next(days.Length)]

let ret =
    match d with
    | "monday"
    | "tuesday"
    | "wednesday"
    | "thursday"
    | "friday" -> "weekday"
    | "saturday"
    | "sunday" -> "weekend"
    | w -> failwithf "%s is out of range" w

printfn "%s %s" ret d
```

Categorizing values.  

```F#
open System 

type Choices =
    | A
    | B
    | C

let getVal () =
    match Random().Next(1, 4) with
    | 1 -> A
    | 2 -> B
    | _ -> C

let chx = [ for _ in 1..7 -> getVal () ]
printfn "%A" chx
```
Generating a list of random choices.  

```F#
open System 

type Choices =
    | A
    | B
    | C

let getVal =
    function
    | 1 -> A
    | 2 -> B
    | _ -> C

let chx =
    [ for _ in 1..7 do
          yield getVal (Random().Next(1, 4)) ]

printfn "%A" chx
```

Another variant of the previous example. The `function` can  
replace `match/with`.  

```F#
let vals = [ 1..7 ]
let vals2 = [ 1; 2; -1; -4 ]
let vals3 = []

let rec traverse e =
    match e with
    | h :: t ->
        printfn "%d" h
        traverse t
    | [] -> printfn ""

traverse vals
traverse vals2
traverse vals3
```

### Head & tail

Going over a list with recursive pattern matching and  
`t::h` cons pattern.  

```F#
open System

printf "Enter a number: "

let value = Console.ReadLine()

let n =
    match Int32.TryParse value with
    | true, num -> num
    | _ -> failwithf "'%s' is not an integer" value


let f = function
    | value when value > 0 -> printfn "positive value"
    | value when value = 0 -> printfn "zero"
    | value when value < 0 -> printfn "negative value"

f (int value)
f n
```
Pattern matching with exception handling.  

### Pattern matching with records

```F#
type User =
    { FirstName: string
      LastName: string
      Occupation: string }

let users =
    [ { FirstName = "John"
        LastName = "Doe"
        Occupation = "gardener" }
      { FirstName = "Jane"
        LastName = "Doe"
        Occupation = "teacher" }
      { FirstName = "Roger"
        LastName = "Roe"
        Occupation = "driver" } ]

for user in users do
    match user with
    | { LastName = "Doe" } -> printfn "%A" user
    | _ -> ignore ()
```

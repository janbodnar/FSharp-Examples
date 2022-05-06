# Match expressions

The match expression provides branching control that  
is based on the comparison of an expression with  
a set of patterns.  

```F#
let vals = [ 1; -3; 5; 6; 0; 4; -9; 11; 22; -7 ]

for wal in vals do

    match wal with
    | n when n < 0 -> printfn "%d is negative" n
    | n when n > 0 -> printfn "%d is positive" n
    | _ -> printfn "zero"
```    
Printing a message for each value of a list.  

```F#
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
type Choices =
    | A
    | B
    | C

let getVal () =
    match Random().Next(1, 4) with
    | 1 -> A
    | 2 -> B
    | 3 -> C
    | x -> failwithf "%i is out of range" x


let chx = [ for _ in 1..7 -> getVal () ]
printfn "%A" chx
```
Generating a list of random choices.  

```F#
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

Another variant of the previous example.  

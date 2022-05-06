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

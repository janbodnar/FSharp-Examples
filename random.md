# Random

Generating random values

## Print random value

```F#
open System

let rnd = new Random()

for _ in 1..20 do
    let r = rnd.Next(1, 100)
    printf "%d " r

printfn ""
```
Print 20 random values between 1..100.  


## Generate a random list of values 

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

## Random vals from endless seq

```F#
open System

let rnd = Random()

let randomVals =
    seq {
        while true do
            yield rnd.Next(100)
    }

let firstTenRandom =
    randomVals
    |> Seq.truncate 10
    |> Seq.sort
    |> Seq.toList

printfn "%A" firstTenRandom
```

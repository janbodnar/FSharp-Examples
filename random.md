# Random

Generating random values became more robust. Since 2016, in .NET Core the default seed has been changed  
from `Environment.TickCount` to `Guid.NewGuid().GetHashCode()`. It makes 'safe' to create  
several random instances in a loop.

## Print random value

```F#
open System

let rnd = new Random()

for _ in 1..20 do
    let r = rnd.Next(1, 100)
    printf "%d " r

printfn ""
```
Print 20 random values between 1 and 99. Upper bound is exclusive.  

## Pick a random value from list

```F#
open System

let words = ["sky"; "cup"; "tall"; "falcon"; "cloud"]

let rnd = Random()
let re = words |> List.item (rnd.Next(words.Length))

printfn "%s" re
```

## Get a list of random integers 

```F#
open System

let rnd = Random()

let rndVals =
    [ for i in 0..100 do
          rnd.Next(1, 101) ]

printfn "%A" rndVals
```
100 random ints from 1..100

## Generate a random list of values 

```F#
open System

type Choice =
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

## Pick random values from list

```F#
open System

let rnd = new Random()

let vals = [ 1..100 ]
let idx = [ for _ in 1..7 -> rnd.Next(0, 100) ]

let res = idx |> List.map (fun e -> (List.item e vals))
printfn "%A" res
```
Pick seven values from a list randomly.  


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

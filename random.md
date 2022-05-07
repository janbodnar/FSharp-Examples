# Random

Generating random values


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

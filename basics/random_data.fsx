open System

let rand = Random()

let randomVals = seq { while true do yield rand.Next(100) }

let firstTenRandom = 
    randomVals
    |> Seq.truncate 10
    |> Seq.sort
    |> Seq.toList

printfn "%A" firstTenRandom

    

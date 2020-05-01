open System

// dotnet run 1 2 3 4

[<EntryPoint>]
let main argv =

    printfn "The arguments are: %A" argv

    // The sumBy function is a combination of map and sum that first
    // projects all elements to a numeric value and then sums the results.

    if Seq.isEmpty argv then

        printfn "program requires arguments"
        Environment.Exit 1

    let sum = argv |> Seq.sumBy int
    let avg = argv |> Seq.map float |> Seq.average

    printfn "The sum is: %d" sum
    printfn "The avg is: %f" avg

    0

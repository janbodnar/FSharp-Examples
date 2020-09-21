[1..10] |> List.filter (fun n -> n % 2 = 0) |> printfn "%A"

[1..10] |> List.filter (fun n -> n % 2 = 0) |> List.iter (printf "%d ")

let vals = [1..5]

let double input = input |> List.map (fun x -> x * 2)
vals |> double |> printf "%A\n"

let triple = List.map((*) 3)
vals |> triple |> printf "%A\n"

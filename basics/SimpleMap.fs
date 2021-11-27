let vals = [1..5]

let double input = input |> List.map (fun x -> x * 2)
vals |> double |> printf "%A\n"

let output = if true then "yes" else "no"
printf "%A\n" output

let triple = List.map((*) 3)
vals |> triple |> printf "%A\n"

let rec quicksort data =
    match data with
    | [] -> []
    | h::t ->
        let lesser = List.filter ((>) h) t
        let greater = List.filter ((<=) h) t
        (quicksort lesser) @[h] @(quicksort greater)


let vals = [4; 2; 1; 3; 7; 6; 5; 8]

let sorted = quicksort vals
printfn "%A" sorted

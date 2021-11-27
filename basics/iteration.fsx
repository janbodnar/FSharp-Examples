let nums = [1 .. 4]

// for loop
let iter1 nums =
    for x in nums do
        printfn "%d" x

// higher-order function
let iter2 nums =
    List.iter (printfn "%d") nums

// recursive function and pattern matching
let rec iter3 nums =
    match nums with
    | [] -> ()
    | h :: t ->
        printfn "%d" h
        iter3 t

iter1 nums
printfn "----------------"

iter2 nums
printfn "----------------"

iter3 nums

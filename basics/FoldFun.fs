let nums = [3;4;5]

// * is a function
let product1 = nums |> List.fold (*) 1
let product2 = nums |> List.fold (fun total n -> total * n) 1


printfn "%d" product1
printfn "%d" product2

// fold is a top-down iteration
// reduce is a special case of fold

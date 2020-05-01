// pattern matching expression
let rec factorial1 n =
    match n with
    | 0 -> 1
    | _ -> n * factorial1 (n - 1)

//  pattern matching function for single-argument function
let rec factorial2 = function
    | 0 -> 1
    | n -> n * factorial2 (n - 1)

// fold and range operator
let factorial3 n = [1..n] |> Seq.fold (*) 1


let num = 10

printfn "%d" (factorial1 num)
printfn "%d" (factorial2 num)
printfn "%d" (factorial3 num)

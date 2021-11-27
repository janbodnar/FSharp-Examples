let mutable x = 10
let y = 20

printfn "x: %i" x
printfn "y: %i" y

x <- 15 // <- instead of =

printfn "-----------------------"

printfn "x: %i" x
printfn "y: %i" y

// arrays are mutable

let vals = [|
    1
    2
    3
    4
|]

vals[0] <- 11
vals[1] <- 12
vals[2] <- 13
vals[3] <- 14

printfn "%A" vals

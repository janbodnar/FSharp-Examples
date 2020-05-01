let inc x = x + 1
let negate x = x * -1
let square x = x * x

let chain x =
    int (negate (square x))

printfn "%d" (chain 7)

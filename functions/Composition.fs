let inc x = x + 1
let negate x = x * -1
let square x = x * x


// forward composition operator
let fcomp1 = inc >> negate >> square

// backward composition operator
let fcomp2 = inc << negate << square

printfn "%d" (fcomp1 10)
printfn "%d" (fcomp2 10)

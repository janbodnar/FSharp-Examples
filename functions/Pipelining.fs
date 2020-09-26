let inc x = x + 1
let negate x = x * -1
let square x = x * x


let pipeline1 x = inc x |> negate |> square
let pipeline2 x = inc <| (negate <| square x)

printfn "%d" (pipeline1 10)
printfn "%d" (pipeline2 10)


// The composition operators take two functions and return a function; by
// contrast, the pipeline operators take a function and an argument and 
// return a value. 

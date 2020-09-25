// Simple add function
let add x y = x + y

// We fix the first argument to derive the increment function
let inc = add 1

printfn "%d = %d" (inc 5) (add 1 5)

//Can we do the same with subtract?
let sub x y = x - y

let decw = sub 1 // WRONG: this is 1 - y and not x - 1!

printfn "%d = %d" (decw 5) (sub 1 5)

// Let's define a function that reorder arguments
let invert2 f y x = f x y

let dec = invert2 sub 1

printfn "%d = %d" (dec 5) (sub 5 1)

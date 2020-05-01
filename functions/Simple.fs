// The let keyword defines a named function.
// no parentheses used for function parameters
let square x = x * x
printfn "%d" (square 3)

let add x y = x + y
printfn "%d" (add 2 3)

// multiline function with indents; no semicolons needed
// isEven  is a nested fun, List.filter is a library function
let evens vals =
    let isEven x = x % 2 = 0
    List.filter isEven vals

let oneToFive = [1; 2; 3; 4; 5]

printfn "%A" (evens oneToFive)

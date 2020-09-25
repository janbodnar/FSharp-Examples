let multiply x y z = x * y * z

printfn "%d" (multiply 3 4 5)

let mul1 = multiply 3
let mul12 = mul1 4

let res = (mul12 5)

printfn "%d" res

printfn "%d" (((multiply 3) 4) 5)

// Currying is a transformation of a function with multiple arguments into a
// sequence of nested functions with a single argument. The currying allows to
// perform function specialization and composition.

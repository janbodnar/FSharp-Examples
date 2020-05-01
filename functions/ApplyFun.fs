let apply (f: int -> int -> int) x y = f x y
let apply2 (f: int -> int -> int) x y = f x y

let mul x y = x * y
let res = apply mul 5 7
printfn "%d" res

let res2 = apply2 (fun x y -> x * y ) 5 7
printfn "%d" res2

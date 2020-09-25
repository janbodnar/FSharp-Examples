//  currying with partial application

let addTwoNumbers x y = x + y
let add7ToNumber = addTwoNumbers 7

printfn "%d" (add7ToNumber 5)

(* --------------------------- *)

let add a b =
    a + b

printfn "%d" (add 2 3)
printfn "%d" ((add 2) 3)

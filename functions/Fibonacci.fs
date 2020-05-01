
let rec fib n =
   match n with
   | 0 -> 0
   | 1 -> 1
   | _ -> fib (n - 1) + fib (n - 2)


for i = 1 to 10 do
  printfn "Fibonacci %d: %d" i (fib i)

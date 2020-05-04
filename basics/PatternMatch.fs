open System

printf "Enter a number: "

let value = Console.ReadLine()

let n =
    match Int32.TryParse value with
    | true, num -> num
    | _ -> failwithf "'%s' is not an integer" value


let f = function
    | value when value > 0 -> printfn "positive value"
    | value when value = 0 -> printfn "zero"
    | value when value < 0 -> printfn "negative value"

f (int value)
f n

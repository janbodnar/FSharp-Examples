// right pipe is useful when we have a chain of pipes, but for one of them you
// want to pass the data in as the first param, rather than the last.


let replace (old:string) _new (a:string) = a.Replace(old, _new)
let add a b = sprintf "%s %s" a b
let toUpper (a:string) = a.ToUpper()

// pipe "hello" through the functions
"hello" 
    |> replace "h" "j"
    |> add "world"  // "world is the first param
    |> toUpper
    |> printfn "%s"

// pipe "hello" through the functions
"hello" 
    |> replace "h" "j"
    |> add <| "world" // "world is the second param
    |> toUpper
    |> printfn "%s"

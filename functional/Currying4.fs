
let names = ["Eric"; "Maria"; "Peter"; "Lucia"]

let greet greeting name = 
    sprintf "%s %s" greeting name

// for name in names do 
//     printfn "%s" (greet "Hello" name)

let greeting = "Hello"

let res =
    names 
    |> Seq.map (greet greeting)
 
res |> Seq.iter (printfn "%s")

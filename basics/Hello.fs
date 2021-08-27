let greet name =
    printfn "Hello %s" name

[<EntryPoint>]
let main argv =

    printfn "Hello there"
    greet "Lucia"
    
    0 // forgetting a return value is an error in main

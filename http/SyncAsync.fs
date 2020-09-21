open System
open FSharp.Data

[<EntryPoint>]
let main argv =

    // asynchronous
    async { let! html = Http.AsyncRequestString("http://webcode.me")
            printfn "%d" html.Length 
            printfn "%s" html
    } |> Async.Start
    
    // synchronous
    let testHtml = Http.RequestString("http://test.webcode.me")
    printfn "%s" testHtml

    Console.ReadKey() |> ignore

    0 

// dotnet add package FSharp.Data

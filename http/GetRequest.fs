open FSharp.Data.HttpRequestHeaders
open FSharp.Data

[<EntryPoint>]
let main argv =

    // HTTP GET request
    let data = 
        Http.RequestString("https://httpbin.org/get",
            query   = [ "name", "John Doe"; "occupation", "gardener" ],
            headers = [ Accept HttpContentTypes.Json ])

    printfn "%s" data

    0 

// dotnet add package FSharp.Data

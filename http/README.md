# HTTP requests

## GET request with FSharp.Data  

Addding query parameters and setting a header option.  

```F#
open FSharp.Data.HttpRequestHeaders
open FSharp.Data

[<EntryPoint>]
let main argv =

    let data = 
        Http.RequestString("https://httpbin.org/get",
            query   = [ "name", "John Doe"; "occupation", "gardener" ],
            headers = [ Accept HttpContentTypes.Json ])

    printfn "%s" data

    0 

// dotnet add package FSharp.Data
```

## JSON POST request with FSharp.Data
 
Since `{ }` characters are used for string interpolation and JSON also uses  
these characters, we need to double the {{ and }} or use `sprintf` function.  

```F#
#r "nuget: FSharp.Data"

open FSharp.Data

let myurl = fsi.CommandLineArgs[1]
let key = "key-id"

let url =
    $"https://api-gateway.example.com/gateway/clearcache?developerKey={key}"

let rb = TextRequest (sprintf """{ "url": "%s" }""" myurl)
let rb2 = TextRequest $"""{{ "url": "{myurl}" }}"""

printfn "%A" rb
printfn "%A" rb2

let res =
    Http.RequestString(
        url,
        httpMethod = "POST",
        body = rb2,
        headers = [ ("Content-Type", "application/json") ]
    )

printfn "%s" res
```

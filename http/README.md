# HTTP requests

## 

```f#
#r "nuget: FSharp.Data"

open FSharp.Data

let myurl = fsi.CommandLineArgs[1]
let key = "key-id"

let url =
    $"https://api-gateway.ezoic.com/gateway/cdnservices/clearcache?developerKey={key}"

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

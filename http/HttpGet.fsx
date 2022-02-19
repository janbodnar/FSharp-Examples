#r "nuget: FsHttp"

open FsHttp
open System
open FsHttp.DslCE

let resp = get "http://webcode.me" { send }
Console.WriteLine resp.content.Headers
Console.WriteLine resp.statusCode
Console.WriteLine resp.content

resp |>  Response.toText |> (Console.WriteLine)

# HttpClient

## Download image async

```F#
open System.Net.Http
open System.IO

let getImage (url: string) =

    task {
        use httpClient = new HttpClient()
        let! imageBytes = httpClient.GetByteArrayAsync(url)

        return imageBytes
    }


let url = "http://webcode.me/favicon.ico"

let data =
    getImage (url)
    |> Async.AwaitTask
    |> Async.RunSynchronously

File.WriteAllBytes("favicon.ico", data)
```


## Execute multiple requests async

```F#
open System.Net.Http
open System.Text.RegularExpressions
open System.Threading.Tasks

let fetchTitleAsync (url: string) =

    task {

        use client = new HttpClient()
        let! html = client.GetStringAsync(url)
        let pattern = "<title>\s*(.+?)\s*</title>"

        let m = Regex.Match(html, pattern)
        return m.Value
    }

let sites =
    [| "http://webcode.me"
       "http://example.com"
       "https://bing.com"
       "http://httpbin.org"
       "https://ifconfig.me"
       "http://termbin.com"
       "https://github.com" |]

let titles =
    sites
    |> Array.map fetchTitleAsync
    |> Task.WhenAll
    |> Async.AwaitTask
    |> Async.RunSynchronously

titles
|> Array.iter (fun title -> printfn $"%s{title}")
```

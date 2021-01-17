open System.IO
open System.Net

let downloadPage (url: string) =
    async {
        let req = HttpWebRequest.Create(url)
        use! resp = req.AsyncGetResponse()
        use respStream = resp.GetResponseStream()
        use sr = new StreamReader(respStream)
        return sr.ReadToEnd()
    }

let parallelDownload () =
    let sites =
        [ "http://webcode.me"
          "http://test.webcode.me" ]

    let htmlOfSites =
        Async.Parallel [ for site in sites -> downloadPage site ]
        |> Async.RunSynchronously

    printfn "%A" htmlOfSites

parallelDownload ()

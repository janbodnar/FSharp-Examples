open System.Text
open System.Net
open System.IO

let url = "https://httpbin.org/post"

let req =
    HttpWebRequest.Create(url) :?> HttpWebRequest
req.Method <- "POST"

// Encode body with POST data as array of bytes
let postBytes =
    Encoding.ASCII.GetBytes("name=John Doe&occupation=gardener")

req.ContentType <- "application/x-www-form-urlencoded"
req.ContentLength <- int64 postBytes.Length

// Write data to the request
let reqStream = req.GetRequestStream()
reqStream.Write(postBytes, 0, postBytes.Length)
reqStream.Close()

let resp = req.GetResponse() 
let stream = resp.GetResponseStream() 
let reader = new StreamReader(stream) 
let html = reader.ReadToEnd()

printfn "%s" html

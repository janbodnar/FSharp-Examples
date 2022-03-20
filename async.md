# Asynchronous programming

## Primes

```F#
let isPrime (n:int) =
   let bound = int (sqrt (float n))
   seq {2 .. bound} |> Seq.forall (fun x -> n % x <> 0)

// async workflow
let primeAsync n =
    async { return (n, isPrime n) }

/// Return primes between m and n using multiple threads
let primes m n =
    seq {m .. n}
        |> Seq.map primeAsync
        |> Async.Parallel
        |> Async.RunSynchronously
        |> Array.filter snd
        |> Array.map fst

primes 1000000 1002000
    |> Array.iter (printfn "%d")
```

## Multiple HTTP requests

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

## Async read files

```F#
open System.IO

let readFilesTask path1 path2 =
    task {
        let! bytes1 = File.ReadAllBytesAsync path1
        let! bytes2 = File.ReadAllBytesAsync path2
        return Array.append bytes1 bytes2
    }

let printBytes data =

    let row data =
        data |> Array.iter (fun e -> printf "%d " e)
        printf "\n"

    let chunked = data |> Array.chunkBySize 10
    chunked |> (Array.map row)

let fname1 = "words.txt"
let fname2 = "words2.txt"

readFilesTask fname1 fname2
|> Async.AwaitTask
|> Async.RunSynchronously
|> printBytes
```

## Async requests with Playwright

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright

let selectParagraphs () =

    task {
        use! pw = Playwright.CreateAsync()
        let! browser = pw.Chromium.LaunchAsync()

        let! page = browser.NewPageAsync()
        let! _ = page.GotoAsync("http://webcode.me")
        
        let! es1 = page.QuerySelectorAllAsync("p")

        for e in es1 do
            let! r = e.TextContentAsync()
            printfn "%s" (r)


        let! e2 = page.QuerySelectorAsync("p")
        let! r2 = e2.TextContentAsync()

        printfn "%s" r2
    }

selectParagraphs ()
|> Async.AwaitTask
|> Async.RunSynchronously
```


## Builder

```F#
#r "nuget: Microsoft.Playwright"
// I am lazy and I don't want to convert Task to FSharpAsync all the time. Let's
// use Task computation expression from Ply nuget package:
// #r "nuget: Ply"

// https://mcode.it/blog/2021-07-16-playwright/?hmsr=joyk.com&utm_source=joyk.com&utm_medium=referral

open Microsoft.Playwright
// open FSharp.Control.Tasks
open System.Threading.Tasks

type PlaywrightBuilder() =
    // Required - creates default value, which returned value can be passed over
    // next custom keywords. The type returned by the next methods has to
    // conform to the returned value type. So each function will have such
    // signature; Task<IPage> * ... (our parameters) ... -> Task<IPage>.
    member _.Yield _ =
        task {
            let! pw = Playwright.CreateAsync()
            let! browser = pw.Firefox.LaunchAsync(BrowserTypeLaunchOptions(Headless = true))
            return! browser.NewPageAsync()
        }

    // CustomOperation attribute is the keyword definition that makes it
    // possible to use in our computation expression.
    [<CustomOperation "visit">]
    member _.Visit(page: Task<IPage>, url) =
        task {
            let! page = page
            let! _ = page.GotoAsync(url)
            return page
        }

    // And now we go with the repeatable boring stuff...
    [<CustomOperation "screenshot">]
    member _.Screenshot(page: Task<IPage>, name) =
        task {
            let! page = page
            let! _ = page.ScreenshotAsync(PageScreenshotOptions(Path = $"{name}.png"))
            return page
        }

    [<CustomOperation "write">]
    member _.Write(page: Task<IPage>, selector, value) =
        task {
            let! page = page
            let! _ = page.FillAsync(selector, value)
            return page
        }

    [<CustomOperation "click">]
    member _.Click(page: Task<IPage>, selector) =
        task {
            let! page = page
            let! _ = page.ClickAsync(selector)
            return page
        }

    [<CustomOperation "wait">]
    member _.Wait(page: Task<IPage>, seconds) =
        task {
            let! page = page
            let! _ = page.WaitForTimeoutAsync(seconds)
            return page
        }

    [<CustomOperation "waitFor">]
    member _.WaitFor(page: Task<IPage>, selector) =
        task {
            let! page = page
            let! _ = page.WaitForSelectorAsync(selector)
            return page
        }
// Let's create our computation expression and use it!
let playwright = PlaywrightBuilder()

playwright {
    visit "https://duckduckgo.com/"
    write "input" "mcode.it"
    click "input[type='submit']"
    click "text=mcode.it - Marcin Golenia Blog"
    waitFor "text=Yet another IT blog?"
    screenshot "mcode-screen"
}
|> Async.AwaitTask
|> Async.RunSynchronously
```
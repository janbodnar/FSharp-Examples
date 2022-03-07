# Playwright

## Get title

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright
open System

let url = "http://webcode.me"

let getTitle() =
    task {
        use! pw = Playwright.CreateAsync()
        let! browser = pw.Chromium.LaunchAsync()
        let! page = browser.NewPageAsync()
        let! _ = page.GotoAsync(url)

        return! page.TitleAsync()
    }

getTitle()
|> Async.AwaitTask
|> Async.RunSynchronously
|> Console.WriteLine
```

## Get user agent

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright
open System.Text
open System

let getUserAgent (url: string) (ehds: Map<string, string>) =
    task {
        use! web = Playwright.CreateAsync()
        let! browser = web.Chromium.LaunchAsync()
        let! page = browser.NewPageAsync()

        do! page.SetExtraHTTPHeadersAsync ehds

        let! resp = page.GotoAsync(url)
        let! body = resp.BodyAsync()

        return Encoding.UTF8.GetString(body)
    }

let url = "http://webcode.me/ua.php"
let ehds = Map ["User-Agent", "F# script"]

getUserAgent url ehds
|> Async.AwaitTask
|> Async.RunSynchronously
|> Console.WriteLine
```

## Intercept request & response

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright
open System.Text
open System


let getUserAgent (url: string) =
    task {
        use! web = Playwright.CreateAsync()

        let opts = BrowserTypeLaunchOptions(Headless = false)

        let! browser = web.Chromium.LaunchAsync(opts)
        let! page = browser.NewPageAsync()

        page.Request.AddHandler(fun _ req -> Console.WriteLine($">> {req.Method} {req.Url}"))
        page.Response.AddHandler(fun _ resp -> Console.WriteLine($"<< {resp.Status} {resp.Url}"))

        let! resp = page.GotoAsync(url)
        let! body = resp.BodyAsync()

        return Encoding.UTF8.GetString(body)
    }

let url = "http://webcode.me/"

getUserAgent url
|> Async.AwaitTask
|> Async.RunSynchronously
|> Console.WriteLine
```

## Take screenshot

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright

let createScreenshot () =
    task {
        use! web = Playwright.CreateAsync()
        let! browser = web.Chromium.LaunchAsync()
        let! page = browser.NewPageAsync()
        let! _ = page.GotoAsync("http://webcode.me")
        let screenshotOptions = PageScreenshotOptions()
        screenshotOptions.Path <- "webcode.png"

        let! _ = page.ScreenshotAsync(screenshotOptions)
        ()
    }

createScreenshot ()
|> Async.AwaitTask
|> Async.RunSynchronously

printfn "Screenshot created"
```

## Click element

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright

let clickElement () =

    task {
        use! pw = Playwright.CreateAsync()
        let! browser = pw.Chromium.LaunchAsync()

        let! page = browser.NewPageAsync()

        let! _ = page.GotoAsync("http://example.com")
        do! page.ClickAsync("a")

        return! page.TitleAsync()
    }

clickElement ()
|> Async.AwaitTask
|> Async.RunSynchronously
|> printfn "%s"
```

## Query selectors


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


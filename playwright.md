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

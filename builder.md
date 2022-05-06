# Builders 

## Playwright builder

```F#
#r "nuget: Microsoft.Playwright"

open Microsoft.Playwright
open System.Threading.Tasks

type PlaywrightBuilder() =

    member _.Yield _ =
        task {
            let! pw = Playwright.CreateAsync()
            let! browser = pw.Firefox.LaunchAsync(BrowserTypeLaunchOptions(Headless = true))
            return! browser.NewPageAsync()
        }

    [<CustomOperation "visit">]
    member _.Visit(page: Task<IPage>, url) =
        task {
            let! page = page
            let! _ = page.GotoAsync(url)
            return page
        }

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

let playwright = PlaywrightBuilder()

playwright {
    visit "https://duckduckgo.com/"
    write "input" "root.cz"
    click "input[type='submit']"
    click "text=/Root\.cz/"
    waitFor "text=Root.cz - informace nejen ze světa Linuxu"
    screenshot "root"
}
|> Async.AwaitTask
|> Async.RunSynchronously
```

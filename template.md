# Templates

# Simple template

We use Scriban to generate HTML data by merging HTML template with data.  

```F#
#r "nuget: Scriban"

open Scriban

type User = { name: string; occupation: string }

let renderHtml users =
    let html =
        """
        <ul>
        {{- for user in users }}
          <li> {{ user.name }}: {{ user.occupation }} </li>
        {{- end }}
        </ul>
        """

    let result = Template.Parse(html)
    result.Render({| users = users |})


let users =
    [ { name = "John Doe"
        occupation = "gardener" }
      { name = "Roger Roe"
        occupation = "driver" }
      { name = "Peter Smith"
        occupation = "shopkeeper" } ]


let result = renderHtml users

printfn "%s" result
```

# Generate PDF from HTML file

Create HTML data from a template & generate a PDF file from this  
data. The example uses Scriban & Playwright.  

```HTML
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
</head>
<body>

    <p>
        Hello {{model.name}}!
    </p>
    
</body>
</html>
```


```F#
#r "nuget: Scriban"
#r "nuget: Microsoft.Playwright"

open System.IO
open Scriban
open System
open System.Text
open Microsoft.Playwright


let getHtml fileName : string =

    let tempFile = File.ReadAllText(fileName)
    let tpl = Template.Parse(tempFile)

    let model = Map [ "name", "John Doe" ]
    let msg = tpl.Render({| model = model |})

    printfn "%s" msg

    msg


let genPdf (data: string) =

    let base64 =  Convert.ToBase64String(Encoding.UTF8.GetBytes(data))
    let dataUrl = $"data:text/html;base64,{base64}"
    
    // let url = $"data:text/html,{data}"
    // printfn "%s" dataUrl
    // printfn "%s" url

    task {

        use! playwright = Playwright.CreateAsync()        
        let! browser = playwright.Chromium.LaunchAsync()

        let! context = browser.NewContextAsync()
        let! page = context.NewPageAsync()
        let! _ = page.GotoAsync(dataUrl)

        let! output = page.PdfAsync(PagePdfOptions(Format = "A4", Landscape = false))

        File.WriteAllBytesAsync("output.pdf", output)
        |> ignore

        ()
    }


let fileName = "template.html"
let msg = getHtml fileName

genPdf msg
|> Async.AwaitTask
|> Async.RunSynchronously
```

A specific data url is passed to the `page.GotoAsync` method.  


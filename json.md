# JSON

With built-in `System.Text.Json`  

## Parse JSON

```F#
open System.Text.Json
open System

let data =
    """[ {"name": "John Doe", "occupation": "gardener"}, 
    {"name": "Peter Novak", "occupation": "driver"} ]"""

let r = JsonDocument.Parse data

let root = r.RootElement
let u1 = root[0]
let u2 = root[1]

Console.WriteLine u1
Console.WriteLine u2

Console.WriteLine(u1.GetProperty("name"))
Console.WriteLine(u1.GetProperty("occupation"))
```

## Serialize JSON

```F#
open System.Text.Json
open System

let nums = [ 1; 2; 3; 4; 5; 6 ]

let r = JsonSerializer.Serialize nums
Console.WriteLine r

let d = JsonSerializer.Deserialize r

d |> List.iter (fun e -> printfn "%d" e)

Console.WriteLine "----------------------------------"

d |> List.map int |> List.sum |> Console.WriteLine

Console.WriteLine "----------------------------------"

let words =
    Map [ 1, "wood"
          2, "rock"
          3, "ocean"
          4, "forest"
          5, "plant" ]

let options = new JsonSerializerOptions(WriteIndented = true)
let r2 = JsonSerializer.Serialize(words, options)
Console.WriteLine r2

let d2: Map<int, string> = JsonSerializer.Deserialize r2

for e in d2.Keys do 
    Console.WriteLine e

for e in d2.Values do 
    Console.WriteLine e
```
# Maps


```F#
open System

let words = Map [1, "book"; 2, "sky"; 3, "work"; 4, "cloud"]

printfn "%A" words

words |> Map.iter (fun k v -> Console.WriteLine($"{k}: {v}"))

for key in words.Keys do
    Console.WriteLine key

for value in words.Values do
    Console.WriteLine value
```

```F#
open System

let words =
    Map [ 1, "book"
          2, "sky"
          3, "work"
          4, "cloud"
          5, "water"
          6, "war" ]

for p in words do
    Console.WriteLine $"{p.Key}: {p.Value}"

Console.WriteLine "--------------------------------"

Console.WriteLine words.Count

Console.WriteLine words[1]
Console.WriteLine words[2]
Console.WriteLine words[3]

Console.WriteLine "--------------------------------"

words
|> Map.filter (fun _ v -> v.Contains "w")
|> Map.values
|> Seq.iter Console.WriteLine
```
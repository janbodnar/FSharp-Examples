# Maps


## Map

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

## Filter & count

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

## Map.empty

`Map.empty` creates an empty map.  

```F#
type User = {
    Name: string
    Occupation: string
}

let users =
   Map.empty.
      Add(1, {Name="John Doe"; Occupation="gardener"}).
      Add(2, {Name="Roger Roe"; Occupation="driver"}).
      Add(3, {Name="Lucy Smith"; Occupation="teacher"}).
      Add(4, {Name="Tom Jones"; Occupation="programmer"})

users |> Map.iter (fun k v -> printfn $"{k}: {v}")
```

## Map.ofList

```F#
let words = ["sky"; "cup"; "rock"; "pen"; "pearl"; "cloud"]
let n = words.Length
let idxs = [1..n]

let data = List.zip idxs words
printfn "%A" data

printfn "-----------------"

let m = data |> Map.ofList

m |> Map.iter (fun k v -> printfn $"{k}: {v}");
```


## List of maps

```F#
let fruits1 = Map [ "oranges", 2; "bananas", 3 ]
let fruits2 = Map [ "plums", 2; "kiwis", 3 ]

let fruits = [ Map[1, fruits1]; Map[2, fruits2] ]

fruits
|> List.iter (Map.iter (fun k v -> printfn $"{k} {v}"))

for nested in fruits do
  for e in nested do
        printfn $"{e.Key} {e.Value}"
```

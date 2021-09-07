# F# regular expressions

## find matches and their indexes

Example I

```
open System.Text.RegularExpressions

let content =
    @"Foxes are omnivorous mammals belonging to several genera
of the family Canidae. Foxes have a flattened skull, upright triangular ears,
a pointed, slightly upturned snout, and a long bushy tail. Foxes live on every
continent except Antarctica. By far the most common and widespread species of
fox is the red fox."


let found =
    seq {
        for m in Regex.Matches(content, "(?i)fox(es)?") do
            yield m.Value, m.Index
    }

found
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

Example II

```
open System.Text.RegularExpressions

let content =
    @"Foxes are omnivorous mammals belonging to several genera
of the family Canidae. Foxes have a flattened skull, upright triangular ears,
a pointed, slightly upturned snout, and a long bushy tail. Foxes live on every
continent except Antarctica. By far the most common and widespread species of
fox is the red fox."


let found =
    Regex.Matches(content, "(?i)fox(es)?")
    |> Seq.map (fun m -> m.Value, m.Index)

found
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```


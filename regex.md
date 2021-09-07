# F# regular expressions



## Matching with IsMatch

```
open System
open System.Text.RegularExpressions

let words =
    [ "Seven"
      "even"
      "Maven"
      "Amen"
      "eleven" ]

let rx = Regex(@".even", RegexOptions.Compiled)

words
|> List.map
    (fun e ->
        if rx.IsMatch(e) then
            Console.WriteLine($"{e} matches")
        else
            Console.WriteLine($"{e} does not match"))
```

## Alternations

```
open System
open System.Text.RegularExpressions

let users =
    [ "Jane"
      "Thomas"
      "Robert"
      "Lucy"
      "Beky"
      "John"
      "Peter"
      "Andy" ]

let rx =
    Regex("Jane|Beky|Robert", RegexOptions.Compiled)


users |> List.filter rx.IsMatch |> List.iter Console.WriteLine
```


## Find all matches and their indexes

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

## Boundaries

```
open System.Text.RegularExpressions

let text = "This island is beautiful"

let rx = Regex(@"\bis\b", RegexOptions.Compiled)

let matches =
    rx.Matches(text)
    |> Seq.map (fun m -> m.Value, m.Index)

matches
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

## Capturing groups

```
open System.Text.RegularExpressions

let sites =
    [ "webcode.me"
      "zetcode.com"
      "spoznaj"
      "freebsd.org"
      "netbsd.org" ]

let rx =
    Regex(@"(\w+)\.(\w+)", RegexOptions.Compiled)


let check e =
    let m = rx.Match(e)
    (m.Value, m.Groups.[1], m.Groups.[2])


// let found = sites |> List.map (fun e -> check (e))
let found = sites |> List.map check

printfn "%A" found
```


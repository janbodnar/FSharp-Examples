# Lists

## Simple

```F#
let vals = [ 1; 2; 3; 4; 5; 6 ]

printfn "%A" vals
printfn "%d" vals.Head
printfn "%d" vals.Length
printfn "%A" vals.Tail
```

## Iteration

```F#
let vals = [ 1; 2; 3; 4; 5; 6 ]

vals |> List.iter (printfn "%d")

printfn "------------------------"

for e in vals do 
    printfn "%d" e
```

## Merge lists

```F#
let a = [1; 2; 3; 4]
let b = [4; 4; 5; 6]

let merged = a @ b |> List.distinct
printfn "%A" merged
```

## List comprehensions

List comprehension is a powerful syntax to generate lists.  

```F#
let vals = [ -1; 0; 2; -2; 1; 3; 4; -6 ]

let pos =
    [ for e in vals do
          if e > 0 then yield e ]

printfn "%A" pos

printfn "---------------------------------"

[ for e in 1 .. 100 -> e * e ] |> printfn "%A"


printfn "---------------------------------"

[ for a in 1 .. 100 do
    if a % 3 = 0 && a % 5 = 0 then yield a] |> printfn "%A"

printfn "---------------------------------"

let vals3 =
    [ for x in 1 .. 3 do
          for y in 1 .. 10 -> x, y ]

printfn "%A" vals3
```
---

```F#
let vals = [ 1; -2; -3; 4; 5 ]

[ for v in vals do
      let f = fun e -> e > 0
      if f(v) then yield v ] |> printfn "%A"
```
let expressions can be used inside comprehensions  

## List.forall

```F#
let nums = [ -1; -2; 3; 0; -4; 5; 6 ]

let words =
    [ "war"
      "water"
      "wrath"
      "warm"
      "wrong" ]


let isAllPositive data = List.forall (fun e -> e > 0) data

let allBeginWith (data: list<string>) (c: string) =
    List.forall (fun (e: string) -> e.StartsWith(c)) data

printfn "%b" (isAllPositive nums)
printfn "%b" (allBeginWith words "w")
```
# Lists

## Simple

```F#
let vals = [ 1; 2; 3; 4; 5; 6 ]

printfn "%A" vals
printfn "%d" vals.Head
printfn "%d" vals.Length
printfn "%A" vals.Tail
```

## Indexes

List elements are accessed through their indexes.  

```F#
let words = ["pen"; "cup"; "dog"; "person";
    "cement"; "coal"; "spectacles"; "cup"; "bread"]

let w1 = List.item 1 words
printfn "%s" w1

let w2 = words[0]
printfn "%s" w2

let i1 = List.findIndex(fun e -> e = "cup") words
printfn $"The first index of cup is {i1}"

let i2 = List.findIndexBack(fun e -> e = "cup") words
printfn $"The last index of cup is {i2}"
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

## Sorting 

```F#
let nums = [ -1; 6; -2; 3; 0; -4; 5; 1; 2 ]

nums |> List.sort |> printfn "%A" 
nums |> List.sortDescending |> printfn "%A" 

nums |> List.sortBy (abs) |> printfn "%A" 
nums |> List.sortByDescending (abs) |> printfn "%A" 
```

```F#
type User =
    { Name: string
      Occupation: string
      Salary: int }

let users =
    [ { Name = "John Doe"
        Occupation = "gardener"
        Salary = 1280 }
      { Name = "Roger Roe"
        Occupation = "driver"
        Salary = 860 }
      { Name = "Tom Brown"
        Occupation = "shopkeeper"
        Salary = 990 } ]

users
|> List.sortBy (fun u -> u.Salary)
|> List.iter (fun u -> printfn "%A" u)

printfn "--------------------------------"

users
|> List.sortByDescending (fun u -> u.Occupation)
|> List.iter (fun u -> printfn "%A" u)
```

## List comprehensions

List comprehension is a powerful syntax to generate lists.  
In F#, we can create list comprehensions with ranges and generators. 

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

// need type annotations b/c compiler had problems determining types
let allBeginWith (data: list<string>) (c: string) =
    List.forall (fun (e: string) -> e.StartsWith(c)) data

printfn "%b" (isAllPositive nums)
printfn "%b" (allBeginWith words "w")
```

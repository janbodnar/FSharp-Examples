# Sorting 

## Ints and strings 

```F#
let nums = [ 7; 9; 3; -2; 8; 1; 0 ]

let words =
    [ "sky"
      "cloud"
      "atom"
      "brown"
      "den"
      "kite"
      "town" ]

List.sort nums |> printfn "%A" 
List.sortDescending nums |> printfn "%A" 
List.sort words |> printfn "%A" 
List.sortDescending words |> printfn "%A" 
```

## Words ci

```F#
let words =
    [ "sky"
      "Sun"
      "Albert"
      "cloud"
      "by"
      "Earth"
      "else"
      "atom"
      "brown"
      "a"
      "den"
      "kite"
      "town" ]

words |> List.sort |> printfn "%A" 
words |> List.sortBy (fun e -> e.ToLower()) |> printfn "%A" 
```

## Sort ints lex. & words by length

```F#
let vals = [2; 22; 12; 7; 32; 9; 3; 13; 31; -12; 8; 10; 11]
let words = ["sky"; "cloud"; "by"; "atomic"; "brown"; "a"; "den"; "curious"]

vals |> List.sortBy string |> printfn "%A" 
words |> List.sortBy (fun e -> e.Length) |> printfn "%A" 
```

## Sort tuples

```F#
let vals =
    [ (1, 3)
      (4, 3)
      (3, 0)
      (4, 0)
      (0, 1)
      (0, 3)
      (2, 2)
      (0, 0)
      (1, 1)
      (3, 3) ]

vals |> List.sortBy (fun (e, _) -> e) |> printfn "%A" 
vals |> List.sortBy fst |> printfn "%A" 

printfn "------------------------" 

vals |> List.sortBy (fun (_, e) -> e) |> printfn "%A" 
vals |> List.sortBy snd |> printfn "%A" 
```

---

```F#
let students =
    [ ("Patrick", 89)
      ("Lucia", 92)
      ("Veronika", 72)
      ("Robert", 78)
      ("Maria", 65)
      ("Andrea", 51)
      ("Ondrej", 45) ]


students |> List.sortBy fst |> printfn "%A" 
students |> List.sortByDescending fst |> printfn "%A" 

printfn "------------------------------------" 

students |> List.sortBy snd |> printfn "%A" 
students |> List.sortByDescending snd |> printfn "%A" 
```

## Sort records

```F#
open System

type User =
    { FirstName: string
      LastName: string
      Salary: int }

let users =
    [ { FirstName = "John"
        LastName = "Doe"
        Salary = 1230 }
      { FirstName = "John"
        LastName = "Doe"
        Salary = 1230 }
      { FirstName = "Lucy"
        LastName = "Novak"
        Salary = 670 }
      { FirstName = "Ben"
        LastName = "Walter"
        Salary = 2050 }
      { FirstName = "Robin"
        LastName = "Brown"
        Salary = 2300 }
      { FirstName = "Joe"
        LastName = "Draker"
        Salary = 1190 }
      { FirstName = "Janet"
        LastName = "Doe"
        Salary = 980 } ]

users |> List.sortBy (fun u -> u.LastName) |> List.iter Console.WriteLine

Console.WriteLine "---------------------"

users |> List.sortBy (fun u -> u.Salary) |> List.iter Console.WriteLine
```
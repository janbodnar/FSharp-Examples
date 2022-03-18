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

## Sort descending

```F#
module Operators =
    let flip f x y = f y x

let vals = [ 2; 1; 0; -1; -2; 4; 5; 3; 9; 7; -8 ]
// let vals = [ -2147483648; -1; 0; 1; 1; 2147483647 ]

vals
|> List.sortByDescending (fun e -> e)
|> printfn "%A"

vals |> List.sortByDescending id |> printfn "%A"
vals |> List.sortBy (fun e -> -e) |> printfn "%A" // problems with overflow
vals |> List.sortBy (~~~) |> printfn "%A"
vals |> List.sortBy (~-) |> printfn "%A" // problems with overflow

vals
|> List.sortBy (fun e -> ~~~e)
|> printfn "%A"

vals
|> List.sortWith (Operators.flip compare)
|> printfn "%A"
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

## Sort by surnames

```F#
let names =
    [ "John Doe"
      "Lucy Smith"
      "Benjamin Young"
      "Robert Brown"
      "Thomas Moore"
      "Linda Black"
      "Adam Smith"
      "Jane Smith" ]

names
|> List.sortBy (fun e -> e.Split(" ")[1])
|> printfn "%A"

names
|> List.sortBy (fun e -> let a = e.Split(" ") in Array.get a 1)
|> printfn "%A"
```

## Sort by multiple fields

```F#
type User =
    { FirstName: string
      LastName: string
      Salary: int }
    override this.ToString() =
        $"{this.FirstName} {this.LastName}, {this.Salary}"

let users =
    [ { FirstName = "John"
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
      { FirstName = "Vivien"
        LastName = "Doe"
        Salary = 1010 }
      { FirstName = "Joe"
        LastName = "Draker"
        Salary = 1190 }
      { FirstName = "Albert"
        LastName = "Novak"
        Salary = 1930 }
      { FirstName = "Janet"
        LastName = "Doe"
        Salary = 980 }
      { FirstName = "Ken"
        LastName = "Novak"
        Salary = 2990 } ]

users
|> List.sortBy (fun e -> e.LastName, e.Salary)
|> List.iter (fun e -> printfn $"{e}")
```

## Custom comparator

The example compares words by lengh; if they are of same  
length then by alphabetical order, case insensitively

```F#
let cmp (s1:string) (s2:string) = function
  match compare s2.Length s1.Length with
    | 0 -> compare (s1.ToLower()) (s2.ToLower())
    | x -> x
 
let words =
    [ "sky"
      "eye"
      "Sun"
      "Albert"
      "cloud"
      "by"
      "Earth"
      "else"
      "of"
      "atom"
      "brown"
      "lie"
      "a"
      "be"
      "den"
      "kite"
      "to"
      "town" ]
 
let sorted = List.sortWith cmp words
printfn "%A" sorted
```

## Sort by rating

```F#
type Rating =
    | APlus = 1
    | A = 2
    | AMinus = 3
    | BPlus = 4
    | B = 5
    | BMinus = 6
    | CPlus = 7
    | C = 8
    | CMinus = 9
    | DPlus = 10
    | D = 11

type Product =
    { Name: string
      Rating: Rating }
    override this.ToString() = $"{this.Name}, {this.Rating}"

let products =
    [ { Name = "Product A"
        Rating = Rating.A }
      { Name = "Product B"
        Rating = Rating.AMinus }
      { Name = "Product C"
        Rating = Rating.B }
      { Name = "Product D"
        Rating = Rating.APlus }
      { Name = "Product E"
        Rating = Rating.D }
      { Name = "Product F"
        Rating = Rating.C }
      { Name = "Product G"
        Rating = Rating.CMinus }
      { Name = "Product G"
        Rating = Rating.CPlus } ]

products
|> List.sortBy (fun e -> e.Rating)
|> List.iter (printfn "%O")
```
# Match expressions

The match expression provides branching control that is based on  
the comparison of an expression with a set of patterns. In addition to  
pattern matching expression we have pattern matching function.  

The function version is a short hand for the full match syntax in the  
special case where the match statement is the entire function and the  
function only has a single argument (tuples count as one).

## String constant 

```F#
open System
open System.Globalization

printf "What is the capital of Slovakia?: "

let name = Console.ReadLine() 
let lowered = name.ToLower()
let capital = CultureInfo.CurrentCulture.TextInfo.ToTitleCase lowered

let msg = match capital with
            | "Bratislava" -> "correct answer"
            | _ -> "wrong answer"


printfn $"{msg}"
```

## Multiple options

Multiple options are separeted with |. 

```F#

let grades = ["A"; "B"; "C"; "D"; "E"; "F"; "FX"]

for grade in grades do

    match grade with
    | "A" | "B" | "C" | "D" | "E" | "F" -> printfn "%s" "passed"
    | _ -> printfn "%s" "failed"
```

## Guards

Printing a message for each value of a list using `when` guards.  
With `_`  we create an exhaustive matching.  

```F#
let vals = [ 1; -3; 5; 6; 0; 4; -9; 11; 22; -7 ]

for wal in vals do

    match wal with
    | n when n < 0 -> printfn "%d is negative" n
    | n when n > 0 -> printfn "%d is positive" n
    | _ -> printfn "zero"
```    
 
## Factorial

```F#
let rec factorial (n: bigint): bigint =

    match n with
    | n when n = 0I || n = 1I -> 1I
    | n -> n * factorial (n - 1I)


for i in 0I..32I do

    let f = factorial(i)
    printfn $"{i} {f}"
```

## Enums 

```F#
open System

type Day =
    | Monday
    | Tuesday
    | Wednesday
    | Thursday
    | Friday
    | Saturday
    | Sunday


let days = [ Monday; Tuesday; Wednesday; Thursday; 
    Friday; Saturday; Sunday ]

let rnd = new Random()

let res = days 
        |> Seq.sortBy (fun _ -> rnd.Next()) 
        |> Seq.take 3

for e in res do

    match e with
    | Monday -> printfn "%s" "monday"
    | Tuesday ->  printfn "%s" "tuesday"
    | Wednesday ->  printfn "%s" "wednesay"
    | Thursday ->  printfn "%s" "thursday"
    | Friday ->  printfn "%s" "friday"
    | Saturday ->  printfn "%s" "saturday"
    | Sunday ->  printfn "%s" "sunday"
```

## 2-e lists

```F#
let vals =
    [ [ 1; 2; 3 ]
      [ 1; 2 ]
      [ 3; 4 ]
      [ 8; 8 ]
      [ 0 ] ]

let twoels (sub: int list) =
    match sub with
    | [ x; y ] -> printfn "%A" [ x; y ]
    | _ -> ()

for sub in vals do
    twoels sub
```
Printing 2-e sublists with pattern matching and for loop.  

```F#
let vals =
    [ [ 1; 2; 3 ]
      [ 1; 2 ]
      [ 3; 4 ]
      [ 6; 5; 3; 2; 4; 5 ]
      [ 8; 8 ]
      [ 0 ] ]

let twoels =
    function
    | [ x; y ] -> printfn "%A" [ x; y ]
    | _ -> ()

vals |> List.map twoels
```

Printing 2-e sublists with pattern matching and List.map.  

```F#
let vals =
    [ [ 1; 2; 3 ]
      [ 1; 2 ]
      [ 3; 4 ]
      [ 6; 5; 3; 2; 4; 5 ]
      [ 8; 8 ]
      [ 0 ] ]

let rec twoels (data: int list list) : unit =
    match data with
    | h :: t when h.Length = 2 ->
        printfn "%A" h
        twoels t
    | h :: t when h.Length <> 2 -> twoels t
    | _ -> ()

twoels vals
```

Printing 2-e sublists with recursive algorithms.  

---

```F#
open System 

let rand = new Random()

let days =
    [ "monday"
      "tuesday"
      "wednesday"
      "thursday"
      "friday"
      "saturday"
      "sunday" ]

let d = days[rand.Next(days.Length)]

let ret =
    match d with
    | "monday"
    | "tuesday"
    | "wednesday"
    | "thursday"
    | "friday" -> "weekday"
    | "saturday"
    | "sunday" -> "weekend"
    | w -> failwithf "%s is out of range" w

printfn "%s %s" ret d
```

## Categorizing values.  

```F#
open System 

type Choices =
    | A
    | B
    | C

let getVal () =
    match Random().Next(1, 4) with
    | 1 -> A
    | 2 -> B
    | _ -> C

let chx = [ for _ in 1..7 -> getVal () ]
printfn "%A" chx
```

## Generating a list of random choices.  

```F#
open System 

type Choices =
    | A
    | B
    | C

let getVal =
    function
    | 1 -> A
    | 2 -> B
    | _ -> C

let chx =
    [ for _ in 1..7 do
          yield getVal (Random().Next(1, 4)) ]

printfn "%A" chx
```

Another variant of the previous example. The `function` can  
replace `match/with`.  

```F#
let vals = [ 1..7 ]
let vals2 = [ 1; 2; -1; -4 ]
let vals3 = []

let rec traverse e =
    match e with
    | h :: t ->
        printfn "%d" h
        traverse t
    | [] -> printfn ""

traverse vals
traverse vals2
traverse vals3
```

## Head & tail

Going over a list with recursive pattern matching and  
`t::h` cons pattern.  

## Exception handling

```F#
open System

printf "Enter a number: "

let value = Console.ReadLine()

let n =
    match Int32.TryParse value with
    | true, num -> num
    | _ -> failwithf "'%s' is not an integer" value


let f =
    function
    | value when value > 0 -> printfn "positive value"
    | value when value = 0 -> printfn "zero"
    | value when value < 0 -> printfn "negative value"
    | _ -> ()

f (int value)
f n
```

Pattern matching with exception handling.  

## Matching types with :? operator

```F#
open System.Collections

type User =
    { FirstName: string
      LastName: string
      Occupation: string }

let vals = new ArrayList()
vals.Add(1.2)
vals.Add(22)
vals.Add(true)
vals.Add("falcon")

vals.Add(
    { FirstName = "John"
      LastName = "Doe"
      Occupation = "gardener" }
)

for wal in vals do
    match wal with
    | :? int -> printfn "an integer"
    | :? float -> printfn "a float"
    | :? bool -> printfn "a boolean"
    | :? User -> printfn "a User"
    | _ -> ()
```

## Pattern matching with records

```F#
type User =
    { FirstName: string
      LastName: string
      Occupation: string }

let users =
    [ { FirstName = "John"
        LastName = "Doe"
        Occupation = "gardener" }
      { FirstName = "Jane"
        LastName = "Doe"
        Occupation = "teacher" }
      { FirstName = "Roger"
        LastName = "Roe"
        Occupation = "driver" } ]

for user in users do
    match user with
    | { LastName = "Doe" } -> printfn "%A" user
    | _ -> ()
```

## Multiple guards 

```F#
type User =
    { name: string
      salary: int
      years: int }

let users =
    [ { name = "John Doe"
        salary = 1250
        years = 4 }
      { name = "Jane Doe"
        salary = 850
        years = 6 }
      { name = "Roger Roe"
        salary = 1120
        years = 7 }
      { name = "Peter Smith"
        salary = 780
        years = 2 }
      { name = "Sam Walter"
        salary = 2250
        years = 8 } ]

for user in users do
    match user with
    | user when user.salary > 1000 && user.years > 5 -> printfn "%A" user
    | _ -> ()
    
printfn "------------------------"

users
|> List.iter (fun e ->
    match e with
    | user when user.salary > 1000 && user.years > 5 -> printfn "%A" user
    | _ -> ())    
```

## Active pattern

```F#
let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

let TestVal input =
   match input with
   | Even -> printfn "%d is even" input
   | Odd -> printfn "%d is odd" input

TestVal 7
TestVal 11
TestVal 32
```

## List comprehension

Match pattern in a list comprehension.  

```F#
let res =
    [ for e in 1..100 do
          match e with
          | e when e % 3 = 0 -> yield "fizz"
          | e when e % 5 = 0 -> yield "buzz"
          | e when e % 15 = 0 -> yield "fizzbuzz"
          | _ -> yield (string e) ]


printfn "%A" res
```

## Regex match

```F#
open System.Text.RegularExpressions


let (|RegEx|_|) p i =
    let m = Regex.Match(i, p)

    if m.Success then
        Some m.Groups
    else
        None


let CheckRegex (msg) =
    match msg with
    | RegEx @"\d+" g -> printfn "Digit: %A" g
    | RegEx @"\w+" g -> printfn "Word : %A" g
    | _ -> printfn "Not recognized"


CheckRegex "an old falcon"
CheckRegex "1984"
CheckRegex "3 hawks"
```



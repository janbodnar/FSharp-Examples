# Match expressions

The match expression provides branching control that is based on  
the comparison of an expression with a set of patterns. In addition to  
pattern matching expression we have pattern matching function.  

The function version is a short hand for the full match syntax in the  
special case where the match statement is the entire function and the  
function only has a single argument (tuples count as one).

## String constant 

Pattern matching with string constants provides direct value comparison  
for branching logic based on exact string matches.  

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

The example reads user input, normalizes it to title case, and matches  
against the expected string constant. The wildcard pattern `_` handles  
all cases that don't match the specific constant value.

## Multiple options

Multiple options are separeted with |. 

```F#

let grades = ["A"; "B"; "C"; "D"; "E"; "F"; "FX"]

for grade in grades do

    match grade with
    | "A" | "B" | "C" | "D" | "E" | "F" -> printfn "%s" "passed"
    | _ -> printfn "%s" "failed"
```

Multiple patterns can be combined with the OR operator `|` to match  
against several values in a single case. This reduces code duplication  
when multiple values should trigger the same behavior.

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

Guards add conditional logic to pattern matching using `when` clauses.  
This enables complex pattern matching based on computed conditions  
rather than just structural matches. The underscore `_` provides  
exhaustive matching for cases not covered by specific guards.
 
## Factorial

Recursive pattern matching with guards demonstrates complex conditional  
logic combined with recursive function calls for mathematical computations.  

```F#
let rec factorial (n: bigint): bigint =

    match n with
    | n when n = 0I || n = 1I -> 1I
    | n -> n * factorial (n - 1I)


for i in 0I..32I do

    let f = factorial(i)
    printfn $"{i} {f}"
```

This factorial implementation uses pattern matching with guards to  
handle base cases and recursive calls. The `bigint` type prevents  
integer overflow for large factorial calculations, making it suitable  
for computing factorials of numbers beyond standard integer limits.

## Enums 

Pattern matching with discriminated unions provides type-safe enumeration  
handling with exhaustive case coverage checking at compile time.  

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

Discriminated unions enable type-safe enumerations with pattern matching.  
The compiler ensures all cases are handled, preventing runtime errors  
from missing cases. Random shuffling demonstrates working with collections  
of union values while maintaining type safety.

## 2-element lists

Pattern matching with list structures enables precise matching based on  
list length and element count using literal list patterns.  

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

The literal list pattern `[x; y]` matches exactly two-element lists,  
extracting the values into variables `x` and `y`. This demonstrates  
structural pattern matching for specific list shapes.
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

Using the `function` keyword creates a lambda expression with pattern  
matching. This is shorthand for `fun x -> match x with ...` and works  
well with higher-order functions like List.map for functional processing.

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

Recursive pattern matching with guards combines list deconstruction  
(`h :: t`) with length checking. This approach processes lists  
iteratively using pattern matching for control flow while maintaining  
functional programming principles.

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

Multiple alternatives in pattern matching can handle related values  
together. This example categorizes days into weekdays and weekends  
using multiple pattern alternatives combined with the OR operator.

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

Pattern matching in function expressions enables concise value  
transformation. This example demonstrates random choice generation  
using pattern matching to convert integers into discriminated union  
values for type-safe enumeration handling.

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

The `function` keyword creates pattern matching functions without  
explicit match expressions. This approach provides cleaner syntax  
for single-argument functions that primarily perform pattern matching  
operations on their input.

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

The head-tail pattern `h :: t` deconstructs lists into their first  
element and remaining elements. This recursive approach processes  
lists element by element until reaching the empty list base case,  
demonstrating fundamental functional list processing techniques.

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

Pattern matching with tuples from TryParse methods provides safe  
parsing with error handling. The tuple deconstruction `(true, num)`  
extracts both the success flag and parsed value, enabling robust  
input validation without exceptions.

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

Type testing with the `:?` operator enables pattern matching on  
object types at runtime. This is useful when working with heterogeneous  
collections or .NET interop scenarios where type information is  
determined at runtime rather than compile time.

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

Record pattern matching extracts and matches specific fields while  
ignoring others. This selective matching enables filtering and  
processing based on partial record structure, making it ideal  
for working with complex data types.

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

Multiple guards combine multiple conditions using logical operators  
like `&&` for complex filtering logic. This approach enables  
sophisticated data filtering based on multiple record fields  
while maintaining readable and maintainable code structure.

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

Active patterns define custom pattern matching logic that can be  
reused across different match expressions. The `|Even|Odd|` pattern  
encapsulates the modulo logic, making the match expression more  
readable and the pattern reusable throughout the codebase.

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

List comprehensions can incorporate pattern matching for conditional  
element generation. However, this example has a logic error - the  
conditions should check divisibility by 15 first, then 5 and 3  
to avoid incorrect "fizz" results for multiples of 15.

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

Active patterns with partial matching using `|Pattern|_|` enable  
optional pattern matching that returns Some or None. This regex  
active pattern encapsulates regular expression matching logic,  
making complex text processing more readable and reusable.

## Boolean matching

Simple boolean pattern matching provides clear branching logic  
for true/false conditions with readable case handling.  

```F#
let checkCondition condition =
    match condition with
    | true -> "Condition is true"
    | false -> "Condition is false"

let results = [true; false; true] |> List.map checkCondition
printfn "%A" results
```

Boolean pattern matching offers an alternative to if-then-else  
expressions with explicit case handling. This approach makes  
the logic more explicit and integrates well with F#'s pattern  
matching ecosystem for consistent code style.

## Numeric ranges

Pattern matching with guard clauses enables range-based classifications  
for numeric values using boundary conditions.  

```F#
let classifyNumber n =
    match n with
    | n when n < 0 -> "Negative"
    | n when n = 0 -> "Zero"
    | n when n <= 10 -> "Small positive"
    | n when n <= 100 -> "Medium positive"
    | _ -> "Large positive"

[(-5); 0; 3; 25; 150] |> List.iter (fun n ->
    printfn "%d: %s" n (classifyNumber n))
```

Guard clauses enable complex conditional matching that goes beyond  
simple structural patterns. This approach combines pattern matching  
syntax with boolean expressions for flexible value classification  
based on computed conditions.

## Tuple deconstruction

Tuple patterns enable simultaneous matching and extraction of  
multiple values from structured data.  

```F#
let processCoordinate coord =
    match coord with
    | (0, 0) -> "Origin"
    | (x, 0) -> sprintf "On X-axis at %d" x
    | (0, y) -> sprintf "On Y-axis at %d" y
    | (x, y) when x = y -> sprintf "Diagonal at (%d, %d)" x y
    | (x, y) -> sprintf "Point at (%d, %d)" x y

let coordinates = [(0, 0); (5, 0); (0, 3); (4, 4); (2, 7)]
coordinates |> List.iter (fun coord ->
    printfn "%A -> %s" coord (processCoordinate coord))
```

Tuple deconstruction extracts individual elements while enabling  
pattern matching on the structure. This combines destructuring  
with conditional logic for comprehensive tuple processing in  
a single match expression.

## Option types

Option pattern matching provides null-safe handling for values  
that may or may not exist.  

```F#
let safeDivision x y =
    match y with
    | 0 -> None
    | _ -> Some (x / y)

let processResult result =
    match result with
    | Some value -> sprintf "Result: %d" value
    | None -> "Division by zero"

let calculations = [(10, 2); (15, 3); (8, 0); (12, 4)]
calculations |> List.iter (fun (x, y) ->
    let result = safeDivision x y
    printfn "%d / %d = %s" x y (processResult result))
```

Option types eliminate null reference exceptions by explicitly  
representing the absence of values. Pattern matching on Some/None  
provides safe access to optional values with clear error handling  
paths built into the type system.

## List pattern matching

Advanced list patterns enable sophisticated list processing  
with structural matching and element extraction.  

```F#
let describeList lst =
    match lst with
    | [] -> "Empty list"
    | [x] -> sprintf "Single element: %d" x
    | [x; y] -> sprintf "Two elements: %d, %d" x y
    | x :: y :: [] -> sprintf "Two elements (alt): %d, %d" x y
    | x :: xs when xs.Length = 2 -> 
        sprintf "Three elements: %d and two more" x
    | x :: _ -> sprintf "Multiple elements, first is %d" x

let testLists = [
    []; [42]; [1; 2]; [1; 2; 3]; [1; 2; 3; 4; 5]
]

testLists |> List.iter (fun lst ->
    printfn "%A -> %s" lst (describeList lst))
```

List patterns combine literal matching `[]`, `[x; y]` with  
deconstruction patterns `x :: xs` for flexible list processing.  
This enables matching specific list lengths while extracting  
elements for further processing.

## Array pattern matching

Array patterns enable fixed-size array matching with element  
extraction and boundary checking.  

```F#
let processArray arr =
    match arr with
    | [||] -> "Empty array"
    | [|x|] -> sprintf "Single element: %d" x
    | [|x; y|] -> sprintf "Two elements: %d, %d" x y
    | [|x; y; z|] -> sprintf "Three elements: %d, %d, %d" x y z
    | arr when arr.Length > 10 -> sprintf "Large array with %d elements" arr.Length
    | arr -> sprintf "Array with %d elements" arr.Length

let testArrays = [
    [||]; [|1|]; [|1; 2|]; [|1; 2; 3|]; [|1..15|]
]

testArrays |> List.iter (fun arr ->
    printfn "%A -> %s" arr (processArray arr))
```

Array patterns use `[|...|]` syntax for matching fixed-size arrays.  
Unlike lists, arrays have constant-time length access, making  
guard clauses with length checks efficient for array processing  
and classification.

## Nested patterns

Complex nested patterns enable deep structural matching  
with multiple levels of deconstruction.  

```F#
type Customer = { Name: string; Age: int }
type Order = { Customer: Customer; Items: string list; Total: decimal }

let categorizeOrder order =
    match order with
    | { Customer = { Age = age }; Total = total } when age < 18 -> 
        "Minor customer order"
    | { Customer = { Name = name }; Items = [] } ->
        sprintf "Empty order for %s" name
    | { Customer = { Name = name }; Items = [item] } ->
        sprintf "Single item (%s) for %s" item name
    | { Customer = { Name = name }; Items = items; Total = total } 
        when total > 1000m ->
        sprintf "High value order for %s with %d items" name items.Length
    | { Customer = { Name = name }; Items = items } ->
        sprintf "Standard order for %s with %d items" name items.Length

let orders = [
    { Customer = { Name = "Alice"; Age = 16 }; Items = ["Book"]; Total = 25m }
    { Customer = { Name = "Bob"; Age = 35 }; Items = []; Total = 0m }
    { Customer = { Name = "Carol"; Age = 42 }; Items = ["Laptop"]; Total = 1500m }
    { Customer = { Name = "Dave"; Age = 28 }; Items = ["Phone"; "Case"]; Total = 800m }
]

orders |> List.iter (fun order ->
    printfn "%s" (categorizeOrder order))
```

Nested patterns extract data from multiple structural levels  
simultaneously. This enables complex business logic implementation  
with clean, readable pattern matching that mirrors the data  
structure organization.

## Parameterized active patterns

Parameterized active patterns accept arguments to customize  
pattern matching behavior dynamically.  

```F#
let (|DivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some () else None

let (|InRange|_|) min max n =
    if n >= min && n <= max then Some n else None

let classifyWithParameters n =
    match n with
    | DivisibleBy 15 -> sprintf "%d is divisible by 15" n
    | DivisibleBy 3 -> sprintf "%d is divisible by 3" n
    | DivisibleBy 5 -> sprintf "%d is divisible by 5" n
    | InRange 50 100 -> sprintf "%d is in range 50-100" n
    | InRange 1 10 -> sprintf "%d is in range 1-10" n
    | _ -> sprintf "%d doesn't match patterns" n

[3; 5; 15; 7; 75; 42] |> List.iter (fun n ->
    printfn "%s" (classifyWithParameters n))
```

Parameterized active patterns create reusable pattern logic  
with configurable parameters. This enables creating pattern  
libraries that can be customized for different matching  
requirements while maintaining clean syntax.

## Multi-case active patterns

Multi-case active patterns provide domain-specific classifications  
with up to seven distinct cases.  

```F#
type Priority = Low | Medium | High | Critical

let (|TaskPriority|) (daysUntilDue: int) =
    match daysUntilDue with
    | d when d < 0 -> Critical
    | d when d <= 1 -> High
    | d when d <= 7 -> Medium
    | _ -> Low

let (|Season|) month =
    match month with
    | 12 | 1 | 2 -> "Winter"
    | 3 | 4 | 5 -> "Spring"  
    | 6 | 7 | 8 -> "Summer"
    | 9 | 10 | 11 -> "Autumn"
    | _ -> "Invalid month"

let processTasks tasks =
    tasks |> List.iter (fun (task, days) ->
        match days with
        | TaskPriority priority ->
            printfn "%s: %A priority (%d days)" task priority days)

let showSeasons months =
    months |> List.iter (fun month ->
        match month with
        | Season season -> printfn "Month %d: %s" month season)

processTasks [("Report", 2); ("Meeting", 0); ("Review", -1); ("Planning", 14)]
showSeasons [1; 4; 7; 10; 13]
```

Multi-case active patterns encapsulate complex classification  
logic into reusable patterns. This creates domain-specific  
languages within F# that make business logic more readable  
and maintainable.

## Partial active patterns with return values

Partial active patterns can return extracted values along  
with the match success indication.  

```F#
open System.Text.RegularExpressions

let (|ParseInt|_|) str =
    match System.Int32.TryParse(str) with
    | true, value -> Some value
    | _ -> None

let (|Email|_|) input =
    let pattern = @"^[^@]+@[^@]+\.[^@]+$"
    if Regex.IsMatch(input, pattern) then
        let parts = input.Split('@')
        Some (parts.[0], parts.[1])
    else None

let (|PhoneNumber|_|) input =
    let pattern = @"^(\d{3})-(\d{3})-(\d{4})$"
    let m = Regex.Match(input, pattern)
    if m.Success then
        Some (m.Groups.[1].Value, m.Groups.[2].Value, m.Groups.[3].Value)
    else None

let processInput input =
    match input with
    | ParseInt value -> sprintf "Number: %d" value
    | Email (user, domain) -> sprintf "Email - User: %s, Domain: %s" user domain
    | PhoneNumber (area, exchange, number) -> 
        sprintf "Phone: (%s) %s-%s" area exchange number
    | _ -> sprintf "Unrecognized format: %s" input

let inputs = [
    "42"; "john@example.com"; "123-456-7890"; "invalid"; "alice@test.org"
]

inputs |> List.iter (fun input ->
    printfn "%s -> %s" input (processInput input))
```

Partial active patterns returning values combine pattern matching  
with data extraction. This enables parsing and validation in  
a single pattern match while extracting structured data from  
string inputs or complex objects.

## Discriminated union with data

Discriminated unions carrying data enable rich pattern matching  
with value extraction from union cases.  

```F#
type Shape =
    | Circle of radius: float
    | Rectangle of width: float * height: float
    | Triangle of base: float * height: float

type Result<'T> =
    | Success of 'T
    | Error of string

let calculateArea shape =
    match shape with
    | Circle radius -> Success (System.Math.PI * radius * radius)
    | Rectangle (width, height) -> Success (width * height)
    | Triangle (baseLen, height) -> Success (0.5 * baseLen * height)

let processCalculation result =
    match result with
    | Success area -> sprintf "Area: %.2f" area
    | Error message -> sprintf "Error: %s" message

let shapes = [
    Circle 5.0
    Rectangle (4.0, 6.0)
    Triangle (3.0, 8.0)
]

shapes |> List.iter (fun shape ->
    let result = calculateArea shape
    printfn "%A -> %s" shape (processCalculation result))
```

Discriminated unions with data combine type safety with pattern  
matching and value extraction. This enables modeling complex  
domain concepts while maintaining compile-time verification  
of all case handling.

## Exception pattern matching

Exception pattern matching provides structured error handling  
with different responses for specific exception types.  

```F#
open System

let safeOperation operation =
    try
        operation () |> Some
    with
    | :? DivideByZeroException as ex -> 
        printfn "Division by zero: %s" ex.Message
        None
    | :? ArgumentException as ex ->
        printfn "Invalid argument: %s" ex.Message
        None
    | :? InvalidOperationException as ex ->
        printfn "Invalid operation: %s" ex.Message
        None
    | ex -> 
        printfn "Unexpected error: %s" ex.Message
        None

let operations = [
    (fun () -> 10 / 2)
    (fun () -> 10 / 0)
    (fun () -> failwith "Custom error")
    (fun () -> raise (ArgumentException("Bad argument")))
]

operations |> List.iteri (fun i op ->
    printfn "Operation %d:" (i + 1)
    match safeOperation op with
    | Some result -> printfn "Result: %d" result
    | None -> printfn "Operation failed"
    printfn "")
```

Exception pattern matching enables fine-grained error handling  
with specific responses for different exception types. This  
provides more control than generic exception handling while  
maintaining type safety and clear error recovery paths.

## When guards with complex conditions

Advanced guard clauses enable sophisticated conditional logic  
within pattern matching expressions.  

```F#
type Student = { Name: string; Grade: int; Attendance: float }

let evaluateStudent student =
    match student with
    | { Grade = g; Attendance = a } when g >= 90 && a >= 0.95 -> "Excellence Award"
    | { Grade = g; Attendance = a } when g >= 80 && a >= 0.90 -> "Honor Roll"  
    | { Grade = g; Attendance = a } when g >= 70 && a >= 0.80 -> "Good Standing"
    | { Grade = g; Attendance = a } when g >= 60 && a >= 0.70 -> "Needs Improvement"
    | { Grade = g } when g < 60 -> "Academic Probation"
    | { Attendance = a } when a < 0.70 -> "Attendance Warning"
    | _ -> "Review Required"

let students = [
    { Name = "Alice"; Grade = 95; Attendance = 0.98 }
    { Name = "Bob"; Grade = 85; Attendance = 0.92 }
    { Name = "Carol"; Grade = 75; Attendance = 0.85 }
    { Name = "Dave"; Grade = 55; Attendance = 0.88 }
    { Name = "Eve"; Grade = 80; Attendance = 0.65 }
]

students |> List.iter (fun student ->
    let status = evaluateStudent student
    printfn "%s (Grade: %d, Attendance: %.0f%%): %s" 
            student.Name student.Grade (student.Attendance * 100.0) status)
```

Complex guard conditions combine multiple field access with  
boolean logic for sophisticated decision making. This enables  
business rule implementation directly within pattern matching  
for clear and maintainable code.

## Pattern matching with computation expressions

Pattern matching integrates with computation expressions  
for monadic processing with structured data handling.  

```F#
type AsyncResult<'T> = Async<Result<'T, string>>

let asyncResult = async {
    return Ok 42
}

let processAsyncResult (ar: AsyncResult<int>) = async {
    let! result = ar
    return
        match result with
        | Ok value when value > 50 -> sprintf "High value: %d" value
        | Ok value when value > 0 -> sprintf "Positive value: %d" value
        | Ok value -> sprintf "Non-positive value: %d" value
        | Error message -> sprintf "Error: %s" message
}

// Example with option computation expression
type OptionBuilder() =
    member _.Bind(x, f) = Option.bind f x
    member _.Return(x) = Some x

let option = OptionBuilder()

let processOptions x y = option {
    let! a = x
    let! b = y
    return
        match (a, b) with
        | (0, _) | (_, 0) -> "Contains zero"
        | (a, b) when a = b -> sprintf "Equal values: %d" a
        | (a, b) -> sprintf "Different values: %d, %d" a b
}

// Usage
asyncResult 
|> processAsyncResult 
|> Async.RunSynchronously 
|> printfn "%s"

processOptions (Some 5) (Some 5) |> printfn "%A"
processOptions (Some 3) (Some 7) |> printfn "%A"
processOptions (Some 0) (Some 5) |> printfn "%A"
```

Pattern matching within computation expressions combines  
monadic processing with structured data handling. This  
enables clean error handling and data transformation  
pipelines with declarative syntax.

## Function composition with pattern matching

Pattern matching functions can be composed for complex  
data processing pipelines.  

```F#
type LogLevel = Debug | Info | Warning | Error | Critical

type LogEntry = {
    Level: LogLevel
    Message: string
    Timestamp: System.DateTime
}

let filterByLevel level =
    function
    | { Level = l } when l = level -> true
    | _ -> false

let formatEntry =
    function
    | { Level = Critical; Message = msg; Timestamp = ts } ->
        sprintf "[CRITICAL %s] %s" (ts.ToString("HH:mm:ss")) msg
    | { Level = Error; Message = msg; Timestamp = ts } ->
        sprintf "[ERROR %s] %s" (ts.ToString("HH:mm:ss")) msg
    | { Level = Warning; Message = msg; Timestamp = ts } ->
        sprintf "[WARN %s] %s" (ts.ToString("HH:mm:ss")) msg
    | { Level = Info; Message = msg; Timestamp = ts } ->
        sprintf "[INFO %s] %s" (ts.ToString("HH:mm:ss")) msg
    | { Level = Debug; Message = msg; Timestamp = ts } ->
        sprintf "[DEBUG %s] %s" (ts.ToString("HH:mm:ss")) msg

let processLogs logs level =
    logs
    |> List.filter (filterByLevel level)
    |> List.map formatEntry

let sampleLogs = [
    { Level = Info; Message = "Application started"; Timestamp = System.DateTime.Now }
    { Level = Warning; Message = "Low memory"; Timestamp = System.DateTime.Now }
    { Level = Error; Message = "Database connection failed"; Timestamp = System.DateTime.Now }
    { Level = Debug; Message = "Variable state"; Timestamp = System.DateTime.Now }
]

processLogs sampleLogs Error |> List.iter (printfn "%s")
```

Function composition with pattern matching creates reusable  
processing pipelines. This functional approach enables  
building complex data transformations from simple,  
composable pattern matching functions.

## Pattern matching with sequences

Lazy sequence pattern matching enables efficient processing  
of large or infinite data streams.  

```F#
let rec processSequence seq =
    match seq with
    | [] -> []
    | x :: xs when x % 2 = 0 -> x :: processSequence xs
    | x :: xs -> processSequence xs

let processSeqLazy (seq: seq<int>) =
    seq
    |> Seq.choose (function
        | x when x % 3 = 0 -> Some (sprintf "Divisible by 3: %d" x)
        | x when x % 5 = 0 -> Some (sprintf "Divisible by 5: %d" x)
        | _ -> None)
    |> Seq.take 5

// Infinite sequence
let infiniteNumbers = Seq.initInfinite id

let fibonacciSeq = 
    let rec fib a b = seq {
        yield a
        yield! fib b (a + b)
    }
    fib 0 1

fibonacciSeq
|> Seq.take 10
|> Seq.choose (function
    | n when n % 2 = 0 -> Some n
    | _ -> None)
|> Seq.iter (printfn "Even Fibonacci: %d")

infiniteNumbers
|> processSeqLazy
|> Seq.iter (printfn "%s")
```

Sequence pattern matching enables lazy evaluation with pattern  
matching for memory-efficient processing of large datasets.  
This combines F#'s lazy evaluation with structured pattern  
matching for optimal performance.

## Advanced record patterns

Complex record pattern matching enables sophisticated  
data extraction and transformation scenarios.  

```F#
type Address = { Street: string; City: string; State: string; Zip: string }
type Person = { 
    Name: string
    Age: int
    Address: Address
    Email: string option
    Phone: string option
}

let classifyPerson person =
    match person with
    | { Age = age; Address = { State = state }; Email = Some email } 
        when age < 18 && state = "CA" ->
        sprintf "Minor in CA with email: %s" email
    | { Age = age; Email = None; Phone = None } when age >= 65 ->
        "Senior with no contact info"
    | { Address = { City = "New York"; State = "NY" }; Email = Some _ } ->
        "NYC resident with email"
    | { Address = { State = state }; Phone = Some phone } 
        when List.contains state ["TX"; "FL"; "CA"] ->
        sprintf "Major state resident: %s" phone
    | { Name = name; Age = age } when age >= 21 ->
        sprintf "Adult: %s" name
    | { Name = name } ->
        sprintf "Minor: %s" name

let people = [
    { Name = "Alice"; Age = 16; 
      Address = { Street = "123 Main"; City = "Los Angeles"; State = "CA"; Zip = "90210" };
      Email = Some "alice@test.com"; Phone = None }
    { Name = "Bob"; Age = 67; 
      Address = { Street = "456 Oak"; City = "Miami"; State = "FL"; Zip = "33101" };
      Email = None; Phone = None }
    { Name = "Carol"; Age = 35;
      Address = { Street = "789 Pine"; City = "New York"; State = "NY"; Zip = "10001" };
      Email = Some "carol@example.com"; Phone = Some "555-0123" }
]

people |> List.iter (fun person ->
    printfn "%s" (classifyPerson person))
```

Advanced record patterns combine nested matching, option handling,  
and guard clauses for comprehensive data classification. This  
enables complex business logic implementation with clear,  
readable pattern matching expressions.

## Pattern matching with maps and sets

Collection pattern matching enables structured processing  
of F# collection types with custom logic.  

```F#
open System.Collections.Generic

let analyzeMap (map: Map<string, int>) =
    match map |> Map.toList with
    | [] -> "Empty map"
    | [(key, value)] -> sprintf "Single entry: %s = %d" key value
    | pairs when pairs |> List.forall (fun (_, v) -> v > 0) ->
        sprintf "All positive values, %d entries" pairs.Length
    | pairs when pairs |> List.exists (fun (_, v) -> v < 0) ->
        sprintf "Contains negative values, %d entries" pairs.Length
    | pairs -> sprintf "Mixed values, %d entries" pairs.Length

let analyzeSet (set: Set<int>) =
    match set |> Set.toList with
    | [] -> "Empty set"
    | [x] -> sprintf "Single element: %d" x
    | xs when xs |> List.forall (fun x -> x % 2 = 0) ->
        sprintf "All even numbers: %A" xs
    | xs when xs |> List.forall (fun x -> x > 0) ->
        sprintf "All positive numbers: %A" xs
    | xs -> sprintf "Mixed numbers: %A" xs

// Test maps
let maps = [
    Map.empty
    Map.ofList [("a", 1)]
    Map.ofList [("x", 5); ("y", 10); ("z", 3)]
    Map.ofList [("p", -2); ("q", 8)]
]

// Test sets
let sets = [
    Set.empty
    Set.ofList [42]
    Set.ofList [2; 4; 6; 8]
    Set.ofList [1; 3; 5; 7]
    Set.ofList [-1; 0; 1; 2]
]

printfn "Map Analysis:"
maps |> List.iter (fun m -> printfn "  %s" (analyzeMap m))

printfn "\nSet Analysis:"
sets |> List.iter (fun s -> printfn "  %s" (analyzeSet s))
```

Collection pattern matching converts immutable collections  
to lists for structural pattern matching. This enables  
sophisticated analysis of collection contents while  
leveraging F#'s powerful pattern matching capabilities.

## Custom operators with pattern matching

Custom operators combined with pattern matching create  
domain-specific languages for specialized processing.  

```F#
type Expr =
    | Const of int
    | Add of Expr * Expr
    | Mul of Expr * Expr
    | Sub of Expr * Expr
    | Div of Expr * Expr

let rec evaluate expr =
    match expr with
    | Const n -> n
    | Add (left, right) -> evaluate left + evaluate right
    | Sub (left, right) -> evaluate left - evaluate right
    | Mul (left, right) -> evaluate left * evaluate right
    | Div (left, right) -> 
        let r = evaluate right
        if r = 0 then failwith "Division by zero"
        else evaluate left / r

let rec simplify expr =
    match expr with
    | Add (Const 0, expr) | Add (expr, Const 0) -> simplify expr
    | Mul (Const 0, _) | Mul (_, Const 0) -> Const 0
    | Mul (Const 1, expr) | Mul (expr, Const 1) -> simplify expr
    | Sub (expr, Const 0) -> simplify expr
    | Div (expr, Const 1) -> simplify expr
    | Add (left, right) -> Add (simplify left, simplify right)
    | Sub (left, right) -> Sub (simplify left, simplify right)
    | Mul (left, right) -> Mul (simplify left, simplify right)
    | Div (left, right) -> Div (simplify left, simplify right)
    | expr -> expr

// Custom operators
let (+.) left right = Add (left, right)
let (-.) left right = Sub (left, right)
let (*.) left right = Mul (left, right)
let (/.) left right = Div (left, right)

// Usage
let expr1 = Const 10 +. Const 5 *. Const 2
let expr2 = Const 0 +. Const 7 *. Const 1
let expr3 = Const 15 /. Const 3 -. Const 0

printfn "Expression 1: %d" (evaluate expr1)
printfn "Expression 2 simplified: %A" (simplify expr2)
printfn "Expression 3: %d" (evaluate expr3)
```

Custom operators with pattern matching enable building  
domain-specific expression languages. This demonstrates  
advanced pattern matching for symbolic computation  
and expression tree manipulation.

## Pattern matching with inheritance

Object-oriented pattern matching enables polymorphic  
processing with type-safe casting and method dispatch.  

```F#
[<AbstractClass>]
type Animal(name: string) =
    member _.Name = name
    abstract member MakeSound: unit -> string

type Dog(name: string, breed: string) =
    inherit Animal(name)
    member _.Breed = breed
    override _.MakeSound() = "Woof!"

type Cat(name: string, indoor: bool) =
    inherit Animal(name)
    member _.IsIndoor = indoor
    override _.MakeSound() = "Meow!"

type Bird(name: string, canFly: bool) =
    inherit Animal(name)
    member _.CanFly = canFly
    override _.MakeSound() = "Tweet!"

let describeAnimal (animal: Animal) =
    let sound = animal.MakeSound()
    match animal with
    | :? Dog as dog -> 
        sprintf "%s is a %s breed dog that says '%s'" 
                dog.Name dog.Breed sound
    | :? Cat as cat ->
        let location = if cat.IsIndoor then "indoor" else "outdoor"
        sprintf "%s is an %s cat that says '%s'" 
                cat.Name location sound
    | :? Bird as bird ->
        let flight = if bird.CanFly then "flying" else "flightless"
        sprintf "%s is a %s bird that says '%s'" 
                bird.Name flight sound
    | _ -> sprintf "%s is an unknown animal that says '%s'" 
                  animal.Name sound

let animals = [
    Dog("Rex", "German Shepherd") :> Animal
    Cat("Whiskers", true) :> Animal
    Bird("Tweety", true) :> Animal
    Bird("Penguin", false) :> Animal
]

animals |> List.iter (fun animal ->
    printfn "%s" (describeAnimal animal))
```

Inheritance pattern matching combines object-oriented  
polymorphism with F# pattern matching. The `:?` operator  
enables safe downcasting with automatic type extraction  
for accessing derived class members.

## Mailbox processor patterns

Mailbox processors use pattern matching for message  
handling in actor-based concurrent programming.  

```F#
type Message =
    | Add of int * AsyncReplyChannel<int>
    | Subtract of int * AsyncReplyChannel<int>
    | GetValue of AsyncReplyChannel<int>
    | Reset

let createCalculator initialValue =
    MailboxProcessor.Start(fun inbox ->
        let rec loop currentValue = async {
            let! message = inbox.Receive()
            match message with
            | Add (value, replyChannel) ->
                let newValue = currentValue + value
                replyChannel.Reply(newValue)
                return! loop newValue
            | Subtract (value, replyChannel) ->
                let newValue = currentValue - value
                replyChannel.Reply(newValue)
                return! loop newValue
            | GetValue replyChannel ->
                replyChannel.Reply(currentValue)
                return! loop currentValue
            | Reset ->
                printfn "Calculator reset to 0"
                return! loop 0
        }
        loop initialValue)

// Usage
let calculator = createCalculator 10

let result1 = calculator.PostAndReply(fun reply -> Add (5, reply))
printfn "After adding 5: %d" result1

let result2 = calculator.PostAndReply(fun reply -> Subtract (3, reply))
printfn "After subtracting 3: %d" result2

let currentValue = calculator.PostAndReply(GetValue)
printfn "Current value: %d" currentValue

calculator.Post(Reset)
```

Mailbox processor pattern matching enables safe concurrent  
programming with message-passing semantics. Each message  
type defines a specific operation with optional reply  
channels for bidirectional communication.

## JSON pattern matching

JSON pattern matching with type providers enables  
structured processing of semi-structured data.  

```F#
open System.Text.Json

type JsonValue =
    | String of string
    | Number of decimal
    | Boolean of bool
    | Array of JsonValue list
    | Object of (string * JsonValue) list
    | Null

let rec parseJson (element: JsonElement) : JsonValue =
    match element.ValueKind with
    | JsonValueKind.String -> String (element.GetString())
    | JsonValueKind.Number -> Number (element.GetDecimal())
    | JsonValueKind.True -> Boolean true
    | JsonValueKind.False -> Boolean false
    | JsonValueKind.Array -> 
        element.EnumerateArray()
        |> Seq.map parseJson
        |> Seq.toList
        |> Array
    | JsonValueKind.Object ->
        element.EnumerateObject()
        |> Seq.map (fun prop -> (prop.Name, parseJson prop.Value))
        |> Seq.toList
        |> Object
    | JsonValueKind.Null -> Null
    | _ -> Null

let rec processJsonValue value =
    match value with
    | String s -> sprintf "Text: %s" s
    | Number n -> sprintf "Number: %M" n
    | Boolean b -> sprintf "Boolean: %b" b
    | Array items -> 
        sprintf "Array with %d items: [%s]" 
                items.Length
                (items |> List.map processJsonValue |> String.concat "; ")
    | Object props ->
        let propStrings = props |> List.map (fun (key, value) ->
            sprintf "%s: %s" key (processJsonValue value))
        sprintf "Object: {%s}" (String.concat "; " propStrings)
    | Null -> "Null"

// Example JSON processing
let jsonString = """
{
    "name": "John Doe",
    "age": 30,
    "active": true,
    "scores": [95, 87, 92],
    "address": {
        "city": "New York",
        "zip": "10001"
    }
}
"""

let doc = JsonDocument.Parse(jsonString)
let parsed = parseJson doc.RootElement
printfn "%s" (processJsonValue parsed)
```

JSON pattern matching creates type-safe processing of  
dynamic JSON data. Pattern matching on JsonValueKind  
enables comprehensive JSON handling with structured  
error handling and type conversion.



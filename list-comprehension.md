# List comprehensions

List comprehensions provide a powerful and declarative syntax for creating  
and transforming lists in F#. They offer an elegant alternative to imperative  
loops and functional combinators like `map`, `filter`, and `collect`,  
allowing developers to express complex list operations in a concise and  
readable manner.

F# list comprehensions are built on computation expressions, specifically  
the list builder, which enables rich pattern matching, conditional logic,  
nested iterations, and data transformations. The syntax supports multiple  
for loops to create cartesian products, if conditions for filtering,  
let expressions for intermediate computations, and match patterns for  
sophisticated data processing.

The key operators in list comprehensions are:  
- `yield` - produces a single value in the resulting list  
- `yield!` - flattens and yields all elements from another collection  
- `for...in` - iterates over sequences, ranges, or collections  
- `if` - applies conditional filtering  
- `let` - defines intermediate values and computations  
- `match` - enables pattern matching within the comprehension  

List comprehensions excel at data processing tasks such as filtering,  
transformation, aggregation, and generating sequences based on mathematical  
formulas or business rules. They provide lazy evaluation benefits when  
chained with other sequence operations and maintain immutability principles  
core to functional programming.

This comprehensive guide demonstrates list comprehensions through practical  
examples, progressing from simple range-based generation to complex nested  
transformations and real-world data processing scenarios.  

## Ranges

Creating lists from numeric ranges with transformations applied to each  
element.  

```F#
let res = [ for e in 1 .. 100 -> e * e ]
printfn "%A" res
```

The arrow syntax (`->`) provides a concise way to transform each element  
in the range. This example generates squares of numbers from 1 to 100,  
demonstrating how list comprehensions can replace manual loops for  
mathematical computations.

## Multiple for loops

Nested iterations create cartesian products, useful for generating  
coordinate pairs, combinations, or exploring multi-dimensional data.  

```F#
let res2 =
    [ for r in 1..8 do
          for c in 1..8 do
              if r <> c then yield (r, c) ]

printfn "%A" res2
```

This example generates all coordinate pairs on an 8x8 grid excluding  
diagonal elements (where row equals column). Multiple for loops enable  
complex data generation patterns common in mathematical and gaming  
applications.

## Generators

Using conditional logic to filter and transform elements from existing  
collections, creating new lists based on specific criteria.  

```F#
let vals = [ -1; 0; 2; -2; 1; 3; 4; -6 ]

let pos =
    [ for e in vals do
          if e > 0 then yield e ]

printfn "%A" pos

printfn "---------------------------------"

[ for a in 1 .. 100 do
    if a % 3 = 0 && a % 5 = 0 then yield a] |> printfn "%A"

printfn "---------------------------------"

let vals3 =
    [ for x in 1 .. 3 do
          for y in 1 .. 10 -> x, y ]

printfn "%A" vals3
```

Generators demonstrate filtering positive numbers, finding multiples of both  
3 and 5 (FizzBuzz divisibility), and creating coordinate pairs. The `yield`  
keyword explicitly produces values, while the arrow syntax provides implicit  
yielding for transformations.

## yield/yield!

Distinguishing between single value production and collection flattening  
in list comprehensions.  

```F#
let res =
    [ for a in 1..5 do
          yield! [ a .. a + 3 ] ]

printfn "%A" res

let chars = [ 'a' .. 'z' ]

let res2 = [for e in chars do yield [e; e; e] ]
printfn "%A" res2
```

```F#
let words = [ "sky"; "cloud"; "park"; "rock"; "war" ]

let res = [ for e in words do yield e ]
printfn "%A" res

let res2 = [ for e in words do yield! e ]
printfn "%A" res2
```

The `yield` operator produces individual values, while `yield!` flattens  
collections into the result. The first example shows `yield!` expanding  
ranges into a flat list. The second demonstrates the difference: `yield`  
preserves strings as single items, while `yield!` explodes each string  
into individual characters.


## Match pattern

Pattern matching enables sophisticated conditional logic within list  
comprehensions, supporting complex decision trees.  

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

This classic FizzBuzz implementation demonstrates how pattern matching  
integrates seamlessly with list comprehensions. Guard clauses (`when`)  
enable complex conditional logic, while the underscore pattern provides  
a default case for unmatched values.

## Let expressions 

Local bindings within comprehensions enable complex computations and  
improve code readability by naming intermediate values.  

```F#
let vals = [ 1; -2; -3; 4; 5 ]

[ for v in vals do
      let f = fun e -> e > 0
      if f(v) then yield v ] |> printfn "%A"
```

Let expressions create local scope within each iteration, allowing  
function definitions, intermediate calculations, and complex transformations.  
This example defines a predicate function locally and applies it for  
filtering positive values.

## Stepwise ranges

Creating sequences with custom step sizes for specialized data generation.  

```F#
let evenNumbers = [ for i in 0 .. 2 .. 20 -> i ]
printfn "Even numbers: %A" evenNumbers

let countdown = [ for i in 10 .. -1 .. 0 -> i ]
printfn "Countdown: %A" countdown
```

Stepwise ranges allow precise control over sequence generation. The first  
example creates even numbers by stepping by 2, while the second demonstrates  
reverse iteration with negative steps for countdown sequences.

## String processing

Demonstrating character and string manipulations using list comprehensions  
for text processing tasks.  

```F#
let text = "Hello World"
let uppercaseChars = [ for c in text do if System.Char.IsLetter(c) then yield System.Char.ToUpper(c) ]
printfn "Uppercase letters: %A" uppercaseChars

let words = ["apple"; "banana"; "cherry"; "date"]
let firstLetters = [ for word in words -> word.[0] ]
printfn "First letters: %A" firstLetters
```

String processing with list comprehensions enables efficient text analysis  
and transformation. The first example filters and transforms letters to  
uppercase, while the second extracts first characters from a word list.

## Mathematical sequences

Generating mathematical sequences and series using comprehension patterns.  

```F#
let fibonacci = [ 
    let mutable a, b = 0, 1
    for i in 1..10 do
        let current = a
        a <- b
        b <- a + current
        yield current 
]
printfn "Fibonacci: %A" fibonacci

let primes = [
    for n in 2..50 do
        let isPrime = not (seq { 2 .. int(sqrt(float n)) } |> Seq.exists (fun x -> n % x = 0))
        if isPrime then yield n
]
printfn "Primes: %A" primes
```

Mathematical sequences showcase computational patterns within list  
comprehensions. The Fibonacci sequence uses mutable variables for state  
tracking, while the prime number generator employs nested sequence  
operations for divisibility testing.

## Nested data structures

Working with lists of lists and complex data transformations.  

```F#
let matrix = [ 
    for i in 1..3 do
        yield [ for j in 1..3 -> i * j ]
]
printfn "Multiplication table: %A" matrix

let flattened = [ 
    for row in matrix do
        yield! row 
]
printfn "Flattened: %A" flattened
```

Nested data structures demonstrate multi-dimensional list creation and  
flattening operations. The first example creates a multiplication table  
as a list of lists, while the second flattens it using `yield!`.

## Conditional filtering

Advanced filtering patterns with multiple conditions and complex logic.  

```F#
let numbers = [1..20]
let filtered = [
    for n in numbers do
        match n with
        | x when x % 3 = 0 && x % 5 = 0 -> yield (x, "FizzBuzz")
        | x when x % 3 = 0 -> yield (x, "Fizz")  
        | x when x % 5 = 0 -> yield (x, "Buzz")
        | x when x > 15 -> yield (x, "High")
        | _ -> ()
]
printfn "Conditional results: %A" filtered
```

Complex conditional filtering combines pattern matching with multiple  
guard clauses. This example categorizes numbers based on divisibility  
and magnitude, demonstrating how empty yields can skip values.

## Data transformation

Transforming records and complex data types within list comprehensions.  

```F#
type Person = { Name: string; Age: int; City: string }

let people = [
    { Name = "Alice"; Age = 30; City = "NYC" }
    { Name = "Bob"; Age = 25; City = "LA" }
    { Name = "Charlie"; Age = 35; City = "NYC" }
]

let adults = [
    for person in people do
        if person.Age >= 30 then
            yield { person with Name = person.Name.ToUpper() }
]
printfn "Adults: %A" adults
```

Data transformation examples show how list comprehensions handle custom  
types and record updates. The example filters adults and transforms  
their names to uppercase using record update syntax.

## Aggregation patterns

Computing aggregated values and statistics during list generation.  

```F#
let sales = [10; 25; 30; 15; 40; 20]
let runningTotals = [
    let mutable sum = 0
    for sale in sales do
        sum <- sum + sale
        yield sum
]
printfn "Running totals: %A" runningTotals

let averages = [
    for i in 1..5 do
        let subset = sales |> List.take i
        yield (List.average (List.map float subset))
]
printfn "Progressive averages: %A" averages
```

Aggregation patterns demonstrate stateful computations within list  
comprehensions. Running totals use mutable accumulation, while progressive  
averages show how to compute statistics over growing data subsets.

## Tuple operations

Working with tuples and coordinate transformations in comprehensions.  

```F#
let coordinates = [(1, 2); (3, 4); (5, 6); (7, 8)]
let distances = [
    for (x, y) in coordinates ->
        sqrt(float(x * x + y * y))
]
printfn "Distances from origin: %A" distances

let swapped = [ for (a, b) in coordinates -> (b, a) ]
printfn "Swapped coordinates: %A" swapped
```

Tuple operations showcase pattern matching and mathematical transformations.  
The first example calculates Euclidean distances from the origin, while  
the second demonstrates simple coordinate swapping.

## Option handling

Processing optional values and handling missing data gracefully.  

```F#
let maybeNumbers = [Some 1; None; Some 3; None; Some 5]
let validNumbers = [
    for opt in maybeNumbers do
        match opt with
        | Some n -> yield n * 2
        | None -> ()
]
printfn "Valid numbers doubled: %A" validNumbers

let defaults = [
    for opt in maybeNumbers ->
        match opt with
        | Some n -> n
        | None -> 0
]
printfn "With defaults: %A" defaults
```

Option handling demonstrates how list comprehensions elegantly process  
nullable or optional data. Pattern matching on `Some`/`None` enables  
safe extraction and default value substitution.

## List concatenation

Combining multiple lists and sequences within comprehensions.  

```F#
let listOfLists = [[1; 2]; [3; 4; 5]; [6]]
let combined = [
    for sublist in listOfLists do
        yield! sublist
]
printfn "Combined: %A" combined

let alternating = [
    for i in 1..5 do
        yield i
        yield -i
]
printfn "Alternating: %A" alternating
```

List concatenation examples show flattening operations and interleaved  
value generation. The first uses `yield!` for list flattening, while  
the second generates alternating positive and negative values.

## Recursive patterns

Implementing recursive logic within list comprehension structures.  

```F#
let factorial n = 
    let rec factHelper acc i = if i <= 1 then acc else factHelper (acc * i) (i - 1)
    factHelper 1 n

let factorials = [ for n in 1..7 -> (n, factorial n) ]
printfn "Factorials: %A" factorials

let powers = [
    for base in 2..4 do
        for exp in 1..3 ->
            (base, exp, pown base exp)
]
printfn "Powers: %A" powers
```

Recursive patterns combine helper functions with list comprehensions for  
complex calculations. The factorial example pairs numbers with their  
factorials, while the powers example creates a table of exponentiations.

## Error handling

Managing exceptions and error conditions within list processing.  

```F#
let divisionCases = [(10, 2); (15, 3); (8, 0); (20, 4)]
let safeDivisions = [
    for (a, b) in divisionCases do
        try
            yield (a, b, Some(a / b))
        with
        | :? System.DivideByZeroException -> 
            yield (a, b, None)
]
printfn "Safe divisions: %A" safeDivisions
```

Error handling shows how try-catch blocks integrate with list  
comprehensions for robust data processing. Division by zero is handled  
gracefully by returning `None` for invalid operations.

## Type conversions

Converting between different data types during list generation.  

```F#
let mixedNumbers = ["1"; "2.5"; "3"; "invalid"; "4.7"]
let converted = [
    for str in mixedNumbers do
        match System.Double.TryParse(str) with
        | (true, value) -> yield int(value)
        | (false, _) -> ()
]
printfn "Converted integers: %A" converted

let formatted = [
    for i in 1..5 ->
        sprintf "Item_%03d" i
]
printfn "Formatted strings: %A" formatted
```

Type conversion examples demonstrate safe parsing and string formatting.  
The first safely converts strings to integers, skipping invalid values,  
while the second shows formatted string generation with padding.

## Set operations

Implementing set-like operations using list comprehensions.  

```F#
let set1 = [1; 2; 3; 4; 5]
let set2 = [4; 5; 6; 7; 8]

let intersection = [
    for x in set1 do
        if List.contains x set2 then yield x
]
printfn "Intersection: %A" intersection

let union = [
    yield! set1
    for x in set2 do
        if not (List.contains x set1) then yield x
]
printfn "Union: %A" union
```

Set operations demonstrate membership testing and duplicate elimination.  
Intersection finds common elements, while union combines sets without  
duplicates using conditional yielding.

## Date and time processing

Working with temporal data and date ranges in comprehensions.  

```F#
open System

let startDate = DateTime(2024, 1, 1)
let weekdays = [
    for i in 0..13 do
        let date = startDate.AddDays(float i)
        if date.DayOfWeek <> DayOfWeek.Saturday && date.DayOfWeek <> DayOfWeek.Sunday then
            yield date.ToString("yyyy-MM-dd")
]
printfn "Weekdays: %A" weekdays
```

Date processing showcases temporal data filtering and formatting.  
This example generates a list of weekdays by skipping weekends,  
demonstrating how business logic integrates with comprehensions.

## Array interop

Converting between lists and arrays within comprehension contexts.  

```F#
let arrayData = [|10; 20; 30; 40; 50|]
let processedList = [
    for element in arrayData do
        let doubled = element * 2
        if doubled > 50 then yield doubled
]
printfn "Processed from array: %A" processedList

let backToArray = [| for x in processedList -> x + 1 |]
printfn "Back to array: %A" backToArray
```

Array interoperability shows seamless conversion between F# lists and  
arrays. List comprehensions naturally consume arrays, while array  
comprehensions create arrays from processed list data.

## Lazy evaluation

Demonstrating deferred computation patterns with sequence expressions.  

```F#
let lazySquares = seq {
    for i in 1..1000000 do
        printfn "Computing square of %d" i
        yield i * i
}

let firstFive = lazySquares |> Seq.take 5 |> Seq.toList
printfn "First five squares: %A" firstFive
```

Lazy evaluation examples show how sequence expressions (`seq {}`) defer  
computation until values are needed. Only the first five squares are  
actually computed despite the large range specification.

## Complex filtering

Multi-stage filtering with intermediate computations and validations.  

```F#
let candidates = [
    ("Alice", 85, "Engineer")
    ("Bob", 92, "Manager") 
    ("Carol", 78, "Designer")
    ("Dave", 88, "Engineer")
]

let qualified = [
    for (name, score, role) in candidates do
        let bonus = if role = "Engineer" then 5 else 0
        let finalScore = score + bonus
        if finalScore >= 85 then
            yield {| Name = name; Score = finalScore; Role = role |}
]
printfn "Qualified candidates: %A" qualified
```

Complex filtering demonstrates multi-criteria evaluation with score  
adjustments and role-based bonuses. Anonymous records provide clean  
output formatting for processed candidate data.

## Pattern decomposition

Advanced pattern matching for complex data structure manipulation.  

```F#
type Shape = 
    | Circle of radius: float
    | Rectangle of width: float * height: float
    | Triangle of base: float * height: float

let shapes = [
    Circle 5.0
    Rectangle (4.0, 6.0)
    Triangle (3.0, 4.0)
    Circle 3.0
]

let areas = [
    for shape in shapes ->
        match shape with
        | Circle radius -> System.Math.PI * radius * radius
        | Rectangle (w, h) -> w * h
        | Triangle (b, h) -> 0.5 * b * h
]
printfn "Areas: %A" areas
```

Pattern decomposition showcases discriminated union processing within  
list comprehensions. Each shape type is pattern matched and its area  
calculated using appropriate geometric formulas.

## List segmentation

Breaking lists into chunks and processing segments independently.  

```F#
let numbers = [1..20]
let chunks = [
    for i in 0 .. 4 .. (List.length numbers - 1) do
        yield numbers |> List.skip i |> List.take 4
]
printfn "Chunks of 4: %A" chunks

let evenOddPairs = [
    for i in 0 .. 2 .. (List.length numbers - 2) do
        yield (numbers.[i], numbers.[i + 1])
]
printfn "Even-odd pairs: %A" evenOddPairs
```

List segmentation demonstrates chunking and pairing operations.  
The first example breaks a list into fixed-size chunks, while  
the second creates adjacent element pairs for comparative analysis.

## Conditional yielding

Advanced conditional logic with multiple yield points and branches.  

```F#
let weatherData = [
    ("Monday", 75, "Sunny")
    ("Tuesday", 68, "Cloudy") 
    ("Wednesday", 82, "Sunny")
    ("Thursday", 59, "Rainy")
    ("Friday", 73, "Partly Cloudy")
]

let recommendations = [
    for (day, temp, condition) in weatherData do
        match condition with
        | "Sunny" when temp > 70 -> 
            yield sprintf "%s: Perfect for outdoor activities!" day
        | "Rainy" ->
            yield sprintf "%s: Stay indoors and read a book." day
        | _ when temp < 60 ->
            yield sprintf "%s: Dress warmly!" day
        | _ ->
            yield sprintf "%s: Nice day for a walk." day
]
printfn "Weather recommendations: %A" recommendations
```

Conditional yielding combines pattern matching with guard clauses for  
sophisticated decision making. Weather recommendations are generated  
based on multiple criteria including temperature and conditions.

## Computational expressions

Integrating list comprehensions with other computation expressions.  

```F#
let asyncResults = [
    for i in 1..5 do
        let computation = async {
            do! Async.Sleep(100)
            return i * i
        }
        yield Async.RunSynchronously computation
]
printfn "Async results: %A" asyncResults

let optionalComputation = [
    for x in [Some 1; None; Some 3; None; Some 5] do
        let result = 
            match x with
            | Some value -> Some (value * 2)
            | None -> None
        match result with
        | Some r -> yield r
        | None -> ()
]
printfn "Optional results: %A" optionalComputation
```

Computational expressions demonstrate how list comprehensions integrate  
with async workflows and option handling. Async computations are  
executed synchronously within the list context, while option chaining  
filters out None values gracefully.

## Performance optimization

Optimizing list comprehensions for better performance and memory usage.  

```F#
let largeRange = 1..10000
let efficientFiltering = [
    for n in largeRange do
        if n % 100 = 0 then // Reduce computation frequency
            let expensive = n * n * n // Expensive operation only when needed
            if expensive > 1000000 then
                yield (n, expensive)
]
printfn "Efficient results: %A" (List.take 5 efficientFiltering)

// Using sequence for lazy evaluation
let lazyComputation = seq {
    for i in 1..1000000 do
        if i % 10000 = 0 then
            yield i * i
}
let firstThree = lazyComputation |> Seq.take 3 |> Seq.toList
printfn "First three lazy results: %A" firstThree
```

Performance optimization shows how to minimize expensive computations by  
placing conditions strategically and using sequence expressions for  
deferred evaluation when working with large datasets.

## File system operations

Processing file system data using list comprehensions for directory  
traversal and file filtering.  

```F#
open System.IO

let currentDir = Directory.GetCurrentDirectory()
let files = Directory.GetFiles(currentDir, "*.md")

let fileInfo = [
    for filePath in files do
        let info = FileInfo(filePath)
        yield {| 
            Name = info.Name
            Size = info.Length
            Extension = info.Extension
            LastModified = info.LastWriteTime
        |}
]
printfn "Markdown files: %A" fileInfo

let largeFiles = [
    for file in fileInfo do
        if file.Size > 1000L then
            yield sprintf "%s (%d bytes)" file.Name file.Size
]
printfn "Large files: %A" largeFiles
```

File system operations demonstrate real-world data processing scenarios  
where list comprehensions filter and transform file metadata efficiently  
using the .NET IO APIs.

## Random data generation

Generating random test data and sampling using list comprehensions.  

```F#
open System

let random = Random(42) // Fixed seed for reproducible results

let randomInts = [
    for _ in 1..10 ->
        random.Next(1, 101)
]
printfn "Random integers: %A" randomInts

let randomPairs = [
    for i in 1..5 do
        for j in 1..3 do
            let x = random.NextDouble() * 10.0
            let y = random.NextDouble() * 10.0
            yield (i, j, (x, y))
]
printfn "Random coordinate pairs: %A" (List.take 5 randomPairs)

let weightedSample = [
    for i in 1..20 do
        let weight = random.NextDouble()
        if weight > 0.7 then // 30% selection probability
            yield (i, weight)
]
printfn "Weighted sample: %A" weightedSample
```

Random data generation showcases how list comprehensions can create  
test datasets, Monte Carlo simulations, and weighted sampling scenarios  
with controlled randomness for reproducible results.

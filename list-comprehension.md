# List comprehensions

List comprehensions are a powerful and concise syntax for creating lists in F#.  
They provide a declarative way to generate, filter, and transform data by  
combining functional programming concepts with readable syntax. List comprehensions  
can include for loops, conditional expressions, pattern matching, let bindings,  
and nested iterations, making them ideal for data processing tasks.

The basic syntax uses square brackets with for..do..yield or for..->  
constructions. You can filter elements with if conditions, transform data  
with expressions, and combine multiple data sources with nested loops.  
This approach often results in more readable and maintainable code compared  
to traditional imperative loops or complex function compositions.  

## Ranges

Range expressions generate sequential lists from start to end values.  
The -> operator creates a direct transformation of each element.

```F#
let res = [ for e in 1 .. 100 -> e * e ]
printfn "%A" res
```

This example creates a list of squares from 1 to 10000. The range 1..100  
generates numbers 1 through 100, and each number is transformed by squaring  
it. The -> operator is shorthand for yield when there's a single expression.

## Multiple for loops

Nested for loops create cartesian products of multiple sequences.  
Each combination of values from different sequences is processed.

```F#
let res2 =
    [ for r in 1..8 do
          for c in 1..8 do
              if r <> c then yield (r, c) ]

printfn "%A" res2
```

This generates coordinate pairs representing all positions on an 8x8 grid  
except diagonal positions (where row equals column). The nested loops create  
all possible combinations, and the if condition filters out unwanted pairs.  
Each valid combination yields a tuple (r, c).

## Generators

Generator expressions process existing collections and create new lists  
based on conditions and transformations. They can filter, transform,  
and combine data from multiple sources.

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

The first example filters positive numbers from a mixed list. The second  
finds numbers divisible by both 3 and 5 (multiples of 15). The third  
demonstrates nested loops creating coordinate pairs, showing how ranges  
and generators can be combined to create structured data.

## yield/yield!

The yield keyword produces single values, while yield! flattens and  
concatenates collections. This allows for flexible list construction  
and transformation patterns.

```F#
let res =
    [ for a in 1..5 do
          yield! [ a .. a + 3 ] ]

printfn "%A" res

let chars = [ 'a' .. 'z' ]

let res2 = [for e in chars do yield [e; e; e] ]
printfn "%A" res2
```

The first example uses yield! to flatten sublists into a single list.  
For each number 1-5, it creates a range of 4 numbers and flattens them.  
The result is [1; 2; 3; 4; 2; 3; 4; 5; 3; 4; 5; 6; 4; 5; 6; 7; 5; 6; 7; 8].  
The second creates a list of character triplets.

```F#
let words = [ "sky"; "cloud"; "park"; "rock"; "war" ]

let res = [ for e in words do yield e ]
printfn "%A" res

let res2 = [ for e in words do yield! e ]
printfn "%A" res2
```

This demonstrates the difference between yield and yield!. The first yields  
each word as a single element, preserving the list of strings. The second  
uses yield! to flatten each string into its individual characters, creating  
a single list of all characters from all words.


## Match pattern

Pattern matching within list comprehensions enables conditional logic  
based on value patterns. This provides powerful filtering and  
transformation capabilities.

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

This implements the classic FizzBuzz algorithm using pattern matching.  
Numbers divisible by 3 become "fizz", by 5 become "buzz", by 15 become  
"fizzbuzz", and all others are converted to strings. Note that the order  
of patterns matters - the most specific patterns should come first.

## Let expressions 

Let bindings within comprehensions allow for intermediate calculations  
and code reuse. This makes complex transformations more readable  
and efficient.

```F#
let vals = [ 1; -2; -3; 4; 5 ]

[ for v in vals do
      let f = fun e -> e > 0
      if f(v) then yield v ] |> printfn "%A"
```

This example defines a local predicate function f within the comprehension  
that tests if a number is positive. The let binding allows reusing the  
function logic and makes the intent clearer. The result filters out  
negative numbers from the input list.

## Basic filtering

Simple filtering operations demonstrate the fundamental use of conditional  
logic in list comprehensions for data selection.

```F#
// Filter even numbers
let evens = [ for n in 1..20 do if n % 2 = 0 then yield n ]
printfn "Even numbers: %A" evens

// Filter numbers greater than threshold
let threshold = 50
let bigNumbers = [ for n in 1..100 do if n > threshold then yield n ]
printfn "Numbers > %d: %A" threshold (bigNumbers |> List.take 5)
```

These examples show basic filtering patterns. The first extracts even  
numbers from 1-20, while the second finds numbers above a threshold.  
Filtering is one of the most common uses of list comprehensions.

```F#
// Filter strings by length
let words = ["cat"; "elephant"; "dog"; "butterfly"; "ant"; "hippopotamus"]
let longWords = [ for word in words do if word.Length > 5 then yield word ]
printfn "Long words: %A" longWords

// Multiple conditions
let nums = [1..50]
let special = [ for n in nums do 
                if n % 3 = 0 && n % 7 <> 0 && n > 10 then yield n ]
printfn "Special numbers: %A" special
```

Complex filtering can combine multiple conditions using logical operators.  
This allows for precise data selection based on multiple criteria.

## Transformations

Transformation operations modify elements while creating new lists,  
allowing for data processing and conversion tasks.

```F#
// Mathematical transformations
let squares = [ for x in 1..10 -> x * x ]
let cubes = [ for x in 1..5 -> x * x * x ]
let roots = [ for x in 1..9 -> sqrt (float x) ]
printfn "Squares: %A" squares
printfn "Cubes: %A" cubes
printfn "Square roots: %A" roots
```

Mathematical transformations are common in scientific and engineering  
applications. These examples show squaring, cubing, and square root  
operations applied to ranges of numbers.

```F#
// String transformations
let names = ["alice"; "BOB"; "Charlie"; "diana"]
let capitalized = [ for name in names -> name.ToUpper() ]
let lengths = [ for name in names -> (name, name.Length) ]
let initials = [ for name in names -> name.[0] ]
printfn "Capitalized: %A" capitalized
printfn "With lengths: %A" lengths
printfn "Initials: %A" initials
```

String processing is another common transformation pattern. These examples  
demonstrate case conversion, length calculation with tupling, and  
character extraction from strings.

## Working with tuples

Tuple operations in comprehensions enable structured data processing  
and coordinate manipulation tasks.

```F#
// Creating coordinate pairs
let coordinates = [ for x in 1..3 do for y in 1..3 -> (x, y) ]
printfn "All coordinates: %A" coordinates

// Distance calculations
let distances = [ for (x, y) in coordinates -> 
                  sqrt (float (x * x + y * y)) ]
printfn "Distances from origin: %A" distances
```

Tuples allow for structured data creation and processing. The first  
example creates coordinate pairs, while the second calculates distances  
from the origin for each coordinate.

```F#
// Tuple decomposition and reconstruction
let pairs = [(1, 2); (3, 4); (5, 6); (7, 8)]
let swapped = [ for (a, b) in pairs -> (b, a) ]
let sums = [ for (a, b) in pairs -> a + b ]
let products = [ for (a, b) in pairs -> (a, b, a * b) ]
printfn "Swapped: %A" swapped
printfn "Sums: %A" sums  
printfn "With products: %A" products
```

Tuple decomposition allows accessing individual elements for processing.  
These examples show swapping, summing, and extending tuples with  
calculated values.

## String processing

String manipulation in comprehensions provides powerful text processing  
capabilities for data cleaning and formatting tasks.

```F#
// Character processing
let sentence = "Hello World"
let chars = [ for c in sentence -> c ]
let upperChars = [ for c in sentence do if c <> ' ' then yield c.ToString().ToUpper() ]
let charCodes = [ for c in sentence -> int c ]
printfn "Characters: %A" chars
printfn "Upper non-spaces: %A" upperChars
printfn "ASCII codes: %A" charCodes
```

Character-level processing allows fine-grained text manipulation. These  
examples extract characters, filter and transform them, and convert  
to ASCII codes for analysis.

```F#
// Word processing
let text = "The quick brown fox jumps over the lazy dog"
let words = text.Split(' ') |> Array.toList
let wordLengths = [ for word in words -> (word, word.Length) ]
let longWords = [ for word in words do if word.Length > 4 then yield word.ToUpper() ]
let wordStarts = [ for word in words -> word.[0] ]
printfn "Word lengths: %A" wordLengths
printfn "Long words: %A" longWords
printfn "First letters: %A" wordStarts
```

Word-level processing enables document analysis and text mining. These  
examples demonstrate length analysis, filtering by size, and extracting  
word characteristics.

## Complex nested patterns

Advanced nesting patterns enable sophisticated data generation and  
processing for complex algorithms and data structures.

```F#
// Multiplication table
let multiTable = [ for i in 1..5 do 
                   for j in 1..5 -> 
                   sprintf "%d x %d = %d" i j (i * j) ]
printfn "Multiplication examples: %A" (multiTable |> List.take 10)

// Matrix generation
let matrix = [ for row in 1..3 do
               for col in 1..3 do
               if row <= col then yield (row, col, row + col) ]
printfn "Upper triangular: %A" matrix
```

Complex nested patterns can generate structured mathematical data.  
The multiplication table demonstrates formatted output generation,  
while the matrix example shows conditional element selection.

```F#
// Combinatorial generation
let suits = ["Hearts"; "Diamonds"; "Clubs"; "Spades"]  
let ranks = ["A"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9"; "10"; "J"; "Q"; "K"]
let deck = [ for suit in suits do 
             for rank in ranks -> 
             sprintf "%s of %s" rank suit ]
printfn "Card deck size: %d" deck.Length
printfn "First 5 cards: %A" (deck |> List.take 5)

// Pascal's triangle row
let pascalRow n = [ for k in 0..n -> 
                    let rec factorial x = if x <= 1 then 1 else x * factorial(x-1)
                    factorial n / (factorial k * factorial (n - k)) ]
printfn "Pascal row 5: %A" (pascalRow 5)
```

Combinatorial generation creates comprehensive datasets. The card deck  
example produces all combinations of suits and ranks, while Pascal's  
triangle demonstrates mathematical sequence generation.

## Conditional logic variations

Advanced conditional patterns provide flexible control flow for  
complex data processing scenarios.

```F#
// Multiple conditional outputs  
let categorize = [ for n in 1..30 do
                   match n with
                   | n when n % 15 = 0 -> yield sprintf "%d: FizzBuzz" n
                   | n when n % 3 = 0 -> yield sprintf "%d: Fizz" n  
                   | n when n % 5 = 0 -> yield sprintf "%d: Buzz" n
                   | n when n % 2 = 0 -> yield sprintf "%d: Even" n
                   | _ -> yield sprintf "%d: Odd" n ]
printfn "Categories: %A" (categorize |> List.take 15)

// Grade classification
let scores = [85; 92; 78; 96; 67; 88; 74; 91; 82; 69]
let grades = [ for score in scores ->
               match score with  
               | s when s >= 90 -> (s, "A")
               | s when s >= 80 -> (s, "B")  
               | s when s >= 70 -> (s, "C")
               | s when s >= 60 -> (s, "D")
               | s -> (s, "F") ]
printfn "Grades: %A" grades
```

Advanced pattern matching enables sophisticated classification systems.  
These examples show multi-category classification and grade assignment  
based on score ranges.

## Mathematical sequences

Mathematical sequence generation demonstrates the power of comprehensions  
for numerical analysis and algorithmic implementations.

```F#
// Fibonacci sequence  
let fibonacci n = 
    let rec fib a b count acc =
        if count <= 0 then List.rev acc
        else fib b (a + b) (count - 1) (a :: acc)
    fib 0 1 n []

let fibs = [ for i in 1..10 -> fibonacci i |> List.head ]
printfn "Fibonacci numbers: %A" fibs

// Prime numbers (simple sieve)
let isPrime n = 
    if n < 2 then false
    else [ 2 .. int(sqrt(float n)) ] |> List.forall (fun x -> n % x <> 0)

let primes = [ for n in 2..50 do if isPrime n then yield n ]
printfn "Prime numbers: %A" primes
```

Mathematical sequences require careful algorithm implementation. The  
Fibonacci example shows recursive calculation, while the prime sieve  
demonstrates filtering with mathematical predicates.

```F#
// Geometric progression
let geometric start ratio count = 
    [ for i in 0..count-1 -> start * (ratio ** float i) ]
let geomSeq = geometric 2.0 3.0 8
printfn "Geometric sequence: %A" geomSeq

// Triangular numbers
let triangular = [ for n in 1..10 -> n * (n + 1) / 2 ]
printfn "Triangular numbers: %A" triangular

// Perfect squares difference
let squareDiff = [ for n in 1..10 -> (n + 1) * (n + 1) - n * n ]
printfn "Square differences: %A" squareDiff
```

Different mathematical sequences showcase various patterns. Geometric  
progressions show exponential growth, triangular numbers demonstrate  
polynomial sequences, and square differences reveal linear patterns.

## Data structure transformations

Converting between different data structures using comprehensions  
enables flexible data manipulation and format conversion.

```F#
// List to indexed pairs
let items = ["apple"; "banana"; "cherry"; "date"]
let indexed = [ for i, item in List.indexed items -> (i + 1, item) ]
printfn "Indexed items: %A" indexed

// Dictionary-like transformations
let keyValues = [ for item in items -> (item.Length, item) ]
let grouped = keyValues |> List.groupBy fst |> List.map (fun (k, vs) -> (k, List.map snd vs))
printfn "Grouped by length: %A" grouped
```

Data structure transformations enable format conversion and organization.  
The indexing example adds position information, while grouping creates  
dictionary-like structures from lists.

```F#
// Flattening nested structures
let nestedLists = [[1; 2]; [3; 4; 5]; [6]; [7; 8; 9]]
let flattened = [ for sublist in nestedLists do yield! sublist ]
printfn "Flattened: %A" flattened

// Creating lookup tables
let alphabet = ['a'..'z']
let lookup = [ for c in alphabet -> (c, int c - int 'a' + 1) ]
printfn "Letter positions: %A" (lookup |> List.take 10)
```

Flattening operations combine nested structures into single collections.  
Lookup table creation maps values to computed results for efficient  
data access patterns.

## Working with sequences

Sequence comprehensions provide lazy evaluation for efficient processing  
of large datasets and infinite sequences.

```F#
// Lazy sequence processing
let lazySquares = seq { for i in 1..1000000 -> i * i }
let firstTenSquares = lazySquares |> Seq.take 10 |> Seq.toList
printfn "First 10 squares: %A" firstTenSquares

// Sequence with side effects
let withLogging = seq { 
    for i in 1..5 do 
        printfn "Processing %d" i
        yield i * 2 
}
let result = withLogging |> Seq.take 3 |> Seq.toList
printfn "Result: %A" result
```

Sequences enable lazy evaluation, computing values only when needed.  
This is essential for large datasets and allows for efficient memory  
usage. Side effects in sequences demonstrate the lazy evaluation.

## Array comprehensions

Array comprehensions provide mutable, indexed collections with better  
performance characteristics for numerical computations.

```F#
// Array generation
let arraySquares = [| for i in 1..10 -> i * i |]
printfn "Array squares: %A" arraySquares

// Multi-dimensional concept  
let coordinates = [| for x in 1..3 do
                     for y in 1..3 -> (x, y) |]
printfn "Coordinate array: %A" coordinates

// Performance-oriented processing
let largeArray = [| for i in 1..1000 do
                    if i % 100 = 0 then yield i |]
printfn "Every 100th: %A" largeArray
```

Array comprehensions use [| |] syntax instead of [ ]. They provide  
better performance for numerical operations and random access patterns.  
Large array generation demonstrates performance benefits.

## Conditional yielding patterns

Advanced yielding patterns provide flexible control over what elements  
are included in the resulting collections.

```F#
// Optional yielding
let processNumbers nums = [
    for n in nums do
        if n > 0 then yield sprintf "Positive: %d" n
        if n < 0 then yield sprintf "Negative: %d" n
        if n = 0 then yield "Zero found"
]
let mixed = [-2; 0; 3; -1; 5]
printfn "Processed: %A" (processNumbers mixed)

// Conditional multiple yields
let expandNumbers = [
    for n in [1; 2; 3] do
        yield n
        if n % 2 = 0 then 
            yield n * 10
            yield n * 100
]
printfn "Expanded: %A" expandNumbers
```

Conditional yielding allows multiple outputs per input element based on  
different conditions. This creates flexible transformation patterns  
for complex data processing requirements.

## Performance considerations

Understanding performance implications helps write efficient comprehensions  
for production applications and large datasets.

```F#
// Efficient filtering
let efficientFilter = [
    for i in 1..10000 do
        let squared = i * i  // Compute once
        if squared % 2 = 0 && squared < 1000 then 
            yield (i, squared)
]
printfn "Efficient results count: %d" efficientFilter.Length

// Avoiding repeated calculations  
let expensiveFunction x = 
    // Simulate expensive computation
    x * x * x + x * x + x + 1

let optimized = [
    for x in 1..100 do
        let result = expensiveFunction x  // Calculate once
        if result % 7 = 0 then yield (x, result)
]
printfn "Optimized results: %A" (optimized |> List.take 5)
```

Performance optimization in comprehensions involves avoiding repeated  
calculations by using let bindings. This reduces computational overhead  
and improves execution speed for complex transformations.



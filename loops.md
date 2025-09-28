# Loops

Loops are fundamental control structures in F# that allow you to execute  
code repeatedly. F# provides both imperative and functional approaches to  
iteration, emphasizing immutability and functional composition over mutable  
state. This guide covers various loop constructs, from basic for loops to  
advanced functional iteration patterns, demonstrating F#'s preference for  
immutable data structures and higher-order functions.  

## for in

The `for in` loop iterates over sequences, collections, and ranges.  

```F#
let vals = seq { 1..5 }

for e in vals do
    printfn "%d" e

printfn "--------------------"

let len = Seq.length (vals) - 1

for idx in 0..len do
    printfn "%d" (Seq.item idx vals)
```

This example demonstrates two approaches to `for in` loops. The first  
loop iterates directly over sequence elements, while the second creates  
an index-based iteration by calculating the sequence length and using  
`Seq.item` to access elements. Direct element iteration is preferred  
for better performance and readability.  

## for to/downto

The `for to` and `for downto` loops iterate over integer ranges.  

```F#
for e = 1 to 5 do
    printfn "%d" e

for e = 5 downto 1 do
    printfn "%d" e
```

These loops provide C-style integer iteration. The `to` keyword creates  
ascending loops, while `downto` creates descending loops. Both bounds  
are inclusive, making them useful for array indexing and mathematical  
computations where precise range control is needed.  

## for with range

Range expressions with step values allow custom iteration patterns.  

```F#
for e in 1..2..10 do
    printfn "%d" e

for e in 10..-2..0 do
    printfn "%d" e
```

Range syntax `start..step..end` enables arithmetic progressions with  
custom step values. Positive steps create ascending sequences, while  
negative steps create descending sequences. This pattern is efficient  
for mathematical iterations and can work with any numeric type.  

## Nested for loops

Nested loops create multi-dimensional iteration patterns.  

```F#
for i in [1; 2; 3; 4; 5; 6; 7; 6; 5; 4; 3; 2; 1] do
    for _ in 1..i do
        printf "*"
    printf "\n"
```

This example creates a diamond pattern using nested loops. The outer  
loop iterates over a list defining row widths, while the inner loop  
prints asterisks for each row. The underscore `_` discards the loop  
variable when its value isn't needed, following F# conventions.  

## for loop and functions

Loops can use function results to determine iteration bounds.  

```F#
open System

let rand1 () =
    Random(DateTime.Now.Millisecond).NextInt64(1, 10)

let rand2 () =
    Random(DateTime.Now.Millisecond).NextInt64(10, 20)

for e in rand1 () .. rand2 () do
    printfn "%d" e

printfn "--------------------------------"

for e = int (rand1 ()) to int (rand2 ()) do
    printfn "%d" e
```

Functions can define dynamic loop bounds, enabling runtime-determined  
iteration ranges. Both range syntax `start..end` and explicit bounds  
`start to end` work with function calls. Type conversion may be needed  
when functions return different numeric types than expected by the loop.  

## While loop

While loops provide imperative iteration with mutable state.  

```F#
let vals = [ 1; 2; 3; 4; 5 ]

let mutable i = 0

while i < vals.Length do
    printfn "%d" vals[i]
    i <- i + 1
```

While loops require mutable variables and explicit state management.  
Though available in F#, functional alternatives like `List.iter`,  
recursion, or higher-order functions are generally preferred for  
immutable programming styles and better composition with other  
functional constructs.  

## List iteration with List.iter

Functional iteration over lists using built-in higher-order functions.  

```F#
let numbers = [1; 2; 3; 4; 5]
let names = ["Alice"; "Bob"; "Charlie"]

// Simple iteration
numbers |> List.iter (printfn "Number: %d")

printfn "--------------------"

// Iteration with index
numbers |> List.iteri (fun i x -> printfn "Index %d: %d" i x)

printfn "--------------------"

// Complex operations during iteration
names |> List.iter (fun name -> 
    let upper = name.ToUpper()
    printfn "%s -> %s" name upper)
```

`List.iter` applies a function to each list element without returning  
values, making it perfect for side effects like printing. `List.iteri`  
provides both element and index. This functional approach eliminates  
mutable counters and provides better composition with other operations.  

## Sequence iteration with Seq.iter

Lazy evaluation and infinite sequence iteration capabilities.  

```F#
let sequence = seq { 1..10 }
let infiniteSeq = Seq.initInfinite (fun i -> i * i)

// Iterate over finite sequence
sequence |> Seq.iter (printfn "Seq element: %d")

printfn "--------------------"

// Take elements from infinite sequence
infiniteSeq 
|> Seq.take 5 
|> Seq.iter (printfn "Square: %d")

printfn "--------------------"

// Filtered iteration
seq { 1..20 }
|> Seq.filter (fun x -> x % 3 = 0)
|> Seq.iter (printfn "Multiple of 3: %d")
```

Sequences enable lazy evaluation, computing elements on-demand. This  
allows working with infinite sequences safely by combining with `take`,  
`takeWhile`, or filtering operations. Memory usage remains constant  
regardless of theoretical sequence length.  

## Array iteration with Array.iter

Efficient iteration over arrays with mutable operations support.  

```F#
let arr = [| 1; 2; 3; 4; 5 |]
let mutableArr = [| 1; 2; 3; 4; 5 |]

// Simple array iteration
arr |> Array.iter (printfn "Array element: %d")

printfn "--------------------"

// Iteration with index for mutations
mutableArr |> Array.iteri (fun i x -> 
    mutableArr[i] <- x * 2
    printfn "Modified index %d: %d -> %d" i x mutableArr[i])

printfn "--------------------"

// Display final array
mutableArr |> Array.iter (printfn "Final: %d")
```

Arrays provide constant-time access and support in-place mutations.  
`Array.iteri` enables index-based modifications during iteration.  
While mutation is available, functional transformations like `Array.map`  
are preferred for immutable operations.  

## Map iteration with Map.iter

Iterating over key-value pairs in maps and dictionaries.  

```F#
let scoreMap = Map.ofList [
    ("Alice", 95)
    ("Bob", 87)
    ("Charlie", 92)
    ("David", 78)
]

// Iterate over key-value pairs
scoreMap |> Map.iter (fun key value -> 
    printfn "Student: %s, Score: %d" key value)

printfn "--------------------"

// Conditional iteration
scoreMap 
|> Map.filter (fun _ score -> score >= 90)
|> Map.iter (fun name score -> 
    printfn "%s achieved excellence with %d" name score)

printfn "--------------------"

// Transform during iteration
scoreMap |> Map.iter (fun name score ->
    let grade = if score >= 90 then "A" 
                elif score >= 80 then "B"
                else "C"
    printfn "%s: %d (%s)" name score grade)
```

Maps store key-value associations with efficient lookups. `Map.iter`  
provides access to both keys and values during iteration. Combining  
with filtering operations enables selective processing of map contents  
based on keys, values, or both.  

## Set iteration with Set.iter

Iterating over unique elements in mathematical sets.  

```F#
let numberSet = Set.ofList [5; 2; 8; 2; 1; 9; 5; 3]
let stringSet = Set.ofList ["apple"; "banana"; "apple"; "cherry"]

// Set automatically removes duplicates
numberSet |> Set.iter (printfn "Number: %d")

printfn "--------------------"

stringSet |> Set.iter (printfn "Fruit: %s")

printfn "--------------------"

// Set operations during iteration
let evenNumbers = Set.filter (fun x -> x % 2 = 0) numberSet
let oddNumbers = Set.filter (fun x -> x % 2 = 1) numberSet

printfn "Even numbers:"
evenNumbers |> Set.iter (printfn "  %d")

printfn "Odd numbers:"
oddNumbers |> Set.iter (printfn "  %d")
```

Sets maintain unique elements with efficient membership testing.  
Iteration occurs in sorted order. Set operations like union,  
intersection, and difference can be combined with iteration for  
mathematical computations on collections.  

## Recursive loops with tail recursion

Functional loop patterns using recursive functions.  

```F#
// Tail-recursive countdown
let rec countdown n =
    if n <= 0 then
        printfn "Done!"
    else
        printfn "Countdown: %d" n
        countdown (n - 1)

// Tail-recursive list processing
let rec processListTailRec acc list =
    match list with
    | [] -> List.rev acc
    | head :: tail ->
        let result = head * 2
        printfn "Processing: %d -> %d" head result
        processListTailRec (result :: acc) tail

// Tail-recursive sum with iteration effect
let rec sumWithPrint acc nums =
    match nums with
    | [] -> 
        printfn "Final sum: %d" acc
        acc
    | x :: xs ->
        printfn "Adding %d, running total: %d" x (acc + x)
        sumWithPrint (acc + x) xs

// Usage
countdown 5
printfn "--------------------"

let doubled = processListTailRec [] [1; 2; 3; 4; 5]
printfn "Result: %A" doubled

printfn "--------------------"

let total = sumWithPrint 0 [1; 2; 3; 4; 5]
```

Tail recursion provides stack-safe loops with functional purity.  
The F# compiler optimizes tail calls into iterative loops, preventing  
stack overflow for large iterations. Pattern matching on lists enables  
elegant recursive processing without explicit indexing.  

## Loop with break simulation using exceptions

Exception-based early loop termination patterns.  

```F#
exception BreakException

let findFirstEven numbers =
    try
        for num in numbers do
            printfn "Checking: %d" num
            if num % 2 = 0 then
                printfn "Found even number: %d" num
                raise BreakException
            else
                printfn "  %d is odd, continuing..." num
        printfn "No even number found"
        None
    with
    | BreakException -> Some()

let searchWithEarlyExit predicate items =
    try
        for item in items do
            printfn "Examining: %A" item
            if predicate item then
                printfn "Match found: %A" item
                raise BreakException
        printfn "No match found"
        None
    with
    | BreakException -> Some()

// Usage
let numbers1 = [1; 3; 7; 8; 9; 11]
let numbers2 = [1; 3; 7; 9; 11]

printfn "Searching in first list:"
findFirstEven numbers1 |> ignore

printfn "\nSearching in second list:"
findFirstEven numbers2 |> ignore

printfn "\nCustom search:"
let names = ["Alice"; "Bob"; "Charlie"; "David"]
searchWithEarlyExit (fun name -> name.StartsWith("C")) names |> ignore
```

Exception-based breaks provide imperative-style early termination.  
However, functional alternatives like `List.tryFind`, `Seq.tryFind`,  
or recursive functions with pattern matching are more idiomatic and  
performant for finding elements or conditions.  

## Loop with continue simulation using pattern matching

Pattern matching for conditional loop continuation.  

```F#
let processNumbers numbers =
    for num in numbers do
        match num with
        | x when x < 0 -> 
            printfn "Skipping negative number: %d" x
        | 0 ->
            printfn "Skipping zero"
        | x when x % 2 = 0 ->
            printfn "Processing even number: %d (doubled: %d)" x (x * 2)
        | x ->
            printfn "Processing odd number: %d (squared: %d)" x (x * x)

let processConditionally condition processor items =
    for item in items do
        if condition item then
            processor item
        else
            printfn "Skipping item: %A" item

let validateAndProcess validator processor items =
    for item in items do
        match validator item with
        | Some validItem ->
            processor validItem
        | None ->
            printfn "Invalid item skipped: %A" item

// Usage
let mixedNumbers = [-2; 0; 1; 2; 3; 4; -5; 6]
printfn "Processing mixed numbers:"
processNumbers mixedNumbers

printfn "\nConditional processing:"
let words = ["apple"; ""; "banana"; "a"; "cherry"]
processConditionally 
    (fun s -> s.Length > 2)
    (fun s -> printfn "Valid word: %s (length: %d)" s s.Length)
    words

printfn "\nValidation with processing:"
let parseInteger (s: string) = 
    match System.Int32.TryParse(s) with
    | true, n -> Some n
    | false, _ -> None

let stringNumbers = ["123"; "abc"; "456"; "def"; "789"]
validateAndProcess
    parseInteger
    (fun n -> printfn "Parsed number: %d (doubled: %d)" n (n * 2))
    stringNumbers
```

Pattern matching provides elegant conditional processing within loops.  
Guard conditions (`when`) enable complex filtering logic. Optional  
types and validation functions create robust processing pipelines  
without explicit continue statements.  

## Parallel for loops with Array.Parallel.iter

Concurrent iteration for CPU-intensive operations.  

```F#
open System
open System.Diagnostics

let heavyComputation x =
    // Simulate CPU-intensive work
    let result = 
        [1..1000] 
        |> List.fold (fun acc i -> acc + (x * i)) 0
    System.Threading.Thread.Sleep(10) // Additional delay
    result

let numbers = [|1..20|]

printfn "Sequential processing:"
let sw1 = Stopwatch.StartNew()
numbers |> Array.iter (fun x ->
    let result = heavyComputation x
    printfn "Thread %d processed %d -> %d" 
        System.Threading.Thread.CurrentThread.ManagedThreadId x result)
sw1.Stop()
printfn "Sequential time: %d ms" sw1.ElapsedMilliseconds

printfn "\nParallel processing:"
let sw2 = Stopwatch.StartNew()
numbers |> Array.Parallel.iter (fun x ->
    let result = heavyComputation x
    printfn "Thread %d processed %d -> %d" 
        System.Threading.Thread.CurrentThread.ManagedThreadId x result)
sw2.Stop()
printfn "Parallel time: %d ms" sw2.ElapsedMilliseconds

printfn "\nSpeedup ratio: %.2f" 
    (float sw1.ElapsedMilliseconds / float sw2.ElapsedMilliseconds)
```

Parallel iteration distributes work across multiple CPU cores for  
independent operations. Each element is processed concurrently,  
significantly reducing execution time for CPU-bound tasks. Thread  
safety becomes important when sharing mutable state between iterations.  

## Async for loops

Asynchronous iteration for I/O-bound operations.  

```F#
open System
open System.Threading.Tasks

let asyncOperation x = async {
    do! Async.Sleep(100) // Simulate async I/O
    let timestamp = DateTime.Now.ToString("HH:mm:ss.fff")
    return sprintf "Processed: %d at %s" x timestamp
}

let processUrlsAsync urls = async {
    for url in urls do
        try
            let! result = asyncOperation (hash url)
            printfn "URL %s: %s" url result
        with
        | ex -> printfn "Error processing %s: %s" url ex.Message
}

let parallelAsyncProcessing items = async {
    let! results = 
        items
        |> List.map asyncOperation
        |> Async.Parallel
    
    results |> Array.iteri (fun i result ->
        printfn "Result %d: %s" i result)
}

// Usage
let urls = [
    "http://example.com"
    "http://google.com" 
    "http://github.com"
    "http://stackoverflow.com"
]

printfn "Sequential async processing:"
processUrlsAsync urls |> Async.RunSynchronously

printfn "\nParallel async processing:"
[1; 2; 3; 4; 5] |> parallelAsyncProcessing |> Async.RunSynchronously
```

Async workflows enable non-blocking iteration over I/O operations.  
Sequential async processing maintains order while yielding control  
during waits. `Async.Parallel` processes multiple operations  
concurrently, maximizing throughput for independent async tasks.  

## Conditional loops with guards

Guard conditions for sophisticated loop filtering.  

```F#
let processWithGuards items =
    for item in items do
        match item with
        | x when x > 100 && x % 10 = 0 -> 
            printfn "Large round number: %d" x
        | x when x > 50 -> 
            printfn "Medium number: %d (half: %.1f)" x (float x / 2.0)
        | x when x > 0 && x <= 10 -> 
            printfn "Small positive: %d (factorial: %d)" x 
                (List.fold (*) 1 [1..x])
        | 0 -> 
            printfn "Zero encountered"
        | x when x < 0 -> 
            printfn "Negative number: %d (absolute: %d)" x (abs x)
        | x -> 
            printfn "Regular number: %d" x

let filterAndProcess predicate processor items =
    items
    |> List.filter predicate
    |> List.iter (fun item ->
        printfn "Processing filtered item:"
        processor item)

let multiGuardLoop ranges =
    for (start, end_, step) in ranges do
        printfn "Range: %d to %d step %d" start end_ step
        for i in start..step..end_ do
            match i with
            | x when x % 15 = 0 -> printfn "  FizzBuzz (%d)" x
            | x when x % 3 = 0 -> printfn "  Fizz (%d)" x  
            | x when x % 5 = 0 -> printfn "  Buzz (%d)" x
            | x -> printfn "  %d" x

// Usage
let numbers = [120; 75; 5; 0; -10; 25; 150; 1]
processWithGuards numbers

printfn "\nFiltered processing:"
filterAndProcess (fun x -> x % 2 = 0) (fun x -> 
    printfn "  Even number %d squared: %d" x (x * x)) numbers

printfn "\nMulti-guard FizzBuzz:"
multiGuardLoop [(1, 15, 1); (20, 35, 2)]
```

Guard conditions enable complex conditional logic within pattern  
matches. Multiple guards can implement sophisticated filtering and  
processing logic. Combined with filtering functions, guards create  
highly expressive and maintainable loop conditions.  

## Infinite sequences and take/takeWhile

Lazy evaluation with infinite data structures.  

```F#
let fibonacci = 
    let rec fibSeq a b = seq {
        yield a
        yield! fibSeq b (a + b)
    }
    fibSeq 0 1

let primes = 
    let rec sieve numbers = seq {
        match Seq.tryHead numbers with
        | None -> ()
        | Some prime ->
            yield prime
            let remaining = Seq.filter (fun x -> x % prime <> 0) numbers
            yield! sieve remaining
    }
    seq { 2..System.Int32.MaxValue } |> sieve

let powers n = Seq.initInfinite (fun i -> pown n i)

// Safe iteration with take/takeWhile
printfn "First 10 Fibonacci numbers:"
fibonacci 
|> Seq.take 10 
|> Seq.iter (printfn "  %d")

printfn "\nFibonacci numbers under 1000:"
fibonacci 
|> Seq.takeWhile (fun x -> x < 1000)
|> Seq.iter (printfn "  %d")

printfn "\nFirst 8 prime numbers:"
primes
|> Seq.take 8
|> Seq.iter (printfn "  %d")

printfn "\nPowers of 2 less than 1000:"
powers 2
|> Seq.takeWhile (fun x -> x < 1000)
|> Seq.iter (printfn "  %d")

printfn "\nPowers of 3 with index:"
powers 3
|> Seq.take 7
|> Seq.iteri (fun i x -> printfn "  3^%d = %d" i x)
```

Infinite sequences represent unbounded data structures with lazy  
evaluation. `take` limits results to a fixed count, while `takeWhile`  
continues until a condition fails. This pattern enables mathematical  
sequence processing without memory constraints.  

## Loop over key-value pairs

Dictionary and record iteration patterns.  

```F#
open System.Collections.Generic

let studentGrades = Dictionary<string, int list>()
studentGrades.Add("Alice", [95; 87; 92])
studentGrades.Add("Bob", [78; 85; 79])
studentGrades.Add("Charlie", [92; 94; 89])

let employeeRecords = [
    {| Name = "John"; Department = "Engineering"; Salary = 75000 |}
    {| Name = "Jane"; Department = "Marketing"; Salary = 65000 |}
    {| Name = "Mike"; Department = "Engineering"; Salary = 80000 |}
    {| Name = "Sarah"; Department = "Sales"; Salary = 70000 |}
]

// Dictionary iteration
printfn "Student grade analysis:"
for KeyValue(student, grades) in studentGrades do
    let average = List.average (List.map float grades)
    let highest = List.max grades
    let lowest = List.min grades
    printfn "%s: avg=%.1f, high=%d, low=%d, grades=%A" 
        student average highest lowest grades

printfn "\nEmployee records by department:"
employeeRecords
|> List.groupBy (fun emp -> emp.Department)
|> List.iter (fun (dept, employees) ->
    printfn "%s Department:" dept
    employees |> List.iter (fun emp ->
        printfn "  %s: $%s" emp.Name 
            (emp.Salary.ToString("N0"))))

// Custom key-value processing
let processKeyValuePairs pairs processor =
    for (key, value) in pairs do
        processor key value

printfn "\nCustom key-value processing:"
let colorCodes = [
    ("Red", "#FF0000")
    ("Green", "#00FF00") 
    ("Blue", "#0000FF")
    ("Yellow", "#FFFF00")
]

processKeyValuePairs colorCodes (fun color code ->
    printfn "%s: %s (length: %d)" color code code.Length)
```

Key-value iteration supports dictionaries, maps, and tuple lists.  
Pattern matching on `KeyValue` pairs provides clean syntax for  
dictionary processing. Grouping operations create hierarchical  
iteration patterns for complex data analysis.  

## Loop with index using mapi functions

Index-aware functional iteration patterns.  

```F#
let colors = ["red"; "green"; "blue"; "yellow"; "purple"]
let numbers = [|10; 20; 30; 40; 50|]

// List.mapi for indexed processing
printfn "Indexed list processing:"
colors 
|> List.mapi (fun i color -> (i, color.ToUpper()))
|> List.iter (fun (index, upperColor) ->
    printfn "%d: %s" index upperColor)

printfn "\nArray indexed modification:"
numbers
|> Array.mapi (fun i x -> 
    let result = x * (i + 1)
    printfn "Index %d: %d * %d = %d" i x (i + 1) result
    result)
|> Array.iter (printfn "Final: %d")

// Sequence with index
printfn "\nSequence with custom indexing:"
seq { "apple"; "banana"; "cherry"; "date" }
|> Seq.mapi (fun i fruit ->
    sprintf "%d. %s (%d chars)" (i + 1) fruit fruit.Length)
|> Seq.iter (printfn "%s")

// Complex indexed operations
let processWithContext items =
    items
    |> List.mapi (fun i item ->
        let context = {|
            Index = i
            IsFirst = (i = 0)
            IsLast = (i = List.length items - 1)
            Position = sprintf "%d/%d" (i + 1) (List.length items)
            Item = item
        |}
        context)
    |> List.iter (fun ctx ->
        let marker = 
            if ctx.IsFirst then " (FIRST)"
            elif ctx.IsLast then " (LAST)"
            else ""
        printfn "[%s] %A%s" ctx.Position ctx.Item marker)

printfn "\nContext-aware processing:"
processWithContext ["alpha"; "beta"; "gamma"; "delta"]
```

Index-aware iteration provides position information alongside values.  
`mapi` functions transform elements with their indices, enabling  
positional logic. Context objects can encapsulate rich positional  
metadata for complex processing scenarios.  

## Fold as loop alternative

Accumulation patterns replacing traditional loops.  

```F#
// Traditional imperative approach (for comparison)
let imperativeSum list =
    let mutable total = 0
    for x in list do
        total <- total + x
        printfn "Added %d, running total: %d" x total
    total

// Fold-based approach
let functionalSum list =
    list |> List.fold (fun acc x ->
        let newAcc = acc + x
        printfn "Added %d, running total: %d" x newAcc
        newAcc) 0

// Complex fold operations
let analyzeNumbers numbers =
    let initialState = {| 
        Sum = 0; Count = 0; Max = Int32.MinValue; Min = Int32.MaxValue 
    |}
    
    numbers |> List.fold (fun acc x ->
        printfn "Processing %d..." x
        {|
            Sum = acc.Sum + x
            Count = acc.Count + 1
            Max = max acc.Max x
            Min = min acc.Min x
        |}) initialState

// Fold with early termination simulation
let findFirstNegative numbers =
    let folder acc x =
        match acc with
        | Some _ -> acc  // Already found, keep existing result
        | None when x < 0 -> 
            printfn "Found first negative: %d" x
            Some x
        | None -> 
            printfn "Checking %d (positive)" x
            None
    
    numbers |> List.fold folder None

// Right fold for different associativity
let buildString words =
    printfn "Left fold (fold):"
    let leftResult = words |> List.fold (fun acc word -> 
        let result = acc + "-" + word
        printfn "  %s + %s = %s" acc word result
        result) "START"
    
    printfn "Right fold (foldBack):"
    let rightResult = List.foldBack (fun word acc -> 
        let result = word + "-" + acc  
        printfn "  %s + %s = %s" word acc result
        result) words "END"
    
    (leftResult, rightResult)

// Usage
let numbers = [1; 2; 3; 4; 5]
printfn "Imperative sum:"
let imp = imperativeSum numbers

printfn "\nFunctional sum:"
let func = functionalSum numbers

printfn "\nComplex analysis:"
let analysis = analyzeNumbers numbers
printfn "Analysis: %A" analysis

printfn "\nFirst negative search:"
let testNumbers = [5; 3; 8; -2; 1; -7]
let firstNeg = findFirstNegative testNumbers
printfn "Result: %A" firstNeg

printfn "\nString building:"
let words = ["apple"; "banana"; "cherry"]
let (left, right) = buildString words
printfn "Left result: %s" left
printfn "Right result: %s" right
```

Fold operations accumulate values through collections, replacing many  
traditional loops. Left folds (`fold`) and right folds (`foldBack`)  
provide different evaluation orders. Complex state objects enable  
sophisticated analysis in single passes through data structures.  

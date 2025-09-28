# F# Arrays - Comprehensive Guide

Arrays in F# are mutable, zero-indexed collections that provide efficient  
random access to elements. Unlike lists, arrays are stored as contiguous  
blocks of memory, making them ideal for performance-critical operations  
and interoperability with .NET libraries. Arrays support in-place  
modifications and offer better cache locality for numerical computations.  

F# arrays are .NET arrays under the hood, providing seamless integration  
with existing .NET code. They offer O(1) access time for reading and  
writing elements, making them suitable for scenarios requiring frequent  
element access or updates. The Array module provides a rich set of  
functional programming operations while maintaining mutability when needed.  

## Basic Array Creation

Creating arrays using literal syntax and various initialization methods.  

```f#
open System

let vals = [| 1; 2; 3; 4; 5 |]
Console.WriteLine(Array.length vals)

let vals2 = [| 1 .. 100 |]
printfn "%A" vals2[10..20]

Console.WriteLine("-----------------------")

let vals3 = [|
    2
    3
    4
|]

vals3 |> Array.iter Console.WriteLine

Console.WriteLine("-----------------------")

let vals4 = Array.init 10 (fun x -> x + 10)
printfn "%A" vals4

Console.WriteLine("-----------------------")

let vals5 = [|4; 2; 1; -4; -1; 0; 7|]
printfn "%A" vals5

Array.sortInPlace vals5
printfn "%A" vals5
```

This example demonstrates various ways to create arrays in F#. The literal  
syntax [| |] creates arrays with explicit elements. Range expressions  
create sequences of consecutive values. Array.init generates arrays using  
a function. The sortInPlace function modifies arrays in-place for  
performance, showcasing the mutable nature of arrays.

## Array Comprehensions

Array comprehensions use ranges, generators and conditional expressions  
to create arrays declaratively.  

```f#
let vals = [| 1 .. 10 |]
printfn "%A" vals

let vals2 = [| 1 .. 3 .. 10 |]
printfn "%A" vals2

let vals3 =
    [| for e in 1 .. 5 do
           yield (e, e * e, e * e * e) |]

printfn "%A" vals3

let vals4 = [| for e in 1 .. 10 -> e * e |]
printfn "%A" vals4
```

Array comprehensions provide a concise syntax for generating arrays with  
patterns, filters, and transformations. The range syntax creates sequences  
with optional step values. The yield keyword explicitly produces elements,  
while the arrow syntax (->) provides shorthand for simple transformations.  
These comprehensions are evaluated eagerly, creating the entire array in  
memory immediately.


## Array Indexing & Slicing

Array elements are accessed by index, with modern and traditional syntax  
options available.  

```f#
open System 

let vals = [| 1; 3; 4; 6; 7; 8; 9|]

Console.WriteLine(vals.[0])
Console.WriteLine(vals.[1])
Console.WriteLine(vals.[2])

Console.WriteLine("-------------------")

// since .NET 6
Console.WriteLine(vals[0])
Console.WriteLine(vals[1])
Console.WriteLine(vals[2])

Console.WriteLine("-------------------")

// slices
printfn "%A" vals[0..6]
printfn "%A" vals[..6]
printfn "%A" vals[2..]

Console.WriteLine("-------------------")

Console.WriteLine(Array.last vals)
Console.WriteLine(Array.length vals)
```

Since .NET 6, F# supports modern indexing syntax vals[i] alongside the  
traditional vals.[i] syntax. Slicing operations create new arrays with  
subsets of elements using range syntax. F# does not yet support rear-based  
indexing & slicing as proposed in FS-1076. Array access is zero-indexed  
with bounds checking that throws exceptions for invalid indices.

## Empty Arrays and Array Creation

Working with empty arrays and different methods for array initialization.  

```f#
open System

// Empty array
let emptyArray: int array = [||]
let emptyArray2 = Array.empty<string>

printfn "Empty array length: %d" emptyArray.Length
printfn "Empty array2 length: %d" emptyArray2.Length

// Create array with default values
let defaultInts = Array.zeroCreate<int> 5
printfn "Default ints: %A" defaultInts

let defaultStrings = Array.create 3 "default"
printfn "Default strings: %A" defaultStrings

// Create array from sequence
let fromSeq = [1..5] |> Array.ofSeq
printfn "From sequence: %A" fromSeq
```

Array creation methods provide flexibility for different scenarios. Empty  
arrays serve as starting points for accumulation patterns. Array.zeroCreate  
initializes with type defaults, while Array.create fills with specified  
values. Converting from sequences allows leveraging sequence operations  
before materializing as arrays.

## Array Concatenation and Copying

Combining arrays and creating copies for immutable-style operations.  

```f#
let arr1 = [|1; 2; 3|]
let arr2 = [|4; 5; 6|]
let arr3 = [|7; 8; 9|]

// Concatenate arrays
let combined = Array.concat [arr1; arr2; arr3]
printfn "Combined: %A" combined

// Append two arrays
let appended = Array.append arr1 arr2
printfn "Appended: %A" appended

// Copy array
let copied = Array.copy arr1
printfn "Copied: %A" copied
printfn "Original and copy are same reference: %b" (arr1 = copied)

// Shallow copy vs reference
copied[0] <- 99
printfn "After modifying copy - Original: %A, Copy: %A" arr1 copied
```

Array concatenation creates new arrays combining multiple source arrays.  
Array.append joins two arrays efficiently. Array.copy creates shallow  
copies, ensuring modifications to the copy don't affect the original.  
These operations support functional programming patterns while working  
with mutable arrays.

## Array Transformation with Map

Transforming array elements using map operations for data processing.  

```f#
let numbers = [|1; 2; 3; 4; 5|]

// Simple transformation
let doubled = numbers |> Array.map (fun x -> x * 2)
printfn "Doubled: %A" doubled

// String transformation
let strings = numbers |> Array.map (fun x -> sprintf "Number: %d" x)
printfn "To strings: %A" strings

// Complex transformation with tuples
let complexTransform = 
    numbers 
    |> Array.map (fun x -> (x, x * x, x * x * x))
printfn "Complex: %A" complexTransform

// Map with index
let indexed = numbers |> Array.mapi (fun i x -> sprintf "Index %d: %d" i x)
printfn "Indexed: %A" indexed
```

Array.map applies transformations to every element, creating new arrays  
with transformed values. The original array remains unchanged, supporting  
functional programming patterns. Array.mapi provides both index and value  
for more complex transformations. Map operations are ideal for data  
conversion and preparation tasks.

## Array Filtering and Conditions

Selecting array elements based on predicates and conditions.  

```f#
let mixedNumbers = [|1; -2; 3; -4; 5; -6; 7; 8; -9; 10|]

// Filter positive numbers
let positives = mixedNumbers |> Array.filter (fun x -> x > 0)
printfn "Positives: %A" positives

// Filter even numbers
let evens = mixedNumbers |> Array.filter (fun x -> x % 2 = 0)
printfn "Evens: %A" evens

// Complex filtering
let complexFilter = 
    mixedNumbers 
    |> Array.filter (fun x -> x > 0 && x % 2 = 1)
printfn "Positive odds: %A" complexFilter

// Choose (map + filter combined)
let processedNumbers = 
    mixedNumbers 
    |> Array.choose (fun x -> if x > 0 then Some(x * 2) else None)
printfn "Processed positives: %A" processedNumbers
```

Array filtering creates new arrays containing only elements matching  
predicates. Array.filter applies boolean conditions to select elements.  
Array.choose combines mapping and filtering, using Option types to  
include or exclude elements based on complex logic. These operations  
enable powerful data selection patterns.

## Array Searching and Finding

Locating elements and checking array contents with search operations.  

```f#
let fruits = [|"apple"; "banana"; "cherry"; "date"; "elderberry"|]
let numbers = [|10; 20; 30; 40; 50|]

// Find operations
let found = Array.find (fun x -> x.StartsWith("c")) fruits
printfn "Found fruit starting with 'c': %s" found

let tryFound = Array.tryFind (fun x -> x.StartsWith("z")) fruits
printfn "Fruit starting with 'z': %A" tryFound

// Index operations
let index = Array.findIndex (fun x -> x = "cherry") fruits
printfn "Index of 'cherry': %d" index

let tryIndex = Array.tryFindIndex (fun x -> x = "grape") fruits
printfn "Index of 'grape': %A" tryIndex

// Existence checks
let exists = Array.exists (fun x -> x.Length > 8) fruits
printfn "Has fruit with length > 8: %b" exists

let forAll = Array.forall (fun x -> x.Length > 2) fruits
printfn "All fruits have length > 2: %b" forAll
```

Array search operations locate elements using predicates. Find operations  
return elements or indices, with try variants returning Options for safe  
handling. Existence checks verify conditions across array contents.  
These operations provide essential data querying capabilities while  
maintaining type safety.

## Array Sorting and Ordering

Organizing array elements using various sorting strategies and criteria.  

```f#
let unsorted = [|5; 2; 8; 1; 9; 3|]
let words = [|"zebra"; "apple"; "banana"; "cherry"|]

// In-place sorting (mutates original)
let mutableArray = Array.copy unsorted
Array.sortInPlace mutableArray
printfn "Sorted in place: %A" mutableArray

// Functional sorting (returns new array)
let sorted = Array.sort unsorted
printfn "Original: %A" unsorted
printfn "Sorted copy: %A" sorted

// Sort by criteria
let sortedByLength = Array.sortBy (fun s -> s.Length) words
printfn "Sorted by length: %A" sortedByLength

// Descending sort
let descendingSort = Array.sortByDescending id unsorted
printfn "Descending: %A" descendingSort

// Custom comparison
let customSort = Array.sortWith (fun x y -> compare (x % 3) (y % 3)) unsorted
printfn "Sorted by modulo 3: %A" customSort
```

Array sorting provides both mutable and immutable approaches. In-place  
sorting modifies the original array for performance. Functional sorting  
creates new arrays preserving originals. Sort criteria enable ordering  
by computed values or custom comparisons. This flexibility supports  
various data organization requirements.

## Array Aggregation and Reduction

Computing single values from arrays using fold, reduce, and aggregate  
operations.  

```f#
let numbers = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]

// Sum and product
let sum = Array.sum numbers
let product = Array.fold (*) 1 numbers
printfn "Sum: %d, Product: %d" sum product

// Average calculations
let average = Array.average (Array.map float numbers)
printfn "Average: %.2f" average

// Custom aggregations with fold
let concatenated = 
    [|"Hello"; " "; "F#"; " "; "World"|] 
    |> Array.fold (+) ""
printfn "Concatenated: %s" concatenated

// Scan operations (intermediate results)
let runningSum = Array.scan (+) 0 numbers
printfn "Running sum: %A" runningSum

// Reduce (no initial value needed)
let maxValue = Array.reduce max numbers
printfn "Maximum: %d" maxValue
```

Array aggregation transforms entire arrays into single values. Fold  
operations apply binary functions with initial values. Reduce operations  
work without initial values, requiring non-empty arrays. Scan operations  
preserve intermediate results. These operations enable statistical  
calculations and data summarization.

## Array Partitioning and Grouping

Dividing arrays into multiple parts based on conditions and criteria.  

```f#
let mixedData = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12|]

// Partition by predicate
let evens, odds = Array.partition (fun x -> x % 2 = 0) mixedData
printfn "Evens: %A" evens
printfn "Odds: %A" odds

// Split into chunks
let chunked = Array.chunkBySize 3 mixedData
printfn "Chunked by 3: %A" chunked

// Split at specific indices
let firstHalf, secondHalf = Array.splitAt 6 mixedData
printfn "First half: %A" firstHalf
printfn "Second half: %A" secondHalf

// Group by key
let students = [|("Alice", "A"); ("Bob", "B"); ("Charlie", "A"); ("David", "B")|]
let grouped = Array.groupBy snd students
printfn "Grouped by grade: %A" grouped

// Take and skip operations
let taken = Array.take 5 mixedData
let skipped = Array.skip 5 mixedData
printfn "First 5: %A" taken
printfn "Remaining: %A" skipped
```

Array partitioning creates multiple arrays from a single source based  
on conditions or sizes. Partition operations split arrays by predicates.  
Chunking creates fixed-size segments. Grouping organizes elements by  
computed keys. These operations support data organization and batch  
processing scenarios.

## Multi-dimensional Arrays

Working with 2D and higher-dimensional arrays for matrix operations  
and structured data.  

```f#
// 2D array creation
let matrix2D = Array2D.create 3 4 0
let initialized2D = Array2D.init 3 3 (fun i j -> i * 3 + j)

printfn "2D Array dimensions: %d x %d" (Array2D.length1 matrix2D) (Array2D.length2 matrix2D)
printfn "Initialized 2D: %A" initialized2D

// Setting and getting values
matrix2D[0, 0] <- 1
matrix2D[1, 1] <- 5
matrix2D[2, 3] <- 9

printfn "Modified matrix: %A" matrix2D

// Jagged arrays (array of arrays)
let jaggedArray = [|
    [|1; 2; 3|]
    [|4; 5|]
    [|6; 7; 8; 9|]
|]

printfn "Jagged array: %A" jaggedArray
printfn "Second row: %A" jaggedArray[1]

// Matrix operations
let matrixA = Array2D.init 2 2 (fun i j -> i + j + 1)
let matrixB = Array2D.init 2 2 (fun i j -> (i + 1) * (j + 1))

printfn "Matrix A: %A" matrixA
printfn "Matrix B: %A" matrixB
```

Multi-dimensional arrays support matrix operations and structured data  
storage. Array2D provides true 2D arrays with efficient memory layout.  
Jagged arrays offer flexible row sizes using arrays of arrays. These  
structures enable mathematical computations and grid-based data  
manipulation with type safety.

## Array Conversion and Interoperability

Converting between arrays and other collection types for .NET integration.  

```f#
open System.Collections.Generic

// Array to other collections
let sourceArray = [|1; 2; 3; 4; 5|]

let asList = Array.toList sourceArray
let asSeq = Array.toSeq sourceArray
let asGenericList = List<int>(sourceArray)

printfn "As list: %A" asList
printfn "As sequence: %A" (asSeq |> Seq.toArray)
printfn "As generic list: %A" (asGenericList |> Seq.toArray)

// From other collections
let fromList = [1; 2; 3; 4; 5] |> Array.ofList
let fromSeq = seq {1..5} |> Array.ofSeq
let fromGenericList = asGenericList |> Array.ofSeq

printfn "From list: %A" fromList
printfn "From sequence: %A" fromSeq
printfn "From generic list: %A" fromGenericList

// String operations
let stringArray = [|"apple"; "banana"; "cherry"|]
let joined = String.concat ", " stringArray
let split = joined.Split(", ")

printfn "Joined: %s" joined
printfn "Split back: %A" split
```

Array conversion enables seamless interoperability with .NET collections  
and F# types. Conversion functions transform between arrays, lists,  
sequences, and generic collections. These operations facilitate data  
exchange between functional F# code and object-oriented .NET libraries  
while preserving type safety.

## Array Performance and Mutation

Demonstrating efficient array operations and mutation patterns for  
performance-critical code.  

```f#
open System.Diagnostics

let performanceTest size =
    let stopwatch = Stopwatch.StartNew()
    
    // Create large array
    let largeArray = Array.init size (fun i -> i * 2)
    stopwatch.Stop()
    printfn "Array creation (%d elements): %d ms" size stopwatch.ElapsedMilliseconds
    
    stopwatch.Restart()
    // In-place modification
    for i in 0 .. largeArray.Length - 1 do
        largeArray[i] <- largeArray[i] + 1
    stopwatch.Stop()
    printfn "In-place modification: %d ms" stopwatch.ElapsedMilliseconds
    
    stopwatch.Restart()
    // Functional transformation
    let transformed = largeArray |> Array.map (fun x -> x + 1)
    stopwatch.Stop()
    printfn "Functional transformation: %d ms" stopwatch.ElapsedMilliseconds

// Demonstrate mutation patterns
let mutableOperations() =
    let data = Array.init 10 (fun i -> i)
    printfn "Original: %A" data
    
    // Selective mutation
    for i in 0..2..data.Length-1 do
        data[i] <- data[i] * 10
    
    printfn "Even indices multiplied by 10: %A" data

performanceTest 100000
mutableOperations()
```

Array performance characteristics make them suitable for computational  
tasks requiring frequent element access or modification. In-place  
mutations provide optimal performance for algorithms requiring iterative  
updates. Functional transformations maintain immutability principles  
while creating new arrays. Choose mutation patterns based on performance  
requirements and architectural constraints.

## Array Validation and Error Handling

Implementing safe array operations with bounds checking and error  
handling patterns.  

```f#
let safeArrayAccess arr index =
    if index >= 0 && index < Array.length arr then
        Some arr[index]
    else
        None

let safeBounds() =
    let numbers = [|10; 20; 30; 40; 50|]
    
    // Safe access
    match safeArrayAccess numbers 2 with
    | Some value -> printfn "Value at index 2: %d" value
    | None -> printfn "Index 2 is out of bounds"
    
    match safeArrayAccess numbers 10 with
    | Some value -> printfn "Value at index 10: %d" value
    | None -> printfn "Index 10 is out of bounds"

// Try operations for safe array manipulation
let tryOperations() =
    let findSafe predicate arr =
        try
            arr |> Array.find predicate |> Some
        with
        | :? System.Collections.Generic.KeyNotFoundException -> None
    
    let numbers = [|2; 4; 6; 8|]
    
    let found1 = findSafe (fun x -> x > 5) numbers
    let found2 = findSafe (fun x -> x > 10) numbers
    
    printfn "Found > 5: %A" found1
    printfn "Found > 10: %A" found2

// Array validation
let validateArray arr =
    if Array.isEmpty arr then
        Error "Array cannot be empty"
    elif Array.exists (fun x -> x < 0) arr then
        Error "Array contains negative numbers"
    else
        Ok arr

let validation() =
    let testArrays = [
        [||]
        [|1; 2; -3; 4|]
        [|1; 2; 3; 4|]
    ]
    
    testArrays |> List.iter (fun arr ->
        match validateArray arr with
        | Ok validArr -> printfn "Valid array: %A" validArr
        | Error message -> printfn "Invalid: %s" message
    )

safeBounds()
tryOperations()
validation()
```

Safe array operations prevent runtime exceptions through validation and  
bounds checking. Option types handle missing values gracefully. Result  
types encode validation success and failure states. These patterns  
ensure robust array manipulation in production code while maintaining  
F#'s type safety guarantees.

## Array Pattern Matching

Using pattern matching with arrays for structured data processing  
and control flow.  

```f#
let processArray arr =
    match arr with
    | [||] -> "Empty array"
    | [|single|] -> sprintf "Single element: %d" single
    | [|first; second|] -> sprintf "Two elements: %d, %d" first second
    | [|first; second; third|] -> sprintf "Three elements: %d, %d, %d" first second third
    | _ -> sprintf "Array with %d elements, first: %d" arr.Length arr[0]

// Pattern matching examples
let testArrays = [
    [||]
    [|42|]
    [|1; 2|]
    [|1; 2; 3|]
    [|1; 2; 3; 4; 5|]
]

testArrays |> List.iter (fun arr ->
    let result = processArray arr
    printfn "%s" result
)

// Advanced pattern matching with guards
let categorizeArray arr =
    match arr with
    | [||] -> "Empty"
    | [|x|] when x > 0 -> "Single positive"
    | [|x|] when x < 0 -> "Single negative"
    | [|x|] -> "Single zero"
    | arr when Array.length arr > 10 -> "Large array"
    | arr when Array.forall (fun x -> x > 0) arr -> "All positive"
    | arr when Array.exists (fun x -> x < 0) arr -> "Contains negative"
    | _ -> "Mixed small array"

let moreTests = [
    [|5|]
    [|-3|]
    [|0|]
    [|1; 2; 3; 4; 5|]
    [|1; -2; 3|]
    Array.init 15 id
]

moreTests |> List.iter (fun arr ->
    let category = categorizeArray arr
    printfn "Array %A -> %s" arr category
)
```

Pattern matching with arrays enables elegant control flow based on  
array structure and contents. Guards add conditional logic to patterns.  
These patterns support declarative programming styles while maintaining  
efficiency. Pattern matching complements functional programming  
approaches to array processing.

## Array Algorithms and Utilities

Implementing common algorithms and utility functions using arrays  
for computational tasks.  

```f#
// Binary search implementation
let binarySearch (arr: int[]) target =
    let rec search left right =
        if left > right then -1
        else
            let mid = (left + right) / 2
            if arr[mid] = target then mid
            elif arr[mid] < target then search (mid + 1) right
            else search left (mid - 1)
    search 0 (arr.Length - 1)

// Array reversal in place
let reverseInPlace arr =
    let n = Array.length arr
    for i in 0 .. (n/2 - 1) do
        let temp = arr[i]
        arr[i] <- arr[n - 1 - i]
        arr[n - 1 - i] <- temp

// Rotate array elements
let rotateLeft arr positions =
    let n = Array.length arr
    let pos = positions % n
    let result = Array.zeroCreate n
    for i in 0 .. n - 1 do
        result[(i + n - pos) % n] <- arr[i]
    result

// Array algorithms demonstration
let algorithms() =
    // Binary search
    let sortedArray = [|1; 3; 5; 7; 9; 11; 13; 15|]
    let searchResult = binarySearch sortedArray 7
    printfn "Binary search for 7 in %A: index %d" sortedArray searchResult
    
    // Reverse
    let toReverse = [|1; 2; 3; 4; 5|]
    let original = Array.copy toReverse
    reverseInPlace toReverse
    printfn "Reversed %A -> %A" original toReverse
    
    // Rotate
    let toRotate = [|1; 2; 3; 4; 5; 6|]
    let rotated = rotateLeft toRotate 2
    printfn "Rotated left by 2: %A -> %A" toRotate rotated

// Sliding window operations
let slidingWindow size arr =
    if size > Array.length arr then [||]
    else
        Array.init (Array.length arr - size + 1) (fun i ->
            Array.sub arr i size
        )

let windowDemo() =
    let data = [|1; 2; 3; 4; 5; 6; 7; 8|]
    let windows = slidingWindow 3 data
    printfn "Sliding windows of size 3:"
    windows |> Array.iter (fun window -> printfn "  %A" window)

algorithms()
windowDemo()
```

Array algorithms demonstrate computational problem-solving with efficient  
data structures. Binary search provides O(log n) lookups in sorted arrays.  
In-place operations minimize memory allocation. Sliding window operations  
support time-series analysis and pattern recognition. These algorithms  
showcase arrays' utility in performance-critical scenarios.

## Array Functional Programming Patterns

Advanced functional programming techniques using arrays with higher-order  
functions and composition.  

```f#
// Function composition with arrays
let pipeline data =
    data
    |> Array.filter (fun x -> x > 0)
    |> Array.map (fun x -> x * 2)
    |> Array.filter (fun x -> x < 20)
    |> Array.sort

// Curried functions for array processing
let multiplyBy factor = Array.map (fun x -> x * factor)
let filterGreaterThan threshold = Array.filter (fun x -> x > threshold)
let takeFront count = Array.take count

// Composition demonstration
let composedOperation =
    multiplyBy 3
    >> filterGreaterThan 10
    >> takeFront 5

let functionalDemo() =
    let input = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
    
    let pipelineResult = pipeline input
    printfn "Pipeline result: %A" pipelineResult
    
    let composedResult = composedOperation input
    printfn "Composed result: %A" composedResult

// Monadic-style operations
let bind f arr = Array.collect f arr

let arrayComputation() =
    let result =
        [|1; 2; 3|]
        |> bind (fun x -> [|x; x * 2|])
        |> bind (fun x -> if x % 2 = 0 then [|x|] else [||])
    
    printfn "Monadic computation result: %A" result

// Point-free style programming
let processNumbers =
    Array.filter ((<) 0)
    >> Array.map ((*) 2)
    >> Array.sort
    >> Array.take 3

let pointFreeDemo() =
    let numbers = [|5; -2; 8; -1; 3; 7; -4|]
    let result = processNumbers numbers
    printfn "Point-free processing: %A -> %A" numbers result

functionalDemo()
arrayComputation()
pointFreeDemo()
```

Functional programming with arrays leverages composition, currying, and  
higher-order functions for elegant data processing. Function composition  
creates reusable processing pipelines. Point-free style eliminates  
explicit parameters for concise expressions. These patterns demonstrate  
F#'s functional programming capabilities applied to array manipulation.

## Array Memory Layout and Performance

Understanding array memory characteristics and optimization techniques  
for high-performance computing.  

```f#
open System
open System.Diagnostics

// Memory layout demonstration
let memoryDemo() =
    let intArray = [|1; 2; 3; 4; 5|]
    let objArray = [|box 1; box 2; box 3; box 4; box 5|]
    
    printfn "Int array size: %d bytes per element" sizeof<int>
    printfn "Object array: reference types with indirection"
    
    // Value types vs reference types in arrays
    let structArray = [|DateTime.Now; DateTime.Now.AddDays(1.0)|]
    let stringArray = [|"hello"; "world"|]
    
    printfn "Struct array: stored inline"
    printfn "String array: references to heap objects"

// Cache locality demonstration
let cacheLocalityTest size =
    let matrix = Array2D.zeroCreate<int> size size
    let stopwatch = Stopwatch.StartNew()
    
    // Row-major access (cache-friendly)
    for i in 0 .. size - 1 do
        for j in 0 .. size - 1 do
            matrix[i, j] <- i + j
    let rowMajorTime = stopwatch.ElapsedMilliseconds
    
    stopwatch.Restart()
    
    // Column-major access (cache-unfriendly)
    for j in 0 .. size - 1 do
        for i in 0 .. size - 1 do
            matrix[i, j] <- i + j
    let columnMajorTime = stopwatch.ElapsedMilliseconds
    
    printfn "Matrix size: %dx%d" size size
    printfn "Row-major access: %d ms" rowMajorTime
    printfn "Column-major access: %d ms" columnMajorTime

// Array pooling for memory efficiency
let arrayPoolDemo() =
    let pool = System.Buffers.ArrayPool<int>.Shared
    
    let rentedArray = pool.Rent(1000)
    printfn "Rented array length: %d" rentedArray.Length
    
    // Use the array
    for i in 0 .. 999 do
        rentedArray[i] <- i * i
    
    // Return to pool
    pool.Return(rentedArray)
    printfn "Array returned to pool"

memoryDemo()
cacheLocalityTest 1000
arrayPoolDemo()
```

Array memory layout affects performance in computational scenarios.  
Value types store data inline for better cache locality. Reference  
types introduce indirection overhead. Sequential access patterns  
leverage CPU cache effectively. Array pooling reduces garbage  
collection pressure in high-throughput applications.

## Array Interop with .NET Libraries

Seamless integration between F# arrays and .NET Framework libraries  
for comprehensive functionality.  

```f#
open System
open System.Linq
open System.Collections.Generic

// LINQ integration
let linqDemo() =
    let numbers = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
    
    // LINQ extension methods
    let evenSum = numbers.Where(fun x -> x % 2 = 0).Sum()
    let squaredFirst3 = numbers.Take(3).Select(fun x -> x * x).ToArray()
    
    printfn "Sum of evens: %d" evenSum
    printfn "First 3 squared: %A" squaredFirst3

// Generic collections interop
let genericCollectionsDemo() =
    let fsharpArray = [|"apple"; "banana"; "cherry"|]
    
    // Convert to generic list
    let genericList = List<string>(fsharpArray)
    genericList.Add("date")
    genericList.Sort()
    
    printfn "Generic list after operations: %A" (genericList.ToArray())
    
    // Dictionary usage
    let dict = Dictionary<int, string>()
    fsharpArray |> Array.iteri (fun i s -> dict[i] <- s)
    
    printfn "Dictionary contents:"
    for kvp in dict do
        printfn "  %d: %s" kvp.Key kvp.Value

// System.Array static methods
let systemArrayDemo() =
    let source = [|3; 1; 4; 1; 5; 9; 2; 6|]
    let target = Array.zeroCreate<int> source.Length
    
    // Array.Copy
    Array.Copy(source, target, source.Length)
    printfn "Copied array: %A" target
    
    // Array.Sort with custom comparer
    let customSorted = Array.copy source
    Array.Sort(customSorted, fun x y -> compare (x % 3) (y % 3))
    printfn "Custom sorted (by mod 3): %A" customSorted
    
    // Array.Reverse
    let reversed = Array.copy source
    Array.Reverse(reversed)
    printfn "Reversed: %A" reversed

linqDemo()
genericCollectionsDemo()
systemArrayDemo()
```

F# arrays integrate seamlessly with .NET libraries and frameworks.  
LINQ operations provide additional query capabilities. Generic  
collections offer mutable alternatives for specific scenarios.  
System.Array methods provide low-level array manipulation. This  
interoperability enables leveraging the entire .NET ecosystem.

## Advanced Array Operations

Complex array manipulation techniques for sophisticated data processing  
and algorithmic implementations.  

```f#
// Array transpose for matrix operations
let transpose matrix =
    match matrix with
    | [||] -> [||]
    | _ when Array.isEmpty matrix[0] -> [||]
    | _ ->
        let rows = matrix.Length
        let cols = matrix[0].Length
        Array.init cols (fun j ->
            Array.init rows (fun i -> matrix[i][j])
        )

// Zip multiple arrays
let zip3 arr1 arr2 arr3 =
    let minLength = [arr1.Length; arr2.Length; arr3.Length] |> List.min
    Array.init minLength (fun i -> arr1[i], arr2[i], arr3[i])

// Array unfold operation
let unfoldArray generator seed maxLength =
    let result = ResizeArray<_>()
    let mutable current = seed
    
    for _ in 1..maxLength do
        match generator current with
        | Some(value, nextSeed) ->
            result.Add(value)
            current <- nextSeed
        | None -> ()
    
    result.ToArray()

// Advanced operations demonstration
let advancedDemo() =
    // Matrix transpose
    let matrix = [|
        [|1; 2; 3|]
        [|4; 5; 6|]
        [|7; 8; 9|]
    |]
    
    let transposed = transpose matrix
    printfn "Original matrix:"
    matrix |> Array.iter (fun row -> printfn "  %A" row)
    printfn "Transposed:"
    transposed |> Array.iter (fun row -> printfn "  %A" row)
    
    // Zip three arrays
    let names = [|"Alice"; "Bob"; "Charlie"|]
    let ages = [|25; 30; 35|]
    let cities = [|"NYC"; "LA"; "Chicago"|]
    
    let combined = zip3 names ages cities
    printfn "Combined data: %A" combined
    
    // Unfold example (Fibonacci sequence)
    let fibonacci (a, b) =
        if a > 1000 then None
        else Some(a, (b, a + b))
    
    let fibSequence = unfoldArray fibonacci (1, 1) 10
    printfn "Fibonacci sequence: %A" fibSequence

// Moving average calculation
let movingAverage windowSize data =
    if windowSize <= 0 || windowSize > Array.length data then [||]
    else
        Array.init (Array.length data - windowSize + 1) (fun i ->
            let window = data[i..i + windowSize - 1]
            Array.average (Array.map float window)
        )

let analysisDemo() =
    let prices = [|100.0; 102.0; 98.0; 105.0; 103.0; 107.0; 109.0; 106.0|]
    let ma3 = movingAverage 3 prices
    
    printfn "Stock prices: %A" prices
    printfn "3-period moving average: %A" ma3

advancedDemo()
analysisDemo()
```

Advanced array operations enable sophisticated data processing and  
algorithmic implementations. Matrix operations support mathematical  
computations. Unfold operations generate sequences from seed values.  
Moving averages demonstrate time-series analysis techniques. These  
operations showcase arrays' capability for complex computational tasks.

## Array Parallel Processing

Leveraging parallel processing capabilities for computationally intensive  
array operations.  

```f#
open System

let parallelDemo() =
    let largeArray = Array.init 1000000 (fun i -> i + 1)
    
    // Parallel map operation
    let parallelSquared = 
        largeArray 
        |> Array.Parallel.map (fun x -> x * x)
    
    printfn "First 10 parallel squared: %A" parallelSquared[0..9]
    
    // Parallel partition
    let parallelEvens, parallelOdds = 
        largeArray[0..999]
        |> Array.Parallel.partition (fun x -> x % 2 = 0)
    
    printfn "Parallel evens count: %d" parallelEvens.Length
    printfn "Parallel odds count: %d" parallelOdds.Length

parallelDemo()
```

Array.Parallel operations utilize multiple CPU cores for data-intensive  
computations. Parallel processing significantly improves performance for  
large arrays and computationally expensive operations. These operations  
maintain functional programming principles while leveraging hardware  
parallelism for optimal performance.

## Array Slicing and SubArrays

Advanced techniques for working with array segments and views without  
copying data.  

```f#
let slicingDemo() =
    let sourceArray = Array.init 20 (fun i -> i * i)
    printfn "Source array: %A" sourceArray
    
    // Basic slicing
    let slice1 = sourceArray[5..10]
    let slice2 = sourceArray[..7]
    let slice3 = sourceArray[15..]
    
    printfn "Slice [5..10]: %A" slice1
    printfn "Slice [..7]: %A" slice2
    printfn "Slice [15..]: %A" slice3
    
    // Array.sub for programmatic slicing
    let subArray = Array.sub sourceArray 3 5
    printfn "Sub-array from index 3, length 5: %A" subArray
    
    // Skip and take combinations
    let processed = 
        sourceArray 
        |> Array.skip 5 
        |> Array.take 8
    
    printfn "Skip 5, take 8: %A" processed

// Windowed operations
let windowedOperations() =
    let timeSeries = [|1.0; 1.5; 2.1; 1.8; 2.3; 2.9; 2.1; 1.7; 2.5|]
    
    // Create sliding windows
    let windows = Array.windowed 3 timeSeries
    printfn "3-element windows:"
    windows |> Array.iteri (fun i w -> printfn "  Window %d: %A" i w)
    
    // Calculate windowed statistics
    let windowedAverages = 
        windows |> Array.map (Array.average)
    printfn "Windowed averages: %A" windowedAverages

slicingDemo()
windowedOperations()
```

Array slicing creates views or copies of array segments efficiently.  
Windowed operations generate overlapping subsequences for time-series  
analysis and signal processing. These operations support streaming  
data analysis and memory-efficient processing of large datasets.

## Array Builders and Custom Creation

Implementing custom array builders and creation patterns for specialized  
use cases.  

```f#
// Custom array builder
type ArrayBuilder() =
    let mutable items = ResizeArray<_>()
    
    member _.Add(item) = items.Add(item); item
    member _.AddRange(items) = items.AddRange(items)
    member _.Count = items.Count
    member _.ToArray() = items.ToArray()
    
    member _.Clear() = items.Clear()

// Array builder usage
let customBuilderDemo() =
    let builder = ArrayBuilder()
    
    builder.Add(1) |> ignore
    builder.Add(2) |> ignore
    builder.Add(3) |> ignore
    
    let result = builder.ToArray()
    printfn "Built array: %A" result

// Conditional array building
let buildConditionally predicate source =
    let builder = ResizeArray<_>()
    
    for item in source do
        if predicate item then
            builder.Add(item)
            builder.Add(item * 2)  // Add transformed version too
    
    builder.ToArray()

let conditionalDemo() =
    let source = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
    let result = buildConditionally (fun x -> x % 3 = 0) source
    printfn "Conditional build (multiples of 3): %A" result

// Array factory functions
let createArithmeticSequence start step count =
    Array.init count (fun i -> start + i * step)

let createGeometricSequence start ratio count =
    Array.init count (fun i -> start * (pown ratio i))

let sequenceDemo() =
    let arithmetic = createArithmeticSequence 10 3 8
    let geometric = createGeometricSequence 2 3 6
    
    printfn "Arithmetic sequence: %A" arithmetic
    printfn "Geometric sequence: %A" geometric

customBuilderDemo()
conditionalDemo()
sequenceDemo()
```

Custom array builders provide flexible patterns for incremental array  
construction. Conditional building enables complex array creation logic.  
Factory functions encapsulate common array generation patterns. These  
techniques support domain-specific array creation requirements.

## Array Type Annotations and Generics

Working with strongly-typed arrays and generic array operations for  
type-safe programming.  

```f#
// Generic array functions
let processGenericArray<'T when 'T : comparison> (arr: 'T[]) =
    let sorted = Array.sort arr
    let first = Array.tryHead sorted
    let last = Array.tryLast sorted
    
    (sorted, first, last)

let genericDemo() =
    // Integer arrays
    let intArray = [|3; 1; 4; 1; 5|]
    let intResult = processGenericArray intArray
    printfn "Int result: %A" intResult
    
    // String arrays
    let stringArray = [|"zebra"; "apple"; "banana"|]
    let stringResult = processGenericArray stringArray
    printfn "String result: %A" stringResult

// Type-constrained array operations
let numericOperations<'T when 'T : (static member (+) : 'T -> 'T -> 'T) 
                              and 'T : (static member Zero : 'T)
                              and 'T : comparison> (arr: 'T[]) =
    let sum = Array.fold (+) LanguagePrimitives.GenericZero arr
    let max = Array.max arr
    let min = Array.min arr
    
    (sum, min, max)

let numericDemo() =
    let intArray = [|1; 2; 3; 4; 5|]
    let floatArray = [|1.1; 2.2; 3.3; 4.4; 5.5|]
    
    let intStats = numericOperations intArray
    let floatStats = numericOperations floatArray
    
    printfn "Int stats (sum, min, max): %A" intStats
    printfn "Float stats (sum, min, max): %A" floatStats

// Array of tuples and records
type Student = { Name: string; Grade: float }

let structuredArrayDemo() =
    let students = [|
        { Name = "Alice"; Grade = 85.5 }
        { Name = "Bob"; Grade = 92.0 }
        { Name = "Charlie"; Grade = 78.3 }
    |]
    
    let averageGrade = students |> Array.averageBy (fun s -> s.Grade)
    let topStudent = students |> Array.maxBy (fun s -> s.Grade)
    
    printfn "Average grade: %.1f" averageGrade
    printfn "Top student: %s (%.1f)" topStudent.Name topStudent.Grade

genericDemo()
numericDemo()
structuredArrayDemo()
```

Generic array operations enable type-safe programming with reusable  
functions. Type constraints ensure operations are valid for specific  
types. Structured arrays with records and tuples support complex data  
modeling. These patterns demonstrate F#'s strong type system applied  
to array programming.

## Array I/O and Serialization

Reading, writing, and serializing arrays for data persistence and  
interoperability.  

```f#
open System.IO
open System.Text.Json

// File I/O operations
let fileIODemo() =
    let numbers = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
    let tempFile = Path.GetTempFileName()
    
    try
        // Write array to file
        let content = numbers |> Array.map string |> String.concat ","
        File.WriteAllText(tempFile, content)
        printfn "Wrote array to file: %s" tempFile
        
        // Read array from file
        let readContent = File.ReadAllText(tempFile)
        let readNumbers = 
            readContent.Split(',') 
            |> Array.map int
        
        printfn "Read from file: %A" readNumbers
        printfn "Arrays equal: %b" (numbers = readNumbers)
    
    finally
        if File.Exists(tempFile) then File.Delete(tempFile)

// JSON serialization
let jsonDemo() =
    let data = [|
        ("Alice", 25)
        ("Bob", 30)
        ("Charlie", 35)
    |]
    
    // Serialize to JSON
    let json = JsonSerializer.Serialize(data)
    printfn "JSON: %s" json
    
    // Deserialize from JSON
    let deserialized = JsonSerializer.Deserialize<(string * int)[]>(json)
    printfn "Deserialized: %A" deserialized

// Binary serialization with BinaryWriter
let binaryDemo() =
    let floatArray = [|1.1; 2.2; 3.3; 4.4; 5.5|]
    
    use memoryStream = new MemoryStream()
    use writer = new BinaryWriter(memoryStream)
    
    // Write array length and elements
    writer.Write(floatArray.Length)
    floatArray |> Array.iter writer.Write
    
    // Read back
    memoryStream.Position <- 0L
    use reader = new BinaryReader(memoryStream)
    
    let length = reader.ReadInt32()
    let readArray = Array.init length (fun _ -> reader.ReadDouble())
    
    printfn "Original: %A" floatArray
    printfn "Read back: %A" readArray

fileIODemo()
jsonDemo()
binaryDemo()
```

Array I/O operations enable data persistence and exchange with external  
systems. Text-based formats provide human-readable storage. JSON  
serialization offers structured data exchange. Binary formats provide  
compact, efficient storage. These patterns support various data  
persistence requirements.

## Array Performance Profiling

Measuring and optimizing array operation performance for production  
applications.  

```f#
open System
open System.Diagnostics

// Performance measurement utilities
let timeOperation name operation =
    let stopwatch = Stopwatch.StartNew()
    let result = operation()
    stopwatch.Stop()
    printfn "%s took %d ms" name stopwatch.ElapsedMilliseconds
    result

let performanceComparisons() =
    let size = 1000000
    let data = Array.init size (fun i -> i)
    
    // Compare different iteration patterns
    timeOperation "For loop" (fun () ->
        let mutable sum = 0
        for i in 0 .. data.Length - 1 do
            sum <- sum + data[i]
        sum
    ) |> ignore
    
    timeOperation "Array.fold" (fun () ->
        Array.fold (+) 0 data
    ) |> ignore
    
    timeOperation "Array.sum" (fun () ->
        Array.sum data
    ) |> ignore
    
    // Compare filtering approaches
    let filterData = Array.init size (fun i -> i)
    
    timeOperation "Manual filter" (fun () ->
        let result = ResizeArray<_>()
        for item in filterData do
            if item % 2 = 0 then result.Add(item)
        result.ToArray()
    ) |> ignore
    
    timeOperation "Array.filter" (fun () ->
        Array.filter (fun x -> x % 2 = 0) filterData
    ) |> ignore

// Memory allocation profiling
let memoryProfiling() =
    let initialMemory = GC.GetTotalMemory(false)
    
    // Create large arrays and measure allocation
    let arrays = Array.init 100 (fun i -> Array.create 10000 i)
    
    let afterAllocation = GC.GetTotalMemory(false)
    printfn "Memory allocated: %d bytes" (afterAllocation - initialMemory)
    
    // Force garbage collection
    GC.Collect()
    GC.WaitForPendingFinalizers()
    
    let afterGC = GC.GetTotalMemory(true)
    printfn "Memory after GC: %d bytes increase" (afterGC - initialMemory)

performanceComparisons()
memoryProfiling()
```

Performance profiling identifies bottlenecks in array operations.  
Timing comparisons reveal efficiency differences between approaches.  
Memory profiling tracks allocation patterns and garbage collection  
impact. These techniques enable optimization of performance-critical  
array processing code.

## Array Testing and Validation

Implementing comprehensive testing strategies for array-based functions  
and algorithms.  

```f#
// Property-based testing concepts
let testArrayProperty name property testCases =
    printfn "Testing property: %s" name
    let results = 
        testCases 
        |> Array.map (fun case ->
            try
                let result = property case
                (case, Ok result)
            with
            | ex -> (case, Error ex.Message)
        )
    
    let (passed, failed) = 
        results |> Array.partition (fun (_, result) -> 
            match result with Ok _ -> true | Error _ -> false)
    
    printfn "  Passed: %d, Failed: %d" passed.Length failed.Length
    
    if failed.Length > 0 then
        printfn "  Failures:"
        failed |> Array.iter (fun (case, error) ->
            match error with
            | Error msg -> printfn "    %A: %s" case msg
            | _ -> ())

// Array invariant testing
let arrayInvariantTests() =
    // Test: Array.length after operations
    let lengthInvariant arr =
        let doubled = Array.map ((*) 2) arr
        doubled.Length = arr.Length
    
    let lengthTestCases = [|
        [||]
        [|1|]
        [|1; 2; 3|]
        Array.init 100 id
    |]
    
    testArrayProperty "Length invariant after map" lengthInvariant lengthTestCases
    
    // Test: Sort correctness
    let sortCorrectness arr =
        let sorted = Array.sort arr
        let pairs = Array.pairwise sorted
        Array.forall (fun (a, b) -> a <= b) pairs
    
    let sortTestCases = [|
        [|3; 1; 4; 1; 5; 9; 2; 6|]
        [|5; 4; 3; 2; 1|]
        [|1|]
        [||]
        Array.init 50 (fun _ -> Random().Next(100))
    |]
    
    testArrayProperty "Sort produces ordered array" sortCorrectness sortTestCases

// Edge case testing
let edgeCaseTests() =
    printfn "Edge case testing:"
    
    // Empty array operations
    let emptyArray: int[] = [||]
    
    try
        let _ = Array.head emptyArray
        printfn "ERROR: Array.head on empty array should throw"
    with
    | :? System.ArgumentException -> printfn "✓ Array.head correctly throws on empty array"
    
    // Boundary conditions
    let singleElement = [|42|]
    let headResult = Array.head singleElement
    let tailResult = Array.tail singleElement
    
    printfn "✓ Single element head: %d" headResult
    printfn "✓ Single element tail: %A" tailResult
    
    // Large array handling
    let largeArray = Array.create 1000000 1
    let sumResult = Array.sum largeArray
    printfn "✓ Large array sum: %d" sumResult

arrayInvariantTests()
edgeCaseTests()
```

Comprehensive array testing ensures correctness and robustness.  
Property-based testing verifies invariants across multiple inputs.  
Edge case testing validates boundary conditions and error handling.  
These testing strategies provide confidence in array-based  
implementations and prevent regression bugs.

## Array Best Practices

Guidelines and patterns for effective array usage in F# applications  
following functional programming principles.  

```f#
// Immutable array patterns
let immutablePatterns() =
    printfn "=== Immutable Array Patterns ==="
    
    // Prefer creating new arrays over mutation
    let original = [|1; 2; 3; 4; 5|]
    
    // Good: functional transformation
    let doubled = original |> Array.map (fun x -> x * 2)
    printfn "Original preserved: %A" original
    printfn "New array created: %A" doubled
    
    // Avoid: in-place mutation (unless performance critical)
    let mutableCopy = Array.copy original
    for i in 0 .. mutableCopy.Length - 1 do
        mutableCopy[i] <- mutableCopy[i] * 2
    printfn "Mutated copy: %A" mutableCopy

// Composition patterns
let compositionPatterns() =
    printfn "\n=== Composition Patterns ==="
    
    let data = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
    
    // Good: function composition
    let processData =
        Array.filter (fun x -> x % 2 = 0)
        >> Array.map (fun x -> x * x)
        >> Array.take 3
    
    let result = processData data
    printfn "Composed operation result: %A" result
    
    // Pipeline style for readability
    let pipelineResult =
        data
        |> Array.filter (fun x -> x % 2 = 0)
        |> Array.map (fun x -> x * x)
        |> Array.take 3
    
    printfn "Pipeline result: %A" pipelineResult

// Error handling patterns
let errorHandlingPatterns() =
    printfn "\n=== Error Handling Patterns ==="
    
    let safeArrayOperation arr index =
        if index >= 0 && index < Array.length arr then
            Some arr[index]
        else
            None
    
    let numbers = [|10; 20; 30|]
    
    // Safe operations with Option types
    match safeArrayOperation numbers 1 with
    | Some value -> printfn "Safe access succeeded: %d" value
    | None -> printfn "Safe access failed"
    
    // Use try* variants for safe operations
    let safeFindResult = numbers |> Array.tryFind (fun x -> x > 15)
    printfn "Safe find result: %A" safeFindResult

// Performance considerations
let performanceConsiderations() =
    printfn "\n=== Performance Considerations ==="
    
    // Choose appropriate collection type
    printfn "Arrays: O(1) access, good for frequent indexing"
    printfn "Lists: O(1) prepend, good for sequential processing"
    printfn "Sequences: Lazy evaluation, good for large or infinite data"
    
    // Parallel operations for CPU-intensive tasks
    let largeData = Array.init 100000 id
    
    let sequentialSum = largeData |> Array.sum
    let parallelSum = largeData |> Array.Parallel.map (fun x -> x) |> Array.sum
    
    printfn "Sequential and parallel sums equal: %b" (sequentialSum = parallelSum)

// Documentation and naming
let documentationPatterns() =
    printfn "\n=== Documentation Patterns ==="
    
    /// Calculates the moving average over a specified window size
    /// Returns empty array if windowSize > array length
    let movingAverage windowSize (data: float[]) =
        if windowSize <= 0 || windowSize > data.Length then [||]
        else
            Array.init (data.Length - windowSize + 1) (fun i ->
                let window = data[i..i + windowSize - 1]
                Array.average window
            )
    
    let testData = [|1.0; 2.0; 3.0; 4.0; 5.0|]
    let ma3 = movingAverage 3 testData
    printfn "Moving average example: %A" ma3

immutablePatterns()
compositionPatterns()
errorHandlingPatterns()
performanceConsiderations()
documentationPatterns()
```

Array best practices emphasize immutability, composition, and type  
safety. Functional transformations preserve data integrity. Safe  
operations prevent runtime errors. Performance considerations guide  
collection choice. Documentation ensures maintainable code. Following  
these patterns results in robust, efficient F# applications.

## Array Extensions and Custom Operations

Creating custom array operations and extensions to enhance functionality  
for domain-specific requirements.  

```f#
// Array extension methods
module ArrayExtensions =
    let median (arr: 'T[]) =
        let sorted = Array.sort arr
        let len = sorted.Length
        if len = 0 then failwith "Cannot find median of empty array"
        elif len % 2 = 1 then
            sorted[len / 2]
        else
            // For simplicity, returning the lower middle element
            sorted[len / 2 - 1]
    
    let mode (arr: 'T[]) =
        arr
        |> Array.groupBy id
        |> Array.maxBy (snd >> Array.length)
        |> fst
    
    let chunk (size: int) (arr: 'T[]) =
        if size <= 0 then failwith "Chunk size must be positive"
        Array.chunkBySize size arr

// Custom array operations
let customOperations() =
    let numbers = [|1; 2; 2; 3; 3; 3; 4; 5|]
    
    let median = ArrayExtensions.median numbers
    let mode = ArrayExtensions.mode numbers
    let chunks = ArrayExtensions.chunk 3 numbers
    
    printfn "Numbers: %A" numbers
    printfn "Median: %A" median
    printfn "Mode: %A" mode
    printfn "Chunks of 3: %A" chunks

// Domain-specific array operations
module Statistics =
    let standardDeviation (data: float[]) =
        if data.Length = 0 then 0.0
        else
            let mean = Array.average data
            let sumSquaredDiffs = 
                data 
                |> Array.sumBy (fun x -> (x - mean) ** 2.0)
            sqrt (sumSquaredDiffs / float data.Length)
    
    let percentile (p: float) (data: float[]) =
        if p < 0.0 || p > 100.0 then failwith "Percentile must be between 0 and 100"
        let sorted = Array.sort data
        let index = (p / 100.0) * float (sorted.Length - 1)
        let lower = int (floor index)
        let upper = int (ceil index)
        
        if lower = upper then sorted[lower]
        else
            let weight = index - float lower
            sorted[lower] * (1.0 - weight) + sorted[upper] * weight

let statisticsDemo() =
    let data = [|1.0; 2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0|]
    
    let stdDev = Statistics.standardDeviation data
    let p25 = Statistics.percentile 25.0 data
    let p75 = Statistics.percentile 75.0 data
    
    printfn "Data: %A" data
    printfn "Standard deviation: %.2f" stdDev
    printfn "25th percentile: %.2f" p25
    printfn "75th percentile: %.2f" p75

// Functional array combinators
let combinators() =
    // Applicative-style operations
    let applyArray (funcs: ('a -> 'b)[]) (values: 'a[]) =
        [|
            for f in funcs do
                for v in values do
                    yield f v
        |]
    
    let functions = [|((+) 1); ((*) 2); (fun x -> x * x)|]
    let values = [|1; 2; 3|]
    
    let results = applyArray functions values
    printfn "Applicative results: %A" results
    
    // Monadic bind for arrays
    let bind (f: 'a -> 'b[]) (arr: 'a[]) =
        Array.collect f arr
    
    let expandNumbers x = [|x; x + 10; x + 20|]
    let expanded = bind expandNumbers [|1; 2|]
    printfn "Monadic expansion: %A" expanded

customOperations()
statisticsDemo()
combinators()
```

Custom array extensions provide domain-specific functionality beyond  
standard library operations. Extension methods integrate seamlessly  
with existing array syntax. Statistical operations demonstrate  
mathematical array processing. Functional combinators enable advanced  
composition patterns. These extensions showcase F#'s extensibility  
and expressiveness for specialized array operations.
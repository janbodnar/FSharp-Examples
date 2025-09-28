# Maps

F# Maps are immutable key-value data structures that provide efficient  
lookup, insertion, and removal operations. Maps in F# are implemented as  
balanced binary trees, offering O(log n) performance for most operations.  
They are particularly useful for creating associations between keys and  
values, implementing lookup tables, caches, and maintaining ordered  
collections where keys need to be unique.  

Maps support any comparable type as keys (strings, numbers, tuples, etc.)  
and can store any type as values. They maintain keys in sorted order,  
making them ideal for scenarios where you need both fast lookups and  
ordered iteration. F# provides comprehensive map operations through the  
Map module, enabling functional programming patterns for data manipulation.  

## Basic Map Creation

Creating maps with literal syntax and basic operations for key-value pairs.  

```F#
open System

let words = Map [1, "book"; 2, "sky"; 3, "work"; 4, "cloud"]

printfn "%A" words

words |> Map.iter (fun k v -> Console.WriteLine($"{k}: {v}"))

for key in words.Keys do
    Console.WriteLine key

for value in words.Values do
    Console.WriteLine value
```

This example demonstrates basic map creation using the Map constructor  
with key-value pairs. The Map.iter function applies a function to each  
key-value pair, while the Keys and Values properties provide access to  
collections of keys and values respectively. Maps maintain insertion  
order and provide efficient lookups.  

## Map Filtering and Counting

Working with map filtering operations and accessing map properties.  

```F#
open System

let words =
    Map [ 1, "book"
          2, "sky"
          3, "work"
          4, "cloud"
          5, "water"
          6, "war" ]

for p in words do
    Console.WriteLine $"{p.Key}: {p.Value}"

Console.WriteLine "--------------------------------"

Console.WriteLine words.Count

Console.WriteLine words[1]
Console.WriteLine words[2]
Console.WriteLine words[3]

Console.WriteLine "--------------------------------"

words
|> Map.filter (fun _ v -> v.Contains "w")
|> Map.values
|> Seq.iter Console.WriteLine
```

This example shows how to access map elements by key using indexer syntax  
and demonstrates filtering operations. The Map.filter function creates a  
new map containing only elements that match the predicate. The Count  
property returns the number of key-value pairs, and Map.values extracts  
all values as a sequence for further processing.  

## Creating Empty Maps

Using Map.empty to create empty maps and adding elements incrementally.  

`Map.empty` creates an empty map.  

```F#
type User = {
    Name: string
    Occupation: string
}

let users =
   Map.empty.
      Add(1, {Name="John Doe"; Occupation="gardener"}).
      Add(2, {Name="Roger Roe"; Occupation="driver"}).
      Add(3, {Name="Lucy Smith"; Occupation="teacher"}).
      Add(4, {Name="Tom Jones"; Occupation="programmer"})

users |> Map.iter (fun k v -> printfn $"{k}: {v}")
```

The Map.empty function creates an empty immutable map, which can then be  
populated using the Add method. Each Add operation returns a new map with  
the additional key-value pair, following F#'s immutable data structure  
principles. Method chaining allows for fluent map construction.  

## Converting Lists to Maps

Creating maps from existing list data using Map.ofList function.  

```F#
let words = ["sky"; "cup"; "rock"; "pen"; "pearl"; "cloud"]
let n = words.Length
let idxs = [1..n]

let data = List.zip idxs words
printfn "%A" data

printfn "-----------------"

let m = data |> Map.ofList

m |> Map.iter (fun k v -> printfn $"{k}: {v}");
```

This example demonstrates converting a list of tuples into a map using  
Map.ofList. First, List.zip combines two lists into tuples, creating  
key-value pairs. Then Map.ofList constructs a map from these pairs.  
This pattern is useful for creating lookup tables from parallel lists  
or transforming data structures.  


## Working with Nested Maps

Creating and iterating over collections containing multiple maps.  

```F#
let fruits1 = Map [ "oranges", 2; "bananas", 3 ]
let fruits2 = Map [ "plums", 2; "kiwis", 3 ]

let fruits = [ Map[1, fruits1]; Map[2, fruits2] ]

fruits
|> List.iter (Map.iter (fun k v -> printfn $"{k} {v}"))

for nested in fruits do
  for e in nested do
        printfn $"{e.Key} {e.Value}"
```

This example shows how to work with collections of maps, demonstrating  
nested data structures. The first approach uses functional iteration  
with List.iter and Map.iter to process all key-value pairs. The second  
approach uses imperative for loops to achieve the same result, showing  
different iteration patterns available in F#.  

## Map.add and Map.remove

Adding and removing elements from maps using functional operations.  

```F#
let initial = Map ["a", 1; "b", 2; "c", 3]

let withNew = initial |> Map.add "d" 4
let withoutB = withNew |> Map.remove "b"

printfn "Original: %A" initial
printfn "With 'd': %A" withNew  
printfn "Without 'b': %A" withoutB

// Chaining operations
let result = 
    initial
    |> Map.add "d" 4
    |> Map.add "e" 5
    |> Map.remove "a"

printfn "Final result: %A" result
```

Map.add creates a new map with an additional key-value pair, while  
Map.remove creates a new map without the specified key. Both operations  
return new immutable maps, preserving the original. Operations can be  
chained using the pipe operator for fluent transformations.  

## Map.tryFind and Map.containsKey

Safe key lookup operations that avoid exceptions.  

```F#
let inventory = Map ["apples", 50; "bananas", 30; "oranges", 25]

let checkItem item =
    match Map.tryFind item inventory with
    | Some quantity -> printfn "%s: %d in stock" item quantity
    | None -> printfn "%s: not found" item

checkItem "apples"
checkItem "grapes"

printfn "Has bananas: %b" (Map.containsKey "bananas" inventory)
printfn "Has grapes: %b" (Map.containsKey "grapes" inventory)

// Using Option.defaultValue for safe access
let getQuantity item = 
    inventory |> Map.tryFind item |> Option.defaultValue 0

printfn "Apple quantity: %d" (getQuantity "apples")
printfn "Grape quantity: %d" (getQuantity "grapes")
```

Map.tryFind returns an Option type, providing safe access without  
exceptions when keys don't exist. Map.containsKey checks for key  
existence returning a boolean. These operations are preferred over  
direct indexing when key presence is uncertain, enabling robust  
error handling patterns.  

## Map Transformations with Map.map

Transforming map values while preserving keys and structure.  

```F#
let prices = Map ["coffee", 4.50; "tea", 3.25; "juice", 5.75]

let withTax = prices |> Map.map (fun _ price -> price * 1.15)
let rounded = withTax |> Map.map (fun _ price -> round price)

printfn "Original prices: %A" prices
printfn "With 15%% tax: %A" withTax
printfn "Rounded: %A" rounded

// Transform both keys and values
let uppercasePrices = 
    prices 
    |> Map.toList
    |> List.map (fun (k, v) -> (k.ToUpper(), v))
    |> Map.ofList

printfn "Uppercase keys: %A" uppercasePrices
```

Map.map applies a function to all values in the map, creating a new map  
with transformed values but identical keys and structure. The function  
receives both key and value, though the key parameter can be ignored  
with underscore. For key transformations, convert to list, transform,  
and reconstruct the map.  

## Map.choose for Selective Transformations

Filtering and transforming maps simultaneously using Map.choose.  

```F#
let students = Map [
    "Alice", 85
    "Bob", 72
    "Charlie", 91
    "Diana", 68
    "Eve", 94
]

let honors = students |> Map.choose (fun name grade ->
    if grade >= 85 then Some $"{name} (Grade A)"
    else None)

let gradeBounds = students |> Map.choose (fun name grade ->
    match grade with
    | g when g >= 90 -> Some "A"
    | g when g >= 80 -> Some "B"  
    | g when g >= 70 -> Some "C"
    | _ -> None)

printfn "All students: %A" students
printfn "Honor students: %A" honors
printfn "Passing grades: %A" gradeBounds
```

Map.choose combines filtering and transformation in a single operation.  
The function returns Some for elements to keep (with potential  
transformation) or None for elements to exclude. This is more efficient  
than separate filter and map operations and enables complex conditional  
transformations.  

## Map.fold for Aggregations

Computing aggregate values from map contents using fold operations.  

```F#
let sales = Map [
    "January", 15000
    "February", 18500
    "March", 22000
    "April", 19500
    "May", 21000
]

let totalSales = sales |> Map.fold (+) 0
let averageSales = totalSales / sales.Count
let maxSale = sales |> Map.fold (fun acc _ v -> max acc v) 0

printfn "Monthly sales: %A" sales
printfn "Total sales: %d" totalSales
printfn "Average sales: %d" averageSales  
printfn "Maximum month: %d" maxSale

// Building summary statistics
let summary = 
    sales |> Map.fold (fun (sum, count, max) _ value ->
        (sum + value, count + 1, max |> max value)
    ) (0, 0, 0)

printfn "Summary (sum, count, max): %A" summary
```

Map.fold accumulates values across all map entries, taking an accumulator  
function and initial value. The function receives the accumulator, key,  
and value for each entry. This enables computing totals, averages, and  
other aggregate statistics. Complex aggregations can return tuples to  
compute multiple values simultaneously.  

## Map Comparison Operations

Comparing maps for equality and performing set-like operations.  

```F#
let map1 = Map ["a", 1; "b", 2; "c", 3]
let map2 = Map ["a", 1; "b", 2; "c", 3]  
let map3 = Map ["a", 1; "b", 4; "d", 5]

printfn "map1 = map2: %b" (map1 = map2)
printfn "map1 = map3: %b" (map1 = map3)

// Finding common keys
let commonKeys = 
    Set.intersect (Set.ofList (Map.keys map1)) (Set.ofList (Map.keys map3))

printfn "Common keys: %A" commonKeys

// Merging maps with conflict resolution
let mergeMaps map1 map2 =
    Map.fold (fun acc key value ->
        Map.add key (
            match Map.tryFind key acc with
            | Some existing -> existing + value
            | None -> value
        ) acc
    ) map1 map2

let merged = mergeMaps map1 map3
printfn "Merged with sum: %A" merged
```

Maps support structural equality comparison, checking both keys and  
values. Set operations on keys enable finding intersections and  
differences. Custom merge functions handle conflicts when combining  
maps, such as summing values for duplicate keys or applying other  
resolution strategies.  

## Converting Between Collections

Converting maps to and from other F# collection types.  

```F#
let originalMap = Map ["x", 10; "y", 20; "z", 30]

// Convert to different collection types
let asList = originalMap |> Map.toList
let asArray = originalMap |> Map.toArray  
let asSeq = originalMap |> Map.toSeq

let keysList = originalMap |> Map.keys |> Seq.toList
let valuesList = originalMap |> Map.values |> Seq.toList

printfn "Original map: %A" originalMap
printfn "As list: %A" asList
printfn "As array: %A" asArray
printfn "Keys: %A" keysList
printfn "Values: %A" valuesList

// Convert back from different sources
let fromSeq = [("a", 1); ("b", 2); ("c", 3)] |> Map.ofSeq
let fromArray = [|("p", 100); ("q", 200)|] |> Map.ofArray

printfn "From sequence: %A" fromSeq
printfn "From array: %A" fromArray
```

F# provides comprehensive conversion functions between maps and other  
collection types. Map.toList, Map.toArray, and Map.toSeq convert maps  
to sequences of key-value tuples. Corresponding ofList, ofArray, and  
ofSeq functions create maps from tuple sequences. These conversions  
enable interoperability with different collection types.  

## Maps with Custom Types

Using custom types as keys and values in maps with proper comparison.  

```F#
[<Measure>] type celsius
[<Measure>] type fahrenheit

type City = {
    Name: string
    Country: string
} with
    override this.ToString() = $"{this.Name}, {this.Country}"

type WeatherData = {
    Temperature: float<celsius>
    Humidity: int
    Conditions: string
}

let cityWeather = Map [
    {Name="London"; Country="UK"}, 
        {Temperature=15.0<celsius>; Humidity=80; Conditions="Cloudy"}
    {Name="Paris"; Country="France"}, 
        {Temperature=18.0<celsius>; Humidity=65; Conditions="Sunny"}
    {Name="Berlin"; Country="Germany"}, 
        {Temperature=12.0<celsius>; Humidity=75; Conditions="Rainy"}
]

for kvp in cityWeather do
    let city = kvp.Key
    let weather = kvp.Value
    printfn "%O: %g°C, %d%% humidity, %s" 
        city weather.Temperature weather.Humidity weather.Conditions

// Find cities by temperature range
let mildCities = 
    cityWeather 
    |> Map.filter (fun _ weather -> 
        weather.Temperature >= 14.0<celsius> && 
        weather.Temperature <= 20.0<celsius>)

printfn "Mild weather cities: %A" (mildCities |> Map.keys |> Seq.toList)
```

Maps work with any comparable custom types as keys, including records  
and discriminated unions. Custom types must implement comparison  
interfaces properly. Units of measure enhance type safety for numeric  
values. Complex types as both keys and values enable rich domain  
modeling with efficient lookups.  

## Map Partitioning and Grouping

Dividing maps into groups based on criteria.  

```F#
let employees = Map [
    1, ("Alice", "Engineering", 75000)
    2, ("Bob", "Sales", 65000)  
    3, ("Charlie", "Engineering", 82000)
    4, ("Diana", "Marketing", 58000)
    5, ("Eve", "Sales", 71000)
    6, ("Frank", "Engineering", 69000)
]

// Partition by salary threshold
let (highEarners, regularEarners) = 
    employees |> Map.partition (fun _ (_, _, salary) -> salary > 70000)

printfn "High earners: %A" highEarners
printfn "Regular earners: %A" regularEarners

// Group by department
let byDepartment =
    employees
    |> Map.toSeq
    |> Seq.groupBy (fun (_, (_, dept, _)) -> dept)
    |> Seq.map (fun (dept, emps) -> 
        dept, emps |> Seq.map (fun (id, (name, _, salary)) -> 
            id, (name, salary)) |> Map.ofSeq)
    |> Map.ofSeq

for dept in byDepartment do
    printfn "%s department: %A" dept.Key dept.Value
```

Map.partition splits a map into two maps based on a predicate function.  
For grouping by arbitrary criteria, convert to sequences, use Seq.groupBy,  
then reconstruct maps. This enables organizing data into logical groups  
while maintaining the benefits of map lookups within each group.  

## Nested Map Operations

Working with maps containing other maps for hierarchical data.  

```F#
let inventory = Map [
    "Electronics", Map [
        "Laptops", 15
        "Phones", 32
        "Tablets", 8
    ]
    "Books", Map [
        "Fiction", 124
        "Non-fiction", 89
        "Technical", 45
    ]
    "Clothing", Map [
        "Shirts", 67
        "Pants", 43
        "Shoes", 28
    ]
]

// Safe nested lookup
let getStock category item =
    inventory 
    |> Map.tryFind category
    |> Option.bind (Map.tryFind item)
    |> Option.defaultValue 0

printfn "Laptop stock: %d" (getStock "Electronics" "Laptops")
printfn "Magazines stock: %d" (getStock "Books" "Magazines")

// Update nested values
let updateStock category item newQuantity map =
    match Map.tryFind category map with
    | Some categoryMap ->
        let updatedCategory = categoryMap |> Map.add item newQuantity
        map |> Map.add category updatedCategory
    | None ->
        map |> Map.add category (Map [item, newQuantity])

let updatedInventory = 
    inventory
    |> updateStock "Electronics" "Laptops" 20
    |> updateStock "Books" "Magazines" 15

printfn "Updated laptop stock: %d" 
    (getStock "Electronics" "Laptops")
printfn "New magazines stock: %d" 
    (getStock "Books" "Magazines")
```

Nested maps represent hierarchical data structures like trees or  
catalogs. Safe access uses Option.bind to chain nullable lookups,  
avoiding exceptions when intermediate keys don't exist. Updating  
nested maps requires careful reconstruction of the entire path,  
maintaining immutability throughout the hierarchy.  

## Map Performance Considerations

Understanding performance characteristics and optimization strategies.  

```F#
open System.Diagnostics

let measureTime label operation =
    let sw = Stopwatch.StartNew()
    let result = operation()
    sw.Stop()
    printfn "%s: %dms" label sw.ElapsedMilliseconds
    result

// Compare map vs dictionary performance
let testData = [1..100000] |> List.map (fun i -> i, $"value_{i}")

let mapCreation() =
    testData |> Map.ofList

let mapLookups map =
    [1..1000] |> List.map (fun i -> 
        let key = System.Random().Next(1, 100000)
        Map.tryFind key map)

let immutableMap = measureTime "Map creation" mapCreation
let _ = measureTime "Map lookups" (fun () -> mapLookups immutableMap)

// Memory usage comparison
let smallMap = Map [1..10] |> List.map (fun i -> i, i * i) |> Map.ofList
let largeMap = Map [1..10000] |> List.map (fun i -> i, i * i) |> Map.ofList

printfn "Small map size: %d items" smallMap.Count
printfn "Large map size: %d items" largeMap.Count

// Batch operations for better performance
let batchUpdates map updates =
    updates |> List.fold (fun acc (k, v) -> Map.add k v acc) map

let updates = [1..1000] |> List.map (fun i -> i, $"updated_{i}")
let batchedMap = measureTime "Batch updates" (fun () -> 
    batchUpdates Map.empty updates)
```

F# Maps offer O(log n) performance for lookups, insertions, and deletions  
due to their balanced tree implementation. They excel in functional  
scenarios with frequent immutable updates but may be slower than  
Dictionary for pure lookup scenarios. Batch operations can improve  
performance by reducing intermediate allocations.  

## Map-based Caching Patterns

Implementing caching and memoization using maps.  

```F#
let mutable fibCache = Map.empty<int, bigint>

let rec fibonacci n =
    match Map.tryFind n fibCache with
    | Some result -> result
    | None ->
        let result = 
            match n with
            | 0 | 1 -> 1I
            | _ -> fibonacci (n-1) + fibonacci (n-2)
        fibCache <- Map.add n result fibCache
        result

// Test caching performance  
printfn "fib(40) = %A" (fibonacci 40)
printfn "Cache size: %d" fibCache.Count
printfn "fib(45) = %A" (fibonacci 45)  // Uses cached values
printfn "Final cache size: %d" fibCache.Count

// Immutable cache with state threading
type Cache<'K, 'V when 'K : comparison> = Map<'K, 'V>

let memoize (f: 'K -> 'V) =
    let mutable cache = Map.empty
    fun key ->
        match Map.tryFind key cache with
        | Some value -> value
        | None ->
            let value = f key
            cache <- Map.add key value cache
            value

let expensiveOperation x = 
    System.Threading.Thread.Sleep(100)  // Simulate work
    x * x * x

let cachedOperation = memoize expensiveOperation

printfn "First call: %d" (cachedOperation 5)   // Slow
printfn "Second call: %d" (cachedOperation 5)  // Fast (cached)
```

Maps provide excellent foundations for caching due to efficient lookups  
and immutable sharing. Memoization patterns store function results to  
avoid recomputation. While mutable references are sometimes necessary  
for performance, functional approaches using state threading maintain  
immutability benefits while enabling caching strategies.  

## Configuration and Settings Maps

Using maps for application configuration and settings management.  

```F#
type LogLevel = Debug | Info | Warning | Error

type DatabaseConfig = {
    Server: string
    Port: int
    Database: string
    Timeout: int
}

let parseConfig (settings: Map<string, string>) =
    let getRequired key =
        match Map.tryFind key settings with
        | Some value -> value
        | None -> failwith $"Required setting '{key}' not found"
    
    let getInt key defaultValue =
        match Map.tryFind key settings with
        | Some value -> 
            match System.Int32.TryParse(value) with
            | true, num -> num
            | _ -> defaultValue
        | None -> defaultValue
    
    let getLogLevel key defaultLevel =
        match Map.tryFind key settings with
        | Some "Debug" -> Debug
        | Some "Info" -> Info  
        | Some "Warning" -> Warning
        | Some "Error" -> Error
        | _ -> defaultLevel

    {|
        Database = {
            Server = getRequired "db.server"
            Port = getInt "db.port" 5432
            Database = getRequired "db.name"
            Timeout = getInt "db.timeout" 30
        }
        LogLevel = getLogLevel "log.level" Info
        MaxConnections = getInt "max.connections" 100
    |}

let configSettings = Map [
    "db.server", "localhost"
    "db.name", "myapp"  
    "db.port", "5432"
    "log.level", "Debug"
    "max.connections", "50"
]

let config = parseConfig configSettings
printfn "Database config: %A" config.Database
printfn "Log level: %A" config.LogLevel
printfn "Max connections: %d" config.MaxConnections

// Environment-specific overrides
let prodOverrides = Map [
    "db.server", "prod-db-server"
    "log.level", "Error"
    "max.connections", "200"
]

let prodSettings = 
    configSettings 
    |> Map.fold (fun acc k v -> Map.add k v acc) prodOverrides

let prodConfig = parseConfig prodSettings
printfn "Production config: %A" prodConfig
```

Maps excel at configuration management due to their key-value nature  
and immutable properties. Type-safe parsing functions convert string  
settings to appropriate types with default values and validation.  
Environment-specific configurations can override base settings through  
map merging, enabling flexible deployment scenarios.  

## Map-based State Machines

Using maps to represent state transitions and business logic.  

```F#
type OrderStatus = 
    | Pending | Confirmed | Shipped | Delivered | Cancelled

type OrderEvent = 
    | Confirm | Ship | Deliver | Cancel | Return

let transitions = Map [
    (Pending, Confirm), Confirmed
    (Pending, Cancel), Cancelled
    (Confirmed, Ship), Shipped
    (Confirmed, Cancel), Cancelled
    (Shipped, Deliver), Delivered
    (Delivered, Return), Confirmed
]

let processEvent currentStatus event =
    match Map.tryFind (currentStatus, event) transitions with
    | Some newStatus -> 
        printfn "Transition: %A + %A -> %A" currentStatus event newStatus
        Some newStatus
    | None -> 
        printfn "Invalid transition: %A + %A" currentStatus event
        None

// Test state machine
let mutable orderStatus = Pending
printfn "Initial status: %A" orderStatus

match processEvent orderStatus Confirm with
| Some newStatus -> orderStatus <- newStatus
| None -> ()

match processEvent orderStatus Ship with
| Some newStatus -> orderStatus <- newStatus  
| None -> ()

match processEvent orderStatus Cancel with  // Invalid transition
| Some newStatus -> orderStatus <- newStatus
| None -> ()

printfn "Final status: %A" orderStatus
```

Maps naturally represent state transition tables for state machines  
and business workflows. Tuples of (current state, event) serve as keys  
mapping to new states. This approach provides clear, declarative  
specification of valid transitions while making invalid transitions  
explicit through missing map entries.  

## Frequency Analysis with Maps

Counting occurrences and analyzing data distributions using maps.  

```F#
let text = "the quick brown fox jumps over the lazy dog"
let words = text.Split(' ') |> Array.toList

let wordFrequency = 
    words
    |> List.fold (fun acc word ->
        let count = Map.tryFind word acc |> Option.defaultValue 0
        Map.add word (count + 1) acc
    ) Map.empty

printfn "Word frequencies: %A" wordFrequency

// Find most common words
let topWords = 
    wordFrequency
    |> Map.toList
    |> List.sortByDescending snd
    |> List.take 3

printfn "Top 3 words: %A" topWords

// Character frequency analysis
let charFrequency =
    text.ToCharArray()
    |> Array.filter (fun c -> c <> ' ')
    |> Array.fold (fun acc char ->
        let count = Map.tryFind char acc |> Option.defaultValue 0
        Map.add char (count + 1) acc
    ) Map.empty

let sortedChars = 
    charFrequency
    |> Map.toList  
    |> List.sortByDescending snd

printfn "Character frequencies: %A" (sortedChars |> List.take 5)
```

Maps are ideal for frequency analysis and histogram generation.  
The fold pattern with Option.defaultValue handles the first occurrence  
of each item gracefully. Sorting converted key-value pairs enables  
finding most/least frequent items. This pattern applies to any  
counting or aggregation scenario.  

## Maps for Graph Representations

Representing graphs and networks using adjacency maps.  

```F#
type Graph<'T when 'T : comparison> = Map<'T, 'T list>

let cityConnections = Map [
    "New York", ["Boston"; "Philadelphia"; "Washington"]
    "Boston", ["New York"; "Portland"]  
    "Philadelphia", ["New York"; "Washington"; "Pittsburgh"]
    "Washington", ["New York"; "Philadelphia"; "Richmond"]
    "Pittsburgh", ["Philadelphia"; "Cleveland"]
    "Cleveland", ["Pittsburgh"; "Detroit"]
    "Portland", ["Boston"]
    "Richmond", ["Washington"]
    "Detroit", ["Cleveland"]
]

let getNeighbors city graph =
    Map.tryFind city graph |> Option.defaultValue []

let rec findPath visited current destination graph =
    if current = destination then
        Some [current]
    elif Set.contains current visited then
        None
    else
        let neighbors = getNeighbors current graph
        let newVisited = Set.add current visited
        
        neighbors
        |> List.tryPick (fun neighbor ->
            findPath newVisited neighbor destination graph
            |> Option.map (fun path -> current :: path))

match findPath Set.empty "New York" "Detroit" cityConnections with
| Some path -> printfn "Path found: %A" path
| None -> printfn "No path exists"

// Graph statistics
let totalEdges = 
    cityConnections 
    |> Map.toList
    |> List.sumBy (fun (_, neighbors) -> neighbors.Length)

printfn "Total cities: %d" cityConnections.Count
printfn "Total connections: %d" totalEdges
printfn "Average connections per city: %.2f" 
    (float totalEdges / float cityConnections.Count)
```

Maps effectively represent graphs through adjacency lists, where keys  
are nodes and values are neighbor lists. This representation supports  
efficient neighbor lookup and graph traversal algorithms. Path finding  
uses recursion with visited set tracking to avoid cycles. Graph  
analytics become straightforward with map operations.  

## Best Practices Summary

Key recommendations for effective F# map usage.  

```F#
// 1. Prefer Map.tryFind over direct indexing
let safeAccess key map =
    match Map.tryFind key map with
    | Some value -> $"Found: {value}"
    | None -> "Not found"

// 2. Use Map.choose for filter + transform
let processGrades grades =
    grades |> Map.choose (fun student grade ->
        if grade >= 70 then Some $"{student}: Pass"
        else None)

// 3. Leverage immutability for safe sharing
let baseConfig = Map ["timeout", "30"; "retries", "3"]
let devConfig = baseConfig |> Map.add "debug", "true" 
let prodConfig = baseConfig |> Map.add "logging", "minimal"

// 4. Use batch operations for multiple updates  
let applyUpdates map updates =
    updates |> List.fold (fun acc (k, v) -> Map.add k v acc) map

// 5. Consider Map.partition for conditional splits
let categorizeByThreshold threshold values =
    values |> Map.partition (fun _ value -> value > threshold)

// 6. Use appropriate conversion functions
let efficientConstruction items =
    items |> List.toArray |> Map.ofArray  // If items is large

printfn "Map best practices demonstrated"

// Performance tip: Reuse maps when possible
let sharedLookup = Map [1, "one"; 2, "two"; 3, "three"]
let variations = [
    sharedLookup |> Map.add 4 "four"
    sharedLookup |> Map.add 5 "five"  
    sharedLookup |> Map.remove 1
]

printfn "Shared structure enables efficient variations"
```

Effective F# map usage follows functional programming principles:  
prefer safe operations like tryFind, leverage immutability for sharing,  
use specialized functions like choose for combined operations, and  
understand performance characteristics. Maps excel in scenarios  
requiring ordered keys, immutable updates, and functional composition  
patterns.  

## Map Union and Intersection

Combining maps using union and intersection operations.  

```F#
let map1 = Map ["a", 1; "b", 2; "c", 3]
let map2 = Map ["b", 20; "c", 30; "d", 4]

// Union with custom merge function
let unionWith combiner map1 map2 =
    Map.fold (fun acc key value ->
        match Map.tryFind key acc with
        | Some existing -> Map.add key (combiner existing value) acc
        | None -> Map.add key value acc
    ) map1 map2

let sumUnion = unionWith (+) map1 map2
let maxUnion = unionWith max map1 map2

printfn "Map1: %A" map1
printfn "Map2: %A" map2  
printfn "Sum union: %A" sumUnion
printfn "Max union: %A" maxUnion

// Intersection keeping values from first map
let intersect map1 map2 =
    Map.filter (fun key _ -> Map.containsKey key map2) map1

let commonEntries = intersect map1 map2
printfn "Intersection: %A" commonEntries
```

Map union and intersection operations require custom logic since there's  
no built-in support. Union operations need conflict resolution strategies  
for duplicate keys, such as summing, taking maximum, or other combining  
functions. Intersection filters maps to contain only keys present in  
both maps, useful for finding common elements.  

## Map Indexing and Reverse Lookups

Creating bidirectional lookups and indexed access patterns.  

```F#
let employees = Map [
    101, "Alice Johnson"
    102, "Bob Smith"  
    103, "Carol Davis"
    104, "David Wilson"
]

// Create reverse lookup map
let nameToId = 
    employees
    |> Map.toSeq
    |> Seq.map (fun (id, name) -> (name, id))
    |> Map.ofSeq

printfn "ID to Name: %A" employees
printfn "Name to ID: %A" nameToId

// Bidirectional lookup functions
let findNameById id = Map.tryFind id employees
let findIdByName name = Map.tryFind name nameToId

printfn "Name for 102: %A" (findNameById 102)
printfn "ID for Carol Davis: %A" (findIdByName "Carol Davis")

// Index-based access simulation
let getByIndex index map =
    map 
    |> Map.toList
    |> List.tryItem index
    |> Option.map snd

printfn "Employee at index 1: %A" (getByIndex 1 employees)
```

Reverse lookups enable bidirectional map searches by creating inverted  
key-value mappings. This is useful for scenarios requiring lookups in  
both directions. Index-based access converts maps to lists temporarily,  
though this is inefficient for frequent use. Consider using arrays  
for scenarios requiring frequent index-based access.  

## Map Serialization and Persistence  

Converting maps to/from JSON and other serialization formats.  

```F#
open System.Text.Json
open System.IO

let productCatalog = Map [
    "P001", {| Name = "Laptop"; Price = 999.99; InStock = true |}
    "P002", {| Name = "Mouse"; Price = 29.99; InStock = false |}
    "P003", {| Name = "Keyboard"; Price = 79.99; InStock = true |}
]

// Serialize to JSON
let serializeMap map =
    map
    |> Map.toSeq
    |> dict
    |> JsonSerializer.Serialize

let json = serializeMap productCatalog
printfn "Serialized JSON:\n%s" json

// Save to file
File.WriteAllText("catalog.json", json)

// Deserialize from JSON
let deserializeMap<'K, 'V when 'K : comparison> (json: string) =
    JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<'K, 'V>>(json)
    |> Seq.map (|KeyValue|)
    |> Map.ofSeq

let loadedCatalog = File.ReadAllText("catalog.json")
printfn "Loaded from file: %s" loadedCatalog

// Custom serialization for complex types
let serializeToKeyValuePairs map =
    map
    |> Map.toList
    |> List.map (fun (k, v) -> $"{k}={v}")
    |> String.concat ";"

let parseKeyValuePairs (str: string) =
    str.Split(';')
    |> Array.choose (fun pair ->
        match pair.Split('=') with
        | [|key; value|] -> Some (key, value)
        | _ -> None)
    |> Map.ofArray

let simpleMap = Map ["key1", "value1"; "key2", "value2"]
let serialized = serializeToKeyValuePairs simpleMap
let deserialized = parseKeyValuePairs serialized

printfn "Original: %A" simpleMap
printfn "Serialized: %s" serialized  
printfn "Deserialized: %A" deserialized
```

Map serialization enables persistence and data exchange. JSON  
serialization works well through dictionary conversion, though type  
information may be lost. Custom serialization formats provide more  
control over the process. Always consider round-trip consistency  
when implementing serialization schemes.  

## Map-based Command Pattern

Implementing command patterns and dispatching using maps.  

```F#
type Command = string * string list

let commands = Map [
    "greet", fun args ->
        match args with
        | [name] -> $"Hello, {name}!"
        | [] -> "Hello, World!"
        | _ -> "Usage: greet [name]"
    
    "add", fun args ->
        match args |> List.map System.Int32.TryParse with
        | (true, a) :: (true, b) :: _ -> $"{a} + {b} = {a + b}"
        | _ -> "Usage: add <number1> <number2>"
    
    "echo", fun args ->
        args |> String.concat " "
    
    "help", fun _ ->
        let availableCommands = commands |> Map.keys |> String.concat ", "
        $"Available commands: {availableCommands}"
]

let executeCommand (command: string) (args: string list) =
    match Map.tryFind command commands with
    | Some handler -> handler args
    | None -> $"Unknown command: {command}. Type 'help' for available commands."

// Test the command system
let testCommands = [
    ("greet", ["Alice"])
    ("greet", [])
    ("add", ["5"; "3"])
    ("echo", ["Hello"; "from"; "F#"])
    ("help", [])
    ("invalid", ["test"])
]

for (cmd, args) in testCommands do
    let result = executeCommand cmd args
    printfn "> %s %s" cmd (String.concat " " args)
    printfn "  %s" result
    printfn ""
```

Maps provide excellent foundations for command patterns and dispatching  
systems. Keys represent command names while values contain handler  
functions. This approach enables extensible, declarative command  
systems with clean separation between command registration and  
execution logic.  

## Map-based Dependency Injection

Simple dependency injection and service location using maps.  

```F#
type ILogger = 
    abstract member Log : string -> unit

type IDatabase =
    abstract member GetUser : int -> string option

type ConsoleLogger() =
    interface ILogger with
        member _.Log(message) = printfn "[LOG] %s" message

type InMemoryDatabase() =
    let users = Map [1, "Alice"; 2, "Bob"; 3, "Charlie"]
    interface IDatabase with
        member _.GetUser(id) = Map.tryFind id users

// Service container
let mutable services = Map.empty<System.Type, obj>

let registerService<'T> (instance: 'T) =
    services <- Map.add typeof<'T> (box instance) services

let getService<'T>() =
    match Map.tryFind typeof<'T> services with
    | Some service -> Some (unbox<'T> service)
    | None -> None

// Register services
registerService<ILogger> (ConsoleLogger())
registerService<IDatabase> (InMemoryDatabase())

// Use services
let userService userId =
    match getService<ILogger>(), getService<IDatabase>() with
    | Some logger, Some database ->
        logger.Log($"Looking up user {userId}")
        match database.GetUser(userId) with
        | Some name -> 
            logger.Log($"Found user: {name}")
            Some name
        | None ->
            logger.Log($"User {userId} not found")
            None
    | _ -> failwith "Required services not registered"

// Test the service
let result = userService 2
printfn "User lookup result: %A" result
```

Maps can implement simple dependency injection containers by storing  
type-to-instance mappings. This approach provides basic service  
location capabilities for small applications. While not as sophisticated  
as dedicated DI frameworks, it demonstrates functional approaches to  
dependency management and service composition.  

## Map-based Event Sourcing

Implementing simple event sourcing patterns with maps for state tracking.  

```F#
type AccountId = AccountId of int
type Event = 
    | AccountCreated of AccountId * string * decimal
    | MoneyDeposited of AccountId * decimal * System.DateTime
    | MoneyWithdrawn of AccountId * decimal * System.DateTime

type Account = {
    Id: AccountId
    Owner: string  
    Balance: decimal
    Created: System.DateTime
}

let applyEvent state event =
    match event with
    | AccountCreated (id, owner, initialBalance) ->
        let account = {
            Id = id
            Owner = owner
            Balance = initialBalance
            Created = System.DateTime.Now
        }
        Map.add id account state
    
    | MoneyDeposited (id, amount, _) ->
        match Map.tryFind id state with
        | Some account -> 
            Map.add id { account with Balance = account.Balance + amount } state
        | None -> state
    
    | MoneyWithdrawn (id, amount, _) ->
        match Map.tryFind id state with
        | Some account when account.Balance >= amount ->
            Map.add id { account with Balance = account.Balance - amount } state
        | _ -> state

let events = [
    AccountCreated (AccountId 1, "Alice", 100m)
    AccountCreated (AccountId 2, "Bob", 50m)
    MoneyDeposited (AccountId 1, 25m, System.DateTime.Now)
    MoneyWithdrawn (AccountId 2, 10m, System.DateTime.Now)
    MoneyWithdrawn (AccountId 1, 200m, System.DateTime.Now) // Should fail
]

let finalState = events |> List.fold applyEvent Map.empty

printfn "Final account states:"
for kvp in finalState do
    printfn "Account %A: %s - Balance: $%.2f" 
        kvp.Key kvp.Value.Owner kvp.Value.Balance

// Replay events to specific point
let stateAfterThreeEvents = 
    events 
    |> List.take 3 
    |> List.fold applyEvent Map.empty

printfn "\nState after first 3 events:"
for kvp in stateAfterThreeEvents do
    printfn "Account %A: Balance: $%.2f" kvp.Key kvp.Value.Balance
```

Event sourcing uses maps to maintain current state while preserving  
event history. Events are immutable facts, while state maps represent  
current system state derived from event application. This pattern  
enables time travel, audit trails, and system recovery by replaying  
events to any point in time.  

## Advanced Map Patterns

Complex map usage patterns for sophisticated scenarios.  

```F#
// Multi-level grouping with maps
let sales = [
    ("Q1", "North", "Laptops", 15000)
    ("Q1", "North", "Phones", 8000)
    ("Q1", "South", "Laptops", 12000)  
    ("Q2", "North", "Laptops", 18000)
    ("Q2", "South", "Phones", 9500)
]

let groupSales data =
    data
    |> List.fold (fun acc (quarter, region, product, amount) ->
        let quarterMap = Map.tryFind quarter acc |> Option.defaultValue Map.empty
        let regionMap = Map.tryFind region quarterMap |> Option.defaultValue Map.empty
        let currentAmount = Map.tryFind product regionMap |> Option.defaultValue 0
        
        let newRegionMap = Map.add product (currentAmount + amount) regionMap  
        let newQuarterMap = Map.add region newRegionMap quarterMap
        Map.add quarter newQuarterMap acc
    ) Map.empty

let groupedSales = groupSales sales

// Query the nested structure
let getQuarterTotal quarter data =
    data
    |> Map.tryFind quarter
    |> Option.map (Map.toSeq >> Seq.sumBy (fun (_, regionMap) ->
        regionMap |> Map.toSeq |> Seq.sumBy snd))
    |> Option.defaultValue 0

printfn "Q1 Total: %d" (getQuarterTotal "Q1" groupedSales)
printfn "Q2 Total: %d" (getQuarterTotal "Q2" groupedSales)

// Map-based trie for prefix matching
type Trie = Map<char, Trie * bool>

let insertWord word trie =
    let rec insert chars currentTrie =
        match chars with
        | [] -> currentTrie
        | c :: rest ->
            let (subTrie, _) = Map.tryFind c currentTrie |> Option.defaultValue (Map.empty, false)
            let newSubTrie = insert rest subTrie
            let isEndOfWord = List.isEmpty rest
            Map.add c (newSubTrie, isEndOfWord) currentTrie
    insert (word |> Seq.toList) trie

let hasPrefix prefix trie =
    let rec check chars currentTrie =
        match chars with
        | [] -> true
        | c :: rest ->
            match Map.tryFind c currentTrie with
            | Some (subTrie, _) -> check rest subTrie
            | None -> false
    check (prefix |> Seq.toList) trie

let words = ["cat"; "car"; "card"; "care"; "careful"; "dog"; "dodge"]
let trie = words |> List.fold (fun acc word -> insertWord word acc) Map.empty

printfn "Has prefix 'car': %b" (hasPrefix "car" trie)
printfn "Has prefix 'dog': %b" (hasPrefix "dog" trie)
printfn "Has prefix 'xyz': %b" (hasPrefix "xyz" trie)
```

Advanced map patterns include multi-level nested grouping for  
hierarchical data organization and trie structures for efficient  
prefix matching. These patterns demonstrate maps' versatility for  
complex data structures while maintaining functional programming  
principles and immutability benefits.  

## Map Performance Monitoring

Profiling and monitoring map operations for optimization insights.  

```F#
open System.Diagnostics
open System.Collections.Generic

let timeOperation name operation =
    let stopwatch = Stopwatch.StartNew()
    let result = operation()
    stopwatch.Stop()
    printfn "%s: %dms" name stopwatch.ElapsedMilliseconds
    result

// Compare F# Map vs .NET Dictionary
let testSize = 100000
let testData = [1..testSize] |> List.map (fun i -> (i, $"value_{i}"))

// F# Map creation and operations
let fsharpMapTest() =
    let map = timeOperation "F# Map creation" (fun () ->
        testData |> Map.ofList)
    
    let _ = timeOperation "F# Map 1000 lookups" (fun () ->
        [1..1000] |> List.map (fun i -> 
            Map.tryFind (i * 100) map))
    
    let _ = timeOperation "F# Map iteration" (fun () ->
        map |> Map.fold (fun acc _ _ -> acc + 1) 0)
    
    printfn "F# Map size: %d" map.Count

// .NET Dictionary comparison
let dictionaryTest() =
    let dict = timeOperation "Dictionary creation" (fun () ->
        let d = Dictionary<int, string>()
        for (k, v) in testData do d.[k] <- v
        d)
    
    let _ = timeOperation "Dictionary 1000 lookups" (fun () ->
        [1..1000] |> List.map (fun i -> 
            dict.TryGetValue(i * 100)))
    
    let _ = timeOperation "Dictionary iteration" (fun () ->
        dict |> Seq.fold (fun acc _ -> acc + 1) 0)
    
    printfn "Dictionary size: %d" dict.Count

printfn "Performance Comparison (F# Map vs Dictionary):"
printfn "============================================="
fsharpMapTest()
printfn ""
dictionaryTest()

// Memory usage estimation
let estimateMapMemory map =
    let keys = map |> Map.keys |> Seq.length
    let avgKeySize = 4 // int
    let avgValueSize = 20 // estimated string
    let overhead = 32 // tree node overhead estimate
    keys * (avgKeySize + avgValueSize + overhead)

let sampleMap = [1..1000] |> List.map (fun i -> (i, $"value_{i}")) |> Map.ofList
let estimatedBytes = estimateMapMemory sampleMap
printfn "\nEstimated memory usage: %d bytes (%.2f KB)" 
    estimatedBytes (float estimatedBytes / 1024.0)
```

Performance monitoring helps understand map behavior characteristics  
and guides optimization decisions. F# Maps excel in functional scenarios  
with immutable updates and sharing, while dictionaries perform better  
for pure lookup scenarios. Memory overhead varies based on key-value  
sizes and tree structure, making measurement important for large datasets.  

# F# Computation Expressions - 25 Essential Examples

This comprehensive guide covers F# computation expressions with 25 practical  
examples, from basic async workflows to advanced custom computation builders.  
Each example demonstrates different approaches to composing and sequencing  
computations in F# applications.

## Basic Async Workflow

A simple async computation that demonstrates the fundamental async builder.  

```fsharp
open System
open System.Threading.Tasks

let basicAsyncExample() =
    async {
        do! Async.Sleep(1000)
        printfn "Hello from async!"
        return 42
    }

let runBasicAsync() =
    let result = basicAsyncExample() |> Async.RunSynchronously
    printfn "Result: %d" result

// Usage
runBasicAsync()
```

This example shows the basic async computation expression syntax using  
the async builder. The async { } block creates an asynchronous workflow  
that can be composed with other async operations. The do! operator  
performs asynchronous binding, while return yields a value.  

## Async with Error Handling

Demonstrating error handling within async computation expressions.  

```fsharp
open System

let asyncWithErrorHandling() =
    async {
        try
            do! Async.Sleep(500)
            let random = Random()
            let value = random.Next(1, 10)
            
            if value > 5 then
                return sprintf "Success: %d" value
            else
                failwith "Random value too low"
        with
        | ex -> 
            printfn "Error caught: %s" ex.Message
            return "Error handled gracefully"
    }

let runAsyncWithErrorHandling() =
    let result = asyncWithErrorHandling() |> Async.RunSynchronously
    printfn "Result: %s" result

// Usage
runAsyncWithErrorHandling()
```

Error handling in async workflows follows standard F# exception patterns.  
The try-with block inside the async expression catches exceptions that  
occur during asynchronous operations, allowing for graceful error recovery  
and maintaining the async context throughout the computation.  

## Sequence Comprehension

Using seq computation expressions for lazy sequence generation.  

```fsharp
let generateSquares max =
    seq {
        for i in 1..max do
            let square = i * i
            printfn "Computing square of %d" i
            yield square
    }

let generateConditionalSequence max =
    seq {
        for i in 1..max do
            if i % 2 = 0 then
                yield i * 2
            else
                yield! seq { yield i; yield i + 1 }
    }

// Usage
let squares = generateSquares 5
printfn "Squares: %A" (squares |> Seq.toList)

let conditional = generateConditionalSequence 6
printfn "Conditional: %A" (conditional |> Seq.toList)
```

Sequence expressions create lazy sequences that compute values on demand.  
The yield keyword produces single values, while yield! flattens nested  
sequences. This lazy evaluation means computations only occur when values  
are actually consumed, providing efficient memory usage for large datasets.  

## Option Workflow

Working with option types using option computation expressions.  

```fsharp
type Customer = {
    Id: int
    Name: string
    Email: string option
}

let customers = [
    { Id = 1; Name = "Alice"; Email = Some "alice@example.com" }
    { Id = 2; Name = "Bob"; Email = None }
    { Id = 3; Name = "Charlie"; Email = Some "charlie@example.com" }
]

let findCustomerEmail customerId =
    option {
        let! customer = customers |> List.tryFind (fun c -> c.Id = customerId)
        let! email = customer.Email
        return email.ToUpper()
    }

// Usage
match findCustomerEmail 1 with
| Some email -> printfn "Customer email: %s" email
| None -> printfn "Customer or email not found"

match findCustomerEmail 2 with
| Some email -> printfn "Customer email: %s" email  
| None -> printfn "Customer or email not found"
```

Option computation expressions provide elegant handling of nullable values  
through the option builder. The let! operator extracts values from options,  
automatically handling None cases by short-circuiting the computation.  
This eliminates nested pattern matching for complex option chains.  

## Result Workflow

Error handling using result computation expressions for railway programming.  

```fsharp
type ValidationError =
    | EmptyName
    | InvalidEmail
    | AgeTooYoung

type Person = {
    Name: string
    Email: string
    Age: int
}

let validateName name =
    if String.IsNullOrWhiteSpace(name) then
        Error EmptyName
    else
        Ok name

let validateEmail email =
    if email |> String.contains "@" then
        Ok email
    else
        Error InvalidEmail

let validateAge age =
    if age >= 18 then
        Ok age
    else
        Error AgeTooYoung

let createPerson name email age =
    result {
        let! validName = validateName name
        let! validEmail = validateEmail email
        let! validAge = validateAge age
        return {
            Name = validName
            Email = validEmail
            Age = validAge
        }
    }

// Usage
match createPerson "John Doe" "john@example.com" 25 with
| Ok person -> printfn "Created person: %A" person
| Error EmptyName -> printfn "Error: Name cannot be empty"
| Error InvalidEmail -> printfn "Error: Invalid email format"
| Error AgeTooYoung -> printfn "Error: Age must be 18 or older"

match createPerson "" "invalid-email" 16 with
| Ok person -> printfn "Created person: %A" person
| Error error -> printfn "Validation failed: %A" error
```

Result computation expressions implement railway-oriented programming  
for error handling. Each validation step returns either Ok or Error,  
and the let! operator automatically handles error propagation. The first  
Error encountered stops the computation and returns that error, providing  
clean and composable error handling without nested conditionals.  

## List Comprehension

Creating and transforming lists using list computation expressions.  

```fsharp
let generatePythonTriples limit =
    [
        for a in 1..limit do
            for b in a..limit do
                for c in b..limit do
                    if a*a + b*b = c*c then
                        yield (a, b, c)
    ]

let processNumbers numbers =
    [
        for n in numbers do
            if n % 2 = 0 then
                yield n / 2
            else
                yield! [n; n * 2; n * 3]
    ]

// Usage
let triples = generatePythonTriples 20
printfn "Pythagorean triples: %A" triples

let processed = processNumbers [1; 2; 3; 4; 5]
printfn "Processed numbers: %A" processed
```

List comprehensions provide declarative syntax for list generation and  
transformation. Multiple nested loops create cartesian products, while  
conditional expressions filter results. The yield! operator flattens  
nested lists, allowing complex transformations to be expressed concisely  
and readably without explicit recursion or fold operations.  

## Array Comprehension

Working with arrays using array computation expressions for performance.  

```fsharp
let generateMatrix rows cols =
    [|
        for i in 0..rows-1 do
            for j in 0..cols-1 do
                yield (i, j, i * cols + j)
    |]

let processDataParallel data =
    [|
        for item in data do
            let processed = item * item + 1
            if processed > 10 then
                yield processed
    |]

let createLookupTable size =
    [|
        for i in 0..size-1 do
            yield [|
                for j in 0..size-1 do
                    yield i * j
            |]
    |]

// Usage
let matrix = generateMatrix 3 4
printfn "Matrix: %A" matrix

let processed = processDataParallel [|1..10|]
printfn "Processed: %A" processed

let lookup = createLookupTable 5
printfn "Lookup table shape: %d x %d" lookup.Length lookup.[0].Length
```

Array comprehensions create mutable arrays with better performance for  
numerical computations. The syntax mirrors list comprehensions but produces  
arrays that support in-place updates and provide better cache locality.  
This makes them ideal for mathematical operations and data processing  
scenarios where performance is critical.  

## Async Parallel Operations

Combining multiple async operations with parallel execution.  

```fsharp
open System.Net.Http
open System.Threading.Tasks

let fetchUrl (client: HttpClient) url =
    async {
        try
            let! response = client.GetStringAsync(url) |> Async.AwaitTask
            return Ok (url, response.Length)
        with
        | ex -> return Error (url, ex.Message)
    }

let fetchMultipleUrls urls =
    async {
        use client = new HttpClient()
        let! results = 
            urls
            |> List.map (fetchUrl client)
            |> Async.Parallel
        return results
    }

let processResults results =
    results
    |> Array.fold (fun (success, errors) result ->
        match result with
        | Ok (url, length) -> 
            (url, length) :: success, errors
        | Error (url, error) -> 
            success, (url, error) :: errors
    ) ([], [])

// Usage (commented out as it requires network access)
(*
let urls = [
    "https://www.google.com"
    "https://www.github.com"
    "https://www.stackoverflow.com"
]

let runParallelFetch() =
    async {
        let! results = fetchMultipleUrls urls
        let (successful, failed) = processResults results
        
        printfn "Successful requests:"
        successful |> List.iter (fun (url, length) ->
            printfn "  %s: %d characters" url length)
        
        printfn "Failed requests:"
        failed |> List.iter (fun (url, error) ->
            printfn "  %s: %s" url error)
    }
*)
```

Async parallel operations allow concurrent execution of multiple async  
workflows using Async.Parallel. This pattern is essential for I/O bound  
operations like web requests, where sequential execution would be  
inefficient. The computation expression syntax maintains readability  
while enabling powerful concurrency patterns.  

## Custom Maybe Builder

Creating a custom computation expression for Maybe/Option semantics.  

```fsharp
type MaybeBuilder() =
    member _.Return(value) = Some value
    member _.ReturnFrom(option) = option
    member _.Bind(option, func) =
        match option with
        | Some value -> func value
        | None -> None
    member _.Zero() = None
    member _.Combine(option1, option2) =
        match option1 with
        | Some _ -> option1
        | None -> option2

let maybe = MaybeBuilder()

let divide x y =
    if y = 0.0 then None else Some (x / y)

let sqrt x =
    if x < 0.0 then None else Some (sqrt x)

let complexCalculation a b c =
    maybe {
        let! step1 = divide a b
        let! step2 = sqrt step1
        let! step3 = divide step2 c
        return step3 * 100.0
    }

// Usage
match complexCalculation 100.0 4.0 2.0 with
| Some result -> printfn "Result: %.2f" result
| None -> printfn "Calculation failed"

match complexCalculation 100.0 0.0 2.0 with
| Some result -> printfn "Result: %.2f" result
| None -> printfn "Calculation failed (division by zero)"
```

Custom computation builders extend F#'s computation expression system  
by defining specific operations like Bind, Return, and Combine. The Maybe  
builder implements short-circuiting behavior for option chains, making  
complex optional computations more readable than nested pattern matching.  

## State Computation

Managing state through computation expressions with a custom State monad.  

```fsharp
type State<'state, 'value> = State of ('state -> 'value * 'state)

type StateBuilder() =
    member _.Return(value) = State (fun state -> (value, state))
    member _.Bind(State computation, func) =
        State (fun state ->
            let (value, newState) = computation state
            let (State nextComputation) = func value
            nextComputation newState)

let state = StateBuilder()

let getState = State (fun s -> (s, s))
let setState newState = State (fun _ -> ((), newState))

let incrementCounter =
    state {
        let! current = getState
        do! setState (current + 1)
        return current
    }

let doubleCounter =
    state {
        let! current = getState
        do! setState (current * 2)
        return current
    }

let complexStateOperation =
    state {
        let! initial = getState
        let! after1 = incrementCounter
        let! after2 = doubleCounter  
        let! final = getState
        return (initial, after1, after2, final)
    }

// Usage
let runState (State computation) initialState =
    computation initialState

let (result, finalState) = runState complexStateOperation 5
printfn "Results: %A, Final state: %d" result finalState
```

State computation expressions encapsulate stateful computations in a  
purely functional way. The State monad threads state through a sequence  
of computations without exposing mutable state. This pattern enables  
complex stateful algorithms while maintaining functional purity and  
composability.  

## Async Result Combination

Combining async and result for error handling in asynchronous operations.  

```fsharp
open System
open System.IO

type FileError =
    | FileNotFound of string
    | AccessDenied of string
    | InvalidContent of string

let asyncResult = AsyncResultBuilder()

type AsyncResultBuilder() =
    member _.Return(value) = async { return Ok value }
    member _.ReturnFrom(asyncResult) = asyncResult
    member _.Bind(asyncResult, func) =
        async {
            match! asyncResult with
            | Ok value -> return! func value
            | Error error -> return Error error
        }

let readFileAsync path =
    async {
        try
            if not (File.Exists(path)) then
                return Error (FileNotFound path)
            else
                let! content = File.ReadAllTextAsync(path) |> Async.AwaitTask
                return Ok content
        with
        | :? UnauthorizedAccessException ->
            return Error (AccessDenied path)
        | ex ->
            return Error (InvalidContent ex.Message)
    }

let processFileContent content =
    if String.IsNullOrWhiteSpace(content) then
        Error (InvalidContent "File content is empty")
    else
        Ok (content.Length)

let analyzeFile path =
    asyncResult {
        let! content = readFileAsync path
        let! length = async { return processFileContent content }
        return sprintf "File %s has %d characters" path length
    }

// Usage (with a test file)
let testFileProcessing() =
    async {
        // Create a test file
        let testPath = Path.GetTempFileName()
        do! File.WriteAllTextAsync(testPath, "Hello, World!") |> Async.AwaitTask
        
        match! analyzeFile testPath with
        | Ok result -> printfn "Success: %s" result
        | Error (FileNotFound path) -> printfn "File not found: %s" path
        | Error (AccessDenied path) -> printfn "Access denied: %s" path
        | Error (InvalidContent msg) -> printfn "Invalid content: %s" msg
        
        // Cleanup
        File.Delete(testPath)
    }

// testFileProcessing() |> Async.RunSynchronously
```

AsyncResult combines async and result computation expressions to handle  
both asynchronous operations and error scenarios elegantly. This pattern  
is essential for I/O operations that can fail, providing composable error  
handling without nested try-catch blocks or complex error propagation.  

## Query Expression

Using query expressions for data transformation and filtering.  

```fsharp
type Product = {
    Id: int
    Name: string
    Price: decimal
    Category: string
    InStock: bool
}

let products = [
    { Id = 1; Name = "Laptop"; Price = 999.99m; Category = "Electronics"; InStock = true }
    { Id = 2; Name = "Book"; Price = 19.99m; Category = "Education"; InStock = true }
    { Id = 3; Name = "Phone"; Price = 699.99m; Category = "Electronics"; InStock = false }
    { Id = 4; Name = "Desk"; Price = 299.99m; Category = "Furniture"; InStock = true }
    { Id = 5; Name = "Chair"; Price = 149.99m; Category = "Furniture"; InStock = true }
]

let expensiveInStockProducts =
    query {
        for product in products do
        where (product.Price > 200m && product.InStock)
        sortByDescending product.Price
        select { product with Name = product.Name.ToUpper() }
    }

let categoryPriceSummary =
    query {
        for product in products do
        where product.InStock
        groupBy product.Category into g
        select (g.Key, g.Sum(fun p -> p.Price))
    }

// Usage
printfn "Expensive in-stock products:"
expensiveInStockProducts |> Seq.iter (fun p ->
    printfn "  %s: $%.2f" p.Name p.Price)

printfn "\nCategory price summary:"
categoryPriceSummary |> Seq.iter (fun (category, total) ->
    printfn "  %s: $%.2f" category total)
```

Query expressions provide SQL-like syntax for data manipulation in F#.  
They support filtering, sorting, grouping, and projection operations  
on collections. The syntax maps to LINQ operations under the hood,  
making it familiar to developers from other .NET languages while  
maintaining F#'s functional programming style.  

## Validation Builder

Creating a validation computation expression for accumulating errors.  

```fsharp
type ValidationResult<'a> =
    | Success of 'a
    | Failure of string list

type ValidationBuilder() =
    member _.Return(value) = Success value
    member _.ReturnFrom(result) = result
    member _.Bind(result, func) =
        match result with
        | Success value -> func value
        | Failure errors -> Failure errors
    member _.Apply(resultFunc, resultValue) =
        match (resultFunc, resultValue) with
        | (Success func, Success value) -> Success (func value)
        | (Success _, Failure errors) -> Failure errors
        | (Failure errors, Success _) -> Failure errors
        | (Failure errors1, Failure errors2) -> Failure (errors1 @ errors2)

let validation = ValidationBuilder()

let validateRequired field value =
    if String.IsNullOrWhiteSpace(value) then
        Failure [sprintf "%s is required" field]
    else
        Success value

let validateEmail email =
    if email |> String.contains "@" && email |> String.contains "." then
        Success email
    else
        Failure ["Invalid email format"]

let validateAge age =
    if age >= 18 then
        Success age
    else
        Failure ["Age must be 18 or older"]

type Customer = {
    Name: string
    Email: string
    Age: int
}

let createCustomer name email age =
    validation {
        let! validName = validateRequired "Name" name
        let! validEmail = validateEmail email
        let! validAge = validateAge age
        return {
            Name = validName
            Email = validEmail
            Age = validAge
        }
    }

// Usage
match createCustomer "John" "john@example.com" 25 with
| Success customer -> printfn "Created customer: %A" customer
| Failure errors -> 
    printfn "Validation errors:"
    errors |> List.iter (fun error -> printfn "  - %s" error)

match createCustomer "" "invalid-email" 16 with
| Success customer -> printfn "Created customer: %A" customer
| Failure errors ->
    printfn "Validation errors:"
    errors |> List.iter (fun error -> printfn "  - %s" error)
```

Validation computation expressions accumulate all validation errors  
instead of stopping at the first failure. This provides better user  
experience by showing all validation issues at once. The Apply method  
enables applicative-style validation where multiple validations can  
be combined while preserving all error messages.  

## Reader Monad

Implementing dependency injection through the Reader computation expression.  

```fsharp
type Reader<'env, 'a> = Reader of ('env -> 'a)

type ReaderBuilder() =
    member _.Return(value) = Reader (fun _ -> value)
    member _.Bind(Reader computation, func) =
        Reader (fun env ->
            let value = computation env
            let (Reader nextComputation) = func value
            nextComputation env)

let reader = ReaderBuilder()

type DatabaseConfig = {
    ConnectionString: string
    Timeout: int
}

type LoggingConfig = {
    Level: string
    OutputPath: string
}

type AppConfig = {
    Database: DatabaseConfig
    Logging: LoggingConfig
    AppName: string
}

let askConfig = Reader (fun config -> config)

let getDatabaseConnection =
    reader {
        let! config = askConfig
        return sprintf "Connected to %s with timeout %d" 
            config.Database.ConnectionString 
            config.Database.Timeout
    }

let getLogger =
    reader {
        let! config = askConfig
        return sprintf "Logger configured: %s -> %s" 
            config.Logging.Level 
            config.Logging.OutputPath
    }

let runApplication =
    reader {
        let! config = askConfig
        let! dbConnection = getDatabaseConnection
        let! logger = getLogger
        return sprintf "Starting %s\n%s\n%s" 
            config.AppName dbConnection logger
    }

// Usage
let appConfig = {
    Database = { ConnectionString = "Server=localhost"; Timeout = 30 }
    Logging = { Level = "INFO"; OutputPath = "/var/log/app.log" }
    AppName = "My Application"
}

let runReader (Reader computation) env = computation env
let result = runReader runApplication appConfig
printfn "%s" result
```

The Reader monad provides a clean way to handle dependency injection  
in functional programming. Dependencies are threaded through computations  
without explicit parameter passing. This pattern eliminates the need  
for global variables while maintaining testability and composability  
of components that depend on external configuration or services.  

## Workflow Builder

Creating a workflow computation expression for business process modeling.  

```fsharp
type WorkflowStep<'a> =
    | Success of 'a
    | Failed of string
    | Pending of (unit -> WorkflowStep<'a>)

type WorkflowBuilder() =
    member _.Return(value) = Success value
    member _.Bind(step, func) =
        match step with
        | Success value -> func value
        | Failed error -> Failed error
        | Pending continuation -> 
            Pending (fun () ->
                match continuation() with
                | Success value -> func value
                | Failed error -> Failed error
                | Pending nextContinuation -> Pending nextContinuation)
    
    member _.Delay(func) = Pending func
    member _.ReturnFrom(step) = step

let workflow = WorkflowBuilder()

type Order = {
    Id: string
    Amount: decimal
    Status: string
}

let validateOrder order =
    if order.Amount > 0m then
        Success { order with Status = "Validated" }
    else
        Failed "Invalid order amount"

let chargePayment order =
    // Simulate payment processing
    if order.Amount <= 1000m then
        Success { order with Status = "Paid" }
    else
        Failed "Payment failed - amount too large"

let shipOrder order =
    Success { order with Status = "Shipped" }

let sendConfirmation order =
    Success { order with Status = "Confirmed" }

let processOrder order =
    workflow {
        let! validated = validateOrder order
        let! charged = chargePayment validated
        let! shipped = shipOrder charged
        let! confirmed = sendConfirmation shipped
        return confirmed
    }

// Usage
let testOrder = { Id = "ORD-001"; Amount = 99.99m; Status = "New" }

let rec executeWorkflow step =
    match step with
    | Success result -> 
        printfn "Workflow completed: %A" result
        result
    | Failed error -> 
        printfn "Workflow failed: %s" error
        failwith error
    | Pending continuation ->
        printfn "Continuing workflow..."
        executeWorkflow (continuation())

let result = processOrder testOrder |> executeWorkflow
```

Workflow computation expressions model business processes with steps  
that can succeed, fail, or be deferred. This pattern is useful for  
complex business logic where operations may need to be retried,  
logged, or executed in specific sequences while maintaining clean  
separation between the workflow definition and execution strategy.  

## Parsing Combinator

Building parser combinators using computation expressions.  

```fsharp
type ParseResult<'a> =
    | Success of 'a * string
    | Failure of string

type Parser<'a> = Parser of (string -> ParseResult<'a>)

type ParserBuilder() =
    member _.Return(value) = Parser (fun input -> Success (value, input))
    member _.Bind(Parser parser, func) =
        Parser (fun input ->
            match parser input with
            | Success (value, remaining) ->
                let (Parser nextParser) = func value
                nextParser remaining
            | Failure error -> Failure error)

let parser = ParserBuilder()

let satisfyChar predicate =
    Parser (fun input ->
        if String.length input > 0 && predicate input.[0] then
            Success (input.[0], input.[1..])
        else
            Failure "Character does not satisfy predicate")

let parseChar c =
    satisfyChar (fun ch -> ch = c)

let parseDigit = satisfyChar System.Char.IsDigit
let parseLetter = satisfyChar System.Char.IsLetter

let parseMany (Parser p) =
    let rec loop acc input =
        match p input with
        | Success (value, remaining) -> loop (value::acc) remaining
        | Failure _ -> Success (List.rev acc, input)
    Parser (loop [])

let parseInt =
    parser {
        let! digits = parseMany parseDigit
        if List.isEmpty digits then
            return failwith "No digits found"
        else
            let digitStr = digits |> List.map string |> String.concat ""
            return int digitStr
    }

let parseEmail =
    parser {
        let! namePart = parseMany parseLetter
        let! _ = parseChar '@'
        let! domainPart = parseMany parseLetter
        let! _ = parseChar '.'
        let! extension = parseMany parseLetter
        
        let name = namePart |> List.map string |> String.concat ""
        let domain = domainPart |> List.map string |> String.concat ""
        let ext = extension |> List.map string |> String.concat ""
        
        return sprintf "%s@%s.%s" name domain ext
    }

// Usage
let runParser (Parser p) input = p input

match runParser parseInt "123abc" with
| Success (value, remaining) -> printfn "Parsed int: %d, remaining: %s" value remaining
| Failure error -> printfn "Parse error: %s" error

match runParser parseEmail "john@example.com" with
| Success (email, remaining) -> printfn "Parsed email: %s, remaining: %s" email remaining
| Failure error -> printfn "Parse error: %s" error
```

Parser combinators use computation expressions to build complex parsers  
from simple components. Each parser consumes input and returns either  
success with the parsed value and remaining input, or failure. The  
monadic composition allows building sophisticated parsers through  
simple, composable building blocks.  

## Event Sourcing

Implementing event sourcing patterns with computation expressions.  

```fsharp
open System

type Event =
    | AccountCreated of accountId: string * initialBalance: decimal
    | MoneyDeposited of accountId: string * amount: decimal
    | MoneyWithdrawn of accountId: string * amount: decimal

type Account = {
    Id: string
    Balance: decimal
    CreatedAt: DateTime
    LastModified: DateTime
}

type EventStore = {
    Events: Event list
}

type EventSourcingBuilder() =
    member _.Return(value) = fun store -> (value, store)
    member _.Bind(computation, func) =
        fun store ->
            let (value, newStore) = computation store
            let nextComputation = func value
            nextComputation newStore

let eventSourcing = EventSourcingBuilder()

let appendEvent event =
    fun store ->
        let newStore = { store with Events = event :: store.Events }
        ((), newStore)

let getEvents () =
    fun store -> (List.rev store.Events, store)

let applyEvent account event =
    match event with
    | AccountCreated (id, balance) ->
        { Id = id; Balance = balance; CreatedAt = DateTime.Now; LastModified = DateTime.Now }
    | MoneyDeposited (_, amount) ->
        { account with Balance = account.Balance + amount; LastModified = DateTime.Now }
    | MoneyWithdrawn (_, amount) ->
        { account with Balance = account.Balance - amount; LastModified = DateTime.Now }

let rebuildAccount accountId =
    eventSourcing {
        let! events = getEvents ()
        let relevantEvents = 
            events 
            |> List.filter (function
                | AccountCreated (id, _) -> id = accountId
                | MoneyDeposited (id, _) -> id = accountId
                | MoneyWithdrawn (id, _) -> id = accountId)
        
        let account = 
            relevantEvents
            |> List.fold applyEvent { Id = ""; Balance = 0m; CreatedAt = DateTime.MinValue; LastModified = DateTime.MinValue }
        
        return account
    }

let createAccount accountId initialBalance =
    eventSourcing {
        let event = AccountCreated (accountId, initialBalance)
        do! appendEvent event
        let! account = rebuildAccount accountId
        return account
    }

let deposit accountId amount =
    eventSourcing {
        let event = MoneyDeposited (accountId, amount)
        do! appendEvent event
        let! account = rebuildAccount accountId
        return account
    }

// Usage
let initialStore = { Events = [] }

let (account1, store1) = createAccount "ACC-001" 1000m initialStore
printfn "Created account: %A" account1

let (account2, store2) = deposit "ACC-001" 500m store1
printfn "After deposit: %A" account2

let (finalAccount, _) = rebuildAccount "ACC-001" store2
printfn "Rebuilt account: %A" finalAccount
```

Event sourcing computation expressions provide a clean way to manage  
event streams and state reconstruction. Each operation appends events  
to the store and can rebuild current state from the event history.  
This pattern ensures complete auditability and enables features like  
time travel debugging and temporal queries.  

## Resource Management

Implementing automatic resource management with computation expressions.  

```fsharp
open System
open System.IO

type Resource<'a> = {
    Value: 'a
    Dispose: unit -> unit
}

type ResourceBuilder() =
    member _.Return(value) = { Value = value; Dispose = fun () -> () }
    member _.Bind(resource, func) =
        let nextResource = func resource.Value
        { 
            Value = nextResource.Value
            Dispose = fun () -> 
                nextResource.Dispose()
                resource.Dispose()
        }
    member _.Using(disposable: #IDisposable, func) =
        try
            func disposable
        finally
            disposable.Dispose()

let resource = ResourceBuilder()

let createTempFile content =
    let path = Path.GetTempFileName()
    File.WriteAllText(path, content)
    { 
        Value = path
        Dispose = fun () -> 
            if File.Exists(path) then 
                File.Delete(path)
                printfn "Deleted temp file: %s" path
    }

let openFileReader path =
    let reader = new StreamReader(path)
    { 
        Value = reader
        Dispose = fun () -> 
            reader.Dispose()
            printfn "Closed file reader for: %s" path
    }

let processFile content =
    resource {
        let! tempPath = createTempFile content
        let! reader = openFileReader tempPath
        let fileContent = reader.ReadToEnd()
        return (tempPath, fileContent.Length)
    }

// Usage with automatic cleanup
let runWithCleanup computation =
    let result = computation
    result.Dispose()
    result.Value

let (path, length) = runWithCleanup (processFile "Hello, Resource Management!")
printfn "Processed file %s with length %d" path length
```

Resource management computation expressions ensure proper cleanup of  
resources through automatic disposal. The pattern composes resource  
acquisition and cleanup operations, guaranteeing that all resources  
are properly disposed even if exceptions occur. This is particularly  
useful for file I/O, database connections, and network resources.  

## Continuation Monad

Implementing continuation-passing style with computation expressions.  

```fsharp
type Cont<'a, 'r> = Cont of (('a -> 'r) -> 'r)

type ContBuilder() =
    member _.Return(value) = Cont (fun k -> k value)
    member _.Bind(Cont computation, func) =
        Cont (fun k ->
            computation (fun a ->
                let (Cont nextComputation) = func a
                nextComputation k))

let cont = ContBuilder()

let callCC func =
    Cont (fun k ->
        let escape = fun value -> Cont (fun _ -> k value)
        let (Cont computation) = func escape
        computation k)

let add x y = x + y
let multiply x y = x * y

let complexCalculation x y =
    cont {
        let! sum = cont { return add x y }
        let! product = cont { return multiply x y }
        return (sum, product)
    }

let calculationWithEscapeHatch x y =
    callCC (fun escape ->
        cont {
            if x < 0 || y < 0 then
                return! escape "Error: negative numbers not allowed"
            else
                let! sum = cont { return add x y }
                let! product = cont { return multiply x y }
                return sprintf "Sum: %d, Product: %d" sum product
        })

// Usage
let runCont (Cont computation) = computation id

let (sum, product) = runCont (complexCalculation 5 3)
printfn "Results: sum = %d, product = %d" sum product

let result1 = runCont (calculationWithEscapeHatch 4 6)
printfn "Calculation 1: %s" result1

let result2 = runCont (calculationWithEscapeHatch -1 5)
printfn "Calculation 2: %s" result2
```

Continuation monads enable powerful control flow patterns including  
early exit, backtracking, and exception handling. The callCC (call  
with current continuation) function provides escape hatches that can  
short-circuit computations. This pattern is useful for complex control  
flow scenarios where traditional exception handling is insufficient.  

## Trampoline Computation

Implementing stack-safe recursion using trampoline computation expressions.  

```fsharp
type Trampoline<'a> =
    | Done of 'a
    | Continue of (unit -> Trampoline<'a>)

type TrampolineBuilder() =
    member _.Return(value) = Done value
    member _.Bind(tramp, func) =
        match tramp with
        | Done value -> func value
        | Continue continuation -> Continue (fun () ->
            let next = continuation()
            match next with
            | Done value -> func value
            | Continue nextContinuation -> Continue nextContinuation)
    member _.Delay(func) = Continue func

let trampoline = TrampolineBuilder()

let rec runTrampoline computation =
    match computation with
    | Done value -> value
    | Continue continuation -> runTrampoline (continuation())

let factorial n =
    let rec factorialTail acc n =
        trampoline {
            if n <= 1 then
                return acc
            else
                return! Continue (fun () -> factorialTail (acc * n) (n - 1))
        }
    factorialTail 1 n

let fibonacci n =
    let rec fibTail a b count =
        trampoline {
            if count <= 0 then
                return a
            else
                return! Continue (fun () -> fibTail b (a + b) (count - 1))
        }
    fibTail 0 1 n

let mutualRecursion n =
    let rec isEven n =
        trampoline {
            if n = 0 then
                return true
            else
                return! Continue (fun () -> isOdd (n - 1))
        }
    and isOdd n =
        trampoline {
            if n = 0 then
                return false
            else
                return! Continue (fun () -> isEven (n - 1))
        }
    isEven n

// Usage
let fact10 = factorial 10 |> runTrampoline
printfn "10! = %d" fact10

let fib20 = fibonacci 20 |> runTrampoline
printfn "fibonacci(20) = %d" fib20

let isEven1000 = mutualRecursion 1000 |> runTrampoline
printfn "1000 is even: %b" isEven1000

// Test with large number to show stack safety
let largeFact = factorial 50 |> runTrampoline
printfn "50! = %A" largeFact
```

Trampoline computation expressions enable stack-safe recursion by  
converting recursive calls into iterative loops. Instead of growing  
the call stack, each recursive step returns a continuation that can  
be executed iteratively. This pattern prevents stack overflow errors  
for deep recursions while maintaining the clarity of recursive code.  

## Free Monad

Implementing the Free monad pattern for building DSLs with computation expressions.  

```fsharp
type Free<'f, 'a> =
    | Pure of 'a
    | Free of 'f

type FreeBuilder<'f>() =
    member _.Return(value) = Pure value
    member _.Bind(free, func) =
        match free with
        | Pure value -> func value
        | Free instruction -> Free instruction  // Simplified for demo

// Console DSL
type ConsoleInstruction<'a> =
    | WriteLine of string * (unit -> 'a)
    | ReadLine of (string -> 'a)

type ConsoleDSL<'a> = Free<ConsoleInstruction<ConsoleDSL<'a>>, 'a>

let console = FreeBuilder<ConsoleInstruction<ConsoleDSL<'a>>>()

let writeLine text =
    Free (WriteLine (text, fun () -> Pure ()))

let readLine () =
    Free (ReadLine (fun input -> Pure input))

// File DSL  
type FileInstruction<'a> =
    | ReadFile of string * (string -> 'a)
    | WriteFile of string * string * (unit -> 'a)
    | DeleteFile of string * (unit -> 'a)

type FileDSL<'a> = Free<FileInstruction<FileDSL<'a>>, 'a>

let file = FreeBuilder<FileInstruction<FileDSL<'a>>>()

let readFile path =
    Free (ReadFile (path, fun content -> Pure content))

let writeFile path content =
    Free (WriteFile (path, content, fun () -> Pure ()))

// Combined program using both DSLs
let interactiveFileProgram =
    // This is a conceptual example - actual implementation would need
    // more complex Free monad machinery
    async {
        printfn "Enter filename to read:"
        let filename = Console.ReadLine()
        
        try
            let content = System.IO.File.ReadAllText(filename)
            printfn "File content:"
            printfn "%s" content
            
            printfn "Enter new content to append:"
            let newContent = Console.ReadLine()
            let updatedContent = content + "\n" + newContent
            
            System.IO.File.WriteAllText(filename, updatedContent)
            printfn "File updated successfully!"
            
        with
        | ex -> printfn "Error: %s" ex.Message
    }

// Usage demonstration
let runInteractiveDemo() =
    // Create a test file
    let testFile = System.IO.Path.GetTempFileName()
    System.IO.File.WriteAllText(testFile, "Initial content")
    
    printfn "Demo file created at: %s" testFile
    printfn "Content: %s" (System.IO.File.ReadAllText(testFile))
    
    // Cleanup
    System.IO.File.Delete(testFile)
    printfn "Demo file deleted"

// Run the demo
runInteractiveDemo()
```

Free monads enable building domain-specific languages (DSLs) as  
composition of computation expressions. Each instruction type represents  
an operation in the DSL, and the Free monad provides the structure  
for composing these operations. This pattern separates the description  
of computations from their execution, enabling testing, optimization,  
and multiple interpretation strategies.  

## Zipper Computation

Implementing navigation through data structures using Zipper with computation expressions.  

```fsharp
type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

type Direction = Left | Right

type TreeZipper<'a> = {
    Focus: Tree<'a>
    Path: (Direction * Tree<'a>) list
}

type ZipperBuilder<'a>() =
    member _.Return(zipper) = Some zipper
    member _.Bind(option, func) =
        match option with
        | Some zipper -> func zipper
        | None -> None

let zipper<'a> = ZipperBuilder<'a>()

let createZipper tree = { Focus = tree; Path = [] }

let goLeft zipper =
    match zipper.Focus with
    | Node (left, right) -> 
        Some { Focus = left; Path = (Left, right) :: zipper.Path }
    | Leaf _ -> None

let goRight zipper =
    match zipper.Focus with
    | Node (left, right) ->
        Some { Focus = right; Path = (Right, left) :: zipper.Path }
    | Leaf _ -> None

let goUp zipper =
    match zipper.Path with
    | (Left, right) :: rest ->
        Some { Focus = Node (zipper.Focus, right); Path = rest }
    | (Right, left) :: rest ->
        Some { Focus = Node (left, zipper.Focus); Path = rest }
    | [] -> None

let updateFocus newValue zipper =
    { zipper with Focus = newValue }

let getFocus zipper = zipper.Focus

let navigateAndUpdate tree =
    let initialZipper = createZipper tree
    
    zipper {
        let! z1 = Some initialZipper
        let! z2 = goLeft z1
        let! z3 = goRight z2
        let updatedZ3 = updateFocus (Leaf "Updated!") z3
        let! z4 = goUp updatedZ3
        let! z5 = goUp z4
        return getFocus z5
    }

// Usage
let sampleTree = 
    Node (
        Node (Leaf "A", Node (Leaf "B", Leaf "C")),
        Leaf "D"
    )

let result = navigateAndUpdate sampleTree
match result with
| Some updatedTree -> printfn "Updated tree: %A" updatedTree
| None -> printfn "Navigation failed"

// Helper function to print tree structure
let rec printTree depth tree =
    let indent = String.replicate depth "  "
    match tree with
    | Leaf value -> printfn "%sLeaf: %s" indent value
    | Node (left, right) ->
        printfn "%sNode:" indent
        printTree (depth + 1) left
        printTree (depth + 1) right

printfn "\nOriginal tree:"
printTree 0 sampleTree

match result with
| Some updatedTree ->
    printfn "\nUpdated tree:"
    printTree 0 updatedTree
| None -> ()
```

Zipper computation expressions provide functional navigation and editing  
of tree-like data structures. The zipper maintains both the current  
focus and the path back to the root, enabling efficient navigation  
and local updates without reconstructing entire data structures.  
This pattern is essential for functional editors and tree transformations.  

## Identity Monad

Implementing the simplest monad for understanding monadic patterns.  

```fsharp
type Identity<'a> = Identity of 'a

type IdentityBuilder() =
    member _.Return(value) = Identity value
    member _.Bind(Identity value, func) = func value
    member _.ReturnFrom(identity) = identity

let identity = IdentityBuilder()

let addOne x = Identity (x + 1)
let multiplyBy2 x = Identity (x * 2)
let toString x = Identity (string x)

let chainOperations x =
    identity {
        let! step1 = addOne x
        let! step2 = multiplyBy2 step1
        let! step3 = toString step2
        return sprintf "Final result: %s" step3
    }

let computeWithLogging x =
    identity {
        let! initial = Identity x
        printfn "Starting with: %d" initial
        
        let! doubled = Identity (initial * 2)
        printfn "After doubling: %d" doubled
        
        let! squared = Identity (doubled * doubled)
        printfn "After squaring: %d" squared
        
        return squared
    }

// Usage
let (Identity result1) = chainOperations 5
printfn "%s" result1

let (Identity result2) = computeWithLogging 3
printfn "Final computation result: %d" result2
```

The Identity monad is the simplest monad that just wraps values without  
adding any computational context. While it seems trivial, it's valuable  
for understanding monadic patterns and serves as a baseline for more  
complex monads. It demonstrates the core monadic operations without  
additional complexity from side effects or error handling.  

## Logger Monad

Implementing computation expressions with automatic logging capabilities.  

```fsharp
type Logger<'a> = Logger of ('a * string list)

type LoggerBuilder() =
    member _.Return(value) = Logger (value, [])
    member _.Bind(Logger (value, logs1), func) =
        let Logger (nextValue, logs2) = func value
        Logger (nextValue, logs1 @ logs2)
    member _.Zero() = Logger ((), [])

let logger = LoggerBuilder()

let log message value =
    Logger (value, [message])

let divide x y =
    logger {
        let! _ = log (sprintf "Dividing %f by %f" x y) ()
        if y = 0.0 then
            let! _ = log "Warning: Division by zero!" ()
            return nan
        else
            let result = x / y
            let! _ = log (sprintf "Division result: %f" result) ()
            return result
    }

let complexMathOperation x y z =
    logger {
        let! _ = log (sprintf "Starting complex operation with %f, %f, %f" x y z) ()
        
        let! step1 = divide x y
        let! step2 = divide step1 z
        let! step3 = log (sprintf "Adding 10 to %f" step2) (step2 + 10.0)
        
        let! _ = log (sprintf "Final result: %f" step3) ()
        return step3
    }

let runLogger (Logger (value, logs)) =
    printfn "Execution logs:"
    logs |> List.iteri (fun i log -> printfn "  %d. %s" (i+1) log)
    printfn "Final result: %A" value
    value

// Usage
let result1 = divide 10.0 2.0
runLogger result1 |> ignore

printfn "\nComplex operation:"
let result2 = complexMathOperation 100.0 5.0 2.0
runLogger result2 |> ignore

printfn "\nOperation with division by zero:"
let result3 = complexMathOperation 50.0 0.0 3.0
runLogger result3 |> ignore
```

The Logger monad automatically accumulates log messages throughout  
computation chains. Each operation can add log entries that are  
collected and preserved through monadic binding. This pattern is  
invaluable for debugging and auditing complex computations without  
cluttering the core logic with explicit logging calls.  

## Supply Monad

Implementing computation expressions that consume resources from a supply.  

```fsharp
type Supply<'s, 'a> = Supply of ('s list -> ('a * 's list) option)

type SupplyBuilder<'s>() =
    member _.Return(value) = Supply (fun supply -> Some (value, supply))
    member _.Bind(Supply computation, func) =
        Supply (fun supply ->
            match computation supply with
            | Some (value, remainingSupply) ->
                let (Supply nextComputation) = func value
                nextComputation remainingSupply
            | None -> None)

let supply<'s> = SupplyBuilder<'s>()

let takeOne<'s> () : Supply<'s, 's option> =
    Supply (fun items ->
        match items with
        | [] -> Some (None, [])
        | head :: tail -> Some (Some head, tail))

let peek<'s> () : Supply<'s, 's option> =
    Supply (fun items ->
        match items with
        | [] -> Some (None, items)
        | head :: _ -> Some (Some head, items))

let runSupply (Supply computation) initialSupply = 
    computation initialSupply

// Generate unique IDs from a supply
let generateUniqueIds count =
    supply {
        let mutable ids = []
        for _ in 1..count do
            let! maybeId = takeOne()
            match maybeId with
            | Some id -> ids <- id :: ids
            | None -> printfn "Warning: Supply exhausted"
        return List.rev ids
    }

// Process items with resource consumption
let processWithResources items =
    supply {
        let mutable results = []
        for item in items do
            let! resource = takeOne()
            match resource with
            | Some res ->
                let processed = sprintf "Processed %s with %s" item res
                results <- processed :: results
            | None ->
                let processed = sprintf "Processed %s with NO RESOURCE" item
                results <- processed :: results
        return List.rev results
    }

// Usage
let idSupply = [1..10]
let (Some (generatedIds, remainingIds)) = runSupply (generateUniqueIds 5) idSupply
printfn "Generated IDs: %A" generatedIds
printfn "Remaining IDs: %A" remainingIds

let resourceSupply = ["CPU"; "Memory"; "Disk"; "Network"]
let itemsToProcess = ["Task1"; "Task2"; "Task3"; "Task4"; "Task5"]
let (Some (processedItems, remainingResources)) = 
    runSupply (processWithResources itemsToProcess) resourceSupply
printfn "\nProcessed items:"
processedItems |> List.iter (printfn "  %s")
printfn "Remaining resources: %A" remainingResources
```

The Supply monad manages finite resources that get consumed during  
computation. Each operation can request resources from the supply,  
and the monad tracks what remains available. This pattern is useful  
for resource allocation, unique ID generation, and any scenario where  
you need to distribute limited resources across multiple operations.  

Computation expressions in F# provide a powerful and flexible way to  
compose and sequence computations while maintaining clean, readable code.  
From basic async operations to advanced patterns like free monads and  
zippers, they enable elegant solutions to complex programming challenges  
while preserving functional programming principles and type safety.  

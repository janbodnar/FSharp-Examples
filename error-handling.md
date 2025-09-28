# F# Error Handling

F# provides several mechanisms for error handling including Option types,  
Result types, and traditional exception handling. These approaches enable  
safe, composable error handling patterns that make code more reliable and  
maintainable.  

## Basic Option type

The Option type represents values that might or might not exist, providing  
a safer alternative to null references.  

```F#
let tryDivide x y =
    if y = 0 then None
    else Some (x / y)

let result1 = tryDivide 10 2
let result2 = tryDivide 10 0

match result1 with
| Some value -> printfn "Result: %d" value
| None -> printfn "Division failed"

match result2 with
| Some value -> printfn "Result: %d" value
| None -> printfn "Division failed"
```

The Option type uses Some to wrap valid values and None to represent  
absence of a value. Pattern matching extracts values safely without  
null reference exceptions.  

## Option with default values

Use Option.defaultValue or Option.defaultWith to provide fallback values  
when dealing with None cases.  

```F#
let numbers = [Some 1; None; Some 3; None; Some 5]

let withDefaults = 
    numbers 
    |> List.map (Option.defaultValue 0)

printfn "With defaults: %A" withDefaults

let withDefaultFunc = 
    numbers 
    |> List.map (Option.defaultWith (fun () -> System.Random().Next(1, 100)))

printfn "With default function: %A" withDefaultFunc
```

Option.defaultValue provides a constant fallback, while defaultWith accepts  
a function that generates the default value when needed.  

## Option mapping and binding

Transform Option values using map and bind operations for composable  
error handling chains.  

```F#
let tryParseInt (s: string) =
    match System.Int32.TryParse s with
    | true, value -> Some value
    | false, _ -> None

let trySquareRoot x =
    if x >= 0.0 then Some (sqrt x)
    else None

let processInput input =
    input
    |> tryParseInt
    |> Option.map float
    |> Option.bind trySquareRoot
    |> Option.map (sprintf "Square root: %.2f")
    |> Option.defaultValue "Invalid input"

printfn "%s" (processInput "16")
printfn "%s" (processInput "-4")
printfn "%s" (processInput "abc")
```

Option.map transforms the value inside Some, while Option.bind chains  
operations that return Option types. This creates a pipeline that stops  
at the first None.  

## Basic Result type

The Result type represents operations that can succeed (Ok) or fail (Error),  
providing more detailed error information than Option.  

```F#
type DivisionError = DivideByZero | InvalidInput

let divide x y =
    match y with
    | 0 -> Error DivideByZero
    | _ when x < 0 || y < 0 -> Error InvalidInput
    | _ -> Ok (x / y)

let result1 = divide 10 2
let result2 = divide 10 0
let result3 = divide -5 2

let printResult result =
    match result with
    | Ok value -> printfn "Success: %d" value
    | Error DivideByZero -> printfn "Error: Cannot divide by zero"
    | Error InvalidInput -> printfn "Error: Negative numbers not allowed"

printResult result1
printResult result2
printResult result3
```

Result types use custom error types to provide specific error information,  
making error handling more explicit and type-safe than exceptions.  

## Result mapping and error handling

Transform Result values and handle errors using map, mapError, and bind  
operations for robust error propagation.  

```F#
type ValidationError = 
    | EmptyString 
    | TooShort 
    | InvalidFormat

let validateEmail (email: string) =
    if System.String.IsNullOrWhiteSpace email then Error EmptyString
    elif email.Length < 5 then Error TooShort
    elif not (email.Contains "@") then Error InvalidFormat
    else Ok email

let formatEmail (email: string) =
    email.ToLower().Trim()

let processEmail (input: string) =
    input
    |> validateEmail
    |> Result.map formatEmail
    |> Result.map (sprintf "Processed email: %s")
    |> Result.mapError (function
        | EmptyString -> "Email cannot be empty"
        | TooShort -> "Email too short"
        | InvalidFormat -> "Invalid email format")

let printEmailResult result =
    match result with
    | Ok msg -> printfn "%s" msg
    | Error msg -> printfn "Error: %s" msg

printEmailResult (processEmail "John@Example.COM")
printEmailResult (processEmail "abc")
printEmailResult (processEmail "")
```

Result.map transforms successful values, Result.mapError transforms error  
values, and pattern matching on the error type provides specific handling.  

## Exception handling with try-with

Traditional exception handling using try-with expressions for operations  
that might throw exceptions.  

```F#
open System

let safeDivision x y =
    try
        let result = x / y
        Ok result
    with
    | :? DivideByZeroException -> Error "Division by zero"
    | :? OverflowException -> Error "Arithmetic overflow"
    | ex -> Error (sprintf "Unexpected error: %s" ex.Message)

let results = [
    safeDivision 10 2
    safeDivision 10 0
    safeDivision Int32.MaxValue 1
]

results |> List.iter (function
    | Ok value -> printfn "Result: %d" value
    | Error msg -> printfn "Error: %s" msg)
```

The try-with expression catches specific exception types and converts them  
to Result values, providing a bridge between exception-based and  
functional error handling.  

## Pattern matching for error handling

Use pattern matching with guards and nested patterns for complex error  
handling scenarios.  

```F#
open System

type ParseResult = 
    | Success of int
    | ParseFailure of string
    | OutOfRange of int * int * int

let parseWithRange min max input =
    match Int32.TryParse input with
    | false, _ -> ParseFailure input
    | true, value when value < min -> OutOfRange (value, min, max)
    | true, value when value > max -> OutOfRange (value, min, max)
    | true, value -> Success value

let handleParseResult result =
    match result with
    | Success value -> 
        printfn "Successfully parsed: %d" value
    | ParseFailure input -> 
        printfn "Could not parse '%s' as integer" input
    | OutOfRange (value, min, max) -> 
        printfn "Value %d is outside range [%d, %d]" value min max

let inputs = ["42"; "abc"; "150"; "-10"]
let results = inputs |> List.map (parseWithRange 0 100)
results |> List.iter handleParseResult
```

Pattern matching with custom discriminated unions provides expressive  
error handling that captures specific error conditions with associated data.  

## Combining Option and Result

Convert between Option and Result types and combine them in error handling  
pipelines for maximum flexibility.  

```F#
let optionToResult error option =
    match option with
    | Some value -> Ok value
    | None -> Error error

let resultToOption result =
    match result with
    | Ok value -> Some value
    | Error _ -> None

let findUser userId =
    if userId > 0 && userId <= 100 then Some (sprintf "User_%d" userId)
    else None

let validateUserId userId =
    if userId > 0 then Ok userId
    else Error "User ID must be positive"

let processUser userId =
    userId
    |> validateUserId
    |> Result.bind (findUser >> optionToResult "User not found")
    |> Result.map (sprintf "Found: %s")

let testUsers = [-1; 0; 50; 150]
testUsers |> List.iter (fun id ->
    match processUser id with
    | Ok msg -> printfn "%s" msg
    | Error err -> printfn "Error: %s" err)
```

Converting between Option and Result types enables flexible composition  
of different error handling patterns within the same pipeline.  

## Railway Oriented Programming

Implement railway oriented programming patterns for elegant error handling  
composition and flow control.  

```F#
type Email = Email of string
type Age = Age of int
type User = { Email: Email; Age: Age }

type ValidationError = 
    | InvalidEmail of string
    | InvalidAge of int

let validateEmail email =
    if email.Contains "@" && email.Length > 5 then Ok (Email email)
    else Error (InvalidEmail email)

let validateAge age =
    if age >= 0 && age <= 120 then Ok (Age age)
    else Error (InvalidAge age)

let createUser email age =
    match validateEmail email, validateAge age with
    | Ok validEmail, Ok validAge -> Ok { Email = validEmail; Age = validAge }
    | Error emailErr, Ok _ -> Error emailErr
    | Ok _, Error ageErr -> Error ageErr
    | Error emailErr, Error _ -> Error emailErr  // Return first error

let (>>=) result func =
    match result with
    | Ok value -> func value
    | Error err -> Error err

let createUserPipeline email age =
    Ok (email, age)
    >>= fun (e, a) -> validateEmail e |> Result.map (fun ve -> (ve, a))
    >>= fun (ve, a) -> validateAge a |> Result.map (fun va -> { Email = ve; Age = va })

let testCases = [
    ("john@example.com", 30)
    ("invalid-email", 25)
    ("jane@example.com", -5)
    ("bad", -10)
]

testCases |> List.iter (fun (email, age) ->
    match createUser email age with
    | Ok user -> printfn "Created user: %A" user
    | Error (InvalidEmail e) -> printfn "Invalid email: %s" e
    | Error (InvalidAge a) -> printfn "Invalid age: %d" a)
```

Railway oriented programming treats success and failure as separate tracks,  
with operations that either continue on the success track or switch to  
the failure track.  

## Custom error types

Define custom error types using discriminated unions to provide rich  
error information and type-safe error handling.  

```F#
open System.IO

type FileError = 
    | FileNotFound of string
    | AccessDenied of string
    | InvalidFormat of string * string

type ProcessingError =
    | FileError of FileError
    | ParseError of int * string
    | ValidationError of string

let readFile fileName =
    try
        let content = File.ReadAllText fileName
        Ok content
    with
    | :? FileNotFoundException -> Error (FileError (FileNotFound fileName))
    | :? UnauthorizedAccessException -> Error (FileError (AccessDenied fileName))
    | ex -> Error (FileError (InvalidFormat (fileName, ex.Message)))

let parseLines content =
    try
        let lines = content.Split('\n')
        let numbers = lines |> Array.mapi (fun i line ->
            match System.Int32.TryParse(line.Trim()) with
            | true, num -> Ok num
            | false, _ -> Error (ParseError (i + 1, line)))
        
        let errors = numbers |> Array.choose (function Error e -> Some e | Ok _ -> None)
        if errors.Length > 0 then Error errors.[0]
        else Ok (numbers |> Array.map (function Ok n -> n | Error _ -> 0))
    with
    | ex -> Error (ValidationError ex.Message)

let processFile fileName =
    readFile fileName
    |> Result.bind parseLines

let handleError = function
    | FileError (FileNotFound f) -> sprintf "File not found: %s" f
    | FileError (AccessDenied f) -> sprintf "Access denied: %s" f
    | FileError (InvalidFormat (f, msg)) -> sprintf "Invalid format in %s: %s" f msg
    | ParseError (line, content) -> sprintf "Parse error at line %d: %s" line content
    | ValidationError msg -> sprintf "Validation error: %s" msg

// Create a test file
File.WriteAllText("numbers.txt", "10\n20\nabc\n30")

match processFile "numbers.txt" with
| Ok numbers -> printfn "Numbers: %A" numbers
| Error err -> printfn "Error: %s" (handleError err)

match processFile "missing.txt" with
| Ok numbers -> printfn "Numbers: %A" numbers
| Error err -> printfn "Error: %s" (handleError err)
```

Custom error types provide hierarchical error information that can be  
pattern matched for specific error handling while maintaining type safety.  

## Async error handling

Handle errors in asynchronous computations using Async.Catch and Result  
types for robust async error handling.  

```F#
open System
open System.Net.Http
open System.Threading.Tasks

let httpClient = new HttpClient()

let asyncTryDownload url = async {
    try
        let! response = httpClient.GetStringAsync(url) |> Async.AwaitTask
        return Ok response
    with
    | :? HttpRequestException as ex -> return Error (sprintf "HTTP error: %s" ex.Message)
    | :? TaskCanceledException -> return Error "Request timed out"
    | ex -> return Error (sprintf "Unexpected error: %s" ex.Message)
}

let processUrls urls = async {
    let! results = urls |> List.map asyncTryDownload |> Async.Parallel
    return results |> Array.toList
}

let urls = [
    "https://httpbin.org/status/200"
    "https://httpbin.org/status/404"
    "https://invalid-url-that-does-not-exist.com"
]

async {
    let! results = processUrls urls
    results |> List.iteri (fun i result ->
        match result with
        | Ok content -> printfn "URL %d: Success (%d chars)" i content.Length
        | Error msg -> printfn "URL %d: %s" i msg)
} |> Async.RunSynchronously

httpClient.Dispose()
```

Async error handling combines the Async workflow with Result types to  
provide safe asynchronous operations that capture and propagate errors  
without throwing exceptions.  

## Error accumulation

Accumulate multiple validation errors instead of stopping at the first  
error, useful for form validation and data processing.  

```F#
type ValidationResult<'T> = Result<'T, string list>

let validateRequired fieldName value =
    if String.IsNullOrWhiteSpace value then Error [sprintf "%s is required" fieldName]
    else Ok value

let validateLength fieldName minLength value =
    if String.length value < minLength then 
        Error [sprintf "%s must be at least %d characters" fieldName minLength]
    else Ok value

let validateEmail email =
    if email.Contains "@" then Ok email
    else Error ["Invalid email format"]

let combineValidations validations =
    let errors = validations |> List.collect (function Error errs -> errs | Ok _ -> [])
    if List.isEmpty errors then 
        Ok (validations |> List.map (function Ok v -> v | Error _ -> ""))
    else Error errors

type Person = { Name: string; Email: string; Password: string }

let validatePerson name email password =
    let nameResult = 
        validateRequired "Name" name
        |> Result.bind (validateLength "Name" 2)
    
    let emailResult = 
        validateRequired "Email" email
        |> Result.bind validateEmail
    
    let passwordResult = 
        validateRequired "Password" password
        |> Result.bind (validateLength "Password" 6)

    match nameResult, emailResult, passwordResult with
    | Ok n, Ok e, Ok p -> Ok { Name = n; Email = e; Password = p }
    | _ -> 
        let allErrors = [nameResult; emailResult; passwordResult]
                       |> List.collect (function Error errs -> errs | Ok _ -> [])
        Error allErrors

let testData = [
    ("John", "john@example.com", "secret123")
    ("", "invalid-email", "abc")
    ("J", "jane@example.com", "")
]

testData |> List.iter (fun (name, email, password) ->
    match validatePerson name email password with
    | Ok person -> printfn "Valid person: %A" person
    | Error errors -> 
        printfn "Validation errors:"
        errors |> List.iter (printfn "  - %s"))
```

Error accumulation collects all validation errors instead of short-circuiting  
on the first error, providing better user experience in validation scenarios.  

## Integration with .NET exceptions

Bridge between F# error handling and .NET exception handling for  
interoperability with existing .NET libraries.  

```F#
open System
open System.IO

let tryExecute f =
    try
        Ok (f ())
    with
    | ex -> Error ex

let executeWithRetry maxRetries f =
    let rec loop attempt =
        match tryExecute f with
        | Ok result -> Ok result
        | Error ex when attempt < maxRetries ->
            printfn "Attempt %d failed: %s" attempt ex.Message
            System.Threading.Thread.Sleep(1000)
            loop (attempt + 1)
        | Error ex -> Error ex
    loop 1

let riskyOperation () =
    let random = Random()
    let value = random.Next(1, 5)
    if value = 1 then failwith "Random failure occurred"
    else sprintf "Success with value %d" value

let handleOperationResult result =
    match result with
    | Ok value -> printfn "Operation succeeded: %s" value
    | Error ex -> printfn "Operation failed after retries: %s" ex.Message

// Test the retry mechanism
for i in 1..3 do
    printfn "\nTest run %d:" i
    executeWithRetry 3 riskyOperation |> handleOperationResult
```

Integration patterns allow gradual adoption of functional error handling  
while maintaining compatibility with exception-based .NET code.  

## File operations with error handling

Robust file operations using Result types for comprehensive error handling  
in I/O scenarios.  

```F#
open System.IO
open System.Text

type FileOperationError =
    | FileNotFound of string
    | DirectoryNotFound of string  
    | AccessDenied of string
    | InvalidPath of string
    | UnknownError of string

let safeFileOperation operation fileName =
    try
        Ok (operation fileName)
    with
    | :? FileNotFoundException -> Error (FileNotFound fileName)
    | :? DirectoryNotFoundException -> Error (DirectoryNotFound (Path.GetDirectoryName fileName))
    | :? UnauthorizedAccessException -> Error (AccessDenied fileName)
    | :? ArgumentException -> Error (InvalidPath fileName)
    | ex -> Error (UnknownError ex.Message)

let safeReadAllText = safeFileOperation File.ReadAllText
let safeWriteAllText fileName content = safeFileOperation (fun f -> File.WriteAllText(f, content)) fileName

let processTextFile inputFile outputFile transform =
    safeReadAllText inputFile
    |> Result.map transform
    |> Result.bind (safeWriteAllText outputFile)

let upperCaseTransform text = text.ToUpper()

let handleFileError = function
    | FileNotFound f -> sprintf "File not found: %s" f
    | DirectoryNotFound d -> sprintf "Directory not found: %s" d
    | AccessDenied f -> sprintf "Access denied to file: %s" f
    | InvalidPath f -> sprintf "Invalid file path: %s" f
    | UnknownError msg -> sprintf "Unknown error: %s" msg

// Create test file
File.WriteAllText("input.txt", "hello world\nthis is a test\n")

match processTextFile "input.txt" "output.txt" upperCaseTransform with
| Ok _ -> 
    printfn "File processed successfully"
    printfn "Output: %s" (File.ReadAllText "output.txt")
| Error err -> printfn "Error: %s" (handleFileError err)

// Test with non-existent file
match processTextFile "missing.txt" "output2.txt" upperCaseTransform with
| Ok _ -> printfn "File processed successfully"
| Error err -> printfn "Error: %s" (handleFileError err)
```

File operations demonstrate real-world error handling where multiple  
failure modes must be handled gracefully with appropriate error messages.  

## Parsing with error recovery

Implement parsing with error recovery using Result types to handle  
malformed input gracefully.  

```F#
open System

type ParseError = 
    | InvalidNumber of string * int
    | MissingValue of int
    | InvalidFormat of string

type ConfigValue = 
    | StringValue of string
    | NumberValue of int
    | BoolValue of bool

let parseConfigLine lineNumber (line: string) =
    if String.IsNullOrWhiteSpace line then Error (MissingValue lineNumber)
    else
        let parts = line.Split('=')
        if parts.Length <> 2 then Error (InvalidFormat line)
        else
            let key = parts.[0].Trim()
            let value = parts.[1].Trim()
            
            // Try to parse as different types
            match Int32.TryParse value with
            | true, num -> Ok (key, NumberValue num)
            | false, _ ->
                match Boolean.TryParse value with
                | true, bool -> Ok (key, BoolValue bool)
                | false, _ -> Ok (key, StringValue value)

let parseConfig content =
    let lines = content.Split('\n') |> Array.map (fun s -> s.Trim())
    let results = lines |> Array.mapi (fun i line -> 
        if String.IsNullOrWhiteSpace line then Ok None
        else parseConfigLine (i + 1) line |> Result.map Some)
    
    let errors = results |> Array.choose (function Error e -> Some e | Ok _ -> None)
    let values = results |> Array.choose (function Ok (Some v) -> Some v | _ -> None)
    
    if errors.Length > 0 then Error (Array.toList errors)
    else Ok (Map.ofArray values)

let configContent = """
name=MyApp
version=1.2.3
port=8080
debug=true
timeout=30

invalid line without equals
database_url=postgresql://localhost/mydb
"""

match parseConfig configContent with
| Ok config ->
    printfn "Parsed configuration:"
    config |> Map.iter (fun key value ->
        let valueStr = match value with
                      | StringValue s -> sprintf "\"%s\"" s
                      | NumberValue n -> string n
                      | BoolValue b -> string b
        printfn "  %s = %s" key valueStr)
| Error errors ->
    printfn "Parsing errors:"
    errors |> List.iter (function
        | InvalidNumber (s, line) -> printfn "  Line %d: Invalid number '%s'" line s
        | MissingValue line -> printfn "  Line %d: Missing value" line
        | InvalidFormat line -> printfn "  Invalid format: '%s'" line)
```

Parsing with error recovery demonstrates how to collect and report multiple  
parsing errors while still extracting valid data where possible.  

## Web request error handling

Handle HTTP requests with comprehensive error handling for network  
operations and API calls.  

```F#
open System
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks

type HttpError = 
    | NetworkError of string
    | HttpStatusError of int * string
    | JsonParseError of string
    | Timeout

let httpClient = new HttpClient()
httpClient.Timeout <- TimeSpan.FromSeconds(10.0)

let safeHttpGet url = async {
    try
        let! response = httpClient.GetAsync(url) |> Async.AwaitTask
        if response.IsSuccessStatusCode then
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return Ok content
        else
            return Error (HttpStatusError (int response.StatusCode, response.ReasonPhrase))
    with
    | :? TaskCanceledException -> return Error Timeout
    | :? HttpRequestException as ex -> return Error (NetworkError ex.Message)
    | ex -> return Error (NetworkError ex.Message)
}

let parseJson<'T> json =
    try
        let result = JsonSerializer.Deserialize<'T>(json)
        Ok result
    with
    | ex -> Error (JsonParseError ex.Message)

type ApiResponse = { message: string; status: string }

let fetchApiData url = async {
    let! httpResult = safeHttpGet url
    return httpResult |> Result.bind parseJson<ApiResponse>
}

let handleHttpError = function
    | NetworkError msg -> sprintf "Network error: %s" msg
    | HttpStatusError (code, reason) -> sprintf "HTTP %d: %s" code reason
    | JsonParseError msg -> sprintf "JSON parsing error: %s" msg
    | Timeout -> "Request timed out"

let testUrls = [
    "https://httpbin.org/json"
    "https://httpbin.org/status/404"
    "https://invalid-url.example.com"
]

for url in testUrls do
    printfn "\nTesting URL: %s" url
    async {
        let! result = fetchApiData url
        match result with
        | Ok data -> printfn "Success: %A" data
        | Error err -> printfn "Error: %s" (handleHttpError err)
    } |> Async.RunSynchronously

httpClient.Dispose()
```

Web request error handling demonstrates real-world scenarios where network  
failures, HTTP errors, and data parsing errors must all be handled  
appropriately.  

## Database operations with error handling

Simulate database operations with comprehensive error handling for data  
access scenarios.  

```F#
open System
open System.Collections.Generic

type DatabaseError = 
    | ConnectionFailed of string
    | QueryFailed of string * string
    | RecordNotFound of int
    | DuplicateKey of string
    | ValidationFailed of string list

type User = { Id: int; Name: string; Email: string }

type Database() =
    let users = Dictionary<int, User>()
    let mutable isConnected = false
    
    member _.Connect() =
        // Simulate occasional connection failures
        if Random().Next(1, 10) > 8 then Error (ConnectionFailed "Database unavailable")
        else 
            isConnected <- true
            Ok ()
    
    member _.FindUser(id: int) =
        if not isConnected then Error (ConnectionFailed "Not connected")
        elif users.ContainsKey(id) then Ok users.[id]
        else Error (RecordNotFound id)
    
    member _.SaveUser(user: User) =
        if not isConnected then Error (ConnectionFailed "Not connected")
        elif user.Id > 0 && users.ContainsKey(user.Id) then 
            Error (DuplicateKey (sprintf "User ID %d already exists" user.Id))
        elif String.IsNullOrWhiteSpace user.Name then
            Error (ValidationFailed ["Name is required"])
        elif not (user.Email.Contains "@") then
            Error (ValidationFailed ["Invalid email format"])
        else
            users.[user.Id] <- user
            Ok user

let performDatabaseOperation operation =
    let db = Database()
    db.Connect() |> Result.bind (fun _ -> operation db)

let handleDatabaseError = function
    | ConnectionFailed msg -> sprintf "Connection failed: %s" msg
    | QueryFailed (query, msg) -> sprintf "Query failed '%s': %s" query msg
    | RecordNotFound id -> sprintf "Record with ID %d not found" id
    | DuplicateKey msg -> sprintf "Duplicate key: %s" msg
    | ValidationFailed errors -> sprintf "Validation failed: %s" (String.concat ", " errors)

let testUsers = [
    { Id = 1; Name = "John Doe"; Email = "john@example.com" }
    { Id = 2; Name = ""; Email = "jane@example.com" }  // Invalid: empty name
    { Id = 3; Name = "Bob Smith"; Email = "invalid-email" }  // Invalid: bad email
]

// Test saving users
testUsers |> List.iter (fun user ->
    match performDatabaseOperation (fun db -> db.SaveUser(user)) with
    | Ok savedUser -> printfn "Saved user: %A" savedUser
    | Error err -> printfn "Error saving user: %s" (handleDatabaseError err))

// Test finding users
[1; 2; 99] |> List.iter (fun id ->
    match performDatabaseOperation (fun db -> db.FindUser(id)) with
    | Ok user -> printfn "Found user: %A" user
    | Error err -> printfn "Error finding user: %s" (handleDatabaseError err))
```

Database error handling showcases how to handle connection failures,  
validation errors, and data integrity issues in a type-safe manner.  

## Computation expressions for error handling

Create custom computation expressions to simplify error handling workflows  
and make code more readable.  

```F#
type ResultBuilder() =
    member _.Return(x) = Ok x
    member _.Bind(x, f) = Result.bind f x
    member _.ReturnFrom(x) = x
    member _.Zero() = Ok ()
    
    member _.TryWith(body, handler) =
        try body()
        with ex -> handler ex
    
    member _.TryFinally(body, compensation) =
        try body()
        finally compensation()

let result = ResultBuilder()

type ValidationError = string

let validatePositive name value =
    if value > 0 then Ok value
    else Error (sprintf "%s must be positive" name)

let validateRange name min max value =
    if value >= min && value <= max then Ok value
    else Error (sprintf "%s must be between %d and %d" name min max)

let calculateTotal price quantity discount = result {
    let! validPrice = validatePositive "Price" price
    let! validQuantity = validatePositive "Quantity" quantity
    let! validDiscount = validateRange "Discount" 0 100 discount
    
    let subtotal = validPrice * validQuantity
    let discountAmount = subtotal * validDiscount / 100
    let total = subtotal - discountAmount
    
    return {| Subtotal = subtotal; Discount = discountAmount; Total = total |}
}

let testOrders = [
    (10, 5, 10)    // Valid order
    (-5, 3, 10)    // Invalid: negative price
    (10, 0, 10)    // Invalid: zero quantity
    (10, 5, 150)   // Invalid: discount too high
]

testOrders |> List.iter (fun (price, qty, discount) ->
    match calculateTotal price qty discount with
    | Ok order -> printfn "Order total: %A" order
    | Error msg -> printfn "Order error: %s" msg)

// Async computation expression example
type AsyncResultBuilder() =
    member _.Return(x) = async { return Ok x }
    member _.Bind(x, f) = async {
        let! result = x
        match result with
        | Ok value -> return! f value
        | Error err -> return Error err
    }
    member _.ReturnFrom(x) = x

let asyncResult = AsyncResultBuilder()

let asyncValidatePositive name value = async {
    // Simulate async validation (e.g., database check)
    do! Async.Sleep 100
    return validatePositive name value
}

let asyncCalculateTotal price quantity = asyncResult {
    let! validPrice = asyncValidatePositive "Price" price
    let! validQuantity = asyncValidatePositive "Quantity" quantity
    return validPrice * validQuantity
}

async {
    let! result = asyncCalculateTotal 10 5
    match result with
    | Ok total -> printfn "Async total: %d" total
    | Error msg -> printfn "Async error: %s" msg
} |> Async.RunSynchronously
```

Computation expressions provide a clean, readable syntax for chaining  
operations that might fail, making error handling code more maintainable  
and expressive.  

## Error handling best practices

Demonstrate best practices for error handling including error aggregation,  
logging, and recovery strategies.  

```F#
open System
open System.IO

type LogLevel = Info | Warning | Error | Critical

type Logger() =
    member _.Log level message =
        let timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        let levelStr = match level with
                      | Info -> "INFO"
                      | Warning -> "WARN"  
                      | Error -> "ERROR"
                      | Critical -> "CRIT"
        printfn "[%s] %s: %s" timestamp levelStr message

let logger = Logger()

type BusinessError = 
    | InvalidInput of string
    | ProcessingFailed of string
    | SystemError of exn

let logAndReturn level message error =
    logger.Log level message
    Error error

let withLogging operation errorMessage =
    try
        operation()
    with
    | ex -> 
        logger.Log Error (sprintf "%s: %s" errorMessage ex.Message)
        Error (SystemError ex)

let processBusinessRule input = result {
    do! if String.IsNullOrWhiteSpace input then 
            Error (InvalidInput "Input cannot be empty")
        else Ok ()
    
    do! if input.Length < 3 then
            Error (InvalidInput "Input must be at least 3 characters")
        else Ok ()
    
    let processed = input.ToUpper().Trim()
    logger.Log Info (sprintf "Successfully processed: %s" processed)
    return processed
}

let recoverableOperation input =
    match processBusinessRule input with
    | Ok result -> Ok result
    | Error (InvalidInput msg) -> 
        logger.Log Warning (sprintf "Attempting recovery: %s" msg)
        // Try to recover with default value
        if String.IsNullOrWhiteSpace input then Ok "DEFAULT"
        else Ok (input.PadRight(3, 'X'))
    | Error err -> Error err

let batchProcess inputs =
    let results = inputs |> List.map recoverableOperation
    let successes = results |> List.choose (function Ok v -> Some v | Error _ -> None)
    let failures = results |> List.choose (function Error e -> Some e | Ok _ -> None)
    
    logger.Log Info (sprintf "Processed %d items: %d succeeded, %d failed" 
                    inputs.Length successes.Length failures.Length)
    
    if failures.Length > 0 then
        logger.Log Warning "Some items failed to process"
        failures |> List.iter (function
            | InvalidInput msg -> logger.Log Warning (sprintf "Invalid input: %s" msg)
            | ProcessingFailed msg -> logger.Log Error (sprintf "Processing failed: %s" msg)
            | SystemError ex -> logger.Log Critical (sprintf "System error: %s" ex.Message))
    
    {| Successes = successes; Failures = failures |}

let testInputs = [
    "hello"      // Valid
    ""           // Empty - will be recovered
    "hi"         // Too short - will be recovered  
    "world"      // Valid
    null         // Null - will cause error
]

let result = batchProcess (testInputs |> List.filter (fun x -> x <> null))
printfn "Final results: %A" result
```

Best practices include comprehensive logging, graceful error recovery,  
error aggregation for batch operations, and appropriate error levels  
for different scenarios.  

## Advanced error composition

Combine multiple error handling patterns for complex scenarios requiring  
sophisticated error composition and transformation.  

```F#
open System

type ValidationRule<'T> = 'T -> Result<'T, string>

let createRule name predicate errorMsg : ValidationRule<'T> = fun value ->
    if predicate value then Ok value
    else Error (sprintf "%s: %s" name errorMsg)

let combineRules (rules: ValidationRule<'T> list) : ValidationRule<'T> = fun value ->
    let results = rules |> List.map (fun rule -> rule value)
    let errors = results |> List.choose (function Error e -> Some e | Ok _ -> None)
    if List.isEmpty errors then Ok value
    else Error (String.concat "; " errors)

// Email validation rules
let emailRules = [
    createRule "Required" (fun s -> not (String.IsNullOrWhiteSpace s)) "Email is required"
    createRule "Format" (fun s -> s.Contains "@") "Email must contain @"
    createRule "Length" (fun s -> s.Length >= 5) "Email must be at least 5 characters"
    createRule "Domain" (fun s -> s.Contains ".") "Email must contain domain"
]

// Age validation rules  
let ageRules = [
    createRule "Range" (fun a -> a >= 0 && a <= 120) "Age must be between 0 and 120"
    createRule "Adult" (fun a -> a >= 18) "Must be at least 18 years old"
]

type Person = { Name: string; Email: string; Age: int }

let validateEmail = combineRules emailRules
let validateAge = combineRules ageRules

let validatePerson person = result {
    let! validName = 
        if String.IsNullOrWhiteSpace person.Name then Error "Name is required"
        else Ok person.Name
    let! validEmail = validateEmail person.Email
    let! validAge = validateAge person.Age
    
    return { Name = validName; Email = validEmail; Age = validAge }
}

// Pipeline composition for complex transformations
let (>=>) f g = fun x -> f x |> Result.bind g

let trimString (s: string) = Ok (s.Trim())
let toLowerCase (s: string) = Ok (s.ToLower())
let validateThenTransform = validateEmail >=> trimString >=> toLowerCase

let processPersonData personData =
    personData
    |> List.map validatePerson
    |> List.indexed
    |> List.map (fun (i, result) -> 
        match result with
        | Ok person -> sprintf "Person %d: Valid - %A" i person
        | Error errors -> sprintf "Person %d: Invalid - %s" i errors)

let testPersons = [
    { Name = "John Doe"; Email = "john@example.com"; Age = 25 }
    { Name = ""; Email = "jane@example.com"; Age = 30 }
    { Name = "Bob Smith"; Email = "invalid-email"; Age = 17 }
    { Name = "Alice Johnson"; Email = "alice@domain.com"; Age = 150 }
]

processPersonData testPersons |> List.iter (printfn "%s")

// Test email transformation pipeline
let emailTests = [
    "  JOHN@EXAMPLE.COM  "
    "invalid-email"
    ""
]

emailTests |> List.iter (fun email ->
    match validateThenTransform email with
    | Ok processed -> printfn "Email '%s' -> '%s'" email processed
    | Error msg -> printfn "Email '%s' -> Error: %s" email msg)
```

Advanced error composition demonstrates how to build reusable validation  
rules, compose them into complex validators, and create processing  
pipelines that handle multiple types of errors gracefully.  

## Monadic error chaining

Chain operations that may fail using monadic bind operations for  
clean error propagation in complex workflows.  

```F#
open System

type ProcessingError = 
    | InputError of string
    | TransformationError of string  
    | OutputError of string

let (>>=) result f =
    match result with
    | Ok value -> f value
    | Error err -> Error err

let validateInput (input: string) =
    if String.IsNullOrWhiteSpace input then Error (InputError "Input cannot be empty")
    elif input.Length < 3 then Error (InputError "Input too short") 
    else Ok input

let transformToNumber (input: string) =
    match Int32.TryParse input with
    | true, num when num > 0 -> Ok num
    | true, _ -> Error (TransformationError "Number must be positive")
    | false, _ -> Error (TransformationError "Invalid number format")

let formatOutput num =
    try
        Ok (sprintf "Processed number: %d (squared: %d)" num (num * num))
    with
    | ex -> Error (OutputError ex.Message)

let processChain input =
    validateInput input
    >>= transformToNumber  
    >>= formatOutput

let testInputs = ["123"; "abc"; ""; "-50"; "42"]

testInputs |> List.iter (fun input ->
    match processChain input with
    | Ok result -> printfn "Success: %s" result
    | Error (InputError msg) -> printfn "Input Error: %s" msg
    | Error (TransformationError msg) -> printfn "Transform Error: %s" msg  
    | Error (OutputError msg) -> printfn "Output Error: %s" msg)
```

Monadic chaining with custom bind operators enables clean composition  
of operations that may fail at different stages with specific error types.  

## Retry mechanisms with exponential backoff

Implement retry logic with exponential backoff for handling transient  
failures in distributed systems and external service calls.  

```F#
open System
open System.Threading

type RetryPolicy = {
    MaxAttempts: int
    BaseDelay: int
    MaxDelay: int
    BackoffMultiplier: float
}

let defaultRetryPolicy = {
    MaxAttempts = 3
    BaseDelay = 1000
    MaxDelay = 10000
    BackoffMultiplier = 2.0
}

let withRetry policy operation = 
    let rec retry attempt =
        match operation() with
        | Ok result -> Ok result
        | Error err when attempt >= policy.MaxAttempts -> Error err
        | Error err ->
            let delay = min policy.MaxDelay 
                          (int (float policy.BaseDelay * (policy.BackoffMultiplier ** float (attempt - 1))))
            printfn "Attempt %d failed: %s. Retrying in %dms..." attempt err delay
            Thread.Sleep(delay)
            retry (attempt + 1)
    retry 1

let unreliableOperation () =
    let random = Random()
    let success = random.Next(1, 4) = 1  // 33% success rate
    if success then Ok "Operation completed successfully"
    else Error "Temporary service unavailable"

// Test the retry mechanism
for i in 1..3 do
    printfn "\nTest run %d:" i
    match withRetry defaultRetryPolicy unreliableOperation with
    | Ok msg -> printfn "Final result: %s" msg
    | Error err -> printfn "Failed after all retries: %s" err
```

Retry mechanisms with exponential backoff help handle transient failures  
gracefully while avoiding overwhelming failing services with requests.  

## Type-safe configuration parsing

Parse application configuration with comprehensive validation and  
type safety using discriminated unions and Result types.  

```F#
open System
open System.IO
open System.Text.Json

type ConfigError = 
    | MissingKey of string
    | InvalidValue of string * string
    | ParseError of string

type LogLevel = Debug | Info | Warning | Error
type DatabaseConfig = { Host: string; Port: int; Database: string }
type AppConfig = { 
    LogLevel: LogLevel
    Database: DatabaseConfig  
    ApiTimeout: TimeSpan
    MaxRetries: int
}

let parseLogLevel = function
    | "debug" | "Debug" -> Ok Debug
    | "info" | "Info" -> Ok Info  
    | "warning" | "Warning" -> Ok Warning
    | "error" | "Error" -> Ok Error
    | level -> Error (InvalidValue ("LogLevel", level))

let parseTimeSpan (value: string) =
    match TimeSpan.TryParse value with
    | true, ts -> Ok ts
    | false, _ -> Error (InvalidValue ("TimeSpan", value))

let parseConfig (json: string) =
    try
        use doc = JsonDocument.Parse json
        let root = doc.RootElement
        
        result {
            let! logLevel = 
                match root.TryGetProperty("logLevel") with
                | true, prop -> parseLogLevel prop.GetString()
                | false, _ -> Error (MissingKey "logLevel")
            
            let! dbHost = 
                match root.TryGetProperty("database") with
                | true, db -> 
                    match db.TryGetProperty("host") with
                    | true, host -> Ok host.GetString()
                    | false, _ -> Error (MissingKey "database.host")
                | false, _ -> Error (MissingKey "database")
            
            let! dbPort = 
                match root.TryGetProperty("database") with
                | true, db -> 
                    match db.TryGetProperty("port") with
                    | true, port -> 
                        if port.ValueKind = JsonValueKind.Number then Ok port.GetInt32()
                        else Error (InvalidValue ("database.port", port.ToString()))
                    | false, _ -> Error (MissingKey "database.port")
                | false, _ -> Error (MissingKey "database")
            
            let! dbName = 
                match root.TryGetProperty("database") with
                | true, db -> 
                    match db.TryGetProperty("name") with
                    | true, name -> Ok name.GetString()
                    | false, _ -> Error (MissingKey "database.name")
                | false, _ -> Error (MissingKey "database")
            
            let! timeout = 
                match root.TryGetProperty("apiTimeout") with
                | true, prop -> parseTimeSpan prop.GetString()
                | false, _ -> Ok (TimeSpan.FromSeconds(30.0))  // default
            
            let! maxRetries = 
                match root.TryGetProperty("maxRetries") with
                | true, prop -> 
                    if prop.ValueKind = JsonValueKind.Number then Ok prop.GetInt32()
                    else Error (InvalidValue ("maxRetries", prop.ToString()))
                | false, _ -> Ok 3  // default
            
            return {
                LogLevel = logLevel
                Database = { Host = dbHost; Port = dbPort; Database = dbName }
                ApiTimeout = timeout
                MaxRetries = maxRetries
            }
        }
    with
    | ex -> Error (ParseError ex.Message)

let configJson = """{
  "logLevel": "Info",
  "database": {
    "host": "localhost",
    "port": 5432,
    "name": "myapp"
  },
  "apiTimeout": "00:01:00",
  "maxRetries": 5
}"""

let invalidConfigJson = """{
  "logLevel": "InvalidLevel",
  "database": {
    "host": "localhost"
  }
}"""

[configJson; invalidConfigJson] |> List.iter (fun json ->
    match parseConfig json with
    | Ok config -> printfn "Valid config: %A" config
    | Error (MissingKey key) -> printfn "Missing key: %s" key
    | Error (InvalidValue (key, value)) -> printfn "Invalid value for %s: %s" key value
    | Error (ParseError msg) -> printfn "Parse error: %s" msg)
```

Type-safe configuration parsing ensures application settings are validated  
at startup, preventing runtime errors from invalid configuration values.  

## Concurrent error handling

Handle errors in concurrent operations using parallel processing with  
Result types to collect both successes and failures safely.  

```F#
open System
open System.Threading.Tasks

type WorkItem = { Id: int; Data: string }
type ProcessingResult = { 
    Successes: (int * string) list
    Failures: (int * string) list  
    ProcessingTime: TimeSpan
}

let processWorkItem (item: WorkItem) = async {
    try
        // Simulate processing time
        do! Async.Sleep (Random().Next(100, 500))
        
        // Simulate random failures
        if Random().Next(1, 5) = 1 then
            return Error (item.Id, sprintf "Processing failed for item %d" item.Id)
        else
            let result = item.Data.ToUpper()
            return Ok (item.Id, sprintf "Processed: %s" result)
    with
    | ex -> return Error (item.Id, ex.Message)
}

let processConcurrently maxConcurrency workItems = async {
    let startTime = DateTime.Now
    
    let semaphore = new System.Threading.SemaphoreSlim(maxConcurrency)
    
    let processWithSemaphore item = async {
        let! _ = semaphore.WaitAsync() |> Async.AwaitTask
        try
            return! processWorkItem item
        finally
            semaphore.Release() |> ignore
    }
    
    let! results = workItems |> List.map processWithSemaphore |> Async.Parallel
    
    let endTime = DateTime.Now
    let processingTime = endTime - startTime
    
    let successes = results |> Array.choose (function Ok s -> Some s | Error _ -> None) |> Array.toList
    let failures = results |> Array.choose (function Error f -> Some f | Ok _ -> None) |> Array.toList
    
    semaphore.Dispose()
    
    return { 
        Successes = successes
        Failures = failures
        ProcessingTime = processingTime 
    }
}

let workItems = [
    for i in 1..10 -> { Id = i; Data = sprintf "item_%d" i }
]

async {
    printfn "Processing %d items with max concurrency of 3..." workItems.Length
    let! result = processConcurrently 3 workItems
    
    printfn "\nResults:"
    printfn "Successes (%d):" result.Successes.Length
    result.Successes |> List.iter (fun (id, msg) -> printfn "  %d: %s" id msg)
    
    printfn "Failures (%d):" result.Failures.Length  
    result.Failures |> List.iter (fun (id, msg) -> printfn "  %d: %s" id msg)
    
    printfn "Processing time: %A" result.ProcessingTime
} |> Async.RunSynchronously
```

Concurrent error handling enables parallel processing while maintaining  
error isolation and collecting comprehensive results from all operations.  

## Domain modeling with error types

Model business domains using discriminated unions for errors that  
represent specific business rules and validation constraints.  

```F#
open System

type CustomerId = CustomerId of int
type OrderId = OrderId of int  
type ProductId = ProductId of int
type Quantity = Quantity of int

type BusinessError = 
    | CustomerNotFound of CustomerId
    | ProductNotFound of ProductId
    | InsufficientStock of ProductId * requested: int * available: int
    | InvalidQuantity of int
    | OrderProcessingFailed of OrderId * string

type Customer = { Id: CustomerId; Name: string; IsActive: bool }
type Product = { Id: ProductId; Name: string; Price: decimal; Stock: int }
type OrderItem = { ProductId: ProductId; Quantity: Quantity; Price: decimal }
type Order = { Id: OrderId; CustomerId: CustomerId; Items: OrderItem list; Total: decimal }

module Database =
    let customers = [
        { Id = CustomerId 1; Name = "John Doe"; IsActive = true }
        { Id = CustomerId 2; Name = "Jane Smith"; IsActive = false }
    ] |> List.map (fun c -> c.Id, c) |> Map.ofList
    
    let products = [
        { Id = ProductId 1; Name = "Widget"; Price = 10.50m; Stock = 100 }
        { Id = ProductId 2; Name = "Gadget"; Price = 25.00m; Stock = 5 }
        { Id = ProductId 3; Name = "Tool"; Price = 15.75m; Stock = 0 }
    ] |> List.map (fun p -> p.Id, p) |> Map.ofList

module BusinessLogic =
    let validateQuantity qty =
        if qty > 0 && qty <= 1000 then Ok (Quantity qty)
        else Error (InvalidQuantity qty)
    
    let findCustomer customerId =
        match Database.customers |> Map.tryFind customerId with
        | Some customer when customer.IsActive -> Ok customer
        | Some _ -> Error (CustomerNotFound customerId)  // Inactive treated as not found
        | None -> Error (CustomerNotFound customerId)
    
    let findProduct productId =
        match Database.products |> Map.tryFind productId with
        | Some product -> Ok product  
        | None -> Error (ProductNotFound productId)
    
    let checkStock productId (Quantity qty) =
        match findProduct productId with
        | Ok product when product.Stock >= qty -> Ok product
        | Ok product -> Error (InsufficientStock (productId, qty, product.Stock))
        | Error err -> Error err
    
    let createOrderItem productId quantity = result {
        let! validQty = validateQuantity quantity
        let! product = checkStock productId validQty
        return { ProductId = productId; Quantity = validQty; Price = product.Price }
    }
    
    let processOrder customerId orderItems = result {
        let! customer = findCustomer customerId
        let! items = orderItems |> List.map (fun (pid, qty) -> createOrderItem pid qty) 
                                |> List.fold (fun acc item ->
                                    match acc, item with
                                    | Ok items, Ok newItem -> Ok (newItem :: items)  
                                    | Error err, _ -> Error err
                                    | Ok _, Error err -> Error err) (Ok [])
        let total = items |> List.sumBy (fun item -> 
            let (Quantity qty) = item.Quantity
            item.Price * decimal qty)
        let orderId = OrderId (Random().Next(1000, 9999))
        return { Id = orderId; CustomerId = customerId; Items = items; Total = total }
    }

let handleBusinessError = function
    | CustomerNotFound (CustomerId id) -> sprintf "Customer %d not found or inactive" id
    | ProductNotFound (ProductId id) -> sprintf "Product %d not found" id
    | InsufficientStock (ProductId pid, requested, available) -> 
        sprintf "Insufficient stock for product %d: requested %d, available %d" pid requested available
    | InvalidQuantity qty -> sprintf "Invalid quantity: %d (must be 1-1000)" qty
    | OrderProcessingFailed (OrderId id, msg) -> sprintf "Order %d processing failed: %s" id msg

let testOrders = [
    (CustomerId 1, [(ProductId 1, 5); (ProductId 2, 2)])  // Valid order
    (CustomerId 2, [(ProductId 1, 3)])                    // Inactive customer
    (CustomerId 1, [(ProductId 3, 1)])                    // Out of stock
    (CustomerId 1, [(ProductId 1, 2000)])                 // Invalid quantity
    (CustomerId 99, [(ProductId 1, 1)])                   // Customer not found
]

testOrders |> List.iter (fun (customerId, items) ->
    match BusinessLogic.processOrder customerId items with
    | Ok order -> printfn "Order created: %A" order
    | Error err -> printfn "Order failed: %s" (handleBusinessError err))
```

Domain modeling with error types creates a self-documenting system where  
business rules are encoded in the type system and errors represent  
specific business scenarios.  
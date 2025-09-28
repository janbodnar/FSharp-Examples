# Conditions

Conditional statements in F# enable decision-making based on boolean  
expressions. F# provides if-then, elif, and else constructs for  
controlling program flow based on different conditions.  

## Single conditions

Single conditions evaluate one boolean expression at a time. Each if  
statement is independent and all matching conditions will execute.  

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 

if r < 0 then 
    printfn "negative value" 

if r = 0 then
    printfn "zero" 
```

This example creates a random number and tests it with three separate  
if statements. Each condition is evaluated independently, so multiple  
conditions could execute if the random number meets their criteria.  

## Multiple conditions

Multiple conditions use elif to chain related conditions together.  
Only the first matching condition executes, providing mutually  
exclusive branching logic.  

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 
elif r < 0 then 
    printfn "negative value" 
elif r = 0 then 
    printfn "zero" 
```

The elif keyword creates a chain of conditions where only the first  
matching condition executes. This is more efficient than separate if  
statements when dealing with mutually exclusive conditions.  

## else keyword

The else keyword provides a catch-all branch when no previous  
conditions match. It ensures exhaustive handling of all possible  
cases.  

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 
elif r < 0 then 
    printfn "negative value" 
else 
    printfn "zero" 
```

The else branch handles any case not covered by previous conditions.  
This creates exhaustive conditional logic and eliminates the need  
for explicit equality checks in simple cases.  

## Nested conditionals

Nested conditionals allow complex decision trees by placing if  
statements inside other conditional branches.  

```F#
open System

let age = Random().Next(1, 100)
let hasLicense = Random().Next(0, 2) = 1

if age >= 18 then
    if hasLicense then
        printfn "Can drive legally"
    else
        printfn "Old enough but needs license"
else
    if age >= 16 then
        printfn "Can get learner's permit"
    else
        printfn "Too young to drive"
```

Nested conditionals create hierarchical decision making. Each level  
narrows down the conditions, allowing for sophisticated branching  
logic based on multiple criteria.  

## Boolean operators

Boolean operators (&&, ||, not) combine multiple conditions into  
complex expressions for more sophisticated conditional logic.  

```F#
open System

let temperature = Random().Next(-10, 40)
let isRaining = Random().Next(0, 2) = 1
let hasUmbrella = Random().Next(0, 2) = 1

if temperature > 20 && not isRaining then
    printfn "Perfect weather for a walk"
elif temperature > 15 && (not isRaining || hasUmbrella) then
    printfn "Good weather, umbrella helps"
elif temperature < 0 || (isRaining && not hasUmbrella) then
    printfn "Stay inside today"
else
    printfn "Okay weather, dress appropriately"
```

Boolean operators enable complex conditions combining multiple boolean  
values. The && operator requires all conditions to be true, || requires  
at least one to be true, and not inverts boolean values.  

## Type checking conditionals

Type checking conditionals use pattern matching or type tests to  
branch based on the runtime type of values.  

```F#
let checkType (value: obj) =
    if value :? string then
        printfn "It's a string: %s" (value :?> string)
    elif value :? int then
        printfn "It's an integer: %d" (value :?> int)
    elif value :? float then
        printfn "It's a float: %f" (value :?> float)
    else
        printfn "Unknown type: %A" value

checkType "hello"
checkType 42
checkType 3.14
checkType [1; 2; 3]
```

The :? operator tests if a value is of a specific type, while :?> casts  
the value to that type. This enables type-safe conditional branching  
with dynamic type checking.  

## Option type conditionals

Option types represent values that may or may not exist, providing  
null-safe conditional handling using Some and None patterns.  

```F#
let tryDivide x y =
    if y = 0 then None else Some (float x / float y)

let checkDivision x y =
    let result = tryDivide x y
    if result.IsSome then
        printfn "%d ÷ %d = %f" x y result.Value
    else
        printfn "Cannot divide %d by zero" x

checkDivision 10 2
checkDivision 10 0

// Alternative using pattern matching
let checkDivisionPattern x y =
    match tryDivide x y with
    | Some result -> printfn "%d ÷ %d = %f" x y result
    | None -> printfn "Cannot divide %d by zero" x
```

Option types eliminate null reference exceptions by explicitly  
representing absence of values. The IsSome property and Value  
extraction provide safe access to optional values.  

## Result type conditionals

Result types handle operations that can succeed or fail, providing  
structured error handling with Ok and Error cases.  

```F#
type ValidationError = 
    | EmailTooShort
    | EmailMissingAtSign
    | EmailMissingDomain

let validateEmail email =
    if String.length email < 5 then
        Error EmailTooShort
    elif not (email.Contains("@")) then
        Error EmailMissingAtSign
    elif not (email.Contains(".")) then
        Error EmailMissingDomain
    else
        Ok email

let processEmail email =
    match validateEmail email with
    | Ok validEmail -> printfn "Valid email: %s" validEmail
    | Error EmailTooShort -> printfn "Email too short"
    | Error EmailMissingAtSign -> printfn "Email missing @ sign"
    | Error EmailMissingDomain -> printfn "Email missing domain"

processEmail "test@example.com"
processEmail "bad"
```

Result types provide explicit success/failure handling. The IsOk  
property and ResultValue/ErrorValue accessors enable conditional  
processing based on operation outcomes.  

## String matching conditionals

String conditionals use various string operations for pattern  
matching, case sensitivity, and content validation.  

```F#
let classifyText (text: string) =
    let lowerText = text.ToLower()
    
    if String.IsNullOrEmpty(text) then
        printfn "Empty or null string"
    elif text.StartsWith("http://") || text.StartsWith("https://") then
        printfn "Web URL detected"
    elif text.Contains("@") && text.Contains(".") then
        printfn "Possible email address"
    elif System.Text.RegularExpressions.Regex.IsMatch(text, @"^\d+$") then
        printfn "Numeric string"
    elif lowerText = "hello" || lowerText = "hi" || lowerText = "hey" then
        printfn "Greeting detected"
    elif text.Length > 100 then
        printfn "Long text (%d characters)" text.Length
    else
        printfn "Regular text: %s" text

classifyText "https://example.com"
classifyText "user@domain.com"
classifyText "12345"
classifyText "Hello"
classifyText ""
```

String conditionals leverage string methods like StartsWith, Contains,  
and regular expressions for sophisticated text analysis and  
classification.  

## Numeric range conditionals

Numeric range conditionals classify numbers into categories using  
comparison operators and range checking.  

```F#
open System

let classifyNumber (n: float) =
    if Double.IsNaN(n) then
        printfn "Not a Number (NaN)"
    elif Double.IsPositiveInfinity(n) then
        printfn "Positive Infinity"
    elif Double.IsNegativeInfinity(n) then
        printfn "Negative Infinity"
    elif n = 0.0 then
        printfn "Zero"
    elif abs n < 0.001 then
        printfn "Very close to zero: %g" n
    elif n >= -1.0 && n <= 1.0 then
        printfn "Within unit range: %g" n
    elif n > 1000000.0 then
        printfn "Large number: %g" n
    elif n < -1000000.0 then
        printfn "Large negative number: %g" n
    else
        printfn "Regular number: %g" n

classifyNumber 0.0
classifyNumber 0.0005
classifyNumber 0.5
classifyNumber 1500000.0
classifyNumber Double.NaN
```

Numeric range conditionals handle special floating-point values and  
classify numbers by magnitude, sign, and proximity to specific  
values.  

## List and array conditionals

Collection conditionals check properties like emptiness, length,  
and content to make decisions based on collection state.  

```F#
let analyzeList (items: int list) =
    if List.isEmpty items then
        printfn "Empty list"
    elif items.Length = 1 then
        printfn "Single item: %d" items.Head
    elif items.Length <= 3 then
        printfn "Small list (%d items): %A" items.Length items
    elif List.forall (fun x -> x > 0) items then
        printfn "All positive numbers in list of %d" items.Length
    elif List.exists (fun x -> x < 0) items then
        printfn "Contains negative numbers: %A" items
    elif List.sum items > 100 then
        printfn "Large sum (%d) from list: %A" (List.sum items) items
    else
        printfn "Regular list: %A" items

let analyzeArray (arr: string[]) =
    if Array.isEmpty arr then
        printfn "Empty array"
    elif arr |> Array.forall (String.IsNullOrWhiteSpace) then
        printfn "All empty or whitespace strings"
    elif arr |> Array.exists (fun s -> s.Length > 10) then
        printfn "Contains long strings"
    else
        printfn "Regular string array: %A" arr

analyzeList []
analyzeList [42]
analyzeList [1; 2; 3; 4; 5]
analyzeList [-1; 2; -3]
analyzeArray [||]
analyzeArray [|"hello"; "world"|]
```

Collection conditionals use functions like isEmpty, forall, exists,  
and aggregate functions to analyze collection properties and make  
decisions based on their contents.  

## Record field conditionals

Record conditionals access and compare record fields to make  
decisions based on structured data properties.  

```F#
type Person = {
    Name: string
    Age: int
    Email: string option
    IsActive: bool
}

let analyzePerson person =
    if String.IsNullOrEmpty(person.Name) then
        printfn "Invalid person - no name"
    elif person.Age < 0 || person.Age > 150 then
        printfn "Invalid age: %d" person.Age
    elif not person.IsActive then
        printfn "%s is inactive" person.Name
    elif person.Age < 18 then
        match person.Email with
        | Some email -> printfn "Minor %s has email: %s" person.Name email
        | None -> printfn "Minor %s has no email" person.Name
    elif person.Age >= 65 then
        printfn "Senior citizen: %s (age %d)" person.Name person.Age
    else
        match person.Email with
        | Some email -> printfn "Adult %s (%d) - %s" person.Name person.Age email
        | None -> printfn "Adult %s (%d) - no email" person.Name person.Age

let person1 = { Name = "Alice"; Age = 30; Email = Some "alice@test.com"; IsActive = true }
let person2 = { Name = "Bob"; Age = 16; Email = None; IsActive = true }
let person3 = { Name = "Carol"; Age = 70; Email = Some "carol@test.com"; IsActive = false }

analyzePerson person1
analyzePerson person2
analyzePerson person3
```

Record field conditionals combine multiple field checks with pattern  
matching on option types, enabling sophisticated business logic  
based on structured data validation.  

## Function conditionals

Function conditionals pass functions as parameters and make decisions  
based on function results or function presence.  

```F#
let processWithCondition condition processor fallback value =
    if condition value then
        processor value
    else
        fallback value

let isEven n = n % 2 = 0
let doubleValue n = n * 2
let addOne n = n + 1

let testFunction predicate action (numbers: int list) =
    if List.isEmpty numbers then
        printfn "No numbers to process"
    elif List.forall predicate numbers then
        let results = List.map action numbers
        printfn "All numbers passed condition: %A" results
    else
        let filtered = List.filter predicate numbers
        if List.isEmpty filtered then
            printfn "No numbers passed the condition"
        else
            let results = List.map action filtered
            printfn "Filtered results: %A" results

// Using function conditionals
processWithCondition isEven doubleValue addOne 4 |> printfn "Result: %d"
processWithCondition isEven doubleValue addOne 5 |> printfn "Result: %d"

testFunction isEven doubleValue [2; 4; 6; 8]
testFunction isEven doubleValue [1; 3; 5]
testFunction isEven doubleValue [1; 2; 3; 4; 5]
```

Function conditionals enable higher-order programming patterns where  
behavior changes based on function parameters, creating flexible and  
reusable conditional logic.  

## Exception handling conditionals

Exception handling conditionals use try-with expressions to handle  
errors gracefully and make decisions based on exception types.  

```F#
open System

let safeOperation operation input =
    try
        let result = operation input
        if result.ToString().Length > 10 then
            printfn "Long result: %s" (result.ToString().Substring(0, 10) + "...")
        else
            printfn "Result: %A" result
        true
    with
    | :? DivideByZeroException -> 
        printfn "Division by zero detected"
        false
    | :? FormatException -> 
        printfn "Invalid format in input"
        false
    | :? OverflowException -> 
        printfn "Numeric overflow occurred"
        false
    | ex -> 
        printfn "Unexpected error: %s" ex.Message
        false

let divide x y = x / y
let parseNumber (s: string) = Int32.Parse(s)
let factorial n = 
    if n > 20 then failwith "Too large for factorial"
    else List.fold (*) 1 [1..n]

// Test exception handling conditionals
if safeOperation (divide 10) 2 then printfn "Division succeeded"
if safeOperation (divide 10) 0 then printfn "This won't print"
if safeOperation parseNumber "123" then printfn "Parsing succeeded"  
if safeOperation parseNumber "abc" then printfn "This won't print"
if safeOperation factorial 15 then printfn "Factorial succeeded"
if safeOperation factorial 25 then printfn "This won't print"
```

Exception handling conditionals combine try-with blocks with  
conditional logic to create robust error handling that responds  
differently to various exception types.  

## Computation expression conditionals

Computation expressions provide conditional workflows for specific  
computational contexts like Maybe, Result, or Async operations.  

```F#
type MaybeBuilder() =
    member _.Bind(x, f) = 
        match x with 
        | Some value -> f value 
        | None -> None
    member _.Return(x) = Some x
    member _.Zero() = None

let maybe = MaybeBuilder()

let tryGetUser userId =
    if userId > 0 && userId <= 100 then 
        Some { Name = sprintf "User%d" userId; Age = userId + 20 }
    else 
        None

type User = { Name: string; Age: int }

let processUserWorkflow userId =
    maybe {
        let! user = tryGetUser userId
        if user.Age >= 18 then
            if user.Age < 65 then
                return sprintf "%s is working age (%d)" user.Name user.Age
            else
                return sprintf "%s is retirement age (%d)" user.Name user.Age
        else
            return sprintf "%s is too young to work (%d)" user.Name user.Age
    }

// Test computation expression conditionals
match processUserWorkflow 25 with
| Some message -> printfn "%s" message
| None -> printfn "User not found or processing failed"

match processUserWorkflow 150 with
| Some message -> printfn "%s" message
| None -> printfn "User not found or processing failed"
```

Computation expression conditionals embed conditional logic within  
monadic workflows, enabling clean composition of conditional  
operations with computational contexts.  

## Active pattern conditionals

Active patterns define custom pattern matching that can be used in  
conditional expressions for domain-specific classifications.  

```F#
// Single-case active patterns
let (|Even|Odd|) n = if n % 2 = 0 then Even else Odd
let (|Positive|Negative|Zero|) n = 
    if n > 0 then Positive 
    elif n < 0 then Negative 
    else Zero

// Multi-case active patterns
let (|Small|Medium|Large|Huge|) n =
    if n < 10 then Small
    elif n < 100 then Medium  
    elif n < 1000 then Large
    else Huge

// Parameterized active patterns
let (|DivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some DivisibleBy else None

let classifyNumber n =
    match n with
    | Even & Positive & Small -> printfn "%d: small positive even" n
    | Odd & Negative & Medium -> printfn "%d: medium negative odd" n
    | DivisibleBy 3 -> printfn "%d is divisible by 3" n
    | DivisibleBy 7 -> printfn "%d is divisible by 7" n
    | Zero -> printfn "Zero detected"
    | Huge -> printfn "%d is a huge number" n
    | _ -> printfn "%d doesn't match special patterns" n

// Using active patterns in conditionals
let analyzeWithPatterns numbers =
    if List.forall (function Even -> true | _ -> false) numbers then
        printfn "All numbers are even: %A" numbers
    elif List.exists (function Huge -> true | _ -> false) numbers then
        printfn "Contains huge numbers: %A" numbers
    else
        printfn "Mixed number patterns: %A" numbers

classifyNumber 6
classifyNumber -55
classifyNumber 21
classifyNumber 1500
analyzeWithPatterns [2; 4; 6; 8]
analyzeWithPatterns [1; 1000; 3]
```

Active patterns enable domain-specific conditional logic by defining  
custom pattern matching that integrates seamlessly with F#'s  
conditional and matching constructs.  

## Complex nested conditionals

Complex nested conditionals combine multiple conditional patterns  
to create sophisticated decision-making logic.  

```F#
open System

type UserRole = Admin | Manager | Employee | Guest
type Permission = Read | Write | Delete | Execute

type User = {
    Name: string
    Role: UserRole
    IsActive: bool
    LastLogin: DateTime option
    FailedLogins: int
}

let hasPermission user permission =
    if not user.IsActive then
        false
    elif user.FailedLogins >= 3 then
        false
    else
        match user.Role, permission with
        | Admin, _ -> true
        | Manager, Delete -> false
        | Manager, _ -> true
        | Employee, Write | Employee, Read -> true
        | Employee, _ -> false
        | Guest, Read -> true
        | Guest, _ -> false

let analyzeUserAccess user requestedPermissions =
    if String.IsNullOrEmpty(user.Name) then
        printfn "Invalid user - no name provided"
    elif not user.IsActive then
        printfn "User %s is deactivated" user.Name
    elif user.FailedLogins >= 5 then
        printfn "User %s is locked due to failed logins" user.Name
    else
        match user.LastLogin with
        | None -> 
            if user.Role = Guest then
                printfn "Guest user %s - never logged in" user.Name
            else
                printfn "User %s has never logged in - requires setup" user.Name
        | Some lastLogin ->
            let daysSinceLogin = (DateTime.Now - lastLogin).Days
            if daysSinceLogin > 90 then
                printfn "User %s inactive for %d days" user.Name daysSinceLogin
            elif daysSinceLogin > 30 then
                match user.Role with
                | Admin | Manager -> 
                    printfn "Warning: %s %s inactive for %d days" 
                        (user.Role.ToString()) user.Name daysSinceLogin
                | _ -> 
                    printfn "User %s moderately inactive (%d days)" user.Name daysSinceLogin
            else
                let grantedPermissions = 
                    requestedPermissions 
                    |> List.filter (hasPermission user)
                
                if List.isEmpty grantedPermissions then
                    printfn "User %s has no granted permissions from request" user.Name
                elif List.length grantedPermissions = List.length requestedPermissions then
                    printfn "User %s granted all permissions: %A" user.Name grantedPermissions
                else
                    let deniedPermissions = 
                        requestedPermissions 
                        |> List.filter (hasPermission user >> not)
                    printfn "User %s - Granted: %A, Denied: %A" 
                        user.Name grantedPermissions deniedPermissions

let adminUser = { 
    Name = "Alice"; Role = Admin; IsActive = true; 
    LastLogin = Some (DateTime.Now.AddDays(-5.0)); FailedLogins = 0 
}
let suspiciousUser = { 
    Name = "Bob"; Role = Employee; IsActive = true; 
    LastLogin = Some (DateTime.Now.AddDays(-1.0)); FailedLogins = 4 
}

analyzeUserAccess adminUser [Read; Write; Delete; Execute]
analyzeUserAccess suspiciousUser [Read; Write]
```

Complex nested conditionals demonstrate how multiple conditional  
patterns can be combined to implement sophisticated business logic  
with clear, readable decision trees.  

## Performance-aware conditionals

Performance-aware conditionals optimize execution by ordering  
conditions strategically and using short-circuit evaluation.  

```F#
open System
open System.Diagnostics

let expensiveOperation1 () =
    System.Threading.Thread.Sleep(10)
    true

let expensiveOperation2 () =
    System.Threading.Thread.Sleep(20)
    false

let cheapCheck x = x > 0

// Inefficient: expensive operations run unnecessarily
let inefficientCondition x =
    let sw = Stopwatch.StartNew()
    let result = 
        if expensiveOperation1() && expensiveOperation2() && cheapCheck x then
            "All conditions met"
        elif expensiveOperation2() && expensiveOperation1() then
            "Some conditions met"
        else
            "No conditions met"
    sw.Stop()
    printfn "Inefficient took %d ms: %s" sw.ElapsedMilliseconds result

// Efficient: cheap checks first, short-circuit evaluation
let efficientCondition x =
    let sw = Stopwatch.StartNew()
    let result = 
        if cheapCheck x then
            if expensiveOperation1() then
                if expensiveOperation2() then
                    "All conditions met"
                else
                    "Operation2 failed"
            else
                "Operation1 failed"
        else
            "Cheap check failed"
    sw.Stop()
    printfn "Efficient took %d ms: %s" sw.ElapsedMilliseconds result

// Memoization for repeated expensive conditionals
let memoizedConditions = System.Collections.Generic.Dictionary<int, bool>()

let memoizedExpensiveCheck x =
    if memoizedConditions.ContainsKey(x) then
        memoizedConditions.[x]
    else
        System.Threading.Thread.Sleep(5) // Simulate expensive operation
        let result = x % 17 = 0 // Some complex condition
        memoizedConditions.[x] <- result
        result

let testMemoization values =
    let sw = Stopwatch.StartNew()
    for value in values do
        if memoizedExpensiveCheck value then
            printfn "%d passes memoized condition" value
    sw.Stop()
    printfn "Memoized test took %d ms" sw.ElapsedMilliseconds

// Test performance differences
printfn "Performance comparison:"
inefficientCondition -5
efficientCondition -5

printfn "\nMemoization test:"
let testValues = [1; 17; 34; 17; 51; 1; 68; 34]
testMemoization testValues
testMemoization testValues  // Second run should be faster
```

Performance-aware conditionals demonstrate optimization techniques  
like condition ordering, short-circuit evaluation, and memoization  
to improve execution efficiency in conditional logic.  

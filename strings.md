# Strings

Strings in F# are immutable Unicode text sequences built on the .NET  
String type. F# provides extensive string manipulation capabilities  
through native functions, .NET methods, and powerful pattern matching.  
This comprehensive guide covers string operations from basic creation  
to advanced text processing techniques.  

## Basic string creation

String literals and basic string creation methods demonstrate the  
fundamental ways to create and initialize strings in F#.  

```F#
// String literals with different syntaxes
let simpleString = "Hello, World!"
let verbatimString = @"C:\Users\Name\Documents"
let multilineString = """This is a
multi-line string
with line breaks"""

// Triple-quoted strings preserve formatting
let formattedText = """
    Indented text
        More indented
    Back to first level
"""

printfn "%s" simpleString
printfn "%s" verbatimString
printfn "%s" multilineString
```

F# supports various string literal formats including regular strings,  
verbatim strings with @, and triple-quoted strings for multi-line text.  
Each format serves specific purposes for different text formatting needs.  

## String properties and basic access

Strings provide properties for length, indexing, and character access.  
Understanding these fundamentals enables effective string manipulation.  

```F#
let text = "Programming"

// Basic properties
printfn "Length: %d" text.Length
printfn "Is empty: %b" (String.IsNullOrEmpty(text))
printfn "Is whitespace: %b" (String.IsNullOrWhiteSpace(text))

// Character access
printfn "First char: %c" text[0]
printfn "Last char: %c" text[text.Length - 1]
printfn "Third char: %c" text[2]

// String slicing
printfn "First 4 chars: %s" text[0..3]
printfn "Last 3 chars: %s" text[8..]
printfn "Middle section: %s" text[2..6]
```

String indexing uses zero-based positions, and slicing creates new  
strings from specified ranges. F# provides concise syntax for accessing  
individual characters and substring ranges efficiently.  

## String equality and comparison

F# provides multiple ways to compare strings with different behaviors  
for case sensitivity, culture awareness, and ordering operations.  

```F#
open System

let str1 = "Hello"
let str2 = "HELLO"
let str3 = "Hello"

// Reference equality (same object)
printfn "Reference equal str1 = str3: %b" (obj.ReferenceEquals(str1, str3))

// Value equality (structural)
printfn "Value equal str1 = str3: %b" (str1 = str3)
printfn "Case sensitive equal: %b" (str1 = str2)

// Case insensitive comparison
printfn "Case insensitive equal: %b" 
    (String.Equals(str1, str2, StringComparison.OrdinalIgnoreCase))

// String ordering
printfn "Compare str1 to str2: %d" (String.Compare(str1, str2))
printfn "Compare ignoring case: %d" 
    (String.Compare(str1, str2, StringComparison.OrdinalIgnoreCase))
```

String comparison in F# distinguishes between reference equality,  
structural equality, and customizable comparison operations. Different  
comparison methods handle case sensitivity and cultural differences.  

## String searching and contains

String searching operations locate substrings, characters, and patterns  
within larger strings using various search strategies and options.  

```F#
open System

let text = "The quick brown fox jumps over the lazy dog"

// Basic contains operations
printfn "Contains 'fox': %b" (text.Contains("fox"))
printfn "Contains 'cat': %b" (text.Contains("cat"))

// Index-based searching
printfn "Index of 'fox': %d" (text.IndexOf("fox"))
printfn "Last index of 'the': %d" (text.LastIndexOf("the"))
printfn "Index of 'elephant': %d" (text.IndexOf("elephant")) // -1 if not found

// Case insensitive searching
printfn "Contains 'FOX' (ignore case): %b" 
    (text.IndexOf("FOX", StringComparison.OrdinalIgnoreCase) >= 0)

// Character searching
printfn "Index of 'q': %d" (text.IndexOf('q'))
printfn "Last index of 'o': %d" (text.LastIndexOf('o'))
```

String searching methods return indices for found substrings or -1  
when no match exists. Case sensitivity and cultural comparison options  
provide flexible search behavior for different application needs.  

## Repeat

String repetition creates new strings by duplicating existing content  
multiple times. F# provides built-in functions for efficient repetition.  

```F#
printfn "%s" (String.replicate 5 "falcon ")
printfn "%s" (String.concat " " (Array.create 5 "falcon"))
```

String.replicate efficiently repeats a string pattern, while Array.create  
with String.concat provides alternative repetition through arrays. Both  
methods create new immutable strings with repeated content patterns.  

## Concat list of strings

String concatenation combines multiple strings into a single result.  
F# offers various concatenation methods with different performance  
characteristics and formatting options.  

```F#
open System

let words = ["sky"; "cloud"; "cup"; "snow"; "water"; "war"; "rock"]
let output = words |> Seq.reduce (fun acc e -> (sprintf $"{acc}, {e}"))
printfn $"{output}"

let output2 = words |> List.reduce (fun acc e -> acc + Environment.NewLine + e)
printfn $"{output2}"

let output3 = String.concat ", " words
printfn $"{output3}"

let output4 = words |> String.concat ", "
printfn $"{output4}"
```

String concatenation methods vary in efficiency and functionality.  
String.concat provides optimal performance for joining collections,  
while reduce operations offer custom formatting control for complex  
joining scenarios with different delimiters and transformations.  

## String splitting and tokenization

String splitting breaks text into smaller components based on delimiters  
or patterns. This fundamental operation enables text parsing and analysis.  

```F#
open System

let text = "apple,banana,orange,grape"
let csvData = "John,25,Engineer,New York"
let sentence = "The quick brown fox jumps"

// Basic splitting
let fruits = text.Split(',')
printfn "Fruits: %A" fruits

// Split with multiple delimiters
let words = sentence.Split([|' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
printfn "Words: %A" words

// Split CSV data
let personData = csvData.Split(',')
let name, age, job, city = personData[0], personData[1], personData[2], personData[3]
printfn "Person: %s, Age: %s, Job: %s, City: %s" name age job city

// Split with string delimiter
let path = "folder/subfolder/file.txt"
let pathParts = path.Split('/')
printfn "Path parts: %A" pathParts
```

String splitting converts delimited text into arrays for processing.  
Options like RemoveEmptyEntries handle consecutive delimiters, while  
multiple delimiter characters enable flexible text parsing scenarios.  

## String trimming and whitespace handling

Whitespace management removes unnecessary spaces, tabs, and line breaks  
from string boundaries or throughout the text content.  

```F#
open System

let paddedText = "   Hello, World!   "
let mixedWhitespace = "\t\n  Welcome to F#  \r\n"
let multiSpaced = "Too    many    spaces    here"

// Basic trimming operations
printfn "Original: '%s'" paddedText
printfn "Trimmed: '%s'" (paddedText.Trim())
printfn "Trim start: '%s'" (paddedText.TrimStart())
printfn "Trim end: '%s'" (paddedText.TrimEnd())

// Trim specific characters
let customTrimmed = "***Important Message***"
printfn "Custom trim: '%s'" (customTrimmed.Trim('*'))

// Handle mixed whitespace
printfn "Mixed whitespace: '%s'" mixedWhitespace
printfn "Cleaned: '%s'" (mixedWhitespace.Trim())

// Replace multiple spaces with single spaces
let normalized = System.Text.RegularExpressions.Regex.Replace(
    multiSpaced, @"\s+", " ")
printfn "Normalized: '%s'" normalized
```

Trimming operations clean string boundaries and normalize whitespace  
formatting. Custom character trimming removes specific characters,  
while regex replacement handles complex whitespace normalization tasks.  

## String replacement and substitution

String replacement modifies text by substituting patterns with new  
content. F# supports simple replacements and complex pattern-based  
transformations for text processing tasks.  

```F#
open System
open System.Text.RegularExpressions

let originalText = "The quick brown fox jumps over the lazy dog"
let htmlText = "<p>Hello <b>world</b> and <i>everyone</i>!</p>"

// Simple string replacement
let replaced1 = originalText.Replace("fox", "cat")
printfn "Simple replace: %s" replaced1

let replaced2 = originalText.Replace("the", "a")
printfn "Replace all 'the': %s" replaced2

// Case-sensitive vs case-insensitive replacement
let caseReplace = originalText.Replace("THE", "A") // No change
let caseIgnoreReplace = Regex.Replace(originalText, "THE", "A", RegexOptions.IgnoreCase)
printfn "Case sensitive: %s" caseReplace
printfn "Case insensitive: %s" caseIgnoreReplace

// Remove HTML tags with regex
let plainText = Regex.Replace(htmlText, "<[^>]*>", "")
printfn "HTML stripped: %s" plainText

// Multiple replacements in sequence
let multiReplace = 
    originalText
        .Replace("quick", "slow")
        .Replace("brown", "red")
        .Replace("jumps", "walks")
printfn "Multiple replacements: %s" multiReplace
```

String replacement operations modify text content by substituting  
patterns with new values. Regular expressions enable complex pattern  
matching for advanced text transformations and content filtering.  

## String validation and checking

String validation verifies content format, structure, and constraints  
using built-in checks and custom validation logic for data integrity.  

```F#
open System
open System.Text.RegularExpressions

let validateEmail email =
    let pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
    Regex.IsMatch(email, pattern)

let validatePhone phone =
    let digits = Regex.Replace(phone, @"[^\d]", "")
    digits.Length = 10

let validatePassword password =
    password.Length >= 8 &&
    password |> Seq.exists Char.IsUpper &&
    password |> Seq.exists Char.IsLower &&
    password |> Seq.exists Char.IsDigit

// Test email validation
let emails = ["user@example.com"; "invalid.email"; "test@domain.org"]
emails |> List.iter (fun email ->
    printfn "Email '%s' valid: %b" email (validateEmail email))

// Test phone validation
let phones = ["123-456-7890"; "(555) 123-4567"; "12345"]
phones |> List.iter (fun phone ->
    printfn "Phone '%s' valid: %b" phone (validatePhone phone))

// Test password validation
let passwords = ["weak"; "StrongPass123"; "NoDigits"; "nocaps123"]
passwords |> List.iter (fun pwd ->
    printfn "Password '%s' valid: %b" pwd (validatePassword pwd))
```

String validation ensures data quality through format checking and  
constraint verification. Custom validation functions combine multiple  
criteria to enforce business rules and data integrity requirements.  

## String case conversion

Case conversion transforms string capitalization for consistent  
formatting, comparison operations, and user interface presentation.  

```F#
open System
open System.Globalization

let mixedCaseText = "Hello World! This is F# Programming."
let acronym = "api"
let properName = "john DOE"

// Standard case conversions
printfn "Original: %s" mixedCaseText
printfn "Upper: %s" (mixedCaseText.ToUpper())
printfn "Lower: %s" (mixedCaseText.ToLower())

// Culture-specific case conversion
let turkishText = "İstanbul"
printfn "Turkish upper (invariant): %s" (turkishText.ToUpperInvariant())
printfn "Turkish upper (Turkish): %s" 
    (turkishText.ToUpper(CultureInfo("tr-TR")))

// Title case conversion
let titleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
    mixedCaseText.ToLower())
printfn "Title case: %s" titleCase

// Custom case conversions
let capitalize (s: string) =
    if s.Length > 0 then
        s[0].ToString().ToUpper() + s[1..].ToLower()
    else s

let camelCase (s: string) =
    s.Split(' ')
    |> Array.mapi (fun i word ->
        if i = 0 then word.ToLower()
        else capitalize word)
    |> String.concat ""

printfn "Capitalized: %s" (capitalize properName)
printfn "Camel case: %s" (camelCase "convert this text")
```

Case conversion functions handle various capitalization scenarios  
including standard upper/lower, culture-specific transformations,  
title case, and custom formatting patterns for different use cases.  

## String to numeric conversions

The `int` built-in function converts a string to an integer, while  
other conversion functions handle different numeric types with error  
handling and validation capabilities.  

```f#
open System

let vals = ("2", 1, "4", 6, "11")

let a, b, c, d, e = vals
let sum = int a + b + int c + d + int e

Console.WriteLine(sum)
```

String to integer conversion uses the int function for simple cases.  
The tuple destructuring enables individual element access while mixing  
string and numeric types in calculations and data processing scenarios.  

## Safe numeric parsing

Safe parsing methods handle invalid input gracefully without throwing  
exceptions, returning option types or success indicators for validation.  

```F#
open System

let tryParseInt (s: string) =
    match Int32.TryParse(s) with
    | (true, value) -> Some value
    | (false, _) -> None

let parseWithDefault defaultValue s =
    match tryParseInt s with
    | Some value -> value
    | None -> defaultValue

// Test safe parsing
let testStrings = ["123"; "abc"; "456.78"; ""; "-789"]

testStrings |> List.iter (fun s ->
    match tryParseInt s with
    | Some value -> printfn "'%s' -> %d" s value
    | None -> printfn "'%s' -> Invalid" s)

// Parse with defaults
let numbers = testStrings |> List.map (parseWithDefault 0)
printfn "Parsed with defaults: %A" numbers

// Multiple numeric types
let parseDouble s = 
    match Double.TryParse(s) with
    | (true, value) -> Some value
    | (false, _) -> None

let parseDecimal s =
    match Decimal.TryParse(s) with
    | (true, value) -> Some value
    | (false, _) -> None

printfn "Double parse '123.45': %A" (parseDouble "123.45")
printfn "Decimal parse '$123.45': %A" (parseDecimal "$123.45")
```

Safe parsing prevents runtime exceptions by returning option types  
for validation results. Default value strategies handle parsing failures  
gracefully while maintaining application stability and data integrity.  

## Convert array ints to strings

Converting collections of numbers to strings enables formatted output  
and text-based data serialization for storage and transmission purposes.  

```f#
open System

let nums = [| 2; 4; 6; 8 |]

let output =
    nums
    |> Array.map (sprintf "%i")
    |> String.concat ","

Console.WriteLine(output)
```

Array transformation with sprintf converts integers to strings using  
format specifiers. String.concat joins the converted values with  
delimiters, creating CSV-style output for data serialization needs.  

## Numeric formatting and culture

Numeric formatting provides precise control over number representation  
including decimal places, thousands separators, and cultural conventions.  

```F#
open System
open System.Globalization

let numbers = [1234.56; -9876.54; 0.123; 42.0]

// Standard numeric formats
numbers |> List.iter (fun n ->
    printfn "Number: %.2f" n
    printfn "Currency: %s" (n.ToString("C"))
    printfn "Fixed point: %s" (n.ToString("F2"))
    printfn "Scientific: %s" (n.ToString("E2"))
    printfn "Percentage: %s" ((n/100.0).ToString("P1"))
    printfn "---")

// Culture-specific formatting
let amount = 12345.67
let usCulture = CultureInfo("en-US")
let germanCulture = CultureInfo("de-DE")
let japaneseCulture = CultureInfo("ja-JP")

printfn "US format: %s" (amount.ToString("C", usCulture))
printfn "German format: %s" (amount.ToString("C", germanCulture))
printfn "Japanese format: %s" (amount.ToString("C", japaneseCulture))

// Custom format strings
let customFormat = "#,##0.00;(#,##0.00);Zero"
printfn "Custom positive: %s" ((12345.67).ToString(customFormat))
printfn "Custom negative: %s" ((-12345.67).ToString(customFormat))
printfn "Custom zero: %s" ((0.0).ToString(customFormat))
```

Numeric formatting supports standard format strings and cultural  
conventions for international applications. Custom format patterns  
provide precise control over positive, negative, and zero value display.  

## Building/formatting strings

String formatting combines values into structured text using various  
approaches including concatenation, sprintf, interpolation, and  
StringBuilder for different performance and readability requirements.  

```f#
open System
open System.Text

let name = "John Doe"
let age = 33

let msg1 = name + " is " + string age + " years old"
printfn $"{msg1}"

let msg2 = sprintf "%s is %d years old" name age
printfn $"{msg2}"

let msg3 = $"{name} is {age} years old"
printfn $"{msg3}"

let msg4 = String.Format("{0} is {1} years old", name, age)
printfn $"{msg4}"

let builder = StringBuilder()
let msg5 = builder.AppendFormat("{0} is {1} years old", name, age)
printfn $"{msg5}"
```

String formatting methods offer different approaches for combining  
values into text. String interpolation provides modern syntax while  
sprintf offers type safety and StringBuilder optimizes performance  
for multiple concatenation operations in loops or complex scenarios.  

## Advanced string formatting

Advanced formatting techniques handle complex scenarios including  
conditional formatting, alignment, padding, and structured data output.  

```F#
open System

let products = [
    ("Laptop", 999.99, 5)
    ("Mouse", 29.95, 50)
    ("Keyboard", 79.99, 12)
    ("Monitor", 299.99, 8)
]

// Table formatting with alignment
printfn "%-12s %10s %8s" "Product" "Price" "Stock"
printfn "%s" (String('-', 32))

products |> List.iter (fun (name, price, stock) ->
    printfn "%-12s %10.2f %8d" name price stock)

// Conditional formatting
let formatStock stock =
    match stock with
    | s when s > 20 -> sprintf "In Stock (%d)" s
    | s when s > 0 -> sprintf "Low Stock (%d)" s
    | _ -> "Out of Stock"

printfn "\nStock Status:"
products |> List.iter (fun (name, _, stock) ->
    printfn "%s: %s" name (formatStock stock))

// Multi-line string building
let buildReport title items =
    let header = sprintf "=== %s ===" title
    let separator = String('=', header.Length)
    let itemLines = 
        items 
        |> List.mapi (fun i item -> sprintf "%d. %s" (i + 1) item)
        |> String.concat "\n"
    
    sprintf "%s\n%s\n%s" header itemLines separator

let report = buildReport "Shopping List" ["Apples"; "Bread"; "Milk"]
printfn "%s" report
```

Advanced formatting creates structured output with alignment, padding,  
and conditional content. Multi-line formatting builds complex documents  
and reports with consistent styling and professional presentation.  

## StringBuilder

StringBuilder provides efficient string building for scenarios requiring  
multiple concatenations, loops, or dynamic string construction with  
optimal memory usage and performance characteristics.  

```F#
open System.Text

let builder = StringBuilder()

Printf.bprintf builder "There are %d " 3
Printf.bprintf builder "hawks in the sky"

printfn "%s" (builder.ToString())
```

```F#
open System.Text

let buf = StringBuilder()

buf.Append("There are " ) |> ignore
buf.Append("three ") |> ignore 
buf.Append("eagles in the sky") |> ignore

printfn $"%s{buf.ToString()}"
```

StringBuilder optimizes string concatenation by maintaining an internal  
buffer that grows as needed. The Printf.bprintf function provides  
formatted output directly to StringBuilder instances for complex  
string building scenarios with type-safe formatting operations.  

## StringBuilder performance optimization

StringBuilder offers advanced features for high-performance string  
operations including capacity management, batch operations, and  
memory-efficient text processing for large-scale applications.  

```F#
open System.Text
open System.Diagnostics

// Performance comparison: String concatenation vs StringBuilder
let measureStringConcat count =
    let sw = Stopwatch.StartNew()
    let mutable result = ""
    for i in 1..count do
        result <- result + sprintf "Item %d, " i
    sw.Stop()
    (result, sw.ElapsedMilliseconds)

let measureStringBuilder count =
    let sw = Stopwatch.StartNew()
    let sb = StringBuilder(count * 20) // Pre-allocate capacity
    for i in 1..count do
        sb.AppendFormat("Item {0}, ", i) |> ignore
    let result = sb.ToString()
    sw.Stop()
    (result, sw.ElapsedMilliseconds)

// Test with different counts
[100; 1000; 5000] |> List.iter (fun count ->
    let (_, concatTime) = measureStringConcat count
    let (_, builderTime) = measureStringBuilder count
    printfn "Count %d: Concat=%dms, Builder=%dms, Ratio=%.2fx" 
        count concatTime builderTime (float concatTime / float builderTime))

// StringBuilder with initial capacity and batch operations
let buildLargeString items =
    let estimatedLength = items |> List.sumBy (fun s -> s.Length + 10)
    let sb = StringBuilder(estimatedLength)
    
    sb.AppendLine("Report Generated") |> ignore
    sb.AppendLine(String('=', 20)) |> ignore
    
    items |> List.iteri (fun i item ->
        sb.AppendFormat("{0,3}. {1}", i + 1, item).AppendLine() |> ignore)
    
    sb.AppendLine(String('=', 20)) |> ignore
    sb.AppendFormat("Total Items: {0}", items.Length) |> ignore
    sb.ToString()

let sampleItems = ["First item"; "Second item"; "Third item with more text"]
let report = buildLargeString sampleItems
printfn "%s" report
```

StringBuilder performance advantages become significant with many  
concatenation operations. Pre-allocating capacity prevents buffer  
reallocations, while batch operations optimize memory usage for  
large-scale text processing and report generation scenarios.  

## String interpolation

F# 5 introduced string interpolation providing modern syntax for  
embedding expressions directly in string literals with compile-time  
type checking and efficient runtime performance.  

```F#
let name = "John Doe"
let occupation = "gardener"

let msg = $"{name} is an {occupation}"
printfn $"{msg}"

printfn $"5 * 8 = {5 * 8}"

printfn $"{58:C}"
printfn $"{58:X}"
```

Typed interpolated strings

```F#
let name = "John Doe"
let age = 34

printfn $"Name: %s{name}, Age: %d{age}"
```

String interpolation combines values and expressions directly within  
string literals. Format specifiers control output appearance while  
typed interpolation ensures compile-time type safety for robust  
code and prevents common formatting errors in production applications.  

## Advanced string interpolation

Advanced interpolation techniques handle complex expressions, formatting  
options, and conditional content for sophisticated text generation  
scenarios with maintainable and readable code.  

```F#
open System

let users = [
    {| Name = "Alice"; Score = 95; Active = true |}
    {| Name = "Bob"; Score = 87; Active = false |}
    {| Name = "Charlie"; Score = 92; Active = true |}
]

// Interpolation with expressions
users |> List.iter (fun user ->
    printfn $"{user.Name}: {user.Score}% ({if user.Active then "Active" else "Inactive"})")

// Interpolation with function calls
let formatPercentage value = sprintf "%.1f%%" (value * 100.0)
let accuracy = 0.847
printfn $"Model accuracy: {formatPercentage accuracy}"

// Multi-line interpolated strings
let temperature = 23.5
let humidity = 65
let weatherReport = $"""
Weather Report:
  Temperature: {temperature:F1}°C
  Humidity: {humidity}%%
  Comfort: {if temperature > 20.0 && humidity < 70 then "Pleasant" else "Uncomfortable"}
  Status: {DateTime.Now:yyyy-MM-dd HH:mm}
"""
printfn "%s" weatherReport

// Interpolation with collections
let numbers = [1; 2; 3; 4; 5]
let sum = numbers |> List.sum
printfn $"Numbers {numbers} sum to {sum} (average: {float sum / float numbers.Length:F2})"

// Conditional interpolation
let showDetails = true
let itemCount = 42
let summary = $"Found {itemCount} items{if showDetails then $" (details: processed in {DateTime.Now:HH:mm})" else ""}"
printfn "%s" summary
```

Advanced string interpolation supports complex expressions, nested  
interpolations, and conditional content. Multi-line interpolated strings  
maintain formatting while embedding dynamic values, enabling readable  
and maintainable text generation for reports and user interfaces.  

## Regular expressions with strings

Regular expressions provide powerful pattern matching capabilities for  
string validation, extraction, and transformation tasks requiring  
sophisticated text processing and data parsing operations.  

```F#
open System.Text.RegularExpressions

let phonePattern = @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}"
let emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
let urlPattern = @"https?://[^\s/$.?#].[^\s]*"

let text = """
Contact info:
Phone: (555) 123-4567
Email: user@example.com
Website: https://www.example.com
Alt Phone: 555.987.6543
"""

// Find matches
let phoneMatches = Regex.Matches(text, phonePattern)
let emailMatches = Regex.Matches(text, emailPattern)
let urlMatches = Regex.Matches(text, urlPattern)

printfn "Phone numbers found:"
phoneMatches |> Seq.iter (fun m -> printfn "  %s" m.Value)

printfn "Email addresses found:"
emailMatches |> Seq.iter (fun m -> printfn "  %s" m.Value)

printfn "URLs found:"
urlMatches |> Seq.iter (fun m -> printfn "  %s" m.Value)

// Extract with named groups
let logPattern = @"(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) (?<level>\w+): (?<message>.*)"
let logEntry = "2023-12-01 14:30:25 ERROR: Database connection failed"

let logMatch = Regex.Match(logEntry, logPattern)
if logMatch.Success then
    printfn "Timestamp: %s" logMatch.Groups["timestamp"].Value
    printfn "Level: %s" logMatch.Groups["level"].Value
    printfn "Message: %s" logMatch.Groups["message"].Value

// Replace with regex
let censorPattern = @"\b\d{4}-\d{4}-\d{4}-\d{4}\b"
let sensitiveText = "Credit card 1234-5678-9012-3456 was declined"
let censored = Regex.Replace(sensitiveText, censorPattern, "****-****-****-****")
printfn "Censored: %s" censored
```

Regular expressions enable complex pattern matching with capture groups,  
replacement operations, and data extraction from structured text. Named  
groups provide readable access to matched components while replacement  
patterns enable sophisticated text transformation and sanitization.  

## String encoding and Unicode

String encoding handles character representation, Unicode support, and  
text conversion between different character sets for international  
applications and data interchange scenarios.  

```F#
open System
open System.Text

// Unicode string handling
let unicodeText = "Hello 世界 🌍 Café naïve résumé"
let emojiText = "😀 👍 🎉 ❤️ 🚀"

printfn "Unicode text: %s" unicodeText
printfn "Length in chars: %d" unicodeText.Length
printfn "Length in bytes (UTF-8): %d" (Encoding.UTF8.GetByteCount(unicodeText))

// Character enumeration with Unicode categories
unicodeText.ToCharArray()
|> Array.iter (fun c ->
    let category = Char.GetUnicodeCategory(c)
    printfn "Char '%c' (U+%04X) - Category: %A" c (int c) category)

// Encoding conversions
let originalText = "Héllo Wörld!"
let utf8Bytes = Encoding.UTF8.GetBytes(originalText)
let utf16Bytes = Encoding.Unicode.GetBytes(originalText)
let asciiBytes = Encoding.ASCII.GetBytes(originalText)

printfn "Original: %s" originalText
printfn "UTF-8 bytes: %A" utf8Bytes
printfn "UTF-16 bytes: %A" utf16Bytes
printfn "ASCII bytes: %A" asciiBytes

// Convert back from bytes
let fromUtf8 = Encoding.UTF8.GetString(utf8Bytes)
let fromUtf16 = Encoding.Unicode.GetString(utf16Bytes)
let fromAscii = Encoding.ASCII.GetString(asciiBytes)

printfn "From UTF-8: %s" fromUtf8
printfn "From UTF-16: %s" fromUtf16
printfn "From ASCII: %s" fromAscii

// Normalize Unicode text
let denormalizedText = "café" // 'é' as combining characters
let normalizedNfc = denormalizedText.Normalize(NormalizationForm.FormC)
let normalizedNfd = denormalizedText.Normalize(NormalizationForm.FormD)

printfn "Denormalized length: %d" denormalizedText.Length
printfn "NFC normalized length: %d" normalizedNfc.Length
printfn "NFD normalized length: %d" normalizedNfd.Length
```

Unicode support ensures proper handling of international text including  
multi-byte characters, emoji, and combining characters. Encoding  
conversions handle data interchange while normalization ensures  
consistent text representation for comparison and storage operations.  

## String performance considerations

String performance optimization involves understanding immutability,  
memory allocation patterns, and choosing appropriate methods for  
different scenarios to achieve optimal application performance.  

```F#
open System
open System.Diagnostics
open System.Text

// Measure string concatenation performance
let measureOperation label operation =
    let sw = Stopwatch.StartNew()
    let result = operation ()
    sw.Stop()
    printfn "%s: %d ms" label sw.ElapsedMilliseconds
    result

// Compare concatenation methods
let iterations = 10000

// String concatenation (inefficient)
measureOperation "String concatenation" (fun () ->
    let mutable result = ""
    for i in 1..iterations do
        result <- result + $"Item {i} "
    result.Length) |> ignore

// StringBuilder (efficient)
measureOperation "StringBuilder" (fun () ->
    let sb = StringBuilder(iterations * 10)
    for i in 1..iterations do
        sb.Append($"Item {i} ") |> ignore
    sb.Length) |> ignore

// String.concat (most efficient for collections)
measureOperation "String.concat" (fun () ->
    let items = [1..iterations] |> List.map (sprintf "Item %d ")
    let result = String.concat "" items
    result.Length) |> ignore

// String interning for repeated strings
let intern1 = String.Intern("repeated string")
let intern2 = String.Intern("repeated string")
printfn "Interned strings equal reference: %b" (obj.ReferenceEquals(intern1, intern2))

// Memory-efficient string operations
let largeText = String.replicate 1000 "This is a test string with some content. "

// Efficient substring operations
measureOperation "Substring" (fun () ->
    largeText.Substring(100, 200).Length) |> ignore

// Efficient string comparison
measureOperation "Ordinal comparison" (fun () ->
    String.Equals(largeText, largeText, StringComparison.Ordinal)) |> ignore

measureOperation "Culture comparison" (fun () ->
    String.Equals(largeText, largeText, StringComparison.CurrentCulture)) |> ignore

// ReadOnlySpan for zero-allocation operations
let spanText = "The quick brown fox jumps"
let span = spanText.AsSpan(4, 5) // "quick"
printfn "Span content: %s" (span.ToString())
```

String performance optimization focuses on minimizing allocations and  
choosing efficient methods for specific scenarios. StringBuilder excels  
for multiple concatenations, String.concat for collections, and spans  
provide zero-allocation substring operations for high-performance code.  

## String pattern matching

Pattern matching with strings enables powerful text analysis and  
processing through F#'s discriminated unions, active patterns, and  
guard clauses for elegant and type-safe string handling.  

```F#
open System

type FileExtension =
    | Image of string
    | Document of string  
    | Code of string
    | Unknown of string

let classifyFile (filename: string) =
    match filename.ToLower() with
    | s when s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") -> Image filename
    | s when s.EndsWith(".pdf") || s.EndsWith(".doc") || s.EndsWith(".txt") -> Document filename
    | s when s.EndsWith(".fs") || s.EndsWith(".cs") || s.EndsWith(".py") -> Code filename
    | _ -> Unknown filename

let processFile fileType =
    match fileType with
    | Image name -> $"Processing image: {name}"
    | Document name -> $"Opening document: {name}"
    | Code name -> $"Editing source code: {name}"
    | Unknown name -> $"Unknown file type: {name}"

// Test file classification
let files = ["photo.jpg"; "report.pdf"; "program.fs"; "data.xyz"]
files |> List.iter (fun file ->
    let classified = classifyFile file
    let message = processFile classified
    printfn "%s" message)

// Active patterns for string parsing
let (|StartsWith|_|) (prefix: string) (s: string) =
    if s.StartsWith(prefix) then Some s else None

let (|EndsWith|_|) (suffix: string) (s: string) =
    if s.EndsWith(suffix) then Some s else None

let (|Contains|_|) (substring: string) (s: string) =
    if s.Contains(substring) then Some s else None

let analyzeUrl url =
    match url with
    | StartsWith "https://" -> "Secure website"
    | StartsWith "http://" -> "Insecure website"
    | StartsWith "ftp://" -> "File transfer protocol"
    | Contains "@" -> "Email address"
    | EndsWith ".com" | EndsWith ".org" | EndsWith ".net" -> "Domain name"
    | _ -> "Unknown format"

let urls = [
    "https://www.example.com"
    "http://insecure.site"
    "user@domain.com"
    "example.org"
    "unknown-format"
]

urls |> List.iter (fun url ->
    printfn "%s -> %s" url (analyzeUrl url))
```

String pattern matching combines discriminated unions and active patterns  
for sophisticated text classification and processing. Active patterns  
provide reusable string matching logic while maintaining type safety  
and enabling complex string analysis with clear, readable code.  

## String manipulation utilities

Utility functions for common string operations provide reusable  
building blocks for text processing, data cleaning, and content  
transformation in real-world applications.  

```F#
open System
open System.Text.RegularExpressions

module StringUtils =
    
    let truncate maxLength (s: string) =
        if s.Length <= maxLength then s
        else s.Substring(0, maxLength - 3) + "..."
    
    let slugify (s: string) =
        s.ToLowerInvariant()
            .Replace(" ", "-")
            |> fun s -> Regex.Replace(s, @"[^a-z0-9\-]", "")
            |> fun s -> Regex.Replace(s, @"-+", "-")
            |> fun s -> s.Trim('-')
    
    let wordCount (s: string) =
        s.Split([|' '; '\t'; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries).Length
    
    let extractNumbers (s: string) =
        Regex.Matches(s, @"-?\d+(?:\.\d+)?")
        |> Seq.cast<Match>
        |> Seq.map (fun m -> Double.Parse(m.Value))
        |> Seq.toList
    
    let removeHtmlTags (html: string) =
        Regex.Replace(html, @"<[^>]*>", "")
    
    let capitalizeWords (s: string) =
        s.Split(' ')
        |> Array.map (fun word ->
            if word.Length > 0 then
                word.[0].ToString().ToUpper() + word.[1..].ToLower()
            else word)
        |> String.concat " "
    
    let levenshteinDistance (s1: string) (s2: string) =
        let len1, len2 = s1.Length, s2.Length
        let d = Array2D.create (len1 + 1) (len2 + 1) 0
        
        for i in 0..len1 do d.[i, 0] <- i
        for j in 0..len2 do d.[0, j] <- j
        
        for i in 1..len1 do
            for j in 1..len2 do
                let cost = if s1.[i-1] = s2.[j-1] then 0 else 1
                d.[i, j] <- min (min (d.[i-1, j] + 1) (d.[i, j-1] + 1)) (d.[i-1, j-1] + cost)
        
        d.[len1, len2]

// Test utility functions
open StringUtils

let testText = "The Quick Brown Fox Jumps Over 123 Lazy Dogs!"
let htmlText = "<p>Hello <b>world</b>! Check out <a href='#'>this link</a>.</p>"

printfn "Original: %s" testText
printfn "Truncated: %s" (truncate 20 testText)
printfn "Slugified: %s" (slugify testText)
printfn "Word count: %d" (wordCount testText)
printfn "Numbers: %A" (extractNumbers testText)
printfn "Capitalized: %s" (capitalizeWords testText.ToLower())

printfn "\nHTML: %s" htmlText
printfn "Plain text: %s" (removeHtmlTags htmlText)

// String similarity
let similar1 = "kitten"
let similar2 = "sitting"
printfn "\nLevenshtein distance between '%s' and '%s': %d" 
    similar1 similar2 (levenshteinDistance similar1 similar2)
```

String utility functions encapsulate common text processing operations  
for reuse across applications. These utilities handle truncation, URL  
slugification, content extraction, and similarity measurement for  
practical text processing needs in web development and data analysis.  

## Conclusion

F# strings provide comprehensive text processing capabilities through  
immutable string types, rich .NET integration, and functional programming  
patterns. From basic operations to advanced Unicode handling and  
performance optimization, F# offers elegant solutions for all text  
processing needs in modern applications.  

This guide demonstrated essential string operations including creation,  
manipulation, formatting, validation, and pattern matching. Performance  
considerations, encoding support, and utility functions enable robust  
text processing for international applications and data-intensive  
scenarios while maintaining code clarity and type safety.  

# F# regular expressions

Regular expressions are powerful pattern matching tools for text processing,  
validation, extraction, and transformation operations. F# provides seamless  
integration with .NET's System.Text.RegularExpressions namespace, enabling  
sophisticated string manipulation through pattern-based operations.  

Regular expressions define search patterns using special characters and  
metacharacters that represent character classes, quantifiers, anchors,  
and grouping constructs. These patterns can match, extract, replace, or  
split text based on complex rules, making them essential for data  
validation, parsing, and text processing tasks.  

F# combines regex power with functional programming paradigms, allowing  
pattern matching, sequence operations, and immutable data structures  
to work seamlessly with regular expression results. The type system  
ensures compile-time safety while maintaining regex flexibility.  

Common use cases include email validation, phone number formatting,  
log file parsing, data extraction from structured text, input  
sanitization, and content transformation for web applications  
and data processing pipelines.  



## Matching with IsMatch

Basic pattern matching tests whether text contains a specific pattern  
without extracting matched content. IsMatch returns boolean results  
for validation and filtering operations.  

```f#
open System
open System.Text.RegularExpressions

let words =
    [ "Seven"
      "even"
      "Maven"
      "Amen"
      "eleven" ]

let rx = Regex(@".even", RegexOptions.Compiled)

words
|> List.map
    (fun e ->
        if rx.IsMatch(e) then
            Console.WriteLine($"{e} matches")
        else
            Console.WriteLine($"{e} does not match"))
```

The IsMatch method performs boolean pattern testing without returning  
matched content. The pattern ".even" matches any character followed by  
"even", demonstrating basic wildcard usage for text filtering operations.  

## Alternations

Alternation patterns match multiple alternative expressions using the  
pipe operator. This enables flexible matching against predefined  
sets of acceptable values.  

```f#
open System
open System.Text.RegularExpressions

let users =
    [ "Jane"
      "Thomas"
      "Robert"
      "Lucy"
      "Beky"
      "John"
      "Peter"
      "Andy" ]

let rx =
    Regex("Jane|Beky|Robert", RegexOptions.Compiled)


users |> List.filter rx.IsMatch |> List.iter Console.WriteLine
```

Alternation uses the pipe character to specify multiple possible matches.  
The pattern "Jane|Beky|Robert" matches any of these three names exactly,  
enabling efficient filtering of predefined options from collections.  

## Matching currency symbols

Unicode property classes match specific character categories like  
currency symbols across different languages and writing systems.  
Property classes provide international text processing capabilities.  

```f#
open System
open System.Text.RegularExpressions

Console.OutputEncoding = Text.Encoding.UTF8

let content = @"Currency symbols: ฿ Thailand bath, ₹  Indian rupee, 
    ₾ Georgian lari, $ Dollar, € Euro, ¥ Yen, £ Pound Sterling";

let pattern = @"\p{Sc}";

let rx = Regex(pattern, RegexOptions.Compiled)
let matches = rx.Matches(content)
              |> Seq.map (fun m -> m.Value, m.Index)

matches
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

The Unicode property class \p{Sc} matches any currency symbol character.  
This pattern recognizes international currency symbols regardless of  
their specific encoding, providing robust globalization support.  

## Find all matches and their indexes

Match collections return all pattern occurrences with position  
information. Multiple techniques exist for extracting matches  
and processing results efficiently.  

Example I

```F#
open System.Text.RegularExpressions

let content =
    @"Foxes are omnivorous mammals belonging to several genera
of the family Canidae. Foxes have a flattened skull, upright triangular ears,
a pointed, slightly upturned snout, and a long bushy tail. Foxes live on every
continent except Antarctica. By far the most common and widespread species of
fox is the red fox."


let found =
    seq {
        for m in Regex.Matches(content, "(?i)fox(es)?") do
            yield m.Value, m.Index
    }

found
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

Sequence expressions provide functional syntax for extracting matches.  
The pattern "(?i)fox(es)?" uses case-insensitive matching with optional  
plural form, demonstrating flags and optional group constructs.  

Example II

```f#
open System.Text.RegularExpressions

let content =
    @"Foxes are omnivorous mammals belonging to several genera
of the family Canidae. Foxes have a flattened skull, upright triangular ears,
a pointed, slightly upturned snout, and a long bushy tail. Foxes live on every
continent except Antarctica. By far the most common and widespread species of
fox is the red fox."


let found =
    Regex.Matches(content, "(?i)fox(es)?")
    |> Seq.map (fun m -> m.Value, m.Index)

found
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

The Regex.Matches method returns MatchCollection objects converted to  
sequences for functional processing. Both approaches yield identical  
results with different syntactic styles for match extraction.  

## Boundaries

Word boundaries ensure pattern matching occurs at word edges,  
preventing partial matches within larger words. Boundaries  
improve matching precision for whole-word searches.  

```f#
open System.Text.RegularExpressions

let text = "This island is beautiful"

let rx = Regex(@"\bis\b", RegexOptions.Compiled)

let matches =
    rx.Matches(text)
    |> Seq.map (fun m -> m.Value, m.Index)

matches
|> Seq.iter (fun (e, idx) -> printfn "%s at %d" e idx)
```

The \b metacharacter represents word boundaries, matching positions  
between word characters and non-word characters. This ensures "is"  
matches as a complete word, not as part of "This" or "island".  

## Capturing groups

Capturing groups extract specific parts of matched patterns using  
parentheses. Groups enable structured data extraction from  
formatted text with positional or named access.  

```f#
open System.Text.RegularExpressions

let sites =
    [ "webcode.me"
      "zetcode.com"
      "spoznaj"
      "freebsd.org"
      "netbsd.org" ]

let rx =
    Regex(@"(\w+)\.(\w+)", RegexOptions.Compiled)


let check e =
    let m = rx.Match(e)
    (m.Value, m.Groups.[1], m.Groups.[2])


// let found = sites |> List.map (fun e -> check (e))
let found = sites |> List.map check

printfn "%A" found
```

Capturing groups use parentheses to define extractable pattern segments.  
The pattern "(\w+)\.(\w+)" captures domain name and extension separately,  
enabling structured parsing of formatted text like website addresses.  

## Character classes

Character classes match specific character sets using square brackets.  
Predefined classes like \d, \w, and \s provide common character  
matching patterns for digits, word characters, and whitespace.  

```f#
open System.Text.RegularExpressions

let testStrings = [
    "abc123"
    "hello world!"  
    "user@domain.com"
    "2023-12-01"
    "  spaces  "
]

// Custom character class
let alphanumeric = Regex(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled)

// Predefined character classes  
let hasDigits = Regex(@"\d", RegexOptions.Compiled)
let hasWhitespace = Regex(@"\s", RegexOptions.Compiled)
let wordChars = Regex(@"^\w+$", RegexOptions.Compiled)

testStrings |> List.iter (fun s ->
    printfn "String: '%s'" s
    printfn "  Alphanumeric only: %b" (alphanumeric.IsMatch(s))
    printfn "  Contains digits: %b" (hasDigits.IsMatch(s))
    printfn "  Contains whitespace: %b" (hasWhitespace.IsMatch(s))
    printfn "  Word characters only: %b" (wordChars.IsMatch(s))
    printfn "")
```

Character classes enable flexible character matching with custom sets  
and predefined shortcuts. Brackets define custom ranges while  
metacharacters like \d, \w, \s match common character categories  
for validation and parsing operations.  

## Quantifiers

Quantifiers specify how many times patterns should repeat using  
special symbols. They control matching frequency with precise  
repetition counts or flexible ranges.  

```f#
open System.Text.RegularExpressions

let phoneNumbers = [
    "123-456-7890"
    "1234567890"  
    "(555) 123-4567"
    "555.123.4567"
    "12-34-56"
    "+1-555-123-4567"
]

// Various quantifier patterns
let exactDigits = Regex(@"^\d{10}$", RegexOptions.Compiled)
let rangeDigits = Regex(@"^\d{3,4}$", RegexOptions.Compiled) 
let oneOrMore = Regex(@"^\d+$", RegexOptions.Compiled)
let zeroOrMore = Regex(@"^\d*$", RegexOptions.Compiled)
let optional = Regex(@"^\+?1?-?\d{3}-?\d{3}-?\d{4}$", RegexOptions.Compiled)

phoneNumbers |> List.iter (fun number ->
    printfn "Number: %s" number
    printfn "  Exactly 10 digits: %b" (exactDigits.IsMatch(number.Replace("-", "").Replace(".", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "")))
    printfn "  Valid phone format: %b" (optional.IsMatch(number))
    printfn "")
```

Quantifiers control pattern repetition with {n} for exact counts,  
{n,m} for ranges, + for one or more, * for zero or more, and ?  
for optional matches. These enable flexible pattern matching  
for variable-length content.  

## Anchors

Anchors match positions rather than characters, ensuring patterns  
occur at specific text locations like line beginnings, endings,  
or word boundaries for precise matching control.  

```f#
open System.Text.RegularExpressions

let textLines = [
    "Hello world"
    "world peace" 
    "Another world here"
    "worldly goods"
    "brave new world"
]

// Position anchors
let startsWith = Regex(@"^world", RegexOptions.Compiled)
let endsWith = Regex(@"world$", RegexOptions.Compiled)  
let wholeWord = Regex(@"\bworld\b", RegexOptions.Compiled)
let entireLine = Regex(@"^world$", RegexOptions.Compiled)

textLines |> List.iter (fun line ->
    printfn "Line: '%s'" line
    printfn "  Starts with 'world': %b" (startsWith.IsMatch(line))
    printfn "  Ends with 'world': %b" (endsWith.IsMatch(line))
    printfn "  Contains whole word 'world': %b" (wholeWord.IsMatch(line))
    printfn "  Is exactly 'world': %b" (entireLine.IsMatch(line))
    printfn "")
```

Anchors provide positional matching with ^ for line start, $ for  
line end, \b for word boundaries, and \A, \Z for absolute string  
boundaries. These ensure patterns match at intended positions  
within text for accurate validation.  

## Escape sequences

Special characters require escaping to match literally rather than  
as metacharacters. Backslash escaping and verbatim strings  
handle regex special character conflicts.  

```f#
open System.Text.RegularExpressions

let specialTexts = [
    "Price: $19.99"
    "Math: 2 + 2 = 4"
    "Question: What?"  
    "Path: C:\\Users\\john"
    "Regex: [a-z]+"
    "Email: user@domain.com"
]

// Escaped special characters
let dollarPrice = Regex(@"\$\d+\.\d{2}", RegexOptions.Compiled)
let mathExpression = Regex(@"\d+ \+ \d+ = \d+", RegexOptions.Compiled)
let questionMark = Regex(@"\?", RegexOptions.Compiled)
let windowsPath = Regex(@"[A-Z]:\\[^<>:""|\?\*]+", RegexOptions.Compiled)
let regexPattern = Regex(@"\[[^\]]+\]\+", RegexOptions.Compiled)

specialTexts |> List.iter (fun text ->
    printfn "Text: '%s'" text
    printfn "  Contains dollar price: %b" (dollarPrice.IsMatch(text))
    printfn "  Contains math expression: %b" (mathExpression.IsMatch(text))
    printfn "  Contains question mark: %b" (questionMark.IsMatch(text))
    printfn "  Contains Windows path: %b" (windowsPath.IsMatch(text))
    printfn "  Contains regex pattern: %b" (regexPattern.IsMatch(text))
    printfn "")
```

Special regex characters like $, +, ?, \, [, ] require backslash  
escaping for literal matching. Verbatim strings (@"") in F# help  
avoid double escaping, making patterns more readable and  
maintainable for complex expressions.  

## Named groups

Named capturing groups provide readable access to matched segments  
using descriptive names instead of numeric indices, improving  
code maintainability and self-documenting regex patterns.  

```f#
open System.Text.RegularExpressions

let logEntries = [
    "2023-12-01 14:30:25 ERROR: Database connection failed"
    "2023-12-01 14:31:10 INFO: User authentication successful"  
    "2023-12-01 14:31:45 WARNING: High memory usage detected"
    "2023-12-01 14:32:00 DEBUG: Processing user request"
]

let logPattern = @"(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}) (?<level>\w+): (?<message>.*)"
let logRegex = Regex(logPattern, RegexOptions.Compiled)

logEntries |> List.iter (fun entry ->
    let match' = logRegex.Match(entry)
    if match'.Success then
        printfn "Entry parsed:"
        printfn "  Date: %s" match'.Groups["date"].Value
        printfn "  Time: %s" match'.Groups["time"].Value  
        printfn "  Level: %s" match'.Groups["level"].Value
        printfn "  Message: %s" match'.Groups["message"].Value
        printfn "")
```

Named groups use (?<name>pattern) syntax for meaningful group  
identification. This provides clear, maintainable access to  
captured content compared to positional indexing, especially  
valuable for complex parsing operations.  

## Greedy vs lazy matching

Quantifiers exhibit greedy behavior by default, matching as much  
text as possible. Lazy quantifiers minimize matches using the  
question mark modifier for precise content extraction.  

```f#
open System.Text.RegularExpressions

let htmlText = """<div>First content</div><div>Second content</div><div>Third content</div>"""
let xmlData = """<tag attr="value1">Content 1</tag><tag attr="value2">Content 2</tag>"""

// Greedy matching  
let greedyDiv = Regex(@"<div>.*</div>", RegexOptions.Compiled)
let greedyTag = Regex(@"<tag.*>", RegexOptions.Compiled)

// Lazy matching
let lazyDiv = Regex(@"<div>.*?</div>", RegexOptions.Compiled) 
let lazyTag = Regex(@"<tag.*?>", RegexOptions.Compiled)

printfn "HTML: %s" htmlText
printfn "Greedy div match: %s" (greedyDiv.Match(htmlText).Value)
printfn "Lazy div matches:"
lazyDiv.Matches(htmlText) |> Seq.iter (fun m -> printfn "  %s" m.Value)

printfn "\nXML: %s" xmlData  
printfn "Greedy tag match: %s" (greedyTag.Match(xmlData).Value)
printfn "Lazy tag matches:"
lazyTag.Matches(xmlData) |> Seq.iter (fun m -> printfn "  %s" m.Value)
```

Greedy quantifiers (* + {n,}) match maximum possible text while  
lazy quantifiers (*? +? {n,}?) match minimum required text.  
Lazy matching prevents over-capturing in structured content  
like HTML, XML, or quoted strings.  

## Replace operations

Regex replacement transforms text by substituting matched patterns  
with new content. Replacement patterns can include captured groups  
for dynamic content transformation based on matched text.  

```f#
open System.Text.RegularExpressions

let originalText = """
Contact John Doe at john.doe@email.com or call (555) 123-4567.
Alternatively, reach Jane Smith via jane.smith@company.org or (555) 987-6543.
Visit our website at https://www.example.com for more information.
"""

// Simple replacement
let phonePattern = @"\(\d{3}\) \d{3}-\d{4}"
let emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
let urlPattern = @"https?://[^\s]+"

let phoneMasked = Regex.Replace(originalText, phonePattern, "[PHONE]")
let emailMasked = Regex.Replace(originalText, emailPattern, "[EMAIL]")
let urlMasked = Regex.Replace(originalText, urlPattern, "[URL]")

printfn "Original text:"
printfn "%s" originalText

printfn "Phone numbers masked:"
printfn "%s" phoneMasked

// Replacement with captured groups
let namePattern = @"(\w+) (\w+)"
let nameReversed = Regex.Replace(originalText, namePattern, "$2, $1")

printfn "Names reversed (Last, First):"
printfn "%s" nameReversed

// Complex replacement with function
let datePattern = @"\d{4}-\d{2}-\d{2}"
let formatDate (m: Match) = 
    let date = System.DateTime.Parse(m.Value)
    date.ToString("MMMM d, yyyy")

let textWithDates = "Meeting on 2023-12-01 and follow-up on 2024-01-15."
let formattedDates = Regex.Replace(textWithDates, datePattern, formatDate)
printfn "Formatted dates: %s" formattedDates
```

Replace operations substitute matched patterns with static text  
or dynamic content using $1, $2 group references. MatchEvaluator  
functions enable complex transformations based on matched  
content for sophisticated text processing.  

## Split operations

Regex splitting divides text at pattern boundaries, providing  
flexible tokenization beyond simple character delimiters.  
Split operations handle complex separators and preserve  
or remove delimiter content.  

```f#
open System.Text.RegularExpressions

let csvData = "John,Doe,30,Engineer;Jane,Smith,25,Designer;Bob,Wilson,35,Manager"
let logData = "2023-12-01 14:30:25|INFO|User login||2023-12-01 14:30:26|ERROR|Connection failed"
let sentenceText = "First sentence. Second sentence! Third sentence? Fourth sentence."

// Simple split on punctuation
let sentences = Regex.Split(sentenceText, @"[.!?]\s*")
                |> Array.filter (fun s -> not (System.String.IsNullOrWhiteSpace(s)))

printfn "Sentences:"
sentences |> Array.iteri (fun i s -> printfn "%d: %s" (i+1) s)

// Split on multiple delimiters
let csvRecords = Regex.Split(csvData, @"[;,]")
printfn "\nCSV fields:"
csvRecords |> Array.iteri (fun i field -> printfn "%d: %s" i field)

// Split with capturing groups (preserves delimiters)  
let logParts = Regex.Split(logData, @"(\|\|)")
printfn "\nLog parts (with separators):"
logParts |> Array.iteri (fun i part -> 
    if not (System.String.IsNullOrEmpty(part)) then
        printfn "%d: %s" i part)

// Advanced splitting with complex patterns
let mixedData = "item1:value1,item2:value2;item3:value3,item4:value4"
let items = Regex.Split(mixedData, @"[,;]")
            |> Array.map (fun item -> 
                let parts = item.Split(':')
                if parts.Length = 2 then Some(parts.[0], parts.[1]) else None)
            |> Array.choose id

printfn "\nParsed items:"
items |> Array.iter (fun (key, value) -> printfn "%s = %s" key value)
```

Regex.Split divides text at pattern matches with options to  
preserve delimiters using capturing groups. This enables  
sophisticated tokenization for structured data parsing  
beyond simple string splitting capabilities.  

## Regex options and flags

Regex options modify pattern matching behavior through flags  
that control case sensitivity, multiline matching, whitespace  
handling, and compilation for performance optimization.  

```f#
open System.Text.RegularExpressions

let multilineText = """
First Line
SECOND line  
third LINE
Final line
"""

let mixedCaseText = "The Quick Brown Fox Jumps Over The Lazy Dog"
let spacedPattern = @"t h e"
let verbosePattern = @"
    \d{4}      # Year
    -          # Separator  
    \d{2}      # Month
    -          # Separator
    \d{2}      # Day
"

// Case sensitivity options
let caseSensitive = Regex(@"the", RegexOptions.None)
let caseInsensitive = Regex(@"the", RegexOptions.IgnoreCase)

printfn "Text: %s" mixedCaseText
printfn "Case sensitive matches: %d" (caseSensitive.Matches(mixedCaseText).Count)
printfn "Case insensitive matches: %d" (caseInsensitive.Matches(mixedCaseText).Count)

// Multiline options
let singleLineMode = Regex(@"^.*line.*$", RegexOptions.None)
let multiLineMode = Regex(@"^.*line.*$", RegexOptions.Multiline)

printfn "\nMultiline text matches:"
printfn "Single line mode: %d" (singleLineMode.Matches(multilineText).Count)
printfn "Multiline mode: %d" (multiLineMode.Matches(multilineText).Count)

// Ignore whitespace and comments (verbose mode)
let dateText = "Date: 2023-12-01"
let compactDate = Regex(@"\d{4}-\d{2}-\d{2}", RegexOptions.Compiled)
let verboseDate = Regex(verbosePattern, RegexOptions.IgnorePatternWhitespace ||| RegexOptions.Compiled)

printfn "\nDate matching:"
printfn "Compact pattern match: %b" (compactDate.IsMatch(dateText))
printfn "Verbose pattern match: %b" (verboseDate.IsMatch(dateText))

// Global options combination
let combinedOptions = RegexOptions.IgnoreCase ||| RegexOptions.Multiline ||| RegexOptions.Compiled
let flexiblePattern = Regex(@"^error.*", combinedOptions)

let logText = """
INFO: System started
ERROR: Database connection failed  
Warning: Low memory
ERROR: Authentication failed
"""

printfn "\nCombined options matches:"
flexiblePattern.Matches(logText) |> Seq.iter (fun m -> 
    printfn "Found: %s" (m.Value.Trim()))
```

RegexOptions flags modify matching behavior with IgnoreCase,  
Multiline, Singleline, IgnorePatternWhitespace for verbose  
patterns, and Compiled for performance. Multiple options  
combine using bitwise OR for flexible pattern control.  

## Lookaheads and lookbehinds

Lookaround assertions match positions without consuming characters,  
enabling complex conditional matching based on surrounding context.  
These provide precise pattern matching for specific scenarios.  

```f#
open System.Text.RegularExpressions

let passwordTests = [
    "password123"      // weak
    "Password123"      // better  
    "Password123!"     // strong
    "Pass123!"         // medium
    "VeryLongPassword123!" // very strong
]

// Positive lookahead - must contain specific elements
let hasLowercase = Regex(@"(?=.*[a-z])", RegexOptions.Compiled)
let hasUppercase = Regex(@"(?=.*[A-Z])", RegexOptions.Compiled)  
let hasDigit = Regex(@"(?=.*\d)", RegexOptions.Compiled)
let hasSpecial = Regex(@"(?=.*[!@#$%^&*])", RegexOptions.Compiled)
let minLength = Regex(@"(?=.{8,})", RegexOptions.Compiled)

// Combined password strength
let strongPassword = Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$", RegexOptions.Compiled)

passwordTests |> List.iter (fun pwd ->
    printfn "Password: %s" pwd
    printfn "  Has lowercase: %b" (hasLowercase.IsMatch(pwd))
    printfn "  Has uppercase: %b" (hasUppercase.IsMatch(pwd))
    printfn "  Has digit: %b" (hasDigit.IsMatch(pwd))
    printfn "  Has special: %b" (hasSpecial.IsMatch(pwd))
    printfn "  Min length: %b" (minLength.IsMatch(pwd))
    printfn "  Strong password: %b" (strongPassword.IsMatch(pwd))
    printfn "")

// Negative lookahead and lookbehind  
let textSamples = [
    "The cat sat on the mat"
    "The dog ran in the park"
    "A bird flew over the tree"
]

// Match "the" not followed by "cat"
let theNotCat = Regex(@"the(?!\s+cat)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

// Match words not preceded by "the"
let notAfterThe = Regex(@"(?<!the\s+)\b(cat|dog|bird)\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

textSamples |> List.iter (fun text ->
    printfn "Text: %s" text
    printfn "  'the' not before 'cat': %A" [for m in theNotCat.Matches(text) -> m.Value]
    printfn "  Animals not after 'the': %A" [for m in notAfterThe.Matches(text) -> m.Value]
    printfn "")
```

Lookaround assertions use (?=) positive lookahead, (?!) negative  
lookahead, (?<=) positive lookbehind, and (?<!) negative lookbehind.  
These enable context-sensitive matching without including  
surrounding text in the match result.  

## Email validation

Email validation demonstrates complex regex patterns for real-world  
data validation, balancing thoroughness with practical usability  
while handling various email format requirements.  

```f#
open System.Text.RegularExpressions

let emailAddresses = [
    "user@domain.com"           // valid
    "first.last@subdomain.domain.org" // valid
    "user+tag@domain.co.uk"     // valid  
    "123@domain.net"            // valid
    "invalid.email"             // invalid
    "@domain.com"               // invalid
    "user@"                     // invalid
    "user@.com"                 // invalid
    "user@domain."              // invalid
    "test@domain-name.com"      // valid
    "user_name@domain123.info"  // valid
]

// Basic email pattern
let basicEmail = Regex(@"^[^@]+@[^@]+\.[^@]+$", RegexOptions.Compiled)

// Comprehensive email pattern
let detailedEmail = Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)

// More strict email pattern  
let strictEmail = Regex(@"^[a-zA-Z0-9]([a-zA-Z0-9._-]*[a-zA-Z0-9])?@[a-zA-Z0-9]([a-zA-Z0-9.-]*[a-zA-Z0-9])?\.[a-zA-Z]{2,}$", RegexOptions.Compiled)

printfn "Email Validation Results:"
printfn "%-30s %-8s %-10s %-8s" "Email" "Basic" "Detailed" "Strict"
printfn "%s" (String.replicate 60 "-")

emailAddresses |> List.iter (fun email ->
    printfn "%-30s %-8b %-10b %-8b" 
        email
        (basicEmail.IsMatch(email))
        (detailedEmail.IsMatch(email))
        (strictEmail.IsMatch(email)))

// Extract email components
let emailComponents = Regex(@"^(?<local>[^@]+)@(?<domain>[^@]+)$", RegexOptions.Compiled)

printfn "\nEmail Components:"
emailAddresses 
|> List.filter detailedEmail.IsMatch
|> List.iter (fun email ->
    let match' = emailComponents.Match(email)
    if match'.Success then
        printfn "Email: %s" email
        printfn "  Local: %s" match'.Groups["local"].Value
        printfn "  Domain: %s" match'.Groups["domain"].Value
        printfn "")
```

Email validation requires balancing complexity with usability.  
Basic patterns catch obvious errors while comprehensive patterns  
handle edge cases. Real applications often use specialized  
validation libraries for full RFC compliance.  

## Phone number validation

Phone number patterns accommodate various international formats,  
optional country codes, and different separator styles while  
providing flexible validation for user input.  

```f#
open System.Text.RegularExpressions

let phoneNumbers = [
    "(555) 123-4567"        // US format with parentheses
    "555-123-4567"          // US format with dashes
    "555.123.4567"          // US format with dots  
    "5551234567"            // US format no separators
    "+1-555-123-4567"       // US with country code
    "+44 20 7946 0958"      // UK format
    "+33 1 42 34 56 78"     // French format
    "123-45-67"             // Too short
    "abc-def-ghij"          // Invalid characters
    "555 123 4567"          // Space separators
]

// US phone number patterns
let usPhoneBasic = Regex(@"^\d{3}-\d{3}-\d{4}$", RegexOptions.Compiled)
let usPhoneFlexible = Regex(@"^(\+?1[-.\s]?)?\(?(\d{3})\)?[-.\s]?(\d{3})[-.\s]?(\d{4})$", RegexOptions.Compiled)
let usPhoneStrict = Regex(@"^(\+1\s?)?(\(\d{3}\)|\d{3})[\s.-]?\d{3}[\s.-]?\d{4}$", RegexOptions.Compiled)

// International phone pattern
let internationalPhone = Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled)

printfn "Phone Number Validation:"
printfn "%-20s %-8s %-10s %-8s %-13s" "Number" "Basic" "Flexible" "Strict" "International"
printfn "%s" (String.replicate 70 "-")

phoneNumbers |> List.iter (fun number ->
    printfn "%-20s %-8b %-10b %-8b %-13b" 
        number
        (usPhoneBasic.IsMatch(number))
        (usPhoneFlexible.IsMatch(number))
        (usPhoneStrict.IsMatch(number))
        (internationalPhone.IsMatch(number.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", ""))))

// Extract phone components
let phoneParser = Regex(@"^(\+?1[-.\s]?)?\(?(\d{3})\)?[-.\s]?(\d{3})[-.\s]?(\d{4})$", RegexOptions.Compiled)

printfn "\nPhone Number Components:"
phoneNumbers
|> List.filter usPhoneFlexible.IsMatch
|> List.iter (fun number ->
    let match' = phoneParser.Match(number)
    if match'.Success then
        printfn "Number: %s" number
        printfn "  Country: %s" (if match'.Groups.[1].Success then match'.Groups.[1].Value else "US")
        printfn "  Area: %s" match'.Groups.[2].Value
        printfn "  Exchange: %s" match'.Groups.[3].Value  
        printfn "  Number: %s" match'.Groups.[4].Value
        printfn "")

// Phone formatting function
let formatPhone (number: string) =
    let cleanNumber = Regex.Replace(number, @"[^\d]", "")
    if cleanNumber.Length = 10 then
        sprintf "(%s) %s-%s" 
            cleanNumber.[0..2] 
            cleanNumber.[3..5] 
            cleanNumber.[6..9]
    elif cleanNumber.Length = 11 && cleanNumber.[0] = '1' then
        sprintf "+1 (%s) %s-%s" 
            cleanNumber.[1..3] 
            cleanNumber.[4..6] 
            cleanNumber.[7..10]
    else number

printfn "Formatted Numbers:"
["5551234567"; "15551234567"; "555-123-4567"] 
|> List.iter (fun num -> printfn "%s -> %s" num (formatPhone num))
```

Phone validation handles diverse formats with flexible patterns  
for user input and strict patterns for data storage. Parsing  
captures components for formatting and normalization while  
international patterns accommodate global phone systems.  

## URL validation

URL validation ensures proper web address format with protocol,  
domain, and optional path components while supporting various  
URL schemes and international domain names.  

```f#
open System.Text.RegularExpressions

let urls = [
    "https://www.example.com"               // valid HTTPS
    "http://example.org"                    // valid HTTP
    "https://subdomain.example.co.uk/path" // valid with subdomain and path
    "ftp://files.example.com"               // valid FTP
    "https://example.com:8080/app"          // valid with port
    "www.example.com"                       // missing protocol
    "https://.com"                          // invalid domain  
    "http://example"                        // missing TLD
    "https://example..com"                  // double dots
    "https://192.168.1.1"                   // valid IP address
    "mailto:user@example.com"               // valid mailto
]

// Basic URL pattern
let basicUrl = Regex(@"^https?://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled)

// Comprehensive URL pattern
let detailedUrl = Regex(@"^(https?|ftp)://([a-zA-Z0-9.-]+\.?[a-zA-Z]{2,}|[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})(:[0-9]{1,5})?(/[^\s]*)?$", RegexOptions.Compiled)

// URL with optional protocol
let flexibleUrl = Regex(@"^(https?://)?([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})(:[0-9]{1,5})?(/[^\s]*)?$", RegexOptions.Compiled)

// Comprehensive URL with named groups
let urlParser = Regex(@"^(?<protocol>https?|ftp)://(?<domain>[a-zA-Z0-9.-]+)(?<port>:[0-9]{1,5})?(?<path>/[^\s]*)?$", RegexOptions.Compiled)

printfn "URL Validation Results:"
printfn "%-35s %-8s %-10s %-10s" "URL" "Basic" "Detailed" "Flexible"
printfn "%s" (String.replicate 70 "-")

urls |> List.iter (fun url ->
    printfn "%-35s %-8b %-10b %-10b" 
        url
        (basicUrl.IsMatch(url))
        (detailedUrl.IsMatch(url))
        (flexibleUrl.IsMatch(url)))

printfn "\nURL Components:"
urls
|> List.filter detailedUrl.IsMatch  
|> List.iter (fun url ->
    let match' = urlParser.Match(url)
    if match'.Success then
        printfn "URL: %s" url
        printfn "  Protocol: %s" match'.Groups["protocol"].Value
        printfn "  Domain: %s" match'.Groups["domain"].Value
        printfn "  Port: %s" (if match'.Groups["port"].Success then match'.Groups["port"].Value else "default")
        printfn "  Path: %s" (if match'.Groups["path"].Success then match'.Groups["path"].Value else "/")
        printfn "")

// URL validation with specific schemes  
let httpOnly = Regex(@"^https://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled)
let anyScheme = Regex(@"^[a-z][a-z0-9+.-]*://[^\s]*$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

printfn "Scheme-specific validation:"
urls |> List.iter (fun url ->
    printfn "%-35s HTTPS: %-5b Any scheme: %b" 
        url 
        (httpOnly.IsMatch(url))
        (anyScheme.IsMatch(url)))
```

URL validation accommodates different protocols, domain formats,  
ports, and paths. Named groups extract URL components for  
processing while flexible patterns handle URLs with or  
without protocols for user-friendly input validation.  

## Date and time validation

Date patterns validate various formats including ISO dates,  
US formats, and European conventions while extracting  
components for parsing and validation operations.  

```f#
open System
open System.Text.RegularExpressions

let dateStrings = [
    "2023-12-01"        // ISO format
    "12/01/2023"        // US format
    "01/12/2023"        // US/European ambiguous
    "01-Dec-2023"       // Month name
    "December 1, 2023"  // Full format
    "2023/12/01"        // Alternative ISO
    "31/02/2023"        // Invalid date
    "13/25/2023"        // Invalid month/day
    "2023-13-01"        // Invalid month
    "01-01-99"          // 2-digit year
]

// Different date patterns
let isoDate = Regex(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled)
let usDate = Regex(@"^\d{1,2}/\d{1,2}/\d{4}$", RegexOptions.Compiled)
let europeanDate = Regex(@"^\d{1,2}/\d{1,2}/\d{4}$", RegexOptions.Compiled)
let monthNameDate = Regex(@"^\d{1,2}-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{4}$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let fullDate = Regex(@"^(January|February|March|April|May|June|July|August|September|October|November|December)\s+\d{1,2},\s+\d{4}$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

// Date component parser
let dateParser = Regex(@"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})$", RegexOptions.Compiled)
let usDateParser = Regex(@"^(?<month>\d{1,2})/(?<day>\d{1,2})/(?<year>\d{4})$", RegexOptions.Compiled)

printfn "Date Validation Results:"
printfn "%-15s %-6s %-6s %-12s %-8s" "Date" "ISO" "US" "Month Name" "Full"
printfn "%s" (String.replicate 55 "-")

dateStrings |> List.iter (fun dateStr ->
    printfn "%-15s %-6b %-6b %-12b %-8b" 
        dateStr
        (isoDate.IsMatch(dateStr))
        (usDate.IsMatch(dateStr))
        (monthNameDate.IsMatch(dateStr))
        (fullDate.IsMatch(dateStr)))

// Time patterns
let timeFormats = [
    "14:30:25"          // 24-hour format
    "2:30:25 PM"        // 12-hour with AM/PM
    "14:30"             // 24-hour without seconds
    "2:30 PM"           // 12-hour without seconds
    "25:30:25"          // Invalid hour
    "14:70:25"          // Invalid minute
]

let time24 = Regex(@"^([01]?[0-9]|2[0-3]):[0-5][0-9](:[0-5][0-9])?$", RegexOptions.Compiled)
let time12 = Regex(@"^(0?[1-9]|1[0-2]):[0-5][0-9](:[0-5][0-9])?\s?(AM|PM)$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

printfn "\nTime Validation:"
printfn "%-12s %-8s %-8s" "Time" "24-hour" "12-hour"
printfn "%s" (String.replicate 30 "-")

timeFormats |> List.iter (fun time ->
    printfn "%-12s %-8b %-8b" 
        time
        (time24.IsMatch(time))
        (time12.IsMatch(time)))

// DateTime combination  
let dateTimePattern = Regex(@"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])$", RegexOptions.Compiled)

let dateTimes = [
    "2023-12-01 14:30:25"
    "2023-12-01  14:30:25"  // Extra space
    "2023/12/01 14:30:25"   // Different separator
]

printfn "\nDateTime Validation:"
dateTimes |> List.iter (fun dt ->
    let match' = dateTimePattern.Match(dt)
    printfn "DateTime: %s -> Valid: %b" dt match'.Success
    if match'.Success then
        printfn "  Date part: %s" match'.Groups["date"].Value
        printfn "  Time part: %s" match'.Groups["time"].Value)
```

Date validation patterns handle multiple formats with component  
extraction for parsing. Time patterns support both 12-hour and  
24-hour formats while combined DateTime patterns validate  
complete timestamp formats for logging and data storage.  

## IP address validation

IP address validation handles IPv4 and IPv6 formats with proper  
octet and segment validation, supporting network configuration  
and security applications requiring address verification.  

```f#
open System.Text.RegularExpressions

let ipAddresses = [
    "192.168.1.1"           // Valid IPv4
    "10.0.0.1"              // Valid IPv4 private
    "255.255.255.255"       // Valid IPv4 broadcast
    "0.0.0.0"               // Valid IPv4 any
    "256.1.1.1"             // Invalid IPv4 - octet too high
    "192.168.1"             // Invalid IPv4 - missing octet
    "192.168.1.1.1"         // Invalid IPv4 - extra octet
    "2001:0db8:85a3:0000:0000:8a2e:0370:7334" // Valid IPv6
    "2001:db8:85a3::8a2e:370:7334"             // Valid IPv6 compressed
    "::1"                   // Valid IPv6 loopback
    "fe80::1"               // Valid IPv6 link-local
    "invalid.ip.address"    // Invalid format
]

// IPv4 validation patterns
let ipv4Simple = Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.Compiled)
let ipv4Strict = Regex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled)

// IPv6 validation pattern (simplified)
let ipv6Pattern = Regex(@"^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$|^([0-9a-fA-F]{1,4}:)*::([0-9a-fA-F]{1,4}:)*[0-9a-fA-F]{1,4}$|^::1$|^::$", RegexOptions.Compiled)

// IP address parser with components
let ipv4Parser = Regex(@"^(?<octet1>\d{1,3})\.(?<octet2>\d{1,3})\.(?<octet3>\d{1,3})\.(?<octet4>\d{1,3})$", RegexOptions.Compiled)

printfn "IP Address Validation:"
printfn "%-35s %-10s %-10s %-8s" "IP Address" "Simple v4" "Strict v4" "IPv6"
printfn "%s" (String.replicate 70 "-")

ipAddresses |> List.iter (fun ip ->
    printfn "%-35s %-10b %-10b %-8b" 
        ip
        (ipv4Simple.IsMatch(ip))
        (ipv4Strict.IsMatch(ip))
        (ipv6Pattern.IsMatch(ip)))

// Validate IPv4 octet ranges
let validateIPv4 (ip: string) =
    let match' = ipv4Parser.Match(ip)
    if match'.Success then
        let octets = [
            match'.Groups["octet1"].Value
            match'.Groups["octet2"].Value  
            match'.Groups["octet3"].Value
            match'.Groups["octet4"].Value
        ]
        octets |> List.forall (fun octet ->
            match System.Int32.TryParse(octet) with
            | true, value when value >= 0 && value <= 255 -> true
            | _ -> false)
    else false

printfn "\nStrict IPv4 Validation (0-255 range):"
ipAddresses
|> List.filter ipv4Simple.IsMatch
|> List.iter (fun ip ->
    printfn "%s -> Valid: %b" ip (validateIPv4 ip))

// Network subnet patterns
let subnetPatterns = [
    "192.168.1.0/24"        // Valid CIDR
    "10.0.0.0/8"            // Valid CIDR
    "172.16.0.0/12"         // Valid CIDR
    "192.168.1.0/33"        // Invalid CIDR - prefix too high
    "256.1.1.0/24"          // Invalid IP in CIDR
]

let cidrPattern = Regex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)/([0-9]|[1-2][0-9]|3[0-2])$", RegexOptions.Compiled)

printfn "\nCIDR Subnet Validation:"
subnetPatterns |> List.iter (fun cidr ->
    printfn "%s -> Valid: %b" cidr (cidrPattern.IsMatch(cidr)))
```

IP address validation requires precise octet range checking for  
IPv4 and complex segment validation for IPv6. CIDR notation  
patterns validate network subnets with proper prefix lengths  
for network configuration and routing applications.  

## HTML tag parsing

HTML parsing with regex handles simple tag extraction and  
attribute parsing for content processing, though complex  
HTML requires specialized parsers for full compliance.  

```f#
open System.Text.RegularExpressions

let htmlContent = """
<html>
<head>
    <title>Sample Page</title>
    <meta charset="utf-8">
    <link rel="stylesheet" href="style.css">
</head>
<body class="main-content" data-page="home">
    <h1 id="header">Welcome</h1>
    <p class="intro">This is a <strong>sample</strong> paragraph with <a href="https://example.com">a link</a>.</p>
    <div class="content">
        <img src="image.jpg" alt="Sample image" width="300" height="200">
        <br>
        <input type="text" name="username" placeholder="Enter username" required>
    </div>
    <!-- This is a comment -->
    <script src="app.js"></script>
</body>
</html>
"""

// Basic tag patterns
let allTags = Regex(@"<[^>]+>", RegexOptions.Compiled)
let openingTags = Regex(@"<([a-zA-Z][a-zA-Z0-9]*)[^>]*>", RegexOptions.Compiled)
let closingTags = Regex(@"</([a-zA-Z][a-zA-Z0-9]*)>", RegexOptions.Compiled)
let selfClosingTags = Regex(@"<([a-zA-Z][a-zA-Z0-9]*)[^>]*/\s*>", RegexOptions.Compiled)

printfn "HTML Tag Extraction:"
printfn "All tags found: %d" (allTags.Matches(htmlContent).Count)
printfn "Opening tags: %d" (openingTags.Matches(htmlContent).Count)
printfn "Closing tags: %d" (closingTags.Matches(htmlContent).Count)
printfn "Self-closing tags: %d" (selfClosingTags.Matches(htmlContent).Count)

// Extract specific tag types
let headingTags = Regex(@"<(h[1-6])[^>]*>(.*?)</\1>", RegexOptions.Compiled ||| RegexOptions.Singleline)
let linkTags = Regex(@"<a\s+[^>]*href=[""']([^""']+)[""'][^>]*>(.*?)</a>", RegexOptions.Compiled ||| RegexOptions.IgnoreCase ||| RegexOptions.Singleline)
let imageTags = Regex(@"<img\s+[^>]*src=[""']([^""']+)[""'][^>]*>", RegexOptions.Compiled ||| RegexOptions.IgnoreCase)

printfn "\nSpecific Elements:"
printfn "Headings:"
headingTags.Matches(htmlContent) |> Seq.iter (fun m ->
    printfn "  <%s>%s</%s>" m.Groups.[1].Value m.Groups.[2].Value m.Groups.[1].Value)

printfn "Links:"
linkTags.Matches(htmlContent) |> Seq.iter (fun m ->
    printfn "  URL: %s, Text: %s" m.Groups.[1].Value m.Groups.[2].Value)

printfn "Images:"  
imageTags.Matches(htmlContent) |> Seq.iter (fun m ->
    printfn "  Source: %s" m.Groups.[1].Value)

// Attribute extraction
let attributePattern = Regex(@"(\w+)=[""']([^""']*)[""']", RegexOptions.Compiled ||| RegexOptions.IgnoreCase)

let extractAttributes (tag: string) =
    attributePattern.Matches(tag)
    |> Seq.map (fun m -> m.Groups.[1].Value, m.Groups.[2].Value)
    |> Seq.toList

printfn "\nAttribute Extraction:"
let bodyTag = Regex(@"<body[^>]*>", RegexOptions.Compiled ||| RegexOptions.IgnoreCase).Match(htmlContent)
if bodyTag.Success then
    let attributes = extractAttributes bodyTag.Value
    printfn "Body tag attributes:"
    attributes |> List.iter (fun (name, value) -> printfn "  %s = %s" name value)

// Clean HTML (remove tags)
let stripHtml = Regex(@"<[^>]*>", RegexOptions.Compiled ||| RegexOptions.Singleline)
let cleanText = stripHtml.Replace(htmlContent, " ")
let normalizedText = Regex.Replace(cleanText, @"\s+", " ").Trim()
printfn "\nText content (first 100 chars): %s..." (if normalizedText.Length > 100 then normalizedText.[0..99] else normalizedText)
```

HTML tag parsing uses regex for simple extraction tasks like  
finding tags, attributes, and content. Complex HTML requires  
dedicated parsers, but regex handles basic tag manipulation  
and content extraction for preprocessing operations.  

## JSON pattern matching

JSON pattern matching extracts values from JSON strings using  
regex patterns for simple parsing scenarios, though proper  
JSON parsing libraries provide more robust solutions.  

```f#
open System.Text.RegularExpressions

let jsonData = """{
  "name": "John Doe",
  "age": 30,
  "email": "john.doe@example.com",
  "address": {
    "street": "123 Main St",
    "city": "Anytown", 
    "zipcode": "12345"
  },
  "hobbies": ["reading", "swimming", "coding"],
  "active": true,
  "salary": 75000.50,
  "department": null
}"""

// Extract string values
let stringValue = Regex(@"""(\w+)""\s*:\s*""([^""]*)""\s*", RegexOptions.Compiled)
let numberValue = Regex(@"""(\w+)""\s*:\s*(\d+(?:\.\d+)?)", RegexOptions.Compiled)
let booleanValue = Regex(@"""(\w+)""\s*:\s*(true|false)", RegexOptions.Compiled)
let nullValue = Regex(@"""(\w+)""\s*:\s*null", RegexOptions.Compiled)

printfn "JSON Value Extraction:"
printfn "\nString values:"
stringValue.Matches(jsonData) |> Seq.iter (fun m ->
    printfn "  %s = %s" m.Groups.[1].Value m.Groups.[2].Value)

printfn "\nNumber values:"
numberValue.Matches(jsonData) |> Seq.iter (fun m ->
    printfn "  %s = %s" m.Groups.[1].Value m.Groups.[2].Value)

printfn "\nBoolean values:"
booleanValue.Matches(jsonData) |> Seq.iter (fun m ->
    printfn "  %s = %s" m.Groups.[1].Value m.Groups.[2].Value)

printfn "\nNull values:"
nullValue.Matches(jsonData) |> Seq.iter (fun m ->
    printfn "  %s = null" m.Groups.[1].Value)

// Extract array values
let arrayPattern = Regex(@"""(\w+)""\s*:\s*\[([^\]]*)\]", RegexOptions.Compiled)
let arrayItemPattern = Regex(@"""([^""]*)""|([^,\s\]]+)", RegexOptions.Compiled)

printfn "\nArray values:"
arrayPattern.Matches(jsonData) |> Seq.iter (fun m ->
    let arrayName = m.Groups.[1].Value
    let arrayContent = m.Groups.[2].Value
    let items = arrayItemPattern.Matches(arrayContent)
                |> Seq.map (fun item -> 
                    if item.Groups.[1].Success then item.Groups.[1].Value 
                    else item.Groups.[2].Value)
                |> Seq.toList
    printfn "  %s = [%s]" arrayName (String.concat "; " items))

// Extract nested object values
let nestedObjectPattern = Regex(@"""address""\s*:\s*{([^}]*)}", RegexOptions.Compiled ||| RegexOptions.Singleline)
let nestedMatch = nestedObjectPattern.Match(jsonData)

if nestedMatch.Success then
    printfn "\nNested object (address):"
    let nestedContent = nestedMatch.Groups.[1].Value
    stringValue.Matches(nestedContent) |> Seq.iter (fun m ->
        printfn "  %s = %s" m.Groups.[1].Value m.Groups.[2].Value)

// JSON validation patterns
let jsonValidators = [
    ("Valid object", """{  "key": "value"  }""")
    ("Valid array", """[ "item1", "item2" ]""")
    ("Invalid object", """{  "key": "value"  """)
    ("Invalid JSON", """{ key: "value" }""")  // Unquoted key
]

let basicJsonObject = Regex(@"^\s*{.*}\s*$", RegexOptions.Compiled ||| RegexOptions.Singleline)
let basicJsonArray = Regex(@"^\s*\[.*\]\s*$", RegexOptions.Compiled ||| RegexOptions.Singleline)

printfn "\nJSON Format Validation:"
jsonValidators |> List.iter (fun (desc, json) ->
    let isObject = basicJsonObject.IsMatch(json)
    let isArray = basicJsonArray.IsMatch(json)
    printfn "%s: Object=%b, Array=%b" desc isObject isArray)
```

JSON regex patterns extract simple values and validate basic  
structure, but complex JSON requires proper parsing libraries.  
Regex works for basic extraction tasks and preliminary  
validation in text processing scenarios.  

## Log file parsing

Log file parsing extracts structured information from various  
log formats including timestamps, levels, messages, and  
custom fields for monitoring and analysis applications.  

```f#
open System.Text.RegularExpressions

let logEntries = [
    "2023-12-01 14:30:25 [INFO] Application started successfully"
    "2023-12-01 14:30:26 [ERROR] Database connection failed: timeout after 30s"
    "2023-12-01 14:30:27 [WARN] Memory usage high: 85% of 8GB"
    "2023-12-01 14:30:28 [DEBUG] User session created: user_id=12345"
    "Dec 01 14:30:29 server.example.com sshd[1234]: Failed password for user from 192.168.1.100"
    "127.0.0.1 - - [01/Dec/2023:14:30:30 +0000] \"GET /api/users HTTP/1.1\" 200 1234"
    "[2023-12-01T14:30:31.123Z] ERROR: Payment processing failed for transaction tx_789"
]

// Standard application log pattern
let appLogPattern = Regex(@"^(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \[(?<level>\w+)\] (?<message>.*)$", RegexOptions.Compiled)

// Syslog pattern
let syslogPattern = Regex(@"^(?<month>\w{3}) (?<day>\d{1,2}) (?<time>\d{2}:\d{2}:\d{2}) (?<host>\S+) (?<process>\w+)\[(?<pid>\d+)\]: (?<message>.*)$", RegexOptions.Compiled)

// Apache/Nginx access log pattern  
let accessLogPattern = Regex(@"^(?<ip>\S+) \S+ \S+ \[(?<timestamp>[^\]]+)\] ""(?<method>\S+) (?<url>\S+) (?<protocol>[^""]+)"" (?<status>\d+) (?<size>\d+)", RegexOptions.Compiled)

// JSON-like log pattern
let jsonLogPattern = Regex(@"^\[(?<timestamp>[^\]]+)\] (?<level>\w+): (?<message>.*)$", RegexOptions.Compiled)

printfn "Log Parsing Results:\n"

logEntries |> List.iteri (fun i entry ->
    printfn "Entry %d: %s" (i+1) entry
    
    // Try application log pattern
    let appMatch = appLogPattern.Match(entry)
    if appMatch.Success then
        printfn "  Type: Application Log"
        printfn "  Timestamp: %s" appMatch.Groups["timestamp"].Value
        printfn "  Level: %s" appMatch.Groups["level"].Value
        printfn "  Message: %s" appMatch.Groups["message"].Value
    
    // Try syslog pattern
    let syslogMatch = syslogPattern.Match(entry)
    if syslogMatch.Success then
        printfn "  Type: Syslog"
        printfn "  Date: %s %s" syslogMatch.Groups["month"].Value syslogMatch.Groups["day"].Value
        printfn "  Time: %s" syslogMatch.Groups["time"].Value
        printfn "  Host: %s" syslogMatch.Groups["host"].Value
        printfn "  Process: %s[%s]" syslogMatch.Groups["process"].Value syslogMatch.Groups["pid"].Value
        printfn "  Message: %s" syslogMatch.Groups["message"].Value
    
    // Try access log pattern
    let accessMatch = accessLogPattern.Match(entry)
    if accessMatch.Success then
        printfn "  Type: Access Log"
        printfn "  IP: %s" accessMatch.Groups["ip"].Value
        printfn "  Timestamp: %s" accessMatch.Groups["timestamp"].Value
        printfn "  Request: %s %s %s" accessMatch.Groups["method"].Value accessMatch.Groups["url"].Value accessMatch.Groups["protocol"].Value
        printfn "  Status: %s" accessMatch.Groups["status"].Value
        printfn "  Size: %s" accessMatch.Groups["size"].Value
    
    // Try JSON log pattern  
    let jsonMatch = jsonLogPattern.Match(entry)
    if jsonMatch.Success then
        printfn "  Type: JSON-like Log"
        printfn "  Timestamp: %s" jsonMatch.Groups["timestamp"].Value
        printfn "  Level: %s" jsonMatch.Groups["level"].Value
        printfn "  Message: %s" jsonMatch.Groups["message"].Value
    
    if not (appMatch.Success || syslogMatch.Success || accessMatch.Success || jsonMatch.Success) then
        printfn "  Type: Unknown format"
    
    printfn "")

// Extract specific information from logs
let errorPattern = Regex(@"(ERROR|FAIL|EXCEPTION)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let ipPattern = Regex(@"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b", RegexOptions.Compiled)
let userIdPattern = Regex(@"user_id=(\d+)", RegexOptions.Compiled)

printfn "Log Analysis:"
printfn "Entries with errors: %d" (logEntries |> List.filter errorPattern.IsMatch |> List.length)
printfn "Unique IP addresses: %A" (
    logEntries 
    |> List.collect (fun entry -> [for m in ipPattern.Matches(entry) -> m.Value])
    |> List.distinct)
printfn "User IDs found: %A" (
    logEntries
    |> List.choose (fun entry -> 
        let match' = userIdPattern.Match(entry)
        if match'.Success then Some match'.Groups.[1].Value else None))
```

Log parsing patterns handle different log formats with structured  
field extraction. Named groups capture timestamps, levels,  
messages, and custom fields while analysis patterns identify  
errors, users, and security events for monitoring systems.  

## Password validation

Password validation enforces security requirements using  
complex regex patterns that check character types, length,  
and forbidden patterns for robust authentication systems.  

```f#
open System.Text.RegularExpressions

let passwordCandidates = [
    "password"                  // weak - common word
    "Password123"               // medium - mixed case, numbers
    "P@ssw0rd123!"             // strong - mixed case, numbers, symbols
    "VeryLongPasswordWithNoNumbers" // weak - no numbers/symbols
    "Sh0rt!"                   // weak - too short
    "MySecurePassword2023!"    // strong - all requirements
    "123456789"                // weak - numbers only
    "ALLUPPERCASE123!"         // medium - no lowercase
    "alllowercase123!"         // medium - no uppercase
    "MixedCaseNoNumbers!"      // medium - no numbers
]

// Password strength requirements
let minLength8 = Regex(@"^.{8,}$", RegexOptions.Compiled)
let minLength12 = Regex(@"^.{12,}$", RegexOptions.Compiled)
let hasLowercase = Regex(@"[a-z]", RegexOptions.Compiled)
let hasUppercase = Regex(@"[A-Z]", RegexOptions.Compiled)
let hasNumbers = Regex(@"\d", RegexOptions.Compiled)
let hasSymbols = Regex(@"[!@#$%^&*(),.?""':;{}|<>]", RegexOptions.Compiled)

// Forbidden patterns
let commonWords = Regex(@"(password|admin|user|login|welcome|123456|qwerty)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let repeatedChars = Regex(@"(.)\1{2,}", RegexOptions.Compiled)  // 3+ repeated characters
let sequentialNumbers = Regex(@"(012|123|234|345|456|567|678|789)", RegexOptions.Compiled)
let sequentialChars = Regex(@"(abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

// Comprehensive password validation
let validatePassword (password: string) =
    let checks = [
        ("Minimum 8 characters", minLength8.IsMatch(password))
        ("Minimum 12 characters", minLength12.IsMatch(password))
        ("Has lowercase", hasLowercase.IsMatch(password))
        ("Has uppercase", hasUppercase.IsMatch(password))
        ("Has numbers", hasNumbers.IsMatch(password))
        ("Has symbols", hasSymbols.IsMatch(password))
        ("No common words", not (commonWords.IsMatch(password)))
        ("No repeated chars", not (repeatedChars.IsMatch(password)))
        ("No sequential numbers", not (sequentialNumbers.IsMatch(password)))
        ("No sequential chars", not (sequentialChars.IsMatch(password)))
    ]
    
    let passedChecks = checks |> List.filter snd |> List.length
    let strength = 
        match passedChecks with
        | n when n >= 8 -> "Very Strong"
        | n when n >= 6 -> "Strong"
        | n when n >= 4 -> "Medium"
        | n when n >= 2 -> "Weak"
        | _ -> "Very Weak"
    
    (checks, strength, passedChecks)

printfn "Password Strength Analysis:\n"

passwordCandidates |> List.iter (fun password ->
    let (checks, strength, score) = validatePassword password
    printfn "Password: %s" password
    printfn "Strength: %s (%d/10 checks passed)" strength score
    printfn "Details:"
    checks |> List.iter (fun (check, passed) ->
        printfn "  %-25s %s" check (if passed then "✓" else "✗"))
    printfn "")

// Generate password policy regex
let strongPasswordPolicy = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""':;{}|<>])(?!.*(.)\1{2,})(?!.*(password|admin|user|login|welcome|123456|qwerty)).{12,}$"
let strongPasswordRegex = Regex(strongPasswordPolicy, RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

printfn "Policy-based validation (single regex):"
passwordCandidates |> List.iter (fun password ->
    let isStrong = strongPasswordRegex.IsMatch(password)
    printfn "%-30s %s" password (if isStrong then "PASS" else "FAIL"))

// Custom password requirements
let customRequirements = [
    ("Banking", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{16,}$")
    ("Standard", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
    ("High Security", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])(?=.*[^a-zA-Z0-9]).{20,}$")
]

printfn "\nCustom Security Levels:"
customRequirements |> List.iter (fun (name, pattern) ->
    let regex = Regex(pattern, RegexOptions.Compiled)
    printfn "%s requirements:" name
    passwordCandidates 
    |> List.filter regex.IsMatch
    |> List.iter (fun pwd -> printfn "  %s" pwd)
    printfn "")
```

Password validation combines multiple regex patterns for  
comprehensive security checking. Single-pattern validation  
provides policy enforcement while detailed checking offers  
user feedback for password improvement and security compliance.  

## File path matching

File path patterns validate and parse filesystem paths for  
Windows, Unix, and web URLs, handling different separators  
and path formats for cross-platform applications.  

```f#
open System.Text.RegularExpressions

let filePaths = [
    @"C:\Users\john\Documents\file.txt"        // Windows absolute
    @".\relative\path\file.txt"                // Windows relative
    "/home/john/documents/file.txt"            // Unix absolute
    "./relative/path/file.txt"                 // Unix relative
    "~/documents/file.txt"                     // Unix home
    @"\\server\share\file.txt"                 // UNC path
    "file.txt"                                 // Just filename
    "/var/log/app.log"                         // Unix system path
    @"C:\Program Files (x86)\App\config.xml"  // Windows with spaces
    "https://example.com/path/file.html"       // Web URL
]

// Path validation patterns
let windowsPath = Regex(@"^[A-Za-z]:\\(?:[^<>:""/\\|?*]+\\)*[^<>:""/\\|?*]*$", RegexOptions.Compiled)
let unixPath = Regex(@"^/?(?:[^/\0]+/)*[^/\0]*$", RegexOptions.Compiled)
let uncPath = Regex(@"^\\\\[^\\]+\\[^\\]+(?:\\[^\\]*)*$", RegexOptions.Compiled)
let relativePath = Regex(@"^\.{1,2}[/\\]", RegexOptions.Compiled)
let homePath = Regex(@"^~[/\\]", RegexOptions.Compiled)

// File extension patterns
let hasExtension = Regex(@"\.[a-zA-Z0-9]+$", RegexOptions.Compiled)
let specificExtensions = Regex(@"\.(txt|doc|pdf|jpg|png|html|xml|json)$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

printfn "File Path Analysis:"
printfn "%-40s %-8s %-6s %-5s %-8s %-6s %-8s %-12s" "Path" "Windows" "Unix" "UNC" "Relative" "Home" "Has Ext" "Specific Ext"
printfn "%s" (String.replicate 100 "-")

filePaths |> List.iter (fun path ->
    printfn "%-40s %-8b %-6b %-5b %-8b %-6b %-8b %-12b" 
        (if path.Length > 40 then path.[0..36] + "..." else path)
        (windowsPath.IsMatch(path))
        (unixPath.IsMatch(path))
        (uncPath.IsMatch(path))
        (relativePath.IsMatch(path))
        (homePath.IsMatch(path))
        (hasExtension.IsMatch(path))
        (specificExtensions.IsMatch(path)))

// Path component extraction
let pathParser = Regex(@"^(?:(?<drive>[A-Za-z]):)?(?<separator>[\\\/])?(?<directories>(?:[^<>:""/\\|?*]+[\\\/])*)?(?<filename>[^<>:""/\\|?*]+?)(?<extension>\.[^.]*)?$", RegexOptions.Compiled)

printfn "\nPath Component Extraction:"
filePaths 
|> List.take 5  // Show first 5 for brevity
|> List.iter (fun path ->
    let match' = pathParser.Match(path)
    if match'.Success then
        printfn "Path: %s" path
        printfn "  Drive: %s" (if match'.Groups["drive"].Success then match'.Groups["drive"].Value else "none")
        printfn "  Separator: %s" (if match'.Groups["separator"].Success then match'.Groups["separator"].Value else "none")
        printfn "  Directories: %s" (if match'.Groups["directories"].Success then match'.Groups["directories"].Value else "none")
        printfn "  Filename: %s" (if match'.Groups["filename"].Success then match'.Groups["filename"].Value else "none")
        printfn "  Extension: %s" (if match'.Groups["extension"].Success then match'.Groups["extension"].Value else "none")
        printfn "")

// Security validation (path traversal detection)
let pathTraversal = Regex(@"(\.\.[\\/]|[\\/]\.\.[\\/]|[\\/]\.\.$)", RegexOptions.Compiled)
let suspiciousChars = Regex(@"[<>:""|?*\x00-\x1f]", RegexOptions.Compiled)

let suspiciousPaths = [
    "../../../etc/passwd"
    "file.txt"
    "..\\..\\windows\\system32"
    "normal/path/file.txt"
    "file<script>.txt"
]

printfn "Security Analysis:"
suspiciousPaths |> List.iter (fun path ->
    let hasTraversal = pathTraversal.IsMatch(path)
    let hasSuspicious = suspiciousChars.IsMatch(path)
    printfn "%-25s Traversal: %-5b Suspicious: %b" path hasTraversal hasSuspicious)
```

File path validation handles platform-specific formats and  
extracts path components for processing. Security patterns  
detect path traversal attacks and invalid characters to  
prevent filesystem security vulnerabilities.  

## Credit card validation

Credit card validation uses Luhn algorithm patterns and  
issuer-specific formats to validate card numbers for  
e-commerce and payment processing applications.  

```f#
open System.Text.RegularExpressions

let creditCardNumbers = [
    "4532015112830366"      // Valid Visa
    "4000000000000002"      // Valid Visa test
    "5555555555554444"      // Valid MasterCard test
    "378282246310005"       // Valid Amex test
    "6011111111111117"      // Valid Discover test
    "1234567890123456"      // Invalid - fails Luhn
    "4532-0151-1283-0366"   // Valid Visa with separators
    "4532 0151 1283 0366"   // Valid Visa with spaces
    "123456789012345"       // Invalid - wrong length
    "453201511283036A"      // Invalid - contains letters
]

// Card type patterns
let visaPattern = Regex(@"^4[0-9]{12}(?:[0-9]{3})?$", RegexOptions.Compiled)
let mastercardPattern = Regex(@"^5[1-5][0-9]{14}$|^2(?:2[2-9][0-9]|[3-6][0-9]{2}|7[01][0-9]|720)[0-9]{12}$", RegexOptions.Compiled)
let amexPattern = Regex(@"^3[47][0-9]{13}$", RegexOptions.Compiled)
let discoverPattern = Regex(@"^6(?:011|5[0-9]{2})[0-9]{12}$", RegexOptions.Compiled)
let dinersClubPattern = Regex(@"^3[0689][0-9]{13}$", RegexOptions.Compiled)
let jcbPattern = Regex(@"^(?:2131|1800|35\d{3})\d{11}$", RegexOptions.Compiled)

// Clean card number (remove separators)
let cleanCardNumber (cardNumber: string) =
    Regex.Replace(cardNumber, @"[\s\-]", "")

// Luhn algorithm validation
let validateLuhn (cardNumber: string) =
    let digits = 
        cardNumber
        |> Seq.rev
        |> Seq.mapi (fun i c -> 
            let digit = int c - int '0'
            if i % 2 = 1 then
                let doubled = digit * 2
                if doubled > 9 then doubled - 9 else doubled
            else digit)
        |> Seq.sum
    
    digits % 10 = 0

// Comprehensive card validation
let validateCreditCard (cardNumber: string) =
    let cleaned = cleanCardNumber cardNumber
    let digitsOnly = Regex(@"^\d+$", RegexOptions.Compiled).IsMatch(cleaned)
    
    if not digitsOnly then
        ("Invalid", "Contains non-digit characters", false)
    else
        let cardType =
            if visaPattern.IsMatch(cleaned) then "Visa"
            elif mastercardPattern.IsMatch(cleaned) then "MasterCard"
            elif amexPattern.IsMatch(cleaned) then "American Express"
            elif discoverPattern.IsMatch(cleaned) then "Discover"
            elif dinersClubPattern.IsMatch(cleaned) then "Diners Club"
            elif jcbPattern.IsMatch(cleaned) then "JCB"
            else "Unknown"
        
        let luhnValid = validateLuhn cleaned
        let status = if luhnValid then "Valid" else "Invalid (Luhn check failed)"
        
        (cardType, status, luhnValid)

printfn "Credit Card Validation Results:"
printfn "%-20s %-15s %-25s %-8s" "Card Number" "Type" "Status" "Valid"
printfn "%s" (String.replicate 75 "-")

creditCardNumbers |> List.iter (fun cardNumber ->
    let (cardType, status, isValid) = validateCreditCard cardNumber
    let displayNumber = if cardNumber.Length > 20 then cardNumber.[0..16] + "..." else cardNumber
    printfn "%-20s %-15s %-25s %-8b" displayNumber cardType status isValid)

// Mask credit card numbers for display
let maskCreditCard (cardNumber: string) =
    let cleaned = cleanCardNumber cardNumber
    if cleaned.Length >= 4 then
        let masked = String.replicate (cleaned.Length - 4) "*" + cleaned.[cleaned.Length-4..]
        // Add separators for readability
        if cleaned.Length = 16 then
            sprintf "%s-%s-%s-%s" masked.[0..3] masked.[4..7] masked.[8..11] masked.[12..15]
        elif cleaned.Length = 15 then
            sprintf "%s-%s-%s" masked.[0..3] masked.[4..9] masked.[10..14]
        else masked
    else cardNumber

printfn "\nMasked Card Numbers:"
creditCardNumbers
|> List.filter (fun card -> let (_, _, valid) = validateCreditCard card in valid)
|> List.iter (fun card ->
    printfn "Original: %s -> Masked: %s" card (maskCreditCard card))

// Extract card info for processing
let cardInfoPattern = Regex(@"^(?<type>Visa|MasterCard|Amex|American\s+Express|Discover)\s*:?\s*(?<number>[\d\s\-]+)$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

let cardInputs = [
    "Visa: 4532015112830366"
    "MasterCard 5555555555554444"
    "Amex: 378282246310005"
]

printfn "\nCard Info Extraction:"
cardInputs |> List.iter (fun input ->
    let match' = cardInfoPattern.Match(input)
    if match'.Success then
        let cardType = match'.Groups["type"].Value
        let cardNumber = cleanCardNumber match'.Groups["number"].Value
        let (detectedType, status, valid) = validateCreditCard cardNumber
        printfn "Input: %s" input
        printfn "  Stated type: %s, Detected type: %s, Valid: %b" cardType detectedType valid)
```

Credit card validation combines pattern matching for card types  
with Luhn algorithm verification for mathematical validity.  
Masking functions protect sensitive data while extraction  
patterns process user input for payment systems.  

## CSS selector patterns

CSS selector patterns extract and validate CSS selectors,  
class names, and IDs for web development tools and  
HTML/CSS processing applications.  

```f#
open System.Text.RegularExpressions

let cssSelectors = [
    "div"                           // Element selector
    ".header"                       // Class selector
    "#main-content"                 // ID selector
    "div.header"                    // Element with class
    "div#main"                      // Element with ID
    ".header .nav"                  // Descendant selectors
    "div > p"                       // Child selector
    "input[type='text']"            // Attribute selector
    "a:hover"                       // Pseudo-class
    "p::first-line"                 // Pseudo-element
    "div.header, div.footer"        // Multiple selectors
    ".nav li:nth-child(2n+1)"       // Complex pseudo-class
    "*"                             // Universal selector
    "div + p"                       // Adjacent sibling
    "div ~ p"                       // General sibling
]

let cssRules = """
.header {
    background-color: #333;
    color: white;
    padding: 10px;
}

#main-content {
    margin: 20px;
    font-size: 16px;
}

div.article > p {
    line-height: 1.5;
    margin-bottom: 15px;
}

@media (max-width: 768px) {
    .responsive-grid {
        display: block;
    }
}
"""

// CSS selector component patterns
let elementSelector = Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled)
let classSelector = Regex(@"^\.[a-zA-Z_\-][a-zA-Z0-9_\-]*$", RegexOptions.Compiled)
let idSelector = Regex(@"^#[a-zA-Z_\-][a-zA-Z0-9_\-]*$", RegexOptions.Compiled)
let attributeSelector = Regex(@"\[[^\]]+\]", RegexOptions.Compiled)
let pseudoClass = Regex(@":[a-zA-Z\-]+(\([^)]*\))?", RegexOptions.Compiled)
let pseudoElement = Regex(@"::[a-zA-Z\-]+", RegexOptions.Compiled)

printfn "CSS Selector Analysis:"
printfn "%-25s %-8s %-7s %-4s %-5s %-7s %-8s" "Selector" "Element" "Class" "ID" "Attr" "Pseudo" "PseudoEl"
printfn "%s" (String.replicate 80 "-")

cssSelectors |> List.iter (fun selector ->
    let cleanSelector = selector.Split(',').[0].Trim() // Take first selector if multiple
    printfn "%-25s %-8b %-7b %-4b %-5b %-7b %-8b" 
        selector
        (elementSelector.IsMatch(cleanSelector))
        (classSelector.IsMatch(cleanSelector))
        (idSelector.IsMatch(cleanSelector))
        (attributeSelector.IsMatch(cleanSelector))
        (pseudoClass.IsMatch(cleanSelector))
        (pseudoElement.IsMatch(cleanSelector)))

// Extract CSS rules and selectors
let cssRulePattern = Regex(@"(?<selector>[^{]+)\s*\{\s*(?<properties>[^}]*)\s*\}", RegexOptions.Compiled ||| RegexOptions.Singleline)
let cssPropertyPattern = Regex(@"(?<property>[a-zA-Z\-]+)\s*:\s*(?<value>[^;]+)", RegexOptions.Compiled)

printfn "\nCSS Rules Extraction:"
cssRulePattern.Matches(cssRules) |> Seq.iter (fun ruleMatch ->
    let selector = ruleMatch.Groups["selector"].Value.Trim()
    let properties = ruleMatch.Groups["properties"].Value
    
    printfn "Selector: %s" selector
    printfn "Properties:"
    cssPropertyPattern.Matches(properties) |> Seq.iter (fun propMatch ->
        let property = propMatch.Groups["property"].Value.Trim()
        let value = propMatch.Groups["value"].Value.Trim()
        printfn "  %s: %s" property value)
    printfn "")

// CSS class and ID extraction from HTML
let htmlWithCss = """
<div class="header main-header" id="top-header">
    <nav class="navigation">
        <ul class="nav-list">
            <li class="nav-item active"><a href="#home">Home</a></li>
            <li class="nav-item"><a href="#about">About</a></li>
        </ul>
    </nav>
</div>
"""

let classPattern = Regex(@"class=[""']([^""']+)[""']", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let idPattern = Regex(@"id=[""']([^""']+)[""']", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

printfn "HTML Class and ID Extraction:"
let classes = classPattern.Matches(htmlWithCss)
              |> Seq.collect (fun m -> m.Groups.[1].Value.Split(' ') |> Array.filter (fun s -> not (String.IsNullOrWhiteSpace(s))))
              |> Seq.distinct
              |> Seq.toList

let ids = idPattern.Matches(htmlWithCss)
          |> Seq.map (fun m -> m.Groups.[1].Value)
          |> Seq.toList

printfn "Classes found: %A" classes
printfn "IDs found: %A" ids

// Validate CSS naming conventions
let validateCssName (name: string) =
    let validName = Regex(@"^[a-zA-Z_\-][a-zA-Z0-9_\-]*$", RegexOptions.Compiled)
    let bemConvention = Regex(@"^[a-zA-Z]+(__[a-zA-Z]+)?(--[a-zA-Z]+)*$", RegexOptions.Compiled)
    let kebabCase = Regex(@"^[a-z]+(-[a-z]+)*$", RegexOptions.Compiled)
    
    (validName.IsMatch(name), bemConvention.IsMatch(name), kebabCase.IsMatch(name))

let cssNames = ["header"; "main-content"; "nav__item"; "button--primary"; "123invalid"; "_private"; "kebab-case-name"]

printfn "\nCSS Naming Convention Validation:"
printfn "%-18s %-8s %-6s %-10s" "Name" "Valid" "BEM" "Kebab Case"
printfn "%s" (String.replicate 45 "-")

cssNames |> List.iter (fun name ->
    let (valid, bem, kebab) = validateCssName name
    printfn "%-18s %-8b %-6b %-10b" name valid bem kebab)
```

CSS selector patterns validate and parse CSS syntax for  
web development tools. Pattern extraction from HTML and CSS  
enables automated analysis, validation, and processing  
of stylesheets and markup for development workflows.  

## SQL query parsing

SQL query parsing extracts components from SQL statements  
for analysis, validation, and security checking in  
database applications and development tools.  

```f#
open System.Text.RegularExpressions

let sqlQueries = [
    "SELECT * FROM users WHERE active = 1"
    "INSERT INTO products (name, price) VALUES ('Widget', 19.99)"
    "UPDATE customers SET email = 'new@email.com' WHERE id = 123"
    "DELETE FROM orders WHERE created_date < '2023-01-01'"
    "SELECT u.name, p.title FROM users u JOIN posts p ON u.id = p.user_id"
    "CREATE TABLE inventory (id INT PRIMARY KEY, item_name VARCHAR(100))"
    "DROP TABLE old_data"
    "SELECT COUNT(*) FROM sales GROUP BY region HAVING COUNT(*) > 100"
    "SELECT * FROM products WHERE name LIKE '%phone%' ORDER BY price DESC"
    "UNION ALL SELECT id, name FROM archived_users"
]

// SQL statement type patterns
let selectPattern = Regex(@"^\s*SELECT\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let insertPattern = Regex(@"^\s*INSERT\s+INTO\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let updatePattern = Regex(@"^\s*UPDATE\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let deletePattern = Regex(@"^\s*DELETE\s+FROM\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let createPattern = Regex(@"^\s*CREATE\s+TABLE\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let dropPattern = Regex(@"^\s*DROP\s+TABLE\b", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

// SQL component extraction patterns
let tableNamePattern = Regex(@"(?:FROM|JOIN|INTO|UPDATE|TABLE)\s+([a-zA-Z_][a-zA-Z0-9_]*)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let columnPattern = Regex(@"SELECT\s+(.*?)\s+FROM", RegexOptions.IgnoreCase ||| RegexOptions.Compiled ||| RegexOptions.Singleline)
let wherePattern = Regex(@"WHERE\s+(.*?)(?:\s+GROUP\s+BY|\s+ORDER\s+BY|\s+HAVING|$)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled ||| RegexOptions.Singleline)

printfn "SQL Query Analysis:"
printfn "%-50s %-8s %-8s %-8s %-8s %-8s %-6s" "Query (truncated)" "SELECT" "INSERT" "UPDATE" "DELETE" "CREATE" "DROP"
printfn "%s" (String.replicate 105 "-")

sqlQueries |> List.iter (fun query ->
    let displayQuery = if query.Length > 50 then query.[0..46] + "..." else query
    printfn "%-50s %-8b %-8b %-8b %-8b %-8b %-6b" 
        displayQuery
        (selectPattern.IsMatch(query))
        (insertPattern.IsMatch(query))
        (updatePattern.IsMatch(query))
        (deletePattern.IsMatch(query))
        (createPattern.IsMatch(query))
        (dropPattern.IsMatch(query)))

// Extract SQL components
printfn "\nSQL Component Extraction:"
sqlQueries |> List.iter (fun query ->
    printfn "Query: %s" query
    
    // Extract table names
    let tables = tableNamePattern.Matches(query)
                |> Seq.map (fun m -> m.Groups.[1].Value)
                |> Seq.distinct
                |> Seq.toList
    printfn "  Tables: %A" tables
    
    // Extract columns (for SELECT statements)
    let columnMatch = columnPattern.Match(query)
    if columnMatch.Success then
        let columns = columnMatch.Groups.[1].Value.Trim()
        printfn "  Columns: %s" columns
    
    // Extract WHERE conditions
    let whereMatch = wherePattern.Match(query)
    if whereMatch.Success then
        let whereClause = whereMatch.Groups.[1].Value.Trim()
        printfn "  WHERE: %s" whereClause
    
    printfn "")

// SQL injection detection patterns
let suspiciousPatterns = [
    (Regex(@"'.*'.*'", RegexOptions.IgnoreCase ||| RegexOptions.Compiled), "Multiple quotes")
    (Regex(@";\s*(DROP|DELETE|UPDATE|INSERT)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled), "Statement stacking")
    (Regex(@"(UNION\s+(ALL\s+)?SELECT)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled), "UNION injection")
    (Regex(@"(OR|AND)\s+\d+\s*=\s*\d+", RegexOptions.IgnoreCase ||| RegexOptions.Compiled), "Boolean injection")
    (Regex(@"(EXEC|EXECUTE|SP_|XP_)", RegexOptions.IgnoreCase ||| RegexOptions.Compiled), "Stored procedure execution")
    (Regex(@"(--|\*\/|\*)", RegexOptions.Compiled), "Comment injection")
]

let suspiciousQueries = [
    "SELECT * FROM users WHERE username = 'admin' OR '1'='1'"
    "SELECT * FROM products; DROP TABLE users; --"
    "SELECT * FROM orders UNION SELECT username, password FROM users"
    "SELECT * FROM items WHERE id = 1 AND 1=1"
    "SELECT * FROM data; EXEC sp_configure 'show advanced options', 1"
    "SELECT * FROM users WHERE name = 'John' /* comment */ AND active = 1"
]

printfn "SQL Injection Detection:"
suspiciousQueries |> List.iter (fun query ->
    printfn "Query: %s" query
    let threats = suspiciousPatterns
                  |> List.choose (fun (pattern, description) ->
                      if pattern.IsMatch(query) then Some description else None)
    if threats.IsEmpty then
        printfn "  Status: Safe"
    else
        printfn "  Threats detected: %A" threats
    printfn "")

// Extract parameterized query placeholders
let parameterPattern = Regex(@"[@?]\w+|\?\s*(?=,|\)|\s|$)", RegexOptions.Compiled)
let parameterizedQueries = [
    "SELECT * FROM users WHERE id = @userId"
    "INSERT INTO products (name, price) VALUES (?, ?)"
    "UPDATE customers SET email = @email WHERE id = @customerId"
]

printfn "Parameterized Query Analysis:"
parameterizedQueries |> List.iter (fun query ->
    let parameters = parameterPattern.Matches(query)
                    |> Seq.map (fun m -> m.Value)
                    |> Seq.toList
    printfn "Query: %s" query
    printfn "  Parameters: %A" parameters)
```

SQL query parsing identifies statement types, extracts components  
like table names and columns, and detects potential security  
threats. Parameter extraction supports query analysis while  
injection detection patterns help secure database applications.  

## Configuration file parsing

Configuration file parsing extracts key-value pairs, sections,  
and settings from various configuration formats including  
INI files, environment files, and custom configuration syntax.  

```f#
open System.Text.RegularExpressions

let configContent = """
# Database configuration
[database]
host = localhost
port = 5432
username = admin
password = secret123
database_name = myapp

# Application settings
[application]
debug = true
log_level = INFO
max_connections = 100
timeout = 30.5

# Email configuration  
[email]
smtp_server = smtp.gmail.com
smtp_port = 587
use_tls = true
from_address = noreply@example.com

# Invalid entries
invalid_line_without_equals
= value_without_key
[malformed section
key with spaces = value
"""

// Configuration file patterns
let sectionPattern = Regex(@"^\s*\[([^\]]+)\]\s*$", RegexOptions.Compiled)
let keyValuePattern = Regex(@"^\s*([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(.+?)\s*$", RegexOptions.Compiled)
let commentPattern = Regex(@"^\s*#.*$", RegexOptions.Compiled)
let emptyLinePattern = Regex(@"^\s*$", RegexOptions.Compiled)

// Parse configuration file
let parseConfig (content: string) =
    let lines = content.Split([|'\n'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
    
    let mutable currentSection = "global"
    let mutable config = Map.empty<string, Map<string, string>>
    let mutable errors = []
    
    lines |> Array.iteri (fun lineNum line ->
        let lineNumber = lineNum + 1
        
        if commentPattern.IsMatch(line) || emptyLinePattern.IsMatch(line) then
            () // Skip comments and empty lines
        else
            let sectionMatch = sectionPattern.Match(line)
            let keyValueMatch = keyValuePattern.Match(line)
            
            if sectionMatch.Success then
                currentSection <- sectionMatch.Groups.[1].Value
                if not (config.ContainsKey(currentSection)) then
                    config <- config.Add(currentSection, Map.empty)
            elif keyValueMatch.Success then
                let key = keyValueMatch.Groups.[1].Value
                let value = keyValueMatch.Groups.[2].Value
                
                let sectionMap = config.TryFind(currentSection) |> Option.defaultValue Map.empty
                let updatedSection = sectionMap.Add(key, value)
                config <- config.Add(currentSection, updatedSection)
            else
                errors <- (lineNumber, line.Trim()) :: errors)
    
    (config, List.rev errors)

let (parsedConfig, parseErrors) = parseConfig configContent

printfn "Configuration Parsing Results:\n"

// Display parsed configuration
parsedConfig |> Map.iter (fun section settings ->
    printfn "[%s]" section
    settings |> Map.iter (fun key value ->
        printfn "  %s = %s" key value)
    printfn "")

// Display parsing errors
if not parseErrors.IsEmpty then
    printfn "Parsing Errors:"
    parseErrors |> List.iter (fun (lineNum, line) ->
        printfn "  Line %d: %s" lineNum line)
    printfn ""

// Environment file parsing (.env format)
let envContent = """
# Environment variables
DATABASE_URL=postgresql://user:pass@localhost:5432/db
API_KEY=abc123def456
DEBUG=true
PORT=3000
REDIS_URL=redis://localhost:6379
# Comment line
EMPTY_VALUE=
QUOTED_VALUE="value with spaces"
"""

let envPattern = Regex(@"^\s*([A-Z_][A-Z0-9_]*)\s*=\s*(.*?)\s*$", RegexOptions.Compiled)
let quotedValuePattern = Regex(@"^[""'](.*)['""']$", RegexOptions.Compiled)

printfn "Environment File Parsing:"
envContent.Split([|'\n'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
|> Array.iter (fun line ->
    let envMatch = envPattern.Match(line)
    if envMatch.Success then
        let key = envMatch.Groups.[1].Value
        let value = envMatch.Groups.[2].Value
        
        // Handle quoted values
        let cleanValue = 
            let quotedMatch = quotedValuePattern.Match(value)
            if quotedMatch.Success then quotedMatch.Groups.[1].Value else value
        
        printfn "  %s = %s" key cleanValue)

// Validate configuration values
let validateConfigValue (key: string) (value: string) =
    match key.ToLower() with
    | k when k.Contains("port") -> 
        match System.Int32.TryParse(value) with
        | (true, port) when port > 0 && port <= 65535 -> Some "Valid port"
        | _ -> Some "Invalid port number"
    | k when k.Contains("debug") ->
        match value.ToLower() with
        | "true" | "false" -> Some "Valid boolean"
        | _ -> Some "Invalid boolean value"
    | k when k.Contains("email") || k.Contains("address") ->
        let emailPattern = Regex(@"^[^@]+@[^@]+\.[^@]+$", RegexOptions.Compiled)
        if emailPattern.IsMatch(value) then Some "Valid email"
        else Some "Invalid email format"
    | _ -> None

printfn "\nConfiguration Validation:"
parsedConfig |> Map.iter (fun section settings ->
    settings |> Map.iter (fun key value ->
        match validateConfigValue key value with
        | Some validation -> printfn "  [%s] %s = %s (%s)" section key value validation
        | None -> ()))
```

Configuration parsing extracts structured data from various  
formats using section and key-value patterns. Validation  
ensures configuration values meet application requirements  
while error reporting helps debug malformed configuration files.  

## XML and markup parsing

XML parsing extracts elements, attributes, and content from  
markup documents using regex patterns for lightweight  
processing when full XML parsers are unnecessary.  

```f#
open System.Text.RegularExpressions

let xmlContent = """<?xml version="1.0" encoding="UTF-8"?>
<catalog>
    <book id="1" genre="fiction">
        <title>The Great Adventure</title>
        <author>John Smith</author>
        <price currency="USD">19.99</price>
        <published>2023</published>
    </book>
    <book id="2" genre="mystery">
        <title>Detective Stories</title>
        <author>Jane Doe</author>
        <price currency="EUR">15.50</price>
        <published>2022</published>
    </book>
    <!-- Comment: More books could be added here -->
    <metadata>
        <created>2023-12-01</created>
        <version>1.0</version>
    </metadata>
</catalog>"""

// XML element patterns
let xmlDeclaration = Regex(@"<\?xml[^>]*\?>", RegexOptions.Compiled)
let xmlComment = Regex(@"<!--.*?-->", RegexOptions.Compiled ||| RegexOptions.Singleline)
let xmlElement = Regex(@"<(?<tag>[a-zA-Z][a-zA-Z0-9]*)\b[^>]*>(?<content>.*?)</\1>", RegexOptions.Compiled ||| RegexOptions.Singleline)
let selfClosingElement = Regex(@"<(?<tag>[a-zA-Z][a-zA-Z0-9]*)\b[^>]*/\s*>", RegexOptions.Compiled)
let xmlAttribute = Regex(@"(\w+)\s*=\s*[""']([^""']*)[""']", RegexOptions.Compiled)

printfn "XML Structure Analysis:"
printfn "Has XML declaration: %b" (xmlDeclaration.IsMatch(xmlContent))
printfn "Comment count: %d" (xmlComment.Matches(xmlContent).Count)
printfn "Element count: %d" (xmlElement.Matches(xmlContent).Count)
printfn "Self-closing elements: %d" (selfClosingElement.Matches(xmlContent).Count)

// Extract specific elements
let bookPattern = Regex(@"<book\b[^>]*>.*?</book>", RegexOptions.Compiled ||| RegexOptions.Singleline)
let titlePattern = Regex(@"<title>([^<]*)</title>", RegexOptions.Compiled)
let pricePattern = Regex(@"<price[^>]*>([^<]*)</price>", RegexOptions.Compiled)

printfn "\nBook Information:"
bookPattern.Matches(xmlContent) |> Seq.iteri (fun i bookMatch ->
    let bookXml = bookMatch.Value
    printfn "Book %d:" (i + 1)
    
    // Extract attributes from book tag
    let bookTagPattern = Regex(@"<book\b([^>]*)>", RegexOptions.Compiled)
    let bookTagMatch = bookTagPattern.Match(bookXml)
    if bookTagMatch.Success then
        let attributes = xmlAttribute.Matches(bookTagMatch.Groups.[1].Value)
        printfn "  Attributes:"
        attributes |> Seq.iter (fun attr ->
            printfn "    %s = %s" attr.Groups.[1].Value attr.Groups.[2].Value)
    
    // Extract content
    let titleMatch = titlePattern.Match(bookXml)
    let priceMatch = pricePattern.Match(bookXml)
    
    if titleMatch.Success then
        printfn "  Title: %s" titleMatch.Groups.[1].Value
    if priceMatch.Success then
        printfn "  Price: %s" priceMatch.Groups.[1].Value
    printfn "")

// Extract all text content (strip tags)
let stripXmlTags = Regex(@"<[^>]*>", RegexOptions.Compiled)
let textContent = stripXmlTags.Replace(xmlContent, " ")
let normalizedText = Regex.Replace(textContent, @"\s+", " ").Trim()

printfn "Text Content (first 100 chars):"
printfn "%s..." (if normalizedText.Length > 100 then normalizedText.[0..99] else normalizedText)

// Validate XML structure (basic)
let validateXmlStructure (xml: string) =
    let openingTags = Regex(@"<([a-zA-Z][a-zA-Z0-9]*)\b[^/>]*>", RegexOptions.Compiled)
    let closingTags = Regex(@"</([a-zA-Z][a-zA-Z0-9]*)>", RegexOptions.Compiled)
    
    let openTags = openingTags.Matches(xml) 
                   |> Seq.map (fun m -> m.Groups.[1].Value) 
                   |> Seq.toList
    let closeTags = closingTags.Matches(xml) 
                    |> Seq.map (fun m -> m.Groups.[1].Value) 
                    |> Seq.toList
    
    let openCount = openTags |> List.countBy id |> Map.ofList
    let closeCount = closeTags |> List.countBy id |> Map.ofList
    
    let isBalanced = openCount |> Map.forall (fun tag count ->
        Map.tryFind tag closeCount = Some count)
    
    (isBalanced, openTags, closeTags)

let xmlSamples = [
    "<root><child>content</child></root>"  // Valid
    "<root><child>content</root>"          // Invalid - mismatched tags
    "<root><child>content</child>"         // Invalid - unclosed root
    "<self-closing />"                     // Valid self-closing
]

printfn "\nXML Validation:"
xmlSamples |> List.iter (fun sample ->
    let (isBalanced, openTags, closeTags) = validateXmlStructure sample
    printfn "XML: %s" sample
    printfn "  Balanced: %b" isBalanced
    printfn "  Open tags: %A" openTags
    printfn "  Close tags: %A" closeTags
    printfn "")

// Extract namespace information
let namespacePattern = Regex(@"xmlns:?(\w+)?=[""']([^""']*)[""']", RegexOptions.Compiled)
let namespacedContent = """<root xmlns:book="http://example.com/books" xmlns="http://example.com/default">
    <book:title>Sample Title</book:title>
    <book:author xmlns:person="http://example.com/person">
        <person:name>John Doe</person:name>
    </book:author>
</root>"""

printfn "Namespace Extraction:"
namespacePattern.Matches(namespacedContent) |> Seq.iter (fun nsMatch ->
    let prefix = if nsMatch.Groups.[1].Success then nsMatch.Groups.[1].Value else "default"
    let uri = nsMatch.Groups.[2].Value
    printfn "  %s -> %s" prefix uri)
```

XML parsing with regex handles simple extraction tasks like  
finding elements, attributes, and content. While not suitable  
for complex XML processing, regex patterns work well for  
basic markup analysis and content extraction scenarios.  

## Performance optimization

Regex performance optimization techniques include compilation,  
pattern simplification, and efficient matching strategies  
for high-performance text processing applications.  

```f#
open System.Text.RegularExpressions
open System.Diagnostics

let generateTestData size =
    let random = System.Random(42) // Seed for reproducible results
    let words = [|"apple"; "banana"; "cherry"; "date"; "elderberry"; "fig"; "grape"|]
    let emails = [|"user@domain.com"; "test@example.org"; "admin@site.net"|]
    let phones = [|"555-1234"; "555-5678"; "555-9999"|]
    
    [|1..size|] |> Array.map (fun _ ->
        let wordCount = random.Next(5, 20)
        let text = [|1..wordCount|] |> Array.map (fun _ -> words.[random.Next(words.Length)]) |> String.concat " "
        let email = emails.[random.Next(emails.Length)]
        let phone = phones.[random.Next(phones.Length)]
        sprintf "%s Contact: %s Phone: %s" text email phone)

let testData = generateTestData 10000

// Performance comparison: Compiled vs Non-compiled
let emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
let nonCompiledRegex = Regex(emailPattern)
let compiledRegex = Regex(emailPattern, RegexOptions.Compiled)

let measureTime (name: string) (action: unit -> 'a) =
    let stopwatch = Stopwatch.StartNew()
    let result = action()
    stopwatch.Stop()
    printfn "%s: %d ms" name stopwatch.ElapsedMilliseconds
    result

printfn "Performance Comparison (10,000 operations):\n"

// Test non-compiled regex
let nonCompiledResults = measureTime "Non-compiled Regex" (fun () ->
    testData |> Array.sumBy (fun text -> nonCompiledRegex.Matches(text).Count))

// Test compiled regex  
let compiledResults = measureTime "Compiled Regex" (fun () ->
    testData |> Array.sumBy (fun text -> compiledRegex.Matches(text).Count))

printfn "Results: Non-compiled=%d, Compiled=%d\n" nonCompiledResults compiledResults

// Pattern optimization examples
let inefficientPattern = @".*@.*\..*"
let efficientPattern = @"[^@]+@[^@.]+\.[^@]+"
let veryEfficientPattern = @"\S+@\S+\.\S+"

let inefficientRegex = Regex(inefficientPattern, RegexOptions.Compiled)
let efficientRegex = Regex(efficientPattern, RegexOptions.Compiled) 
let veryEfficientRegex = Regex(veryEfficientPattern, RegexOptions.Compiled)

let sampleEmails = [|
    "user@domain.com"
    "invalid.email"
    "test@example.org"
    "notanemail"
    "admin@site.net.uk"
|]

printfn "Pattern Efficiency Comparison:"

measureTime "Inefficient Pattern (.*@.*\\..*)" (fun () ->
    [|1..1000|] |> Array.iter (fun _ ->
        sampleEmails |> Array.iter (fun email -> 
            inefficientRegex.IsMatch(email) |> ignore))) |> ignore

measureTime "Efficient Pattern ([^@]+@[^@.]+\\.[^@]+)" (fun () ->
    [|1..1000|] |> Array.iter (fun _ ->
        sampleEmails |> Array.iter (fun email -> 
            efficientRegex.IsMatch(email) |> ignore))) |> ignore

measureTime "Very Efficient Pattern (\\S+@\\S+\\.\\S+)" (fun () ->
    [|1..1000|] |> Array.iter (fun _ ->
        sampleEmails |> Array.iter (fun email -> 
            veryEfficientRegex.IsMatch(email) |> ignore))) |> ignore

// Memory usage optimization
let heavyPattern = @"(\w+)\s+(\w+)\s+(\w+)\s+(\w+)\s+(\w+)\s+(\w+)\s+(\w+)\s+(\w+)"
let lightPattern = @"\w+(?:\s+\w+){7}"

let heavyRegex = Regex(heavyPattern, RegexOptions.Compiled)
let lightRegex = Regex(lightPattern, RegexOptions.Compiled)

let longText = String.replicate 1000 "word1 word2 word3 word4 word5 word6 word7 word8 "

printfn "\nCapturing Groups vs Non-capturing:"

measureTime "Heavy Pattern (8 capturing groups)" (fun () ->
    [|1..100|] |> Array.iter (fun _ ->
        heavyRegex.Matches(longText) |> Seq.iter ignore)) |> ignore

measureTime "Light Pattern (non-capturing)" (fun () ->
    [|1..100|] |> Array.iter (fun _ ->
        lightRegex.Matches(longText) |> Seq.iter ignore)) |> ignore

// Anchoring for performance
let unanchoredPattern = @"\d{3}-\d{3}-\d{4}"
let anchoredPattern = @"^\d{3}-\d{3}-\d{4}$"

let unanchoredRegex = Regex(unanchoredPattern, RegexOptions.Compiled)
let anchoredRegex = Regex(anchoredPattern, RegexOptions.Compiled)

let phoneNumbers = [|
    "555-123-4567"
    "not a phone number at all, this is a very long string that doesn't match"
    "123-456-7890"
    "another very long string that definitely doesn't contain a phone number"
|]

printfn "\nAnchoring Optimization:"

measureTime "Unanchored Pattern" (fun () ->
    [|1..1000|] |> Array.iter (fun _ ->
        phoneNumbers |> Array.iter (fun phone -> 
            unanchoredRegex.IsMatch(phone) |> ignore))) |> ignore

measureTime "Anchored Pattern" (fun () ->
    [|1..1000|] |> Array.iter (fun _ ->
        phoneNumbers |> Array.iter (fun phone -> 
            anchoredRegex.IsMatch(phone) |> ignore))) |> ignore

// Best practices summary
printfn "\nPerformance Best Practices:"
printfn "1. Use RegexOptions.Compiled for frequently used patterns"
printfn "2. Avoid greedy quantifiers (.*) when possible" 
printfn "3. Use character classes instead of alternations"
printfn "4. Anchor patterns when validating entire strings"
printfn "5. Minimize capturing groups when captures aren't needed"
printfn "6. Use specific character classes (\\d vs [0-9])"
printfn "7. Consider preprocessing text to reduce regex complexity"
```

Performance optimization focuses on pattern efficiency, regex  
compilation, and matching strategies. Compiled patterns,  
specific character classes, anchoring, and minimal capturing  
significantly improve regex performance in high-throughput  
text processing applications.  

## Unicode and internationalization

Unicode patterns handle international text with character  
classes for different scripts, normalization considerations,  
and cultural-specific matching requirements for global  
applications supporting multiple languages.  

```f#
open System.Text.RegularExpressions

let internationalText = """
English: Hello World!
Spanish: ¡Hola Mundo!
French: Bonjour le Monde!
German: Hallo Welt!
Russian: Привет мир!
Chinese: 你好世界！
Japanese: こんにちは世界！
Arabic: مرحبا بالعالم!
Hindi: हैलो वर्ल्ड!
Korean: 안녕하세요 세계!
Greek: Γεια σου κόσμε!
Hebrew: שלום עולם!
Thai: สวัสดีโลก!
"""

// Unicode character class patterns
let latinScript = Regex(@"\p{IsLatin}", RegexOptions.Compiled)
let cyrillicScript = Regex(@"\p{IsCyrillic}", RegexOptions.Compiled)
let cjkScript = Regex(@"\p{IsCJK}", RegexOptions.Compiled)
let arabicScript = Regex(@"\p{IsArabic}", RegexOptions.Compiled)

// Unicode categories
let letters = Regex(@"\p{L}", RegexOptions.Compiled)
let marks = Regex(@"\p{M}", RegexOptions.Compiled)
let numbers = Regex(@"\p{N}", RegexOptions.Compiled)
let punctuation = Regex(@"\p{P}", RegexOptions.Compiled)
let symbols = Regex(@"\p{S}", RegexOptions.Compiled)

printfn "Unicode Script Detection:"
internationalText.Split('\n')
|> Array.filter (fun line -> not (System.String.IsNullOrWhiteSpace(line)))
|> Array.iter (fun line ->
    printfn "Line: %s" line
    printfn "  Latin: %b" (latinScript.IsMatch(line))
    printfn "  Cyrillic: %b" (cyrillicScript.IsMatch(line))
    printfn "  CJK: %b" (cjkScript.IsMatch(line))
    printfn "  Arabic: %b" (arabicScript.IsMatch(line))
    printfn "")

// International email validation
let internationalEmails = [
    "user@domain.com"           // ASCII
    "测试@测试.中国"             // Chinese
    "пользователь@домен.рф"      // Russian  
    "usuario@domínio.org"       // Spanish with accent
    "δοκιμή@παράδειγμα.ελ"      // Greek
    "משתמש@דוגמה.ישראל"        // Hebrew
    "utilisateur@domaine.français" // French with special chars
]

// ASCII-only email pattern
let asciiEmail = Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)

// International email pattern (simplified)
let intlEmail = Regex(@"^[\p{L}\p{N}._%+-]+@[\p{L}\p{N}.-]+\.[\p{L}]{2,}$", RegexOptions.Compiled)

printfn "International Email Validation:"
printfn "%-30s %-10s %-15s" "Email" "ASCII Only" "International"
printfn "%s" (String.replicate 60 "-")

internationalEmails |> List.iter (fun email ->
    printfn "%-30s %-10b %-15b" 
        (if email.Length > 30 then email.[0..26] + "..." else email)
        (asciiEmail.IsMatch(email))
        (intlEmail.IsMatch(email)))

// Unicode normalization considerations
let accentedWords = [
    "café"      // é as single character
    "cafe\u0301" // e + combining acute accent
    "naïve"     // ï as single character  
    "nai\u0308ve" // i + combining diaeresis
]

let simpleWordPattern = Regex(@"^[a-zA-Z]+$", RegexOptions.Compiled)
let unicodeWordPattern = Regex(@"^[\p{L}]+$", RegexOptions.Compiled)

printfn "\nUnicode Normalization:"
accentedWords |> List.iter (fun word ->
    printfn "Word: %s (length: %d)" word word.Length
    printfn "  Simple pattern: %b" (simpleWordPattern.IsMatch(word))
    printfn "  Unicode pattern: %b" (unicodeWordPattern.IsMatch(word))
    printfn "")

// Cultural-specific patterns
let phonePatterns = [
    ("US", @"^\+?1?[-.\s]?\(?([0-9]{3})\)?[-.\s]?([0-9]{3})[-.\s]?([0-9]{4})$")
    ("UK", @"^\+?44\s?[0-9]{4}\s?[0-9]{6}$")
    ("France", @"^\+?33\s?[0-9]\s?([0-9]{2}\s?){4}$")
    ("Germany", @"^\+?49\s?[0-9]{3,4}\s?[0-9]{6,8}$")
]

let testPhones = [
    "555-123-4567"          // US
    "+1 555 123 4567"       // US international
    "+44 1234 567890"       // UK
    "+33 1 23 45 67 89"     // France
    "+49 123 12345678"      // Germany
]

printfn "Cultural Phone Pattern Matching:"
testPhones |> List.iter (fun phone ->
    printfn "Phone: %s" phone
    phonePatterns |> List.iter (fun (country, pattern) ->
        let regex = Regex(pattern, RegexOptions.Compiled)
        printfn "  %s: %b" country (regex.IsMatch(phone)))
    printfn "")
```

Unicode regex patterns support international text processing  
with script detection, character category matching, and  
cultural-specific validation. Proper Unicode handling  
ensures applications work correctly across languages and regions.  

## Atomic groups and possessive quantifiers

Atomic groups and possessive quantifiers prevent backtracking  
for improved performance and precise matching control in  
complex patterns where standard quantifiers cause issues.  

```f#
open System.Text.RegularExpressions

// Note: .NET regex doesn't support atomic groups (?>...) or possessive quantifiers directly
// These examples show equivalent techniques using non-backtracking patterns

let catastrophicBacktrackingExample = """
This is a test string with lots of a's: aaaaaaaaaaaaaaaaaaaaaaaaaaab
Another string: aaaaaaaaaaaaaaaaaaaaaaaaaaaac  
Normal string: hello world
Problematic: aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaX
"""

// Problematic pattern that can cause excessive backtracking
let backtrackingPattern = @"a+a+b"
let optimizedPattern = @"a++b"  // Possessive (not directly supported in .NET)
let efficientPattern = @"a*ab"   // Equivalent without backtracking issues

let backtrackingRegex = Regex(backtrackingPattern, RegexOptions.Compiled)
let efficientRegex = Regex(efficientPattern, RegexOptions.Compiled)

let testStrings = [
    "aaab"                          // Should match
    "aaaaaaab"                      // Should match
    "aaaaaaaaaaaaaaaaaaaaab"        // Should match
    "aaaaaaaaaaaaaaaaaaaaaX"        // Should not match (causes backtracking)
]

printfn "Backtracking Comparison:"
testStrings |> List.iter (fun test ->
    printfn "String: %s" test
    
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let backtrackResult = backtrackingRegex.IsMatch(test)
    stopwatch.Stop()
    let backtrackTime = stopwatch.ElapsedTicks
    
    stopwatch.Restart()
    let efficientResult = efficientRegex.IsMatch(test)
    stopwatch.Stop()
    let efficientTime = stopwatch.ElapsedTicks
    
    printfn "  Backtracking pattern: %b (%d ticks)" backtrackResult backtrackTime
    printfn "  Efficient pattern: %b (%d ticks)" efficientResult efficientTime
    printfn "")

// Simulating atomic group behavior with lookahead
let atomicGroupLike = """
String with quoted text: "This is a quoted string" and more text.
Another: "Unclosed quoted string and more text here.
Mixed: "First quote" and "second quote" text.
"""

// Traditional greedy pattern (can over-match)
let greedyQuoted = Regex(@""".*""", RegexOptions.Compiled)

// Atomic group simulation using lookahead
let atomicQuoted = Regex(@"""(?>[^""]*)+""", RegexOptions.Compiled) // Not directly supported
let efficientQuoted = Regex(@"""[^""]*""", RegexOptions.Compiled)    // Equivalent efficient pattern

printfn "Quoted String Extraction:"
printfn "Text: %s" atomicGroupLike

printfn "\nGreedy matches:"
greedyQuoted.Matches(atomicGroupLike) |> Seq.iter (fun m ->
    printfn "  %s" m.Value)

printfn "Efficient matches:"
efficientQuoted.Matches(atomicGroupLike) |> Seq.iter (fun m ->
    printfn "  %s" m.Value)

// Preventing backtracking in nested quantifiers
let nestedQuantifierText = "aaaaaaaaaaaaaaaaaaaaaaaaaaab"
let problematicNested = @"(a+)+(b|c)"
let efficientNested = @"a+(b|c)"

let problematicRegex = Regex(problematicNested, RegexOptions.Compiled)
let efficientNestedRegex = Regex(efficientNested, RegexOptions.Compiled)

printfn "\nNested Quantifier Performance:"
let longAString = String.replicate 30 "a" + "X"  // No match, causes backtracking

let measureRegexTime (regex: Regex) (text: string) =
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let result = regex.IsMatch(text)
    stopwatch.Stop()
    (result, stopwatch.ElapsedTicks)

let (prob1, time1) = measureRegexTime problematicRegex nestedQuantifierText
let (prob2, time2) = measureRegexTime problematicRegex longAString
let (eff1, eTime1) = measureRegexTime efficientNestedRegex nestedQuantifierText  
let (eff2, eTime2) = measureRegexTime efficientNestedRegex longAString

printfn "Matching string '%s':" nestedQuantifierText
printfn "  Problematic: %b (%d ticks)" prob1 time1
printfn "  Efficient: %b (%d ticks)" eff1 eTime1

printfn "Non-matching string (causes backtracking):"
printfn "  Problematic: %b (%d ticks)" prob2 time2
printfn "  Efficient: %b (%d ticks)" eff2 eTime2

// Best practices for avoiding backtracking
printfn "\nBacktracking Prevention Techniques:"
printfn "1. Use specific character classes instead of .*"
printfn "2. Anchor patterns when possible (^, $)"
printfn "3. Use non-capturing groups (?:...) when captures aren't needed"
printfn "4. Replace nested quantifiers with flattened alternatives"
printfn "5. Use character classes [^...] to prevent overmatching"
printfn "6. Consider preprocessing text to simplify regex requirements"
printfn "7. Test patterns with edge cases to identify backtracking"
```

Atomic groups and possessive quantifiers prevent excessive  
backtracking in complex patterns. While .NET doesn't directly  
support these constructs, equivalent techniques using  
character classes and anchoring achieve similar performance  
improvements without backtracking issues.  

## Real-world parsing scenarios

Real-world parsing combines multiple regex patterns to extract  
structured data from complex formats like server logs,  
configuration files, and data exports requiring robust  
error handling and flexible pattern matching.  

```f#
open System.Text.RegularExpressions

// Apache/Nginx combined log format parsing
let accessLogData = """
192.168.1.1 - - [01/Dec/2023:14:30:25 +0000] "GET /api/users HTTP/1.1" 200 1234 "https://example.com/login" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
10.0.0.5 - admin [01/Dec/2023:14:30:26 +0000] "POST /api/login HTTP/1.1" 401 89 "-" "curl/7.68.0"
172.16.0.10 - - [01/Dec/2023:14:30:27 +0000] "GET /static/style.css HTTP/1.1" 304 0 "https://example.com/" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36"
192.168.1.100 - user123 [01/Dec/2023:14:30:28 +0000] "DELETE /api/posts/456 HTTP/1.1" 204 0 "https://example.com/posts" "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36"
"""

let combinedLogPattern = Regex(@"^(?<ip>\S+) \S+ (?<user>\S+) \[(?<timestamp>[^\]]+)\] ""(?<method>\S+) (?<url>\S+) (?<protocol>[^""]+)"" (?<status>\d+) (?<size>\d+) ""(?<referer>[^""]*)"" ""(?<useragent>[^""]*)""$", RegexOptions.Compiled ||| RegexOptions.Multiline)

type LogEntry = {
    IP: string
    User: string
    Timestamp: string
    Method: string
    URL: string
    Protocol: string
    Status: int
    Size: int
    Referer: string
    UserAgent: string
}

let parseAccessLogs (logData: string) =
    combinedLogPattern.Matches(logData)
    |> Seq.choose (fun m ->
        try
            Some {
                IP = m.Groups["ip"].Value
                User = if m.Groups["user"].Value = "-" then "" else m.Groups["user"].Value
                Timestamp = m.Groups["timestamp"].Value
                Method = m.Groups["method"].Value
                URL = m.Groups["url"].Value
                Protocol = m.Groups["protocol"].Value
                Status = int m.Groups["status"].Value
                Size = int m.Groups["size"].Value
                Referer = if m.Groups["referer"].Value = "-" then "" else m.Groups["referer"].Value
                UserAgent = m.Groups["useragent"].Value
            }
        with
        | _ -> None)
    |> Seq.toList

let logEntries = parseAccessLogs accessLogData

printfn "Parsed Access Log Entries:"
logEntries |> List.iteri (fun i entry ->
    printfn "Entry %d:" (i + 1)
    printfn "  IP: %s" entry.IP
    printfn "  User: %s" (if entry.User = "" then "anonymous" else entry.User)
    printfn "  Method: %s" entry.Method
    printfn "  URL: %s" entry.URL
    printfn "  Status: %d" entry.Status
    printfn "  Size: %d bytes" entry.Size
    printfn "")

// CSV data with complex quoting and escaping
let csvData = """
"Name","Age","Email","Notes"
"John Doe",30,"john.doe@example.com","Regular customer, prefers email contact"
"Jane Smith",25,"jane.smith@company.org","VIP customer, ""premium"" account holder"
"Bob Wilson",45,"bob.wilson@domain.net","New customer
Multi-line notes with, commas and ""quotes"""
"Alice Johnson",35,"alice@example.com",""
"Charlie Brown",28,"charlie.brown@email.org","Account created on 12/01/2023"
"""

// Complex CSV parsing with quoted fields and embedded newlines
let csvFieldPattern = Regex(@"""([^""]|"""")*""|[^,\r\n]*", RegexOptions.Compiled)
let quotedFieldPattern = Regex(@"^""(.*)""$", RegexOptions.Compiled ||| RegexOptions.Singleline)

let parseCsvLine (line: string) =
    csvFieldPattern.Matches(line)
    |> Seq.map (fun m ->
        let field = m.Value.TrimEnd(',')
        let quotedMatch = quotedFieldPattern.Match(field)
        if quotedMatch.Success then
            quotedMatch.Groups.[1].Value.Replace("""""", "\"")
        else field)
    |> Seq.toArray

printfn "CSV Parsing Results:"
let csvLines = csvData.Trim().Split('\n')
csvLines |> Array.iteri (fun i line ->
    if not (System.String.IsNullOrWhiteSpace(line)) then
        let fields = parseCsvLine line
        printfn "Row %d: %A" (i + 1) fields)

// Configuration file with complex syntax
let configData = """
# Application configuration
[database]
host = localhost
port = 5432
# Connection string with embedded semicolons
connection_string = "Server=localhost;Database=myapp;User Id=admin;Password=secret;123;"

[features]
# Boolean flags  
enable_logging = true
debug_mode = false
# Numeric values
max_connections = 100
timeout = 30.5

# Array values
allowed_ips = ["192.168.1.1", "10.0.0.1", "172.16.0.1"]
admin_users = ["admin", "root", "administrator"]

[advanced]
# JSON-like nested configuration
settings = {
    "theme": "dark",
    "notifications": {
        "email": true,
        "sms": false
    }
}
"""

// Multi-pattern configuration parser
let configSectionPattern = Regex(@"^\s*\[([^\]]+)\]\s*$", RegexOptions.Compiled)
let configKeyValuePattern = Regex(@"^\s*([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(.+?)\s*$", RegexOptions.Compiled)
let configCommentPattern = Regex(@"^\s*#.*$", RegexOptions.Compiled)
let configStringPattern = Regex(@"^""(.*)""$", RegexOptions.Compiled ||| RegexOptions.Singleline)
let configArrayPattern = Regex(@"^\[(.*)\]$", RegexOptions.Compiled ||| RegexOptions.Singleline)
let configBoolPattern = Regex(@"^(true|false)$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)
let configNumberPattern = Regex(@"^\d+(\.\d+)?$", RegexOptions.Compiled)

let parseConfigValue (value: string) =
    let stringMatch = configStringPattern.Match(value)
    let arrayMatch = configArrayPattern.Match(value)
    let boolMatch = configBoolPattern.Match(value)
    let numberMatch = configNumberPattern.Match(value)
    
    if stringMatch.Success then
        ("string", stringMatch.Groups.[1].Value)
    elif arrayMatch.Success then
        ("array", arrayMatch.Groups.[1].Value)
    elif boolMatch.Success then
        ("boolean", value.ToLower())
    elif numberMatch.Success then
        ("number", value)
    else
        ("object", value)

printfn "\nConfiguration Parsing:"
configData.Split('\n')
|> Array.filter (fun line -> not (System.String.IsNullOrWhiteSpace(line)) && not (configCommentPattern.IsMatch(line)))
|> Array.iter (fun line ->
    let sectionMatch = configSectionPattern.Match(line)
    let keyValueMatch = configKeyValuePattern.Match(line)
    
    if sectionMatch.Success then
        printfn "[%s]" sectionMatch.Groups.[1].Value
    elif keyValueMatch.Success then
        let key = keyValueMatch.Groups.[1].Value
        let value = keyValueMatch.Groups.[2].Value
        let (valueType, parsedValue) = parseConfigValue value
        printfn "  %s = %s (%s)" key parsedValue valueType)
```

Real-world parsing scenarios require combining multiple  
regex patterns with error handling and type conversion.  
Structured parsing extracts meaningful data from complex  
formats while handling edge cases and malformed input  
gracefully for robust production applications.  

## Regex compilation and caching

Regex compilation and intelligent caching strategies optimize  
performance for applications with dynamic patterns or  
high-frequency matching operations requiring efficient  
pattern management and memory usage.  

```f#
open System.Text.RegularExpressions
open System.Collections.Concurrent
open System.Diagnostics

// Regex cache implementation
type RegexCache() =
    let cache = ConcurrentDictionary<string * RegexOptions, Regex>()
    let mutable hitCount = 0
    let mutable missCount = 0
    
    member this.GetRegex(pattern: string, options: RegexOptions) =
        let key = (pattern, options)
        match cache.TryGetValue(key) with
        | (true, regex) -> 
            System.Threading.Interlocked.Increment(&hitCount) |> ignore
            regex
        | (false, _) ->
            System.Threading.Interlocked.Increment(&missCount) |> ignore
            let newRegex = Regex(pattern, options ||| RegexOptions.Compiled)
            cache.TryAdd(key, newRegex) |> ignore
            newRegex
    
    member this.GetRegex(pattern: string) = 
        this.GetRegex(pattern, RegexOptions.None)
    
    member this.Stats = (hitCount, missCount, cache.Count)
    
    member this.Clear() = 
        cache.Clear()
        hitCount <- 0
        missCount <- 0

let regexCache = RegexCache()

// Performance testing with and without caching
let testPatterns = [|
    @"\d{3}-\d{3}-\d{4}"        // Phone number
    @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"  // Email
    @"^https?://[^\s/$.?#].[^\s]*$"  // URL
    @"^\d{4}-\d{2}-\d{2}$"      // Date
    @"[A-Z]{2,3}\d{3,4}"        // Postal code
|]

let testData = [|
    "555-123-4567"
    "user@example.com"
    "https://www.example.com"
    "2023-12-01"
    "AB1234"
    "invalid data"
|]

// Test without caching (creating new Regex each time)
let testWithoutCache () =
    let stopwatch = Stopwatch.StartNew()
    let mutable totalMatches = 0
    
    for _ in 1..1000 do
        for pattern in testPatterns do
            for data in testData do
                let regex = Regex(pattern, RegexOptions.Compiled)
                if regex.IsMatch(data) then
                    totalMatches <- totalMatches + 1
    
    stopwatch.Stop()
    (totalMatches, stopwatch.ElapsedMilliseconds)

// Test with caching
let testWithCache () =
    regexCache.Clear()
    let stopwatch = Stopwatch.StartNew()
    let mutable totalMatches = 0
    
    for _ in 1..1000 do
        for pattern in testPatterns do
            for data in testData do
                let regex = regexCache.GetRegex(pattern)
                if regex.IsMatch(data) then
                    totalMatches <- totalMatches + 1
    
    stopwatch.Stop()
    let (hits, misses, cacheSize) = regexCache.Stats
    (totalMatches, stopwatch.ElapsedMilliseconds, hits, misses, cacheSize)

printfn "Regex Caching Performance Comparison:\n"

let (matchesNoCache, timeNoCache) = testWithoutCache()
printfn "Without caching:"
printfn "  Matches found: %d" matchesNoCache
printfn "  Time elapsed: %d ms" timeNoCache

let (matchesWithCache, timeWithCache, hits, misses, cacheSize) = testWithCache()
printfn "\nWith caching:"
printfn "  Matches found: %d" matchesWithCache
printfn "  Time elapsed: %d ms" timeWithCache
printfn "  Cache hits: %d" hits
printfn "  Cache misses: %d" misses
printfn "  Cache size: %d" cacheSize
printfn "  Performance improvement: %.1fx" (float timeNoCache / float timeWithCache)

// Dynamic pattern compilation
let dynamicPatternTests = [|
    ("Phone", @"\d{3}-\d{3}-\d{4}")
    ("Email", @"[^@]+@[^@]+\.[^@]+")
    ("IP", @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
    ("Date", @"\d{4}-\d{2}-\d{2}")
    ("Time", @"\d{2}:\d{2}:\d{2}")
    ("Phone", @"\d{3}-\d{3}-\d{4}")  // Duplicate pattern
    ("Email", @"[^@]+@[^@]+\.[^@]+") // Duplicate pattern
|]

printfn "\nDynamic Pattern Compilation:"
dynamicPatternTests |> Array.iter (fun (name, pattern) ->
    let regex = regexCache.GetRegex(pattern)
    printfn "Pattern '%s': %s" name pattern)

let (finalHits, finalMisses, finalCacheSize) = regexCache.Stats
printfn "\nFinal cache statistics:"
printfn "  Total hits: %d" finalHits
printfn "  Total misses: %d" finalMisses
printfn "  Cache size: %d patterns" finalCacheSize
printfn "  Hit ratio: %.1f%%" (float finalHits / float (finalHits + finalMisses) * 100.0)

// Memory usage considerations
let measureMemoryUsage (action: unit -> unit) =
    System.GC.Collect()
    System.GC.WaitForPendingFinalizers()
    System.GC.Collect()
    
    let beforeMemory = System.GC.GetTotalMemory(false)
    action()
    let afterMemory = System.GC.GetTotalMemory(false)
    afterMemory - beforeMemory

printfn "\nMemory Usage Comparison:"

let memoryWithoutCache = measureMemoryUsage (fun () ->
    for _ in 1..100 do
        for pattern in testPatterns do
            let regex = Regex(pattern, RegexOptions.Compiled)
            testData |> Array.iter (fun data -> regex.IsMatch(data) |> ignore))

let memoryWithCache = measureMemoryUsage (fun () ->
    for _ in 1..100 do
        for pattern in testPatterns do
            let regex = regexCache.GetRegex(pattern)
            testData |> Array.iter (fun data -> regex.IsMatch(data) |> ignore))

printfn "Memory without caching: %d bytes" memoryWithoutCache
printfn "Memory with caching: %d bytes" memoryWithCache
printfn "Memory savings: %d bytes" (memoryWithoutCache - memoryWithCache)

// Best practices for regex caching
printfn "\nRegex Caching Best Practices:"
printfn "1. Cache compiled regex objects for frequently used patterns"
printfn "2. Use thread-safe collections for concurrent access"
printfn "3. Implement cache size limits to prevent memory leaks"
printfn "4. Monitor cache hit ratios to optimize pattern usage"
printfn "5. Consider pattern normalization for better cache efficiency"
printfn "6. Use weak references for large caches to allow GC"
printfn "7. Profile memory usage with different cache sizes"
```

Regex compilation and caching significantly improve performance  
by reusing compiled patterns. Intelligent caching strategies  
with hit ratio monitoring and memory management optimize  
resource usage while maintaining high-performance pattern  
matching in production applications.  

## Advanced replacement techniques

Advanced regex replacement operations use complex substitution  
patterns, conditional replacements, and custom functions  
for sophisticated text transformations and data processing  
scenarios requiring dynamic content generation.  

```f#
open System.Text.RegularExpressions
open System

// Complex replacement with backreferences and nested groups
let sourceText = """
Contact Information:
John Doe - john.doe@example.com - (555) 123-4567
Jane Smith - jane.smith@company.org - (555) 987-6543  
Bob Wilson - bob.wilson@domain.net - (555) 555-1234
Alice Johnson - alice@example.com - (555) 111-2222
"""

// Pattern to extract name, email, and phone
let contactPattern = Regex(@"([A-Za-z\s]+) - ([^-]+@[^-]+) - \((\d{3})\) (\d{3})-(\d{4})", RegexOptions.Compiled)

// Basic backreference replacement
let formatAsXml = contactPattern.Replace(sourceText, 
    "<contact><name>$1</name><email>$2</email><phone>$3-$4-$5</phone></contact>")

printfn "XML Format Replacement:"
printfn "%s\n" formatAsXml

// Conditional replacement based on matched content
let conditionalPattern = Regex(@"(\w+)@(\w+\.(com|org|net))", RegexOptions.Compiled)

let conditionalReplacement = MatchEvaluator(fun (m: Match) ->
    let username = m.Groups.[1].Value
    let domain = m.Groups.[2].Value
    let tld = m.Groups.[3].Value
    
    match tld with
    | "com" -> sprintf "[COMMERCIAL] %s at %s" username domain
    | "org" -> sprintf "[ORGANIZATION] %s at %s" username domain
    | "net" -> sprintf "[NETWORK] %s at %s" username domain
    | _ -> m.Value)

let conditionalResult = conditionalPattern.Replace(sourceText, conditionalReplacement)

printfn "Conditional Replacement Based on TLD:"
printfn "%s\n" conditionalResult

// Complex mathematical replacement
let mathText = "Calculate: 15 + 25, 100 - 30, 8 * 7, 64 / 8, 2 ^ 3"
let mathPattern = Regex(@"(\d+)\s*([+\-*/^])\s*(\d+)", RegexOptions.Compiled)

let mathEvaluator = MatchEvaluator(fun (m: Match) ->
    let left = double m.Groups.[1].Value
    let operator = m.Groups.[2].Value
    let right = double m.Groups.[3].Value
    
    let result = 
        match operator with
        | "+" -> left + right
        | "-" -> left - right
        | "*" -> left * right
        | "/" -> if right <> 0.0 then left / right else Double.NaN
        | "^" -> Math.Pow(left, right)
        | _ -> Double.NaN
    
    sprintf "(%s = %.2f)" m.Value result)

let mathResults = mathPattern.Replace(mathText, mathEvaluator)
printfn "Mathematical Expression Evaluation:"
printfn "%s\n" mathResults

// Template replacement with named groups
let templateText = """
Dear {{name}},

Thank you for your order #{{order_id}} placed on {{date}}.
Your total amount of ${{amount}} will be charged to your card ending in {{card_last4}}.

Estimated delivery: {{delivery_date}}
Tracking number: {{tracking}}

Best regards,
{{company_name}}
"""

let templateData = Map.ofList [
    ("name", "John Doe")
    ("order_id", "12345")  
    ("date", "December 1, 2023")
    ("amount", "99.99")
    ("card_last4", "1234")
    ("delivery_date", "December 5, 2023")
    ("tracking", "1Z999AA1234567890")
    ("company_name", "Example Corp")
]

let templatePattern = Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled)

let templateEvaluator = MatchEvaluator(fun (m: Match) ->
    let key = m.Groups.[1].Value
    match templateData.TryFind(key) with
    | Some value -> value
    | None -> "[MISSING: " + key + "]")

let populatedTemplate = templatePattern.Replace(templateText, templateEvaluator)
printfn "Template Replacement:"
printfn "%s\n" populatedTemplate

// URL transformation and sanitization
let urlText = """
Check out these links:
https://www.example.com/page?param1=value1&param2=value2
http://insecure.site/path
https://subdomain.example.org/long/path/to/resource
ftp://files.example.com/document.pdf
mailto:user@example.com
"""

let urlPattern = Regex(@"(https?|ftp|mailto):([^\s]+)", RegexOptions.Compiled)

let urlTransformer = MatchEvaluator(fun (m: Match) ->
    let protocol = m.Groups.[1].Value
    let rest = m.Groups.[2].Value
    
    match protocol with
    | "http" -> sprintf "[INSECURE] %s%s" protocol rest
    | "https" -> sprintf "[SECURE] %s%s" protocol rest  
    | "ftp" -> sprintf "[FILE] %s%s" protocol rest
    | "mailto" -> sprintf "[EMAIL] %s" rest
    | _ -> m.Value)

let transformedUrls = urlPattern.Replace(urlText, urlTransformer)
printfn "URL Transformation:"
printfn "%s\n" transformedUrls

// Data format conversion
let csvData = "John,Doe,30,Engineer|Jane,Smith,25,Designer|Bob,Wilson,35,Manager"
let csvPattern = Regex(@"([^,]+),([^,]+),([^,]+),([^|]+)", RegexOptions.Compiled)

let jsonConverter = MatchEvaluator(fun (m: Match) ->
    let firstName = m.Groups.[1].Value
    let lastName = m.Groups.[2].Value
    let age = m.Groups.[3].Value
    let occupation = m.Groups.[4].Value
    
    sprintf """{"firstName":"%s","lastName":"%s","age":%s,"occupation":"%s"}""" 
        firstName lastName age occupation)

let jsonData = csvPattern.Replace(csvData.Replace("|", "\n"), jsonConverter)
printfn "CSV to JSON Conversion:"
printfn "%s\n" jsonData

// Markdown to HTML conversion (simplified)
let markdownText = """
# Main Header
This is **bold text** and *italic text*.
Here's a [link](https://example.com) and `inline code`.

## Sub Header  
* List item 1
* List item 2
* List item 3

Here's a code block:
```
function example() {
    return "Hello World";
}
```
"""

let mdReplacements = [
    (Regex(@"^# (.+)$", RegexOptions.Multiline), "<h1>$1</h1>")
    (Regex(@"^## (.+)$", RegexOptions.Multiline), "<h2>$1</h2>")
    (Regex(@"\*\*([^*]+)\*\*", RegexOptions.Compiled), "<strong>$1</strong>")
    (Regex(@"\*([^*]+)\*", RegexOptions.Compiled), "<em>$1</em>")
    (Regex(@"`([^`]+)`", RegexOptions.Compiled), "<code>$1</code>")
    (Regex(@"\[([^\]]+)\]\(([^)]+)\)", RegexOptions.Compiled), "<a href=\"$2\">$1</a>")
    (Regex(@"^\* (.+)$", RegexOptions.Multiline), "<li>$1</li>")
]

let mutable htmlText = markdownText
mdReplacements |> List.iter (fun (pattern, replacement) ->
    htmlText <- pattern.Replace(htmlText, replacement))

printfn "Markdown to HTML Conversion:"
printfn "%s" htmlText
```

Advanced replacement techniques combine backreferences, custom  
evaluator functions, and conditional logic for sophisticated  
text transformations. Template systems, data format conversions,  
and markup processing demonstrate practical applications of  
dynamic regex replacement operations.  

## Testing and debugging regex patterns

Regex testing and debugging techniques ensure pattern correctness  
through systematic validation, performance testing, and  
edge case analysis for reliable pattern matching in  
production applications.  

```f#
open System.Text.RegularExpressions
open System.Diagnostics

// Comprehensive test framework for regex patterns
type RegexTestCase = {
    Input: string
    ShouldMatch: bool
    ExpectedGroups: (string * string) list option
    Description: string
}

type RegexTester(pattern: string, options: RegexOptions) =
    let regex = Regex(pattern, options)
    
    member this.Pattern = pattern
    member this.Regex = regex
    
    member this.RunTests(testCases: RegexTestCase list) =
        let mutable passCount = 0
        let mutable failCount = 0
        
        printfn "Testing pattern: %s\n" pattern
        
        testCases |> List.iteri (fun i testCase ->
            let match' = regex.Match(testCase.Input)
            let actualMatch = match'.Success
            let testPassed = actualMatch = testCase.ShouldMatch
            
            if testPassed then
                passCount <- passCount + 1
                printfn "✓ Test %d: %s" (i + 1) testCase.Description
            else
                failCount <- failCount + 1
                printfn "✗ Test %d: %s" (i + 1) testCase.Description
                printfn "  Input: %s" testCase.Input
                printfn "  Expected match: %b, Actual match: %b" testCase.ShouldMatch actualMatch
            
            // Test expected groups if provided
            match testCase.ExpectedGroups with
            | Some expectedGroups when testCase.ShouldMatch && actualMatch ->
                expectedGroups |> List.iter (fun (groupName, expectedValue) ->
                    let actualValue = match'.Groups.[groupName].Value
                    if actualValue = expectedValue then
                        printfn "  ✓ Group '%s': %s" groupName actualValue
                    else
                        printfn "  ✗ Group '%s': Expected '%s', Got '%s'" groupName expectedValue actualValue)
            | _ -> ()
            
            printfn "")
        
        printfn "Test Summary: %d passed, %d failed\n" passCount failCount
        (passCount, failCount)

// Test email validation pattern
let emailTester = RegexTester(@"^(?<local>[a-zA-Z0-9._%+-]+)@(?<domain>[a-zA-Z0-9.-]+)\.(?<tld>[a-zA-Z]{2,})$", RegexOptions.Compiled)

let emailTests = [
    { Input = "user@example.com"; ShouldMatch = true; ExpectedGroups = Some [("local", "user"); ("domain", "example"); ("tld", "com")]; Description = "Standard email" }
    { Input = "test.user+tag@subdomain.example.org"; ShouldMatch = true; ExpectedGroups = Some [("local", "test.user+tag"); ("domain", "subdomain.example"); ("tld", "org")]; Description = "Complex email with subdomain" }
    { Input = "invalid.email"; ShouldMatch = false; ExpectedGroups = None; Description = "Missing @ symbol" }
    { Input = "user@"; ShouldMatch = false; ExpectedGroups = None; Description = "Missing domain" }
    { Input = "@domain.com"; ShouldMatch = false; ExpectedGroups = None; Description = "Missing local part" }
    { Input = "user@domain"; ShouldMatch = false; ExpectedGroups = None; Description = "Missing TLD" }
    { Input = "user@domain.c"; ShouldMatch = false; ExpectedGroups = None; Description = "TLD too short" }
    { Input = "user name@domain.com"; ShouldMatch = false; ExpectedGroups = None; Description = "Space in local part" }
]

let (emailPassed, emailFailed) = emailTester.RunTests(emailTests)

// Test phone number pattern with multiple formats
let phoneTester = RegexTester(@"^(?<country>\+?1[-.\s]?)?(?<area>\(?(\d{3})\)?)[-.\s]?(?<exchange>\d{3})[-.\s]?(?<number>\d{4})$", RegexOptions.Compiled)

let phoneTests = [
    { Input = "555-123-4567"; ShouldMatch = true; ExpectedGroups = Some [("area", "555"); ("exchange", "123"); ("number", "4567")]; Description = "Standard format" }
    { Input = "(555) 123-4567"; ShouldMatch = true; ExpectedGroups = Some [("area", "(555)"); ("exchange", "123"); ("number", "4567")]; Description = "Parentheses format" }
    { Input = "+1-555-123-4567"; ShouldMatch = true; ExpectedGroups = Some [("country", "+1-"); ("exchange", "123"); ("number", "4567")]; Description = "International format" }
    { Input = "5551234567"; ShouldMatch = true; ExpectedGroups = Some [("area", "555"); ("exchange", "123"); ("number", "4567")]; Description = "No separators" }
    { Input = "123-45-67"; ShouldMatch = false; ExpectedGroups = None; Description = "Too short" }
    { Input = "555-123-456A"; ShouldMatch = false; ExpectedGroups = None; Description = "Contains letter" }
    { Input = "1-555-123-4567-890"; ShouldMatch = false; ExpectedGroups = None; Description = "Too long" }
]

let (phonePassed, phoneFailed) = phoneTester.RunTests(phoneTests)

// Performance testing framework
let performanceTest (regex: Regex) (testInputs: string array) (iterations: int) =
    let stopwatch = Stopwatch.StartNew()
    let mutable totalMatches = 0
    
    for _ in 1..iterations do
        for input in testInputs do
            if regex.IsMatch(input) then
                totalMatches <- totalMatches + 1
    
    stopwatch.Stop()
    (totalMatches, stopwatch.ElapsedMilliseconds)

let performanceInputs = [|
    "user@example.com"
    "invalid.email"
    "test@domain.org"
    "not an email at all"
    "another.user@company.net"
|]

printfn "Performance Comparison:"
let (matches1, time1) = performanceTest (Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}")) performanceInputs 10000
let (matches2, time2) = performanceTest (Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.Compiled)) performanceInputs 10000

printfn "Non-compiled: %d matches in %d ms" matches1 time1
printfn "Compiled: %d matches in %d ms" matches2 time2
printfn "Performance improvement: %.1fx\n" (float time1 / float time2)

// Edge case testing
let edgeCaseTester = RegexTester(@"^[\w.-]+@[\w.-]+\.\w{2,}$", RegexOptions.Compiled)

let edgeCaseTests = [
    { Input = ""; ShouldMatch = false; ExpectedGroups = None; Description = "Empty string" }
    { Input = " "; ShouldMatch = false; ExpectedGroups = None; Description = "Single space" }
    { Input = String.replicate 1000 "a" + "@example.com"; ShouldMatch = true; ExpectedGroups = None; Description = "Very long local part" }
    { Input = "user@" + String.replicate 100 "domain."; ShouldMatch = false; ExpectedGroups = None; Description = "Very long domain" }
    { Input = "üser@example.com"; ShouldMatch = false; ExpectedGroups = None; Description = "Unicode characters" }
    { Input = "user@example.com\n"; ShouldMatch = false; ExpectedGroups = None; Description = "Trailing newline" }
    { Input = "user@example.com "; ShouldMatch = false; ExpectedGroups = None; Description = "Trailing space" }
]

printfn "Edge Case Testing:"
let (edgePassed, edgeFailed) = edgeCaseTester.RunTests(edgeCaseTests)

// Debugging helper functions
let debugPattern (pattern: string) (input: string) =
    let regex = Regex(pattern, RegexOptions.Compiled)
    let match' = regex.Match(input)
    
    printfn "Debug Pattern: %s" pattern
    printfn "Input: %s" input
    printfn "Match: %b" match'.Success
    
    if match'.Success then
        printfn "Full match: %s (Index: %d, Length: %d)" match'.Value match'.Index match'.Length
        
        match'.Groups |> Seq.iteri (fun i group ->
            if i > 0 then // Skip the full match group (index 0)
                printfn "  Group %d: '%s' (Index: %d, Length: %d)" i group.Value group.Index group.Length)
        
        // Named groups
        regex.GetGroupNames() 
        |> Array.filter (fun name -> name <> "0") // Skip the full match group
        |> Array.iter (fun name ->
            let group = match'.Groups.[name]
            if group.Success then
                printfn "  Named group '%s': '%s' (Index: %d, Length: %d)" name group.Value group.Index group.Length)
    
    printfn ""

// Debug examples
printfn "Pattern Debugging Examples:"
debugPattern @"(\d{4})-(\d{2})-(\d{2})" "2023-12-01"
debugPattern @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})" "2023-12-01"
debugPattern @"(\w+)@(\w+\.\w+)" "user@example.com"

// Common pattern issues and fixes
printfn "Common Pattern Issues and Solutions:"
printfn "1. Catastrophic backtracking: Use atomic groups or possessive quantifiers"
printfn "2. Unintended matches: Use anchors (^$) for full string matching"  
printfn "3. Performance issues: Compile frequently used patterns"
printfn "4. Unicode issues: Use \\p{L} instead of [a-zA-Z] for international text"
printfn "5. Escaping issues: Use verbatim strings (@\"\") for regex patterns in F#"
printfn "6. Group capture overhead: Use non-capturing groups (?:) when possible"
printfn "7. Case sensitivity: Consider RegexOptions.IgnoreCase for flexible matching"
```

Regex testing frameworks validate pattern correctness through  
systematic test cases, performance benchmarks, and edge case  
analysis. Debugging techniques with group inspection and  
match details help identify and fix pattern issues for  
reliable production regex implementations.  

# Input & output


## Read file line by line

`File.ReadLines` returns `IEnumerable<string>`

```F#
open System.IO

let fname = "words.txt"
let lines = File.ReadLines(fname) 

lines |> Seq.iter (printfn "%s")
```

## Read user console input

```F#
open System

printf "Enter your name:"

let name = Console.ReadLine()

let msg =
    if String.IsNullOrWhiteSpace(name) then
        "wrong input"
    else
        $"Hello {name}!"

printfn $"{msg}"
```

## Write a line

```F#
open System.IO

let fileName = "data.txt"
use w = new StreamWriter(fileName)

w.WriteLine("a line")
```

## Write data

```F#
open System.IO

let writeData() =

  use sw = new StreamWriter("data.txt")

  fprintf sw "Today is a beautiful day\n"
  fprintf sw "We go swimming and fishing\n"


writeData()
```

## Get file names

```F#
open System.IO

let files = Directory.GetFiles(".")

printfn "%A" files

let names = files |> Array.map Path.GetFileName
printfn "%A" names
```

## Source directory

Write to source directory  

```F#
open System.IO

let writeToFile () = 

    let data = "an old falcon"
    File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "file.txt"), data)

writeToFile()
```

## List files

```F#
open System.IO

let listFiles (dirName:string) =

    let fdata = Directory.GetFiles dirName |> List.ofSeq

    fdata |> List.map (fun fileName -> 
        Path.GetFileName(fileName), FileInfo(fileName).Length)


let fdata = listFiles "/root/Documents"

for file in fdata do
    printfn "%s - %d bytes" (fst file) (snd file)
```

## word start with 

```F#
open System.IO
open System

let words = File.ReadAllLines("words.txt")

let w_words =
    words
    |> Seq.filter(fun word -> word.StartsWith("w"))

w_words |> Seq.iter (printfn "%s")
```

## WriteAllText

```F#
open System.IO 

File.WriteAllText("words.txt", "sky\nfalcon\nrock\nhawk\nocean\ncloud")
let msg = File.ReadAllText("words.txt")
printfn "%s\n" msg
```

## use vs using 

```F#
open System.IO

let readFile path = 
    use r = new StreamReader(new FileStream(path, FileMode.Open))
    printfn "%s" (r.ReadToEnd())

let readFile2 path = 
    using (new StreamReader(new FileStream(path, FileMode.Open)))  
        (fun r -> printfn "%s" (r.ReadToEnd()))

readFile "words.txt"
printfn "----------------------------"
readFile2 "words.txt"
```

The using function and the use binding are nearly equivalent ways to  
accomplish the same thing. The using keyword provides more control over when  
Dispose is called. When you use using, Dispose is called at the end of the  
function or lambda expression; when you use the use keyword, Dispose is  
called at the end of the containing code block. In general, you should prefer  
to use use instead of the using function.  
https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/dd233240(v=vs.100)?redirectedfrom=MSDN  

## Read nth line

```F#
open System.IO

let fname = "words.txt"
let data = File.ReadAllLines(fname)
let n = 1

printfn "%s" data[n] 
```

---

```F#
open System
open System.IO

[<EntryPoint>]
let main args =

    let n = Int32.Parse(args.[1]) - 1
    use r = new StreamReader(args.[0])

    let lines =
        Seq.unfold
            (fun (reader: StreamReader) ->
                if (reader.EndOfStream) then
                    None
                else
                    Some(reader.ReadLine(), reader))
            r

    let line = Seq.item n lines // Seq.nth throws an ArgumentException, if not not enough lines available
    
    Console.WriteLine(line)
    0
```

## Read files asynchronously

```F#
open System.IO

let readFilesTask path1 path2 =
    task {
        let! bytes1 = File.ReadAllBytesAsync path1
        let! bytes2 = File.ReadAllBytesAsync path2
        return Array.append bytes1 bytes2
    }

let printBytes data =

    let row data =
        data |> Array.iter (fun e -> printf "%d " e)
        printf "\n"

    let chunked = data |> Array.chunkBySize 10
    chunked |> (Array.map row)

let fname1 = "words.txt"
let fname2 = "words2.txt"

readFilesTask fname1 fname2
|> Async.AwaitTask
|> Async.RunSynchronously
|> printBytes
```

## Sort words by frequency  

```F#
open System
open System.IO
open System.Text.RegularExpressions

let fileName = "the-king-james-bible.txt"
let data = File.ReadAllText(fileName)

let dig = Regex(@"\d")
let rx = Regex("[a-z-A-Z']+")

let matches = rx.Matches(data)

let topTen =
    matches
    |> Seq.map (fun m -> m.Value)
    |> Seq.filter (dig.IsMatch >> not)
    |> Seq.countBy id
    |> Seq.sortByDescending snd
    |> Seq.take 10

topTen
|> Seq.iter (fun (e, n) -> Console.WriteLine($"{e}: {n}"))
```


## Read CSV data into User types

```F#
open System.IO

type User = {
    UserName : string
    Occupation: string
    Age: int
}


let output data =
    data 
    |> Seq.iter (fun e -> printfn "%A" e)

let parseLine (line:string) : User option =
    match line.Split(',') with
    | [| userName; occupation; age |] -> 
        Some { 
            UserName = userName
            Occupation = occupation
            Age = int age
        }
    | _ -> None

let readFile path = 
    seq { 
        use reader = new StreamReader(File.OpenRead(path))
        while not reader.EndOfStream do
            yield reader.ReadLine() 
    }

"users.csv" |> readFile 
            |> Seq.skip 1  
            |> Seq.map parseLine 
            |> Seq.choose id
            |> output
```

## Read CSV data II

```F#
open System.IO

type User = {
    UserName : string
    Occupation: string
    Age: int
}


let output data =
    data
    |> Seq.iter (fun e -> printfn "%A" e)

let parseLine (line:string) : User option =
    match line.Split(',') with
    | [| userName; occupation; age |] ->
        Some {
            UserName = userName
            Occupation = occupation
            Age = int age
        }
    | _ -> None

let parse (data:string seq) =
    data
    |> Seq.skip 1
    |> Seq.map parseLine
    |> Seq.choose id


type FileReader = string -> Result<string seq,exn>

let readFile : FileReader =
    fun path ->
        try
            seq { use reader = new StreamReader(File.OpenRead(path))
                  while not reader.EndOfStream do
                      yield reader.ReadLine() }
            |> Ok
        with
        | ex -> Error ex

let loadData (fileReader: FileReader) path =
    match path |> fileReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn "Error: %A" ex.Message

loadData readFile "users.csv"
```


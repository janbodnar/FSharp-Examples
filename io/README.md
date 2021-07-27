
**Read CSV data into User types**  
```
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

**Read CSV data II**  

```
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


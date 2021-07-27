
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


# Input & output

## Read user console input

```f#
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
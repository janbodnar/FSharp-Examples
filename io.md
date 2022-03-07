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

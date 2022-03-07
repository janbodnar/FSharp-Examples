# Arrays


```f#
open System

let vals = [| 1; 2; 3; 4; 5 |]
Console.WriteLine(Array.length vals)

let vals2 = [| 1 .. 100 |]
printfn "%A" vals2[10..20]

Console.WriteLine("-----------------------")

let vals3 = [|
    2
    3
    4
|]

vals3 |> Array.iter Console.WriteLine

Console.WriteLine("-----------------------")

let vals4 = Array.init 10 (fun x -> x + 10)
printfn "%A" vals4

Console.WriteLine("-----------------------")

let vals5 = [|4; 2; 1; -4; -1; 0; 7|]
printfn "%A" vals5

Array.sortInPlace vals5
printfn "%A" vals5
```

## Array comprehensions

using ranges & generators  

```f#
let vals = [| 1 .. 10 |]
printfn "%A" vals

let vals2 = [| 1 .. 3 .. 10 |]
printfn "%A" vals2

let vals3 =
    [| for e in 1 .. 5 do
           yield (e, e * e, e * e * e) |]

printfn "%A" vals3

let vals4 = [| for e in 1 .. 10 -> e * e |]
printfn "%A" vals4
```


## Array indexing & slicing

Since .NET 6, we can use `vals[i]` in addition to the old `vals.[i]`. F# does not yet support  
rear-based indexing & slicing: https://github.com/fsharp/fslang-design/blob/main/preview/FS-1076-from-the-end-slicing.md

```f#
open System 

let vals = [| 1; 3; 4; 6; 7; 8; 9|]

Console.WriteLine(vals.[0])
Console.WriteLine(vals.[1])
Console.WriteLine(vals.[2])

Console.WriteLine("-------------------")

// since .NET 6
Console.WriteLine(vals[0])
Console.WriteLine(vals[1])
Console.WriteLine(vals[2])

Console.WriteLine("-------------------")

// slices
printfn "%A" vals[0..6]
printfn "%A" vals[..6]
printfn "%A" vals[2..]

Console.WriteLine("-------------------")

Console.WriteLine(Array.last vals)
Console.WriteLine(Array.length vals)
```
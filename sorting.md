# Sorting 

## Ints and strings 

```F#
let nums = [ 7; 9; 3; -2; 8; 1; 0 ]

let words =
    [ "sky"
      "cloud"
      "atom"
      "brown"
      "den"
      "kite"
      "town" ]

List.sort nums |> printfn "%A" 
List.sortDescending nums |> printfn "%A" 
List.sort words |> printfn "%A" 
List.sortDescending words |> printfn "%A" 
```


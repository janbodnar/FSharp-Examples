# List comprehensions

List comprehensions allow us to build lists dynamically with ranges and generators.  
We can use (multiple) for loops and if conditions, let expressions and match patterns.  

## Ranges

```F#
let res = [ for e in 1 .. 100 -> e * e ]
printfn "%A" res
```

## Multiple for loops

```F#
let res2 =
    [ for r in 1..8 do
          for c in 1..8 do
              if r <> c then yield (r, c) ]

printfn "%A" res2
```

## Generators

```F#
let vals = [ -1; 0; 2; -2; 1; 3; 4; -6 ]

let pos =
    [ for e in vals do
          if e > 0 then yield e ]

printfn "%A" pos

printfn "---------------------------------"

[ for a in 1 .. 100 do
    if a % 3 = 0 && a % 5 = 0 then yield a] |> printfn "%A"

printfn "---------------------------------"

let vals3 =
    [ for x in 1 .. 3 do
          for y in 1 .. 10 -> x, y ]

printfn "%A" vals3
```

## yield/yield!

```F#
let res =
    [ for a in 1..5 do
          yield! [ a .. a + 3 ] ]

printfn "%A" res

let chars = [ 'a' .. 'z' ]

let res2 = [for e in chars do yield [e; e; e] ]
printfn "%A" res2
```

```F#
let words = [ "sky"; "cloud"; "park"; "rock"; "war" ]

let res = [ for e in words do yield e ]
printfn "%A" res

let res2 = [ for e in words do yield! e ]
printfn "%A" res2
```


## Match pattern

```F#
let res =
    [ for e in 1..100 do
          match e with
          | e when e % 3 = 0 -> yield "fizz"
          | e when e % 5 = 0 -> yield "buzz"
          | e when e % 15 = 0 -> yield "fizzbuzz"
          | _ -> yield (string e) ]

printfn "%A" res
```

## Let expressions 

```F#
let vals = [ 1; -2; -3; 4; 5 ]

[ for v in vals do
      let f = fun e -> e > 0
      if f(v) then yield v ] |> printfn "%A"
```



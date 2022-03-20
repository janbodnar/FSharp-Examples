# Loops

## for in

```F#
let vals = seq { 1..5 }

for e in vals do
    printfn "%d" e

printfn "--------------------"

let len = Seq.length (vals) - 1

for idx in 0..len do
    printfn "%d" (Seq.item idx vals)
```

## for to/downto

```F#
for e = 1 to 5 do
    printfn "%d" e

for e = 5 downto 1 do
    printfn "%d" e
```

## for with range

```F#
for e in 1..2..10 do
    printfn "%d" e

for e in 10..-2..0 do
    printfn "%d" e
```

## Nested for loops

```F#
for i in [1; 2; 3; 4; 5; 6; 7; 6; 5; 4; 3; 2; 1] do
    for _ in 1..i do
        printf "*"
    printf "\n"
```

## While loop

```F#
let vals = [ 1; 2; 3; 4; 5 ]

let mutable i = 0

while i < vals.Length do
    printfn "%d" val[i]
    i <- i + 1
```

# Conditions

# Single conditions

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 

if r < 0 then 
    printfn "negative value" 

if r = 0 then
    printfn "zero" 
```

## Multiple conditions

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 
elif r < 0 then 
    printfn "negative value" 
elif r = 0 then 
    printfn "zero" 
```

## else keyword

```F#
open System

let r = Random().Next(-5, 5)

if r > 0 then 
    printfn "positive value" 
elif r < 0 then 
    printfn "negative value" 
else 
    printfn "zero" 
```

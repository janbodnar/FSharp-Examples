# FSharp-Examples


F# is a functional language for .NET. Highly influenced by OCalm, first appeared  
in 2005.  

`$ dotnet new console -lang F# -o Simple`  
`$ cd Simple`  
`$ dotnet run`  

Turn off telemetry  

`export DOTNET_CLI_TELEMETRY_OPTOUT=1`  

Run F# scripts  

`$ dotnet fsi simple.fsx`  

Algorithm to calculate powers; F# script with  
#time directive  

```
// exponentiation
let rec power x n: bigint =
    if n = 0I then 1I
    else x * (power x (n-1I))

// more efficient algorithm
let rec power2 x n: bigint =
    if n = 0I then 1I
    //if n is even
    elif (n % 2I = 0I) then ((power2 x (n/2I)) * (power2 x (n/2I)))
     //if n is odd
    else x * (power2 x (n-1I))

#time
//printfn "%A" (power 1254I 290I)
printfn "%A" (power 1254I 29000I)
#time
```

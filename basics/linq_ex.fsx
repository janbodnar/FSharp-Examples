open System
open System.Linq

let data = [ 0 .. 10 ]

let res =
    data
        .Where(fun n -> n % 2 = 1)
        .Select(fun n -> n * 2)
        .ToList()

res.ForEach(fun n -> printfn "%i" n)

Console.WriteLine("----------------------")

// No need to use LINQ in F#; F# has built-in alternatives

// List functions

data
|> List.where (fun e -> e % 2 = 1)
|> List.map (fun e -> e * 2)
|> printfn "%A"

// query expressions

query {
    for n in data do
        where (n % 2 = 1)
        select (n * 2)
}
|> printfn "%A"

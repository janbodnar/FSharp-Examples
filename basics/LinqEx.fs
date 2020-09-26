open System.Linq
 
let data = [0..10]
let oddNums = data.Where(fun n -> n % 2 = 1).Select(fun n -> n * 2).ToList()
oddNums.ForEach(fun n -> printfn "%i" n)


// No need to use LINQ in F#; F# has built-in 
// tools 

// let data = [1..10]

// let oddvals = 
//     query {
//         for n in data do
//         where (n % 2 = 1) 
//         select (n * 2)
//     }

// printfn "%A" oddvals

// for e in oddvals do 
//     printfn "%i" e

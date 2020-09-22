open System.IO
open System

let words = File.ReadAllLines("words.txt")

let w_words =
    words
    // |> Seq.filter(fun word -> if word.StartsWith("w") then true else false)
    |> Seq.filter(fun word -> word.StartsWith("w"))

w_words |> Seq.iter (printfn "%s")

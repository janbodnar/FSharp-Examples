open System.IO

[<EntryPoint>]
let main argv =

    let fileName = "data.txt"
    use w = new StreamWriter(fileName)

    w.WriteLine("a line")

    0 

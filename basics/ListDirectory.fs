open System.IO

[<EntryPoint>]
let main argv =

    Directory.GetFiles("/home/janbodnar/Documents/prog/python", "*.py", 
                            SearchOption.AllDirectories) 
        |> Array.map Path.GetFileName 
        |> Array.iter (printfn "%s") 

    0



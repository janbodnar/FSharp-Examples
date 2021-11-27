open System.IO

let fileName = "/home/jano7/Documents/prog/python"

Directory.GetFiles(fileName, "*.py", SearchOption.AllDirectories)
|> Array.map Path.GetFileName
|> Array.iter (printfn "%s")

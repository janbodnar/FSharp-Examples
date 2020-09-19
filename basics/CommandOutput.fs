open System

[<EntryPoint>]
let main argv =

    let p = new System.Diagnostics.Process();
    p.StartInfo.FileName <- "ls"
    p.StartInfo.Arguments <- ("-l ..")
    p.StartInfo.RedirectStandardOutput <- true
    p.StartInfo.UseShellExecute <- false
    p.Start() |> ignore 

    printfn "%s" (p.StandardOutput.ReadToEnd())
    
    0 

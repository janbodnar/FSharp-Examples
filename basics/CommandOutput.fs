open System

[<EntryPoint>]
let main argv =

    let p = new System.Diagnostics.Process();
    p.StartInfo.FileName <- "ls"
    p.StartInfo.Arguments <- ("-l ..")
    p.StartInfo.RedirectStandardOutput <- true
    p.StartInfo.UseShellExecute <- false
    p.Start() |> ignore 

    printfn "result %A" (p.StandardOutput.ReadToEnd())
    printfn "done"

    0 

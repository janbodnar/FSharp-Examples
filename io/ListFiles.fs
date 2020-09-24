open System.IO

let listFiles (dirName:string) =

    let fdata = Directory.GetFiles dirName |> List.ofSeq

    fdata |> List.map (fun fileName -> 
        Path.GetFileName(fileName), FileInfo(fileName).Length)


let fdata = listFiles "/root/Documents"

for file in fdata do
    printfn "%s - %d bytes" (fst file) (snd file)

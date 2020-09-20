open System.IO 

File.WriteAllText("words.txt", "sky\nfalcon\nrock\nhawk\nocean\ncloud")
let msg = File.ReadAllText("words.txt")
printfn "%s\n" msg

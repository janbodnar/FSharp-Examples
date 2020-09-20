open System
open System.IO

let readFile path = 
    use r = new StreamReader(new FileStream(path, FileMode.Open))
    Console.WriteLine(r.ReadToEnd())

let readFile2 path = 
    using (new StreamReader(new FileStream(path, FileMode.Open)))  
        (fun r -> Console.WriteLine(r.ReadToEnd()))

readFile "words.txt"
readFile2 "words.txt"

// The using function and the use binding are nearly equivalent ways to
// accomplish the same thing. The using keyword provides more control over when
// Dispose is called. When you use using, Dispose is called at the end of the
// function or lambda expression; when you use the use keyword, Dispose is
// called at the end of the containing code block. In general, you should prefer
// to use use instead of the using function.
// https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/dd233240(v=vs.100)

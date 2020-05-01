let reader = new System.IO.StringReader("sky\ncloud\nwood\nforest")

//let nextLine   =  reader.ReadLine()  //wrong
let nextLine() =  reader.ReadLine()  //correct
//let nextLine   =  fun() -> reader.ReadLine()  //correct


printfn "%s" (nextLine())
printfn "%s" (nextLine())

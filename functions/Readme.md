This is a value, not a function  
    let hello =  
        printfn "Hello there"


This is a function

    let hello () =
        printfn "Hello there"

The following function definitions are the same. The first  
uses the lambda expression; the second is the conventional function definition.  

    let add = fun x y -> x + y
    let add2 x y = x + y

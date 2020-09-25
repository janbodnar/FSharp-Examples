let addTwoNumbers x y = x + y
let add7ToNumber = addTwoNumbers 7

printfn "%d" (add7ToNumber 5)

// Every function in F# has one parameter and returns one value. That value can
// be of type unit, designated by (), which is similar in concept to void in
// some other languages.
// When you have a function that appears to have more than one parameter, F#
// treats it as several functions, each with one parameter, that are then
// "curried" to come up with the result we want. 

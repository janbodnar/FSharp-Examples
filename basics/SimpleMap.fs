let vals = [1..5]

let double input = input |> List.map (fun x -> x * 2)
vals |> double |> printf "%A\n"


let user = {| FirstName = "John Doe"; Age = 33 |} // Anonymous record
let updated = {| user with Age = 34 |}

printf "%A\n" user
printf "%A\n" updated

let output = if true then "yes" else "no"
printf "%A\n" output

// F# works like maths - if something is on both sides 
// of the = it can often be ommited, giving an even terser syntax
let triple = List.map((*) 3)

vals |> triple |> printf "%A\n"

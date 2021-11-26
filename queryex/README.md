# F# query expressions 

```f#
open System

let vals = [| 1; 2; 3; 4; 5; 6|]
 
let res = query {
   for v in vals do
   where (v <> 3)
   select v
}

for e in res do
    Console.WriteLine(e)

Console.WriteLine(vals.GetType())
```

`where` operator  

```f#
open System

type User = {
    Name: string
    Occupation: string
}

let users = [
    { Name = "John Doe"; Occupation = "gardener" }
    { Name = "Roger Roe"; Occupation = "driver" }
    { Name = "Thomas Monroe"; Occupation = "trader" }
    { Name = "Gregory Smith"; Occupation = "teacher" }
    { Name = "Lucia Bellington"; Occupation = "teacher" }
]

let n = query {
    for user in users do
    select user
    count
}

Console.WriteLine(n)

let last = query {
    for user in users do
    last
}

Console.WriteLine(last)

Console.WriteLine("teachers:")

let teachers = query {
    for user in users do
    where (user.Occupation = "teacher")
    select user
}

teachers |> Seq.iter Console.WriteLine
```

`count`, `last`, `where` operators  

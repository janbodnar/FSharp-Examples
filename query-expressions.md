# F# query expressions 

```f#
open System

let vals = [| 1; 2; 3; 4; 5; 6|]

let lst = query {
    for e in vals do
    last
}

Console.WriteLine(lst)

let fst = query {
    for e in vals do
    head
}

Console.WriteLine(fst)

let n = query {
    for e in vals do
    nth 3
}

Console.WriteLine(n)
```

`first`, `last` operators  

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

```f#
open System

type User = {
    FirstName: string
    LastName: string
    Salary: int
}

let users = [
    { FirstName = "Robert"; LastName = "Novak"; Salary = 1770 }
    { FirstName = "John"; LastName = "Doe"; Salary = 1230 }
    { FirstName = "Lucy"; LastName = "Novak"; Salary = 670 }
    { FirstName = "Ben"; LastName = "Walter"; Salary = 2050 }
    { FirstName = "Robin"; LastName = "Brown"; Salary = 2300 }
    { FirstName = "Amy"; LastName = "Doe"; Salary = 1250 }
    { FirstName = "Joe"; LastName = "Draker"; Salary = 1190 }
    { FirstName = "Janet"; LastName = "Doe"; Salary = 980 }
    { FirstName = "Peter"; LastName = "Novak"; Salary = 990 }
    { FirstName = "Albert"; LastName = "Novak"; Salary = 1930 }
]

let sorted = query {
    for user in users do
    sortBy user.LastName
    thenBy user.Salary
    select user
}

sorted |> Seq.iter Console.WriteLine
```

`sortBy`, `thenBy` operators  


```F#
open System.Linq

type Revenue =
    { Id: int
      Quarter: string
      Amount: int }

let revenues = [
    { Id = 1; Quarter = "Q1"; Amount = 2340 };
    { Id = 2; Quarter = "Q1"; Amount = 1200 };
    { Id = 3; Quarter = "Q1"; Amount = 980 };
    { Id = 4; Quarter = "Q2"; Amount = 340 };
    { Id = 5; Quarter = "Q2"; Amount = 780 };
    { Id = 6; Quarter = "Q3"; Amount = 2010 };
    { Id = 7; Quarter = "Q3"; Amount = 3370 };
    { Id = 8; Quarter = "Q4"; Amount = 540 }
]

query {
    for revenue in revenues do
        groupBy revenue.Quarter into g
        where (g.Count() = 2)

        select
            {| Quarter = g.Key
               Total = g.Sum(fun c -> c.Amount) |}
}
|> Seq.iter (fun e -> printfn "%A" e)
```

GroupBy and  sum  

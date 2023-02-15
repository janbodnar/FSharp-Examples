# Records

A record is an aggregate of named values. It is immutable by default.  
Records can contain members. It is possible to create mutable values  
with `mutable` keyword.

## Simple example

A record is defined with the `type` keyword. The values are specified between  
the `{ }` brackets.  

```F#
type User =
    { FirstName: string; LastName: string; Occupation: string; Salary: int }

let users =
    [ { FirstName = "Robert"
        LastName = "Novak"
        Occupation = "teacher"
        Salary = 1770 }
      { FirstName = "John"
        LastName = "Doe"
        Occupation = "gardener"
        Salary = 1230 }
      { FirstName = "Lucy"
        LastName = "Novak"
        Occupation = "accountant"
        Salary = 670 } ]

users
|> List.iter (fun user -> (printfn "%A" user))

users |> List.iter (printfn "%A")
```

## Optional semicolons 

The semicolons between the values of a record are optional.  

```F#
type User =
    { FirstName: string
      LastName: string
      Occupation: string
      Salary: int }

let users =
    [ { FirstName = "Robert"
        LastName = "Novak"
        Occupation = "teacher"
        Salary = 1770 }
      { FirstName = "John"
        LastName = "Doe"
        Occupation = "gardener"
        Salary = 1230 }
      { FirstName = "Lucy"
        LastName = "Novak"
        Occupation = "accountant"
        Salary = 670 } ]

users
|> List.iter (fun user -> (printfn "%A" user))

users |> List.iter (printfn "%A")
```

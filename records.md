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

## The with keyword

New records can be derived from existing records using `with`.  

```F#
type User = { name: string; occupation: string }

let u1 =
    { name = "John Doe"
      occupation = "gardener" }

printfn "%A" u1

let u2 = { u1 with name = "Peter Smith"}
printfn "%A" u2
```

## Deconstructing

Deconstructing is unpacking types into single pieces.  

```F#
type User = { name: string; Occupation: string }

let u1 =
    { name = "John Doe"
      Occupation = "gardener" }

let { name = n1; Occupation = o1 } = u1
printfn "%s %s" n1 o1

let { name = _; Occupation = o2 } = u1
printfn "%s" o2

let { name = n2 } = u1
printfn "%s" n2
```

## Equality

Records have structural equality.

```F#
type User =
    { Name: string
      Occupation: string }

let u1 =
    { Name = "John Doe"
      Occupation = "gardener" }

let u2 =
    { Name = "Roger Roe"
      Occupation = "driver" }

printfn "%A" (u1 = u2)
```


## Record output 

The `%A` specifier is used for pretty-printing tuples, records and union types.
The `%O` is used for other objects, using `ToString`.  

```F#
type User =
    { Name: string
      Occupation: string }
    override this.ToString() =
        sprintf "%s %s" this.Name this.Occupation

let u1 =
    { Name = "John Doe"
      Occupation = "gardener" }

let u2 =
    { Name = "Roger Roe"
      Occupation = "driver" }

printfn "%A" u1
printfn "%O" u2
```

## Members

We can define members with `member`.  

```F#
type User =
  { Name: string
    Occupation: string }

    member this.Info() =
        $"{this.Name} is a {this.Occupation}"


let u1 = { Name= "John Doe"; Occupation="gardener" }
let u2 = { Name= "Roger Roe"; Occupation="driver" }

printfn "%s" (u1.Info())
printfn "%s" (u2.Info())
```






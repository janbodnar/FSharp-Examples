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

## Order of fields

The order of the fields is not relevant. F# recognizes records by the   
name and type of its fields.  

```F#
type User = { Name: string; Occupation: string }

let u1 =
    { Name = "John Doe"
      Occupation = "gardener" }

let u2 =
    { Occupation = "driver"
      Name = "Roger Roe" }

printfn "%A" u1
printfn "%A" u2
```

## Accessing record fields

The fields of a record are accessed using the dot operator.  

```F#
type User = { Name: string; Occupation: string }

let u =
    { Name = "John Doe"
      Occupation = "gardener" }

printfn "%s" u.Name
printfn "%s" u.Occupation
```



## The with keyword

New records can be derived from existing records using `with`.  

```F#
type User = { Name: string; Occupation: string }

let u1 =
    { Name = "John Doe"
      Occupation = "gardener" }

printfn "%A" u1

let u2 = { u1 with Name = "Peter Smith"}
printfn "%A" u2
```

## Deconstructing

Deconstructing is unpacking types into single pieces.  

```F#
type User = { Name: string; Occupation: string }

let u1 =
    { Name = "John Doe"
      Occupation = "gardener" }

let { Name = n1; Occupation = o1 } = u1
printfn "%s %s" n1 o1

let { Name = _; Occupation = o2 } = u1
printfn "%s" o2

let { Name = n2 } = u1
printfn "%s" n2
```

## Nesting records

We can nest a record inside another record with `and`.  

```F#
type User =
    { Name: string
      Occupation: string
      Address: Address }

and Address = { Line1: string; Line2: string }


let u1 =
    { Name = "John Doe"
      Occupation = "gardener"
      Address =
        { Line1 = "Address 1"
          Line2 = "Address 2" } }

printfn "%A" u1

let u2 =
    { Name = "Roger Doe"
      Occupation = "driver"
      Address =
        { Line1 = "Address 1"
          Line2 = "Address 2" } }

printfn "%A" u2
```
---

Multiple nested records.  

```F#
type User =
    { Name: string
      Occupation: string
      Address: Address
      Colours: Colours }

and Address = { Line1: string; Line2: string }
and Colours = { Col1: string; Col2: string }

let u1 =
    { Name = "John Doe"
      Occupation = "gardener"
      Address =
        { Line1 = "Address 1"
          Line2 = "Address 2" }
      Colours = { Col1 = "red"; Col2 = "blue" } }

printfn "%A" u1

let u2 =
    { Name = "Roger Doe"
      Occupation = "driver"
      Address =
        { Line1 = "Address 1"
          Line2 = "Address 2" }
      Colours = { Col1 = "red"; Col2 = "green" } }

printfn "%A" u2
```

## Equality

Records have structural equality. Structural equality is when two objects contain the same values.  

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

## Pattern matching 

Records can be used with pattern matching. 

```F#
type User =
    { FirstName: string
      LastName: string
      Occupation: string }

let users =
    [ { FirstName = "John"
        LastName = "Doe"
        Occupation = "gardener" }
      { FirstName = "Jane"
        LastName = "Doe"
        Occupation = "teacher" }
      { FirstName = "Roger"
        LastName = "Roe"
        Occupation = "driver" } ]

for user in users do
    match user with
    | { LastName = "Doe" } -> printfn "%A" user
    | _ -> ()
```
The example prints all Does.  






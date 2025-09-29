# F# Query Expressions - Comprehensive Guide

Query expressions in F# provide SQL-like syntax for querying and transforming  
data collections. They offer a declarative approach to data manipulation,  
making complex filtering, sorting, grouping, and projection operations  
readable and maintainable. Query expressions compile to LINQ operations  
under the hood, ensuring excellent performance and seamless interoperability  
with .NET libraries.

F# query expressions support various data sources including arrays, lists,  
sequences, and any IEnumerable<T> collection. They provide a unified syntax  
for working with data regardless of the underlying collection type. The  
query computational expression translates F# query syntax into efficient  
LINQ method calls, enabling powerful data processing capabilities.

Query expressions excel at expressing complex data transformations that  
would otherwise require multiple function compositions or nested loops.  
They support projection, filtering, aggregation, grouping, joining, and  
ordering operations. The syntax closely resembles SQL, making it familiar  
to developers with database experience while maintaining F#'s functional  
programming principles.

## Basic Element Access

Accessing individual elements from collections using head, last, and nth.  

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

Query expressions provide convenient access to collection elements without  
explicit indexing. The `head` operator retrieves the first element, `last`  
gets the final element, and `nth` accesses elements by zero-based position.  
These operators eliminate manual array bounds checking and provide clean,  
readable syntax for element access operations.

## Filtering with Where

Filtering collections based on conditional predicates using where operator.  

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

The `where` operator filters collections based on boolean predicates,  
returning only elements that satisfy the specified condition. Combined  
with `select`, it enables powerful data filtering and transformation  
operations. The query expression syntax compiles to efficient LINQ  
operations, maintaining good performance for large datasets.

## Working with Records

Demonstrating query operations on structured data using record types.  

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

Query expressions work seamlessly with F# record types, allowing property  
access and filtering based on record fields. The `count` operator returns  
the number of elements in the query result. Multiple query operations can  
be combined to create sophisticated data processing pipelines while  
maintaining readable and declarative syntax.

## Sorting Operations

Sorting data using sortBy and thenBy operators for multi-level ordering.  

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

The `sortBy` operator provides primary sorting criteria, while `thenBy`  
enables secondary sorting for equal primary values. This creates  
hierarchical sorting that maintains stable ordering. The operators  
accept lambda expressions for custom sorting logic and work with any  
comparable data types.

## Grouping and Aggregation

Grouping data and performing aggregate operations like Sum and Count.  

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

The `groupBy` operator partitions data into groups based on key values,  
enabling aggregate calculations like Sum, Count, Average, and others.  
The `into g` syntax creates group objects that provide access to both  
the key and the grouped elements. Anonymous records provide clean  
syntax for structured query results.

## Select Transformations

Projecting data into different shapes using select operations.  

```f#
open System

type Employee = {
    Id: int
    Name: string
    Department: string
    Salary: decimal
    StartDate: System.DateTime
}

let employees = [
    { Id = 1; Name = "Alice Johnson"; Department = "IT"; Salary = 75000m; StartDate = DateTime(2020, 1, 15) }
    { Id = 2; Name = "Bob Smith"; Department = "HR"; Salary = 65000m; StartDate = DateTime(2019, 3, 22) }
    { Id = 3; Name = "Carol Davis"; Department = "IT"; Salary = 80000m; StartDate = DateTime(2021, 7, 10) }
    { Id = 4; Name = "David Wilson"; Department = "Finance"; Salary = 70000m; StartDate = DateTime(2018, 11, 5) }
]

let employeeNames = query {
    for emp in employees do
    select emp.Name
}

let departmentSummary = query {
    for emp in employees do
    select (emp.Department, emp.Name, emp.Salary)
}

employeeNames |> Seq.iter (printfn "Employee: %s")
Console.WriteLine("Department Summary:")
departmentSummary |> Seq.iter (fun (dept, name, salary) -> 
    printfn "%s - %s: $%.2f" dept name salary)
```

Select operations transform query results by projecting data into new  
shapes. Simple projections extract specific fields, while complex  
projections can create tuples, anonymous records, or computed values.  
This enables data reshaping without modifying original collections.

## Distinct Values

Removing duplicate elements from query results using distinct operator.  

```f#
open System

let numbers = [| 1; 2; 2; 3; 3; 3; 4; 4; 5 |]

let uniqueNumbers = query {
    for n in numbers do
    select n
    distinct
}

let uniqueDepartments = query {
    for emp in employees do
    select emp.Department
    distinct
}

Console.WriteLine("Unique numbers:")
uniqueNumbers |> Seq.iter (printfn "%d")

Console.WriteLine("Unique departments:")
uniqueDepartments |> Seq.iter (printfn "%s")
```

The `distinct` operator eliminates duplicate values from query results  
based on value equality. It works with any comparable type and maintains  
the original order of first occurrences. Distinct is particularly useful  
when extracting unique values from collections or query projections.

## Min and Max Operations

Finding minimum and maximum values using aggregation operators.  

```f#
open System

let scores = [| 85; 92; 78; 96; 88; 79; 94; 87 |]

let minScore = query {
    for score in scores do
    minBy score
}

let maxScore = query {
    for score in scores do
    maxBy score
}

let minSalaryEmployee = query {
    for emp in employees do
    minBy emp.Salary
}

let maxSalaryEmployee = query {
    for emp in employees do
    maxBy emp.Salary
}

Console.WriteLine($"Min score: {minScore}")
Console.WriteLine($"Max score: {maxScore}")
Console.WriteLine($"Lowest paid: {minSalaryEmployee.Name} - ${minSalaryEmployee.Salary}")
Console.WriteLine($"Highest paid: {maxSalaryEmployee.Name} - ${maxSalaryEmployee.Salary}")
```

The `minBy` and `maxBy` operators find elements with minimum and maximum  
values based on specified criteria. They return the actual elements rather  
than just the min/max values, enabling access to complete records.  
These operators work with any comparable selector expression.

## Average and Sum Calculations

Computing statistical aggregates using sum and average operations.  

```f#
open System

let salesData = [
    { Id = 1; Quarter = "Q1"; Amount = 15000 }
    { Id = 2; Quarter = "Q1"; Amount = 18000 }
    { Id = 3; Quarter = "Q2"; Amount = 22000 }
    { Id = 4; Quarter = "Q2"; Amount = 19000 }
    { Id = 5; Quarter = "Q3"; Amount = 25000 }
    { Id = 6; Quarter = "Q3"; Amount = 21000 }
]

let totalSales = query {
    for sale in salesData do
    sumBy sale.Amount
}

let averageSales = query {
    for sale in salesData do
    averageBy (float sale.Amount)
}

let averageSalary = query {
    for emp in employees do
    averageBy (float emp.Salary)
}

Console.WriteLine($"Total sales: ${totalSales}")
Console.WriteLine($"Average sales: ${averageSales:F2}")
Console.WriteLine($"Average salary: ${averageSalary:F2}")
```

Sum and average operations compute aggregate statistics across query  
results. `sumBy` totals numeric values, while `averageBy` calculates  
means. Both operators accept selector expressions for computing  
aggregates over specific fields or derived values.

## Contains and Exists Checks  

Testing for element presence using contains and exists operations.  

```f#
open System

let targetDepartments = ["IT"; "Engineering"]
let highSalaryThreshold = 75000m

let hasITEmployees = query {
    for emp in employees do
    exists (emp.Department = "IT")
}

let hasHighEarners = query {
    for emp in employees do  
    exists (emp.Salary > highSalaryThreshold)
}

let containsSpecificId = query {
    for emp in employees do
    select emp.Id
    contains 3
}

Console.WriteLine($"Has IT employees: {hasITEmployees}")
Console.WriteLine($"Has high earners: {hasHighEarners}")
Console.WriteLine($"Contains employee ID 3: {containsSpecificId}")
```

The `exists` operator tests whether any elements satisfy a predicate,  
returning a boolean result. The `contains` operator checks for specific  
value presence in query results. Both operations provide efficient  
short-circuiting evaluation, stopping at the first matching element.

## Take and Skip Operations

Limiting query results using take and skip for pagination.  

```f#
open System

let numbers = [| 1..20 |]

let firstFive = query {
    for n in numbers do
    take 5
}

let skipFirstFive = query {
    for n in numbers do
    skip 5
    take 5
}

let topSalariedEmployees = query {
    for emp in employees do
    sortByDescending emp.Salary
    take 2
}

Console.WriteLine("First five numbers:")
firstFive |> Seq.iter (printfn "%d")

Console.WriteLine("Numbers 6-10:")
skipFirstFive |> Seq.iter (printfn "%d")

Console.WriteLine("Top 2 salaries:")
topSalariedEmployees |> Seq.iter (fun emp -> 
    printfn "%s: $%.2f" emp.Name emp.Salary)
```

Take and skip operations enable result pagination and limiting. `take`  
returns the first n elements, while `skip` bypasses the first n elements.  
Combined with sorting, these operators implement common pagination  
patterns for web applications and data processing scenarios.

## Where with Complex Predicates

Advanced filtering using compound conditions and nested predicates.  

```f#
open System

let products = [
    {| Id = 1; Name = "Laptop"; Price = 999.99m; Category = "Electronics"; InStock = true; Rating = 4.5 |}
    {| Id = 2; Name = "Book"; Price = 19.99m; Category = "Education"; InStock = true; Rating = 4.2 |}
    {| Id = 3; Name = "Phone"; Price = 699.99m; Category = "Electronics"; InStock = false; Rating = 4.7 |}
    {| Id = 4; Name = "Desk"; Price = 299.99m; Category = "Furniture"; InStock = true; Rating = 4.0 |}
    {| Id = 5; Name = "Monitor"; Price = 249.99m; Category = "Electronics"; InStock = true; Rating = 4.3 |}
]

let expensiveElectronics = query {
    for p in products do
    where (p.Category = "Electronics" && p.Price > 500m && p.InStock)
    select p
}

let highlyRatedAffordable = query {
    for p in products do  
    where (p.Rating >= 4.2 && p.Price < 300m)
    select (p.Name, p.Price, p.Rating)
}

Console.WriteLine("Expensive Electronics in Stock:")
expensiveElectronics |> Seq.iter (fun p -> 
    printfn "%s: $%.2f" p.Name p.Price)

Console.WriteLine("Highly Rated Affordable Items:")
highlyRatedAffordable |> Seq.iter (fun (name, price, rating) ->
    printfn "%s: $%.2f (Rating: %.1f)" name price rating)
```

Complex where predicates combine multiple conditions using logical  
operators. Boolean expressions can reference multiple properties and  
include mathematical comparisons. This enables sophisticated filtering  
logic that would require multiple filter steps in other approaches.

## Join Operations

Combining data from multiple collections using inner join.  

```f#
open System

type Department = {
    Id: int
    Name: string
    Budget: decimal
}

let departments = [
    { Id = 1; Name = "IT"; Budget = 500000m }
    { Id = 2; Name = "HR"; Budget = 200000m }
    { Id = 3; Name = "Finance"; Budget = 300000m }
    { Id = 4; Name = "Marketing"; Budget = 250000m }
]

type Employee2 = {
    Id: int
    Name: string
    DepartmentId: int
    Salary: decimal
}

let employees2 = [
    { Id = 1; Name = "Alice"; DepartmentId = 1; Salary = 75000m }
    { Id = 2; Name = "Bob"; DepartmentId = 2; Salary = 65000m }
    { Id = 3; Name = "Carol"; DepartmentId = 1; Salary = 80000m }
    { Id = 4; Name = "David"; DepartmentId = 3; Salary = 70000m }
]

let employeeDepartments = query {
    for emp in employees2 do
    join dept in departments on (emp.DepartmentId = dept.Id)
    select (emp.Name, dept.Name, emp.Salary, dept.Budget)
}

Console.WriteLine("Employee-Department Information:")
employeeDepartments |> Seq.iter (fun (empName, deptName, salary, budget) ->
    printfn "%s works in %s (Salary: $%.0f, Budget: $%.0f)" empName deptName salary budget)
```

Join operations combine data from multiple collections based on matching  
keys. The `join` keyword links collections using equality predicates,  
creating Cartesian products of matching elements. Joins enable relational  
data processing patterns familiar from SQL database operations.

## Group Join Operations

Performing group joins to create hierarchical data structures.  

```f#
open System

let departmentEmployees = query {
    for dept in departments do
    groupJoin emp in employees2 on (dept.Id = emp.DepartmentId) into empGroup
    select (dept.Name, dept.Budget, empGroup)
}

Console.WriteLine("Departments with Employee Groups:")
departmentEmployees |> Seq.iter (fun (deptName, budget, employees) ->
    printfn "\n%s (Budget: $%.0f):" deptName budget
    employees |> Seq.iter (fun emp -> 
        printfn "  - %s: $%.0f" emp.Name emp.Salary))
```

Group joins create hierarchical relationships by grouping joined elements.  
The `groupJoin` operation produces groups of related elements rather than  
flat Cartesian products. This enables master-detail data structures  
and hierarchical query results without manual grouping steps.

## Left Outer Join

Implementing left outer joins to include unmatched elements.  

```f#
open System

let allDepartments = [
    { Id = 1; Name = "IT"; Budget = 500000m }
    { Id = 2; Name = "HR"; Budget = 200000m }
    { Id = 3; Name = "Finance"; Budget = 300000m }
    { Id = 4; Name = "Marketing"; Budget = 250000m }
    { Id = 5; Name = "Legal"; Budget = 150000m }
]

let leftOuterJoin = query {
    for dept in allDepartments do
    leftOuterJoin emp in employees2 on (dept.Id = emp.DepartmentId) into empGroup
    for emp in empGroup.DefaultIfEmpty() do
    select (dept.Name, emp)
}

Console.WriteLine("All Departments with Employees (including empty):")
leftOuterJoin |> Seq.iter (fun (deptName, emp) ->
    match emp with
    | null -> printfn "%s: No employees" deptName
    | e -> printfn "%s: %s ($%.0f)" deptName e.Name e.Salary)
```

Left outer joins include all elements from the left collection regardless  
of matches in the right collection. The `leftOuterJoin` operation with  
`DefaultIfEmpty()` handles unmatched elements by providing null values.  
This preserves complete datasets when relationships are optional.

## Multiple Sorting Criteria

Advanced sorting with multiple thenBy operations for complex ordering.  

```f#
open System

type Student = {
    Name: string
    Grade: char
    Score: int
    Subject: string
    Age: int
}

let students = [
    { Name = "Alice"; Grade = 'A'; Score = 95; Subject = "Math"; Age = 20 }
    { Name = "Bob"; Grade = 'B'; Score = 87; Subject = "Math"; Age = 19 }
    { Name = "Carol"; Grade = 'A'; Score = 92; Subject = "Science"; Age = 21 }
    { Name = "David"; Grade = 'B'; Score = 89; Subject = "Math"; Age = 20 }
    { Name = "Eve"; Grade = 'A'; Score = 96; Subject = "Science"; Age = 19 }
    { Name = "Frank"; Grade = 'C'; Score = 78; Subject = "Math"; Age = 22 }
]

let complexSort = query {
    for student in students do
    sortBy student.Grade
    thenByDescending student.Score
    thenBy student.Age
    thenBy student.Name
    select student
}

Console.WriteLine("Students sorted by Grade (asc), Score (desc), Age (asc), Name (asc):")
complexSort |> Seq.iter (fun s ->
    printfn "%c - %s: %d points, age %d" s.Grade s.Name s.Score s.Age)
```

Multiple `thenBy` operations create complex sorting hierarchies with  
fine-grained control over ordering. Each additional sort criterion  
applies to groups with equal values in previous criteria. This enables  
sophisticated data presentation and ranking scenarios.

## Nested Queries

Using subqueries and nested query expressions for complex operations.  

```f#
open System

let orders = [
    {| Id = 1; CustomerId = 1; Amount = 150.00m; Date = DateTime(2023, 1, 15) |}
    {| Id = 2; CustomerId = 2; Amount = 200.00m; Date = DateTime(2023, 1, 20) |}
    {| Id = 3; CustomerId = 1; Amount = 75.00m; Date = DateTime(2023, 2, 10) |}
    {| Id = 4; CustomerId = 3; Amount = 300.00m; Date = DateTime(2023, 2, 15) |}
    {| Id = 5; CustomerId = 2; Amount = 125.00m; Date = DateTime(2023, 3, 5) |}
]

let customers = [
    {| Id = 1; Name = "Alice Corp"; City = "New York" |}
    {| Id = 2; Name = "Bob Inc"; City = "Seattle" |}
    {| Id = 3; Name = "Carol LLC"; City = "Chicago" |}
]

// Find customers with above-average order amounts
let avgOrderAmount = query {
    for order in orders do
    averageBy (float order.Amount)
}

let highValueCustomers = query {
    for customer in customers do
    where (query {
        for order in orders do
        where (order.CustomerId = customer.Id)
        exists (order.Amount > decimal avgOrderAmount)
    })
    select customer
}

Console.WriteLine($"Average order amount: ${avgOrderAmount:F2}")
Console.WriteLine("Customers with above-average orders:")
highValueCustomers |> Seq.iter (fun c -> printfn "%s from %s" c.Name c.City)
```

Nested queries enable complex filtering logic by embedding query  
expressions within predicates. Subqueries can test for existence,  
calculate aggregates, or perform correlated operations. This provides  
SQL-like functionality for sophisticated data analysis scenarios.

## Conditional Selection

Using conditional logic within select projections.  

```f#
open System

let inventory = [
    {| Product = "Laptop"; Quantity = 5; ReorderLevel = 10 |}
    {| Product = "Mouse"; Quantity = 25; ReorderLevel = 20 |}
    {| Product = "Keyboard"; Quantity = 8; ReorderLevel = 15 |}
    {| Product = "Monitor"; Quantity = 12; ReorderLevel = 10 |}
]

let inventoryStatus = query {
    for item in inventory do
    select (item.Product, 
            item.Quantity,
            if item.Quantity <= item.ReorderLevel then "Reorder Required" 
            else "In Stock")
}

let categorizedProducts = query {
    for item in inventory do
    select {|
        Product = item.Product
        Status = match item.Quantity with
                 | q when q = 0 -> "Out of Stock"
                 | q when q <= item.ReorderLevel -> "Low Stock"
                 | q when q > item.ReorderLevel * 2 -> "Overstocked"
                 | _ -> "Normal"
        Quantity = item.Quantity
    |}
}

Console.WriteLine("Inventory Status:")
inventoryStatus |> Seq.iter (fun (product, qty, status) ->
    printfn "%s: %d units - %s" product qty status)

Console.WriteLine("\nCategorized Products:")
categorizedProducts |> Seq.iter (fun p ->
    printfn "%s: %s (%d units)" p.Product p.Status p.Quantity)
```

Conditional selection enables dynamic data transformation within queries.  
If-then expressions and pattern matching create computed fields based on  
existing data. This eliminates the need for post-processing steps and  
keeps transformation logic within the query expression.

## Working with Dates

Filtering and grouping operations on DateTime values.  

```f#
open System

type Transaction = {
    Id: int
    Amount: decimal
    Date: DateTime
    Category: string
}

let transactions = [
    { Id = 1; Amount = 100m; Date = DateTime(2023, 1, 15); Category = "Food" }
    { Id = 2; Amount = 50m; Date = DateTime(2023, 1, 22); Category = "Gas" }
    { Id = 3; Amount = 200m; Date = DateTime(2023, 2, 10); Category = "Shopping" }
    { Id = 4; Amount = 75m; Date = DateTime(2023, 2, 20); Category = "Food" }
    { Id = 5; Amount = 300m; Date = DateTime(2023, 3, 5); Category = "Rent" }
    { Id = 6; Amount = 25m; Date = DateTime(2023, 3, 12); Category = "Gas" }
]

let recentTransactions = query {
    for t in transactions do
    where (t.Date >= DateTime(2023, 2, 1))
    sortByDescending t.Date
    select t
}

let monthlyTotals = query {
    for t in transactions do
    groupBy t.Date.Month into monthGroup
    select (monthGroup.Key, monthGroup.Sum(fun x -> x.Amount))
}

let firstQuarter = query {
    for t in transactions do
    where (t.Date.Month <= 3)
    select t
    sumBy (fun x -> x.Amount)
}

Console.WriteLine("Recent Transactions (Feb 2023 onwards):")
recentTransactions |> Seq.iter (fun t ->
    printfn "%s - $%.2f (%s)" (t.Date.ToShortDateString()) t.Amount t.Category)

Console.WriteLine("\nMonthly Totals:")
monthlyTotals |> Seq.iter (fun (month, total) ->
    printfn "Month %d: $%.2f" month total)

Console.WriteLine($"\nFirst Quarter Total: ${firstQuarter}")
```

DateTime operations enable time-based filtering and grouping. Date  
properties like Month, Year, and Day support temporal analysis. Query  
expressions handle date comparisons naturally, making time-series  
analysis and reporting straightforward to implement.

## String Operations

String manipulation and filtering within query expressions.  

```f#
open System

let names = [| "Alice Johnson"; "Bob Smith"; "Carol Davis"; "David Wilson"; "Eve Brown" |]

let longNames = query {
    for name in names do
    where (name.Length > 10)
    select name
}

let initialsOnly = query {
    for name in names do
    select (name.Split(' ') |> Array.map (fun n -> n.[0]) |> String)
}

let namesByFirstLetter = query {
    for name in names do
    where (name.StartsWith("A") || name.StartsWith("B"))
    select name.ToUpper()
}

let containsFilter = query {
    for name in names do
    where (name.Contains("son"))
    select name
}

Console.WriteLine("Long names (> 10 characters):")
longNames |> Seq.iter (printfn "%s")

Console.WriteLine("\nInitials:")
initialsOnly |> Seq.iter (printfn "%s")

Console.WriteLine("\nNames starting with A or B (uppercase):")
namesByFirstLetter |> Seq.iter (printfn "%s")

Console.WriteLine("\nNames containing 'son':")
containsFilter |> Seq.iter (printfn "%s")
```

String operations within queries enable text processing and filtering.  
String methods like Length, StartsWith, Contains, and ToUpper integrate  
seamlessly with query expressions. Complex string manipulation can be  
performed during projection, eliminating separate processing steps.

## Grouping with Multiple Keys

Complex grouping operations using compound keys and anonymous types.  

```f#
open System

type Sale = {
    Id: int
    Product: string
    Region: string
    Quarter: string
    Amount: decimal
    SalesRep: string
}

let sales = [
    { Id = 1; Product = "Laptop"; Region = "North"; Quarter = "Q1"; Amount = 1500m; SalesRep = "Alice" }
    { Id = 2; Product = "Mouse"; Region = "North"; Quarter = "Q1"; Amount = 50m; SalesRep = "Bob" }
    { Id = 3; Product = "Laptop"; Region = "South"; Quarter = "Q1"; Amount = 1200m; SalesRep = "Carol" }
    { Id = 4; Product = "Laptop"; Region = "North"; Quarter = "Q2"; Amount = 1800m; SalesRep = "Alice" }
    { Id = 5; Product = "Mouse"; Region = "South"; Quarter = "Q2"; Amount = 75m; SalesRep = "David" }
    { Id = 6; Product = "Keyboard"; Region = "North"; Quarter = "Q2"; Amount = 200m; SalesRep = "Bob" }
]

let regionQuarterSales = query {
    for sale in sales do
    groupBy (sale.Region, sale.Quarter) into group
    select (group.Key, group.Sum(fun s -> s.Amount), group.Count())
}

let productRegionAnalysis = query {
    for sale in sales do
    groupBy {| Product = sale.Product; Region = sale.Region |} into group
    select {|
        ProductRegion = group.Key
        TotalSales = group.Sum(fun s -> s.Amount)
        AverageSale = group.Average(fun s -> float s.Amount)
        TransactionCount = group.Count()
    |}
}

Console.WriteLine("Sales by Region and Quarter:")
regionQuarterSales |> Seq.iter (fun ((region, quarter), total, count) ->
    printfn "%s %s: $%.2f (%d transactions)" region quarter total count)

Console.WriteLine("\nProduct-Region Analysis:")
productRegionAnalysis |> Seq.iter (fun analysis ->
    printfn "%s in %s: $%.2f total, $%.2f avg (%d sales)" 
        analysis.ProductRegion.Product 
        analysis.ProductRegion.Region
        analysis.TotalSales 
        analysis.AverageSale 
        analysis.TransactionCount)
```

Multi-key grouping creates hierarchical data analysis using tuples or  
anonymous types as compound keys. This enables cross-tabulation and  
multi-dimensional aggregation similar to pivot table operations.  
Complex business intelligence queries become straightforward to express.

## Set Operations

Union, intersect, and except operations for set-based data processing.  

```f#
open System

let techSkills = [| "C#"; "F#"; "JavaScript"; "Python"; "SQL" |]
let dataSkills = [| "Python"; "R"; "SQL"; "Excel"; "Tableau" |]
let webSkills = [| "JavaScript"; "HTML"; "CSS"; "React"; "Node.js" |]

let allSkills = query {
    for skill in techSkills do
    select skill
    union (query {
        for skill in dataSkills do
        select skill
    })
}

let commonTechData = query {
    for skill in techSkills do
    select skill
    intersect (query {
        for skill in dataSkills do
        select skill  
    })
}

let techOnlySkills = query {
    for skill in techSkills do
    select skill
    except (query {
        for skill in dataSkills do
        select skill
    })
}

Console.WriteLine("All unique skills:")
allSkills |> Seq.iter (printfn "- %s")

Console.WriteLine("\nCommon tech and data skills:")
commonTechData |> Seq.iter (printfn "- %s")

Console.WriteLine("\nTech-only skills:")
techOnlySkills |> Seq.iter (printfn "- %s")
```

Set operations provide mathematical set functionality within query  
expressions. Union combines distinct elements, intersect finds common  
elements, and except returns differences. These operations enable  
complex data comparison and analysis scenarios.

## Quantifier Operations

Universal and existential quantification using all and exists operators.  

```f#
open System

type Product = {
    Name: string
    Price: decimal
    InStock: bool
    Rating: float
    Category: string
}

let products = [
    { Name = "Laptop Pro"; Price = 1299m; InStock = true; Rating = 4.8; Category = "Electronics" }
    { Name = "Wireless Mouse"; Price = 29m; InStock = true; Rating = 4.2; Category = "Electronics" }
    { Name = "USB Cable"; Price = 15m; InStock = false; Rating = 4.0; Category = "Electronics" }
    { Name = "Gaming Chair"; Price = 299m; InStock = true; Rating = 4.5; Category = "Furniture" }
]

let allElectronicsInStock = query {
    for p in products do
    where (p.Category = "Electronics")
    all p.InStock
}

let hasExpensiveProducts = query {
    for p in products do
    exists (p.Price > 1000m)
}

let allHighRated = query {
    for p in products do
    all (p.Rating >= 4.0)
}

let hasOutOfStockElectronics = query {
    for p in products do
    where (p.Category = "Electronics")
    exists (not p.InStock)
}

Console.WriteLine($"All electronics in stock: {allElectronicsInStock}")
Console.WriteLine($"Has expensive products (>$1000): {hasExpensiveProducts}")
Console.WriteLine($"All products highly rated (>=4.0): {allHighRated}")
Console.WriteLine($"Has out-of-stock electronics: {hasOutOfStockElectronics}")
```

Quantifier operations test conditions across entire collections. The  
`all` operator checks universal conditions (all elements satisfy predicate),  
while `exists` tests existential conditions (at least one element satisfies  
predicate). These provide logical quantification for validation scenarios.

## Working with Sequences

Processing infinite and large sequences efficiently using query expressions.  

```f#
open System

let infiniteNumbers = Seq.initInfinite id

let evenNumbers = query {
    for n in infiniteNumbers do
    where (n % 2 = 0)
    take 10
    select n
}

let fibonacci = Seq.unfold (fun (a, b) -> Some(a, (b, a + b))) (0, 1)

let largeFibonacci = query {
    for fib in fibonacci do
    where (fib > 100)
    take 5
    select fib
}

let primeNumbers = 
    let isPrime n = 
        n > 1 && not (seq { 2..int(sqrt(float n)) } |> Seq.exists (fun x -> n % x = 0))
    seq { 2..1000 } |> Seq.filter isPrime

let largePrimes = query {
    for prime in primeNumbers do
    where (prime > 500)
    take 10
    select prime
}

Console.WriteLine("First 10 even numbers:")
evenNumbers |> Seq.iter (printfn "%d")

Console.WriteLine("\nFibonacci numbers > 100 (first 5):")
largeFibonacci |> Seq.iter (printfn "%d")

Console.WriteLine("\nPrime numbers > 500 (first 10):")
largePrimes |> Seq.iter (printfn "%d")
```

Query expressions work efficiently with sequences, including infinite  
sequences. Lazy evaluation ensures only needed elements are computed.  
The combination of where, take, and select creates efficient processing  
pipelines for large or infinite data sources.

## Custom Comparison Operations

Implementing custom ordering and equality for complex data types.  

```f#
open System

[<CustomComparison; CustomEquality>]
type Version = {
    Major: int
    Minor: int
    Patch: int
} with
    interface IComparable with
        member x.CompareTo(obj) =
            match obj with
            | :? Version as v -> 
                let majorComp = x.Major.CompareTo(v.Major)
                if majorComp <> 0 then majorComp
                else
                    let minorComp = x.Minor.CompareTo(v.Minor)
                    if minorComp <> 0 then minorComp
                    else x.Patch.CompareTo(v.Patch)
            | _ -> invalidArg "obj" "Cannot compare Version to different type"
    
    override x.Equals(obj) =
        match obj with
        | :? Version as v -> x.Major = v.Major && x.Minor = v.Minor && x.Patch = v.Patch
        | _ -> false
    
    override x.GetHashCode() = (x.Major, x.Minor, x.Patch).GetHashCode()

type Software = {
    Name: string
    Version: Version
    ReleaseDate: DateTime
}

let software = [
    { Name = "App A"; Version = { Major = 2; Minor = 1; Patch = 0 }; ReleaseDate = DateTime(2023, 1, 15) }
    { Name = "App B"; Version = { Major = 1; Minor = 5; Patch = 2 }; ReleaseDate = DateTime(2023, 2, 20) }
    { Name = "App C"; Version = { Major = 2; Minor = 0; Patch = 1 }; ReleaseDate = DateTime(2023, 3, 10) }
    { Name = "App D"; Version = { Major = 1; Minor = 5; Patch = 3 }; ReleaseDate = DateTime(2023, 1, 30) }
]

let sortedByVersion = query {
    for app in software do
    sortBy app.Version
    select app
}

let latestVersions = query {
    for app in software do
    where (app.Version.Major >= 2)
    select app
}

Console.WriteLine("Software sorted by version:")
sortedByVersion |> Seq.iter (fun app ->
    printfn "%s v%d.%d.%d" app.Name app.Version.Major app.Version.Minor app.Version.Patch)

Console.WriteLine("\nSoftware with version 2.0+:")
latestVersions |> Seq.iter (fun app ->
    printfn "%s v%d.%d.%d" app.Name app.Version.Major app.Version.Minor app.Version.Patch)
```

Custom comparison enables domain-specific sorting and equality for  
complex types. Implementing IComparable allows sortBy operations on  
custom data structures. This provides semantic ordering that matches  
business logic rather than default structural comparison.

## Query Optimization Patterns

Techniques for optimizing query performance and memory usage.  

```f#
open System
open System.Diagnostics

let largeDataSet = [| 1..1000000 |]

// Efficient: Use exists for early termination
let hasLargeNumberEfficient = query {
    for n in largeDataSet do
    exists (n > 900000)
}

// Less efficient: Using where + count for existence check
let hasLargeNumberInefficient = 
    let count = query {
        for n in largeDataSet do
        where (n > 900000)
        count
    }
    count > 0

// Efficient: Combine operations in single query
let efficientPipeline = query {
    for n in largeDataSet do
    where (n % 2 = 0)
    where (n > 500000)
    take 100
    select (n * 2)
}

// Less efficient: Multiple query operations
let inefficientPipeline = 
    let step1 = query {
        for n in largeDataSet do
        where (n % 2 = 0)
        select n
    }
    let step2 = query {
        for n in step1 do
        where (n > 500000)
        select n
    }
    let step3 = query {
        for n in step2 do
        take 100
        select (n * 2)
    }
    step3

// Measure performance
let timeQuery name queryFunc =
    let sw = Stopwatch.StartNew()
    let result = queryFunc() |> Seq.toArray
    sw.Stop()
    printfn "%s: %dms, %d results" name sw.ElapsedMilliseconds result.Length

timeQuery "Efficient exists" (fun () -> seq { yield hasLargeNumberEfficient })
timeQuery "Inefficient count" (fun () -> seq { yield hasLargeNumberInefficient })
timeQuery "Efficient pipeline" (fun () -> efficientPipeline)
timeQuery "Inefficient pipeline" (fun () -> inefficientPipeline)
```

Query optimization focuses on early termination, combining operations,  
and avoiding unnecessary materialization. Using exists instead of count  
for existence checks improves performance. Single queries with multiple  
criteria outperform multiple separate query operations due to reduced  
iteration overhead.

## Error Handling in Queries

Robust query expressions with proper error handling and validation.  

```f#
open System

type SafeResult<'T> = 
    | Success of 'T
    | Error of string

let tryQuery (queryFunc: unit -> 'T) : SafeResult<'T> =
    try
        Success (queryFunc())
    with
    | :? System.ArgumentException as ex -> Error $"Argument error: {ex.Message}"
    | :? System.InvalidOperationException as ex -> Error $"Operation error: {ex.Message}"
    | ex -> Error $"Unexpected error: {ex.Message}"

let numbers = [| 1; 2; 3; 4; 5 |]
let emptyNumbers: int array = [||]

// Safe query operations
let safeFirst = tryQuery (fun () ->
    query {
        for n in numbers do
        head
    })

let safeFirstEmpty = tryQuery (fun () ->
    query {
        for n in emptyNumbers do
        head
    })

let safeAverage = tryQuery (fun () ->
    query {
        for n in numbers do
        averageBy (float n)
    })

let safeAverageEmpty = tryQuery (fun () ->
    query {
        for n in emptyNumbers do
        averageBy (float n)
    })

// Helper function to print results
let printResult name result =
    match result with
    | Success value -> printfn "%s: Success - %A" name value
    | Error msg -> printfn "%s: %s" name msg

printResult "First element" safeFirst
printResult "First of empty" safeFirstEmpty
printResult "Average" safeAverage
printResult "Average of empty" safeAverageEmpty

// Query with validation
let validateAndQuery data predicate =
    if Array.isEmpty data then
        Error "Cannot query empty collection"
    else
        tryQuery (fun () ->
            query {
                for item in data do
                where (predicate item)
                select item
            } |> Seq.toArray)

let validatedQuery = validateAndQuery numbers (fun n -> n > 3)
let validatedEmptyQuery = validateAndQuery emptyNumbers (fun n -> n > 0)

printResult "Validated query" validatedQuery  
printResult "Validated empty query" validatedEmptyQuery
```

Error handling in queries prevents runtime exceptions through defensive  
programming. Wrapping query operations in try-catch blocks provides  
graceful error handling. Validation before query execution catches  
common error conditions like empty collections early.

`groupBy` and `Sum`  

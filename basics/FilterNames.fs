let people = [("Amber", 23, "Design")
              ("Wendy", 35, "Sales")
              ("Alex", 39, "Marketing")
              ("Carlos", 51, "Sales")]

let filtered =
    people
        |>Seq.map (fun (name, age, dept) -> name)
        |>Seq.filter (fun name -> name.StartsWith "A")
        |>Seq.toList

printfn "%A" filtered

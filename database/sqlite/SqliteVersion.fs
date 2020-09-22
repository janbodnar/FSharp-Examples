open System
open System.Data.SQLite

[<EntryPoint>]
let main argv =

    let cs = "Data Source=:memory:"
    let stm = "SELECT SQLITE_VERSION()"

    use con = new SQLiteConnection(cs)
    con.Open()

    use cmd = new SQLiteCommand(stm, con)
    let version = cmd.ExecuteScalar()

    printfn "SQLite version: %O" version

    0 


// dotnet add package System.Data.SQLite.Core

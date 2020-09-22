open MySql.Data.MySqlClient

// dotnet add package MySql.Data

let showAll = 
    let cs = @"server=localhost;userid=user7;password=s$cret;database=testdb";

    use con = new MySqlConnection(cs)
    con.Open()

    let sql = "SELECT * FROM cities"
    use cmd = new MySqlCommand(sql, con)

    use rdr = cmd.ExecuteReader()

    while rdr.Read() do
        printfn "%d %s %d" (rdr.GetInt32 0) (rdr.GetString 1) (rdr.GetInt32 2)

showAll

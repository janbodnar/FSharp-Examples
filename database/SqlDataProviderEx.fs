open FSharp.Data.Sql

// dotnet add package SQLProvider

[<Literal>]
let ConnectionString = 
    "Data Source=data/test.db;Version=3;"

[<Literal>]
let ResolutionPath = 
    "/home/janbodnar/.nuget/packages/system.data.sqlite.core/1.0.113.1/lib/net46"

type sql = SqlDataProvider<
            DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
            SQLiteLibrary = Common.SQLiteLibrary.SystemDataSQLite,
            ConnectionString = ConnectionString, 
            ResolutionPath = ResolutionPath>

let ctx = sql.GetDataContext()

let data =
    query {
        for city in ctx.Main.Cities do
        where (city.Population > 1000000L)
        select (city.Name, city.Population)
    } |> Seq.toList


printfn "%A" data

data |> List.iter (printfn "%O")


(* copy SQLite.Interop.dll  from 
/home/janbodnar/.nuget/packages/system.data.sqlite.core/1.0.113.1/runtimes/linux-x64/native/netstandard2.1
to
/home/janbodnar/.nuget/packages/system.data.sqlite.core/1.0.113.1/lib/net46
https://stackoverflow.com/questions/38776391/problems-with-resolutionpath-using-sqlprovider-with-sqlite
*)

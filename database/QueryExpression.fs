let data = [ 1; 2; 5; 7; 8; 11; 18; 21; 24; 27;]

let getEvenInts = fun (arr : list<int> ) -> async {
    let q = query {
      for n in arr do
      where (( n % 2 ) = 0)
      select n
    }
    return q
  }

let getOddInts = fun (arr : list<int> ) -> async {
    let q = query {
      for n in arr do
      where (( n % 2 ) <> 0)
      select n
    }
    return q
  }

let evens = getEvenInts data |> Async.RunSynchronously
let odds = getOddInts data |> Async.RunSynchronously

printf "Even numbers: "
evens |> Seq.iter (printf "%d ")

printfn "\n------------------------"

printf "Odd numbers: "
odds |> Seq.iter (printf "%d ")

printf "\n"

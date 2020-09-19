open System

[<EntryPoint>]
let main argv =
    
    let triples n = [ for a in [1..n] do
                      for b in [a..n] do
                      for c in [b..n] do
                      if (a*a + b*b = c*c) then yield (a,b,c)]

    printf "%A\n" (triples 25)

    0

// A Pythagorean triple is defined as three positive integers (a, b, c) 
// where a < b < c, and a^2 + b^2 = c^2. 

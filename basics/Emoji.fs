open System


[<EntryPoint>]
let main argv =

    let runes = "♔♕♖♗♘♚♛♜♝♞".EnumerateRunes()

    // '♔', '♕', '♖', '♗', '♘', '♚', '♛', '♜', '♝', '♞'

    printfn "%A" runes

    for rune in runes do
    
          printfn "%O" rune
        //   Console.WriteLine rune

    0 

open System.IO

let writeData() =

  use sw = new StreamWriter("data.txt")

  fprintf sw "Today is a beautiful day\n"
  fprintf sw "We go swimming and fishing\n"


writeData()

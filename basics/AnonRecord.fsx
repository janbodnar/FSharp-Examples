let user = {| FirstName = "John Doe"; Age = 33 |} // Anonymous record
let user2 = {| user with Age = 34 |}

printf "%A\n" user
printf "%A\n" user2

let showMessage name age =
    printf "%s is %i years old\n" name age

("John Doe", 34) ||> showMessage

let showMessage2 name age occupation =
    printf "%s is %i years old and he is a %s\n" name age occupation

("John Doe", 34, "gardener") |||> showMessage2

// pass 2 parameters with ||> and 3 parameters with |||>

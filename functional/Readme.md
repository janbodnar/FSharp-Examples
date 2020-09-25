Every function in F# has one parameter and returns one value. That value can
be of type unit, designated by (), which is similar in concept to void in
some other languages.
When you have a function that appears to have more than one parameter, F#
treats it as several functions, each with one parameter, that are then
"curried" to come up with the result we want. 

This naturally leads to a style of programming where you always keep the most 
important argument (usually the main data structure that you give as the input) 
to any function as its last argument. It makes function composition much easier.

Currying is about fixing arguments of functions from left to right. It's useful 
to configurate code and embed parameters that usually serve to define the context 
of a function execution (i.e. a database connection object). 

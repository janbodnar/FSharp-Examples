# Object-Oriented Programming

F# supports object-oriented programming with classes, inheritance, and  
interfaces. While F# is primarily functional, OOP features provide  
interoperability with .NET libraries and enable familiar design patterns.  

## Basic class

A simple class definition with a primary constructor and methods.  

```F#
type Person(name: string, age: int) =
    member this.Name = name
    member this.Age = age
    
    member this.Greet() =
        $"Hello, I'm {this.Name} and I'm {this.Age} years old"

let person = Person("Alice", 25)
printfn "%s" (person.Greet())
printfn "Name: %s, Age: %d" person.Name person.Age
```

The class `Person` has a primary constructor that takes `name` and `age`.  
Properties and methods are defined using the `member` keyword. The `this`  
keyword refers to the current instance.  

## Class with mutable field

Classes can contain mutable fields that can be modified after creation.  

```F#
type BankAccount(initialBalance: decimal) =
    let mutable balance = initialBalance
    
    member this.Balance = balance
    
    member this.Deposit(amount: decimal) =
        balance <- balance + amount
        
    member this.Withdraw(amount: decimal) =
        if amount <= balance then
            balance <- balance - amount
            true
        else
            false

let account = BankAccount(100.0m)
printfn "Initial balance: %M" account.Balance

account.Deposit(50.0m)
printfn "After deposit: %M" account.Balance

let success = account.Withdraw(30.0m)
printfn "Withdrawal success: %b, Balance: %M" success account.Balance
```

The `mutable` keyword allows the balance field to be modified. Methods  
can change the internal state while maintaining encapsulation.  

## Properties with getters and setters

Properties can have custom getter and setter logic.  

```F#
type Temperature() =
    let mutable celsius = 0.0
    
    member this.Celsius
        with get() = celsius
        and set(value) = celsius <- value
        
    member this.Fahrenheit
        with get() = celsius * 9.0 / 5.0 + 32.0
        and set(value) = celsius <- (value - 32.0) * 5.0 / 9.0

let temp = Temperature()
temp.Celsius <- 25.0
printfn "25°C = %.1f°F" temp.Fahrenheit

temp.Fahrenheit <- 86.0
printfn "86°F = %.1f°C" temp.Celsius
```

Properties with custom logic allow computed values and validation.  
The Fahrenheit property converts to/from Celsius automatically.  

## Static members

Static members belong to the type rather than instances.  

```F#
type MathUtils() =
    static member Pi = 3.14159265359
    
    static member CircleArea(radius: double) =
        MathUtils.Pi * radius * radius
        
    static member RectangleArea(width: double, height: double) =
        width * height

printfn "Pi: %f" MathUtils.Pi
printfn "Circle area (r=5): %f" (MathUtils.CircleArea(5.0))
printfn "Rectangle area (3x4): %f" (MathUtils.RectangleArea(3.0, 4.0))
```

Static members are accessed using the type name. They don't require  
instance creation and are useful for utility functions and constants.  

## Additional constructors

Classes can have multiple constructors with different parameters.  

```F#
type Rectangle(width: double, height: double) =
    let mutable w = width
    let mutable h = height
    
    // Additional constructor for square
    new(side: double) = Rectangle(side, side)
    
    // Additional parameterless constructor
    new() = Rectangle(1.0, 1.0)
    
    member this.Width = w
    member this.Height = h
    member this.Area = w * h
    
    member this.Scale(factor: double) =
        w <- w * factor
        h <- h * factor

let rect1 = Rectangle(5.0, 3.0)  // Primary constructor
let rect2 = Rectangle(4.0)       // Square constructor
let rect3 = Rectangle()          // Default constructor

printfn "Rect1: %fx%f, Area: %f" rect1.Width rect1.Height rect1.Area
printfn "Rect2: %fx%f, Area: %f" rect2.Width rect2.Height rect2.Area
printfn "Rect3: %fx%f, Area: %f" rect3.Width rect3.Height rect3.Area
```

Additional constructors use the `new` keyword and must call the primary  
constructor. This provides flexibility in object creation.  

## Inheritance

Classes can inherit from base classes using the `inherit` keyword.  

```F#
type Animal(name: string) =
    member this.Name = name
    
    abstract member MakeSound: unit -> string
    default this.MakeSound() = "Some generic sound"
    
    member this.Introduce() =
        $"I'm {this.Name} and I say: {this.MakeSound()}"

type Dog(name: string, breed: string) =
    inherit Animal(name)
    
    member this.Breed = breed
    
    override this.MakeSound() = "Woof!"

type Cat(name: string) =
    inherit Animal(name)
    
    override this.MakeSound() = "Meow!"

let dog = Dog("Buddy", "Golden Retriever")
let cat = Cat("Whiskers")

printfn "%s" (dog.Introduce())
printfn "Breed: %s" dog.Breed
printfn "%s" (cat.Introduce())
```

Inheritance allows code reuse and polymorphism. The `override` keyword  
redefines virtual or abstract methods from the base class.  

## Abstract classes

Abstract classes cannot be instantiated and may contain abstract members.  

```F#
[<AbstractClass>]
type Shape() =
    abstract member Area: double
    abstract member Perimeter: double
    
    member this.Display() =
        printfn "Shape - Area: %.2f, Perimeter: %.2f" this.Area this.Perimeter

type Circle(radius: double) =
    inherit Shape()
    
    member this.Radius = radius
    
    override this.Area = System.Math.PI * radius * radius
    override this.Perimeter = 2.0 * System.Math.PI * radius

type Square(side: double) =
    inherit Shape()
    
    member this.Side = side
    
    override this.Area = side * side
    override this.Perimeter = 4.0 * side

let circle = Circle(5.0)
let square = Square(4.0)

circle.Display()
square.Display()
```

Abstract classes define contracts that derived classes must implement.  
The `[<AbstractClass>]` attribute marks the class as abstract.  

## Interfaces

Interfaces define contracts that types must implement.  

```F#
type IDrawable =
    abstract member Draw: unit -> string
    abstract member Color: string with get, set

type IResizable =
    abstract member Resize: double -> unit

type Circle(radius: double, color: string) =
    let mutable r = radius
    let mutable c = color
    
    interface IDrawable with
        member this.Draw() = $"Drawing a {c} circle with radius {r}"
        member this.Color 
            with get() = c
            and set(value) = c <- value
    
    interface IResizable with
        member this.Resize(factor: double) = r <- r * factor
    
    member this.Radius = r

let circle = Circle(5.0, "red")
let drawable = circle :> IDrawable
let resizable = circle :> IResizable

printfn "%s" (drawable.Draw())
resizable.Resize(2.0)
printfn "After resize: %s" (drawable.Draw())
```

Interfaces are implemented using the `interface` keyword. The `:>` operator  
performs upcast to interface types.  

## Object expressions

Object expressions create instances that implement interfaces without  
defining named classes.  

```F#
type ILogger =
    abstract member Log: string -> unit

let consoleLogger = 
    { new ILogger with
        member this.Log(message) = printfn "LOG: %s" message }

let fileLogger filename =
    { new ILogger with
        member this.Log(message) = 
            System.IO.File.AppendAllText(filename, message + "\n") }

consoleLogger.Log("Application started")
consoleLogger.Log("Processing data")

let logger = fileLogger "app.log"
logger.Log("Writing to file")
```

Object expressions provide a concise way to implement interfaces without  
creating separate type definitions. Useful for callbacks and small  
implementations.  

## Generic classes

Classes can be parameterized with generic type parameters.  

```F#
type Stack<'T>() =
    let mutable items: 'T list = []
    
    member this.Push(item: 'T) =
        items <- item :: items
        
    member this.Pop() =
        match items with
        | [] -> failwith "Stack is empty"
        | head :: tail ->
            items <- tail
            head
            
    member this.IsEmpty = List.isEmpty items
    member this.Count = List.length items
    
    member this.Peek() =
        match items with
        | [] -> failwith "Stack is empty"
        | head :: _ -> head

let intStack = Stack<int>()
intStack.Push(1)
intStack.Push(2)
intStack.Push(3)

printfn "Count: %d" intStack.Count
printfn "Top: %d" (intStack.Peek())
printfn "Popped: %d" (intStack.Pop())
printfn "Count after pop: %d" intStack.Count

let stringStack = Stack<string>()
stringStack.Push("Hello")
stringStack.Push("World")
printfn "String stack top: %s" (stringStack.Peek())
```

Generic classes use type parameters (`'T`) that are resolved at compile  
time. This provides type safety while maintaining code reusability.  

## Virtual methods

Virtual methods can be overridden by derived classes.  

```F#
type Vehicle(brand: string) =
    member this.Brand = brand
    
    abstract member StartEngine: unit -> string
    default this.StartEngine() = "Engine started"
    
    virtual member GetInfo() = $"Vehicle: {this.Brand}"

type Car(brand: string, model: string) =
    inherit Vehicle(brand)
    
    member this.Model = model
    
    override this.StartEngine() = "Car engine started with key"
    override this.GetInfo() = $"Car: {this.Brand} {this.Model}"

type ElectricCar(brand: string, model: string, batteryCapacity: int) =
    inherit Car(brand, model)
    
    member this.BatteryCapacity = batteryCapacity
    
    override this.StartEngine() = "Electric motor started silently"
    override this.GetInfo() = 
        $"Electric Car: {this.Brand} {this.Model} ({this.BatteryCapacity}kWh)"

let vehicles = [
    Vehicle("Generic") :> Vehicle
    Car("Toyota", "Camry") :> Vehicle
    ElectricCar("Tesla", "Model 3", 75) :> Vehicle
]

for vehicle in vehicles do
    printfn "%s" (vehicle.GetInfo())
    printfn "%s" (vehicle.StartEngine())
    printfn ""
```

Virtual methods provide polymorphic behavior. Each derived class can  
provide its own implementation while maintaining a common interface.  

## Method overloading

Methods can be overloaded with different parameter types or counts.  

```F#
type Calculator() =
    member this.Add(a: int, b: int) = a + b
    member this.Add(a: double, b: double) = a + b
    member this.Add(a: string, b: string) = a + b
    
    member this.Multiply(a: int, b: int) = a * b
    member this.Multiply(a: int, b: int, c: int) = a * b * c
    member this.Multiply(numbers: int array) = Array.fold (*) 1 numbers

let calc = Calculator()

printfn "Int add: %d" (calc.Add(5, 3))
printfn "Double add: %f" (calc.Add(2.5, 3.7))
printfn "String add: %s" (calc.Add("Hello ", "World"))

printfn "Multiply 2 numbers: %d" (calc.Multiply(4, 6))
printfn "Multiply 3 numbers: %d" (calc.Multiply(2, 3, 4))
printfn "Multiply array: %d" (calc.Multiply([|2; 3; 5|]))
```

Method overloading allows multiple methods with the same name but  
different signatures. The compiler selects the appropriate method  
based on parameter types.  

## Access modifiers

Access modifiers control member visibility.  

```F#
type BankAccount(accountNumber: string, initialBalance: decimal) =
    let mutable balance = initialBalance
    let mutable isLocked = false
    
    // Public members (default)
    member this.AccountNumber = accountNumber
    member this.Balance = balance
    
    // Private method
    member private this.ValidateAmount(amount: decimal) =
        amount > 0.0m && amount <= 10000.0m
    
    // Internal method  
    member internal this.LockAccount() =
        isLocked <- true
        
    member internal this.UnlockAccount() =
        isLocked <- false
    
    member this.Deposit(amount: decimal) =
        if isLocked then
            false
        elif this.ValidateAmount(amount) then
            balance <- balance + amount
            true
        else
            false
    
    member this.Withdraw(amount: decimal) =
        if isLocked then
            false
        elif this.ValidateAmount(amount) && amount <= balance then
            balance <- balance - amount
            true
        else
            false

let account = BankAccount("12345", 1000.0m)

printfn "Account: %s, Balance: %M" account.AccountNumber account.Balance

let success1 = account.Deposit(100.0m)
printfn "Deposit success: %b, New balance: %M" success1 account.Balance

// account.ValidateAmount(50.0m) // Error: private method not accessible
account.LockAccount() // Internal method accessible in same assembly

let success2 = account.Withdraw(50.0m)
printfn "Withdraw success (locked): %b" success2
```

Access modifiers include `public` (default), `private`, `internal`, and  
`protected`. They control which code can access class members.  

## Type checking and casting

F# provides type checking and casting operations for OOP scenarios.  

```F#
type Animal(name: string) =
    member this.Name = name
    abstract member MakeSound: unit -> string
    default this.MakeSound() = "Generic sound"

type Dog(name: string) =
    inherit Animal(name)
    member this.Breed = "Unknown"
    override this.MakeSound() = "Woof!"

type Cat(name: string) =
    inherit Animal(name)
    member this.Lives = 9
    override this.MakeSound() = "Meow!"

let animals: Animal list = [
    Dog("Buddy") :> Animal
    Cat("Whiskers") :> Animal
    Animal("Generic")
]

for animal in animals do
    printfn "%s says %s" animal.Name (animal.MakeSound())
    
    // Type checking with :?
    if animal :? Dog then
        let dog = animal :?> Dog // Downcast
        printfn "  This is a dog with breed: %s" dog.Breed
    elif animal :? Cat then
        let cat = animal :?> Cat // Downcast
        printfn "  This cat has %d lives" cat.Lives
    else
        printfn "  This is a generic animal"
        
    // Pattern matching with type test
    match animal with
    | :? Dog as dog -> printfn "  Pattern: Dog breed %s" dog.Breed
    | :? Cat as cat -> printfn "  Pattern: Cat with %d lives" cat.Lives
    | _ -> printfn "  Pattern: Unknown animal type"
```

The `:?` operator checks types, `:?>` performs downcasting, and `:>`  
performs upcasting. Pattern matching also supports type tests.  

## IDisposable implementation

Classes can implement IDisposable for resource management.  

```F#
open System

type FileManager(filename: string) =
    let mutable file = Some(System.IO.File.Create(filename))
    let mutable disposed = false
    
    member this.WriteData(data: string) =
        match file with
        | Some f when not disposed ->
            let bytes = System.Text.Encoding.UTF8.GetBytes(data)
            f.Write(bytes, 0, bytes.Length)
            f.Flush()
        | _ -> failwith "File is disposed"
    
    interface IDisposable with
        member this.Dispose() =
            if not disposed then
                match file with
                | Some f -> 
                    f.Close()
                    f.Dispose()
                | None -> ()
                file <- None
                disposed <- true

// Using 'use' for automatic disposal
let useFileManager() =
    use fm = new FileManager("test.txt")
    fm.WriteData("Hello World!\n")
    fm.WriteData("F# OOP example\n")
    // Automatically disposed when leaving scope

useFileManager()

// Manual disposal
let fm2 = new FileManager("test2.txt")
try
    fm2.WriteData("Manual management\n")
finally
    (fm2 :> IDisposable).Dispose()

printfn "Files created and disposed"
```

IDisposable ensures proper resource cleanup. The `use` keyword provides  
automatic disposal, similar to C#'s `using` statement.  

## Equality and comparison

Classes can override equality and implement comparison interfaces.  

```F#
open System

[<CustomEquality; CustomComparison>]
type Person(name: string, age: int) =
    member this.Name = name
    member this.Age = age
    
    override this.Equals(obj) =
        match obj with
        | :? Person as other -> this.Name = other.Name && this.Age = other.Age
        | _ -> false
    
    override this.GetHashCode() =
        hash (this.Name, this.Age)
    
    interface IComparable with
        member this.CompareTo(obj) =
            match obj with
            | :? Person as other ->
                let nameComparison = String.Compare(this.Name, other.Name)
                if nameComparison <> 0 then nameComparison
                else this.Age.CompareTo(other.Age)
            | _ -> failwith "Cannot compare with non-Person"
    
    interface IComparable<Person> with
        member this.CompareTo(other) =
            let nameComparison = String.Compare(this.Name, other.Name)
            if nameComparison <> 0 then nameComparison
            else this.Age.CompareTo(other.Age)
    
    override this.ToString() = $"{this.Name} ({this.Age})"

let people = [
    Person("Alice", 30)
    Person("Bob", 25)
    Person("Alice", 25)
    Person("Charlie", 35)
]

// Test equality
let alice1 = Person("Alice", 30)
let alice2 = Person("Alice", 30)
printfn "Alice1 = Alice2: %b" (alice1 = alice2)

// Test sorting
let sortedPeople = people |> List.sort
printfn "Sorted people:"
sortedPeople |> List.iter (printfn "  %O")
```

Custom equality and comparison enable objects to work with collections  
and sorting. The attributes `[<CustomEquality; CustomComparison>]` are  
required when implementing these interfaces.  

## Events in classes

Classes can define and raise events for the observer pattern.  

```F#
open System

type BankAccount(initialBalance: decimal) =
    let mutable balance = initialBalance
    let balanceChanged = Event<decimal>()
    let lowBalanceWarning = Event<string>()
    
    [<CLIEvent>]
    member this.BalanceChanged = balanceChanged.Publish
    
    [<CLIEvent>]
    member this.LowBalanceWarning = lowBalanceWarning.Publish
    
    member this.Balance = balance
    
    member this.Deposit(amount: decimal) =
        balance <- balance + amount
        balanceChanged.Trigger(balance)
    
    member this.Withdraw(amount: decimal) =
        if amount <= balance then
            balance <- balance - amount
            balanceChanged.Trigger(balance)
            
            if balance < 100.0m then
                lowBalanceWarning.Trigger($"Low balance warning: {balance}")
            true
        else
            false

let account = BankAccount(500.0m)

// Subscribe to events
account.BalanceChanged.Add(fun newBalance ->
    printfn "Balance changed to: %M" newBalance)

account.LowBalanceWarning.Add(fun warning ->
    printfn "WARNING: %s" warning)

// Trigger events
account.Deposit(100.0m)
account.Withdraw(400.0m)
account.Withdraw(150.0m)
```

Events provide a way for classes to notify interested parties about  
state changes. The `[<CLIEvent>]` attribute makes events compatible  
with .NET event conventions.  

## Builder pattern

The builder pattern constructs complex objects step by step.  

```F#
type EmailBuilder() =
    let mutable recipient = ""
    let mutable subject = ""
    let mutable body = ""
    let mutable attachments = []
    let mutable priority = "Normal"
    
    member this.To(address: string) =
        recipient <- address
        this
    
    member this.Subject(text: string) =
        subject <- text
        this
    
    member this.Body(content: string) =
        body <- content
        this
    
    member this.Attach(filename: string) =
        attachments <- filename :: attachments
        this
    
    member this.Priority(level: string) =
        priority <- level
        this
    
    member this.Build() =
        if recipient = "" then failwith "Recipient required"
        if subject = "" then failwith "Subject required"
        
        {| To = recipient
           Subject = subject
           Body = body
           Attachments = List.rev attachments
           Priority = priority |}

let email = 
    EmailBuilder()
        .To("user@example.com")
        .Subject("F# OOP Example")
        .Body("Hello from F#!")
        .Attach("document.pdf")
        .Attach("image.jpg")
        .Priority("High")
        .Build()

printfn "Email: %A" email
```

The builder pattern provides a fluent interface for constructing objects  
with many optional parameters. Each method returns the builder instance  
for method chaining.  

## Factory pattern

Factory patterns create objects without specifying exact classes.  

```F#
type ILogger =
    abstract member Log: string -> unit

type ConsoleLogger() =
    interface ILogger with
        member this.Log(message) = printfn "CONSOLE: %s" message

type FileLogger(filename: string) =
    interface ILogger with
        member this.Log(message) = 
            System.IO.File.AppendAllText(filename, $"{System.DateTime.Now}: {message}\n")

type LoggerFactory() =
    static member CreateLogger(loggerType: string, ?parameter: string) =
        match loggerType.ToLower() with
        | "console" -> ConsoleLogger() :> ILogger
        | "file" -> 
            let filename = parameter |> Option.defaultValue "default.log"
            FileLogger(filename) :> ILogger
        | _ -> failwith $"Unknown logger type: {loggerType}"

// Usage
let consoleLogger = LoggerFactory.CreateLogger("console")
let fileLogger = LoggerFactory.CreateLogger("file", "app.log")

consoleLogger.Log("Application started")
fileLogger.Log("Data processed")

// Factory with configuration
type LoggerConfig = { Type: string; Filename: string option; Level: string }

type ConfigurableLoggerFactory() =
    static member CreateLogger(config: LoggerConfig) =
        match config.Type.ToLower() with
        | "console" -> ConsoleLogger() :> ILogger
        | "file" -> 
            let filename = config.Filename |> Option.defaultValue "default.log"
            FileLogger(filename) :> ILogger
        | _ -> failwith $"Unknown logger type: {config.Type}"

let config = { Type = "file"; Filename = Some("system.log"; Level = "Info" }
let systemLogger = ConfigurableLoggerFactory.CreateLogger(config)
systemLogger.Log("System initialized")
```

Factory patterns encapsulate object creation logic and provide flexibility  
in choosing which concrete types to instantiate based on parameters.  

## Observer pattern

The observer pattern allows objects to notify multiple observers of  
state changes.  

```F#
type IObserver<'T> =
    abstract member Update: 'T -> unit

type IObservable<'T> =
    abstract member Subscribe: IObserver<'T> -> unit
    abstract member Unsubscribe: IObserver<'T> -> unit

type WeatherStation() =
    let mutable observers: IObserver<string * float * float> list = []
    let mutable temperature = 0.0
    let mutable humidity = 0.0
    let mutable location = ""
    
    interface IObservable<string * float * float> with
        member this.Subscribe(observer) =
            observers <- observer :: observers
            
        member this.Unsubscribe(observer) =
            observers <- observers |> List.filter (fun o -> not (obj.ReferenceEquals(o, observer)))
    
    member this.SetWeatherData(loc: string, temp: float, hum: float) =
        location <- loc
        temperature <- temp
        humidity <- hum
        this.NotifyObservers()
    
    member private this.NotifyObservers() =
        let data = (location, temperature, humidity)
        observers |> List.iter (fun observer -> observer.Update(data))

type CurrentConditionsDisplay() =
    interface IObserver<string * float * float> with
        member this.Update((location, temperature, humidity)) =
            printfn "Current conditions at %s: %.1f°C, %.1f%% humidity" location temperature humidity

type StatisticsDisplay() =
    let mutable temperatures: float list = []
    
    interface IObserver<string * float * float> with
        member this.Update((location, temperature, humidity)) =
            temperatures <- temperature :: temperatures
            let avg = List.average temperatures
            let min = List.min temperatures
            let max = List.max temperatures
            printfn "Statistics for %s: Avg=%.1f°C, Min=%.1f°C, Max=%.1f°C" location avg min max

let weatherStation = WeatherStation()
let currentDisplay = CurrentConditionsDisplay()
let statsDisplay = StatisticsDisplay()

let observable = weatherStation :> IObservable<string * float * float>

observable.Subscribe(currentDisplay :> IObserver<string * float * float>)
observable.Subscribe(statsDisplay :> IObserver<string * float * float>)

weatherStation.SetWeatherData("New York", 25.5, 65.0)
weatherStation.SetWeatherData("New York", 23.0, 70.0)
weatherStation.SetWeatherData("New York", 27.2, 60.0)
```

The observer pattern decouples subjects from observers, allowing dynamic  
subscription and notification of state changes.  

## Decorator pattern

The decorator pattern adds behavior to objects without altering their  
structure.  

```F#
type ICoffee =
    abstract member Cost: unit -> decimal
    abstract member Description: unit -> string

type SimpleCoffee() =
    interface ICoffee with
        member this.Cost() = 2.0m
        member this.Description() = "Simple coffee"

type CoffeeDecorator(coffee: ICoffee) =
    interface ICoffee with
        member this.Cost() = coffee.Cost()
        member this.Description() = coffee.Description()

type MilkDecorator(coffee: ICoffee) =
    inherit CoffeeDecorator(coffee)
    
    interface ICoffee with
        member this.Cost() = coffee.Cost() + 0.5m
        member this.Description() = coffee.Description() + ", milk"

type SugarDecorator(coffee: ICoffee) =
    inherit CoffeeDecorator(coffee)
    
    interface ICoffee with
        member this.Cost() = coffee.Cost() + 0.25m
        member this.Description() = coffee.Description() + ", sugar"

type ChocolateDecorator(coffee: ICoffee) =
    inherit CoffeeDecorator(coffee)
    
    interface ICoffee with
        member this.Cost() = coffee.Cost() + 0.75m
        member this.Description() = coffee.Description() + ", chocolate"

let coffee = SimpleCoffee() :> ICoffee
printfn "%s - $%.2f" (coffee.Description()) (coffee.Cost())

let coffeeWithMilk = MilkDecorator(coffee) :> ICoffee
printfn "%s - $%.2f" (coffeeWithMilk.Description()) (coffeeWithMilk.Cost())

let coffeeWithMilkAndSugar = SugarDecorator(coffeeWithMilk) :> ICoffee
printfn "%s - $%.2f" (coffeeWithMilkAndSugar.Description()) (coffeeWithMilkAndSugar.Cost())

let fancyCoffee = 
    SimpleCoffee()
    |> fun c -> MilkDecorator(c) :> ICoffee
    |> fun c -> SugarDecorator(c) :> ICoffee
    |> fun c -> ChocolateDecorator(c) :> ICoffee

printfn "%s - $%.2f" (fancyCoffee.Description()) (fancyCoffee.Cost())
```

The decorator pattern allows adding features to objects dynamically.  
Each decorator wraps the previous object and adds its own behavior.  

## Adapter pattern

The adapter pattern makes incompatible interfaces work together.  

```F#
// Existing class with incompatible interface
type LegacyPrinter() =
    member this.PrintOldFormat(text: string) =
        printfn "LEGACY: %s" (text.ToUpper())

// Target interface we want to use
type IModernPrinter =
    abstract member Print: string -> unit
    abstract member PrintWithFormat: string * string -> unit

// Adapter to make LegacyPrinter work with IModernPrinter
type PrinterAdapter(legacyPrinter: LegacyPrinter) =
    interface IModernPrinter with
        member this.Print(text: string) =
            legacyPrinter.PrintOldFormat(text)
            
        member this.PrintWithFormat(text: string, format: string) =
            let formattedText = 
                match format.ToLower() with
                | "bold" -> $"**{text}**"
                | "italic" -> $"*{text}*"
                | _ -> text
            legacyPrinter.PrintOldFormat(formattedText)

// Modern printer implementation
type ModernPrinter() =
    interface IModernPrinter with
        member this.Print(text: string) =
            printfn "MODERN: %s" text
            
        member this.PrintWithFormat(text: string, format: string) =
            let formattedText = 
                match format.ToLower() with
                | "bold" -> $"\u001b[1m{text}\u001b[0m"  // ANSI bold
                | "italic" -> $"\u001b[3m{text}\u001b[0m"  // ANSI italic
                | _ -> text
            printfn "MODERN: %s" formattedText

let legacyPrinter = LegacyPrinter()
let adapter = PrinterAdapter(legacyPrinter) :> IModernPrinter
let modernPrinter = ModernPrinter() :> IModernPrinter

let printers = [adapter; modernPrinter]

for printer in printers do
    printer.Print("Hello World")
    printer.PrintWithFormat("Important Message", "bold")
    printfn ""
```

The adapter pattern enables legacy code to work with new interfaces  
without modifying the original classes. It acts as a bridge between  
incompatible interfaces.  

## Template method pattern

The template method pattern defines algorithm structure in a base class  
while allowing subclasses to override specific steps.  

```F#
[<AbstractClass>]
type DataProcessor() =
    // Template method
    member this.ProcessData() =
        let data = this.ReadData()
        let processedData = this.TransformData(data)
        this.WriteData(processedData)
        this.Cleanup()
    
    // Abstract methods to be implemented by subclasses
    abstract member ReadData: unit -> string list
    abstract member TransformData: string list -> string list
    abstract member WriteData: string list -> unit
    
    // Hook method with default implementation
    abstract member Cleanup: unit -> unit
    default this.Cleanup() = printfn "Default cleanup performed"

type CSVProcessor(inputFile: string, outputFile: string) =
    inherit DataProcessor()
    
    override this.ReadData() =
        printfn "Reading CSV data from %s" inputFile
        ["Name,Age"; "Alice,30"; "Bob,25"; "Carol,35"]  // Simulated data
    
    override this.TransformData(data: string list) =
        printfn "Transforming CSV data"
        data |> List.map (fun line -> 
            if line.Contains(",") then
                let parts = line.Split(',')
                if parts.Length = 2 && parts.[0] <> "Name" then
                    $"{parts.[0]} is {parts.[1]} years old"
                else
                    line
            else
                line)
    
    override this.WriteData(data: string list) =
        printfn "Writing processed data to %s" outputFile
        data |> List.iter (printfn "  %s")

type JSONProcessor(apiUrl: string) =
    inherit DataProcessor()
    
    override this.ReadData() =
        printfn "Fetching JSON data from %s" apiUrl
        ["{\"name\":\"David\",\"age\":28}"; "{\"name\":\"Eve\",\"age\":32}"]  // Simulated
    
    override this.TransformData(data: string list) =
        printfn "Parsing and transforming JSON data"
        data |> List.map (fun json ->
            // Simplified JSON parsing simulation
            let name = json.Substring(json.IndexOf("\":\"") + 3, 
                                     json.IndexOf("\",") - json.IndexOf("\":\"") - 3)
            let ageStr = json.Substring(json.LastIndexOf(":") + 1, 
                                       json.LastIndexOf("}") - json.LastIndexOf(":") - 1)
            $"{name} (Age: {ageStr})")
    
    override this.WriteData(data: string list) =
        printfn "Outputting JSON-derived data to console"
        data |> List.iter (printfn "  %s")
    
    override this.Cleanup() =
        printfn "JSON processor cleanup: closing connections"

let csvProcessor = CSVProcessor("data.csv", "output.txt")
let jsonProcessor = JSONProcessor("https://api.example.com/users")

printfn "Processing CSV:"
csvProcessor.ProcessData()

printfn "\nProcessing JSON:"
jsonProcessor.ProcessData()
```

The template method pattern provides a framework for algorithms while  
allowing customization of specific steps. The base class controls the  
overall flow, and subclasses implement the details.  

## Sealed classes and methods

Sealed classes cannot be inherited, and sealed methods cannot be overridden.  

```F#
// Regular class that can be inherited
type Vehicle() =
    abstract member Start: unit -> string
    default this.Start() = "Vehicle started"
    
    virtual member GetType() = "Generic Vehicle"

// Sealed class - cannot be inherited
[<Sealed>]
type Car() =
    inherit Vehicle()
    
    override this.Start() = "Car engine started"
    
    // Sealed method - cannot be overridden by further inheritance
    abstract member Accelerate: unit -> string
    default this.Accelerate() = "Car is accelerating"
    
    override this.GetType() = "Car"

// This would cause a compilation error:
// type SportsCar() = inherit Car()  // Error: Car is sealed

// Non-sealed derived class
type Truck() =
    inherit Vehicle()
    
    override this.Start() = "Truck engine started"
    
    // This method can be overridden
    virtual member LoadCargo() = "Loading cargo"
    
    override this.GetType() = "Truck"

// Can inherit from Truck since it's not sealed
type DeliveryTruck() =
    inherit Truck()
    
    override this.Start() = "Delivery truck started"
    override this.LoadCargo() = "Loading packages for delivery"
    override this.GetType() = "Delivery Truck"

let vehicles = [
    Car() :> Vehicle
    Truck() :> Vehicle
    DeliveryTruck() :> Vehicle
]

for vehicle in vehicles do
    printfn "%s: %s" (vehicle.GetType()) (vehicle.Start())
```

Sealed classes and methods provide control over inheritance hierarchies.  
Use sealing to prevent further inheritance when the design is complete  
and shouldn't be extended.  

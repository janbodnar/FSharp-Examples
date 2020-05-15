open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    do
        this.ClientSize <- Size(350, 250)
        this.Title <- "First program"

        let layout = new StackLayout()

        let button = new Button(Text = "Quit")

        button.Click.Add(fun e -> Application.Instance.Quit())

        layout.Items.Add(new StackLayoutItem((button)))

        this.Content <- layout

[<EntryPoint;STAThread>]
let main argv =
    (new Application()).Run(new MyForm())
    0

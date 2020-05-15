open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    do
        base.ClientSize <- Size(600, 400)
        base.Title <- "CheckBox"

        let layout = new StackLayout()
        let button = new CheckBox(Text = "Show Title")
        button.Checked <- new Nullable<bool>(true)

        button.CheckedChanged.Add(fun e -> 

            let title = this.Title

            if title = String.Empty then
                this.Title <- "CheckBox"
            else
                this.Title <- ""
        )

        layout.Items.Add(new StackLayoutItem((button)))

        base.Content <- layout

[<EntryPoint;STAThread>]
let main argv = 
    (new Application()).Run(new MyForm())
    0        

open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    
    do
        this.ClientSize <- Size(600, 400)
        this.Title <- "ComboBox"

        let dropdown = new DropDown()
        let label = new Label(Text = "...")
        
        ["falcon"; "rock"; "sky"; "cloud"]  |> List.iter dropdown.Items.Add

        dropdown.SelectedValueChanged.Add(fun e -> 
            label.Text <- dropdown.SelectedValue.ToString()
            )

        let layout = 
            new TableLayout(
                Spacing = Size(15, 5), Padding = Padding(10)
            )
        [
            TableRow(
                TableCell(dropdown, true), 
                TableCell(label, false)
            )
            TableRow(ScaleHeight = true)
        ] |> List.iter layout.Rows.Add

        this.Content <- layout
        

[<EntryPoint;STAThread>]
let main argv = 
    (new Application()).Run(new MyForm())
    0

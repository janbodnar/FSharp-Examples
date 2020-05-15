open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    
    do
        this.ClientSize <- Size(600, 400)
        this.Title <- "DropDown"

        let dropdown = new DropDown()
        let label = new Label(Text = "cloud")
        
        ["falcon"; "rock"; "sky"; "cloud"]  |> List.iter dropdown.Items.Add
        dropdown.SelectedKey <- "cloud" 

        dropdown.SelectedValueChanged.Add(fun e -> 
            label.Text <- dropdown.SelectedValue.ToString()
            Console.WriteLine dropdown.SelectedValue
            Console.WriteLine dropdown.SelectedIndex

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

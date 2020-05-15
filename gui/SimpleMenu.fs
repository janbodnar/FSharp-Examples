open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    do
        base.ClientSize <- Size(600, 400)
        base.Title <- "Simple menu"


        base.Menu <- new MenuBar()

        let newCmd = new Command(MenuText = "New")
        newCmd.Shortcut <- Application.Instance.CommonModifier ||| Keys.N
        newCmd.Executed.Add(fun e -> MessageBox.Show(this, "New command") |> ignore)

        let quitCmd = new Command(MenuText = "Quit")
        quitCmd.Shortcut <- Application.Instance.CommonModifier ||| Keys.Q
        quitCmd.Executed.Add(fun e -> Application.Instance.Quit())        

        let fileItem = new ButtonMenuItem(Text = "&File")
        fileItem.Items.Add(newCmd) |> ignore
        fileItem.Items.Add(quitCmd) |> ignore

        base.Menu.Items.Add(fileItem)

[<EntryPoint;STAThread>]
let main argv = 
    (new Application()).Run(new MyForm())
    0        

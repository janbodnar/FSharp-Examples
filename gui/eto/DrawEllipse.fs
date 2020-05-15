open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    do
        base.ClientSize <- Size(600, 400)
        base.Title <- "Simple menu"

        let drawable = new Drawable()

        drawable.Paint.Add(fun e ->
            let g = e.Graphics
            g.TranslateTransform(10.f, 10.f)
            // g.DrawEllipse(Colors.Blue, 0.f, 0.f, 100.f, 100.f)
            g.FillEllipse(Colors.White, 0.f, 0.f, 100.f, 100.f)
        )

        this.Content <- drawable

[<EntryPoint;STAThread>]
let main argv =
    (new Application()).Run(new MyForm())
    0

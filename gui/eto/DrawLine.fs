open System
open Eto.Forms
open Eto.Drawing

type MyForm() as this =
    inherit Form()
    do
        base.ClientSize <- Size(600, 400)
        base.Title <- "Draw line"

        let drawable = new Drawable()

        drawable.Paint.Add(fun e ->

            let g = e.Graphics

            let p1 = PointF(10.f, 10.f)
            let p2 = PointF(250.f, 250.f)
            let col = Colors.White

            g.DrawLine(col, p1, p2)
        )

        this.Content <- drawable

[<EntryPoint;STAThread>]
let main argv =
    (new Application()).Run(new MyForm())
    0

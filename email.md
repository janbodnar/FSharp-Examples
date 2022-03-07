# Emails 

## Send simple mail with SmtpClient

```F#
open System.Net.Mail
open System.Net

let from = "from@example.com"
let _to = "to@example.com"
let subject = "Test mail"
let body = "Test body"
let username = "username" 
let password = "password" 
let host = "smtp.mailtrap.io"
let port = 2525

let sendMail = 

    use client = new SmtpClient(host, port)
    client.Credentials <- NetworkCredential(username, password)
    client.EnableSsl <- true

    client.Send(from, _to, subject, body)

    printf "Email sent"

printfn "%s", sendMail.GetType()
```

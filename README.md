# Clarity Emailer
Project Scenario for Clarity Ventures to prove my worth.

### Scenario
_You are being asked to build a working implement of a “client’s” feature request._

_The client has a high-volume application which must be able to send emails to customer._

_It is critical that a user not be interrupted or delayed while navigating the website simply because an
email fails to send – i.e. other code must be able to call a mail routine without waiting for a result._

_Your challenge is to address this problem with the following constraints:_

* Code should be written in C#.
* Send Email Method should be in a dll that can be reused throughout different applications and
entry points.
* Email sender, recipient, subject, and body (not attachments), and date must be logged/stored
indefinitely with status of send attempt.
* If email fails to send it should either be retried until success or a max of 3 times whichever
comes first, and can be sent in succession or over a period of time.
* Please store all credentials in an appsettings instead of hardcoded.
* At minimum that method/dll should be called from a console application.
* _Extra Credit_ if attached to an API that can be called from Postman.
* _**EXTRA Credit**_ if a front end (wpf/asp.net web application/etc...) calls the API to send the email.
* In any scenario you should be able to take in an input of a recipient email to send a test email. 

## Test & Usage

Clone the repository locally
```bash
git clone https://github.com/CrimsonOrion/ClarityEmailer.git
```
Run the API project:
```bash
cd clarityemailer\clarityemailer.api
dotnet run clarityemailer.api
```
Run the ASP.NET project:
```bash
cd clarityemailer\clarityemailer.ui.aspnet
dotnet run clarityemailerwebsite
```

Then you can go to the [ASP.NET page](https://localhost:7135)
<p align="left">
  <img src="https://www.crimsonorion.com/img/ClarityEmailer-ASPNET-Startpage.png" alt="ASP.NET UI Startup page" />
</p>

In the [ASP.NET page](https://localhost:7135), select either the [Library](https://localhost:7135/LibraryEmailer) or [API](https://localhost:7135/APIEmailer) link and enter an email address to send an email via Core Library or API call.

---
(You can also go to the [API page](https://localhost:7185/swagger) and look at the Swagger UI)
<p align="left">
  <img src="https://www.crimsonorion.com/img/ClarityEmailer-API-Swagger.png" alt="ASP.NET API Swagger page" />
</p>

---

Run the Console project:
```bash
cd clarityemailer\clarityemailer.ui.console
dotnet run clarityemailer -api -toaddress EMAIL@ADDRESS.COM
```

#### Options
* `-api`_optional_: Use the API to send email. *_Ignore this argument to send via Core Library_.
* `-toaddress`: The email address you want to send the message to. _*If you don't supply this argument, it will prompt you to supply an email address_.
---

Run the WPF project:
```bash
cd clarityemailer\clarityemailer.ui.wpf
dotnet run clarityemailerwpf
```

Clarity Emailer (WPF):
<p align="left">
  <img src="https://www.crimsonorion.com/img/ClarityEmailer-WPF-Start.png" alt="WPF Start page" />
</p>

Select either <img src="https://www.crimsonorion.com/img/ClarityEmailer-WPF-EmailAPI.png" alt="Email via API" /> or <img src="https://www.crimsonorion.com/img/ClarityEmailer-WPF-EmailLibrary.png" alt="Email via Library" /> to send via the desired method.
---

#### Test via Postman
You can also test via [Postman](https://www.postman.com) using this [collection](https://www.crimsonorion.com/img/ClarityEmailer.postman_collection.json).
<p align="left">
  <img src="https://www.crimsonorion.com/img/ClarityEmailer-Postman.png" alt="Postman image" />
</p>

### Hopes
I hope you guys like the project. If you have any questions, feel free to email me at crimsonorion@gmail.com. I look forward to talking to you guys about it!

### Planned implementations

* ~~ASP.NET MVC API~~
* ~~Console~~
* ~~WPF~~
* ~~ASP.NET Web Application~~
* ~~Fix logging~~

### Outside Dependencies

* [Library.NET.Emailer](https://www.nuget.org/packages/Library.NET.Emailer/)
* [Library.NET.Logging](https://www.nuget.org/packages/Library.NET.Logging/)
* [Prism.DryIoC](https://www.nuget.org/packages/Prism.DryIoC/)
* [Prism.WPF](https://www.nuget.org/packages/Prism.WPF/)
* [MahApps.Metro](https://www.nuget.org/packages/MahApps.Metro/)
* [MahApps.Metro.IconPacks](https://www.nuget.org/packages/MahApps.Metro.IconPacks/)

## License
[MIT](https://choosealicense.com/licenses/mit/)

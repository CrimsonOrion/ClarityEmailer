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

## Test

Clone the repository locally
```bash
git clone https://github.com/CrimsonOrion/ClarityEmailer.git
```

## Usage

_Coming Soon_

### Planned implementations

* ~~ASP.NET MVC API~~
* ~~Console~~
* ~~WPF~~
* ASP.NET Web Application

### Outside Dependencies

* [Library.NET.Emailer](https://www.nuget.org/packages/Library.NET.Emailer/)
* [Library.NET.Logging](https://www.nuget.org/packages/Library.NET.Logging/)
* [Prism.DryIoC](https://www.nuget.org/packages/Prism.DryIoC/)
* [Prism.WPF](https://www.nuget.org/packages/Prism.WPF/)
* [MahApps.Metro](https://www.nuget.org/packages/MahApps.Metro/)
* [MahApps.Metro.IconPacks](https://www.nuget.org/packages/MahApps.Metro.IconPacks/)

## License
[MIT](https://choosealicense.com/licenses/mit/)

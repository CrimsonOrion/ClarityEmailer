namespace ClarityEmailer.Core.Models;
public class EmailConfigOptionsModel
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string Password { get; set; }
}
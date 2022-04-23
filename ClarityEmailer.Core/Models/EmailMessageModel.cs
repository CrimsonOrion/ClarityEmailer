namespace ClarityEmailer.Core.Models;
public class EmailMessageModel
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime Sent { get; set; }
}
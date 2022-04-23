using ClarityEmailer.API.Models;
using ClarityEmailer.Core;
using ClarityEmailer.Core.Models;
using ClarityEmailer.Core.Processors;

using FluentEmail.Core.Models;

using Library.NET.Logging;

using Microsoft.AspNetCore.Mvc;

using System.Text;

namespace ClarityEmailer.API.Controllers;
[Route("[controller]")]
[ApiController]
public class SendEmailController : ControllerBase
{
    private readonly ICustomLogger _logger;
    private readonly IEmailProcessor _emailer;

    public SendEmailController(ICustomLogger logger, IEmailProcessor emailer)
    {
        _logger = logger;
        _emailer = emailer;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        using StreamReader streamReader = new(_logger.LogFileInfo.FullName, Encoding.UTF8);
        var text = await streamReader.ReadToEndAsync();
        return text;
    }

    [HttpPost]
    public async Task<ActionResult<EmailMessageModel>> Post(EmailMessageDTO model)
    {
        EmailMessageModel message = new()
        {
            FromAddress = GlobalConfig.EmailConfig.SenderEmail,
            ToAddress = model.ToAddress,
            Subject = "Test email",
            Body = "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
        };
        try
        {
            SendResponse result = await _emailer.SendEmailAsync(message);

            if (result.Successful)
            {
                message.Sent = DateTime.UtcNow;
                return message;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occured during post.");
        }

        return BadRequest();
    }
}
using ClarityEmailer.API.Models;
using ClarityEmailer.Core;
using ClarityEmailer.Core.Models;
using ClarityEmailer.Core.Processors;

using FluentEmail.Core.Models;

using Library.NET.Logging;

using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace ClarityEmailer.API.Controllers;
[Route("[controller]")]
[ApiController]
public class SendEmailController : ControllerBase
{
    private readonly ILogger<SendEmailController> _logger;
    private readonly IEmailProcessor _emailer;
    private readonly IConfiguration _configuration;
    private readonly ICustomLogger _customLogger;

    public SendEmailController(ILogger<SendEmailController> logger, IEmailProcessor emailer, IConfiguration configuration, ICustomLogger customLogger)
    {
        _logger = logger;
        _emailer = emailer;
        _configuration = configuration;
        _customLogger = customLogger;
    }

    [HttpPost]
    public async Task<ActionResult<EmailMessageModel>> Post(EmailMessageDTO model)
    {
        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = _configuration["Email Settings:Smtp Server"],
            SmtpPort = Convert.ToInt16(_configuration["Email Settings:Smtp Port"]),
            SenderEmail = _configuration["Email Settings:Sender Email"],
            Password = _configuration["Email Settings:Password"],
        };

        EmailMessageModel message = new()
        {
            FromAddress = GlobalConfig.EmailConfig.SenderEmail,
            ToAddress = model.ToAddress,
            Subject = "Test email",
            Body = "<p>Hello from the API Send Email Controller!</p><p>-Jim Lynch</p>"
        };

        try
        {
            SendResponse result = await _emailer.SendEmailAsync(message);

            if (result.Successful)
            {
                message.Sent = DateTime.UtcNow;
                message.Result = "Success";
                var json = JsonSerializer.Serialize(message);
                _logger.LogInformation("{json}", json);
                _customLogger.LogInformation(json);
                return message;
            }
        }
        catch (Exception ex)
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogError(ex, "{json}", json);
            _customLogger.LogError(ex, json);
        }

        return BadRequest();
    }
}
using ClarityEmailer.API.Logging;
using ClarityEmailer.API.Models;
using ClarityEmailer.Core;
using ClarityEmailer.Core.Models;
using ClarityEmailer.Core.Processors;

using FluentEmail.Core.Models;

using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json;

namespace ClarityEmailer.API.Controllers;
[Route("[controller]")]
[ApiController]
public class SendEmailController : ControllerBase
{
    private readonly ILogger<SendEmailController> _logger;
    private readonly IEmailProcessor _emailer;
    private readonly IConfiguration _configuration;
    private readonly string _logFilename;

    public SendEmailController(ILogger<SendEmailController> logger, IEmailProcessor emailer, IConfiguration configuration)
    {
        _logger = logger;
        _emailer = emailer;
        _configuration = configuration;

        _logFilename = configuration["Log File Name"];

        LogToFile.FilePath = _logFilename;
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
                await LogToFile.WriteEntryAsync(json);
                return message;
            }
        }
        catch (Exception ex)
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogError(ex, "{json}", json);
            await LogToFile.WriteEntryAsync(json);
        }

        return BadRequest();
    }
}
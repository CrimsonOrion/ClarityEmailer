using ClarityEmailer.Core;
using ClarityEmailer.Core.Models;
using ClarityEmailer.Core.Processors;

using Library.NET.Logging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;

namespace ClarityEmailer.UI.ASPNET.Pages;
public class LibraryEmailerModel : PageModel
{
    private readonly ILogger<LibraryEmailerModel> _logger;
    private readonly ICustomLogger _customLogger;
    private readonly IConfiguration _config;
    private readonly IEmailProcessor _emailProcessor;

    [BindProperty]
    [RegularExpression(@$"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage = "The email address is not valid.")]
    // [EmailAddress] doesn't consider abc@abc an invalid email. This regex requires the .blah part.
    public string ToAddress { get; set; }

    public string Status { get; private set; }

    public LibraryEmailerModel(ILogger<LibraryEmailerModel> logger, IConfiguration config, IEmailProcessor emailProcessor, ICustomLogger customLogger)
    {
        _logger = logger;
        _customLogger = customLogger;
        _config = config;
        _emailProcessor = emailProcessor;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrEmpty(ToAddress))
        {
            return Page();
        }

        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = _config["Email Settings:Smtp Server"] ?? "",
            SmtpPort = Convert.ToInt16((_config["Email Settings:Smtp Port"]) ?? "25"),
            SenderEmail = _config["Email Settings:Sender Email"] ?? "",
            Password = _config["Email Settings:Password"] ?? ""
        };

        EmailMessageModel message = new()
        {
            FromAddress = GlobalConfig.EmailConfig.SenderEmail,
            ToAddress = ToAddress.Trim(),
            Subject = "Test Email",
            Body = "<p>Hello from the ASP.NET email message via Core Library!</p><p>-Jim Lynch</p>"
        };

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var statusMessage = $"Sending Email to {message.ToAddress}";

        _logger.LogInformation(statusMessage);
        _customLogger.LogInformation(statusMessage);

        Status = statusMessage;
        FluentEmail.Core.Models.SendResponse result = await _emailProcessor.SendEmailAsync(message);

        if (result.Successful)
        {
            statusMessage = "Message Sent Successfully";
            Status = statusMessage;
            _logger.LogInformation(statusMessage);
            _customLogger.LogInformation(statusMessage);
            return Page();
        }

        statusMessage = "Email did not send correctly. Check logs for more information.";
        Status = statusMessage;
        _logger.LogError(statusMessage);
        _customLogger.LogError(new("Email failed to send"), statusMessage);
        return Page();
    }
}
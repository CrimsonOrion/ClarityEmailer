using Library.NET.Logging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ClarityEmailer.UI.ASPNET.Pages;

public class APIEmailerModel : PageModel
{
    private readonly ILogger<APIEmailerModel> _logger;
    private readonly ICustomLogger _customLogger;
    private readonly IConfiguration _config;

    [BindProperty]
    [RegularExpression(@$"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage = "The email address is not valid.")]
    // [EmailAddress] doesn't consider abc@abc an invalid email. This regex requires the .blah part.
    public string ToAddress { get; set; }

    public string Status { get; private set; }

    public APIEmailerModel(ILogger<APIEmailerModel> logger, IConfiguration config, ICustomLogger customLogger)
    {
        _logger = logger;
        _customLogger = customLogger;
        _config = config;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrEmpty(ToAddress))
        {
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var statusMessage = $"Sending Email to {ToAddress}";

        _logger.LogInformation(statusMessage);
        Status = statusMessage;

        // Send email via API
        Status = await SendEmailThroughAPI();
        return Page();
    }

    private async Task<string> SendEmailThroughAPI()
    {
        var message = new { ToAddress = ToAddress.Trim() };
        StringContent? data = new(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

        var port = _config["Port List:Release"];

#if DEBUG
        port = _config["Port List:Debug"];
#endif

        var url = $"https://localhost:{port}/SendEmail";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("XApiKey", _config["XApiKey"]);

        var result = string.Empty;

        try
        {
            HttpResponseMessage response = await client.PostAsync(url, data);
            result = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError(result);
                _customLogger.LogError(new($"Status Code: {response.StatusCode}"), result);
                return result;
            }
        }
        catch (Exception ex)
        {
            result = ex.Message;
            _logger.LogError(result);
            _customLogger.LogError(ex, $"Attempted to send email to {ToAddress}, but it failed with the following error:\r\n{result}");
            return result;
        }

        _logger.LogInformation(result);
        _customLogger.LogInformation(result);
        return result;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace ClarityEmailer.UI.ASPNET.Pages;

public class APIEmailerModel : PageModel
{
    private readonly ILogger<APIEmailerModel> _logger;
    private readonly IConfiguration _config;

    [BindProperty]
    [RegularExpression(@$"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage = "The email address is not valid.")]
    // [EmailAddress] doesn't consider abc@abc an invalid email. This regex requires the .blah part.
    public string ToAddress { get; set; }

    public string Status { get; private set; }

    public APIEmailerModel(ILogger<APIEmailerModel> logger, IConfiguration config)
    {
        _logger = logger;
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

        var url = "https://localhost:7185/SendEmail";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("XApiKey", _config["XApiKey"]);
        HttpResponseMessage response = await client.PostAsync(url, data);

        var result = response.Content.ReadAsStringAsync().Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError(result);
            return result;
        }

        _logger.LogInformation(result);
        return result;
    }
}
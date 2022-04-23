namespace ClarityEmailer.UI.Console;
public class App : IApp
{
    private readonly ICustomLogger _logger;
    private readonly IEmailProcessor _emailProcessor;

    public App(ICustomLogger logger, IEmailProcessor emailProcessor)
    {
        _logger = logger;
        _emailProcessor = emailProcessor;
    }

    public async Task RunLibraryAsync(EmailMessageModel model)
    {
        if (string.IsNullOrEmpty(model.ToAddress))
        {
            _logger.LogError(new("Missing Parameter"), "Missing -toAddress parameter.");
            return;
        }

        FluentEmail.Core.Models.SendResponse result = await _emailProcessor.SendEmailAsync(model);

        if (result.Successful)
        {
            _logger.LogInformation("Email Sent Successfully");
        }
    }

    public async Task RunAPIAsync(EmailMessageModel model)
    {
        var json = JsonSerializer.Serialize(model);
        StringContent? data = new(json, Encoding.UTF8, "application/json");

        var url = "https://localhost:7185/SendEmail";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("XApiKey", GlobalConfig.XApiKey.XApiKey);
        HttpResponseMessage response = await client.PostAsync(url, data);

        var result = response.Content.ReadAsStringAsync().Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError(new("Error with request"), result);
            return;
        }

        _logger.LogInformation(result);
    }
}
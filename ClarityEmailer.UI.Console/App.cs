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
        FluentEmail.Core.Models.SendResponse result = await _emailProcessor.SendEmailAsync(model);

        if (result.Successful)
        {
            _logger.LogInformation("Email Sent Successfully");
        }
    }

    public async Task RunAPIAsync(EmailMessageModel model)
    {
        string json = JsonSerializer.Serialize(model);
        StringContent? data = new(json, Encoding.UTF8, "application/json");

        string url = "https://localhost:7179/SendEmail";
        using HttpClient client = new();

        HttpResponseMessage response = await client.PostAsync(url, data);

        string result = response.Content.ReadAsStringAsync().Result;
        _logger.LogInformation(result);
    }
}
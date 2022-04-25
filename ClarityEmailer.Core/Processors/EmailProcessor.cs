namespace ClarityEmailer.Core.Processors;
public class EmailProcessor : IEmailProcessor
{
    private readonly ICustomLogger _logger;

    public EmailProcessor(ICustomLogger logger) => _logger = logger;

    public async Task<SendResponse> SendEmailAsync(EmailMessageModel model)
    {
        var attempts = 0;
        SendResponse result = null;
        StringBuilder stringBuilder = new();

        stringBuilder
            .AppendLine($"From: {model.FromAddress}")
            .AppendLine($"To: {model.ToAddress}")
            .AppendLine($"Subject: {model.Subject}")
            .AppendLine($"Body: {model.Body}");

        while (attempts < 3)
        {
            attempts++;
            try
            {
                result = await GetEmailResponse(model);

                if (result.Successful)
                {
                    stringBuilder.AppendLine($"Sent: {DateTime.UtcNow}");
                    stringBuilder.AppendLine($"Result: Success");
                    _logger.LogInformation(stringBuilder.ToString());
                    return result;
                }

                StringBuilder errorStringBuilder = new();

                foreach (var err in result.ErrorMessages)
                {
                    errorStringBuilder.AppendLine(err);
                }

                var errorMessage = "Email Failed to Send.";

                errorMessage += attempts < 3 ? $" Resending after {attempts} second(s)." : " No more retries remaining.";

                _logger.LogError(new(errorMessage), $"{stringBuilder}\r\n{errorStringBuilder}");
                Thread.Sleep(1000 * attempts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured during post.");
            }
        }

        return result;
    }

    private static async Task<SendResponse> GetEmailResponse(EmailMessageModel model)
    {
        IEmailer emailer = new FluentEmailerMailKit
        {
            SmtpOptions = new()
            {
                Server = GlobalConfig.EmailConfig.SmtpServer,
                Port = GlobalConfig.EmailConfig.SmtpPort,
                User = GlobalConfig.EmailConfig.SenderEmail,
                Password = GlobalConfig.EmailConfig.Password,
                SocketOptions = MailKit.Security.SecureSocketOptions.Auto,
                RequiresAuthentication = true
            },
            EmailOptions = new()
            {
                ToAddresses = new List<AddressModel>() { new AddressModel(model.ToAddress) },
                ReplyToAddress = new AddressModel(model.FromAddress),
                FromAddress = new AddressModel(model.FromAddress),
                Subject = model.Subject,
                Body = model.Body,
                IsBodyHTML = true
            }
        };

        SendResponse result = await emailer.SetOptions().SendEmailAsync();

        return result;
    }
}
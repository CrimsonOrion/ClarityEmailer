namespace ClarityEmailer.Core.Processors;
public interface IEmailProcessor
{
    Task<SendResponse> SendEmailAsync(EmailMessageModel model);
}
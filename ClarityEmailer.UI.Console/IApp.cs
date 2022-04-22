
namespace ClarityEmailer.UI.Console;

public interface IApp
{
    Task RunAPIAsync(EmailMessageModel model);
    Task RunLibraryAsync(EmailMessageModel model);
}
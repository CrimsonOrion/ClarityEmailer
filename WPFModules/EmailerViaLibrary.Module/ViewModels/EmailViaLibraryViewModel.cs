using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmailerViaLibrary.Module.ViewModels;
public class EmailViaLibraryViewModel : BindableBase, INavigationAware
{
    #region Email Via Library View Properties

    private string _emailAddress;
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly ICustomLogger _logger;
    private readonly IEmailProcessor _emailProcessor;

    public static string Title => "Send Email via Library";

    public string EmailAddress
    {
        get => _emailAddress;
        set => SetProperty(ref _emailAddress, value.Trim());
    }

    #endregion

    #region Delegate Commands

    public DelegateCommand SendEmailCommand => new(SendEmail);

    #endregion

    #region Constructor

    public EmailViaLibraryViewModel(IDialogCoordinator dialogCoordinator, ICustomLogger logger, IEmailProcessor emailProcessor)
    {
        _dialogCoordinator = dialogCoordinator;
        _logger = logger;
        _emailProcessor = emailProcessor;
    }

    #endregion

    #region Methods

    #region Navigation

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
    public void OnNavigatedTo(NavigationContext navigationContext) { }

    #endregion

    #region Private

    private async void SendEmail()
    {
        if (await _dialogCoordinator.ShowMessageAsync(this, "Confirm", $"Send email to {EmailAddress}?", MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Negative)
        {
            return;
        }

        var title = "Please wait...";
        var message = "Starting Email Processor...";
        ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, title, message, true);
        controller.SetMessage(message);
        controller.SetIndeterminate();
        try
        {
            using CancellationTokenSource _tokensource = new();
            controller.Canceled += (object? sender, EventArgs e) =>
            {
                message = "Cancelling";
                controller.SetMessage(message);
                _tokensource.Cancel();
            };

            // Send Email
            await Task.Run(() => Thread.Sleep(100));

            SendResponse result = await _emailProcessor.SendEmailAsync(new()
            {
                ToAddress = EmailAddress,
                FromAddress = GlobalConfig.EmailConfig.SenderEmail,
                Subject = "Test Email",
                Body = "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
            });

            message = "Sending email...";
            controller.SetMessage(message);
            await Task.Run(() => Thread.Sleep(100));

            title = "Completed";
            message = result.Successful ? $"Email successfully sent to {EmailAddress}" : $"Email was not sent successfully sent to {EmailAddress}. Please check the log for more information.";

        }
        catch (OperationCanceledException cancelEx)
        {
            title = "Cancelled";
            message = cancelEx.Message;
        }
        catch (Exception ex)
        {
            title = "Error";
            message = $"There is a problem sending the email.\r\n\r\n{ex.Message}";
            _logger.LogError(ex, "Problem in SendEmail()");
        }
        finally
        {
            await controller.CloseAsync();
            await _dialogCoordinator.ShowMessageAsync(this, title, message);
        }
    }

    #endregion

    #endregion
}
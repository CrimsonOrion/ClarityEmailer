using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmailerViaAPI.Module.ViewModels;
public class EmailViaAPIViewModel : BindableBase, INavigationAware
{
    #region Email Via API View Properties

    private string _emailAddress;
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly ICustomLogger _logger;

    public static string Title => "Send Email via API";

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

    public EmailViaAPIViewModel(IDialogCoordinator dialogCoordinator, ICustomLogger logger)
    {
        _dialogCoordinator = dialogCoordinator;
        _logger = logger;
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
        var message = "Connecting to API...";
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

            var successful = await SendEmailThroughAPI();

            message = "Sending email...";
            controller.SetMessage(message);
            await Task.Run(() => Thread.Sleep(100));

            title = "Completed";
            message = successful ? $"Email successfully sent to {EmailAddress}" : $"Email was not sent successfully sent to {EmailAddress}. Please check the API log for more information.";

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
        }
        finally
        {
            await controller.CloseAsync();
            await _dialogCoordinator.ShowMessageAsync(this, title, message);
        }
    }

    private async Task<bool> SendEmailThroughAPI()
    {
        var json = JsonSerializer.Serialize(new
        {
            ToAddress = EmailAddress,
            FromAddress = GlobalConfig.EmailConfig.SenderEmail,
            Subject = "Test Email",
            Body = "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
        });
        StringContent? data = new(json, Encoding.UTF8, "application/json");

        var url = "https://localhost:7185/SendEmail";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("XApiKey", GlobalConfig.XApiKey.XApiKey);
        HttpResponseMessage response = await client.PostAsync(url, data);

        var result = response.Content.ReadAsStringAsync().Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError(new("Error with request"), result);
            return false;
        }

        _logger.LogInformation(result);
        return true;
    }

    #endregion

    #endregion
}
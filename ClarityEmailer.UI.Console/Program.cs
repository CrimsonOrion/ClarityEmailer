using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClarityEmailer.UI.Console;
internal class Program
{
    private static IServiceProvider _serviceProvider;

    private static async Task Main(string[] args)
    {
        RegisterServices();

        IApp app = _serviceProvider.GetService<IApp>();

        await app.RunLibraryAsync(CreateModel(args));

        DisposeServices();
    }

    private static void RegisterServices()
    {
        ICustomLogger logger = new CustomLogger(new("ConsoleLog.txt"), true, LogLevel.Information);

        string settingsFile = "appSettings.json";

#if DEBUG
        settingsFile = "appSettings.Development.json";
#endif


        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(settingsFile, false, true)
            .Build();

        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = configuration["Email Settings:Smtp Server"],
            SmtpPort = Convert.ToInt16(configuration["Email Settings:Smtp Port"]),
            SenderEmail = configuration["Email Settings:Sender Email"],
            Password = configuration["Email Settings:Password"]
        };

        IServiceCollection collection = new ServiceCollection()
            .AddScoped<IApp, App>()
            .AddScoped<IEmailProcessor, EmailProcessor>()

            .AddSingleton(logger)
            ;


        _serviceProvider = collection.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        if (_serviceProvider is not null and IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static EmailMessageModel CreateModel(string[] args)
    {
        if (args.Length == 0)
        {
            return new EmailMessageModel
            {
                FromAddress = GlobalConfig.EmailConfig.SenderEmail,
                ToAddress = "crimsonorion@gmail.com",
                Subject = "Test Email",
                Body = "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
            };
        }

        // -fromAddress claritytest@crimsonorion.com
        // -toAddress crimsonorion@gmail.com
        // -subject "Test Email"
        // -body "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
        EmailMessageModel model = new();
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-fromAddress":
                    model.FromAddress = args[++i];
                    break;
                case "-toAddress":
                    model.ToAddress = args[++i];
                    break;
                case "-subject":
                    model.Subject = args[++i];
                    break;
                case "-body":
                    model.Body = args[++i];
                    break;
                default:
                    break;
            }
        }

        return model;
    }
}
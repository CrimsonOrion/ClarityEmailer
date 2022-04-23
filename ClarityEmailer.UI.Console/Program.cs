using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClarityEmailer.UI.Console;
internal class Program
{
    private static IServiceProvider _serviceProvider;

    /// <summary>
    /// Main method for ClarityEmailer
    /// </summary>
    /// <param name="args">acceptable args are -api: (calls API for sendmail); [emailaddress]: sends the email to the specified email address</param>
    private static async Task Main(string[] args)
    {
        var runAPI = false;

        if (args.Contains("-api"))
        {
            runAPI = true;
        }

        // If -toaddress isn't present, give the user the option to add an email address.
        if (!args.Contains("-toaddress"))
        {
            System.Console.WriteLine("Please enter recipient address:");
            args = new string[] { "-toaddress", System.Console.ReadLine() };
        }

        RegisterServices();

        IApp app = _serviceProvider.GetService<IApp>();

        Task task = runAPI ? app.RunAPIAsync(CreateModel(args)) : app.RunLibraryAsync(CreateModel(args));

        await task;

        System.Console.WriteLine("Press any key to close application...");
        System.Console.ReadKey();

        DisposeServices();
    }

    private static void RegisterServices()
    {
        ICustomLogger logger = new CustomLogger(new("ConsoleLog.txt"), true, LogLevel.Information);

        var settingsFile = "appSettings.json";

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
        GlobalConfig.XApiKey = new()
        {
            XApiKey = configuration["XApiKey"]
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
        EmailMessageModel model = new()
        {
            FromAddress = GlobalConfig.EmailConfig.SenderEmail,
            Subject = "Test Email",
            Body = "<p>Hello from the email message!</p><p>-Jim Lynch</p>"
        };

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i]?.ToLower() == "-toaddress")
            {
                model.ToAddress = args[++i];
                break;
            }
        }

        return model;
    }
}
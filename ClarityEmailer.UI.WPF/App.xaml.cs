using System;
using System.Windows;

namespace ClarityEmailer.UI.WPF;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
        ThemeManager.Current.SyncTheme();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Set up the custom logger to go to Desktop
        ICustomLogger logger = new CustomLogger(new("WPFEmailer.log"), true, LogLevel.Information);

        var appSettingsFilename = "appSettings.json";

#if DEBUG
        appSettingsFilename = "appSettings.Development.json";
#endif

        // Set up the configuration using the appSettings.json file.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsFilename, false, true)
            .Build();


        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = configuration["Email Settings:Smtp Server"] ?? "",
            SmtpPort = Convert.ToInt16((configuration["Email Settings:Smtp Port"]) ?? "25"),
            SenderEmail = configuration["Email Settings:Sender Email"] ?? "",
            Password = configuration["Email Settings:Password"] ?? ""
        };

        GlobalConfig.XApiKey = new()
        {
            XApiKey = configuration["XApiKey"]
        };

        GlobalConfig.Ports = new()
        {
            DebugPort = Convert.ToInt16(configuration["Port List:Debug"]),
            ReleasePort = Convert.ToInt16(configuration["Port List:Release"])
        };

        containerRegistry
            .RegisterInstance(logger)
            .RegisterInstance<IDialogCoordinator>(new DialogCoordinator())

            .Register<IEmailProcessor, EmailProcessor>()
            ;
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog
        .AddModule<AboutModule>()
        .AddModule<EmailerViaAPIModule>()
        .AddModule<EmailerViaLibraryModule>();
}
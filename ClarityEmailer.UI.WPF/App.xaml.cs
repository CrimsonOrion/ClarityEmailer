using ClarityEmailer.Core;
using ClarityEmailer.Core.Processors;

using ControlzEx.Theming;

using Library.NET.Logging;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Configuration;

using Prism.Ioc;

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
        ICustomLogger logger = new CustomLogger(new("WPFLog.txt"), true, LogLevel.Information);

        // Set up the configuration using the appSettings.json file.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", false, true)
            .Build();


        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = configuration["Email Settings:Smtp Server"] ?? "",
            SmtpPort = Convert.ToInt16((configuration["Email Settings:Smtp Port"]) ?? "25"),
            SenderEmail = configuration["Email Settings:Sender Email"] ?? "",
            Password = configuration["Email Settings:Password"] ?? ""
        };

        containerRegistry
            .RegisterInstance(logger)

            .RegisterInstance<IDialogCoordinator>(new DialogCoordinator())

            .Register<IEmailProcessor, EmailProcessor>()
            ;
    }

    //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog
    //        .AddModule<CustomerServiceModule>()
    //        .AddModule<MarketingModule>()
    //        .AddModule<SupportServicesModule>()
    //        .AddModule<AccountingModule>()
    //        .AddModule<AboutModule>();
}
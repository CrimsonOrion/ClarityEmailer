using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ClarityEmailer.UI.WPF;
public class MainWindowViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    #region Main Window Properties

    public static string Title => $"Clarity Emailer v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

    #endregion

    #region Delegate Commands

    #region Navigation

    #region Emailer

    public DelegateCommand EmailerScreenCommand => new(EmailerScreen);

    private void EmailerScreen() => Navigate("EmailerView");

    #endregion

    #region About

    public DelegateCommand AboutScreenCommand => new(AboutScreen);

    private void AboutScreen() => Navigate("AboutView");

    #endregion

    #endregion

    #endregion

    #region Constructor

    public MainWindowViewModel(IRegionManager regionManager) => _regionManager = regionManager;

    #endregion

    #region Private Methods

    private void Navigate(string navigationPath, NavigationParameters navigationParameters = null) => _regionManager.RequestNavigate("MainRegion", navigationPath, navigationParameters);

    #endregion
}
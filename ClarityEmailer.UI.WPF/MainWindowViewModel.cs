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

    #region API

    public DelegateCommand EmailViaAPIScreenCommand => new(EmailViaAPIScreen);

    private void EmailViaAPIScreen() => Navigate(KnownViewNames.EmailViaAPIView);

    #endregion

    #region Library

    public DelegateCommand EmailViaLibraryScreenCommand => new(EmailViaLibraryScreen);

    private void EmailViaLibraryScreen() => Navigate(KnownViewNames.EmailViaLibraryView);

    #endregion

    #endregion

    #region About

    public DelegateCommand AboutScreenCommand => new(AboutScreen);

    private void AboutScreen() => Navigate(KnownViewNames.AboutView);

    #endregion

    #endregion

    #endregion

    #region Constructor

    public MainWindowViewModel(IRegionManager regionManager) => _regionManager = regionManager;

    #endregion

    #region Private Methods

    private void Navigate(string navigationPath, NavigationParameters navigationParameters = null) => _regionManager.RequestNavigate(KnownRegionNames.MainRegion, navigationPath, navigationParameters);

    #endregion
}
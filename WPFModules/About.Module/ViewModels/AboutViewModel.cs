using Prism.Mvvm;
using Prism.Regions;

using System;

namespace About.Module.ViewModels;
public class AboutViewModel : BindableBase, INavigationAware
{
    #region About View Properties

    private string _message = $"Welcome, {Environment.UserName[..1].ToUpper()}{Environment.UserName[1..].ToLower()}!\r\nWhat would you like to do today?";
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    private string _patchNotes;
    public string PatchNotes
    {
        get => _patchNotes;
        set => SetProperty(ref _patchNotes, value);
    }

    #endregion

    #region Constructor

    public AboutViewModel() => PatchNotes = "PatchNotes.txt";

    #endregion

    #region Methods

    #region Navigation

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
    public void OnNavigatedTo(NavigationContext navigationContext) { }

    #endregion

    #endregion
}
namespace EmailerViaLibrary.Module;
public class EmailerViaLibraryModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider) { }
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterForNavigation<EmailViaLibraryView>();
}
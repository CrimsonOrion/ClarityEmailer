namespace About.Module;
public class AboutModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider) { }
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterForNavigation<AboutView>();
}
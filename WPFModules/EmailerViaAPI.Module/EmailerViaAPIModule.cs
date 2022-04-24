namespace EmailerViaAPI.Module;
public class EmailerViaAPIModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider) { }
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterForNavigation<EmailViaAPIView>();
}
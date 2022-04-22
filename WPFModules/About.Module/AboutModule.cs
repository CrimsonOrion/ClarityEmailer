using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using System;
using System.Reflection;

namespace About.Module;
public class AboutModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider) => containerProvider
            .Resolve<IRegionManager>()
            .RegisterViewWithRegion("MainRegion", typeof(AboutView));
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterForNavigation<AboutView>();
}
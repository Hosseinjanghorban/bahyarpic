using Prism.Ioc;
using sample_project.Data_access;
using sample_project.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace sample_project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<signuppage>();
            containerRegistry.RegisterForNavigation<loginpage>();
            containerRegistry.RegisterForNavigation<homepage>();
            containerRegistry.RegisterForNavigation<MainWindow>();
            containerRegistry.RegisterForNavigation<home_page>();
            containerRegistry.RegisterSingleton<ISharedDataService, shareddata_service>();


        }
    }
}

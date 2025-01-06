using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Conference.Client.Views;
using Conference.Client.WebServices;
using Conference.Core.WebServices;
using Microsoft.Extensions.Configuration;
using Prism.DryIoc;
using Prism.Ioc;
using System.IO;
using System.Net.Http;

namespace Conference.Client
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            // Initializes Prism.Avalonia - DO NOT REMOVE
            base.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.MainWindow = new MainWindow();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                singleViewLifetime.MainView = new MainView();
            }

#if DEBUG
            this.AttachDevTools();
#endif
            base.OnFrameworkInitializationCompleted();
        }

        protected override AvaloniaObject CreateShell()
            => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<EnterView>();
            containerRegistry.RegisterForNavigation<UserSpaceView>();
            containerRegistry.RegisterForNavigation<CreationNewUserView>();

            containerRegistry.RegisterSingleton<HttpClientService>();

            containerRegistry.Register<IConfiguration>(GetConfiguration);
            containerRegistry.Register<IConnectionService, ConnectionService>();
        }

        private IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}
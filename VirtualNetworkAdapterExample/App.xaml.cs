using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtualNetworkAdapterExample.Commands;
using VirtualNetworkAdapterExample.Services;
using VirtualNetworkAdapterExample.ViewModels;

namespace VirtualNetworkAdapterExample
{
    public partial class App : Application
    {
        private readonly IHost _host;
        
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ResourceService>();
                    services.AddSingleton<HotspotService>(CreateHotspot);

                    services.AddSingleton<CreateDeviceCommand>();
                    services.AddSingleton<RemoveDeviceCommand>();
                    services.AddSingleton<UpdateDeviceListCommand>();
                    
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                    
                })
                .Build();
        }

        private HotspotService CreateHotspot(IServiceProvider service)
        {
            var resourcesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "devcon");
            var resourceService = service.GetRequiredService<ResourceService>();
            return new HotspotService(resourceService, resourcesDirectory);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            
            base.OnExit(e);
        }
    }
}
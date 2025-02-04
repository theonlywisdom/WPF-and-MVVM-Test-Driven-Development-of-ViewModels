﻿using FriendStorage.DataAccess;
using FriendStorage.UI.Dialogue;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
namespace FriendStorage.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<IDataService, FileDataService>();
            services.AddTransient<IFriendDataProvider, FriendDataProvider>();
            services.AddTransient<IFriendEditViewModel, FriendEditViewModel>();

            services.AddTransient<Func<IDataService>>(sp => () => sp.GetRequiredService<IDataService>());

            services.AddTransient<Func<IFriendEditViewModel>>(sp => () => sp.GetRequiredService<IFriendEditViewModel>());

            services.AddTransient<Func<IFriendDataProvider>>(sp => () => sp.GetRequiredService<IFriendDataProvider>());

            services.AddTransient<INavigationDataProvider, NavigationDataProvider>();
            services.AddTransient<INavigationViewModel, NavigationViewModel>();
            services.AddTransient<IMessageDialogueService, MessageDialogueService>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();

            mainWindow?.Show();
        }
    }

}

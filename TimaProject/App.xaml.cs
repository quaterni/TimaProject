using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimaProject.Services;
using TimaProject.Stores;
using TimaProject.ViewModels;

namespace TimaProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<NavigationStore>();

            services.AddSingleton<MainWindow>(
                s => new MainWindow()
                {
                    DataContext = new MainViewModel(s.GetRequiredService<NavigationStore>())
                });

            services.AddSingleton<Func<Type, ViewModelBase>>(
                s => type => (ViewModelBase)s.GetRequiredService(type));

            services.AddTransient<TimerListingViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationServiceFactory<TimerListingViewModel>(_serviceProvider).Navigate();
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }

        private NavigationService<ViewModelBase, TViewModel> NavigationServiceFactory<TViewModel>(IServiceProvider serviceProvider) where TViewModel : ViewModelBase
        {
            return new NavigationService<ViewModelBase, TViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<Func<Type, ViewModelBase>>());
        }
    }
}

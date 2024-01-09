using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using MvvmTools.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Stores;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Containers;
using TimaProject.ViewModels.Factories;
using TimaProject.ViewModels.Validators;

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
            services.AddSingleton<ModalStore>();


            services.AddTransient<CloseModalService>();
            services.AddTransient<OpenModalService>();

            services.AddTransient<TimeValidator>();
            services.AddTransient<AbstractValidator<IProjectName> ,ProjectNameValidator >();

            services.AddSingleton<MainWindow>(
                s => new MainWindow()
                {
                    DataContext = new MainViewModel(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<ModalStore>())
                });

            services.AddSingleton<Func<Type, ViewModelBase>>(
                s => type => (ViewModelBase)s.GetRequiredService(type));

            services.AddSingleton<IRepository<Note>, NoteRepository>();

            services.AddSingleton<IRecordRepository, RecordRepository>();

            services.AddSingleton<IProjectRepository, ProjectRepository>();


            services.AddSingleton<IDateStore, TodayDateStore>();

            services.AddTransient<ProjectFormViewModelFactory>(
                s=> new ProjectFormViewModelFactory(
                        s.GetRequiredService<IProjectRepository>(),
                        s.GetRequiredService<AbstractValidator<IProjectName>>(),
                        s.GetRequiredService<CloseModalService>()));


            services.AddTransient(
                s => new TimeFormViewModelFactory(
                    s.GetRequiredService<TimeValidator>(),
                    s.GetRequiredService<CloseModalService>()));

            services.AddTransient<EditableNoteViewModelFactory>();

            services.AddTransient<NoteFormViewModelFactory>();
            services.AddTransient<ListingNoteViewModelFactory>();

            services.AddTransient(
                s => new EditableRecordViewModelFactory(
                    s.GetRequiredService<IRecordRepository>(),
                    new CompositeNavigationService(
                        ModalParameterizedNavigationService<TimeFormViewModel>(s),
                        s.GetRequiredService<OpenModalService>()),
                    new CompositeNavigationService(
                        ModalParameterizedNavigationService<ProjectFormViewModel>(s),
                        s.GetRequiredService<OpenModalService>()),
                    s.GetRequiredService<TimeFormViewModelFactory>(),
                    s.GetRequiredService<ProjectFormViewModelFactory>(),
                    s.GetRequiredService<TimeValidator>(),
                    s.GetRequiredService<NoteFormViewModelFactory>(),
                    s.GetRequiredService<ListingNoteViewModelFactory>()));

            services.AddTransient(s => TimerViewModelFactory(s));



            services.AddTransient(
                s => new ListingRecordViewModel(
                    s.GetRequiredService<IRecordRepository>(),
                    s.GetRequiredService<EditableRecordViewModelFactory>()
                ));

            services.AddTransient<TimerLayoutViewModel>(
                s => new TimerLayoutViewModel(
                    s.GetRequiredService<TimerViewModel>(),
                    s.GetRequiredService<ListingRecordViewModel>()));

            _serviceProvider = services.BuildServiceProvider();
        }

        private TimerViewModel TimerViewModelFactory(IServiceProvider s)
        {
            return new TimerViewModel(
                            s.GetRequiredService<IRecordRepository>(),
                            new CompositeNavigationService(ModalParameterizedNavigationService<TimeFormViewModel>(s),
                                                           s.GetRequiredService<OpenModalService>()),
                            new CompositeNavigationService(ModalParameterizedNavigationService<ProjectFormViewModel>(s),
                                                           s.GetRequiredService<OpenModalService>()),
                            s.GetRequiredService<TimeFormViewModelFactory>(),
                            s.GetRequiredService<ProjectFormViewModelFactory>(),
                            s.GetRequiredService<TimeValidator>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationServiceFactory<TimerLayoutViewModel>(_serviceProvider).Navigate(null);
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

        private NavigationService<ViewModelBase, TViewModel> TimerLayoutNavigationServiceFactory<TViewModel>(IServiceProvider serviceProvider) where TViewModel : ViewModelBase
        {
            return new NavigationService<ViewModelBase, TViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                (t)=> new TimerLayoutViewModel(
                    serviceProvider.GetRequiredService<TimerViewModel>(),
                    (TViewModel)serviceProvider.GetRequiredService(t)));
        }

        private NavigationService<ModalViewModel, TViewModel> ModalNavigationServiceFactory<TViewModel>(IServiceProvider service) where TViewModel : ViewModelBase
        {
            return new NavigationService<ModalViewModel, TViewModel>(
                service.GetRequiredService<ModalStore>(),
                (t) => new ModalViewModel((TViewModel)service.GetRequiredService(t)));
        }

        private ParameterizedNavigationService<ModalViewModel, TViewModel> ModalParameterizedNavigationService<TViewModel>(IServiceProvider service) where TViewModel : ViewModelBase
        {
            return new ParameterizedNavigationService<ModalViewModel, TViewModel>(
                service.GetRequiredService<ModalStore>(),
                (t) => new ModalViewModel(t));
        }

    }
}

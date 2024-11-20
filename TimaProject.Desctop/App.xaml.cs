using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using MvvmTools.Navigation.Stores;
using System;
using System.Windows;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.LocalServices;
using TimaProject.Desctop.Services;
using TimaProject.Desctop.Services.Factories;
using TimaProject.Desctop.Validators;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Containers;
using TimaProject.Desctop.ViewModels.Factories;



namespace TimaProject.Desctop
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


            services.AddTransient<ITimerExecutorFactory, TimerExecutorFactory>();
            services.AddTransient<ITimeService, TimeService>();
            services.AddTransient<IRecordService, RecordService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IDateService, DateService>();
            services.AddTransient<IValidator<string>, ProjectNameValidator>();
            services.AddTransient<IValidator<TimeDTO>, TimeValidator>();

            services.AddTransient<IProjectContainerFactory, ProjectContainerFactory>();
            services.AddTransient<IProjectFormViewModelFactory, ProjectFormViewModelFactory>();

            services.AddTransient<ITimerViewModel, TimerViewModel>(s =>
                new TimerViewModel(
                    new TimeFormFactory(s.GetRequiredService<ITimeService>(), false),
                    s.GetRequiredService<IProjectFormViewModelFactory>(),
                    s.GetRequiredService<IRecordService>(),
                    s.GetRequiredService<ITimerExecutorFactory>(),
                    s.GetRequiredService<IDateService>())
            );

            services.AddTransient<IAddNoteFormFactory, AddNoteFormViewModelFactory>();

            services.AddTransient<INoteViewModelFactory, NoteViewModelFactory>();

            services.AddTransient<IListingNoteViewModelFactory, ListingNoteViewModelFactory>();

            services.AddTransient<IRecordViewModelFactory, RecordViewModelFactory>(s=>
                new RecordViewModelFactory(
                    s.GetRequiredService<IAddNoteFormFactory>(),
                    s.GetRequiredService<IListingNoteViewModelFactory>(),
                    new TimeFormFactory(s.GetRequiredService<ITimeService>(), true),
                    s.GetRequiredService<IProjectFormViewModelFactory>(),
                    s.GetRequiredService<IRecordService>()));


            services.AddTransient<IListingRecordViewModel, ListingRecordViewModel>();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalStore>();


            services.AddTransient<CloseModalService>();
            services.AddTransient<OpenModalService>();


            services.AddSingleton<MainWindow>(
                s => new MainWindow()
                {
                    DataContext = new StartWindow(null, s.GetRequiredService<TimerLayoutViewModel>())
               });



            services.AddSingleton<Func<Type, ViewModelBase>>(
                s => type => (ViewModelBase)s.GetRequiredService(type));




            services.AddTransient<TimerLayoutViewModel>(
                s => new TimerLayoutViewModel(
                    s.GetRequiredService<ITimerViewModel>(),
                    s.GetRequiredService<IListingRecordViewModel>()));

            _serviceProvider = services.BuildServiceProvider();
        }

        private TimerViewModel TimerViewModelFactory(IServiceProvider s)
        {
            throw new NotImplementedException();
            //return new TimerViewModel(
            //                new CompositeNavigationService(ModalParameterizedNavigationService<TimeFormViewModel>(s),
            //                                               s.GetRequiredService<OpenModalService>()),
            //                new CompositeNavigationService(ModalParameterizedNavigationService<ProjectFormViewModel>(s),
            //                                               s.GetRequiredService<OpenModalService>()),
            //                s.GetRequiredService<ITimeFormViewModelFactory>(),
            //                s.GetRequiredService<IProjectFormViewModelFactory>(),
            //                s.GetRequiredService<IRecordService>(),
            //                s.GetRequiredService<ITimerExecutor>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //NavigationServiceFactory<TimerLayoutViewModel>(_serviceProvider).Navigate(null);
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

        //private NavigationService<ViewModelBase, TViewModel> TimerLayoutNavigationServiceFactory<TViewModel>(IServiceProvider serviceProvider) where TViewModel : ViewModelBase
        //{
        //    return new NavigationService<ViewModelBase, TViewModel>(
        //        serviceProvider.GetRequiredService<NavigationStore>(),
        //        (t)=> new TimerLayoutViewModel(
        //            serviceProvider.GetRequiredService<TimerViewModel>(),
        //            (TViewModel)serviceProvider.GetRequiredService(t)));
        //}

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

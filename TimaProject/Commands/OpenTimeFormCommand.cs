using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using TimaProject.ViewModels;

namespace TimaProject.Commands
{
    public class OpenTimeFormCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        private readonly RecordViewModelWithEdit _recordViewModel;

        public OpenTimeFormCommand(
            INavigationService navigationService,
            RecordViewModelWithEdit recordViewModel)
        {
            _navigationService = navigationService;
            _recordViewModel = recordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _recordViewModel.ApplyTimeForm();
            _navigationService.Navigate(_recordViewModel.TimeForm);
        }
    }
}

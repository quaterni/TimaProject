using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using TimaProject.ViewModels;

namespace TimaProject.Commands
{
    public class OpenTimeFormCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        private readonly RecordViewModelWithEdit _noteViewModel;
        private readonly Func<TimeFormViewModel> _timeFromFactory;

        public OpenTimeFormCommand(INavigationService navigationService,
            RecordViewModelWithEdit noteViewModel,
            Func<TimeFormViewModel> timeFormFactory)
        {
            _navigationService = navigationService;
            _noteViewModel = noteViewModel;
            _timeFromFactory = timeFormFactory;
        }

        public override void Execute(object? parameter)
        {

            var timeFormViewModel = _timeFromFactory();

            timeFormViewModel.Date = _noteViewModel.Date;
            timeFormViewModel.StartTime = _noteViewModel.StartTime;
            if(_noteViewModel.EndTime is not null)
            {
                timeFormViewModel.IsEndTimeEnabled = true;
                timeFormViewModel.EndTime = _noteViewModel.EndTime;
            }
            else
            {
                timeFormViewModel.IsEndTimeEnabled = false;
                timeFormViewModel.EndTime = DateTimeOffset.Now.ToString();
            }


            _noteViewModel.TimeForm = timeFormViewModel;

            _navigationService.Navigate(timeFormViewModel);
        }
    }
}

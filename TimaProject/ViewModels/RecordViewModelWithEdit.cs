using FluentValidation;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Commands;

namespace TimaProject.ViewModels
{
    public class RecordViewModelWithEdit : RecordViewModel
    {
        private readonly Func<TimeFormViewModel> _timeFormFactory;

        public RecordViewModelWithEdit(
            INavigationService timeFormNavigationService, 
            Func<TimeFormViewModel> timeFormFactory, 
            AbstractValidator<RecordViewModel> validator) : base(validator)
        {
            _timeFormFactory = timeFormFactory;
            OpenTimeFormCommand = new OpenTimeFormCommand(timeFormNavigationService, this);
        }

        public ICommand OpenTimeFormCommand { get; }

        private TimeFormViewModel? _timeForm;

        public TimeFormViewModel? TimeForm
        {
            get
            {
                return _timeForm;
            }
            set
            {
                SetValue(ref _timeForm, value);
            }
        }


        public void ApplyTimeForm()
        {
            var timeForm = _timeFormFactory();
            timeForm.IsEndTimeEnabled = !IsActive;
            timeForm.StartTime = StartTime;
            timeForm.EndTime = EndTime;
            timeForm.Date = Date;
            TimeForm = timeForm;
            TimeForm.PropertyChanged += OnTimeFormPropertyChanged;
            TimeForm.Closed += OnTimeFormClosed;
        }

        private void OnTimeFormPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(sender is not TimeFormViewModel && e.PropertyName is null)
            {
                throw new ArgumentException();
            }

            if (TimeForm!.HasPropertyErrors(e.PropertyName!))
            {
                return;
            }
            switch (e.PropertyName)
            {
                case nameof(TimeFormViewModel.StartTime):
                    StartTime = TimeForm.StartTime;
                    break;
                case nameof(TimeFormViewModel.EndTime):
                    EndTime = TimeForm.EndTime;
                    break;
                case nameof(TimeFormViewModel.Date):
                    Date = TimeForm.Date;
                    break;
            }
        }

        private void OnTimeFormClosed(object? sender, EventArgs e)
        {
            TimeForm.PropertyChanged -= OnTimeFormPropertyChanged;
            TimeForm.Closed -= OnTimeFormClosed;
            TimeForm = null;
        }
    }
}

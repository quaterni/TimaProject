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
        public RecordViewModelWithEdit(
            INavigationService timeFormNavigationService, 
            Func<TimeFormViewModel> timeFormFactory, 
            AbstractValidator<RecordViewModel> validator) : base(validator)
        {
            OpenTimeForm = new OpenTimeFormCommand(timeFormNavigationService, this, timeFormFactory);
        }

        public ICommand OpenTimeForm { get; }

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
                if (value != null)
                {
                    OnTimeFormApplied();
                }
            }
        }


        protected void OnTimeFormApplied()
        {
            TimeForm.PropertyChanged += OnTimeFormPropertyChanged;
            TimeForm.Closed += OnTimeFormClosed;
        }

        private void OnTimeFormPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(sender is not TimeFormViewModel && e.PropertyName is null)
            {
                throw new ArgumentException();
            }

            if (TimeForm!.PropertyHasErrors(e.PropertyName!))
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

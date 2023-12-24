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
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class RecordViewModelWithEdit : RecordViewModel
    {
        private readonly Func<TimeFormViewModel> _timeFormFactory;

        public ICommand OpenTimeFormCommand { get; }

        public ICommand OpenProjectFormCommand { get; }

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

        private ProjectFormViewModel? _projectForm;

        public ProjectFormViewModel? ProjectForm
        {
            get
            {
                return _projectForm;
            }
            set
            {
                SetValue(ref _projectForm, value);
            }
        }


        public RecordViewModelWithEdit(
            INavigationService timeFormNavigationService, 
            INavigationService projectFormNavigationService,
            ProjectFormViewModelFactory projectFormViewModelFactory, 
            Func<TimeFormViewModel> timeFormFactory, 
            AbstractValidator<RecordViewModel> validator) : base(validator)
        {
            _timeFormFactory = timeFormFactory;
            OpenTimeFormCommand = new OpenTimeFormCommand(timeFormNavigationService, this);
            OpenProjectFormCommand = new OpenProjectFormCommand(this, projectFormViewModelFactory, projectFormNavigationService);
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
            TimeForm!.PropertyChanged -= OnTimeFormPropertyChanged;
            TimeForm.Closed -= OnTimeFormClosed;
            TimeForm = null;
        }
    }
}

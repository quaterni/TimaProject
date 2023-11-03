using FluentValidation;
using FluentValidation.Validators;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimaProject.Commands;


namespace TimaProject.ViewModels
{
    public class TimeFormViewModel : RecordViewModel
    {
        private bool _isEndTimeEnabled;

        public bool IsEndTimeEnabled
        {
            get
            {
                return _isEndTimeEnabled;
            }
            set
            {
                EndTime = DateTimeOffset.Now.ToString();
                SetValue(ref _isEndTimeEnabled, value);
            }
        }

        public new string EndTime
        {
            get
            {
                return base.EndTime;
            }
            set
            {
                if (IsEndTimeEnabled)
                {
                    base.EndTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }

        private string _time;

        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                SetValue(ref _time, value);
            }
        }

        public ICommand CloseCommand { get; }

        public event EventHandler<EventArgs>? Closed;


        public TimeFormViewModel(AbstractValidator<RecordViewModel> validator, INavigationService closeForm) : base(validator)
        {
            CloseCommand = new NavigationCommand(closeForm);
            _time = string.Empty;
            _isEndTimeEnabled = true;
            PropertyChanged += OnTimeChanged;
        }



        protected override void Validate(string propertyName)
        {
            if(propertyName is nameof(Time))
            {
                ValidateTime();
                return;
            }
            base.Validate(propertyName);
        }

        private void ValidateTime()
        {
            if(!TimeSpan.TryParse(Time, out var time))
            {
                AddError(nameof(Time), "Incorrect input");
                return;
            }
            if(HasPropertyErrors(nameof(EndTime)) || EndTime.Equals(string.Empty))
            {
                AddError(nameof(Time), "Set correct EndTime");
                return;
            }
            DateTime.TryParse(EndTime, out var endTime);
            StartTime = (endTime - time).ToString();
        }

        private void OnTimeChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(!(e.PropertyName == nameof(StartTime) || e.PropertyName == nameof(EndTime)))
            {
                return;
            }

            if(HasPropertyErrors(nameof(StartTime)) || HasPropertyErrors(nameof(EndTime)))
            {
                AddError(nameof(Time), "StartTime or EndTime not correct");
                return;
            }

            if (StartTime.Equals(string.Empty) || EndTime.Equals(string.Empty))
            {
                return;
            }

            DateTime.TryParse(StartTime, out var startTime);
            DateTime.TryParse(EndTime, out var endTime);
            Time = (endTime - startTime).ToString();
        }
    }
}

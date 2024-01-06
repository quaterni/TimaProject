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
using TimaProject.Exceptions;
using TimaProject.Models;


namespace TimaProject.ViewModels
{
    public class TimeFormViewModel :NotifyDataErrorViewModel, ITimeBase
    {
        private IRecordViewModel _source;

        private readonly AbstractValidator<ITimeBase> _validator;

        private readonly TimeSolver _solver;

        private bool _isEndTimeEnabled;

        public bool IsEndTimeEnabled
        {
            get
            {
                return _isEndTimeEnabled;
            }
        }

        private string _endTime;

        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (!IsEndTimeEnabled)
                {
                    throw new SettingDisableEndTimeException();
                }
                SetValue(ref _endTime, value);

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

        private string _startTime;

        public string StartTime
        { 
            get
            {
                return _startTime;
            }
            set
            {
                SetValue(ref _startTime, value);
            }
        }

        private string _date;

        public string Date 
        { 
            get
            {
                return _date;
            }
            set
            {
                SetValue(ref _date, value);
            }
        }

        public ICommand CloseCommand { get; }

        public TimeFormViewModel(
            IRecordViewModel source,
            AbstractValidator<ITimeBase> validator, 
            INavigationService closeForm,
            bool isEndTimeEnabled=true)
        {
            _solver = new TimeSolver(this, validator); 
            _endTime = isEndTimeEnabled ?  source.EndTime : DateTime.Now.ToString();
            _startTime = source.StartTime;
            _time = source.Time;
            _date = source.Date;
            _source = source;
            _validator = validator;
            CloseCommand = new NavigationCommand(closeForm);
            _isEndTimeEnabled = isEndTimeEnabled;
            PropertyChanged += OnPropertyChanged;
        }



        private void SetToSource(string propertyName)
        {
            var properties = new string[] { nameof(StartTime), nameof(EndTime), nameof(Time), nameof(Date)};

            if (!properties.Contains(propertyName))
            {
                return;
            }
            if (!_source.StartTime.Equals(StartTime))
                _source.StartTime = StartTime;

            if (!_source.EndTime.Equals(EndTime))
                    _source.EndTime = EndTime;
            
            if (!_source.Time.Equals(Time))
                    _source.Time = Time;

            if (!_source.Date.Equals(Date))
                    _source.Date = Date;                         
        }

        public override void Dispose()
        {
            PropertyChanged -= OnPropertyChanged;
            base.Dispose();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (HasErrors)
            {
                return;
            }
            if (e.PropertyName != null)
            {
                SetToSource(e.PropertyName);
            }
        }

        protected override void Validate(string propertyName)
        {
            ClearErrors(nameof(EndTime));
            ClearErrors(nameof(StartTime));
            ClearErrors(nameof(Time));
            ClearErrors(nameof(Date));
            ClearAllErrors();

            _solver.Solve(propertyName);

            var validationResult = _validator.Validate(this);

            if(validationResult.IsValid)
            {
                return;
            }

            foreach(var error in validationResult.Errors)
            {
                AddError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}

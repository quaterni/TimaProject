using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Exceptions;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;


namespace TimaProject.Desctop.ViewModels
{
    public class TimeFormViewModel : ObservableValidator, ITimeFormViewModel
    {
        private bool _lock;

        private readonly ITimeService _timeService;

        private bool _canEndTimeEdit;

        public bool CanEndTimeEdit
        {
            get
            {
                return _canEndTimeEdit;
            }
        }

        private string _endTime;

        [CustomValidation(typeof(TimeFormViewModel), nameof(TimeValidation))]
        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (!CanEndTimeEdit)
                {
                    throw new SettingDisableEndTimeException();
                }
                SetProperty(ref _endTime, value, true);

            }
        }

        private string _time;

        [CustomValidation(typeof(TimeFormViewModel), nameof(TimeValidation))]
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                SetProperty(ref _time, value, true);
            }
        }

        private string _startTime;

        [CustomValidation(typeof(TimeFormViewModel), nameof(TimeValidation))]
        public string StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                SetProperty(ref _startTime, value, true);
            }
        }


        private string _date;

        [CustomValidation(typeof(TimeFormViewModel), nameof(TimeValidation))]
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetProperty(ref _date, value, true);

            }
        }


        private string _componentError;

        [CustomValidation(typeof(TimeFormViewModel), nameof(ComponentErrorValidaton))]
        public string ComponentError
        {
            get
            {
                return _componentError;
            }
            set
            {
                SetProperty(ref _componentError, value, true);
            }
        }

        public event EventHandler<TimeDTO>? TimeSelected;

        public event EventHandler<EventArgs>? Closed;

        public ICommand CloseCommand { get; }

        public TimeFormViewModel(
            TimeDTO timeDTO,
            ITimeService timeService,
            bool canEndTimeEdit = true)
        {
            _timeService = timeService;
            _endTime = canEndTimeEdit ? timeDTO.EndTime : DateTime.Now.ToString();
            _startTime = timeDTO.StartTime;
            _time = timeDTO.Time;
            _date = timeDTO.Date;
            _canEndTimeEdit = canEndTimeEdit;
            PropertyChanged += OnTimeValuesChanged;
        }

        private void OnTimeValuesChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(Time):
                case nameof(StartTime): 
                case nameof(EndTime):
                case nameof(Date):
                    if (!HasErrors)
                    {
                        SelectTime();
                    }
                    break;
            }
        }

        private void SelectTime()
        {
            TimeSelected?.Invoke(this, new TimeDTO(StartTime, EndTime, Time, Date));
        }

        private TimeDTO CreateTimeDTO()
        {
            return new TimeDTO(_startTime, _endTime, _time, _date);
        }

        public static ValidationResult ComponentErrorValidaton(string error, ValidationContext context)
        {
            if (string.IsNullOrEmpty(error))
            {
                return ValidationResult.Success;
            }
            return new(error);
        }


        public static ValidationResult TimeValidation(string value, ValidationContext context)
        {           
            TimeFormViewModel instance = (TimeFormViewModel)context.ObjectInstance;
            if (instance._lock)
            {
                return ValidationResult.Success;
            }
            string propertyName = context.DisplayName;
            TimeServiceResult result = instance._timeService.Solve(
                propertyName,
                new TimeDTO(instance.StartTime, instance.EndTime, instance.Time, instance.Date));

            if(result.Result == SolvingResult.PropertyError)
            {
                return new(result.ErrorMessage);
            }
            if(result.Result == SolvingResult.ComponentError)
            {
                instance.ComponentError = result.ErrorMessage;
                return ValidationResult.Success;
            }

            instance.ComponentError = "";
            instance._lock = true;
            instance.StartTime = result.Value.StartTime;
            instance.EndTime = result.Value.EndTime;
            instance.Time = result.Value.Time;
            instance.Date = result.Value.Date;
            instance._lock = false;

            return ValidationResult.Success;

        }


        private void SetToSource(string propertyName)
        {
            //var properties = new string[] { nameof(StartTime), nameof(EndTime), nameof(Time), nameof(Date) };

            //if (!properties.Contains(propertyName))
            //{
            //    return;
            //}
            //if (!_source.StartTime.Equals(StartTime))
            //    _source.StartTime = StartTime;

            //if (!_source.EndTime.Equals(EndTime))
            //    _source.EndTime = EndTime;

            //if (!_source.Time.Equals(Time))
            //    _source.Time = Time;

            //if (!_source.Date.Equals(Date))
            //    _source.Date = Date;
        }

        public void Dispose()
        {
            PropertyChanged -= OnTimeValuesChanged;
        }


    }
}

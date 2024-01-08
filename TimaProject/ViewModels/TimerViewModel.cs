using FluentValidation;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TimaProject.Commands;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public enum TimerState
    {
        NotRunning,
        Running
    }

    public class TimerViewModel: NotifyDataErrorViewModel, IEditRecord, IRecordViewModel
    {
        private TimerState _state;

        public TimerState State
        {
            get 
            {
                return _state; 
            }
            set
            {
                SetValue(ref _state, value);
            }
        }

        private readonly AbstractValidator<ITimeBase> _validator;

        public const int TIMER_INTERVAL_MILLISECONDS = 200;

        private DateTime? _timerStartTime;

        private Record? _record;

        private readonly IRecordRepository _recordRepository;

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

        public ICommand TimerCommand { get; }

        public ICommand OpenTimeFormCommand { get; }

        public ICommand OpenProjectFormCommand { get; }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetValue(ref _title, value);
            }
        }

        private Project _project;

        public Project Project
        {
            get
            {
                return _project;
            }
            set
            {
                SetValue(ref _project, value);
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

        private string _endTime;

        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                SetValue(ref _endTime, value);
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

        public TimerViewModel(
            IRecordRepository noteRepository,
            INavigationService timeFormNavigationService,
            INavigationService projectFormNavigationService,
            TimeFormViewModelFactory timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            AbstractValidator<ITimeBase> validator)
        {
            State = TimerState.NotRunning;
            _validator = validator;
            _recordRepository = noteRepository;
            OpenTimeFormCommand = new OpenTimeFormCommand(this, timeFormFactory, timeFormNavigationService, isEndTimeEnabled:false);
            OpenProjectFormCommand = new OpenProjectFormCommand(this, projectFormViewModelFactory, projectFormNavigationService);
            SetDafultValues();
            TimerCommand = new TimerCommand(this);
            PropertyChanged += OnRecordUpdated;
        }

        private void OnRecordUpdated(object? sender, PropertyChangedEventArgs e)
        {
            if(_record is null)
            {
                return;
            }

            foreach (var error in GetErrors(e.PropertyName))
            {
                if(error is not null)
                {
                    return;
                }
            }
            switch (e.PropertyName)
            {
                case nameof(Title):
                    {
                        _record = _record with { Title = Title };
                        _recordRepository.UpdateItem(_record);
                        break;
                    }
                case nameof(Project):
                    {
                        _record = _record with { Project = Project };
                        _recordRepository.UpdateItem(_record);
                        break;
                    }
                case nameof(StartTime):
                    {
                        var startTime = DateTime.Parse(StartTime);
                        SetStartTime();
                        _record = _record with { StartTime = startTime };
                        _recordRepository.UpdateItem(_record);
                        break;
                    }
                case nameof(Date):
                    {
                        var date = DateOnly.Parse(Date);
                        _record = _record with { Date = date };
                        _recordRepository.UpdateItem(_record);
                        break;
                    }
            }
        }

        private void StartTimer()
        {
            SetStartTime();
            Observable
                .Timer((DateTime)_timerStartTime!, TimeSpan.FromMilliseconds(TIMER_INTERVAL_MILLISECONDS))
                .TakeWhile(_ => State == TimerState.Running)
                .Subscribe(seconds =>
                {
                    Time = (DateTime.Now - (DateTime)_timerStartTime).ToString();
                },
                () =>
                {
                    UpdateRecordWithEndTime();
                    SetDafultValues();
                });
        }

        private void SetStartTime()
        {
            if (HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
            {
                if(_timerStartTime is null)
                {
                    _timerStartTime = DateTime.Now;
                }
                return;
            }
            _timerStartTime = DateTime.Parse(StartTime);
        }



        public void OnStartingTime()
        {
            if(HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
            {
                StartTime = DateTime.Now.ToString();
            }
            State = TimerState.Running;

            CreateRecord();
            StartTimer();
        }

        private void CreateRecord()
        {
            var title = HasPropertyErrors(nameof(Title)) ? string.Empty : Title;
            var project = Project;
            var startTime = HasPropertyErrors(nameof(StartTime)) ?
                                DateTime.Now : DateTime.Parse(StartTime);
            var date = HasPropertyErrors(nameof(Date))
                       || Date.Equals(string.Empty) ?
                        DateOnly.FromDateTime(DateTime.Today) : DateOnly.Parse(Date);
            var record = new Record(startTime, date, Guid.NewGuid())
            {
                Project = project,
                Title = title,
            };

            _record = record;
            _recordRepository.AddItem(record);
        }

        public void OnEndingTime()
        {
            if(State == TimerState.Running)
            {
                State = TimerState.NotRunning;
            }
        }

        private void UpdateRecordWithEndTime()
        {
            var updatedNote = _record! with
            {
                EndTime = DateTime.Now,
            };
            _recordRepository.UpdateItem(updatedNote);
        }

        private void SetDafultValues()
        {
            _record = null;
            _timerStartTime = null;
            Title = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
            Project = Project.Empty;
            Date = DateOnly.FromDateTime(DateTime.Today).ToString();
            Time = "00:00:00";
        }

        protected override void Validate(string propertyName)
        {
            ClearAllErrors();
            var validationResult = _validator.Validate(this);
            if (validationResult.IsValid)
            {
                return;
            }
            foreach(var error in  validationResult.Errors)
            {
                AddError(error.PropertyName, error.ErrorMessage);
            }

        }
    }
}

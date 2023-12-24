using FluentValidation;
using MvvmTools.Navigation.Services;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using TimaProject.Commands;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Services.Factories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class TimerViewModel: RecordViewModelWithEdit, IRecordViewModel
    {
        private const int TIMER_INTERVAL_MILLISECONDS = 200;

        private DispatcherTimer? _dispatcherTimer;

        private DateTime? _startTime;

        private Record? _record;

        private readonly IRecordRepository _recordRepository;

        private readonly IRecordFactory _factory;

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

        public TimerViewModel(IRecordRepository noteRepository, 
            IRecordFactory factory,
            INavigationService timeFormNavigationService,
            INavigationService projectFormNavigationService,
            Func<TimeFormViewModel> timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            AbstractValidator<RecordViewModel> validator): base(timeFormNavigationService, projectFormNavigationService, projectFormViewModelFactory, timeFormFactory, validator)
        {
            _recordRepository = noteRepository;
            _factory = factory;
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
                        _recordRepository.UpdateRecord(_record);
                        break;
                    }
                case nameof(Project):
                    {
                        _record = _record with { Project = Project };
                        _recordRepository.UpdateRecord(_record);
                        break;
                    }
                case nameof(StartTime):
                    {
                        var startTime = DateTime.Parse(StartTime);
                        SetStartTime();
                        _record = _record with { StartTime = startTime };
                        _recordRepository.UpdateRecord(_record);
                        break;
                    }
                case nameof(Date):
                    {
                        var date = DateOnly.Parse(Date);
                        _record = _record with { Date = date };
                        _recordRepository.UpdateRecord(_record);
                        break;
                    }
            }

        }

        private void StartTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(TIMER_INTERVAL_MILLISECONDS);
            SetStartTime();
            _dispatcherTimer.Tick += SetTime;
            _dispatcherTimer.Start();
        }

        private void SetTime(object? sender, EventArgs e)
        {
            Time = (DateTime.Now - _startTime).ToString()!;
        }

        private void SetStartTime()
        {
            if (HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
            {
                if(_startTime is null)
                {
                    _startTime = DateTime.Now;
                }
                return;
            }
            _startTime = DateTime.Parse(StartTime);
        }



        public void OnStartingTime()
        {
            if(HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
            {
                StartTime = DateTime.Now.ToString();
            }

            StartTimer();
            IsActive = true;

            var recordViewModel = new RecordViewModel(_validator)
            {
                Title = HasPropertyErrors(nameof(Title)) ? string.Empty : Title,
                Project = Project,
                StartTime = HasPropertyErrors(nameof(StartTime)) ?
                                DateTimeOffset.Now.ToString() : StartTime,
                Date = HasPropertyErrors(nameof(Date)) 
                       || Date.Equals(string.Empty) ?
                        DateOnly.FromDateTime(DateTime.Today).ToString() : Date
            };

            var newNote = _factory.Create(recordViewModel);
            _record = newNote;
            _recordRepository.AddRecord(newNote);
        }

        public void OnEndingTime()
        {
            if(IsActive)
            {
                StopTimer();
                var updatedNote = _record! with
                {
                    EndTime = DateTime.Now,
                };
                _recordRepository.UpdateRecord(updatedNote);
                IsActive = false;

                SetDafultValues();
            }
        }

        private void StopTimer()
        {
            _dispatcherTimer!.Stop();
            _dispatcherTimer.Tick -= SetTime;
        }

        private void SetDafultValues()
        {
            _record = null;
            _startTime = null;
            _dispatcherTimer = null;
            Title = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
            Project = Project.Empty;
            Date = DateOnly.FromDateTime(DateTime.Today).ToString();
            Time = "00:00:00";
        }
    }
}

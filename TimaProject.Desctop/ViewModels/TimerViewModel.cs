using System.ComponentModel;
using System.Windows.Input;
using TimaProject.Desctop.Commands;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using System;
using CommunityToolkit.Mvvm.Input;
using TimaProject.Desctop.DTOs;
using System.Linq;



namespace TimaProject.Desctop.ViewModels
{
    internal class TimerViewModel : RecordViewModelBase, ITimerViewModel
    {
        private ITimerExecutor? _timerExecutor;
        private readonly ITimerExecutorFactory _timerExecutorFactory;
        private readonly IRecordService _recordService;
        private readonly IDateService _dateReportService;

        private Guid? _recordId;

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

        public ICommand TimerCommand { get; }

        public TimerViewModel(
            ITimeFormViewModelFactory timeFormFactory,
            IProjectFormViewModelFactory projectFormViewModelFactory,
            IRecordService recordService,
            ITimerExecutorFactory timerExecutorFactory,
            IDateService dateReportService) : base(timeFormFactory, projectFormViewModelFactory)
        {
            State = TimerState.NotRunning;
            Time = TimeSpan.Zero.ToString();
            _recordService = recordService;
            _timerExecutorFactory = timerExecutorFactory;
            _dateReportService = dateReportService;


            TimerCommand = new RelayCommand(OnTimerCommand);
            PropertyChanged += OnStartTimeChanged;
            PropertyChanged += OnRecordUpdated;
            
        }

        private void OnRecordUpdated(object? sender, PropertyChangedEventArgs e)
        {
            string[] properties = [
                nameof(StartTime),
                nameof(Title),
                nameof(ProjectName),
                nameof(ProjectId),
                nameof(Date)
            ];
            if (properties.Contains(e.PropertyName) 
                && State == TimerState.Running 
                && _recordId is not null)
            {
                UpdateRecord(true);
            }
        }

        private void UpdateRecord(bool isActive)
        {
            _recordService.UpdateRecord(
                new RecordDto(StartTime, Title, _recordId!.Value)
                {
                    EndTime = EndTime,
                    Date = Date,
                    ProjectId = ProjectId,
                    ProjectName = ProjectName,
                    IsActive = isActive
                });
        }

        private void OnStartTimeChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(State == TimerState.Running 
                && e.PropertyName == nameof(StartTime)
                && _timerExecutor is not null
                && DateTime.TryParse(StartTime, out var startingTime))
            {
                _timerExecutor.StartTime = startingTime;
            }
        }


        private void SetStartTime()
        {
            //if (HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
            //{
            //    if (_timerStartTime is null)
            //    {
            //        _timerStartTime = DateTime.Now;
            //    }
            //    return;
            //}
            //_timerStartTime = DateTime.Parse(StartTime);
        }

        private void OnTimerCommand()
        {

            switch (State)
            {
                case TimerState.Running:
                    StopTimer();
                    break;
                case TimerState.NotRunning:
                    StartTimer();
                    break;
            }
        }

        public void StartTimer()
        {
            _timerExecutor = _timerExecutorFactory.Create();
            if (!DateTime.TryParse(StartTime, out var startingTime))
            {
                startingTime = _timerExecutor.CurrentTime();
                StartTime = startingTime.ToString();
            }
            
            _timerExecutor.StartTime = startingTime;
            _timerExecutor.Tick += OnTimerTick;
            _timerExecutor.Start();

            State = TimerState.Running;

            CreateRecord();
        }

        private void OnTimerTick(object? sender, TimeSpan e)
        {
            Time = e.ToString();
        }

        private void CreateRecord()
        {
            if(!DateOnly.TryParse(Date, out _))
            {
                Date = _dateReportService.CurrentDate().ToString();
            }
            _recordId = Guid.NewGuid();
            _recordService.AddRecord(
                new RecordDto(StartTime, Title, _recordId.Value)
                {
                    Date = Date,
                    ProjectId = ProjectId,
                    ProjectName = ProjectName
                });
        }

        public void StopTimer()
        {
            State = TimerState.NotRunning;
            EndTime = _timerExecutor!.CurrentTime().ToString();
            UpdateRecord(false);

            _recordId = null;
            _timerExecutor.Dispose();
            _timerExecutor = null;
            StartTime = string.Empty;
            EndTime = string.Empty;
            Time = TimeSpan.Zero.ToString();
            Date = string.Empty;
            Title = string.Empty;
            ProjectId = Guid.Empty;
            ProjectName = string.Empty;
        }


        //protected override void Validate(string propertyName)
        //{
        //    ClearAllErrors();
        //    var validationResult = _validator.Validate(this);
        //    if (validationResult.IsValid)
        //    {
        //        return;
        //    }
        //    foreach (var error in validationResult.Errors)
        //    {
        //        AddError(error.PropertyName, error.ErrorMessage);
        //    }

        //}
    }
}

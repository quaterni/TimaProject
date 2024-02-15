using FluentValidation;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TimaProject.Desctop.Commands;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels.Factories;
using TimaProject.Desctop.Interfaces.ViewModels;
using System.Runtime.CompilerServices;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
[assembly: InternalsVisibleToAttribute("TimaProject.Desctop.Tests")]


namespace TimaProject.Desctop.ViewModels
{
    internal class TimerViewModel : RecordViewModelBase, ITimerViewModel
    {
        private readonly ITimerExecutor _timerExecutor;
        private readonly IRecordService _recordService;

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
            INavigationService timeFormNavigationService,
            INavigationService projectFormNavigationService,
            ITimeFormViewModelFactory timeFormFactory,
            IProjectFormViewModelFactory projectFormViewModelFactory,
            IRecordService recordService,
            ITimerExecutor timerExecutor) : base(timeFormFactory, projectFormViewModelFactory)
        {
            State = TimerState.NotRunning;

            //OpenTimeFormCommand = new OpenTimeFormCommand(this, timeFormFactory, timeFormNavigationService, isEndTimeEnabled: false);
            //OpenProjectFormCommand = new OpenProjectFormCommand(this, projectFormViewModelFactory, projectFormNavigationService);

            TimerCommand = new TimerCommand(this);
            PropertyChanged += OnRecordUpdated;
        }

        private void OnRecordUpdated(object? sender, PropertyChangedEventArgs e)
        {
            //if (_record is null)
            //{
            //    return;
            //}

            //foreach (var error in GetErrors(e.PropertyName))
            //{
            //    if (error is not null)
            //    {
            //        return;
            //    }
            //}
            //switch (e.PropertyName)
            //{
            //    case nameof(Title):
            //        {
            //            _record = _record with { Title = Title };
            //            _recordRepository.UpdateItem(_record);
            //            break;
            //        }
            //    case nameof(Project):
            //        {
            //            _record = _record with { Project = Project };
            //            _recordRepository.UpdateItem(_record);
            //            break;
            //        }
            //    case nameof(StartTime):
            //        {
            //            var startTime = DateTime.Parse(StartTime);
            //            SetStartTime();
            //            _record = _record with { StartTime = startTime };
            //            _recordRepository.UpdateItem(_record);
            //            break;
            //        }
            //    case nameof(Date):
            //        {
            //            var date = DateOnly.Parse(Date);
            //            _record = _record with { Date = date };
            //            _recordRepository.UpdateItem(_record);
            //            break;
            //        }
            //}
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



        public void OnStartingTime()
        {
        //    if (HasPropertyErrors(nameof(StartTime)) || StartTime.Equals(string.Empty))
        //    {
        //        StartTime = DateTime.Now.ToString();
        //    }
        //    State = TimerState.Running;

        //    CreateRecord();
        //    StartTimer();
        }

        private void CreateRecord()
        {
            //var title = HasPropertyErrors(nameof(Title)) ? string.Empty : Title;
            //var project = Project;
            //var startTime = HasPropertyErrors(nameof(StartTime)) ?
            //                    DateTime.Now : DateTime.Parse(StartTime);
            //var date = HasPropertyErrors(nameof(Date))
            //           || Date.Equals(string.Empty) ?
            //            DateOnly.FromDateTime(DateTime.Today) : DateOnly.Parse(Date);
            //var record = new Record(startTime, date, Guid.NewGuid())
            //{
            //    Project = project,
            //    Title = title,
            //};

            //_record = record;
            //_recordRepository.AddItem(record);
        }

        public void OnEndingTime()
        {
            if (State == TimerState.Running)
            {
                State = TimerState.NotRunning;
            }
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

using FluentValidation;
using MvvmTools.Navigation.Services;
using System;
using System.ComponentModel;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Services.Factories;


namespace TimaProject.ViewModels
{
    public class TimerViewModel: RecordViewModelWithEdit
    {
        private Record? _record;

        private readonly IRecordRepository _recordRepository;

        private readonly IRecordFactory _factory;

        public TimerViewModel(IRecordRepository noteRepository, 
            IRecordFactory factory,
            INavigationService timeFormNavigationService,
            Func<TimeFormViewModel> timeFormFactory,
            AbstractValidator<RecordViewModel> validator): base(timeFormNavigationService, timeFormFactory, validator)
        {
            _recordRepository = noteRepository;
            _factory = factory;
            SetDafultValues();
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
                        if (startTime == DateTime.MinValue)
                            break; 
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

        public void OnStartingTime()
        {
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
            IsActive = true;

        }

        public void OnEndingTime()
        {
            if(IsActive)
            {
                var updatedNote = _record! with
                {
                    EndTime = DateTime.Now,
                };
                _recordRepository.UpdateRecord(updatedNote);
                IsActive = false;

                SetDafultValues();
            }
        }

        private void SetDafultValues()
        {
            _record = null;
            Title = string.Empty;
            StartTime = DateTimeOffset.MinValue.ToString();
            EndTime = string.Empty;
            Project = Project.Empty;
            Date = DateOnly.FromDateTime(DateTime.Today).ToString();
        }
    }
}

using FluentValidation;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Commands;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class EditableRecordViewModel : NotifyDataErrorViewModel, IRecordViewModel, IEditRecord
    {
        private readonly AbstractValidator<ITimeBase> _validator;

        private readonly TimeSolver _solver;

        private Record _record;
        private readonly IRecordRepository _recordRepository;

        public Record Record
        {
            get
            {
                return _record;
            }
            private set
            {
                SetValue(ref _record, value);
                OnPropertyChanged(nameof(Time));
            }
        }

        public string _time;

        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                SetValue(ref _time, value);
                _solver.Solve(nameof(Time));

            }
        }

        public ICommand RemoveRecordCommand { get; }

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
                _solver.Solve(nameof(StartTime));

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
                _solver.Solve(nameof(EndTime));

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
                _solver.Solve(nameof(Date));
            }
        }

        public ICommand OpenTimeFormCommand { get; }

        public ICommand OpenProjectFormCommand { get; }

        public EditableRecordViewModel(
            Record record,
            IRecordRepository recordRepository,
            INavigationService timeFormNavigationService,
            INavigationService projectFormNavigationService,
            TimeFormViewModelFactory timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            AbstractValidator<ITimeBase> validator)
        {
            if (record.IsActive)
            {
                throw new Exception("Editable record must be not active");
            }
            _validator = validator;
            _solver = new TimeSolver(this, validator);
            _record = record;
            _recordRepository = recordRepository;
            Title = record.Title;
            StartTime = record.StartTime.ToString();
            EndTime = record.EndTime.ToString() ?? string.Empty;
            Date = record.Date.ToString();
            Project = record.Project;
            PropertyChanged += OnRecordUpdated;
            _recordRepository.RepositoryChanged += OnRepositoryUpdated;

            OpenProjectFormCommand = new OpenProjectFormCommand(this, projectFormViewModelFactory, projectFormNavigationService);
            OpenTimeFormCommand = new OpenTimeFormCommand(this, timeFormFactory, timeFormNavigationService);

            RemoveRecordCommand = new CommandCallback((x)=> RemoveRecord());
        }

        private void OnRepositoryUpdated(object? sender, RepositoryChangedEventArgs<Record> e)
        {
            if (e.Item.Id.Equals(Record.Id))
            {
                Record = e.Item;
                Title = Record.Title;
                StartTime = Record.StartTime.ToString();
                EndTime = Record.EndTime.ToString();
                Project = Record.Project;
                Date = Record.Date.ToString();
            }
        }

        private void OnRecordUpdated(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName is null || HasPropertyErrors(e.PropertyName))
            {
                return;
            }
            switch (e.PropertyName)
            {
                case nameof(Title):
                    Record = _record with { Title = Title };
                    _recordRepository.UpdateItem(_record);
                    break;
                case nameof(StartTime):
                    Record = _record with { StartTime = DateTime.Parse(StartTime) };
                    _recordRepository.UpdateItem(_record);
                    break;
                case nameof(EndTime):
                    Record = _record with { EndTime = DateTime.Parse(EndTime!) };
                    _recordRepository.UpdateItem(_record);
                    break;
                case nameof(Date):
                    Record = _record with { Date = DateOnly.Parse(Date) };
                    _recordRepository.UpdateItem(_record);
                    break;
                case nameof(Project):
                    Record = _record with { Project = Project };
                    _recordRepository.UpdateItem(_record);
                    break;
            }

        }

        public void RemoveRecord()
        {
            _recordRepository.RemoveItem(_record);
        }

        protected override void Validate(string propertyName)
        {
            ClearAllErrors();
            var validationResult = _validator.Validate(this);
            if (validationResult.IsValid)
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

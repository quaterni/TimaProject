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
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class EditableRecordViewModel : RecordViewModelWithEdit, IRecordViewModel
    {
        private Record _record;
        private readonly IRecordRepository _recordRepository;

        private Record Record
        {
            get
            {
                return _record;
            }
            set
            {
                SetValue(ref _record, value);
                OnPropertyChanged(nameof(Time));
            }
        }

        public string Time
        {
            get
            {
                if(_record.EndTime is not null )
                {
                    return (_record.EndTime - _record.StartTime).ToString()!;
                }
                return string.Empty;
            }
            set
            {
                throw new NotImplementedException(nameof(Time));
            }
        }

        public ICommand DeleteRecordCommand { get; }

        public EditableRecordViewModel(
            Record record,
            IRecordRepository recordRepository,
            INavigationService timeFormNavigationService,
            INavigationService projectFormNavigationService,
            Func<TimeFormViewModel> timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            AbstractValidator<RecordViewModel> validator) : base(timeFormNavigationService, projectFormNavigationService, projectFormViewModelFactory, timeFormFactory, validator)
        {
            _record = record;
            _recordRepository = recordRepository;
            Title = record.Title;
            StartTime = record.StartTime.ToString();
            EndTime = record.EndTime.ToString() ?? string.Empty;
            Date = record.Date.ToString();
            Project = record.Project;
            PropertyChanged += OnRecordUpdated;
            DeleteRecordCommand = new DeleteRecordCommand(this);
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
                    _recordRepository.UpdateRecord(_record);
                    break;
                case nameof(StartTime):
                    Record = _record with { StartTime = DateTime.Parse(StartTime) };
                    _recordRepository.UpdateRecord(_record);
                    break;
                case nameof(EndTime):
                    Record = _record with { EndTime = DateTime.Parse(EndTime!) };
                    _recordRepository.UpdateRecord(_record);
                    break;
                case nameof(Date):
                    Record = _record with { Date = DateOnly.Parse(Date) };
                    _recordRepository.UpdateRecord(_record);
                    break;
                case nameof(Project):
                    Record = _record with { Project = Project };
                    _recordRepository.UpdateRecord(_record);
                    break;
            }

        }

        public void DeleteRecord()
        {
            _recordRepository.DeleteRecord(_record);
        }
    }
}

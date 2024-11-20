using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmTools.Base;
using System;
using System.Windows.Input;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels
{
    internal abstract class RecordViewModelBase : ObservableObject, IRecordViewModelBase
    {
        private string _startTime;

        public string StartTime
        { 
            get
            {
                return _startTime;
            }
            set
            {
                SetProperty(ref  _startTime, value);
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
                SetProperty(ref _endTime, value);
            }
        }

        private string _time;

        public string Time
        {
            get
            {
                return _time;
            }
            protected set
            {
                SetProperty(ref _time, value);
            }
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
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
                SetProperty(ref _date, value);
            }
        }

        private string _projectName;

        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            protected set
            {
                SetProperty(ref _projectName, value);
            }
        }

        private Guid _projectId;

        public Guid ProjectId
        {
            get
            {
                return _projectId;
            }
            protected set
            {
                SetProperty(ref _projectId, value);
            }
        }

        public ICommand OpenTimeFormCommand { get; }

        private ITimeFormViewModel? _timeFormViewModel;

        public ITimeFormViewModel? TimeFormViewModel
        {
            get
            {
                return _timeFormViewModel;
            }
            set
            {
                SetProperty(ref _timeFormViewModel, value);
                OnPropertyChanged(nameof(IsTimeFormOpened));
            }
        }

        public bool IsTimeFormOpened
        {
            get
            {
                return TimeFormViewModel is not null;
            }
            set
            {
                if (!value && TimeFormViewModel is not null)
                {
                    TimeFormViewModel.CloseCommand.Execute(null);
                }
            }
        }

        public ICommand OpenProjectFormCommand { get; }

        private IProjectFormViewModel? _projectFormViewModel;

        public IProjectFormViewModel? ProjectFormViewModel
        {
            get
            {
                return _projectFormViewModel;
            }
            set
            {
                SetProperty(ref _projectFormViewModel, value);
                OnPropertyChanged(nameof(IsProjectFormOpened));

            }
        }

        public bool IsProjectFormOpened
        {
            get
            {
                return ProjectFormViewModel is not null;
            }
            set
            {
                if (!value && ProjectFormViewModel is not null)
                {
                    ProjectFormViewModel.CloseCommand.Execute(null);
                }
            }
        }

        public RecordViewModelBase(
            ITimeFormViewModelFactory timeFormViewModelFactory, 
            IProjectFormViewModelFactory projectFormViewModelFactory)
        {
            _startTime = string.Empty;
            _endTime = string.Empty;
            _time = string.Empty;
            _date = string.Empty;
            _title = string.Empty;
            _projectName = string.Empty;
            _projectId = Guid.Empty;

            OpenProjectFormCommand = new RelayCommand(
                () => OnProjectFormCreating(projectFormViewModelFactory));
            OpenTimeFormCommand = new RelayCommand(
                () => OnTimeFormCreating(timeFormViewModelFactory));
        }

        private void OnTimeFormCreating(ITimeFormViewModelFactory timeFormViewModelFactory)
        {
            TimeFormViewModel = timeFormViewModelFactory.Create(new TimeDTO(StartTime, EndTime, Time, Date));
            TimeFormViewModel.TimeSelected += OnTimeSelected;
            TimeFormViewModel.Closed += OnTimeFormClosed;
        }

        private void OnTimeFormClosed(object? sender, EventArgs e)
        {
            if(TimeFormViewModel is not null)
            {
                TimeFormViewModel.Dispose();
                TimeFormViewModel = null;
            }
        }

        private void OnTimeSelected(object? sender, TimeDTO e)
        {
            StartTime = e.StartTime;
            EndTime = e.EndTime;
            Time = e.Time;
            Date = e.Date;
        }

        private void OnProjectFormCreating(IProjectFormViewModelFactory projectFormViewModelFactory)
        {
            var s = projectFormViewModelFactory.Create(_projectId);
            ProjectFormViewModel = s;
            ProjectFormViewModel.ProjectSelected += OnProjectSelected;
            ProjectFormViewModel.Closed += OnProjectFromClosed;
        }

        private void OnProjectFromClosed(object? sender, EventArgs e)
        {
            if(ProjectFormViewModel is not null)
            { 
                ProjectFormViewModel.Dispose();
                ProjectFormViewModel = null;
            }
        }

        private void OnProjectSelected(object? sender, ProjectDTO e)
        {
            ProjectName = e.Name;
            ProjectId = e.Id;
        }
    }
}

﻿using CommunityToolkit.Mvvm.Input;
using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.ViewModels.Factories;

namespace TimaProject.Desctop.ViewModels
{
    internal abstract class RecordViewModelBase : ViewModelBase, IRecordViewModelBase
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
                SetValue(ref  _startTime, value);
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

        private string _projectName;

        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                SetValue(ref _projectName, value);
            }
        }

        private Guid _projectId;

        public Guid ProjectId
        {
            get
            {
                return _projectId;
            }
            private set
            {
                SetValue(ref _projectId, value);
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
                SetValue(ref _timeFormViewModel, value);
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
                SetValue(ref _projectFormViewModel, value);
                OnPropertyChanged(nameof(IsTimeFormOpened));

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
            _timeFormViewModel = timeFormViewModelFactory.Create(new TimeDTO(StartTime, EndTime, Time, Date));
            _timeFormViewModel.TimeSelected += OnTimeSelected;
            _timeFormViewModel.Closed += OnTimeFormClosed;
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
        }

        private void OnProjectFormCreating(IProjectFormViewModelFactory projectFormViewModelFactory)
        {
            _projectFormViewModel = projectFormViewModelFactory.Create(_projectId);
            _projectFormViewModel.ProjectSelected += OnProjectSelected;
            _projectFormViewModel.Closed += OnProjectFromClosed;
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
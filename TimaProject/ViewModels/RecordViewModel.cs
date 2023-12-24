﻿using FluentValidation;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TimaProject.Commands;
using TimaProject.Models;


namespace TimaProject.ViewModels
{
    public class RecordViewModel :NotifyDataErrorViewModel, IRecordViewModel
    {
        protected readonly AbstractValidator<RecordViewModel> _validator;

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

        private bool _isActive;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                SetValue(ref _isActive, value);
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

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
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

        public string Time { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RecordViewModel(AbstractValidator<RecordViewModel> validator)
        {
            _startTime = string.Empty;
            _endTime = string.Empty;
            _project = Project.Empty;
            _date = string.Empty;
            _title = string.Empty;
            _isActive = false;

            _validator = validator;
        }

        protected override void Validate(string propertyName)
        {
            var validationResult = _validator.Validate(this);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    if (error.PropertyName == propertyName)
                    {
                        AddError(propertyName, error.ErrorMessage);
                    }
                }
            }
        }
    }
}

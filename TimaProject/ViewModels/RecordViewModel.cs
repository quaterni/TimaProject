using FluentValidation;
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
    public class RecordViewModel :NotifyDataErrorViewModel
    {
        protected readonly AbstractValidator<RecordViewModel> _validator;

        private string? _endTime;

        public string? EndTime
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

        public RecordViewModel(AbstractValidator<RecordViewModel> validator)
        {
            _startTime = string.Empty;
            _endTime = null;
            _date = string.Empty;
            _title = string.Empty;

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

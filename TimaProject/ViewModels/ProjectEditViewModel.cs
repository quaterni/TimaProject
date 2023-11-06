using FluentValidation;
using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels
{
    public class ProjectEditViewModel : NotifyDataErrorViewModel
    {
        private readonly AbstractValidator<ProjectEditViewModel> _validator;

        public ProjectEditViewModel(AbstractValidator<ProjectEditViewModel> validator)
        {
            _name = string.Empty;
            _validator = validator;
        }

        private string _name;

        public string Name
        {
            get 
            {
                return _name; 
            }
            set
            { 
                SetValue(ref _name, value);
            }
        }

        protected override void Validate(string propertyName)
        {
            if(propertyName == nameof(Name))
            {
                var validationResult = _validator.Validate(this);
                if (!validationResult.IsValid)
                {
                    foreach(var error in validationResult.Errors)
                    {
                        if(error.PropertyName == nameof(Name))
                        {
                            AddError(propertyName, error.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}

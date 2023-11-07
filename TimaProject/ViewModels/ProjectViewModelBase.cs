using FluentValidation;
using MvvmTools.Base;

namespace TimaProject.ViewModels
{
    public class ProjectViewModelBase : NotifyDataErrorViewModel
    {
        private readonly AbstractValidator<ProjectViewModelBase> _validator;

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

        public ProjectViewModelBase(AbstractValidator<ProjectViewModelBase> validator)
        {
            _validator = validator;
            _name = string.Empty;
        }

        protected override void Validate(string propertyName)
        {
            if (propertyName == nameof(Name))
            {
                var validationResult = _validator.Validate(this);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        if (error.PropertyName == nameof(Name))
                        {
                            AddError(propertyName, error.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}
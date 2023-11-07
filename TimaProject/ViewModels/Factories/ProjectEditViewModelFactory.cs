using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels.Factories
{
    public class ProjectEditViewModelFactory
    {
        private readonly AbstractValidator<ProjectViewModelBase> _validator;

        public ProjectEditViewModelFactory(AbstractValidator<ProjectViewModelBase> validator)
        {
            this._validator = validator;
        }

        public ProjectEditViewModel Create()
        {
            return new ProjectEditViewModel(_validator);
        }
    }
}

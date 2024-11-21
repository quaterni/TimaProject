using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.Validators
{
    internal class ProjectNameValidator : AbstractValidator<string>
    {
        public ProjectNameValidator(IProjectService projectService)
        {
            RuleFor(x => x).NotEmpty().WithMessage("Project name not empty");
            RuleFor(x => x).Must(x => !projectService.IsProjectNameExisting(x)).WithMessage("Project existing");
        }

    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.Desctop.ViewModels.Validators
{
    public class ProjectNameValidator : AbstractValidator<IProjectName>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectNameValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name must be not empty");
            RuleFor(s => s.Name)
                .Must(name => !_projectRepository.Contains(name))
                .WithMessage("Same project name already exist");
        }
    }
}

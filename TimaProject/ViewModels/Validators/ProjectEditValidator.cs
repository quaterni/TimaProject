﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Repositories;

namespace TimaProject.ViewModels.Validators
{
    public class ProjectEditValidator : AbstractValidator<ProjectEditViewModel>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectEditValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            RuleFor(s=> s.Name).NotEmpty().WithMessage("Name must be not empty");
            RuleFor(s=> s.Name)
                .Must(name => !_projectRepository.Contains(name))
                .WithMessage("Project with that name exists");
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels.Factories
{
    public class ProjectViewModelFactory
    {
        private readonly IProjectRepository _projectRepository;

        private readonly  AbstractValidator<ProjectViewModelBase> _validator;
        private readonly ProjectEditViewModelFactory _projectEditViewModelFactory;

        public ProjectViewModelFactory(
            IProjectRepository projectRepository,
            AbstractValidator<ProjectViewModelBase> validator, 
            ProjectEditViewModelFactory projectEditViewModelFactory)
        {
            _projectRepository = projectRepository;
            _validator = validator;
            _projectEditViewModelFactory = projectEditViewModelFactory;
        }

        public ProjectViewModel Create(Project project)
        {
           return new ProjectViewModel(
                project,
                _projectRepository,
                _projectEditViewModelFactory,
                _validator);
        }
    }
}

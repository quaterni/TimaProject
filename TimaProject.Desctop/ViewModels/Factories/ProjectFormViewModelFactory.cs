using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class ProjectFormViewModelFactory : IProjectFormViewModelFactory
    {
        private readonly IValidator<string> _validator;
        private readonly IProjectContainerFactory _projectContainerFactory;
        private readonly IProjectService _projectService;

        public ProjectFormViewModelFactory(IValidator<string> validator, IProjectContainerFactory projectContainerFactory, IProjectService projectService)
        {
            _validator = validator;
            _projectContainerFactory = projectContainerFactory;
            _projectService = projectService;
        }

        public IProjectFormViewModel Create(Guid projectId)
        {
            return new ProjectFormViewModel(
                projectId,
                _validator,
                _projectService,
                _projectContainerFactory);
        }
    }
}

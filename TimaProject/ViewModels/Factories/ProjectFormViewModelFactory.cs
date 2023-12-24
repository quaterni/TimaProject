using FluentValidation;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels.Factories
{
    public class ProjectFormViewModelFactory
    {
        private readonly IProjectRepository _projectRepository;
        private readonly AbstractValidator<IProjectName> _validator;
        private readonly INavigationService _closeProjectFormNavigationService;

        public ProjectFormViewModelFactory(
            IProjectRepository projectRepository,
            AbstractValidator<IProjectName> validator,
            INavigationService closeProjectFormNavigationService
            )
        {
            _projectRepository = projectRepository;
            _validator = validator;
            _closeProjectFormNavigationService = closeProjectFormNavigationService;
        }

        public ProjectFormViewModel Create(IRecordViewModel sourceRecord)
        {
            return new ProjectFormViewModel(
                sourceRecord,
                _projectRepository,
                _validator,
                _closeProjectFormNavigationService);
        }
    }
}

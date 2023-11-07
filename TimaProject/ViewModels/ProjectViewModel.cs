using FluentValidation;
using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Exceptions;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class ProjectViewModel : ProjectEditViewModel
    {
        private Project _project;

        private readonly IProjectRepository _projectRepository;

        private readonly ProjectEditViewModel _editViewModel;

        private bool _isEmpty;

        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        public ProjectEditViewModel ProjectEditViewModel => _editViewModel;


        public ProjectViewModel(
            Project project,
            IProjectRepository projectRepository,
            ProjectEditViewModelFactory projectEditViewModelFactory,
            AbstractValidator<ProjectViewModelBase> validator) :base(validator)
        {
            _project = project;
            _projectRepository = projectRepository;
            _editViewModel = projectEditViewModelFactory.Create();

            _isEmpty = Project.Empty.Equals(_project);
            Name = project.Name;

            _editViewModel.PropertyChanged += OnProjectEditChanged;
            this.PropertyChanged += OnUpdateProject;
        }


        private void OnUpdateProject(object? sender, PropertyChangedEventArgs e)
        {
            if (IsEmpty)
            {
                throw new ChangeEmptyProjectException();
            }
            if(e.PropertyName == null || HasPropertyErrors(e.PropertyName))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(Name):
                    _project = _project with { Name = Name };
                    _projectRepository.UpdateProject(_project);
                    break;
            }
        }

        private void OnProjectEditChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (IsEmpty)
            {
                throw new ChangeEmptyProjectException();
            }
            if (e.PropertyName == null || ProjectEditViewModel.HasPropertyErrors(e.PropertyName))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(ProjectEditViewModel.Name):
                    Name = ProjectEditViewModel.Name;
                    break;
            }
        }

        public void RemoveProject()
        {
            if (IsEmpty)
            {
                throw new ChangeEmptyProjectException();
            }
            _projectRepository.RemoveProject(_project);
        }

    }
}

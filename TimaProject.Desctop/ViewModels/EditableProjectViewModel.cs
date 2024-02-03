using FluentValidation;
using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TimaProject.Desctop.Commands;
using TimaProject.Desctop.Exceptions;
using TimaProject.Desctop.ViewModels.Factories;
using System.ComponentModel.DataAnnotations;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.DataAccess.Exceptions;

namespace TimaProject.Desctop.ViewModels
{
    public class EditableProjectViewModel : NotifyDataErrorViewModel, IProjectName
    {
        private readonly AbstractValidator<IProjectName> _validator;

        private readonly IProjectRepository _projectRepository;

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

        private Project _project;

        public Project Project
        {
            get
            {
                return _project;
            }
            private set
            {
                SetValue(ref _project, value);
            }
        }

        public ICommand RemoveProjectCommand { get; }

        public EditableProjectViewModel(
            Project project,
            IProjectRepository projectRepository,
            AbstractValidator<IProjectName> validator) : base()
        {
            if (project.Equals(Project.Empty))
            {
                throw new ChangeEmptyProjectException("EditableProjectViewModel cannot be empty project");
            }

            _validator = validator;
            _project = project;
            _projectRepository = projectRepository;

            _name = project.Name;

            PropertyChanged += OnUpdateProject;
            _projectRepository.RepositoryChanged += OnRepositoryChanged;

            RemoveProjectCommand = new CommandCallback((e) => RemoveProject());
        }

        private void OnRepositoryChanged(object? sender, RepositoryChangedEventArgs<Project> e)
        {
            if (e.Item.Id.Equals(_project.Id) && e.Operation == RepositoryChangedOperation.Update)
            {
                Project = e.Item;
            }
        }

        private void OnUpdateProject(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == null || HasPropertyErrors(e.PropertyName))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(Name):
                    Project = Project with { Name = Name };
                    _projectRepository.UpdateItem(Project);
                    break;
            }
        }

        public void RemoveProject()
        {
            _projectRepository.RemoveItem(Project);
        }

        public void UpdateProject()
        {
            _projectRepository.UpdateItem(Project);
        }

        protected override void Validate(string propertyName)
        {
            ClearAllErrors();
            var validationResult = _validator.Validate(this);
            if (validationResult.IsValid)
            {
                return;
            }

            foreach (var error in validationResult.Errors)
            {
                if (error is null)
                {
                    continue;
                }
                AddError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}

using FluentValidation;
using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Commands;
using TimaProject.Exceptions;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Containers;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class ProjectFormViewModel :NotifyDataErrorViewModel, IProjectName
    {
        private readonly AbstractValidator<IProjectName> _validator;

        private readonly IProjectRepository _projectRepository;

        private readonly IRecordViewModel _source;

        private bool _isCanAdd;

        public bool IsCanAdd => _isCanAdd;

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

        private ObservableCollection<ProjectContainerViewModel> _projects;

        public ObservableCollection<ProjectContainerViewModel> Projects
        {
            get
            { 
                return _projects;
            }
            set 
            { 
                SetValue(ref _projects, value);
            }
        }
        public ICommand AddProjectCommand { get; }

        public ICommand CloseProjectFormCommand { get; }

        public ICommand SelectProjectCommand { get; }


        public event EventHandler<EventArgs>? Closed;

        public ProjectFormViewModel(
            IRecordViewModel sourceRecord,
            IProjectRepository projectRepository, 
            AbstractValidator<IProjectName> validator,
            INavigationService closeProjectFormNavigationService)
        {
            _name = string.Empty;
            _source = sourceRecord;
            _projectRepository = projectRepository;
            _validator = validator;
            _isCanAdd = false;
            _projects = MakeListingProject();

            CloseProjectFormCommand = new NavigationCommand(closeProjectFormNavigationService);
            AddProjectCommand = new CommandCallback((e)=> OnAddProjectCommand(e));
            SelectProjectCommand = new CommandCallback(e => OnSelectProjectCommand(e));
            PropertyChanged += OnIsCanAddChanged;
            _projectRepository.RepositoryChanged += OnRepositoryChanged;
        }

        private void OnRepositoryChanged(object? sender, RepositoryChangedEventArgs<Project> e)
        {
            Projects = MakeListingProject();
        }

        private void OnSelectProjectCommand(object? e)
        {
            if(e is not ProjectContainerViewModel container)
            {
                throw new ArgumentException("Command parameter must be ProjectContainerViewModel type");
            }
            Project project = container.IsEmpty ? Project.Empty : container.Item!.Project;
            SetProjectToSource(project);
        }

        public override void Dispose()
        {
            _projectRepository.RepositoryChanged -= OnRepositoryChanged;
            PropertyChanged -= OnIsCanAddChanged;
            base.Dispose();
        }


        private ObservableCollection<ProjectContainerViewModel> MakeListingProject()
        {
            var result = new ObservableCollection<ProjectContainerViewModel>
            {
                new ProjectContainerViewModel(null, true, _source.Project.Equals(Project.Empty))
            };
            var projects = _projectRepository.GetItems((e) => true).OrderBy(x=> x.Name);
            foreach (var project in projects)
            {
                result.Add(new ProjectContainerViewModel(
                    new EditableProjectViewModel(project, _projectRepository, _validator),
                    false,
                    _source.Project.Equals(project)));
            }
            return result;
        }

        private void OnIsCanAddChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Name))
            {
                _isCanAdd = !HasErrors;
                OnPropertyChanged(nameof(IsCanAdd));
            }
        }

        private void OnAddProjectCommand(object? e)
        {
            if (!IsCanAdd)
            {
                throw new AddingInvalidProjectException();
            }
            var project = new Project(Name, Guid.NewGuid());
            _projectRepository.AddItem(project);
            SetProjectToSource(project);
        }


        protected void SetProjectToSource(Project project)
        {
            _source.Project = project;
            CloseProjectFormCommand.Execute(null);
        }

        protected override void Validate(string propertyName)
        {
            ClearAllErrors();
            var validationResult = _validator.Validate(this);
            if (validationResult.IsValid)
            {
                return;
            }
            foreach(var error in  validationResult.Errors)
            {
                if(error is null)
                {
                    continue;
                }
                AddError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}

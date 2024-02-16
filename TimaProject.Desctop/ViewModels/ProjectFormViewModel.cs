using FluentValidation;
using MvvmTools.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TimaProject.Desctop.Commands;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.Factories;
using System.Collections;
using FluentValidation.Results;
using CommunityToolkit.Mvvm.Input;


namespace TimaProject.Desctop.ViewModels
{
    internal class ProjectFormViewModel : ViewModelBase, IProjectFormViewModel
    {
        private readonly IValidator<string> _nameValidator;

        private readonly IProjectService _projectService;

        private readonly IProjectContainerFactory _projectContainerFactory;

        private readonly Guid _selectedProjectId;

        private string _newProjectName;

        public string NewProjectName
        {
            get
            {
                return _newProjectName;
            }
            set
            {
                SetValue(ref _newProjectName, value);
            }
        }

        private Lazy<ObservableCollection<IProjectContainerViewModel>> _lazyProjects;

        public ICommand SelectProjectCommand { get; }

        public ObservableCollection<IProjectContainerViewModel> Projects
        {
            get
            {
                return _lazyProjects.Value;
            }
        }

        private bool _canAddNewProject;

        public bool CanAddNewProject
        {
            get
            {
                return _canAddNewProject;
            }
            set
            {
                SetValue(ref _canAddNewProject, value);
            }
        }

        public ICommand AddNewProjectCommand { get; }

        public ICommand CloseCommand { get; }


        public event EventHandler<EventArgs> Closed;

        public event EventHandler<ProjectDTO> ProjectSelected;


        public ProjectFormViewModel(
            Guid selectedProjectId,
            IValidator<string> nameValidator,
            IProjectService projectService,
            IProjectContainerFactory projectContainerFactory)
        {
            _selectedProjectId = selectedProjectId;
            _newProjectName = string.Empty;
            _nameValidator = nameValidator;
            _projectService = projectService;
            _projectContainerFactory = projectContainerFactory;
            _lazyProjects = new Lazy<ObservableCollection<IProjectContainerViewModel>>(MakeListingProject);

            AddNewProjectCommand = new CommandCallback(OnAddProjectCommand);
            SelectProjectCommand = new CommandCallback(OnSelectProjectCommand);
            CloseCommand = new RelayCommand(Close);
            PropertyChanged += OnNewProjectNameChanged;
        }

        private void OnSelectProjectCommand(object? e)
        {
            if (e is not Guid projectId)
            {
                throw new ArgumentException("Command parameter must be project id");
            }
            var projectContainer = Projects.FirstOrDefault(p=> p.Id.Equals(projectId));
            if (projectContainer is null)
            {
                throw new ArgumentException("Project Id not found");
            }
            var project = new ProjectDTO(projectContainer.Name, projectContainer.Id, projectContainer.IsEmpty);
            SetProjectToSource(project);
        }



        private ObservableCollection<IProjectContainerViewModel> MakeListingProject()
        {
            var result = _projectService
                .GetProjects()
                .Select(p => _projectContainerFactory.Create(p, p.Id.Equals(_selectedProjectId)))
                .OrderBy(p=> p.Name)
                .ToList();
            result.Insert(0, _projectContainerFactory.CreateEmpty(_selectedProjectId.Equals(Guid.Empty)));
            return new ObservableCollection<IProjectContainerViewModel>(result);
        }

        private void OnNewProjectNameChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName is nameof(NewProjectName))
            {
                ValidationResult result = _nameValidator.Validate(NewProjectName);
                CanAddNewProject = result.IsValid;
            }
        }


        private void OnAddProjectCommand(object? e)
        {
            var projectDTO = new ProjectDTO(NewProjectName, Guid.NewGuid(), false);
            _projectService.AddProject(projectDTO);
            SetProjectToSource(projectDTO);
        }


        protected void SetProjectToSource(ProjectDTO project)
        {
            ProjectSelected?.Invoke(this, project);
            Close();
        }

        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

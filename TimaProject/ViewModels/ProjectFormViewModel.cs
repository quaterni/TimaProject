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
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class ProjectFormViewModel :ViewModelBase
    {
        private readonly IProjectRepository _projectRepository;

        private readonly ProjectViewModelFactory _projectViewModelFactory;

        private readonly ProjectEditViewModel _addingProjectEditViewModel;

        public ProjectEditViewModel AddingProjectEditViewModel => _addingProjectEditViewModel;

        private bool _isCanAdd;

        public bool IsCanAdd => _isCanAdd;

        private ObservableCollection<ProjectViewModel> _projects;

        public ObservableCollection<ProjectViewModel> Projects
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

        private Project _selectedProject;

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            private set
            {
                SetValue(ref _selectedProject, value);
            }
        }

        private ProjectViewModel? _selectedProjectViewModel;

        public ProjectViewModel? SelectedProjectViewModel
        {
            get
            { 
                return _selectedProjectViewModel; 
            }
            set
            {
                SetValue(ref _selectedProjectViewModel, value);
                OnSelectedProjectChanged();
            }
        }


        public ICommand CloseProjectFormCommand { get; }

        public ProjectFormViewModel(
            Project sourceProject,
            IProjectRepository projectRepository, 
            ProjectViewModelFactory projectViewModelFactory,
            ProjectEditViewModelFactory projectEditViewModelFacotry,
            INavigationService closeProjectFormNavigationService)
        {
            _selectedProject = sourceProject;
            _isCanAdd = false;
            _projectRepository = projectRepository;
            _projectViewModelFactory = projectViewModelFactory;
            _addingProjectEditViewModel = projectEditViewModelFacotry.Create();
            _addingProjectEditViewModel.PropertyChanged += OnEditProjectViewModelChanged;
            _projectRepository.RepositoryChanged += OnRepositoryChanged;
            _projects = new ObservableCollection<ProjectViewModel>();
            CloseProjectFormCommand = new NavigationCommand(closeProjectFormNavigationService);
            UpdateProjects();
        }


        private void OnSelectedProjectChanged()
        {
            if(_selectedProjectViewModel is null)
            {
                throw new ArgumentNullException(nameof(SelectedProjectViewModel));
            }
            SelectedProject = _projectRepository.
                GetAllProjects()
                .Where(p => p.Name.Equals(_selectedProjectViewModel.Name)).First();
        }


        private void OnRepositoryChanged(object? sender, EventArgs e)
        {
            UpdateProjects();
        }

        private void UpdateProjects()
        {
            var projects = _projectRepository.GetAllProjects();
            _projects.Clear();
            foreach (var project in projects)
            {
                _projects.Add(_projectViewModelFactory.Create(project));
            } 
            var projectViewModels = Projects.Where(projectVM => projectVM.Name.Equals(_selectedProject.Name));
            if(projectViewModels.Count() == 0)
            {
                SelectedProject = Project.Empty;
                return;
            }

            _selectedProjectViewModel = projectViewModels.First();
            OnPropertyChanged(nameof(SelectedProjectViewModel));
        }

        private void OnEditProjectViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ProjectEditViewModel.Name))
            {
                _isCanAdd = !_addingProjectEditViewModel.HasPropertyErrors(nameof(ProjectEditViewModel.Name));
                OnPropertyChanged(nameof(IsCanAdd));
            }
        }

        public void AddProject()
        {
            if (IsCanAdd)
            {
                var project = new Project(
                    AddingProjectEditViewModel.Name,
                    _projectRepository.GetId());
                _projectRepository.AddProject(project);
                SelectedProject = project;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{
    public class ProjectFormViewModelTests
    {
        private readonly IProjectRepository _projectRepository;

        private readonly ProjectFormViewModel _sut;

        public ProjectFormViewModelTests()
        {
            _projectRepository = new ProjectRepository();
            var validator = new ProjectValidator(_projectRepository);

            _sut = new ProjectFormViewModel(
                Project.Empty,
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());
        }

        [Fact]
        public void PropertyChanged_OnIsCanAdd_WhenNameOfProjectEditViewModelChanged_Invoke()
        {
            var result = Assert.Raises<PropertyChangedEventArgs>(
                e => _sut.PropertyChanged += new PropertyChangedEventHandler(e),
                e => _sut.PropertyChanged -= new PropertyChangedEventHandler(e),
                () => _sut.AddingProjectEditViewModel.Name = "Test");

            Assert.Equal(
                nameof(ProjectFormViewModel.IsCanAdd), 
                result.Arguments.PropertyName);
        }

        [Fact]
        public void IsCanAdd_WhenNameOfProjectEditViewModelIsValid_ReturnTrue()
        {
            _sut.AddingProjectEditViewModel.Name = "Test";

            Assert.False(
                _sut.AddingProjectEditViewModel.HasPropertyErrors(nameof(ProjectEditViewModel.Name)));

            Assert.True(_sut.IsCanAdd);
        }

        [Fact]
        public void IsCanAdd_WhenNameOfProjectEditViewModelIsEmpty_ReturnFalse()
        {
            Assert.False(_sut.IsCanAdd);
        }

        [Fact]
        public void IsCanAdd_WhenNameOfProjectEditViewModelNotValid_ReturnFalse()
        {
            _projectRepository.AddProject(new Models.Project("MyProject", 1));
            _sut.AddingProjectEditViewModel.Name = "MyProject";
            Assert.False(_sut.IsCanAdd);
        }

        [Fact]
        public void Projects_AfterInitialization_ContainsProjectsFromRepository()
        {
            var project1 = new Project("MyFirstProject", _projectRepository.GetId());
            _projectRepository.AddProject(project1);

            var project2 = new Project("MySecondProject", _projectRepository.GetId());
            _projectRepository.AddProject(project2);

            var validator = new ProjectValidator(_projectRepository);
            var sut = new ProjectFormViewModel(
                Project.Empty,
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());

            var projects = _projectRepository.GetAllProjects();
            Assert.Equal(projects.Count(), sut.Projects.Count());
            for(int i =0; i< projects.Count; i++)
            {
                Assert.Equal(projects[i].Name, sut.Projects[i].Name);
            }
        }

        [Fact]
        public void Projects_WhenAddedNewProject_Updates()
        {
            var project = new Project("NewProject", _projectRepository.GetId());

            Assert.Empty(_sut.Projects.Where(projectVM => projectVM.Name.Equals(project.Name)));
            _projectRepository.AddProject(project);
            Assert.Contains(_sut.Projects, projectVM => projectVM.Name.Equals(project.Name));
        }

        [Fact]
        public void Projects_WhenUpdateProject_Updates()
        {
            var project = new Project("NewProject", _projectRepository.GetId());
            _projectRepository.AddProject(project);
            var expectedName = "TestName";
            Assert.Empty(_sut.Projects.Where(projectVM => projectVM.Name.Equals(expectedName)));
            _projectRepository.UpdateProject(project with { Name = expectedName});
            Assert.Contains(_sut.Projects, projectVM => projectVM.Name.Equals(expectedName));
        }

        [Fact]
        public void Projects_WhenRemoveProject_Updates()
        {
            var project = new Project("NewProject", _projectRepository.GetId());
            _projectRepository.AddProject(project);
            Assert.Contains(_sut.Projects, projectVM => projectVM.Name.Equals(project.Name));
            _projectRepository.RemoveProject(project);
            Assert.Empty(_sut.Projects.Where(projectVM => projectVM.Name.Equals(project.Name)));
        }

        [Fact]
        public void AddProject_WhenIsCanAdd_AddNewProjectWithNameFromProjectEdit()
        {
            _sut.AddingProjectEditViewModel.Name = "New project";
            Assert.False(_projectRepository.Contains("New project"));

            Assert.True(_sut.IsCanAdd);

            _sut.AddProject();

            Assert.True(_projectRepository.Contains("New project"));
        }

        [Fact]
        public void AddProject_WhenAddNewProject_SetProjectToCurrentProject()
        {
            _sut.AddingProjectEditViewModel.Name = "New project";
            Assert.True(_sut.IsCanAdd);
            _sut.AddProject();

            Assert.Equal("New project", _sut.SelectedProject.Name);
        }

        [Fact]
        public void SelectedProject_WhenItNotFoundInRepository_SetEmptyProject()
        {
            var validator = new ProjectValidator(_projectRepository);
            var sut = new ProjectFormViewModel(
                new Project("New project", 10),
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());
            Assert.Equal(Project.Empty, sut.SelectedProject);
        }

        [Fact]
        public void SelectedProject_WhenItNotFoundInRepository_AfterUpdate_SetEmptyProject()
        {
            var project = new Project("New project", _projectRepository.GetId());
            _projectRepository.AddProject(project);
            var validator = new ProjectValidator(_projectRepository);
            var sut = new ProjectFormViewModel(
                project,
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());
            Assert.Equal(project, sut.SelectedProject);
            _projectRepository.RemoveProject(project);

            Assert.Equal(Project.Empty, sut.SelectedProject);
        }

        [Fact]
        public void SelectedProjectViewModel_AfterUpdateRepository_ContainsInProjects()
        {
            var project = new Project("New project", _projectRepository.GetId());
            _projectRepository.AddProject(project);
            var validator = new ProjectValidator(_projectRepository);
            var sut = new ProjectFormViewModel(
                project,
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());

            Assert.Equal(project, sut.SelectedProject);

            _projectRepository.AddProject(new Project("Project1", _projectRepository.GetId()));
            var refToSelected = sut.SelectedProjectViewModel;
            Assert.Contains(refToSelected, sut.Projects);
            Assert.Equal(refToSelected.Name, project.Name);

            _projectRepository.AddProject(new Project("Project2", _projectRepository.GetId()));
            refToSelected = sut.SelectedProjectViewModel;
            Assert.Contains(refToSelected, sut.Projects);
            Assert.Equal(refToSelected.Name, project.Name);

        }

        [Fact]
        public void SelectedProjectViewModel_AfterChanged_SetSelectedProject()
        {
            var project1 = new Project("FirstProject", _projectRepository.GetId());
            _projectRepository.AddProject(project1);
            var project2 = new Project("SecondProject", _projectRepository.GetId());
            _projectRepository.AddProject(project2);
            var validator = new ProjectValidator(_projectRepository);
            var sut = new ProjectFormViewModel(
                project1,
                _projectRepository,
                new ViewModels.Factories.ProjectViewModelFactory(
                    _projectRepository,
                    validator,
                    new ViewModels.Factories.ProjectEditViewModelFactory(validator)),
                new ViewModels.Factories.ProjectEditViewModelFactory(validator),
                new MockNavigationService());

            Assert.Equal(project1, sut.SelectedProject);

            var newSelectedViewModel = sut.Projects.Last();
            Assert.Equal(project2.Name, newSelectedViewModel.Name);
            sut.SelectedProjectViewModel = newSelectedViewModel;
            Assert.Equal(project2.Name, sut.SelectedProject.Name);
        }
    }
}

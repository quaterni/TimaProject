using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Exceptions;
using TimaProject.ViewModels.Factories;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{
    public class ProjectViewModelTests
    {
        private readonly IProjectRepository _repository;

        private readonly ProjectEditViewModel _editViewModel;

        public ProjectViewModelTests()
        {
            _repository = new ProjectRepository();


        }

        public ProjectViewModel Create(Project project)
        {
            return new ProjectViewModel(
                project,
                _repository,
                new ProjectEditViewModelFactory(new ProjectValidator(_repository)),
                new ProjectValidator(_repository));

        }

        [Fact]
        public void IsEmpty_WhenProjectEmpty_ReturnTrue()
        {
            var sut = Create(Project.Empty);

            Assert.True(sut.IsEmpty);
        }

        [Fact]
        public void IsEmpty_WhenProjectNotEmpty_ReturnFalse()
        {
            var sut = Create(new Project("MyProject", _repository.GetId()));

            Assert.False(sut.IsEmpty);
        }

        [Fact]
        public void RemoveProject_WhenProjectIsEmpty_ThrowsException()
        {
            var sut = Create(Project.Empty);

            Assert.Throws<ChangeEmptyProjectException>(() => sut.RemoveProject());
        }

        [Fact]
        public void OnUpdateProject_WhenProjectIsEmpty_ThrowsException()
        {
            var sut = Create(Project.Empty);

            Assert.Throws<ChangeEmptyProjectException>(() => sut.Name = "Name");
        }

        [Fact]
        public void Name_AfterProjectInits_EqualsNameFromProject()
        {
            var project = new Project("MyProject", _repository.GetId());
            var sut = Create(project);

            Assert.Equal(project.Name ,sut.Name);
        }

        [Fact]
        public void OnUpdateProject_IfChangedNameValid_UpdateNameInRepository()
        {
            var expectedName = "NewName";
            var project = new Project("MyProject", _repository.GetId());
            _repository.AddProject(project);
            var sut = Create(project);
            Assert.False(_repository.Contains(expectedName));
            sut.Name = expectedName;

            Assert.True(_repository.Contains(expectedName));
        }


        [Fact]
        public void OnUpdateProject_IfChangedNameInvalid_PassUpdateNameInRepository()
        {
            var expectedName = "MyProject";
            var project = new Project(expectedName, _repository.GetId());
            _repository.AddProject(project);
            var sut = Create(project);
            sut.Name = "";

            var updatedProject = _repository.GetAllProjects().First();

            Assert.True(_repository.Contains(expectedName));
        }

        [Fact]
        public void RemoveProject_WhenProjectNotEmpty_RemoveProject()
        {
            var project = new Project("MyProject", _repository.GetId());
            _repository.AddProject(project);

            var sut = Create(project);
            Assert.True(_repository.Contains(project.Id));

            sut.RemoveProject();

            Assert.False(_repository.Contains(project.Id));
        }

        [Fact]
        public void OnProjectEditChanged_WhenNameValid_UpdateName()
        {
            var expectedName = "NewName";
            var project = new Project("MyProject", _repository.GetId());
            _repository.AddProject(project);

            var sut = Create(project);

            sut.ProjectEditViewModel.Name = expectedName;

            Assert.Equal(expectedName, sut.Name);
        }

        [Fact]
        public void OnProjectEditChanged_WhenProjectEmpty_ThrowExcepiton()
        {
            var sut = Create(Project.Empty);

            Assert.Throws<ChangeEmptyProjectException>(()=> sut.ProjectEditViewModel.Name = "test");

        }

        [Fact]
        public void OnProjectEditChanged_WhenNameIsInvalid_DoesntChangedName()
        {
            var expectedName = "ProjectName";
            var sut = Create(new Project(expectedName, _repository.GetId()));
            _repository.AddProject(new Project("MyProject", _repository.GetId()));

            sut.ProjectEditViewModel.Name = "MyProject";

            Assert.True(sut.ProjectEditViewModel.HasPropertyErrors(nameof(ProjectEditViewModel.Name)));
            Assert.Equal(expectedName, sut.Name);
        }
    }
}

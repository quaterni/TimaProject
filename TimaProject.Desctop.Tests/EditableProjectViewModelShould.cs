using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Exceptions;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Factories;
using TimaProject.Desctop.ViewModels.Validators;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class EditableProjectViewModelShould
    {

        private readonly EditableProjectViewModel _sut;
        private readonly AbstractValidator<IProjectName> _validator;
        private readonly Mock<IProjectRepository> _mockRepository;
        private static readonly Project _project = new Project("MyProject", Guid.NewGuid());

        public EditableProjectViewModelShould()
        {
            _mockRepository = new Mock<IProjectRepository>();
            _validator = new ProjectNameValidator(_mockRepository.Object);
            _sut = new EditableProjectViewModel(
                _project, 
                _mockRepository.Object,
                _validator);
            _mockRepository.Setup(x => x.Contains("NotValidName")).Returns(true);


        }

        [Fact]
        public void NameEqualProjectName_AfterInitialization()
        {
            Assert.Equal(_sut.Name, _project.Name);
        }

        [Fact]
        public void UpdateProject_IfSetCorrectName()
        {
            var newName = "NewName";
            _mockRepository.Setup(x=> x.Contains(newName)).Returns(false);
            _mockRepository
                .Setup(x => x.UpdateItem(It.IsAny<Project>()))
                .Callback<Project>(e => Assert.Equal(e, _project with { Name = newName }));
            _sut.Name = newName;
            Assert.Equal(newName, _sut.Project.Name);
            _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public void NotUpdateProject_IfSetIncorrectName()
        {
            var newName = "NotValidName";
            _mockRepository
                .Setup(x => x.UpdateItem(It.IsAny<Project>()))
                .Callback<Project>(e => Assert.Equal(e, _project with { Name = newName }));
            _sut.Name = newName;
            Assert.NotEqual(newName, _sut.Project.Name);
            _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Project>()), Times.Never);
        }

        [Theory, MemberData(nameof(NameTestData))]
        public void HasPropertyError_OnIncorrectValues(string name, bool expected)
        {
            _sut.Name = name;
            Assert.Equal(expected, _sut.HasPropertyErrors(nameof(EditableProjectViewModel.Name)));
        }

        public static IEnumerable<object[]> NameTestData()
        {
            yield return new object[] { "", true };
            yield return new object[] { "NotValidName", true };
            yield return new object[] { "ValidName", false };
        }

        [Fact]
        public void RemoveProject()
        {
            _sut.RemoveProject();
            _mockRepository.Verify(x => x.RemoveItem(_project), Times.Once);
        }

        [Fact]
        public void RemoveProjectCommand_InvokeRemoveProject()
        {
            _sut.RemoveProjectCommand.Execute(null);
            _mockRepository.Verify(x => x.RemoveItem(_project), Times.Once);
        }

        [Fact]
        public void UpdateProject_WhenRepositoryChanged()
        {
            var repository = new ProjectRepository();
            repository.AddItem(_project);
            var sut = new EditableProjectViewModel(_project, repository, _validator);
            var expectedName = "NewName";

            var updatedProject = _project with { Name = expectedName };
            repository.UpdateItem(updatedProject);

            Assert.Equal(expectedName, sut.Project.Name);

            var newProject = new Project("AnotherProject", Guid.NewGuid());
            repository.AddItem(newProject);

            Assert.Equal(expectedName, sut.Project.Name);

            repository.UpdateItem(newProject);

            Assert.Equal(expectedName, sut.Project.Name);

            repository.RemoveItem(updatedProject);
            Assert.Equal(expectedName, sut.Project.Name);
        }
    }
}

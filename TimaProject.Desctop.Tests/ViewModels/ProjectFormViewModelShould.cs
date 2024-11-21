using FluentValidation;
using Moq;
using TimaProject.Desctop.ViewModels;
using Xunit;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.DTOs;
using AutoFixture;
using FluentAssertions;
using TimaProject.Desctop.Interfaces.Factories;
using FluentValidation.Results;


namespace TimaProject.Desctop.Tests.ViewModels
{
    public class ProjectFormViewModelShould
    {
        private readonly Mock<IProjectService> _mockProjectService;

        private readonly Mock<IValidator<string>> _mockProjectValidator;

        private readonly Mock<IProjectContainerFactory> _mockContainerFactory;

        private readonly ProjectFormViewModel _sut;

        private static readonly IEnumerable<ProjectDTO> _projects = new Fixture().CreateMany<ProjectDTO>(6);

        public ProjectFormViewModelShould()
        {

            _mockProjectService = new Mock<IProjectService>();
            _mockContainerFactory = new Mock<IProjectContainerFactory>();
            _mockProjectValidator = new Mock<IValidator<string>>();

            _sut = new ProjectFormViewModel(
                _projects.First().Id,
                _mockProjectValidator.Object,
                _mockProjectService.Object,
                _mockContainerFactory.Object);

            _mockContainerFactory
                .Setup(s => s.Create(It.IsAny<ProjectDTO>(), It.IsAny<bool>()))
                .Returns<ProjectDTO, bool>((p, s) =>
                {
                    var mock = new Mock<IProjectContainerViewModel>();
                    mock.SetupGet(s => s.Id).Returns(p.Id);
                    mock.SetupGet(s => s.Name).Returns(p.Name);
                    mock.SetupGet(s => s.IsSelected).Returns(s);
                    return mock.Object;
                });
            _mockContainerFactory
                .Setup(s => s.CreateEmpty(It.IsAny<bool>()))
                .Returns<bool>(s =>
                {
                    var mock = new Mock<IProjectContainerViewModel>();
                    mock.SetupGet(s => s.Id).Returns(Guid.Empty);
                    mock.SetupGet(s => s.IsEmpty).Returns(true);
                    mock.SetupGet(s => s.IsSelected).Returns(s);
                    return mock.Object;
                });

            _mockProjectValidator
                .Setup(s => s.Validate(It.IsAny<string>()))
                .Returns(() =>
                {
                    var mock = new Mock<ValidationResult>();
                    mock.SetupGet(s => s.IsValid).Returns(true);
                    return mock.Object;
                });

            _mockProjectService.Setup(s => s.GetProjects()).Returns(_projects);

        }

        [Fact]
        public void Init()
        {
            _sut.NewProjectName.Should().BeEmpty();
            _sut.CanAddNewProject.Should().BeFalse();
        }

        [Fact]
        public void Projects_LazyInits()
        {
            _mockProjectService.Verify(s => s.GetProjects(), Times.Never);

            _ = _sut.Projects;
            _mockProjectService.Verify(s => s.GetProjects(), Times.Once);

            _ = _sut.Projects;

            _mockProjectService.Verify(s => s.GetProjects(), Times.Once);
        }

        [Fact]
        public void Projects_HasSelectedProject()
        {
            var selectedProject = _projects.First();

            _sut.Projects.Should().ContainSingle(p => p.IsSelected);
            _sut.Projects.Should().Contain(p => p.Id.Equals(selectedProject.Id) && p.IsSelected);
        }

        [Fact]
        public void Projects_HasEmptyProjectFirst()
        {
            _sut.Projects.First().IsEmpty.Should().BeTrue();
            _sut.Projects.Should().ContainSingle(p => p.IsEmpty);
        }

        [Fact]
        public void Projects_OrderedByProjectName()
        {
            _sut.Projects.Skip(1).Should().BeInAscendingOrder(p => p.Name);
        }

        [Fact]
        public void SelectProjectCommand_RaisedProjectSelectedEvent()
        {
            var projectId = _projects.Skip(3).First().Id;

            var result = Assert.Raises<ProjectDTO>(
                h => _sut.ProjectSelected += h,
                h => _sut.ProjectSelected -= h,
                () => _sut.SelectProjectCommand.Execute(projectId));

            result.Arguments.Id.Should().Be(projectId);
        }

        [Fact]
        public void NewProjectName_WhenChanged_InvokeValidator()
        {
            _sut.NewProjectName = new Fixture().Create<string>();

            _mockProjectValidator.Verify(s => s.Validate(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NewProjectName_NotValid_CannotAddIt()
        {
            var notValidName = new Fixture().Create<string>();
            _mockProjectValidator
                .Setup(s => s.Validate(notValidName))
                .Returns(() =>
                {
                    var mock = new Mock<ValidationResult>();
                    mock.SetupGet(s => s.IsValid).Returns(false);
                    return mock.Object;
                });


            _sut.NewProjectName = notValidName;

            _sut.CanAddNewProject.Should().BeFalse();
        }

        [Fact]
        public void NewProjectName_HasValidName_CanAddIt()
        {
            var validName = new Fixture().Create<string>();

            _sut.NewProjectName = validName;

            _sut.CanAddNewProject.Should().BeTrue();
        }

        [Fact]
        public void WhenAddCommandExecuted_RaisesProjectSelectedEvent()
        {
            _sut.NewProjectName = new Fixture().Create<string>();

            var result = Assert.Raises<ProjectDTO>(
                h => _sut.ProjectSelected += h,
                h => _sut.ProjectSelected -= h,
                () => _sut.AddNewProjectCommand.Execute(null));

            result.Sender.Should().Be(_sut);
            result.Arguments.Name.Should().Be(_sut.NewProjectName);
        }

        [Fact]
        public void WhenAddCommandExecuted_AddNewProject()
        {
            _sut.NewProjectName = new Fixture().Create<string>();
            _mockProjectService
                .Setup(x => x.AddProject(It.IsAny<ProjectDTO>()))
                .Callback<ProjectDTO>(p =>
                {
                    p.Name.Should().Be(_sut.NewProjectName);
                    p.IsEmpty.Should().BeFalse();
                });


            _sut.AddNewProjectCommand.Execute(null);

            _mockProjectService.Verify(x => x.AddProject(It.IsAny<ProjectDTO>()), Times.Once);

        }

        [Fact]
        public void WhenProjectSelected_RaisesClosedEvent()
        {
            Assert.Raises<EventArgs>(
                h => _sut.Closed += h,
                h => _sut.Closed -= h,
                () => _sut.SelectProjectCommand.Execute(_projects.First().Id));
        }

        [Fact]
        public void WhenProjectAdded_RaisesClosedEvent()
        {
            _sut.NewProjectName = new Fixture().Create<string>();
            Assert.Raises<EventArgs>(
                h => _sut.Closed += h,
                h => _sut.Closed -= h,
                () => _sut.AddNewProjectCommand.Execute(null));
        }

        [Fact]
        public void WhenClosedCommandExecuted_RaisesClosedEvent()
        {
            Assert.Raises<EventArgs>(
                h => _sut.Closed += h,
                h => _sut.Closed -= h,
                () => _sut.CloseCommand.Execute(null));
        }
    }
}

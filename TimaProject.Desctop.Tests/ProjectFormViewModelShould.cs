using FluentValidation;
using Moq;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Exceptions;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Containers;
using TimaProject.Desctop.ViewModels.Validators;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class ProjectFormViewModelShould
    {
        private readonly Mock<IProjectRepository> _mockRepository;

        private readonly AbstractValidator<IProjectName> _validator;

        private readonly Mock<INavigationService> _mockCloseNavigationService;

        private readonly Mock<IRecordViewModel> _mockSource;

        private readonly ProjectFormViewModel _sut;

        private static List<Project> _projects => new List<Project>()
        {
            new Project("First", Guid.NewGuid()),
            new Project("Second", Guid.NewGuid()),
            new Project("Third", Guid.NewGuid())
        };

        public ProjectFormViewModelShould()
        {
            _mockRepository = new Mock<IProjectRepository>();
            _validator = new ProjectNameValidator(_mockRepository.Object);
            _mockCloseNavigationService = new Mock<INavigationService>();
            _mockSource = new Mock<IRecordViewModel>();
            _mockSource.SetupGet(x => x.Project).Returns(Project.Empty);
            _mockRepository.Setup(x => x.Contains("ExistingProject")).Returns(true);
            _mockRepository.Setup(x => x.GetItems(It.IsAny<Func<Project, bool>>())).Returns(_projects);

            _sut = new ProjectFormViewModel(
                _mockSource.Object,
                _mockRepository.Object,
                _validator,
                _mockCloseNavigationService.Object);
        }



        [Fact]
        public void AddNewProject_IfNameValid_WhenAddCommandExecuted()
        {
            var addingProjectName = "MyNewProject";
            _mockRepository
                .Setup(x => x.AddItem(It.IsAny<Project>()))
                .Callback<Project>(x => Assert.Equal(addingProjectName, x.Name));
            _sut.Name = addingProjectName;
            var validationResult = _validator.Validate(_sut);
            Assert.True(validationResult.IsValid);

            _sut.AddProjectCommand.Execute(null);

            _mockRepository.Verify(x => x.AddItem(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public void HaveEmptyNameAndNotCanAdd_AfterInitalization()
        {
            Assert.Empty(_sut.Name);
            Assert.False(_sut.IsCanAdd);
        }

        [Theory, MemberData(nameof(CanAddTestData))]
        public void CanAdd(string name, bool expected)
        {
            _sut.Name = "ValidName";

            _sut.Name = name;
            Assert.Equal(!expected, _sut.HasErrors);
            Assert.Equal(expected, _sut.IsCanAdd);
        }


        public static IEnumerable<object[]> CanAddTestData()
        {
            yield return new object[] { "AnotherValidName", true };
            yield return new object[] { "", false };
            yield return new object[] { "ExistingProject", false };
        }

        [Fact]
        public void SetToSource_IfNameValid_WhenAddProjectCommandExecuted()
        {
            var addingProjectName = "MyNewProject";
            _sut.Name = addingProjectName;
            Assert.True(_sut.IsCanAdd);
            _mockSource
                .SetupSet(x => x.Project)
                .Callback(x => Assert.Equal(addingProjectName, x.Name));

            _sut.AddProjectCommand.Execute(null);
            _mockSource.VerifySet(x => x.Project = It.IsAny<Project>(), Times.Once);
        }

        [Fact]
        public void CloseProjectForm_IfNameValid_WhenAddProjectCommandExecuted()
        {
            var addingProjectName = "MyNewProject";
            _sut.Name = addingProjectName;
            Assert.True(_sut.IsCanAdd);

            _sut.AddProjectCommand.Execute(null);

            _mockCloseNavigationService.Verify(x=> x.Navigate(It.IsAny<object?>()), Times.Once);
        }

        [Fact]
        public void ThrowAddingInvalidProjectException_IfNameInvalid_WhenAddProjectCommandExecuted()
        {
            Assert.Throws<AddingInvalidProjectException>(() => _sut.AddProjectCommand.Execute(null));
        }

        [Fact]
        public void GetProjectsFromRepository_AfterInitialization()
        {
            _mockRepository.Setup(x=>x.GetItems(It.IsAny<Func<Project,bool>>())).Returns(new List<Project>());
            var sut = new ProjectFormViewModel(
                _mockSource.Object,
                _mockRepository.Object,
                _validator,
                _mockCloseNavigationService.Object);

            _mockRepository.Verify(x => x.GetItems(It.IsAny<Func<Project, bool>>()));
        }

        [Theory, MemberData(nameof(ProjectsTestData))]
        public void HaveEmptyProjectOnProjectsFirst(List<Project> projectsInRepository, List<Project> _)
        {
            _mockRepository.Setup(x => x.GetItems(It.IsAny<Func<Project, bool>>())).Returns(projectsInRepository);
            var sut = new ProjectFormViewModel(
                _mockSource.Object,
                _mockRepository.Object,
                _validator,
                _mockCloseNavigationService.Object);

            Assert.Equal(projectsInRepository.Count() + 1, sut.Projects.Count());
            var emptyProject = sut.Projects.First();
            sut.Projects.Where(x => x.IsEmpty).Single();
            Assert.Null(emptyProject.Item);
            Assert.True(emptyProject.IsEmpty);
        }

        [Theory, MemberData(nameof(ProjectsTestData))]
        public void HaveOrderedProjectsFromRepository_AfterInitialization(List<Project> projectsInRepository, List<Project> expected)
        {
            _mockRepository.Setup(x => x.GetItems(It.IsAny<Func<Project, bool>>())).Returns(projectsInRepository);
            var sut = new ProjectFormViewModel(
                _mockSource.Object,
                _mockRepository.Object,
                _validator,
                _mockCloseNavigationService.Object);

            var result = sut.Projects.Where(x => x.Item != null).Select(x => x.Item!.Project).ToList();
            Assert.Equal(expected, result);

        }

        public static IEnumerable<object[]> ProjectsTestData()
        {
            var project1 = new Project("First", Guid.NewGuid());
            var project2 = new Project("Second", Guid.NewGuid());
            var project3 = new Project("Third", Guid.NewGuid());


            yield return new object[] { new List<Project>(), new List<Project>() };
            yield return new object[] { new List<Project> { project1}, new List<Project> { project1 } };
            yield return new object[] { new List<Project> { project2, project1}, new List<Project> { project1, project2 } };
            yield return new object[] { new List<Project> { project2, project3, project1}, new List<Project> { project1, project2, project3 } };

        }

        [Theory, MemberData(nameof(SelectedProjectTestData))]
        public void SelectedProjectEqualSourceProject(List<Project> projectsInRepository, Project expected)
        {
            _mockSource.SetupGet(x => x.Project).Returns(expected);
            _mockRepository.Setup(x => x.GetItems(It.IsAny<Func<Project, bool>>())).Returns(projectsInRepository);

            var sut = new ProjectFormViewModel(
                _mockSource.Object,
                _mockRepository.Object,
                _validator,
                _mockCloseNavigationService.Object);


            var result = Assert.Single(sut.Projects, x => x.IsSelected);
            if(expected.Equals(Project.Empty))
            {
                Assert.True(result.IsEmpty);
            }
            else
            {
                Assert.Equal(expected, result.Item!.Project);
            }
        }

        public static IEnumerable<object[]> SelectedProjectTestData()
        {
            var project1 = new Project("First", Guid.NewGuid());
            var project2 = new Project("Second", Guid.NewGuid());
            var project3 = new Project("Third", Guid.NewGuid());
            yield return new object[] { new List<Project>(), Project.Empty };
            yield return new object[] { new List<Project> { project1 }, Project.Empty };
            yield return new object[] { new List<Project> { project1 }, project1 };
            yield return new object[] { new List<Project> { project2, project3, project1 }, project2 };
            yield return new object[] { new List<Project> { project2, project3, project1 }, project3 };
            yield return new object[] { new List<Project> { project2, project3, project1 }, project1 };
            yield return new object[] { new List<Project> { project2, project3, project1 }, Project.Empty };
        }

        [Theory, MemberData(nameof(SelectCommandTestData))]
        public void SetProjectToSource_WhenSelectedProjectCommandExecuted(Project expected)
        {
            var container = new ProjectContainerViewModel(
                 expected.Equals(Project.Empty) ?
                    null:
                    new EditableProjectViewModel(
                        expected, _mockRepository.Object, _validator), 
                expected.Equals(Project.Empty), 
                false);


            _sut.SelectProjectCommand.Execute(container);

            _mockSource.VerifySet(x => x.Project = expected, Times.Once);
        }

        [Theory, MemberData(nameof(SelectCommandTestData))]
        public void CloseProjectForm_WhenSelectedProjectCommandExecuted(Project expected)
        {
            var container = new ProjectContainerViewModel(
                 expected.Equals(Project.Empty) ?
                    null :
                    new EditableProjectViewModel(
                        expected, _mockRepository.Object, _validator),
                expected.Equals(Project.Empty),
                false);

            _sut.SelectProjectCommand.Execute(container);

            _mockCloseNavigationService.Verify(x => x.Navigate(It.IsAny<object>()), Times.Once);
        }

        public static IEnumerable<object[]> SelectCommandTestData()
        {
            yield return new object[] { Project.Empty };
            yield return new object[] { _projects[0] };
            yield return new object[] { _projects[1] };
            yield return new object[] { _projects[2] };
        }

        [Fact]
        public void UpdateProjects_WhenRepositoryChanging()
        {
            Assert.PropertyChanged(
                _sut, 
                nameof(ProjectFormViewModel.Projects),
                () => _mockRepository.Raise(
                    x => x.RepositoryChanged += null, 
                    new RepositoryChangedEventArgs<Project>(Project.Empty, It.IsAny<RepositoryChangedOperation>())));
        }



    }
}

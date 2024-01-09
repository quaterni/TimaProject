using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Exceptions;
using TimaProject.Repositories;
using Xunit;

namespace TimaProject.Tests
{
    public class ProjectRepositoryShould : IDisposable
    {
        private bool _isRepositoryChangedRaised;
        private readonly Action<object?, RepositoryChangedEventArgs<Project>> _repositoryChangedAttached;

        private readonly ProjectRepository _sut;

        private static readonly List<Project> _projects = new List<Project>()
        {
            new Project("First", Guid.NewGuid()),
            new Project("Second", Guid.NewGuid()),
            new Project("Third", Guid.NewGuid()),
            new Project("Fourth", Guid.NewGuid())
        };

        public ProjectRepositoryShould()
        {
            _sut = new ProjectRepository();

            _isRepositoryChangedRaised = false;
            _repositoryChangedAttached = (s, e) => _isRepositoryChangedRaised = true;
            _sut.RepositoryChanged += _repositoryChangedAttached.Invoke;


            foreach(var project in _projects)
            {
                _sut.AddItem(project);
            }
        }


        public void Dispose()
        {
            _sut.RepositoryChanged -= _repositoryChangedAttached.Invoke;
        }

        [Fact]
        public void AddItem()
        {
            var sut = new ProjectRepository();
            var project = new Project("MyProject", Guid.NewGuid());

            sut.AddItem(project);

            var projects = sut.GetItems((e) => true);
            Assert.Contains(project, projects);
        }

        [Theory]
        [MemberData(nameof(ContansTestData))]
        public void Contains_ReturnExpectedResult(Project project, bool expected)
        {
            var result = _sut.Contains(project);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ContansTestData()
        {
            yield return new object[] { Project.Empty, true };
            yield return new object[] { _projects[0], true };
            yield return new object[] { _projects[1], true };
            yield return new object[] { _projects[2], true };
            yield return new object[] { _projects[3], true };
            yield return new object[] { new Project(_projects[0].Name, Guid.NewGuid() ), false };
            yield return new object[] { new Project("AnotherProject", Guid.NewGuid() ), false };
        }

        [Fact]
        public void AddItem_ThrowException_WhereAddingProjectContains()
        {
            Assert.Throws<AddingNotUniqueItemException>(() => _sut.AddItem(_projects[0]));
        }

        [Fact]
        public void AddItem_ThrowException_IfAddingProjectHaveNotUniqueName()
        {
            Assert.Throws<AddingNotUniqueItemException>(() => _sut.AddItem(new Project(_projects[0].Name, Guid.NewGuid())));
        }

        [Theory]
        [MemberData(nameof(UpdateItemTestData))]
        public void UpdateItem(Project updatingProject, Project expected)
        {
            _sut.UpdateItem(expected);

            var projects = _sut.GetItems(e => true);
            Assert.DoesNotContain(updatingProject, projects);
            Assert.Contains(expected, projects);
        }

        public static IEnumerable<object[]> UpdateItemTestData()
        {
            yield return new object[] { _projects[0], new Project("new first", _projects[0].Id) };
            yield return new object[] { _projects[1], new Project("new second", _projects[1].Id) };
            yield return new object[] { _projects[2], new Project("new third", _projects[2].Id) };
            yield return new object[] { _projects[3], new Project("new fourth", _projects[3].Id) };
        }

        [Fact]
        public void UpdateItem_ThrowsException_WhenProjectNotContainsInRepsitory()
        {
            var project = new Project("new", Guid.NewGuid());

            var exception = Assert.Throws<NotFoundException<Project>>(()=> _sut.UpdateItem(project));

            Assert.Equal(project, exception.Item);
            Assert.Equal("Project doesn't contain in repository", exception.Message);

        }

        [Theory]
        [MemberData(nameof(RemoveItemTestData))]
        public void RemoveItem_ReturnCorrectValue(Project removingProject, bool expected)
        {
            var result = _sut.RemoveItem(removingProject);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> RemoveItemTestData()
        {
            yield return new object[] { _projects[0], true };
            yield return new object[] { _projects[1], true };
            yield return new object[] { _projects[2], true };
            yield return new object[] { _projects[3], true };
            yield return new object[] { new Project("NotContainingProject", Guid.NewGuid()) , false };
        }

        [Fact]
        public void RemoveItem_ThrowsChangeEmptyProjectExcepiton_WhenRemovingEmptyProject()
        {
            var exception = Assert.Throws<ChangeEmptyProjectException>(() => _sut.RemoveItem(Project.Empty));
        }

        [Fact]
        public void UpdateItem_ThrowsChangeEmptyProjectExcepiton_WhenUpdatingEmptyProject()
        {
            var exception = Assert.Throws<ChangeEmptyProjectException>(() => _sut.UpdateItem(new Project("Empty", Project.Empty.Id)));
        }

        [Theory]
        [MemberData(nameof(ContainsProjectNameTestData))]
        public void Contains_TakeProjectName_ReturnCorrectResult(string projectName, bool expected)
        {
            var result = _sut.Contains(projectName);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ContainsProjectNameTestData()
        {
            yield return new object[] { _projects[0].Name, true };
            yield return new object[] { _projects[1].Name, true };
            yield return new object[] { _projects[2].Name, true };
            yield return new object[] { _projects[3].Name, true };
            yield return new object[] { "Empty", false };
        }

        [Theory]
        [MemberData(nameof(GetItemsTestData))]
        public void GetItems_ReturnMatchedProjects(Func<Project, bool> predicate, List<Project> expectedProjects)
        {
            var result = _sut.GetItems(predicate);
            Assert.Equal(expectedProjects, result);
        }

        public static IEnumerable<object[]> GetItemsTestData()
        {
            yield return new object[] { (Project e) => true, _projects };
            yield return new object[] { (Project e) => e.Equals(_projects[0]), new List<Project> { _projects[0] } };
            yield return new object[] { (Project e) => e.Name.Contains('F'), new List<Project> { _projects[0], _projects[3] } };
            yield return new object[] { (Project e) => false, new List<Project>()};
        }

        [Fact]
        public void GetItems_DoesntReturnEmptyProject()
        {
            var result = _sut.GetItems(e => true);
            Assert.DoesNotContain(Project.Empty, result);
        }

        [Theory, MemberData(nameof(AddItemRaisingEventTestData))]
        public void AddItem_RaisingEvent_OnCorrectInput(Project project, bool isShouldRaising)
        {
            _isRepositoryChangedRaised = false;
            if (isShouldRaising)
            {
                var eventData = Assert.Raises<RepositoryChangedEventArgs<Project>>(
                    s => _sut.RepositoryChanged += s, 
                    s => _sut.RepositoryChanged -= s,
                    () => _sut.AddItem(project));
                Assert.Equal(_sut, eventData.Sender);
                Assert.True(_isRepositoryChangedRaised);
                Assert.Equal(RepositoryChangedOperation.Add, eventData.Arguments.Operation);
                Assert.Equal(project, eventData.Arguments.Item);
            }
            else
            {
                Assert.Throws<AddingNotUniqueItemException>(() => _sut.AddItem(project));
                Assert.False(_isRepositoryChangedRaised);
            }
        }

        public static IEnumerable<object[]> AddItemRaisingEventTestData()
        {
            yield return new object[] { new Project("NewProject", Guid.NewGuid()), true };
            yield return new object[] { _projects[0], false };
        }

        [Theory, MemberData(nameof(UpdateItemRaisingEventTestData))]
        public void UpdateItem_RaisingEvent_OnCorrectInput(Project project, bool isShouldRaising)
        {
            _isRepositoryChangedRaised = false;
            if (isShouldRaising)
            {
                var eventData = Assert.Raises<RepositoryChangedEventArgs<Project>>(
                    s => _sut.RepositoryChanged += s,
                    s => _sut.RepositoryChanged -= s,
                    () => _sut.UpdateItem(project));
                Assert.Equal(_sut, eventData.Sender);
                Assert.True(_isRepositoryChangedRaised);
                Assert.Equal(RepositoryChangedOperation.Update, eventData.Arguments.Operation);
                Assert.Equal(project, eventData.Arguments.Item);
            }
            else
            {
                Assert.Throws<NotFoundException<Project>>(() => _sut.UpdateItem(project));
                Assert.False(_isRepositoryChangedRaised);
            }
        }

        public static IEnumerable<object[]> UpdateItemRaisingEventTestData()
        {
            yield return new object[] { new Project("NewProject", Guid.NewGuid()), false };
            yield return new object[] { _projects[0], true };
        }

        [Theory, MemberData(nameof(RemoveItemRaisingEventTestData))]
        public void RemoveItem_RaisingEvent_OnCorrectInput(Project project, bool isShouldRaising)
        {
            _isRepositoryChangedRaised = false;
            if (isShouldRaising)
            {
                var eventData = Assert.Raises<RepositoryChangedEventArgs<Project>>(
                    s => _sut.RepositoryChanged += s,
                    s => _sut.RepositoryChanged -= s,
                    () => _sut.RemoveItem(project));
                Assert.Equal(_sut, eventData.Sender);
                Assert.True(_isRepositoryChangedRaised);
                Assert.Equal(RepositoryChangedOperation.Remove, eventData.Arguments.Operation);
                Assert.Equal(project, eventData.Arguments.Item);
            }
            else
            {
                Assert.False(_sut.RemoveItem(project));
                Assert.False(_isRepositoryChangedRaised);
            }
        }


        public static IEnumerable<object[]> RemoveItemRaisingEventTestData()
        {
            yield return new object[] { new Project("NewProject", Guid.NewGuid()), false };
            yield return new object[] { _projects[0], true };
        }
    }
}

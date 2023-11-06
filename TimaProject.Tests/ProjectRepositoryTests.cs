using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using Xunit;

namespace TimaProject.Tests
{
    public class ProjectRepositoryTests
    {
        private readonly ProjectRepository _sut;

        public ProjectRepositoryTests()
        {
            _sut = new ProjectRepository();
        }

        [Fact]
        public void GetAllProjects_WhenRepositoryEmpty_ReturnEmptyList()
        {
            var result = _sut.GetAllProjects();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllProject_WhenRepositoryContainsProject_ReturnListWithProjects()
        {
            var project = new Project("MyProject", 1);

            _sut.AddProject(project);

            var result = _sut.GetAllProjects();

            Assert.NotEmpty(result);
            Assert.Contains(project, result);
        }

        [Fact]
        public void Contains_WhenRepositoryContainsProject_ReturnTrue()
        {
            var project = new Project("MyProject", 1);

            _sut.AddProject(project);

            Assert.True(_sut.Contains(project));
        }


        [Fact]
        public void Contains_WhenNameContainsProject_ReturnTrue()
        {
            var project = new Project("MyProject", 1);

            _sut.AddProject(project);

            Assert.True(_sut.Contains(project.Name));
        }

        [Fact]
        public void Contains_WhenProjectNotInRepository_ReturnFalse()
        {
            var project = new Project("MyProject", 1);

            Assert.False(_sut.Contains(project));
        }

        [Fact]
        public void Contains_WhenNameNotInRepository_ReturnFalse()
        {
            Assert.False(_sut.Contains("Test"));
        }

        [Fact]
        public void AddProject_AddNewProject()
        {
            var project = new Project("MyProject", 1);
            _sut.AddProject(project);

            Assert.True(_sut.Contains(project));
        }

        [Fact]
        public void AddProject_AddExistingProject_ThrowException() 
        {
            var project = new Project("MyProject", 1);
            _sut.AddProject(project);

            Assert.ThrowsAny<Exception>(()=> _sut.AddProject(project));
        }

        [Fact]
        public void AddProject_AddProjectWithExistingId_ThrowException()
        {
            var project1 = new Project("MyProject", 1);
            var project2 = new Project("NewProject", 1);
            _sut.AddProject(project1);

            Assert.ThrowsAny<Exception>(() => _sut.AddProject(project2));
        }

        [Fact]
        public void AddProject_AddProjectWithExistingName_ThrowException()
        {
            var project1 = new Project("MyProject", 1);
            var project2 = new Project("MyProject", 2);

            _sut.AddProject(project1);

            Assert.ThrowsAny<Exception>(() => _sut.AddProject(project2));
        }

        [Fact]
        public void UpdateProject_UpdateExistingProject()
        {
            var project = new Project("MyProject", 1);
            _sut.AddProject(project);

            var updatedProject = project with { Name = "NewName" };
            _sut.UpdateProject(updatedProject);

            Assert.True(_sut.Contains(updatedProject.Name));
            Assert.False(_sut.Contains(project.Name));
        }

        [Fact]
        public void UpdateProject_WhenProjectNotInRepository_ThrowException()
        {
            var project = new Project("MyProject", 1);
            _sut.AddProject(project);

            var updatedProject = project with { Name = "NewName", Id = 2 };

            Assert.ThrowsAny<Exception>(() => _sut.UpdateProject(updatedProject));

        }

        [Fact]
        public void RemoveProject_WhenProjectInRepository_ReturnTrue()
        {
            var project = new Project("MyProject", 1);
            _sut.AddProject(project);

            Assert.True(_sut.RemoveProject(project));
            Assert.Empty(_sut.GetAllProjects());
        }

        [Fact]
        public void RemoveProject_WhenProjectNotInRepository_ReturnFalse()
        {
            var project = new Project("MyProject", 1);

            Assert.False(_sut.RemoveProject(project));
        }

        [Fact]
        public void GetId_AfterAddingProject_IncreaseId()
        {
            Assert.Equal(1, _sut.GetId());

            _sut.AddProject(new Project("New", 1));

            Assert.Equal(2, _sut.GetId());
        }
    }
}

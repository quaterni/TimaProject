using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Repositories;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{
    public class ProjectViewModelBaseTests
    {
        private readonly ProjectViewModelBase _sut;
        private readonly ProjectRepository _projectRepository;

        public ProjectViewModelBaseTests()
        {
            _projectRepository = new ProjectRepository();
            _sut = new ProjectViewModelBase(new ProjectValidator(_projectRepository));
        }

        [Fact]
        public void Name_AfterInits_Empty()
        {
            Assert.Empty(_sut.Name);
        }

        [Fact]
        public void HasPropertyErrors_OnName_AfterInits_ReturnFalse()
        {
            Assert.False(_sut.HasPropertyErrors(nameof(ProjectEditViewModel.Name)));
        }


        [Fact]
        public void HasPropertyErrors_OnName_AfterSetCorrectName_ReturnFalse()
        {
            _sut.Name = "Test";
            Assert.False(_sut.HasPropertyErrors(nameof(ProjectEditViewModel.Name)));
        }

        [Fact]
        public void HasPropertyErrors_OnName_AfterSetEmptyStringTwice_ReturnTrue()
        {
            _sut.Name = "Test";
            _sut.Name = string.Empty;
            Assert.True(_sut.HasPropertyErrors(nameof(ProjectEditViewModel.Name)));
        }
    }
}

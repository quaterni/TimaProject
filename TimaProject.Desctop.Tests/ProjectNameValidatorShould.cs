using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Validators;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class ProjectNameValidatorShould
    {
        private readonly ProjectNameValidator _sut;
        private readonly Mock<IProjectRepository> _mockRepository;
        private readonly Mock<IProjectName> _mockProject;

        public ProjectNameValidatorShould()
        {
            _mockRepository= new Mock<IProjectRepository>();
            _mockProject = new Mock<IProjectName>();
            _mockProject.SetupGet(x => x.Name).Returns("Name");

            _sut = new ProjectNameValidator(_mockRepository.Object);
        }

        [Fact]
        public void ValidateCorrectName()
        {
            var validationResult = _sut.Validate(_mockProject.Object);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void NotValidate_IfNameContainsInRepository()
        {
            _mockRepository.Setup(x=> x.Contains("Name")).Returns(true);

            var validationResult = _sut.Validate(_mockProject.Object);
            Assert.False(validationResult.IsValid);

            var validationFailure = validationResult.Errors.Single();
            Assert.Equal(nameof(IProjectName.Name), validationFailure.PropertyName);
            Assert.Equal("Same project name already exist", validationFailure.ErrorMessage);
        }

        [Fact]
        public void NotValidate_IfNameEmpty()
        {
            _mockProject.SetupGet(x => x.Name).Returns(string.Empty);

            var validationResult = _sut.Validate(_mockProject.Object);
            Assert.False(validationResult.IsValid);

            var validationFailure = validationResult.Errors.Single();
            Assert.Equal(nameof(IProjectName.Name), validationFailure.PropertyName);
            Assert.Equal("Name must be not empty", validationFailure.ErrorMessage);
        }

    }
}

using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class RecordViewModelBaseShould
    {
        private readonly RecordViewModelBase _sut;
        private readonly Mock<RecordViewModelBase> _mockSut;

        private readonly Mock<ITimeFormViewModelFactory> _mockTimeFormFactory;
        private readonly Mock<IProjectFormViewModelFactory> _mockProjectFormFactory;
        private readonly Mock<IProjectFormViewModel> _mockProjectForm;
        private readonly Mock<ITimeFormViewModel> _mockTimeForm;

        public RecordViewModelBaseShould()
        {
            _mockTimeFormFactory = new Mock<ITimeFormViewModelFactory>();
            _mockProjectFormFactory = new Mock<IProjectFormViewModelFactory>();
            _mockProjectForm = new Mock<IProjectFormViewModel>();
            _mockTimeForm = new Mock<ITimeFormViewModel>();
            _mockSut = new Mock<RecordViewModelBase>(_mockTimeFormFactory.Object, _mockProjectFormFactory.Object);
            _sut = _mockSut.Object;

            _mockProjectFormFactory
                .Setup(s => s.Create(_sut.ProjectId))
                .Returns(_mockProjectForm.Object);
            _mockTimeFormFactory
                .Setup(s => s.Create(It.IsAny<TimeDTO>()))
                .Returns(_mockTimeForm.Object);
        }

        [Fact]
        public void Init()
        {
            _sut.Title.Should().BeEmpty();
            _sut.StartTime.Should().BeEmpty();
            _sut.EndTime.Should().BeEmpty();
            _sut.Time.Should().BeEmpty();
            _sut.Date.Should().BeEmpty();
            _sut.ProjectName.Should().BeEmpty();
            _sut.ProjectId.Should().BeEmpty();
            _sut.OpenProjectFormCommand.Should().NotBeNull();
            _sut.OpenTimeFormCommand.Should().NotBeNull();
            _sut.TimeFormViewModel.Should().BeNull();
            _sut.ProjectFormViewModel.Should().BeNull();
            _sut.IsProjectFormOpened.Should().BeFalse();
            _sut.IsTimeFormOpened.Should().BeFalse();
        }

        [Fact]
        public void OpenProjectFormCommandExecuted_CallsProjectFromFactoryl()
        {

            _sut.OpenProjectFormCommand.Execute(null);

            _mockProjectFormFactory.Verify(s=> s.Create(_sut.ProjectId), Times.Once());
        }

        [Fact]
        public void ProjectFormViewModel_WhenOpenProjectFormCommandExecuted_NotNull()
        {
            _sut.OpenProjectFormCommand.Execute(null);

            _sut.ProjectFormViewModel.Should().NotBeNull();
        }

        [Fact]
        public void IsProjectFormOpened_WhenOpenProjectFormCommandExecuted_BeTrue()
        {
            _sut.OpenProjectFormCommand.Execute(null);

            _sut.IsProjectFormOpened.Should().BeTrue();
        }

        [Fact]
        public void WhenOpenProjectFormCommandExecuted_SubscribesOnSelectedProjectEvent()
        {
            _sut.OpenProjectFormCommand.Execute(null);

            _mockProjectForm.VerifyAdd(s => s.ProjectSelected += It.IsAny<EventHandler<ProjectDTO>>(), Times.Once);
        }

        [Fact]
        public void WhenOpenProjectFormCommandExecuted_SubscribesOnClosedEvent()
        {
            _sut.OpenProjectFormCommand.Execute(null);

            _mockProjectForm.VerifyAdd(s => s.Closed += It.IsAny<EventHandler>(), Times.Once);
        }

        [Fact]
        public void OpenTimeFormCommandExecuted_CallTimeFormFactory()
        {
            _sut.OpenTimeFormCommand.Execute(null);

            _mockTimeFormFactory.Verify(s => s.Create(It.IsAny<TimeDTO>()), Times.Once);
        }

        [Fact]
        public void TimeFormViewModel_WhenOpenTimeFormExecuted_NotNull()
        {
            _sut.OpenTimeFormCommand.Execute(null);

            _sut.TimeFormViewModel.Should().NotBeNull();
        }

        [Fact]
        public void IsTimeFormOpened_WhenOpenTimeFormExecuted_BeTrue()
        {
            _sut.OpenTimeFormCommand.Execute(null);

            _sut.IsTimeFormOpened.Should().BeTrue();
        }

        [Fact]
        public void WhenTimeFormExecuted_SubscribesOnTimeSelectedEvent()
        {
            _sut.OpenTimeFormCommand.Execute(null);

            _mockTimeForm.VerifyAdd(s => s.TimeSelected += It.IsAny<EventHandler<TimeDTO>>(), Times.Once);
        }

        [Fact]
        public void WhenTimeFormExecuted_SubscribesOnClosedEvent()
        {
            _sut.OpenTimeFormCommand.Execute(null);

            _mockTimeForm.VerifyAdd(s => s.Closed += It.IsAny<EventHandler>(), Times.Once);
        }

    }
}

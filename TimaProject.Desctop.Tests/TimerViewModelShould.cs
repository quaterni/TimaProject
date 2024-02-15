using Moq;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.Stores;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Validators;
using Xunit;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using FluentAssertions;


namespace TimaProject.Desctop.Tests
{

    public class TimerViewModelShould
    {
        private readonly TimerViewModel _sut;
        private readonly Mock<ITimeFormViewModelFactory> _mockTimeFormViewModelFactory;
        private readonly Mock<IProjectFormViewModelFactory> _mockProjectFormViewModelFactory;
        private readonly Mock<IRecordService> _mockRecordService;
        private readonly Mock<ITimerExecutor> _mockTimerExecutor;

        // Create const and static values to testing properties, set that values to mock objects, refactor tests
        public TimerViewModelShould()
        {
            var mockNavigationService = new Mock<INavigationService>();
            _mockTimeFormViewModelFactory = new Mock<ITimeFormViewModelFactory>();
            _mockProjectFormViewModelFactory = new Mock<IProjectFormViewModelFactory>();
            _mockRecordService = new Mock<IRecordService>();
            _mockTimerExecutor = new Mock<ITimerExecutor>();
            _sut = new TimerViewModel(
                mockNavigationService.Object,
                mockNavigationService.Object,
                _mockTimeFormViewModelFactory.Object,
                _mockProjectFormViewModelFactory.Object,
                _mockRecordService.Object,
                _mockTimerExecutor.Object
                );
        }

        [Fact]
        public void Init()
        {
            _sut.State.Should().Be(TimerState.NotRunning);
        }

        //[Fact]
        //public void AddRecordToRepository_WhenStarts()
        //{
        //    var expectedTitle = "Test";
        //    var expectedStartTime = DateTime.Now.AddHours(-2);
        //    var expectedDate = DateOnly.FromDateTime(DateTime.Now);
        //    var expectedProject = new Project("MyProject", Guid.NewGuid());

        //    _sut.Title = expectedTitle;
        //    _sut.StartTime = expectedStartTime.ToString();
        //    _sut.Date = expectedDate.ToString();
        //    _sut.Project = expectedProject;


        //    _mockRepository
        //        .Setup(x => x.AddItem(It.IsAny<Domain.Models.Record>()))
        //        .Callback<Domain.Models.Record>(
        //            x => {
        //                Assert.Equal(expectedTitle, x.Title);
        //                Assert.Equal(expectedStartTime, x.StartTime, TimeSpan.FromSeconds(1));
        //                Assert.Equal(expectedProject, x.Project);
        //                Assert.Equal(expectedDate, x.Date);
        //                Assert.True(x.IsActive);
        //            });

        //    _sut.OnStartingTime();

        //    _mockRepository.Verify(x=> x.AddItem(It.IsAny<Domain.Models.Record>()), Times.Once);
        //}


        //[Fact]
        //public void SetCurrentTime_WhenStartsWithNoValidStartTime()
        //{
        //    _mockRepository
        //        .Setup(x => x.AddItem(It.IsAny<Domain.Models.Record>()))
        //        .Callback<Domain.Models.Record>(
        //            x => {
        //                Assert.Equal(DateTime.Now, x.StartTime, TimeSpan.FromSeconds(1));
        //            });

        //    _sut.StartTime = "";

        //    _sut.OnStartingTime();
        //}

        //[Fact]
        //public void SetCurrentDate_WhenStartsWithNoValidDate()
        //{
        //    _mockRepository
        //        .Setup(x => x.AddItem(It.IsAny<Domain.Models.Record>()))
        //        .Callback<Domain.Models.Record>(
        //            x => {
        //                Assert.Equal(DateOnly.FromDateTime(DateTime.Now), x.Date);
        //            });

        //    _sut.Date = "";

        //    _sut.OnStartingTime();
        //}

        //[Fact]
        //public void UpdateRecord_WhenTimerIsRunning()
        //{
        //    _sut.OnStartingTime();
        //    _mockRepository.Verify(x => x.AddItem(It.IsAny<Domain.Models.Record>()), Times.Once);

        //    _sut.Title = "New Title";
        //    _sut.StartTime = DateTime.Now.ToString();
        //    _sut.Date = "27.11.2023";
        //    _sut.Project = new Project("NewProject", Guid.NewGuid());

        //    _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Domain.Models.Record>()), Times.Exactly(4));

        //}

        //[Fact]
        //public void IgnoreUpdate_IfValuesNotValid_WhenTimerIsRunning()
        //{
        //    _sut.StartTime = DateTime.Now.ToString();
        //    _sut.Date = "27.11.2023";
        //    _sut.OnStartingTime();

        //    _sut.StartTime = "wrong input";
        //    _sut.Date = "wrong input";
        //    _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Domain.Models.Record>()), Times.Never);
        //}


        //[Fact]
        //public void SetCurrentTimeToEndTime_WhenTimerEndsRunning()
        //{
        //    _mockRepository
        //        .Setup(x => x.UpdateItem(It.IsAny<Domain.Models.Record>()))
        //        .Callback<Domain.Models.Record>(x => Assert.Equal(DateTime.Now, (DateTime)x.EndTime, TimeSpan.FromSeconds(1)));

        //    _sut.OnStartingTime();
        //    _sut.OnEndingTime();
        //    Thread.Sleep(TimerViewModel.TIMER_INTERVAL_MILLISECONDS);

        //    _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Domain.Models.Record>()), Times.Once);
        //}

        //[Fact]
        //public void DeactivateTimer()
        //{
        //    _sut.OnStartingTime();
        //    _sut.OnEndingTime();
        //    Assert.Equal(TimerState.NotRunning, _sut.State);
        //}

        //[Fact]
        //public void SetDefaultValues_WhenTimerEndsUp()
        //{
        //    _sut.OnStartingTime();

        //    _sut.OnEndingTime();
        //    Thread.Sleep(TimerViewModel.TIMER_INTERVAL_MILLISECONDS);

        //    Assert.Equal(string.Empty, _sut.Title);
        //    Assert.Equal(string.Empty, _sut.StartTime);
        //    Assert.Empty(_sut.EndTime);
        //    Assert.Equal(Project.Empty, _sut.Project);
        //}

        //[Fact]
        //public void Validates_AfterSettindDefaultValues()
        //{
        //    _sut.OnStartingTime();

        //    _sut.OnEndingTime();

        //    Thread.Sleep(TimerViewModel.TIMER_INTERVAL_MILLISECONDS);

        //    Assert.False(_sut.HasErrors);
        //}

    }
}

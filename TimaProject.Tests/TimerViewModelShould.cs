using Moq;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Stores;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{

    public class TimerViewModelShould
    {
        private readonly TimerViewModel _sut;
        private readonly Mock<IRecordRepository> _mockRepository;
        private readonly Mock<INavigationService> _mockNavigationService;

        public TimerViewModelShould()
        {
            _mockRepository = new Mock<IRecordRepository>();
            _mockNavigationService = new Mock<INavigationService>();
            _sut = new TimerViewModel(
                _mockRepository.Object,
                _mockNavigationService.Object,
                _mockNavigationService.Object,
                null,
                null,
                new TimeValidator());

            _sut.StartTime = DateTimeOffset.Now.ToString();
            _sut.Date = DateOnly.MinValue.ToString();
            _sut.Title = "Test";
        }

        [Fact]
        public void AddRecordToRepository_WhenStarts()
        {
            var expectedTitle = "Test";
            var expectedStartTime = DateTime.Now.AddHours(-2);
            var expectedDate = DateOnly.FromDateTime(DateTime.Now);
            var expectedProject = new Project("MyProject", Guid.NewGuid());

            _sut.Title = expectedTitle;
            _sut.StartTime = expectedStartTime.ToString();
            _sut.Date = expectedDate.ToString();
            _sut.Project = expectedProject;


            _mockRepository
                .Setup(x => x.AddItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(
                    x => {
                        Assert.Equal(expectedTitle, x.Title);
                        Assert.Equal(expectedStartTime, x.StartTime, TimeSpan.FromSeconds(1));
                        Assert.Equal(expectedProject, x.Project);
                        Assert.Equal(expectedDate, x.Date);
                        Assert.True(x.IsActive);
                    });

            _sut.OnStartingTime();

            _mockRepository.Verify(x=> x.AddItem(It.IsAny<Models.Record>()), Times.Once);
        }


        [Fact]
        public void SetCurrentTime_WhenStartsWithNoValidStartTime()
        {
            _mockRepository
                .Setup(x => x.AddItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(
                    x => {
                        Assert.Equal(DateTime.Now, x.StartTime, TimeSpan.FromSeconds(1));
                    });

            _sut.StartTime = "";

            _sut.OnStartingTime();
        }

        [Fact]
        public void SetCurrentDate_WhenStartsWithNoValidDate()
        {
            _mockRepository
                .Setup(x => x.AddItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(
                    x => {
                        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), x.Date);
                    });

            _sut.Date = "";

            _sut.OnStartingTime();
        }

        [Fact]
        public void UpdateRecord_WhenTimerIsRunning()
        {
            _sut.OnStartingTime();
            _mockRepository.Verify(x => x.AddItem(It.IsAny<Models.Record>()), Times.Once);

            _sut.Title = "New Title";
            _sut.StartTime = DateTime.Now.ToString();
            _sut.Date = "27.11.2023";
            _sut.Project = new Project("NewProject", Guid.NewGuid());

            _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Models.Record>()), Times.Exactly(4));

        }

        [Fact]
        public void IgnoreUpdate_IfValuesNotValid_WhenTimerIsRunning()
        {
            _sut.StartTime = DateTime.Now.ToString();
            _sut.Date = "27.11.2023";
            _sut.OnStartingTime();

            _sut.StartTime = "wrong input";
            _sut.Date = "wrong input";
            _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Models.Record>()), Times.Never);
        }


        [Fact]
        public void SetCurrentTimeToEndTime_WhenTimerEndsRunning()
        {
            _mockRepository
                .Setup(x => x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(DateTime.Now, (DateTime)x.EndTime, TimeSpan.FromSeconds(1)));

            _sut.OnStartingTime();
            _sut.OnEndingTime();

            _mockRepository.Verify(x => x.UpdateItem(It.IsAny<Models.Record>()), Times.Once);
        }

        [Fact]
        public void DeactivateTimer()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.Equal(TimerState.NotRunning, _sut.State);
        }

        [Fact]
        public void SetDefaultValues_WhenTimerEndsUp()
        {
            _sut.OnStartingTime();

            _sut.OnEndingTime();
            Assert.Equal(string.Empty, _sut.Title);
            Assert.Equal(string.Empty, _sut.StartTime);
            Assert.Empty(_sut.EndTime);
            Assert.Equal(Project.Empty, _sut.Project);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Services.Factories;
using TimaProject.Stores;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{

    public class TimerTests
    {
        private readonly TimerViewModel _sut;
        private readonly RecordRepository _repository;

        public TimerTests()
        {
            _repository = new RecordRepository();

            _sut = new TimerViewModel(
                _repository,
                new RecordFactory(_repository, new TodayDateStore()),
                new MockNavigationService(),
                null,
                new RecordValidator());

            _sut.StartTime = DateTimeOffset.Now.ToString();
            _sut.Date = DateOnly.MinValue.ToString();
            _sut.Title = "Test";
        }

        private bool IsCurrentTime(DateTimeOffset date)
        {
            return (date - DateTimeOffset.Now) < TimeSpan.FromMilliseconds(500);
        }

        private Models.Record GetFirstRecordFromRepository()
        {
            return _repository.GetRecords(new FilterListingArgs() { IsActive = null }).First();
        }

        [Fact]
        public void OnStartingTime_AddRecordToRepository()
        {
            _sut.OnStartingTime();

            var result = _repository.GetRecords(new FilterListingArgs() { IsActive = null });

            Assert.Single(result);
        }

        [Fact]
        public void OnStartingTime_AddActiveRecord()
        {
            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();

            Assert.True(record.IsActive);
        }

        [Fact]
        public void OnStartingTime_WhenStartTimeIsCorrect_AddRecordWithStartTime()
        {
            _sut.OnStartingTime();
            var record = GetFirstRecordFromRepository();
            Assert.Equal(record.StartTime, DateTimeOffset.Parse(_sut.StartTime));
        }


        [Fact]
        public void OnStartingTime_AddRecordWithTitle()
        {
            var expectedTitle = "MyTitle";
            _sut.Title = expectedTitle;
            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();

            Assert.Equal(expectedTitle, record.Title);
        }

        [Fact]
        public void OnStartingTime_WhenDateIsCorrect_AddRecordWithDate()
        {
            var expectedDate = DateOnly.Parse("27.10.2023");
            _sut.Date = expectedDate.ToString();


            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();

            Assert.Equal(expectedDate, record.Date);
        }

        [Fact]
        public void OnStartingTime_AddRecordWithProject()
        {
            var expectedProject = new Project("MyProject", 1);
            _sut.Project = expectedProject;
            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();

            Assert.Equal(expectedProject, record.Project);
        }

        [Fact]
        public void OnStartingTime_AddNoteWithNullEndTime()
        {
            _sut.OnStartingTime();
            var record = GetFirstRecordFromRepository();

            Assert.Null(record.EndTime);
        }

        [Fact]
        public void OnStartingTime_WhenValidationFailed_StartTimeSetCurrentTime()
        {
            _sut.StartTime = "wrong input";

            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();
            Assert.Equal(DateTime.Now, record.StartTime, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void OnStartingTime_WhenValidationFailed_DateSetToday()
        {
            _sut.Date = "wrong input";

            _sut.OnStartingTime();

            var record = GetFirstRecordFromRepository();
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today), record.Date);
        }

        [Fact]
        public void OnRecordUpdated_WhenTitleChanged_SetTilteInRepository()
        {
            _sut.OnStartingTime();

            _sut.Title = "New Title";

            var record = GetFirstRecordFromRepository();
            Assert.Equal(_sut.Title, record.Title);
        }

        [Fact]
        public void OnRecordUpdated_WhenStartTimeIsIncorrect_IgonoreUpdate()
        {
            _sut.OnStartingTime();
            var expected =  DateTimeOffset.Now;
            _sut.StartTime = expected.ToString();
            _sut.StartTime = "New Title";

            var record = GetFirstRecordFromRepository();
            Assert.Equal(expected, record.StartTime, TimeSpan.FromSeconds(1));
        }


        [Fact]
        public void OnEndingTime_SetCurrentTimeToEndTime()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();

            var record = GetFirstRecordFromRepository();
            Assert.Equal(DateTimeOffset.Now, (DateTime)record.EndTime!, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void OnEndingTimeShould_DeactivateTimer()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.False(_sut.IsActive);
        }

        [Fact]
        public void OnEndingTime_SetDefaultValues()
        {
            _sut.OnStartingTime();

            _sut.OnEndingTime();
            Assert.Equal(string.Empty, _sut.Title);
            Assert.Equal(string.Empty, _sut.StartTime);
            Assert.Empty(_sut.EndTime);
            Assert.Equal(Project.Empty, _sut.Project);
        }

        [Fact]
        public void Timer_AddTwoRecord_WhenItHasWorkedTwoTimes()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.Equal(2, _repository.GetRecords(new FilterListingArgs()).Count());
        }

        [Fact]
        public void OnRecordUpdated_WhenSetCorrectDate_UpdateRecord()
        {
            var expectedDate = DateOnly.Parse("1.11.2023");

            _sut.OnStartingTime();
            _sut.Date = expectedDate.ToString();

            var record = GetFirstRecordFromRepository();
            Assert.Equal(expectedDate, record.Date);
        }

        [Fact]
        public void OnEndingTime_SetTodayDateByDefault()
        {
            var expectedDate = DateOnly.FromDateTime(DateTime.Today);

            _sut.OnStartingTime();
            _sut.OnEndingTime();

            Assert.Equal(expectedDate.ToString(), _sut.Date);
        }

    }
}

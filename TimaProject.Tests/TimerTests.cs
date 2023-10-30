using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Stores;
using TimaProject.ViewModels;
using Xunit;

namespace TimaProject.Tests
{

    public class TimerTests
    {
        private readonly TimerViewModel _sut;
        private readonly MockRecordRepository _testRepository;

        public TimerTests()
        {
            _testRepository = new MockRecordRepository();

            _sut = new TimerViewModel(
                _testRepository,
                new MockRecordFactory(),
                new MockNavigationService(),
                null,
                new MockRecordValidator());

            _sut.StartTime = DateTimeOffset.Now.ToString();
            _sut.Date = DateOnly.MinValue.ToString();
            _sut.Title = "Test";
        }

        private bool IsCurrentTime(DateTimeOffset date)
        {
            return (date - DateTimeOffset.Now) < TimeSpan.FromMilliseconds(500);
        }

        [Fact]
        public void OnStartingTimeShould_AddNoteToRepository()
        {
            _sut.OnStartingTime();
            Assert.True(_testRepository.Notes.Count() > 0);
        }

        [Fact]
        public void OnStartingTimeShould_AddActiveNote()
        {
            _sut.OnStartingTime();
            Assert.True(_testRepository.Notes[0].IsActive);
        }

        [Fact]
        public void OnStartingTime_ShouldAddNoteWithCorrectStartTime()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.StartTime == DateTimeOffset.Parse(_sut.StartTime));
        }


        [Fact]
        public void OnStartingTimeShould_AddNoteWithCorrectTitle()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Title.Equals(_sut.Title));
        }

        [Fact]
        public void OnStartingTimeShould_AddNoteWithCorrectDate()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Date == DateOnly.Parse(_sut.Date));
        }

        [Fact]
        public void OnStartingTimeShould_AddNoteWithCorrectProject()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Project.Equals(_sut.Project));
        }

        [Fact]
        public void OnStartingTimeShould_AddNoteWithNullEndTime()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.Null(note.EndTime);
        }

        [Fact]
        public void OnStartingTimeShould_SetDefaultTitle_WhenValidationFailed()
        {
            _sut.Title = "NotValidate";
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.Equal(string.Empty, note.Title);
        }

        [Fact]
        public void OnStartingTimeShould_SetDefaultStartTime_WhenValidationFailed()
        {
            _sut.StartTime = "df";
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(IsCurrentTime(note.StartTime));
        }

        [Fact]
        public void OnStartingTimeShould_SetDefaultDate_WhenValidationFailed()
        {
            _sut.Date = DateOnly.MaxValue.ToString();
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), note.Date);
        }

        [Fact]
        public void OnNoteUpdatedShould_SetTilteInRepository_WhenTitleChanged()
        {
            _sut.OnStartingTime();
            _sut.Title = "New Title";
            Assert.Equal(_sut.Title, _testRepository.Notes[0].Title);
        }


        [Fact]
        public void OnRecordUpdatedShould_IgonoreUpdate_WhenTitleNotCorrect()
        {
            _sut.OnStartingTime();
            var expected = "New Title";
            _sut.Title = expected;
            _sut.Title = "NotValidate";
            Assert.Equal(expected, _testRepository.Notes[0].Title);
        }


        [Fact]
        public void OnRecordUpdatedShould_IgonoreUpdate_WhenStartTimeNotCorrect()
        {
            _sut.OnStartingTime();
            var expected =  DateTimeOffset.Now;
            _sut.StartTime = expected.ToString();
            _sut.StartTime = "New Title";
            Assert.Equal(expected, _testRepository.Notes[0].StartTime, TimeSpan.FromSeconds(1));
        }


        [Fact]
        public void OnEndingTimeShould_SetCurrentTimeToEndTime()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.Equal(DateTimeOffset.Now, (DateTimeOffset)_testRepository.Notes[0].EndTime!, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void OnEndingTimeShould_DeactivateTimer()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.False(_sut.IsActive);
        }

        [Fact]
        public void OnEndingTimeShould_SetDefaultValues()
        {
            _sut.OnStartingTime();

            _sut.OnEndingTime();
            Assert.Equal(string.Empty, _sut.Title);
            Assert.Equal(DateTimeOffset.MinValue.ToString(), _sut.StartTime);
            Assert.Null(_sut.EndTime);
            Assert.Equal(Project.Empty, _sut.Project);
        }

        [Fact]
        public void TimerShould_AddTwoRecord_WhenItHasWorkedTwoTimes()
        {
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            _sut.OnStartingTime();
            _sut.OnEndingTime();
            Assert.True(_testRepository.Notes.Count == 2);
        }
    }
}

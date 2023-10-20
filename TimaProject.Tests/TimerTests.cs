using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Stores;
using TimaProject.ViewModels;
using Xunit;

namespace TimaProject.Tests
{

    public class TimerTests
    {
        private readonly TimerViewModel _sut;
        private readonly MockNoteRepository _testRepository;

        public TimerTests()
        {
            _testRepository = new MockNoteRepository();

            _sut = new TimerViewModel(
                _testRepository,
                new MockNoteFactory(),
                new MockNavigationService(),
                null,
                new MockNoteValidator());

            _sut.StartTime = DateTimeOffset.Now.ToString();
            _sut.Date = DateOnly.MinValue.ToString();
            _sut.Title = "Test";
        }

        private bool IsCurrentTime(DateTimeOffset date)
        {
            return (date - DateTimeOffset.Now) < TimeSpan.FromMilliseconds(500);
        }

        [Fact]
        public void OnStartingTimeShouldAddNoteToRepository()
        {
            _sut.OnStartingTime();
            Assert.True(_testRepository.Notes.Count() > 0);
        }

        [Fact]
        public void OnStartingTimeShouldAddingActiveNote()
        {
            _sut.OnStartingTime();
            Assert.True(_testRepository.Notes[0].IsActive);
        }

        [Fact]
        public void OnStartingTimeAddedNoteShouldHaveCorrectStartTime()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.StartTime == DateTimeOffset.Parse(_sut.StartTime));
        }


        [Fact]
        public void OnStartingTimeAddedNoteShouldHaveCorrectTitle()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Title.Equals(_sut.Title));
        }

        [Fact]
        public void OnStartingTimeAddedNoteShouldHaveCorrectDate()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Date == DateOnly.Parse(_sut.Date));
        }

        [Fact]
        public void OnStartingTimeAddedNoteShouldHaveCorrectProject()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.True(note.Project.Equals(_sut.Project));
        }

        [Fact]
        public void OnStartingTimeAddedNoteShouldHaveNullEndTime()
        {
            _sut.OnStartingTime();
            var note = _testRepository.Notes[0];
            Assert.Null(note.EndTime);
        }

        [Fact]
        public void OnStartingTimeValidationFailedShouldSetDefaultTitle()
        {
            _sut.OnStartingTime();
            _sut.Title = "NotValidate";
            var note = _testRepository.Notes[0];
            Assert.Equal(note.Title, string.Empty);
        }

        [Fact]
        public void OnStartingTimeValidationFailedShouldSetDefaultStartTime()
        {
            _sut.OnStartingTime();
            _sut.StartTime = DateTimeOffset.MaxValue.ToString();
            var note = _testRepository.Notes[0];
            Assert.True(IsCurrentTime(note.StartTime));
        }

        [Fact]
        public void OnStartingTimeValidationFailedShouldSetDefaultDate()
        {
            _sut.OnStartingTime();
            _sut.Date = DateOnly.MaxValue.ToString();
            var note = _testRepository.Notes[0];
            Assert.Equal(note.Date, DateOnly.FromDateTime(DateTime.Now));
        }

        [Fact]
        public void OnStartingTimeValidationFailedShouldSetDefaultProject()
        {
            Assert.Fail();
        }
    }
}

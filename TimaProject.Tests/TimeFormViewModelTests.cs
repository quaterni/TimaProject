using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;
using Xunit;

namespace TimaProject.Tests
{
    public class TimeFormViewModelTests
    {
        private readonly TimeFormViewModel _sut;

        public TimeFormViewModelTests()
        {
            _sut = new TimeFormViewModel(new RecordValidator(), new MockNavigationService());
        }

        [Fact]
        public void ValidateShould_AddErrorStartTime_WhenStartTimeIncorrect()
        {
            _sut.StartTime = "2355634";
            Assert.True(_sut.PropertyHasErrors(nameof(TimeFormViewModel.StartTime)));
        }


        [Fact]
        public void ValidateShould_AddErrorEndTime_WhenEndTimeIncorrect()
        {
            _sut.EndTime = "2355634";
            Assert.True(_sut.PropertyHasErrors(nameof(TimeFormViewModel.EndTime)));
        }


        [Fact]
        public void ValidateShould_AddErrorDate_WhenDateIncorrect()
        {
            _sut.Date = "2355634";
            Assert.True(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Date)));
        }

        [Fact]
        public void ValidateShould_AddErrorTime_WhenTimeIncorrect()
        {
            _sut.Time = "2355634";
            Assert.True(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void ValidateShould_AddErrorTime_WhenTimeCorrectButEndTimeIncorrect()
        {
            _sut.EndTime = "2355634";
            _sut.Time = "1:00:00";
            Assert.True(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void ValidateShould_ValidTimeAndSetStartTime_WhenTimeAndEndTimeCorrectButStartTimeIncorrect()
        {
            _sut.EndTime = "28.10.2023 14:00";
            _sut.StartTime = "bla bla";
            _sut.Time = "1:00:00";
            Assert.False(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
            DateTimeOffset expected = new DateTimeOffset(2023, 10, 28, 13, 00, 00, TimeSpan.FromHours(5));
            Assert.True(DateTimeOffset.TryParse(_sut.StartTime, out var result));
            Assert.Equal(expected, result);
        }


        [Fact]
        public void ValidateShould_ValidTimeAndSetStartTime_WhenTimeEndTimeAndStartTimeIsCorrect()
        {
            _sut.EndTime = "28.10.2023 14:00";
            _sut.StartTime = "28.10.2023 12:00";
            _sut.Time = "1:00:00";
            Assert.False(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
            DateTimeOffset expected = new DateTimeOffset(2023, 10, 28, 13, 00, 00, TimeSpan.FromHours(5));
            Assert.True(DateTimeOffset.TryParse(_sut.StartTime, out var result));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EndTimeShould_SetCurrentTime_WhenIsEndTimeEnabledFalse()
        {
            _sut.IsEndTimeEnabled = false;
            Assert.True(DateTimeOffset.TryParse(_sut.EndTime, out var result));
            Assert.True(IsCurrentTime(result));

        }

        [Fact]
        public void EndTimeShouldNot_SetNewTime_WhenIsEndTimeEnabledFalse()
        {
            _sut.IsEndTimeEnabled = false;
            var expected = _sut.EndTime;
            _sut.EndTime = "dsfsdf";
            Assert.Equal(expected, _sut.EndTime);

        }


        private bool IsCurrentTime(DateTimeOffset date)
        {
            return (date - DateTimeOffset.Now) < TimeSpan.FromMilliseconds(500);
        }

        [Fact]
        public void TimeShould_BeSet_WhenCerrectStartTimeChangedAndEndTimeIsCorrect()
        {
            _sut.EndTime = "28.10.2023 14:00";
            _sut.Time = "1:00:00";
            _sut.StartTime = "28.10.2023 00:00";
            var expected = "14:00:00";
            Assert.False(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
            Assert.Equal(expected, _sut.Time);
        }


        [Fact]
        public void TimeShould_BeSet_WhenCerrectEndTimeChangedAndStartTimeIsCorrect()
        {
            _sut.Time = "1:00:00";
            _sut.StartTime = "28.10.2023 00:00";
            _sut.EndTime = "28.10.2023 14:00";
            var expected = "14:00:00";
            Assert.False(_sut.PropertyHasErrors(nameof(TimeFormViewModel.Time)));
            Assert.Equal(expected, _sut.Time);
        }
    }
}

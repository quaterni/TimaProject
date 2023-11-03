using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
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
        public void Time_WhenTimeInits_ReturnEmptyString()
        {
            Assert.Equal(string.Empty, _sut.Time);
        }


        [Fact]
        public void HasPropertyErrors_OnTime_AfterTimeInits_ReturnFalse()
        {
            var result = _sut.HasPropertyErrors(nameof(TimeFormViewModel.Time));

            Assert.False(result);
        }

        [Fact]
        public void HasPropertyErrors_OnTime_WhenTimeHasSetEmptyStringTwice_ReturnTrue()
        {
            _sut.Time = "24.05.2022 7:45";
            _sut.Time = "";

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }


        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetIncorrect_ReturnTrue()
        {
            _sut.Time = "2355634";
            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void EndTime_SetCurrentTime_WhenEndTimeDisabled()
        {
            _sut.IsEndTimeEnabled = false;

            Assert.True(DateTimeOffset.TryParse(_sut.EndTime, out var result));
            Assert.Equal(result, DateTimeOffset.Now, TimeSpan.FromSeconds(1));

        }

        [Fact]
        public void EndTime_DoesntChange_WhenEndTimeDisabled()
        {
            _sut.IsEndTimeEnabled = false;
            var expected = _sut.EndTime;
            _sut.EndTime = "dsfsdf";
            Assert.Equal(expected, _sut.EndTime);

        }

        [Fact]
        public void Time_DoesntChange_WhenHasSetCorrectStartTimeButEndTimeEmpty()
        {
            var time = "1:00:00";

            _sut.Time = time;
            _sut.StartTime = "13/04/2024 6:45";
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));

            Assert.Equal(time, _sut.Time);
        }


        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetCorrectStartTime_ButTimeAndEndTimeEmpty_ReturnFalse()
        {
            _sut.StartTime = "13/04/2024 6:45";
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));

            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void Time_DoesntChange_WhenHasSetCorrectEndTime_ButStartTimeEmpty()
        {
            var time = "1:00:00";

            _sut.Time = time;
            _sut.EndTime = "13/04/2024 6:45";
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));

            Assert.Equal(time, _sut.Time);
        }


        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetCorrectEndTime_ButTimeAndStartTimeEmpty_ReturnFalse()
        {
            _sut.EndTime = "13/04/2024 6:45";
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));

            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }

        [Theory]
        [InlineData("13/04/2024 6:45", "bla bla")]
        [InlineData("bla bla", "13/04/2024 6:45")]
        [InlineData("bla bla", "bla bla")]
        [InlineData("13/04/2024 6:45", "12/04/2024 6:45")]
        public void HasPropertyErrors_OnTime_WhenAnyHasErrorsOnStartTimeOrEndTime_ReturnTrue(string startTime, string endTime)
        {
            _sut.StartTime = startTime;
            _sut.EndTime = endTime;
            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetCorrectEndTime_StartTimeEmptyTwice_ReturnTrue()
        {
            _sut.EndTime = "13/04/2024 6:45";
            _sut.StartTime = "13/04/2024 6:35";
            _sut.StartTime = string.Empty;
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }

        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetCorrectStartTime_EndTimeEmptyTwice_ReturnTrue()
        {
            _sut.StartTime = "13/04/2024 6:45";
            _sut.EndTime = "13/04/2024 6:45";
            _sut.EndTime = string.Empty;
            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }



        [Fact]
        public void HasPropertyErrors_OnTime_WhenHasSetCorrectTimeButEndTimeIncorrect_ReturnTrue()
        {
            _sut.EndTime = "2355634";
            _sut.Time = "1:00:00";
            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        }


        [Theory]
        [InlineData("")]
        [InlineData("wrong data")]
        [InlineData("13/04/2024 6:45")]
        public void StartTime_ChangeValue_WhenTimeAndEndTimeCorrect(string startTime)
        {
            _sut.StartTime = startTime;
            _sut.EndTime = "13/04/2024 6:45";
            _sut.Time = "1:00:00";
            var expected = "13/04/2024 5:45";

            Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));
            Assert.Equal(DateTimeOffset.Parse(expected), DateTimeOffset.Parse(_sut.StartTime));
        }


        [Theory]
        [InlineData("")]
        [InlineData("wrong data")]
        [InlineData("13.04.2024 6:45:00")]
        public void StartTime_DoesntChangeValue_WhenTimeIncorrect_ButEndTimeCorrect(string startTime)
        {
            _sut.StartTime = startTime;
            _sut.EndTime = "13/04/2024 6:45";
            _sut.Time = "bla bla";
            var expected = startTime;

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
            Assert.Equal(expected, _sut.StartTime);
        }

        [Theory]
        [InlineData("")]
        [InlineData("wrong data")]
        [InlineData("13.04.2024 6:45:00 +05:00")]
        public void StartTime_DoesntChangeValue_WhenTimeCorrect_ButEndTimeIncorrect(string startTime)
        {
            _sut.StartTime = startTime;
            _sut.EndTime = "bla bla";
            _sut.Time = "1:00:00";
            var expected = startTime;

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));
            Assert.Equal(expected, _sut.StartTime);
        }

        //[Fact]
        //public void ValidateShould_ValidTimeAndSetStartTime_WhenTimeAndEndTimeCorrectButStartTimeIncorrect()
        //{
        //    _sut.EndTime = "28.10.2023 14:00";
        //    _sut.StartTime = "bla bla";
        //    _sut.Time = "1:00:00";
        //    Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        //    DateTimeOffset expected = new DateTimeOffset(2023, 10, 28, 13, 00, 00, TimeSpan.FromHours(5));
        //    Assert.True(DateTimeOffset.TryParse(_sut.StartTime, out var result));
        //    Assert.Equal(expected, result);
        //}


        //[Fact]
        //public void ValidateShould_ValidTimeAndSetStartTime_WhenTimeEndTimeAndStartTimeIsCorrect()
        //{
        //    _sut.EndTime = "28.10.2023 14:00";
        //    _sut.StartTime = "28.10.2023 12:00";
        //    _sut.Time = "1:00:00";
        //    Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        //    DateTimeOffset expected = new DateTimeOffset(2023, 10, 28, 13, 00, 00, TimeSpan.FromHours(5));
        //    Assert.True(DateTimeOffset.TryParse(_sut.StartTime, out var result));
        //    Assert.Equal(expected, result);
        //}



        //private bool IsCurrentTime(DateTimeOffset date)
        //{
        //    return (date - DateTimeOffset.Now) < TimeSpan.FromMilliseconds(500);
        //}

        //[Fact]
        //public void TimeShould_BeSet_WhenCerrectStartTimeChangedAndEndTimeIsCorrect()
        //{
        //    _sut.EndTime = "28.10.2023 14:00";
        //    _sut.Time = "1:00:00";
        //    _sut.StartTime = "28.10.2023 00:00";
        //    var expected = "14:00:00";
        //    Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        //    Assert.Equal(expected, _sut.Time);
        //}


        //[Fact]
        //public void TimeShould_BeSet_WhenCerrectEndTimeChangedAndStartTimeIsCorrect()
        //{
        //    _sut.Time = "1:00:00";
        //    _sut.StartTime = "28.10.2023 00:00";
        //    _sut.EndTime = "28.10.2023 14:00";
        //    var expected = "14:00:00";
        //    Assert.False(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Time)));
        //    Assert.Equal(expected, _sut.Time);
        //}

        //[Fact]
        //public void TimeShouldNot_ChangeStartTime_WhenStartTimeNotValid_IfStartTimeChanged()
        //{
        //    _sut.EndTime = "28.10.2023 14:00";
        //    _sut.Time = "1:00:00";
        //    _sut.StartTime = "29.10.2023 00:00";
        //    var expected = "29.10.2023 00:00";
        //    Assert.Equal(expected, _sut.StartTime);
        //    Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));

        //}

        //[Fact]
        //public void TimeShouldNot_ChangedStartTime_WhenEndTimeNotCorrect()
        //{
        //    _sut.StartTime = "28.10.2023 15:00";
        //    _sut.EndTime = "28.10.2023 14:00";
        //    _sut.Time = "1:00:00";

        //    Assert.Equal("28.10.2023 15:00", _sut.StartTime);
        //    Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));


        //}
    }
}

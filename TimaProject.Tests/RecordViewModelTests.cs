using FluentValidation.Validators;
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
    public class RecordViewModelTests
    {
        private readonly RecordViewModel _sut;

        public RecordViewModelTests()
        {
            _sut = new RecordViewModel(new RecordValidator());
        }

        [Theory]
        [InlineData("27.10.2023 12:45")]
        [InlineData("12:45 27/10/2023")]
        public void HasPropertyErrors_OnStartTime_WhenSetCorrectStartTime_ReturnFalse(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void HasPropertyErrors_OnStartTime_WhenSetIncorrectStartTime_ReturnTrue(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }

        [Theory]
        [InlineData("27.10.2023 12:45")]
        [InlineData("12:45 27/10/2023")]
        public void HasPropertyErrors_OnEndTime_WhenSetCorrectEndTime_ReturnFalse(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void HasPropertyErrors_OnEndTime_WhenSetIncorrectEndTime_ReturnTrue(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }

        [Theory]
        [InlineData("27.10.2023 12:45")]
        [InlineData("27.10.2023")]
        [InlineData("12:45 27/10/2023")]
        public void HasPropertyErrors_OnDate_WhenSetCorrectDate_ReturnFalse(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void HasPropertyErrors_OnDate_WhenSetIncorrectDate_ReturnTrue(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }

        [Fact]
        public void HasPropertyErrors_OnEndTime_WhenEndTimeErlierThanStartTime_ReturnTrue()
        {
            _sut.StartTime = "27.10.2023 14:00";
            _sut.EndTime = "26.10.2023 14:00";
            Assert.True(_sut.HasPropertyErrors(nameof(RecordViewModel.EndTime)));
        }

        [Fact]
        public void HasPropertyErrors_OnStartTime_WhenStartTimeLaterThanEndTime_returnTrue()
        {
            _sut.EndTime = "26.10.2023 14:00";
            _sut.StartTime = "27.10.2023 14:00";
            Assert.True(_sut.HasPropertyErrors(nameof(RecordViewModel.StartTime)));
        }


        [Fact]
        public void StartTime_WhenStartTimeInits_ReturnEmptyString()
        {
            Assert.Equal(string.Empty, _sut.StartTime);
        }

        [Fact]
        public void HasPropertyErrors_OnStartTime_AfterStartTimeInits_ReturnFalse()
        {
            var result = _sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime));

            Assert.False(result);
        }

        [Fact]
        public void EndTime_WhenEndTimeInits_ReturnEmptyString()
        {
            Assert.Equal(string.Empty, _sut.EndTime);
        }


        [Fact]
        public void HasPropertyErrors_OnEndTime_AfterEndTimeInits_ReturnFalse()
        {
            var result = _sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime));

            Assert.False(result);
        }

        [Fact]
        public void Date_WhenDateInits_ReturnEmptyString()
        {
            Assert.Equal(string.Empty, _sut.Date);
        }

        [Fact]
        public void HasPropertyErrors_OnDate_AfterDateInits_ReturnFalse()
        {
            var result = _sut.HasPropertyErrors(nameof(TimeFormViewModel.Date));

            Assert.False(result);
        }

        [Fact]
        public void HasPropertyErrors_OnStartTime_WhenStartTimeHasSetEmptyStringTwice_ReturnTrue()
        {
            _sut.StartTime = "24.05.2022 7:45";
            _sut.StartTime = "";

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.StartTime)));
        }

        [Fact]
        public void HasPropertyErrors_OnEndTime_WhenEndTimeHasSetEmptyStringTwice_ReturnTrue()
        {
            _sut.EndTime = "24.05.2022 7:45";
            _sut.EndTime = "";

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.EndTime)));
        }

        [Fact]
        public void HasPropertyErrors_OnDate_WhenDateHasSetEmptyStringTwice_ReturnTrue()
        {
            _sut.Date = "24.05.2022";
            _sut.Date = "";

            Assert.True(_sut.HasPropertyErrors(nameof(TimeFormViewModel.Date)));
        }


    }
}

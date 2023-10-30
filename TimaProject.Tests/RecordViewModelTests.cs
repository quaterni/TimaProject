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
        public void ValidateShould_ValidateStartTime_WhenSetCorrectStartTime(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void ValidateShould_AddErrorStartTime_WhenSetIncorrectStartTime(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }

        [Theory]
        [InlineData("27.10.2023 12:45")]
        [InlineData("12:45 27/10/2023")]
        public void ValidateShould_ValidateEndTime_WhenSetCorrectEndTime(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void ValidateShould_AddErrorEndTime_WhenSetIncorrectEndTime(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }

        [Theory]
        [InlineData("27.10.2023 12:45")]
        [InlineData("27.10.2023")]
        [InlineData("12:45 27/10/2023")]
        public void ValidateShould_ValidateDate_WhenSetCorrectDate(string input)
        {
            _sut.StartTime = input;
            Assert.False(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }


        [Theory]
        [InlineData("sdfdfh")]
        [InlineData("322547568")]
        [InlineData(null)]
        public void ValidateShould_AddErrorDate_WhenSetIncorrectDate(string input)
        {
            _sut.StartTime = input;
            Assert.True(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }

        [Fact]
        public void ValidateShould_AddErrorEndTime_WhenEndTimeSetErlierThanStartTime()
        {
            _sut.StartTime = "27.10.2023 14:00";
            _sut.EndTime = "26.10.2023 14:00";
            Assert.True(_sut.PropertyHasErrors(nameof(RecordViewModel.EndTime)));
        }

        [Fact]
        public void ValidateShould_AddErrorStartTime_WhenStartTimeLaterThanEndTime()
        {
            _sut.EndTime = "26.10.2023 14:00";
            _sut.StartTime = "27.10.2023 14:00";
            Assert.True(_sut.PropertyHasErrors(nameof(RecordViewModel.StartTime)));
        }

    }
}

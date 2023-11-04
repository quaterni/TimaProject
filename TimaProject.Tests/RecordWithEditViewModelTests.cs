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
    public class RecordWithEditViewModelTests
    {
        private readonly RecordViewModelWithEdit _sut;

        public RecordWithEditViewModelTests()
        {

            _sut = new RecordViewModelWithEdit(
                new MockNavigationService(),
                () => new TimeFormViewModel(new RecordValidator(), new MockNavigationService()),
                new RecordValidator());
        }

        [Fact]
        public void TimeFrom_AfterInitialisation_IsNull()
        {
            Assert.Null(_sut.TimeForm);
        }

        [Fact]
        public void ApplyTimeForm_SetTimeForm()
        {
            _sut.ApplyTimeForm();
            Assert.NotNull(_sut.TimeForm);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("bla bla")]
        [InlineData("24.08.2008 14:57:00")]
        public void ApplyTimeForm_SetCurrentStartTimeToTimeForm(string startTime)
        {
            _sut.StartTime = startTime;
            _sut.ApplyTimeForm();

            Assert.Equal(startTime, _sut.TimeForm!.StartTime);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("bla bla")]
        [InlineData("24.08.2008 14:57")]
        public void ApplyTimeForm_SetCurrentEndTimeToTimeForm(string endTime)
        {
            _sut.EndTime = endTime;
            _sut.ApplyTimeForm();

            Assert.Equal(endTime, _sut.TimeForm!.EndTime);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("bla bla")]
        [InlineData("24.08.2008")]
        public void ApplyTimeForm_SetCurrentDateToTimeForm(string date)
        {
            _sut.Date = date;
            _sut.ApplyTimeForm();

            Assert.Equal(date, _sut.TimeForm!.Date);
        }

        [Fact]
        public void ApplyTimeForm_WhenIsActive_DisableEndTimeToTimeForm()
        {
            _sut.IsActive = true;
            _sut.ApplyTimeForm();
            Assert.False(_sut.TimeForm.IsActive);
        }

        [Fact]
        public void IncorrectFieldsFromTimeFromShould_BeIgnored()
        {
            var startTimeExpected = "27.10.2023 14:45";
            var endTimeExpected = "27.10.2023 20:57";
            var dateExpected = "27.10.2023";
            _sut.StartTime = startTimeExpected;
            _sut.EndTime = endTimeExpected;
            _sut.Date = dateExpected;

            var timeForm = new TimeFormViewModel(new RecordValidator(), new MockNavigationService());
            _sut.TimeForm = timeForm;
            timeForm.StartTime = "sdf";
            timeForm.EndTime = "sdf";
            timeForm.Date = "sfds";
            Assert.Equal(startTimeExpected, _sut.StartTime);
            Assert.Equal(endTimeExpected, _sut.EndTime);
            Assert.Equal(dateExpected, _sut.Date);
        }

    }
}

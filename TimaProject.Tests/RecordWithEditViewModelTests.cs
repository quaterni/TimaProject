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
        public void TimeFromShould_BeNull_AfterInitialisation()
        {
            Assert.Null(_sut.TimeForm);
        }

        [Fact]
        public void StartTimeShuold_BeSetTimeFormStartTime_WhenItCorrect()
        {
            var timeForm = new TimeFormViewModel(new RecordValidator(), new MockNavigationService());
            _sut.TimeForm = timeForm;
            var input = "27.10.2023 14:45";
            timeForm.StartTime = input;
            Assert.Equal(input, _sut.StartTime);
        }


        [Fact]
        public void EndTimeShuold_BeSetTimeFormEndTime_WhenItCorrect()
        {
            var timeForm = new TimeFormViewModel(new RecordValidator(), new MockNavigationService());
            _sut.TimeForm = timeForm;
            var input = "27.10.2023 14:45";
            timeForm.EndTime = input;
            Assert.Equal(input, _sut.EndTime);
        }

        [Fact]
        public void DateShuold_BeSetTimeFormDate_WhenItCorrect()
        {
            var timeForm = new TimeFormViewModel(new RecordValidator(), new MockNavigationService());
            _sut.TimeForm = timeForm;
            var input = "27.10.2023";
            timeForm.Date = input;
            Assert.Equal(input, _sut.Date);
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

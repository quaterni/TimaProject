using Moq;
using TimaProject.Desctop.Exceptions;
using TimaProject.Desctop.ViewModels;
using Xunit;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Services;
using FluentAssertions;
using AutoFixture;
using AutoFixture.Xunit2;


namespace TimaProject.Desctop.Tests.ViewModels
{ 
    public class TimeFormViewModelShould
    {
        private readonly TimeFormViewModel _sut;

        private static TimeDTO _timeDTO
        {
            get
            {
                var date = new DateOnly(2024, 2, 10);
                var startTime = new DateTime(date, new TimeOnly(10, 10, 10));
                var endTime = startTime.AddHours(2).AddMinutes(15).AddSeconds(37);
                var time = endTime - startTime;
                return new TimeDTO(startTime.ToString(), endTime.ToString(), time.ToString(), date.ToString());
            }
        }

        private readonly Mock<ITimeService> _mockTimeService;

        public TimeFormViewModelShould()
        {
            _mockTimeService = new Mock<ITimeService>();

            _sut = new TimeFormViewModel(
                _timeDTO,
                _mockTimeService.Object);

            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.PropertyError,
                    new Fixture().Create<TimeDTO>(),
                    string.Empty));
        }

        [Fact]
        public void Init_GetValuesFromTimeDTO()
        {
            _sut.StartTime.Should().Be(_timeDTO.StartTime);
            _sut.EndTime.Should().Be(_timeDTO.EndTime);
            _sut.Time.Should().Be(_timeDTO.Time);
            _sut.Date.Should().Be(_timeDTO.Date);
        }

        [Fact]
        public void StartTime_WnenChanged_InvokeTimeService()
        {
            var newValue = new Fixture().Create<string>();


            _sut.StartTime = newValue;

            _mockTimeService.Verify(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()), Times.Once);
            _sut.StartTime.Should().Be(newValue);
        }

        [Fact]
        public void EndTime_WnenChanged_InvokeTimeService()
        {
            var newValue = new Fixture().Create<string>();


            _sut.EndTime = newValue;

            _mockTimeService.Verify(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()), Times.Once);
            _sut.EndTime.Should().Be(newValue);
        }

        [Fact]
        public void Time_WnenChanged_InvokeTimeService()
        {
            var newValue = new Fixture().Create<string>();

            _sut.Time = newValue;

            _mockTimeService.Verify(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()), Times.Once);
            _sut.Time.Should().Be(newValue);
        }


        [Fact]
        public void Date_WnenChanged_InvokeTimeService()
        {
            var newValue = new Fixture().Create<string>();

            _sut.Date = newValue;

            _mockTimeService.Verify(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()), Times.Once);
            _sut.Date.Should().Be(newValue);
        }

        [Fact]
        public void StartTime_WhenHasPropertyError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.PropertyError, 
                    new Fixture().Create<TimeDTO>(), 
                    errorMessage));

            _sut.StartTime = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }


        [Fact]
        public void EndTime_WhenHasPropertyError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.PropertyError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.EndTime = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }


        [Fact]
        public void Time_WhenHasPropertyError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.PropertyError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.Time = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }


        [Fact]
        public void Date_WhenHasPropertyError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.PropertyError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.Date = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Single().ErrorMessage.Should().Be(errorMessage);
        }


        [Fact]
        public void StartTime_WhenHasComponentError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.ComponentError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.StartTime = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.StartTime.Should().Be(newValue);

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }

        [Fact]
        public void EndTime_WhenHasComponentError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.ComponentError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.EndTime = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.EndTime.Should().Be(newValue);

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }

        [Fact]
        public void Time_WhenHasComponentError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.ComponentError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.Time = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.Time.Should().Be(newValue);

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }

        [Fact]
        public void Date_WhenHasComponentError()
        {
            string errorMessage = new Fixture().Create<string>();
            string newValue = new Fixture().Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.ComponentError,
                    new Fixture().Create<TimeDTO>(),
                    errorMessage));

            _sut.Date = newValue;

            _sut.HasErrors.Should().BeTrue();
            _sut.Date.Should().Be(newValue);

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Single().ErrorMessage.Should().Be(errorMessage);
            _sut.GetErrors(nameof(ITimeFormViewModel.StartTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.EndTime)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Time)).Should().BeEmpty();
            _sut.GetErrors(nameof(ITimeFormViewModel.Date)).Should().BeEmpty();
        }

        [Fact]
        public void StartTime_WhenValid_ApplysValuesFromTimeService()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(time.StartTime, newEndTime, newTime, newDate),
                    string.Empty));

            _sut.StartTime = newStartTime;

            _sut.StartTime.Should().Be(newStartTime);
            _sut.EndTime.Should().Be(newEndTime);
            _sut.Time.Should().Be(newTime);
            _sut.Date.Should().Be(newDate);
        }

        [Fact]
        public void EndTime_WhenValid_ApplysValuesFromTimeService()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, time.EndTime, newTime, newDate),
                    string.Empty));

            _sut.EndTime = newEndTime;

            _sut.StartTime.Should().Be(newStartTime);
            _sut.EndTime.Should().Be(newEndTime);
            _sut.Time.Should().Be(newTime);
            _sut.Date.Should().Be(newDate);
        }

        [Fact]
        public void Time_WhenValid_ApplysValuesFromTimeService()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, time.Time, newDate),
                    string.Empty));

            _sut.Time = newTime;

            _sut.StartTime.Should().Be(newStartTime);
            _sut.EndTime.Should().Be(newEndTime);
            _sut.Time.Should().Be(newTime);
            _sut.Date.Should().Be(newDate);
        }

        [Fact]
        public void Date_WhenValid_ApplysValuesFromTimeService()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, newTime, time.Date),
                    string.Empty));

            _sut.Date = newDate;

            _sut.StartTime.Should().Be(newStartTime);
            _sut.EndTime.Should().Be(newEndTime);
            _sut.Time.Should().Be(newTime);
            _sut.Date.Should().Be(newDate);
        }

        [Fact]
        public void StartTime_WhenValid_RaisesTimeChanged()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, newTime, newDate),
                    string.Empty));

            var eventInfo = Assert.Raises<TimeDTO>(
                h => _sut.TimeSelected += h,
                h => _sut.TimeSelected -= h,
                ()=> _sut.StartTime = newStartTime);

            eventInfo.Sender.Should().Be(_sut);
            TimeDTO result = eventInfo.Arguments;
            result.StartTime.Should().Be(newStartTime);
            result.EndTime.Should().Be(newEndTime);
            result.Time.Should().Be(newTime);
            result.Date.Should().Be(newDate);
        }

        [Fact]
        public void EndTime_WhenValid_RaisesTimeChanged()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, newTime, newDate),
                    string.Empty));

            var eventInfo = Assert.Raises<TimeDTO>(
                h => _sut.TimeSelected += h,
                h => _sut.TimeSelected -= h,
                () => _sut.EndTime = newEndTime);

            eventInfo.Sender.Should().Be(_sut);
            TimeDTO result = eventInfo.Arguments;
            result.StartTime.Should().Be(newStartTime);
            result.EndTime.Should().Be(newEndTime);
            result.Time.Should().Be(newTime);
            result.Date.Should().Be(newDate);
        }

        [Fact]
        public void Time_WhenValid_RaisesTimeChanged()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, newTime, newDate),
                    string.Empty));

            var eventInfo = Assert.Raises<TimeDTO>(
                h => _sut.TimeSelected += h,
                h => _sut.TimeSelected -= h,
                () => _sut.Time = newTime);

            eventInfo.Sender.Should().Be(_sut);
            TimeDTO result = eventInfo.Arguments;
            result.StartTime.Should().Be(newStartTime);
            result.EndTime.Should().Be(newEndTime);
            result.Time.Should().Be(newTime);
            result.Date.Should().Be(newDate);
        }

        [Fact]
        public void Date_WhenValid_RaisesTimeChanged()
        {
            Fixture fixture = new();
            string newStartTime = fixture.Create<string>();
            string newEndTime = fixture.Create<string>();
            string newTime = fixture.Create<string>();
            string newDate = fixture.Create<string>();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                (prop, time) => new TimeServiceResult(
                    SolvingResult.NoError,
                    new TimeDTO(newStartTime, newEndTime, newTime, newDate),
                    string.Empty));

            var eventInfo = Assert.Raises<TimeDTO>(
                h => _sut.TimeSelected += h,
                h => _sut.TimeSelected -= h,
                () => _sut.Date = newDate);

            eventInfo.Sender.Should().Be(_sut);
            TimeDTO result = eventInfo.Arguments;
            result.StartTime.Should().Be(newStartTime);
            result.EndTime.Should().Be(newEndTime);
            result.Time.Should().Be(newTime);
            result.Date.Should().Be(newDate);
        }

        [Fact]
        public void StartTime_WhenValid_ClearPropertyError()
        {
            Fixture fixture = new();
            string validStartTime = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.StartTime, validStartTime));

            _sut.StartTime = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.StartTime = validStartTime;
            _sut.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void EndTime_WhenValid_ClearPropertyError()
        {
            Fixture fixture = new();
            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.EndTime, validValue));

            _sut.EndTime = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.EndTime = validValue;

            _sut.HasErrors.Should().BeFalse();
        }


        [Fact]
        public void Time_WhenValid_ClearPropertyError()
        {
            Fixture fixture = new();
            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.Time, validValue));

            _sut.Time = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.Time = validValue;

            _sut.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void Date_WhenValid_ClearPropertyError()
        {
            Fixture fixture = new();
            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.Date, validValue));

            _sut.Date = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.Date = validValue;

            _sut.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void StartTime_WhenValid_ClearComponentError()
        {
            Fixture fixture = new();
            _mockTimeService
                 .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                 .Returns<string, TimeDTO>(
                     (prop, time) => new TimeServiceResult(
                         SolvingResult.ComponentError,
                         fixture.Create<TimeDTO>(),
                         fixture.Create<string>()));
            _sut.StartTime = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();

            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.StartTime, validValue));
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.StartTime = validValue;

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Should().BeEmpty();
            _sut.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void EndTime_WhenValid_ClearComponentError()
        {
            Fixture fixture = new();
            _mockTimeService
                 .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                 .Returns<string, TimeDTO>(
                     (prop, time) => new TimeServiceResult(
                         SolvingResult.ComponentError,
                         fixture.Create<TimeDTO>(),
                         fixture.Create<string>()));
            _sut.EndTime = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();

            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.EndTime, validValue));
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.EndTime = validValue;

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Should().BeEmpty();
            _sut.HasErrors.Should().BeFalse();
        }


        [Fact]
        public void Time_WhenValid_ClearComponentError()
        {
            Fixture fixture = new();
            _mockTimeService
                 .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                 .Returns<string, TimeDTO>(
                     (prop, time) => new TimeServiceResult(
                         SolvingResult.ComponentError,
                         fixture.Create<TimeDTO>(),
                         fixture.Create<string>()));
            _sut.Time = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();

            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.Time, validValue));
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.Time = validValue;

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Should().BeEmpty();
            _sut.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void Date_WhenValid_ClearComponentError()
        {
            Fixture fixture = new();
            _mockTimeService
                 .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                 .Returns<string, TimeDTO>(
                     (prop, time) => new TimeServiceResult(
                         SolvingResult.ComponentError,
                         fixture.Create<TimeDTO>(),
                         fixture.Create<string>()));
            _sut.Date = fixture.Create<string>();
            _sut.HasErrors.Should().BeTrue();

            string validValue = fixture.Create<string>();
            fixture.Customize<TimeDTO>(
                t => t.With(x => x.Date, validValue));
            _mockTimeService
                .Setup(s => s.Solve(It.IsAny<string>(), It.IsAny<TimeDTO>()))
                .Returns<string, TimeDTO>(
                    (prop, time) => new TimeServiceResult(
                        SolvingResult.NoError,
                        fixture.Create<TimeDTO>(),
                        string.Empty));

            _sut.Date = validValue;

            _sut.GetErrors(nameof(ITimeFormViewModel.ComponentError)).Should().BeEmpty();
            _sut.HasErrors.Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void EndTime_WhenCantEdit_ThrowException_IfHasSet(string applyingEndTime)
        {
            var sut = new TimeFormViewModel(
                _timeDTO,
                _mockTimeService.Object,
                false);

            Assert.Throws<SettingDisableEndTimeException>(() => sut.EndTime = applyingEndTime);
        }
    }
}

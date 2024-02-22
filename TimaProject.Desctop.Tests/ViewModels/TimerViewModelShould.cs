using Moq;
using TimaProject.Desctop.ViewModels;
using Xunit;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using FluentAssertions;
using AutoFixture;
using TimaProject.Desctop.DTOs;
using AutoFixture.Xunit2;


namespace TimaProject.Desctop.Tests.ViewModels
{

    public class TimerViewModelShould
    {
        private readonly TimerViewModel _sut;
        private readonly Mock<ITimeFormViewModelFactory> _mockTimeFormViewModelFactory;
        private readonly Mock<IProjectFormViewModelFactory> _mockProjectFormViewModelFactory;
        private readonly Mock<IRecordService> _mockRecordService;
        private readonly Mock<ITimerExecutorFactory> _mockTimerExecutorFactory;
        private readonly Mock<ITimerExecutor> _mockTimerExecutor;
        private readonly Mock<IDateReportService> _mockDateReportService;

        private static readonly DateTime s_CurrentTime = new Fixture().Create<DateTime>();
        private static readonly DateOnly s_CurrentDate = DateOnly.FromDateTime(s_CurrentTime);

        public TimerViewModelShould()
        {
            _mockTimeFormViewModelFactory = new Mock<ITimeFormViewModelFactory>();
            _mockProjectFormViewModelFactory = new Mock<IProjectFormViewModelFactory>();
            _mockRecordService = new Mock<IRecordService>();
            _mockTimerExecutorFactory = new Mock<ITimerExecutorFactory>();
            _mockTimerExecutor = new Mock<ITimerExecutor>();
            _mockDateReportService = new Mock<IDateReportService>();

            _sut = new TimerViewModel(
                _mockTimeFormViewModelFactory.Object,
                _mockProjectFormViewModelFactory.Object,
                _mockRecordService.Object,
                _mockTimerExecutorFactory.Object,
                _mockDateReportService.Object);

            _mockTimerExecutorFactory
                .Setup(s => s.Create())
                .Returns(_mockTimerExecutor.Object);

            _mockTimerExecutor
                .Setup(s => s.CurrentTime())
                .Returns(s_CurrentTime);

            _mockDateReportService
                .Setup(s=> s.CurrentDate())
                .Returns(s_CurrentDate);

        }

        [Fact]
        public void Init()
        {
            _sut.State.Should().Be(TimerState.NotRunning);
            _sut.Time.Should().Be(TimeSpan.Zero.ToString());
        }

        [Fact]
        public void StartTime_WhenTimerStarted_SetStartTime()
        {
            DateTime startingTime = new Fixture().Create<DateTime>();
            _sut.StartTime = startingTime.ToString();

            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor
                .VerifySet(
                    s => s.StartTime = It.IsInRange(
                        startingTime.AddSeconds(-1), 
                        startingTime.AddSeconds(1), 
                        Moq.Range.Exclusive), 
                    Times.Once);

            DateTime.Parse(_sut.StartTime).Should().BeCloseTo(startingTime, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void WhenTimerStarted_CreatesTimerExecutor()
        {
            _sut.TimerCommand.Execute(null);

            _mockTimerExecutorFactory.Verify(s => s.Create(), Times.Once);
        }


        [Fact]
        public void StartTime_WhenTimerStartedAndStartingTimeNotValid_SetCurrentTime()
        {
            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor.Verify(s => s.CurrentTime(), Times.Once);
            _mockTimerExecutor
            .VerifySet(
                s => s.StartTime = It.IsInRange(
                    s_CurrentTime.AddSeconds(-1),
                    s_CurrentTime.AddSeconds(1),
                    Moq.Range.Exclusive),
                Times.Once);
            DateTime.Parse(_sut.StartTime).Should().BeCloseTo(s_CurrentTime, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void WhenTimerStarted_StateIsRunning()
        {
            _sut.TimerCommand.Execute(null);

            _sut.State.Should().Be(TimerState.Running);
        }

        [Fact]
        public void WhenTimerStarted_StartsTimerExecutor()
        {
            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor.Verify(s=> s.Start(), Times.Once);
        }

        [Theory, MemberData(nameof(AddingRecords))]
        public void WhenTimerStarted_AddNewRecord(RecordDto recordDTO, RecordDto resultDTO)
        {
            _sut.StartTime = recordDTO.StartTime;
            _sut.Title = recordDTO.Title;
            _sut.Date = recordDTO.Date;

            _mockRecordService
                .Setup(s => s.AddRecord(It.IsAny<RecordDto>()))
                .Callback<RecordDto>(r =>
                {
                    r.StartTime.Should().Be(resultDTO.StartTime);
                    r.Date.Should().Be(resultDTO.Date);
                    r.Title.Should().Be(resultDTO.Title);
                });

            _sut.TimerCommand.Execute(null);

            _mockRecordService
                .Verify(s=> s.AddRecord(It.IsAny<RecordDto>()), Times.Once);
        }

        public static IEnumerable<object[]> AddingRecords()
        {
            yield return new object[]
            {
                new RecordDto("", "", Guid.Empty),
                new RecordDto(s_CurrentTime.ToString(), "", Guid.Empty)
                {
                    Date = s_CurrentDate.ToString()
                }
            };

            yield return new object[]
            {
                new RecordDto("2024-02-07T23:45", "My Title", Guid.Empty)
                {
                    Date="2024-02-16",
                    ProjectName = "MyProject"
                },
                new RecordDto("2024-02-07T23:45", "My Title", Guid.Empty)
                {
                    Date="2024-02-16",
                    ProjectName = "MyProject"
                },
            };
        }

        [Fact]
        public void Date_WhenTimerStartedAndDateNotValid_SetCurrentDate()
        {
            _sut.TimerCommand.Execute(null);

            _mockDateReportService.Verify(s => s.CurrentDate(), Times.Once);
            _sut.Date.Should().Be(s_CurrentDate.ToString());
        }
        
        [Fact]
        public void Date_WhenTimerStarted_SetDate()
        {
            var date = DateOnly.FromDateTime(new Fixture().Create<DateTime>());
            _sut.Date = date.ToString();

            _sut.TimerCommand.Execute(null);

            _mockDateReportService.Verify(s => s.CurrentDate(), Times.Never);
            _sut.Date.Should().Be(date.ToString());
        }

        [Fact]
        public void WhenTimerStarted_SubscribesOnTick()
        {
            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor
                .VerifyAdd(s => s.Tick += It.IsAny<EventHandler<TimeSpan>>(), Times.Once);
        }

        [Theory]
        [AutoData]
        public void Time_WhenTickRaised_SetTimeFromTick(TimeSpan ticks)
        {
            
            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor.Raise(s => s.Tick += null, this, ticks);

            _sut.Time.Should().Be(ticks.ToString());
        }

        [Theory]
        [AutoData]
        public void StartTime_WhenChangedOnRunning_SetToTimerExecutorStartTime(DateTime startTime)
        {
            _sut.TimerCommand.Execute(null);

            _sut.StartTime = startTime.ToString();

            _mockTimerExecutor.VerifySet(
                s => s.StartTime = 
                    It.IsInRange(
                        startTime.AddSeconds(-1), 
                        startTime.AddSeconds(1),
                        Moq.Range.Exclusive), 
                Times.Once);
        }

        [Fact]
        public void WhenPropertiesChangedOnRunning_UpdateRecord()
        {
            _sut.TimerCommand.Execute(null);

            Fixture fixture = new();
            _sut.Title = fixture.Create<string>();

            _sut.StartTime = fixture.Create<DateTime>().ToString();

            _sut.Date = DateOnly.FromDateTime(fixture.Create<DateTime>()).ToString();

            _mockRecordService
                .Verify(
                    s => s.UpdateRecord(It.IsAny<RecordDto>()), 
                    Times.Exactly(3));
        }

        [Fact]
        public void WhenTimerCommandExecuteTwice_StopsTimer()
        {
            _sut.TimerCommand.Execute(null);

            _sut.TimerCommand.Execute(null);

            _sut.State.Should().Be(TimerState.NotRunning);
        }

        [Fact]
        public void WhenTimerStop_UpdateRecordWithCurrentTime()
        {
            _sut.StartTime = new Fixture().Create<DateTime>().ToString();
            _sut.TimerCommand.Execute(null);
            _mockRecordService
                .Setup(s => s.UpdateRecord(It.IsAny<RecordDto>()))
                .Callback<RecordDto>(r =>
                {
                    DateTime.Parse(r.EndTime)
                        .Should()
                        .BeCloseTo(
                            s_CurrentTime,
                            TimeSpan.FromSeconds(1));
                    r.IsActive.Should().BeFalse();
                });

            _sut.TimerCommand.Execute(null);

            _mockTimerExecutor.Verify(s => s.CurrentTime(), Times.Once);

        }

        [Fact]
        public void WhenTimerStop_SetDefaultValues()
        {
            _sut.TimerCommand.Execute(null);
            _sut.TimerCommand.Execute(null);

            _sut.StartTime.Should().BeEmpty();
            _sut.EndTime.Should().BeEmpty();
            _sut.ProjectName.Should().BeEmpty();
            _sut.ProjectId.Should().BeEmpty();
            _sut.Title.Should().BeEmpty();
            _sut.Date.Should().BeEmpty();
            _sut.Time.Should().Be(TimeSpan.Zero.ToString());
        }
    }
}

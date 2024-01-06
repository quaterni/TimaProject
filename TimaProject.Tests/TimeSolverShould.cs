using FluentValidation;
using Moq;
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
    public class TimeSolverShould
    {
        private readonly TimeSolver _sut;
        private readonly Mock<ITimeBase> _mockTimeBase;
        private readonly AbstractValidator<ITimeBase> _validator;

        public TimeSolverShould()
        {
            _mockTimeBase = new Mock<ITimeBase>();
            _validator = new TimeValidator();
            _sut = new TimeSolver(_mockTimeBase.Object, _validator);

            _mockTimeBase
                .SetupGet(x => x.StartTime)
                .Returns("");
            _mockTimeBase
               .SetupGet(x => x.EndTime)
               .Returns("");
            _mockTimeBase
               .SetupGet(x => x.Time)
               .Returns("");
            _mockTimeBase
               .SetupGet(x => x.Date)
               .Returns("");
        }

        [Fact]
        public void PassNotValidValues()
        {
            _sut.Solve(nameof(ITimeBase.StartTime));
            _sut.Solve(nameof(ITimeBase.EndTime));
            _sut.Solve(nameof(ITimeBase.Time));
            _sut.Solve(nameof(ITimeBase.Date));
            _mockTimeBase.VerifySet(x => x.Time = It.IsAny<string>(), Times.Never);
            _mockTimeBase.VerifySet(x => x.EndTime = It.IsAny<string>(), Times.Never);
            _mockTimeBase.VerifySet(x => x.StartTime = It.IsAny<string>(), Times.Never);
            _mockTimeBase.VerifySet(x => x.Date = It.IsAny<string>(), Times.Never);

        }

        [Fact]
        public void SetTime_IfStartTimeAndEndTimeValid()
        {
            _mockTimeBase
                .SetupSet(x => x.Time = It.IsAny<string>())
                .Callback<string>(stringTime => {
                    Assert.True(TimeSpan.TryParse(stringTime, out TimeSpan time));
                    Assert.Equal(2, time.Hours);
                    Assert.Equal(0, time.Minutes);
                    Assert.Equal(0, time.Seconds);
                    Assert.Equal(0, time.Days);
                });

            _mockTimeBase
                .SetupGet(x => x.StartTime)
                .Returns("01.01.2024 10:00");
            _mockTimeBase
               .SetupGet(x => x.EndTime)
               .Returns("01.01.2024 12:00");

            _sut.Solve(nameof(ITimeBase.StartTime));
            _mockTimeBase.VerifySet(x => x.Time = It.IsAny<string>(), Times.Once);

            _sut.Solve(nameof(ITimeBase.EndTime));
            _mockTimeBase.VerifySet(x => x.Time = It.IsAny<string>(), Times.Exactly(2));
        }

        [Fact]
        public void SetStartTime_IfEndTimeAndTimeValid()
        {
            _mockTimeBase
               .SetupSet(x => x.StartTime = It.IsAny<string>())
               .Callback<string>(stringTime => {
                   Assert.True(DateTime.TryParse(stringTime, out DateTime startTime));
                   Assert.Equal(2024, startTime.Year);
                   Assert.Equal(1, startTime.Month);
                   Assert.Equal(1, startTime.Day);
                   Assert.Equal(10, startTime.Hour);
                   Assert.Equal(0, startTime.Minute);
                   Assert.Equal(0, startTime.Second);
               });

            _mockTimeBase
               .SetupGet(x => x.EndTime)
               .Returns("01.01.2024 12:00");

            _mockTimeBase
               .SetupGet(x => x.Time)
               .Returns("2:00");

            _sut.Solve(nameof(ITimeBase.Time));

            _mockTimeBase.VerifySet(x => x.StartTime = It.IsAny<string>(), Times.Once);

        }
    }
}

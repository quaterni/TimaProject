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
    public class TimeValidatorShould
    {
        private readonly AbstractValidator<ITimeBase> _sut;
        private readonly Mock<ITimeBase> _mockTimeBase;

        public TimeValidatorShould()
        {
            _mockTimeBase = new Mock<ITimeBase>();
            _sut = new TimeValidator();
        }

        [Theory, MemberData(nameof(DateTimeTestData))]
        public void StartTimePassOnCorrectTestData(string startTime, bool isValid)
        {
            _mockTimeBase.SetupGet(x=> x.StartTime).Returns(startTime);

            var result = _sut.Validate(_mockTimeBase.Object).ToDictionary();

            if (isValid)
            {
                Assert.False(result.ContainsKey(nameof(ITimeBase.StartTime)));
            }
            else
            {
                Assert.True(result.ContainsKey(nameof(ITimeBase.StartTime)));
                Assert.Single(result[nameof(ITimeBase.StartTime)]);
                Assert.Equal("StartTime not valid.", result[nameof(ITimeBase.StartTime)].First());
            }
        }

        [Theory, MemberData(nameof(DateTimeTestData))]
        public void EndTimePassOnCorrectTestData(string endTime, bool isValid)
        {
            _mockTimeBase.SetupGet(x => x.EndTime).Returns(endTime);

            var result = _sut.Validate(_mockTimeBase.Object).ToDictionary();

            if (isValid)
            {
                Assert.False(result.ContainsKey(nameof(ITimeBase.EndTime)));
            }
            else
            {
                Assert.True(result.ContainsKey(nameof(ITimeBase.EndTime)));
                Assert.Single(result[nameof(ITimeBase.EndTime)]);
                Assert.Equal("EndTime not valid.", result[nameof(ITimeBase.EndTime)].First());
            }
        }

        public static IEnumerable<object[]> DateTimeTestData()
        {
            yield return new object[] { "10.12.2023 10:45:56", true };
            yield return new object[] { "10/12/2023 10:45:56", true };
            yield return new object[] {  "10:45:56 10/12/2023", true };
            yield return new object[] {  "", false };
            yield return new object[] {  "bla bla", false };
        }

        [Theory, MemberData(nameof(DateTestData))]
        public void DatePassOnCorrectTestData(string date, bool isValid)
        {
            _mockTimeBase.SetupGet(x => x.Date).Returns(date);

            var result = _sut.Validate(_mockTimeBase.Object).ToDictionary();

            if (isValid)
            {
                Assert.False(result.ContainsKey(nameof(ITimeBase.Date)));
            }
            else
            {
                Assert.True(result.ContainsKey(nameof(ITimeBase.Date)));
                Assert.Single(result[nameof(ITimeBase.Date)]);
                Assert.Equal("Date not valid.", result[nameof(ITimeBase.Date)].First());
            }
        }

        public static IEnumerable<object[]> DateTestData()
        {
            yield return new object[] { "10.12.2023", true };
            yield return new object[] { "10/12/2023 10:45:56", false };
            yield return new object[] { "10/12/2023", true };
            yield return new object[] { "", false };
            yield return new object[] { "bla bla", false };
        }

        [Fact]
        public void NotPass_IfEndTimeErlierThanStartTime()
        {
            _mockTimeBase.SetupGet(x => x.EndTime).Returns("02.01.2024 10:45:00");
            _mockTimeBase.SetupGet(x => x.StartTime).Returns("02.01.2024 11:45:00");

            var result = _sut.Validate(_mockTimeBase.Object).ToDictionary();
            Assert.True(result.ContainsKey(nameof(ITimeBase.StartTime)));
            Assert.True(result.ContainsKey(nameof(ITimeBase.EndTime)));

            Assert.Single(result[nameof(ITimeBase.EndTime)]);
            Assert.Single(result[nameof(ITimeBase.StartTime)]);

            Assert.Equal("StartTime must be erlier than EndTime.", result[nameof(ITimeBase.StartTime)].First());
            Assert.Equal("StartTime must be erlier than EndTime.", result[nameof(ITimeBase.EndTime)].First());
        }

        [Theory, MemberData(nameof(TimeTestData))]
        public void TimePassOnCorrectTestData(string time, bool isValid)
        {
            _mockTimeBase.SetupGet(x => x.Time).Returns(time);

            var result = _sut.Validate(_mockTimeBase.Object).ToDictionary();

            if (isValid)
            {
                Assert.False(result.ContainsKey(nameof(ITimeBase.Time)));
            }
            else
            {
                Assert.True(result.ContainsKey(nameof(ITimeBase.Time)));
                Assert.Single(result[nameof(ITimeBase.Time)]);
                Assert.Equal("Time not valid.", result[nameof(ITimeBase.Time)].First());
            }
        }

        public static IEnumerable<object[]> TimeTestData()
        {
            yield return new object[] { "10:45:56", true };
            yield return new object[] { "10/12/2023 10:45:56", false };
            yield return new object[] { "14:45", true };
            yield return new object[] { "", false };
            yield return new object[] { "bla bla", false };
        }
    }
}

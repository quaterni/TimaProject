using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Services;
using Xunit;

namespace TimaProject.Desctop.Tests.Services
{
    public class TimeServiceShould
    {
        private readonly TimeService _sut;
        private readonly Mock<IValidator<TimeDTO>> _mockValidator;

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

        public TimeServiceShould()
        {
            _mockValidator = new Mock<IValidator<TimeDTO>>();
            _sut = new TimeService(_mockValidator.Object);

            _mockValidator
                .Setup(s=> s.Validate(It.IsAny<TimeDTO>()))
                .Returns<TimeDTO>(s => 
                {
                    return new ValidationResult();
                });
        }

        [Theory, MemberData(nameof(PropertyErrors))]
        public void WhenPropertyNotValid_ReturnPropertyError(string propertyName, string errorMessage)
        {
            ValidationFailure failure = new ValidationFailure(propertyName, errorMessage);
            ValidationResult validationResult = new(new List<ValidationFailure>() { failure });
            _mockValidator
                 .Setup(s => s.Validate(It.IsAny<TimeDTO>()))
                 .Returns<TimeDTO>(s => validationResult);

            TimeServiceResult solveResult = _sut.Solve(propertyName, _timeDTO);

            solveResult.Result.Should().Be(SolvingResult.PropertyError);
            solveResult.ErrorMessage.Should().Be(errorMessage);
            solveResult.Value.Should().Be(_timeDTO);
        }

        public static IEnumerable<object[]> PropertyErrors()
        {
            yield return new object[] { nameof(ITimeFormViewModel.StartTime), "Input string not valid" };
            yield return new object[] { nameof(ITimeFormViewModel.EndTime), "Input string not valid" };
            yield return new object[] { nameof(ITimeFormViewModel.Time), "Input string not valid" };
            yield return new object[] { nameof(ITimeFormViewModel.Date), "Input string not valid" };
        }

        [Theory, MemberData(nameof(ComponentErrors))]
        public void WhenSolverPropertyNotValid_ReturnComponentErrors(string propertyName, string solverProperty, string errorMessage)
        {
            ValidationFailure failure = new ValidationFailure(solverProperty, "");
            ValidationResult validationResult = new(new List<ValidationFailure>() { failure });
            _mockValidator
                 .Setup(s => s.Validate(It.IsAny<TimeDTO>()))
                 .Returns<TimeDTO>(s => validationResult);

            TimeServiceResult solveResult = _sut.Solve(propertyName, _timeDTO);

            solveResult.Result.Should().Be(SolvingResult.ComponentError);
            solveResult.ErrorMessage.Should().Be(errorMessage);
            solveResult.Value.Should().Be(_timeDTO);
        }


        public static IEnumerable<object[]> ComponentErrors()
        {
            yield return new object[] 
            {
                nameof(ITimeFormViewModel.StartTime),
                nameof(ITimeFormViewModel.EndTime), 
                "Cannot resolve, ending time has error"
            };

            yield return new object[] { 
                nameof(ITimeFormViewModel.EndTime),
                nameof(ITimeFormViewModel.StartTime),
                "Cannot resolve, starting time has error" 
            };

            yield return new object[]
            { 
                nameof(ITimeFormViewModel.Time),
                nameof(ITimeFormViewModel.EndTime), 
                "Cannot resolve, ending time has error" 
            };
        }

        [Theory, MemberData(nameof(SolvingValues))]
        public void WhenValuesCorrect_SolveIt(string propertyName, TimeDTO timeDTO, TimeDTO resultDTO)
        {
            TimeServiceResult solveResult = _sut.Solve(propertyName, timeDTO);

            solveResult.Result.Should().Be(SolvingResult.NoError);
            solveResult.ErrorMessage.Should().BeEmpty();
            solveResult.Value.Should().Be(resultDTO);
        }

        public static IEnumerable<object[]> SolvingValues()
        {
            yield return new object[] { nameof(ITimeFormViewModel.StartTime), _timeDTO, _timeDTO };
            yield return new object[] { nameof(ITimeFormViewModel.EndTime), _timeDTO, _timeDTO };
            yield return new object[] { nameof(ITimeFormViewModel.Time), _timeDTO, _timeDTO };
            yield return new object[] { nameof(ITimeFormViewModel.Date), _timeDTO, _timeDTO };

            var startingTime = DateTime.Parse(_timeDTO.StartTime);
            var endingTime = DateTime.Parse(_timeDTO.EndTime);
            var time = TimeSpan.Parse(_timeDTO.Time);
            var date = DateOnly.Parse(_timeDTO.Date);

            var testDate = date.AddDays(4).ToString();
            yield return new object[]
            { 
                nameof(ITimeFormViewModel.Date), 
                _timeDTO with { Date= testDate },
                _timeDTO with { Date = testDate } 
            };

            var testEndingTime = endingTime
                .AddDays(1)
                .AddHours(3)
                .AddMinutes(23)
                .AddSeconds(56);

            var solvingTime = testEndingTime - startingTime;

            yield return new object[]
            { 
                nameof(ITimeFormViewModel.EndTime), 
                _timeDTO with { EndTime = testEndingTime.ToString() },
                _timeDTO with { EndTime = testEndingTime.ToString() , Time= solvingTime.ToString() } 
            };

            var testStartingTime = startingTime
                .AddDays(-1)
                .AddHours(-3)
                .AddMinutes(-23)
                .AddSeconds(-56);

            solvingTime = endingTime - testStartingTime;

            yield return new object[]
            {
                nameof(ITimeFormViewModel.StartTime),
                _timeDTO with { StartTime = testStartingTime.ToString() },
                _timeDTO with { StartTime = testStartingTime.ToString() , Time= solvingTime.ToString() }
            };

            var testTime = time.Add(TimeSpan.FromMinutes(1234));
            var solveStartingTime = endingTime - testTime;
            yield return new object[]
            {
                nameof(ITimeFormViewModel.Time),
                _timeDTO with { Time = testTime.ToString() },
                _timeDTO with { Time = testTime.ToString(), StartTime = solveStartingTime.ToString() }
            };

        }


        [Theory, MemberData(nameof(AnotherErrors))]
        public void WhenAnotherPropertyNotValid_ReturnNoError(
            string property,string anotrherProperty, TimeDTO timeDTO, TimeDTO resultDTO)
        {
            ValidationFailure failure = new ValidationFailure(anotrherProperty, "");
            ValidationResult validationResult = new(new List<ValidationFailure>() { failure });
            _mockValidator
                 .Setup(s => s.Validate(It.IsAny<TimeDTO>()))
                 .Returns<TimeDTO>(s => validationResult);

            TimeServiceResult solveResult = _sut.Solve(property, timeDTO);

            solveResult.Result.Should().Be(SolvingResult.NoError);
            solveResult.ErrorMessage.Should().BeEmpty();
            solveResult.Value.Should().Be(resultDTO);
        }

        public static IEnumerable<object[]> AnotherErrors()
        {
            yield return new object[] 
            {
                nameof(ITimeFormViewModel.StartTime), 
                nameof(ITimeFormViewModel.Date),
                _timeDTO with {Date= "wrong"},
                _timeDTO with {Date= "wrong"}
            };

            yield return new object[]
            {
                nameof(ITimeFormViewModel.StartTime),
                nameof(ITimeFormViewModel.Time),
                _timeDTO with {Time= "wrong"},
                _timeDTO
            };

            yield return new object[]
            {
                nameof(ITimeFormViewModel.EndTime),
                nameof(ITimeFormViewModel.Date),
                _timeDTO with {Date= "wrong"},
                _timeDTO with {Date= "wrong"}
            };
            yield return new object[]
            {
                nameof(ITimeFormViewModel.EndTime),
                nameof(ITimeFormViewModel.Time),
                _timeDTO with {Time= "wrong"},
                _timeDTO
            };
            yield return new object[]
            {
                nameof(ITimeFormViewModel.Time),
                nameof(ITimeFormViewModel.Date),
                _timeDTO with {Date= "wrong"},
                _timeDTO with {Date= "wrong"}
            };
            yield return new object[]
            {
                nameof(ITimeFormViewModel.Time),
                nameof(ITimeFormViewModel.StartTime),
                _timeDTO with {StartTime= "wrong"},
                _timeDTO
            };
            yield return new object[]
            {
                nameof(ITimeFormViewModel.Date),
                nameof(ITimeFormViewModel.StartTime),
                _timeDTO with {StartTime= "wrong"},
                _timeDTO with {StartTime= "wrong"},
            };

            yield return new object[]
            {
                nameof(ITimeFormViewModel.Date),
                nameof(ITimeFormViewModel.EndTime),
                _timeDTO with {EndTime= "wrong"},
                _timeDTO with {EndTime= "wrong"}
            };

            yield return new object[]
            {
                nameof(ITimeFormViewModel.Date),
                nameof(ITimeFormViewModel.Time),
                _timeDTO with {Time= "wrong"},
                _timeDTO with {Time= "wrong"}
            };
        }

        [Theory, MemberData(nameof(EndingTimeLessStartingTimeTests))]
        public void WhenEndingTimeLessThanStartingTime_ReturnComponentError(
            string property, string errorMessage, TimeDTO timeDTO, TimeDTO resultDTO)
        {
            TimeServiceResult solveResult = _sut.Solve(property, timeDTO);

            solveResult.Result.Should().Be(SolvingResult.ComponentError);
            solveResult.ErrorMessage.Should().Be(errorMessage);
            solveResult.Value.Should().Be(resultDTO);
        }



        public static IEnumerable<object[]> EndingTimeLessStartingTimeTests()
        {
            yield return new object[]
            {
                nameof(ITimeFormViewModel.StartTime),
                "Starting time must be less than ending time.",
                _timeDTO with {StartTime= _timeDTO.EndTime, EndTime= _timeDTO.StartTime},
                _timeDTO with {StartTime= _timeDTO.EndTime, EndTime= _timeDTO.StartTime}
            };

            yield return new object[]
            {
                nameof(ITimeFormViewModel.EndTime),
                "Starting time must be less than ending time.",
                _timeDTO with {StartTime= _timeDTO.EndTime, EndTime= _timeDTO.StartTime},
                _timeDTO with {StartTime= _timeDTO.EndTime, EndTime= _timeDTO.StartTime}
            };
        }
    }
}

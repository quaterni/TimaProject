using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.Services
{
    internal class TimeService : ITimeService
    {
        private readonly IValidator<TimeDTO> _timeValidator;

        public TimeService(IValidator<TimeDTO> timeValidator)
        {
            _timeValidator = timeValidator;
        }

        public TimeServiceResult Solve(string propertyName, TimeDTO timeDTO)
        {
            switch (propertyName)
            {
                case nameof(ITimeFormViewModel.StartTime):
                    return SolveStartTime(timeDTO);
                case nameof(ITimeFormViewModel.EndTime):
                    return SolveEndTime(timeDTO);
                case nameof(ITimeFormViewModel.Time):
                    return SolveTime(timeDTO);
                case nameof(ITimeFormViewModel.Date):
                    return SolveDate(timeDTO);
                default:
                    throw new NotImplementedException();
            }
        }

        private TimeServiceResult SolveStartTime(TimeDTO timeDTO)
        {
            var result = _timeValidator.Validate(timeDTO).ToDictionary();
            if (result.ContainsKey(nameof(ITimeFormViewModel.StartTime)))
            {
                return new TimeServiceResult(
                    SolvingResult.PropertyError, 
                    timeDTO, 
                    result[nameof(ITimeFormViewModel.StartTime)].FirstOrDefault() ?? "");
            }
            if (result.ContainsKey(nameof(ITimeFormViewModel.EndTime)))
            {
                return new TimeServiceResult(
                    SolvingResult.ComponentError,
                    timeDTO,
                    "Cannot resolve, ending time has error");
            }
            DateTime startingTime = DateTime.Parse(timeDTO.StartTime);
            DateTime endingTime = DateTime.Parse(timeDTO.EndTime);

            if(startingTime >= endingTime)
            {
                return new TimeServiceResult(
                    SolvingResult.ComponentError,
                    timeDTO,
                    "Starting time must be less than ending time.");
            }


            TimeSpan time = endingTime - startingTime;

            return new TimeServiceResult(
                SolvingResult.NoError,
                new TimeDTO(
                    startingTime.ToString(),
                    endingTime.ToString(),
                    time.ToString(),
                    timeDTO.Date),
                string.Empty);

        }

        private TimeServiceResult SolveEndTime(TimeDTO timeDTO)
        {
            var result = _timeValidator.Validate(timeDTO).ToDictionary();
            if (result.ContainsKey(nameof(ITimeFormViewModel.EndTime)))
            {
                return new TimeServiceResult(
                    SolvingResult.PropertyError,
                    timeDTO,
                    result[nameof(ITimeFormViewModel.EndTime)].FirstOrDefault() ?? "");
            }
            if (result.ContainsKey(nameof(ITimeFormViewModel.StartTime)))
            {
                return new TimeServiceResult(
                    SolvingResult.ComponentError,
                    timeDTO,
                    "Cannot resolve, starting time has error");
            }
            DateTime startingTime = DateTime.Parse(timeDTO.StartTime);
            DateTime endingTime = DateTime.Parse(timeDTO.EndTime);

            if (startingTime >= endingTime)
            {
                return new TimeServiceResult(
                    SolvingResult.ComponentError,
                    timeDTO,
                    "Starting time must be less than ending time.");
            }


            TimeSpan time = endingTime - startingTime;

            return new TimeServiceResult(
                SolvingResult.NoError,
                new TimeDTO(
                    startingTime.ToString(),
                    endingTime.ToString(),
                    time.ToString(),
                    timeDTO.Date),
                string.Empty);
        }

        private TimeServiceResult SolveTime(TimeDTO timeDTO)
        {
            var result = _timeValidator.Validate(timeDTO).ToDictionary();
            if (result.ContainsKey(nameof(ITimeFormViewModel.Time)))
            {
                return new TimeServiceResult(
                    SolvingResult.PropertyError,
                    timeDTO,
                    result[nameof(ITimeFormViewModel.Time)].FirstOrDefault() ?? "");
            }
            if (result.ContainsKey(nameof(ITimeFormViewModel.EndTime)))
            {
                return new TimeServiceResult(
                    SolvingResult.ComponentError,
                    timeDTO,
                    "Cannot resolve, ending time has error");
            }
            DateTime endingTime = DateTime.Parse(timeDTO.EndTime);
            TimeSpan time = TimeSpan.Parse(timeDTO.Time);

            DateTime startingTime = endingTime - time;

            return new TimeServiceResult(
                SolvingResult.NoError,
                new TimeDTO(
                    startingTime.ToString(),
                    endingTime.ToString(),
                    time.ToString(),
                    timeDTO.Date),
                string.Empty);
        }

        private TimeServiceResult SolveDate(TimeDTO timeDTO)
        {
            var result = _timeValidator.Validate(timeDTO).ToDictionary();
            if (result.ContainsKey(nameof(ITimeFormViewModel.Date)))
            {
                return new TimeServiceResult(
                    SolvingResult.PropertyError,
                    timeDTO,
                    result[nameof(ITimeFormViewModel.Date)].FirstOrDefault() ?? "");
            }
            return new TimeServiceResult(
                SolvingResult.NoError,
                timeDTO,
                string.Empty);
        }
    }
}

using FluentValidation;
using System;

namespace TimaProject.ViewModels.Validators
{
    public class TimeValidator : AbstractValidator<ITimeBase>
    {
        public TimeValidator()
        {
            RuleFor(s => s.StartTime)
                .Must(startTime => DateTime.TryParse(startTime, out _))
                .WithMessage("StartTime not valid.");

            RuleFor(s => s.EndTime)
                .Must(endTime => DateTime.TryParse(endTime, out _))
                .WithMessage("EndTime not valid.");

            RuleFor(s => s.Time)
                .Must(time => TimeSpan.TryParse(time, out _))
                .WithMessage("Time not valid.");

            When(s => DateTime.TryParse(s.StartTime, out var _) &&
                    DateTime.TryParse(s.EndTime, out var _),
                () =>
                {
                    RuleFor(record => record.StartTime)
                        .Must((record, startTime) =>
                            DateTime.Parse(startTime) <=
                            DateTime.Parse(record.EndTime))
                        .WithMessage("StartTime must be erlier than EndTime.");
                    RuleFor(record => record.EndTime)
                        .Must((record, endTime) =>
                            DateTime.Parse(record.StartTime) <=
                            DateTime.Parse(endTime))
                        .WithMessage("StartTime must be erlier than EndTime.");
                }); 

            RuleFor(s => s.Date)
                .Must(date => DateOnly.TryParse(date, out _))
                .WithMessage("Date not valid.");
        }
    }
}

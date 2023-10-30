using FluentValidation;
using System;

namespace TimaProject.ViewModels.Validators
{
    public class RecordValidator : AbstractValidator<RecordViewModel>
    {
        public RecordValidator()
        {
            RuleFor(s => s.StartTime)
                .Must(startTime => DateTimeOffset.TryParse(startTime, out _))
                .WithMessage("StartTime not valid string");

            RuleFor(s => s.EndTime)
                .Must(endTime => DateTimeOffset.TryParse(endTime, out _))
                .WithMessage("EndTime not valid string");
            When(s => DateTimeOffset.TryParse(s.StartTime, out var _) &&
                    DateTimeOffset.TryParse(s.EndTime, out var _),
                () =>
                {
                    RuleFor(record => record.StartTime)
                        .Must((record, startTime) =>
                            DateTimeOffset.Parse(startTime) <
                            DateTimeOffset.Parse(record.EndTime))
                        .WithMessage("StartTime must be erlier than EndTime");
                    RuleFor(record => record.EndTime)
                        .Must((record, endTime) =>
                            DateTimeOffset.Parse(record.StartTime) <
                            DateTimeOffset.Parse(endTime))
                        .WithMessage("StartTime must be erlier than EndTime");
                }); 

            RuleFor(s => s.Date)
                .Must(date => DateOnly.TryParse(date, out _))
                .WithMessage("Date not valid string");
        }
    }
}

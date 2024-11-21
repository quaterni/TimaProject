using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Validators
{
    internal class TimeValidator : AbstractValidator<TimeDTO>
    {
        public TimeValidator()
        {
            RuleFor(s => s.StartTime).Must(s => DateTime.TryParse(s, out _)).WithMessage("StartTime not parsed");
            RuleFor(s => s.EndTime).Must(s => DateTime.TryParse(s, out _)).WithMessage("EndTime not parsed");
            RuleFor(s => s.Time).Must(s => DateTime.TryParse(s, out _)).WithMessage("Time not parsed");
            RuleFor(s => s.Date).Must(s => DateTime.TryParse(s, out _)).WithMessage("Date not parsed");
        }
    }
}

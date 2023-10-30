using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Validators;

namespace TimaProject.Tests
{
    internal class MockRecordValidator : AbstractValidator<RecordViewModel>
    {
        public MockRecordValidator()
        {
            Include(new RecordValidator());
            RuleFor(s => s.Title).NotEqual("NotValidate").WithMessage("some");
            RuleFor(s => s.StartTime).NotEqual(DateTimeOffset.MaxValue.ToString());
            RuleFor(s => s.Date).NotEqual(DateOnly.MaxValue.ToString());
        }
    }
}

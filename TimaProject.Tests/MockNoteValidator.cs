using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;

namespace TimaProject.Tests
{
    internal class MockNoteValidator : AbstractValidator<NoteViewModel>
    {
        public MockNoteValidator()
        {
            RuleFor(s => s.Title).Equal("NotValidate");
            RuleFor(s => s.StartTime).Equal(DateTimeOffset.MaxValue.ToString());
            RuleFor(s => s.Date).Equal(DateOnly.MaxValue.ToString());
        }
    }
}

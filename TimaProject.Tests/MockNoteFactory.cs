using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Services.Factories;
using TimaProject.Stores;
using TimaProject.ViewModels;

namespace TimaProject.Tests
{
    internal class MockNoteFactory : INoteFactory
    {
        public Note Create(NoteViewModel timeNoteViewModel)
        {
            var startTime = DateTimeOffset.Parse(timeNoteViewModel.StartTime);
            DateOnly date = DateOnly.Parse(timeNoteViewModel.Date);

            return new Note(startTime, 1)
            {
                EndTime = null,
                Title = timeNoteViewModel.Title,
                Project = timeNoteViewModel.Project,
                Text = timeNoteViewModel.Text,
                Date = date
            };
        }

        public Note CreateActiveNote(NoteViewModel timeNoteViewModel)
        {
            throw new NotImplementedException();
        }
    }
}

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
    internal class MockRecordFactory : IRecordFactory
    {
        public Record Create(RecordViewModel timeNoteViewModel)
        {
            var startTime = DateTime.Parse(timeNoteViewModel.StartTime);
            DateOnly date = DateOnly.Parse(timeNoteViewModel.Date);

            return new Record(startTime, date, 1)
            {
                EndTime = null,
                Title = timeNoteViewModel.Title,
                Project = timeNoteViewModel.Project
            };
        }

        public Record CreateActiveNote(RecordViewModel timeNoteViewModel)
        {
            throw new NotImplementedException();
        }
    }
}

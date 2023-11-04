using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Stores;
using TimaProject.ViewModels;

namespace TimaProject.Services.Factories
{
    public class RecordFactory : IRecordFactory
    {
        private readonly IRecordRepository _repository;
        private readonly IDateStore _dateStore;

        public RecordFactory(IRecordRepository repository, IDateStore dateStore)
        {
            _repository = repository;
            _dateStore = dateStore;
        }

        public Record Create(RecordViewModel timeNoteViewModel)
        {
            var startTime = DateTime.Parse(timeNoteViewModel.StartTime);
            DateTime? endTime = null;
            if (!timeNoteViewModel.EndTime.Equals(string.Empty))
            {
                endTime = DateTime.Parse(timeNoteViewModel.EndTime);
            }

            var date = DateOnly.Parse(timeNoteViewModel.Date);

            return new Record(startTime, date, (ulong)_repository.GetNewId())
            {
                EndTime = endTime,
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

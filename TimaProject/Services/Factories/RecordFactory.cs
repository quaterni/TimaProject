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
            var startTime = DateTimeOffset.Parse(timeNoteViewModel.StartTime);
            DateTimeOffset? endTime = null;
            if (timeNoteViewModel.EndTime is not null)
            {
                endTime = DateTimeOffset.Parse(timeNoteViewModel.EndTime);
            }

            return new Record(startTime, (ulong)_repository.GetNewId())
            {
                EndTime = endTime,
                Title = timeNoteViewModel.Title,
                Project = timeNoteViewModel.Project,
                Date = _dateStore.Date
            };
        }

        public Record CreateActiveNote(RecordViewModel timeNoteViewModel)
        {
            throw new NotImplementedException();
        }
    }
}

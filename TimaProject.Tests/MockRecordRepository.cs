using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.Tests
{
    internal class MockRecordRepository : IRecordRepository
    {
        private readonly List<Record> _notes = new();

        public List<Record> Notes => _notes;

        public event EventHandler? RecordsChanged;

        event EventHandler<RepositoryChangedEventArgs>? IRecordRepository.RecordsChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void AddRecord(Record note)
        {
            _notes.Add(note);
        }

        public bool Contains(Record record)
        {
            return _notes.Contains(record);
        }

        public bool DeleteRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Record> GetAllRecords(Func<Record, bool>? wherePredicate = null)
        {
            throw new NotImplementedException();
        }

        public ulong GetNewId()
        {
            return 1;
        }

        public IEnumerable<Record> GetRecords(FilterListingArgs filterListingArgs)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecord(Record note)
        {
            _notes[0] = note;
        }
    }
}

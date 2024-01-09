using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Exceptions;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class RecordRepository : IRecordRepository, IDisposable
    {
        public List<Record> _records;
        private readonly IRepository<Note> _noteRepository;

        private bool _lock;

        public RecordRepository(IRepository<Note> noteRepository)
        {
            _lock = false;
            _records = new();
            _noteRepository = noteRepository;
            _noteRepository.RepositoryChanged += OnNoteRepositoryChanged;
        }

        public event EventHandler<RepositoryChangedEventArgs<Record>>? RepositoryChanged;


        public void AddItem(Record record)
        {
            if(_records.Find(t => t.Id == record.Id) is not null)
            {
                throw new AddingNotUniqueItemException("Record already contains in repository.");
            }
            _records.Add(record);
            _lock = true;
            foreach(var note in record.Notes)
            {
                _noteRepository.AddItem(note);
            }
            _lock = false;
            OnRepositoryChanged(RepositoryChangedOperation.Add, record);
        }

        public void UpdateItem(Record record)
        {
            var n = _records.Find(t => t.Id == record.Id);
            if(n is null)
            {
                throw new NotFoundException<Record>(record ,"Record not found in the repository");
            }
            _records[_records.IndexOf(n)] = record;
            OnRepositoryChanged(RepositoryChangedOperation.Update, record);
        }

        public IEnumerable<Record> GetItems(Func<Record, bool>? wherePredicate = null)
        {
            if (wherePredicate == null)
                return _records;
            return _records.Where(wherePredicate);
        }

        private void OnRepositoryChanged(RepositoryChangedOperation operation, Record record)
        {
            RepositoryChanged?.Invoke(this, new RepositoryChangedEventArgs<Record>(record, operation));
        }

        public IEnumerable<Record> GetItems(FilterListingArgs filterListingArgs)
        {
            IEnumerable<Record> result = _records
                .Where(record => filterListingArgs.IsRecordValid(record))
                .OrderByDescending(r => r.EndTime);

            if (filterListingArgs.Count is int count)
            {
                result = result.Take(count);
            }
            return result;
        }

        public bool Contains(Record record)
        {
            return _records.Where(x=> x.Id.Equals(record.Id)).Any();
        }

        public bool RemoveItem(Record record)
        {
            _lock = true;
            foreach(var note in record.Notes)
            {
                _noteRepository.RemoveItem(note);
            }
            var result = _records.Remove(record);
            _lock = false;
            if (result)
            {
                OnRepositoryChanged(RepositoryChangedOperation.Remove, record);
            }
            return result;
        }

        public void Dispose()
        {
            _noteRepository.RepositoryChanged -= OnNoteRepositoryChanged;
        }


        private void OnNoteRepositoryChanged(object? sender, RepositoryChangedEventArgs<Note> e)
        {
            if (_lock)
            {
                return;
            }
            switch (e.Operation)
            {
                case RepositoryChangedOperation.Add:
                    AddNote(e.Item);
                    break;
                case RepositoryChangedOperation.Update:
                    UpdateNote(e.Item);
                    break;
                case RepositoryChangedOperation.Remove:
                    RemoveNote(e.Item);
                    break;
            }
        }

        private void RemoveNote(Note note)
        {
            var record = _records.Where(record => record.Id.Equals(note.RecordId)).Single();
            record.Notes.Remove(note);
            UpdateItem(record);
        }

        private void UpdateNote(Note note)
        {
            var record = _records.Where(record => record.Id.Equals(note.RecordId)).Single();
            var oldNote = record.Notes.Where(item => item.Id.Equals(note.Id)).Single();
            record.Notes[record.Notes.IndexOf(oldNote)] = note;
            UpdateItem(record);
        }

        private void AddNote(Note note)
        {
            var record = _records.Where(record => record.Id.Equals(note.RecordId)).Single();
            record.Notes.Add(note);
            UpdateItem(record);
        }


    }
}

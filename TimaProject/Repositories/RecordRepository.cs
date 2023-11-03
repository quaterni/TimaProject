using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private ulong st_idCounter;

        public List<Record> _records;

        public RecordRepository()
        {
            _records = new();
            st_idCounter = (ulong)_records.Count();
        }

        public event EventHandler<RepositoryChangedEventArgs>? RecordsChanged;

        public ulong GetNewId()
        {
            return ++st_idCounter;
        }

        public void AddRecord(Record record)
        {
            if(_records.Find(t => t.Id == record.Id) is not null)
            {
                throw new Exception("Note with current id exist");
            }
            _records.Add(record);
            OnRepositoryChanged(RepositoryChangedOperation.Add, record);
        }

        public void UpdateRecord(Record record)
        {
            var n = _records.Find(t => t.Id == record.Id);
            if(n is null)
            {
                throw new Exception("Note with current id not exist");
            }
            _records.Remove(n);
            _records.Add(record);
            OnRepositoryChanged(RepositoryChangedOperation.Update, record);
        }

        public IEnumerable<Record> GetAllRecords(Func<Record, bool>? wherePredicate = null)
        {
            if (wherePredicate == null)
                return _records;
            return _records.Where(wherePredicate);
        }

        private void OnRepositoryChanged(RepositoryChangedOperation operation, Record record)
        {
            RecordsChanged?.Invoke(this, new RepositoryChangedEventArgs(operation, record));
        }

        public IEnumerable<Record> GetRecords(FilterListingArgs filterListingArgs)
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
            return _records.Contains(record);
        }

        public bool DeleteRecord(Record record)
        {
            var result = _records.Remove(record);
            if (result)
            {
                OnRepositoryChanged(RepositoryChangedOperation.Delete, record);
            }
            return result;
        }
    }
}

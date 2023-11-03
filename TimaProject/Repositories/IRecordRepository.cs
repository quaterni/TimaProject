using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public interface IRecordRepository
    {
        public ulong GetNewId();

        public void AddRecord(Record record);

        public void UpdateRecord(Record record);

        public bool DeleteRecord(Record record);

        public bool Contains(Record record);

        public IEnumerable<Record> GetAllRecords(Func<Record, bool>? wherePredicate = null);

        public IEnumerable<Record> GetRecords(FilterListingArgs filterListingArgs);

        public event EventHandler<RepositoryChangedEventArgs>? RecordsChanged;
    }
}

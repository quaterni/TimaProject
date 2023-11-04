using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.Stores
{
    public class ListingRecordStore : IDisposable
    {
        private readonly IRecordRepository _recordRepository;

        private List<Record> _records;

        public List<Record> Records
        {
            get
            {
                return _records;
            }
        }

        private FilterListingArgs _filterListingArgs;

        public FilterListingArgs FilterListingArgs
        {
            get 
            { 
                return _filterListingArgs;
            }
            set
            {
                _filterListingArgs = value;
                OnFilterArgsChanged();
            }
        }


        public ListingRecordStore(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
            _recordRepository.RecordsChanged += OnRepositoryChanged;

            _records = new List<Record>();
            _filterListingArgs = new FilterListingArgs();
            OnFilterArgsChanged();
        }

        private void OnRepositoryChanged(object? sender, RepositoryChangedEventArgs e)
        {
            if (!FilterListingArgs.IsRecordValid(e.Record))
            {
                return;
            }
            _records = _recordRepository.GetRecords(FilterListingArgs).ToList();
            ListingChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ListingChanged;

        private void OnFilterArgsChanged()
        {
            _records = _recordRepository.GetRecords(FilterListingArgs).ToList();
            ListingChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _recordRepository.RecordsChanged -= OnRepositoryChanged;
        }
    }
}

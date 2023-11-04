using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Stores;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class ListingRecordViewModel :ViewModelBase
    {
        private readonly ListingRecordStore _listingRecordStore;

        private readonly EditableRecordViewModelFactory _editableRecordViewModelFactory;

        private IRecordRepository _recordRepository;

        private ObservableCollection<EditableRecordViewModel> _records;

        public ObservableCollection<EditableRecordViewModel> Records
        {
            get
            {
                return _records;
            }
            set
            {
                SetValue(ref _records, value);
            }
        }

        public ListingRecordViewModel(
            IRecordRepository noteRepository,
            ListingRecordStore listingRecordStore,
            EditableRecordViewModelFactory editableRecordViewModelFactory)
        {
            _records = new ObservableCollection<EditableRecordViewModel>();
            _editableRecordViewModelFactory = editableRecordViewModelFactory;
            _listingRecordStore = listingRecordStore;
            _recordRepository = noteRepository;
            _listingRecordStore.ListingChanged += OnListingChanged;
            OnListingChanged(this, EventArgs.Empty);
        }

        private void OnListingChanged(object? sender, EventArgs e)
        {
            var recordViewModels = _listingRecordStore.Records
                .Select(record => _editableRecordViewModelFactory.Create(record))
                .ToList();
            if(recordViewModels is null)
            {
                recordViewModels = new List<EditableRecordViewModel>();
            }
            Records = new(recordViewModels);        
        }

        public override void Dispose()
        {
            _recordRepository.RecordsChanged -= OnListingChanged;
            base.Dispose();
        }
    }
}

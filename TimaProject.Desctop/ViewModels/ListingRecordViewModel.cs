using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimaProject.Desctop.ViewModels.Factories;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.Stores;

namespace TimaProject.Desctop.ViewModels
{
    internal class ListingRecordViewModel : ViewModelBase
    {
        //private readonly EditableRecordViewModelFactory _editableRecordViewModelFactory;

        //private IRecordRepository _recordRepository;

        //private ObservableCollection<RecordViewModel> _records;

        //public ObservableCollection<RecordViewModel> Records
        //{
        //    get
        //    {
        //        return _records;
        //    }
        //    set
        //    {
        //        SetValue(ref _records, value);
        //    }
        //}

        public ListingRecordViewModel()
        {

        }

        //private void OnListingChanged(object? sender, EventArgs e)
        //{
        //    var recordViewModels = _recordRepository
        //        .GetItems(x => !x.IsActive)
        //        .OrderByDescending(x => x.EndTime)
        //        .Select(record => _editableRecordViewModelFactory.Create(record))
        //        .ToList();
        //    if (recordViewModels is null)
        //    {
        //        recordViewModels = new List<RecordViewModel>();
        //    }
        //    foreach (var recordViewModel in recordViewModels)
        //    {
        //        var oldVM = _records.Where(item => item.Record.Id.Equals(recordViewModel.Record.Id)).FirstOrDefault();
        //        recordViewModel.IsNoteExpanded = oldVM?.IsNoteExpanded ?? false;
        //    }

        //    Records = new(recordViewModels);
        //}

        //public override void Dispose()
        //{
        //    _recordRepository.RepositoryChanged -= OnListingChanged;
        //    base.Dispose();
        //}
    }
}

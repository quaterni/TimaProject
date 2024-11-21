using System;
using System.Linq;
using TimaProject.Desctop.Interfaces.Factories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.ViewModels
{
    public class ListingRecordViewModel : ObservableObject, IListingRecordViewModel
    {
        private readonly IRecordViewModelFactory _recordViewModelFactory;
        private readonly IRecordService _recordService;
        private readonly IDateService _dateReportService;

        private readonly Lazy<ObservableGroupedCollection<DateContainer, IRecordViewModel>> _lazyRecords;

        public ObservableGroupedCollection<DateContainer, IRecordViewModel> Records
        {
            get
            {
                return _lazyRecords.Value;
            }
        }


        public ListingRecordViewModel(
            IRecordViewModelFactory recordViewModelFactory,
            IRecordService recordService,
            IDateService dateReportService)
        {
            _recordViewModelFactory = recordViewModelFactory;
            _recordService = recordService;
            _dateReportService = dateReportService;

            _lazyRecords = new Lazy<ObservableGroupedCollection<DateContainer, IRecordViewModel>>(CreateRecords);
            _recordService.RecordChanged += OnRecordChanged;
        }

        private void OnRecordChanged(object? sender, EntityChangedEventArgs<RecordDto> e)
        {
            switch (e.Operation)
            {
                case Operation.Add:
                    AddRecord(e.Value);
                    break;
                case Operation.Update:
                    UpdateRecord(e.Value);
                    break;
                case Operation.Delete:
                    DeleteRecord(e.Value);
                    break;

            }
            OnPropertyChanged(nameof(Records));
        }

        private void DeleteRecord(RecordDto value)
        {
            if (value.EndTime is null)
            {
                return;
            }
            foreach (var group in Records)
            {
                var changedValue = group.FirstOrDefault(x => x.Id == value.RecordId);
                if (changedValue is not null)
                {
                    int index = group.IndexOf(changedValue);
                    group.Remove(changedValue);
                }
            }
        }

        // TODO: adding group violate order
        private void UpdateRecord(RecordDto value)
        {
            if (value.EndTime is null)
            {
                return;
            }
            foreach (var group in Records)
            {
                if (group.Key.Date.ToString() == value.Date)
                {
                    var changedValue = group.FirstOrDefault(x => x.Id == value.RecordId);
                    if (changedValue is not null)
                    {
                        int index = group.IndexOf(changedValue);
                        group.Remove(changedValue);
                        group.Insert(index, _recordViewModelFactory.Create(value));
                        return;
                    }
                    group.Add(_recordViewModelFactory.Create(value));
                    return;
                }
            }
            Records.AddGroup(CreateContainer(value)).Add(_recordViewModelFactory.Create(value));

        }

        private void AddRecord(RecordDto value)
        {
            if(value.EndTime is null)
            {
                return;
            }
            foreach(var group in Records)
            {
                if(group.Key.Date.ToString() == value.Date)
                {
                    int index = group.Count();
                    foreach(var record in group)
                    {
                        var dt1 = DateTime.Parse(record.EndTime);
                        var dt2 = DateTime.Parse(value.EndTime);
                        if(dt2 > dt1)
                        {
                            index = group.IndexOf(record);
                        }
                    }
                    group.Insert(index, _recordViewModelFactory.Create(value));
                    return;
                }
            }
            Records.AddGroup(CreateContainer(value)).Add(_recordViewModelFactory.Create(value));
        }

        private DateContainer CreateContainer(RecordDto record)
        {
            var date = DateOnly.Parse(record.Date);
            return new DateContainer(date, _dateReportService.GetTimeAmountPerDate(date));
        }

        private ObservableGroupedCollection<DateContainer, IRecordViewModel> CreateRecords()
        {
            var groups = _recordService
                .GetRecords()
                .Select(x => _recordViewModelFactory.Create(x))
                .GroupBy(x =>
                {
                    var date = DateOnly.Parse(x.Date);
                    var hours = _dateReportService.GetTimeAmountPerDate(date);
                    return new DateContainer(date, hours);
                })
                .OrderBy(x => x.Key.Date)
                .Select(x => new ObservableGroup<DateContainer, IRecordViewModel>(x));
            return new ObservableGroupedCollection<DateContainer, IRecordViewModel>(groups);
        }
    }
}

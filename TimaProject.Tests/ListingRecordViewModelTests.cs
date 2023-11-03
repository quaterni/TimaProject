using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.ViewModels;
using TimaProject.Repositories;
using TimaProject.Stores;
using Xunit;
using System.Windows;
using TimaProject.ViewModels.Factories;
using TimaProject.ViewModels.Validators;
using System.ComponentModel;

namespace TimaProject.Tests
{
    public class ListingRecordViewModelTests
    {
        private readonly ListingRecordViewModel _sut;
        private readonly ListingRecordStore _listingRecordStore;

        private readonly RecordRepository _recordRepository;

        private readonly Models.Record[] _records;

        public ListingRecordViewModelTests()
        {
            _recordRepository = new RecordRepository();
            _listingRecordStore = new ListingRecordStore(_recordRepository);

            _records = new Models.Record[] {
                new Models.Record(DateTime.MinValue, DateOnly.MinValue, 1)
                {
                    Project = new Project("Math", 1)
                },
                new Models.Record(DateTime.MinValue, DateOnly.MinValue, 2)
                {
                    Project = new Project("Math", 1),
                    EndTime = DateTime.MaxValue
                },
                new Models.Record(DateTime.MinValue, DateOnly.MinValue, 3)
                {
                    Project = new Project("C#", 2)
                },
                new Models.Record(DateTime.MinValue, DateOnly.MinValue, 4)
                {
                    Project = new Project("C#", 2),
                    EndTime = DateTime.MaxValue
                }
            };

            foreach (var record in _records)
            {
                _recordRepository.AddRecord(record);
            }

            _sut = new ListingRecordViewModel(
                _recordRepository, 
                _listingRecordStore, 
                new EditableRecordViewModelFactory(
                    _recordRepository,
                    ()=> new MockNavigationService(),
                    null,
                    new RecordValidator()));
        }

        [Fact]
        public void RecordsShould_BeCorrectWithStore()
        {
            Assert.Equal(_listingRecordStore.Records.Count(), _sut.Records.Count());

            for (int i = 0; i < _sut.Records.Count; i++)
            {
                var resultRecord = ReflectRecord(_sut.Records[i]);
                Assert.Equal(_listingRecordStore.Records[i], resultRecord);
            }
        }

        [Fact]
        public void RecordsShuold_BeUpdated_WhenListingStoreChanged()
        {
            var newValue = new Models.Record(
                    DateTime.MinValue,
                    DateOnly.MinValue,
                    10)
            {
                EndTime = DateTime.MaxValue
            };
            _recordRepository.AddRecord(newValue);

            Assert.Equal(_listingRecordStore.Records.Count(), _sut.Records.Count());

            for (int i = 0; i < _sut.Records.Count; i++)
            {
                var resultRecord = ReflectRecord(_sut.Records[i]);
                Assert.Equal(_listingRecordStore.Records[i], resultRecord);
            }
        }

        [Fact]
        public void RecordsShould_InvokeRecordsPropertyChanged_WhenListingStoreChanged()
        {
            var newValue = new Models.Record(
                DateTime.MinValue,
                DateOnly.MinValue,
                10)
            {
                EndTime = DateTime.MaxValue
            };

            var result = Assert.RaisesAny<PropertyChangedEventArgs>(
                e => _sut.PropertyChanged += new PropertyChangedEventHandler(e),
                e => _sut.PropertyChanged -= new PropertyChangedEventHandler(e),
                () => _recordRepository.AddRecord(newValue));

            Assert.Equal(nameof(ListingRecordViewModel.Records), result.Arguments.PropertyName);

        }

        private Models.Record ReflectRecord(EditableRecordViewModel editableRecordViewModel)
        {
            var fieldInfo = typeof(EditableRecordViewModel)
                .GetFields(
                    System.Reflection.BindingFlags.Instance 
                    | System.Reflection.BindingFlags.NonPublic)
                .Where(e => e.Name == "_record").ToArray()[0];
            if(fieldInfo is null)
            {
                throw new NullReferenceException();
            }
            return (Models.Record)fieldInfo.GetValue(editableRecordViewModel)!;
        }

    }
}

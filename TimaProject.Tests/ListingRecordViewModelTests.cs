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
using Moq;
using MvvmTools.Navigation.Services;

namespace TimaProject.Tests
{
    public class ListingRecordViewModelTests
    {
        private readonly ListingRecordViewModel _sut;
        
        private readonly ListingRecordStore _listingRecordStore;

        private readonly Mock<IRecordRepository> _mockRecordRepository;
        private readonly Mock<INavigationService> _mockNavigationService;

        private readonly Models.Record[] _records;

        public ListingRecordViewModelTests()
        {
            _mockNavigationService = new();
            _mockRecordRepository = new();

            _listingRecordStore = new ListingRecordStore(_mockRecordRepository.Object);

            _mockRecordRepository
                .Setup(x => x.GetRecords(It.IsAny<FilterListingArgs>()))
                .Returns(new List<Models.Record>
                {
                    new Models.Record(DateTime.Now.AddDays(-1), DateOnly.FromDateTime(DateTime.Now), 1),
                    new Models.Record(DateTime.Now.AddDays(-2), DateOnly.FromDateTime(DateTime.Now), 2),
                    new Models.Record(DateTime.Now.AddDays(-3), DateOnly.FromDateTime(DateTime.Now), 3),
                });

            _sut = new ListingRecordViewModel(
                _mockRecordRepository.Object, 
                _listingRecordStore, 
                new EditableRecordViewModelFactory(
                    _mockRecordRepository.Object,
                    _mockNavigationService.Object,
                    _mockNavigationService.Object,
                    null,
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
            _mockRecordRepository.Raise(x => x.RecordsChanged += null, new RepositoryChangedEventArgs(RepositoryChangedOperation.Add, newValue));


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
                () => _mockRecordRepository.Raise(x => x.RecordsChanged += null, new RepositoryChangedEventArgs(RepositoryChangedOperation.Add, newValue)));


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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.Stores;
using Xunit;

namespace TimaProject.Tests
{
    public class ListingRecordStoreTests
    {
        private readonly ListingRecordStore _sut;

        private readonly RecordRepository _recordRepository;

        private readonly Models.Record[] _records;

        public ListingRecordStoreTests()
        {
            _recordRepository = new RecordRepository();
            _sut = new ListingRecordStore(_recordRepository);

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

            foreach(var record in _records)
            {
                _recordRepository.AddRecord(record);
            }
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenFilterListingArgsChanged()
        {
            var filterArgs = new FilterListingArgs()
            {
                IsActive = true
            };

            _sut.FilterListingArgs = filterArgs;

            var result = _sut.Records;

            Assert.Equal(2, result.Count());

            Assert.Equal(_records[0], result[0]);
            Assert.Equal(_records[2], result[1]);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenFilterListingArgsChanged2()
        {
            var filterArgs = new FilterListingArgs()
            {
                IsActive = false
            };

            _sut.FilterListingArgs = filterArgs;

            var result = _sut.Records;

            Assert.Equal(2, result.Count());

            Assert.Equal(_records[1], result[0]);
            Assert.Equal(_records[3], result[1]);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenFilterListingArgsChanged3()
        {
            var filterArgs = new FilterListingArgs()
            {
                IsActive = null
            };

            _sut.FilterListingArgs = filterArgs;

            var result = _sut.Records;

            Assert.Equal(4, result.Count());

            Assert.Equal(_records[1], result[0]);
            Assert.Equal(_records[3], result[1]);
            Assert.Equal(_records[0], result[2]);
            Assert.Equal(_records[2], result[3]);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenFilterListingArgsChanged4()
        {
            var filterArgs = new FilterListingArgs()
            {
                Projects = new List<Project>() { new Project("Math", 1)}
            };

            _sut.FilterListingArgs = filterArgs;

            var result = _sut.Records;

            Assert.Single(result);

            Assert.Equal(_records[1], result[0]);
        }

        [Fact]
        public void ListingRecordShould_ApplyAllInactiveRecordsByDefault()
        {
            var result = _sut.Records;

            Assert.Equal(2, result.Count());

            Assert.Equal(_records[1], result[0]);
            Assert.Equal(_records[3], result[1]);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenRepositoryAddValidRecord()
        {
            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5)
            {
                EndTime = DateTime.MaxValue
            };

            _recordRepository.AddRecord(record);

            Assert.Contains(record, _sut.Records);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenRepositoryDeleteValidRecord()
        {
            Assert.Contains(_records[1], _sut.Records);
            _recordRepository.DeleteRecord(_records[1]);
            Assert.DoesNotContain(_records[1], _sut.Records);
        }

        [Fact]
        public void ListingRecordShould_ChangeListing_WhenRepositoryUpdateValidRecord()
        {
            var record = _records[1];
            Assert.Contains(record, _sut.Records);
            var updatedRecord = record with { Project = new Project("TimaProject", 3) };
            _recordRepository.UpdateRecord(updatedRecord);
            Assert.DoesNotContain(record, _sut.Records);
            Assert.Contains(updatedRecord, _sut.Records);
        }

        [Fact]
        public void ListingChangedShould_Invoke_WhenRepositoryDeleteValidRecord()
        {
            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5)
            {
                EndTime = DateTime.MaxValue
            };

            _recordRepository.AddRecord(record);

            Assert.RaisesAny(
                (e) => _sut.ListingChanged += e,
                (e) => _sut.ListingChanged -=e,
                ()=> _recordRepository.DeleteRecord(record));
        }


        [Fact]
        public void ListingChangedShouldNot_Invoke_WhenRepositoryDeleteInvalidRecord()
        {
            var isInvoked = false;

            Action<object?, EventArgs> checkFunc = (s, e)=> isInvoked = true;

            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5);

            _recordRepository.AddRecord(record);

            _sut.ListingChanged += checkFunc.Invoke;

             _recordRepository.DeleteRecord(record);

            _sut.ListingChanged -= checkFunc.Invoke;

            Assert.False(isInvoked);
        }

        [Fact]
        public void ListingChangedShould_Invoke_WhenRepositoryAddValidRecord()
        {
            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5)
            {
                EndTime = DateTime.MaxValue
            };

            Assert.RaisesAny(
                (e) => _sut.ListingChanged += e,
                (e) => _sut.ListingChanged -= e,
                () => _recordRepository.AddRecord(record));
        }

        [Fact]
        public void ListingChangedShouldNot_Invoke_WhenRepositoryAddInvalidRecord()
        {
            var isInvoked = false;

            Action<object?, EventArgs> checkFunc = (s, e) => isInvoked = true;

            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5);


            _sut.ListingChanged += checkFunc.Invoke;

            _recordRepository.AddRecord(record);

            _sut.ListingChanged -= checkFunc.Invoke;

            Assert.False(isInvoked);
        }

        [Fact]
        public void ListingChangedShould_Invoke_WhenRepositoryUpdateValidRecord()
        {
            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5)
            {
                EndTime = DateTime.MaxValue
            };

            _recordRepository.AddRecord(record);
            var updatedRecord = record with { Project = new Project("TimaProject", 3) };

            Assert.RaisesAny(
                (e) => _sut.ListingChanged += e,
                (e) => _sut.ListingChanged -= e,
                () => _recordRepository.UpdateRecord(updatedRecord));     
        }

        [Fact]
        public void ListingChangedShouldNot_Invoke_WhenRepositoryUpdateInvalidRecord()
        {
            var isInvoked = false;

            Action<object?, EventArgs> checkFunc = (s, e) => isInvoked = true;

            var record = new Models.Record(DateTime.MinValue, DateOnly.MinValue, 5);
            
            _recordRepository.AddRecord(record);

            _sut.ListingChanged += checkFunc.Invoke;

            _recordRepository.UpdateRecord(record with { Title = "New Title"});

            _sut.ListingChanged -= checkFunc.Invoke;

            Assert.False(isInvoked);
        }


    }
}

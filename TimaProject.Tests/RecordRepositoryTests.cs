using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Repositories;
using TimaProject.Services.Factories;
using TimaProject.Stores;
using TimaProject.Models;
using TimaProject.ViewModels;
using Xunit;

namespace TimaProject.Tests
{
    public class RecordRepositoryTests
    {
        private readonly IRecordRepository _sut;
        private readonly IRecordRepository _sutForFiltration;

        private readonly DateOnly _filterDate;
        private readonly DateTime _toTime;
        private readonly DateTime _fromTime;

        private readonly Project[] _expectedProjects = new Project[]
        {
            new Project("Math", Guid.NewGuid()),
            new Project("C#", Guid.NewGuid())
        };

        private readonly Models.Record[] _expectedRecords;
        private readonly Models.Record[] _passedRecords;

        public RecordRepositoryTests()
        {
            _filterDate = DateOnly.Parse("02.11.2023");
            _toTime = DateTime.Parse("2.11.2023 20:00");
            _fromTime = DateTime.Parse("01.11.2023 20:00");


            _expectedRecords = new Models.Record[]
            {
                new Models.Record(_fromTime.AddHours(-5), _filterDate, 1)
                {
                    EndTime = _fromTime.AddHours(-3),
                    Project = _expectedProjects[0]
                },
                new Models.Record(_fromTime.AddHours(-5), _filterDate, 2)
                {
                    EndTime = _fromTime,
                    Project = _expectedProjects[1]
                }
            };

            _passedRecords = new Models.Record[]
            {
                new Models.Record(_fromTime, DateOnly.MinValue, 3)
                {
                    EndTime = _toTime.AddHours(2),
                    Project = new Project("Another Project", Guid.NewGuid())
                },
                new Models.Record(_fromTime, DateOnly.MinValue, 4)
                {
                    EndTime = _toTime.AddHours(3)
                }
            };

            _sut = new RecordRepository();
            _sutForFiltration = new RecordRepository();

            foreach(var record in _expectedRecords)
            {
                _sutForFiltration.AddRecord(record);
            }
            foreach (var record in _passedRecords)
            {
                _sutForFiltration.AddRecord(record);
            }
        }

        [Fact]
        public void GetNewIdShould_ReturnNewId()
        {
            for (int i = 0; i < 100; i++)
            {
                var id = _sut.GetNewId();
                Assert.Equal((ulong)i + 1, id);
            }
            Assert.Equal((ulong)101, _sut.GetNewId());
        }

        [Fact]
        public void AddRecordShould_AddNewRecord()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());

            _sut.AddRecord(record);

            Assert.Equal(record, _sut.GetRecords(new FilterListingArgs() { IsActive=null}).ToList()[0]);
        }

        [Fact]
        public void ContainsShould_ReturnTrue_WhenRecordContains()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            
            _sut.AddRecord(record);

            Assert.True(_sut.Contains(record));
        }

        [Fact]
        public void AddNewRecordShuold_ThrowException_WhenRecordContainsInRepository()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            
            _sut.AddRecord(record);

            Assert.Throws<Exception>(() => _sut.AddRecord(record));
        }

        [Fact]
        public void UpdateRecordShould_UpdateRecord()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            _sut.AddRecord(record);

            var updatedRecord = record with { Title = "New Title" };
            _sut.UpdateRecord(updatedRecord);

            var result = _sut.GetRecords(new FilterListingArgs() { IsActive = null }).ToList()[0];

            Assert.Equal(updatedRecord, result);
            Assert.NotEqual(record, result);
        }

        [Fact] public void DeleteRecordShould_DeleteRecord()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            _sut.AddRecord(record);

            Assert.True(_sut.DeleteRecord(record)); 

            Assert.Empty(_sut.GetRecords(new FilterListingArgs()).ToList());
        }

        [Fact]
        public void DeleteRecordShould_ReturnFalse_WhenRecordDontContainsInRepository()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            Assert.False(_sut.DeleteRecord(record));
        }

        [Fact]
        public void UpdateRecordShould_ThrowException_WhenRecordDontContainsInRepository()
        {
            var record = new Models.Record(_toTime, _filterDate, _sut.GetNewId());
            Assert.Throws<Exception>(()=>_sut.UpdateRecord(record));
        }

        [Fact]
        public void GetRecordsShould_ReturnRecordsErlierThanExpectedTime()
        {
            var filterArgs = new FilterListingArgs() { From = _fromTime};

            var result = _sutForFiltration.GetRecords(filterArgs);

            Assert.Contains(_expectedRecords[0], result);
            Assert.Contains(_expectedRecords[1], result);
            Assert.DoesNotContain(_passedRecords[0], result);
            Assert.DoesNotContain(_passedRecords[1], result);

        }

        [Fact]
        public void GetRecordsShould_ReturnRecordsArterExpectedTime()
        {

            var filterArgs = new FilterListingArgs() { To = _toTime };

            var result = _sutForFiltration.GetRecords(filterArgs);

            // terrible
            Assert.Contains(_passedRecords[0], result);
            Assert.Contains(_passedRecords[1], result);
            Assert.DoesNotContain(_expectedRecords[0], result);
            Assert.DoesNotContain(_expectedRecords[1], result);
        }

        [Fact]
        public void GetRecordsShould_ReturnRecordsWithExpectedProjects()
        {
            var filterArgs = new FilterListingArgs()
            {
                Projects = _expectedProjects.ToList()
            };

            var result = _sutForFiltration.GetRecords(filterArgs);
            Assert.Contains(_expectedRecords[0], result);
            Assert.Contains(_expectedRecords[1], result);
            Assert.DoesNotContain(_passedRecords[0], result);
            Assert.DoesNotContain(_passedRecords[1], result);
        }

        [Fact]
        public void GetRecordsSould_ReturnAllNotActiveRecords_WhenFilterArgsByDefault()
        {
            var activeRecord = new Models.Record(DateTime.Now,_filterDate, 5);

            _sutForFiltration.AddRecord(activeRecord);
            
            var result = _sutForFiltration.GetRecords(new FilterListingArgs());
            Assert.Contains(_expectedRecords[0], result);
            Assert.Contains(_expectedRecords[1], result);
            Assert.Contains(_passedRecords[0], result);
            Assert.Contains(_passedRecords[1], result);
            Assert.DoesNotContain(activeRecord, result);
        }

        [Fact]
        public void GetRecordsShould_ReturnCountedRecordsDescendingOrderedByEndTime_WhenCountIsNotNull()
        {
            var filterArgs = new FilterListingArgs()
            {
                Count = 2
            };
            
            var result = _sutForFiltration.GetRecords(filterArgs).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(_passedRecords[1], result[0]);
            Assert.Equal(_passedRecords[0], result[1]);

        }

        [Fact]
        public void GetRecordsShould_ReturnRecordsWithExpectedDate()
        {
            var filterArgs = new FilterListingArgs()
            {
                Date = _filterDate
            };

            var result = _sutForFiltration.GetRecords(filterArgs);

            Assert.Contains(_expectedRecords[0], result);
            Assert.Contains(_expectedRecords[1], result);
            Assert.DoesNotContain(_passedRecords[0], result);
            Assert.DoesNotContain(_passedRecords[1], result);
        }

        [Fact]
        public void GetRecordsShould_ReturnActiveRecords_ByFilterArgs()
        {
            var filterArgs = new FilterListingArgs()
            {
                IsActive = true
            };

            var activeRecord = new Models.Record(DateTime.Now, _filterDate, 5);

            _sutForFiltration.AddRecord(activeRecord);

            var result = _sutForFiltration.GetRecords(filterArgs);
            Assert.DoesNotContain(_expectedRecords[0], result);
            Assert.DoesNotContain(_expectedRecords[1], result);
            Assert.DoesNotContain(_passedRecords[0], result);
            Assert.DoesNotContain(_passedRecords[1], result);
            Assert.Contains(activeRecord, result);
        }

        //[Fact]
        //public void RepositoryChangedShould_InvokeWithCorrectArgs_WhenRecordAdded()
        //{
        //    var record = new Models.Record(_toTime, _filterDate, 1);

        //    var result = Assert.Raises<RepositoryChangedEventArgs>(
        //        e => _sut.RecordsChanged += e,
        //        e => _sut.RecordsChanged -= e,
        //        () => _sut.AddRecord(record));
        //    Assert.Equal(_sut, result.Sender);
        //    Assert.Equal(RepositoryChangedOperation.Add, result.Arguments.Operation);
        //    Assert.Equal(record, result.Arguments.Record);

        //}


        //[Fact]
        //public void RepositoryChangedShould_InvokeWithCorrectArgs_WhenRecordUpdated()
        //{
        //    var record = new Models.Record(_toTime, _filterDate, 1);

        //    _sut.AddRecord(record);

        //    var result = Assert.Raises<RepositoryChangedEventArgs>(
        //        e => _sut.RecordsChanged += e,
        //        e => _sut.RecordsChanged -= e,
        //        () => _sut.UpdateRecord(record));
        //    Assert.Equal(_sut, result.Sender);
        //    Assert.Equal(RepositoryChangedOperation.Remove, result.Arguments.Operation);
        //    Assert.Equal(record, result.Arguments.Record);
        //}

        //[Fact]
        //public void RepositoryChangedShould_InvokeWithCorrectArgs_WhenRecordDeleted()
        //{
        //    var record = new Models.Record(_toTime, _filterDate, 1);

        //    _sut.AddRecord(record);

        //    var result = Assert.Raises<RepositoryChangedEventArgs>(
        //        e => _sut.RecordsChanged += e,
        //        e => _sut.RecordsChanged -= e,
        //        () => _sut.DeleteRecord(record));
        //    Assert.Equal(_sut, result.Sender);
        //    Assert.Equal(RepositoryChangedOperation.Remove, result.Arguments.Operation);
        //    Assert.Equal(record, result.Arguments.Record);
        //}

        [Fact]
        public void RepositoryChangedShouldNot_Invoke_WhenDeleteRecordIsFalse()
        {
            bool isEventRaised = false;

            var checkFunc = new Action<object?, RepositoryChangedEventArgs>((s, e)=> isEventRaised = true);

            var record = new Models.Record(_toTime, _filterDate, 1);
            _sut.RecordsChanged += checkFunc.Invoke;
            Assert.False(_sut.DeleteRecord(record));
            Assert.False(isEventRaised);
           
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Repositories;
using TimaProject.Stores;
using TimaProject.Models;
using TimaProject.ViewModels;
using Xunit;
using TimaProject.Exceptions;

namespace TimaProject.Tests
{
    public class RecordRepositoryShould
    {
        private readonly RecordRepository _sut;

        private static readonly Models.Record _recordInRepo = new Models.Record(DateTime.MinValue, DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid());

        public RecordRepositoryShould()
        {
            _sut = new RecordRepository();
            _sut.AddItem(_recordInRepo);
        }


        [Fact]
        public void AddRecord()
        {
            var newRecord = new Models.Record(DateTime.MinValue, DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid());

            _sut.AddItem(newRecord);

            Assert.Contains(newRecord, _sut.GetItems(x=> true));
        }

        [Fact]
        public void ThrowNotUniqueItemException_WhenAddingRecordThatsContiansInRepository()
        {

            var exception = Assert.Throws<AddingNotUniqueItemException>(()=> _sut.AddItem(_recordInRepo));
            Assert.Equal("Record already contains in repository.", exception.Message);
        }

        [Theory, MemberData(nameof(ContainsTestData))]
        public void ContainsReturnCorrectValue_OnTestData(Models.Record record, bool expected)
        {
            Assert.Equal(expected, _sut.Contains(record));
        }

        public static IEnumerable<object[]> ContainsTestData()
        {
            yield return new object[] { _recordInRepo, true};
            yield return new object[] { _recordInRepo with { StartTime = DateTime.Now}, true};
            yield return new object[] { new Models.Record(DateTime.Now, DateOnly.FromDayNumber(234), Guid.NewGuid()), false};
        }


        [Fact]
        public void UpdateRecord()
        {
            var updatedRecord = _recordInRepo with { Title = "New Title" };
            _sut.UpdateItem(updatedRecord);
           
            Assert.Contains(updatedRecord, _sut.GetItems((x)=> true));
        }

        [Fact]
        public void ThrowNotFoundException_WhenUpdateNonContainingRecord()
        {
            var record = new Models.Record(DateTime.MinValue, DateOnly.FromDayNumber(1), Guid.NewGuid());

            var exception = Assert.Throws<NotFoundException<Models.Record>>(()=> _sut.UpdateItem(record));
            Assert.Equal(record, exception.Item);
            Assert.Equal("Record not found in the repository", exception.Message);
        }

        [Theory, MemberData(nameof(RemoveItemTestData))]
        public void RemoveItem_OnTestData(Models.Record record, bool expected)
        {
            Assert.Equal(expected, _sut.RemoveItem(record));
        }

        public static IEnumerable<object[]> RemoveItemTestData()
        {
            yield return new object[] { _recordInRepo, true };
            yield return new object[] { new Models.Record(DateTime.MinValue, DateOnly.FromDayNumber(1), Guid.NewGuid()), false };
        }




        [Fact]
        public void RaisedRepositoryChanged_AfterAddingNewItem()
        {
            var record = new Models.Record(DateTime.Now, DateOnly.FromDayNumber(0), Guid.NewGuid());

            var result = Assert.Raises<RepositoryChangedEventArgs<Models.Record>>(
                e => _sut.RepositoryChanged += e,
                e => _sut.RepositoryChanged -= e,
                () => _sut.AddItem(record));
            Assert.Equal(_sut, result.Sender);
            Assert.Equal(RepositoryChangedOperation.Add, result.Arguments.Operation);
            Assert.Equal(record, result.Arguments.Item);

        }

        [Fact]
        public void RaisedRepositoryChanged_AfterUpdatingItem()
        {
            var result = Assert.Raises<RepositoryChangedEventArgs<Models.Record>>(
                e => _sut.RepositoryChanged += e,
                e => _sut.RepositoryChanged -= e,
                () => _sut.UpdateItem(_recordInRepo));
            Assert.Equal(_sut, result.Sender);
            Assert.Equal(RepositoryChangedOperation.Update, result.Arguments.Operation);
            Assert.Equal(_recordInRepo, result.Arguments.Item);

        }

        [Fact]
        public void RaisedRepositoryChanged_AfterRemovingItem()
        {
            var result = Assert.Raises<RepositoryChangedEventArgs<Models.Record>>(
                e => _sut.RepositoryChanged += e,
                e => _sut.RepositoryChanged -= e,
                () => _sut.RemoveItem(_recordInRepo));
            Assert.Equal(_sut, result.Sender);
            Assert.Equal(RepositoryChangedOperation.Remove, result.Arguments.Operation);
            Assert.Equal(_recordInRepo, result.Arguments.Item);

        }
    }
}

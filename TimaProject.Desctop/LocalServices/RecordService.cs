using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Domain.Models;
using TimaProject.LocalController;

namespace TimaProject.Desctop.LocalServices
{
    internal class RecordService : IRecordService
    {
        private readonly RecordController _controller;

        public RecordService()
        {
            _controller = new RecordController();
            _controller.RecordChanged += OnRecordChanged;
        }

        private void OnRecordChanged(object? sender, RepositoryChangedEventArgs<RecordContainer> e)
        {
            Operation op = e.Operation switch
            {
                RepositoryChangedOperation.Add => Operation.Add,
                RepositoryChangedOperation.Update => Operation.Update,
                RepositoryChangedOperation.Remove => Operation.Delete
            };

            RecordChanged?.Invoke(this, new EntityChangedEventArgs<RecordDto>(op, ToDto(e.Item)));
        }

        public event EventHandler<EntityChangedEventArgs<RecordDto>>? RecordChanged;

        public void AddRecord(RecordDto record)
        {
            _controller.AddRecord(FromDtoTo(record));
        }

        public bool DeleteRecord(Guid recordId)
        {
            return _controller.DeleteRecord(recordId);
        }

        public IEnumerable<RecordDto> GetRecords()
        {
            var list = new List<RecordDto>();
            foreach(var item in _controller.GetRecords())
            {
                list.Add(ToDto(item));
            }
            return list;
        }

        public void UpdateRecord(RecordDto record)
        {
            _controller.UpdateRecord(FromDtoTo(record));
        }

        private RecordContainer FromDtoTo(RecordDto record)
        {
            return new RecordContainer(record.StartTime, record.Title, record.RecordId)
            {
                IsActive = record.IsActive,
                ProjectId = record.ProjectId,
                ProjectName = record.ProjectName,
                Time = record.Time,
                Date = record.Date,
                EndTime = record.EndTime
            };
        }

        private RecordDto ToDto(RecordContainer record)
        {
            return new RecordDto(record.StartTime, record.Title, record.RecordId)
            {
                IsActive = record.IsActive,
                ProjectId = record.ProjectId,
                ProjectName = record.ProjectName,
                Time = record.Time,
                Date = record.Date,
                EndTime = record.EndTime
            };
        }
    }
}

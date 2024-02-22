using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    public enum Operation
    {
        Add,
        Update,
        Delete
    }

    public class EntityChangedEventArgs<TEntity> : EventArgs
    {
        public EntityChangedEventArgs(Operation operation, TEntity value)
        {
            Operation = operation;
            Value = value;
        }

        public Operation Operation { get; }
        public TEntity Value { get; }
    }

    public interface IRecordService
    {
        void AddRecord(RecordDto record);

        void UpdateRecord(RecordDto record);

        bool DeleteRecord(Guid recordId);

        IEnumerable<RecordDto> GetRecords();

        event EventHandler<EntityChangedEventArgs<RecordDto>> RecordChanged;
    }
}

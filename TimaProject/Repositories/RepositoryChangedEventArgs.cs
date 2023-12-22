using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class RepositoryChangedEventArgs<T> : EventArgs
    {
        public RepositoryChangedOperation Operation { get; }
        public T Item { get; }

        public RepositoryChangedEventArgs(T item, RepositoryChangedOperation operation)
        {
            Operation = operation;
            Item = item;
        }
    }

    public class RepositoryChangedEventArgs : EventArgs
    {
        public RepositoryChangedOperation Operation { get; }
        public Record Record { get; }

        public RepositoryChangedEventArgs(RepositoryChangedOperation operation, Record record)
        {
            Operation = operation;
            Record = record;
        }
    }
}

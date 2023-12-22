using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Exceptions
{
    public class NotFoundException<T> : Exception
    {
        public T Item { get; }

        public NotFoundException(T item)
        {
            Item = item;
        }

        public NotFoundException(T item, string? message) : base(message)
        {
            Item = item;
        }

        public NotFoundException(T item, string? message, Exception? innerException) : base(message, innerException)
        {
            Item = item;
        }

        protected NotFoundException(T item, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Item = item;
        }
    }
}

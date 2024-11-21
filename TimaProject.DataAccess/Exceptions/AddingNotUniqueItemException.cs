using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.DataAccess.Exceptions
{
    public class AddingNotUniqueItemException : Exception
    {
        public AddingNotUniqueItemException()
        {
        }

        public AddingNotUniqueItemException(string? message) : base(message)
        {
        }

        public AddingNotUniqueItemException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AddingNotUniqueItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

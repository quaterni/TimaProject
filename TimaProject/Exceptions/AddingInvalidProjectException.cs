using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Exceptions
{
    public class AddingInvalidProjectException : Exception
    {
        public AddingInvalidProjectException()
        {
        }

        public AddingInvalidProjectException(string? message) : base(message)
        {
        }

        public AddingInvalidProjectException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AddingInvalidProjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

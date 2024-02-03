using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.DataAccess.Exceptions
{
    public class ChangeEmptyProjectException : Exception
    {
        public ChangeEmptyProjectException()
        {
        }

        public ChangeEmptyProjectException(string? message) : base(message)
        {
        }

        public ChangeEmptyProjectException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ChangeEmptyProjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

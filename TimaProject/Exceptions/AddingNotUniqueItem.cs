using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Exceptions
{
    public class AddingNotUniqueItem : Exception
    {
        public AddingNotUniqueItem()
        {
        }

        public AddingNotUniqueItem(string? message) : base(message)
        {
        }

        public AddingNotUniqueItem(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AddingNotUniqueItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

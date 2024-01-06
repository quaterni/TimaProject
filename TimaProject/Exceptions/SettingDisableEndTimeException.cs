using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Exceptions
{
    public class SettingDisableEndTimeException : Exception
    {
        public SettingDisableEndTimeException()
        {
        }

        public SettingDisableEndTimeException(string? message) : base(message)
        {
        }

        public SettingDisableEndTimeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SettingDisableEndTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

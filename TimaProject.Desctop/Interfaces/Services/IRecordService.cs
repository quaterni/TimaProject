using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    internal interface IRecordService
    {
        void AddRecord(RecordDTO record);
    }
}

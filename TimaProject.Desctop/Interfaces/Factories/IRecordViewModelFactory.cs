using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.Interfaces.Factories
{
    public interface IRecordViewModelFactory
    {
        IRecordViewModel Create(RecordDto record);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    internal interface IRecordViewModelBase
    {
        string StartTime { get; set; }
        string EndTime { get; set; }
        string Time { get; set; }
        string Date { get; set; }
        string ProjectName { get; }
        string ProjectId { get; }

    }
}

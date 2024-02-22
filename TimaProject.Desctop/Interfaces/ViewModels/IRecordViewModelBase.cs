using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IRecordViewModelBase : ITimeEditable, IProjectEditable
    {
        string StartTime { get; set; }

        string EndTime { get; set; }

        string Time { get; }

        string Date { get; set; }

        string Title { get; set; }

        string ProjectName { get; }

        Guid ProjectId { get; }

    }
}

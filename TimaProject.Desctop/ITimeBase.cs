using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop
{
    internal interface ITimeBase
    {
        string StartTime { get; set; }
        string EndTime { get; set; }
        string Time { get; set; }
        string Date { get; set; }
    }
}

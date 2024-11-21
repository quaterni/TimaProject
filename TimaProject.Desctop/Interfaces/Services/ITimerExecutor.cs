using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Interfaces.Services
{
    internal interface ITimerExecutor : IDisposable
    {
        void Start();
        DateTime StartTime { get; set; }
        DateTime CurrentTime();
        event EventHandler<TimeSpan> Tick;
    }
}

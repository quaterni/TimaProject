using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.Interfaces.Factories
{
    internal interface ITimerExecutorFactory
    {
        ITimerExecutor Create();
    }
}

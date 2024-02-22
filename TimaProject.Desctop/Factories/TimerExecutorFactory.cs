using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Services;

namespace TimaProject.Desctop.Factories
{
    internal class TimerExecutorFactory : ITimerExecutorFactory
    {
        public ITimerExecutor Create()
        {
            return new TimerExecutor();
        }
    }
}

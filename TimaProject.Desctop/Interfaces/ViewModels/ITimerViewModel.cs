using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public enum TimerState
    {
        NotRunning,
        Running
    }

    internal interface ITimerViewModel : IRecordViewModelBase
    {
        ICommand TimerCommand { get; }
        TimerState State { get; }
    }
}

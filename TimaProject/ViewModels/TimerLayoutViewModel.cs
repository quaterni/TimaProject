using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels
{
    internal class TimerLayoutViewModel : ViewModelBase
    {
        public ViewModelBase TimerViewModel { get; }

        public ViewModelBase CurrentViewModel { get; }

        public TimerLayoutViewModel(ViewModelBase timerViewModel, ViewModelBase currentViewModel)
        {

            TimerViewModel = timerViewModel;
            CurrentViewModel = currentViewModel;
        }
    }
}

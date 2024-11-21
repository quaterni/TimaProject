using MvvmTools.Base;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Containers
{
    internal class TimerLayoutViewModel : ViewModelBase
    {
        public ITimerViewModel TimerViewModel { get; }

        public IListingRecordViewModel CurrentViewModel { get; }

        public TimerLayoutViewModel(ITimerViewModel timerViewModel, IListingRecordViewModel currentViewModel)
        {

            TimerViewModel = timerViewModel;
            CurrentViewModel = currentViewModel;
        }
    }
}

using MvvmTools.Base;

namespace TimaProject.Desctop.ViewModels.Containers
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

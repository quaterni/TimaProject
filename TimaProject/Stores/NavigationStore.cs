using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;

namespace TimaProject.Stores
{
    internal class NavigationStore : INavigationStore<ViewModelBase>
    {
        private ViewModelBase? _currentViewModel;

        public ViewModelBase? CurrentViewModel
        { 
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel?.Dispose();
                    _currentViewModel = value;
                    OnCurrentViewModelChanged();
                }
            }
        }

        public event EventHandler? CurrentViewModelChanged;

        public void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}

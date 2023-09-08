using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;

namespace TimaProject.Stores
{
    internal interface INavigationStore<TViewModel> where TViewModel : ViewModelBase
    {
        public TViewModel? CurrentViewModel { get; set; }

        public event EventHandler? CurrentViewModelChanged;
    }
}

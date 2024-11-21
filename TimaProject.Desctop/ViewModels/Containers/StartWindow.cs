using MvvmTools.Base;
using MvvmTools.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.ViewModels.Containers
{
    internal class StartWindow
    {
        public object? CurrentViewModel { get; }
        public object? CurrentModalViewModel { get; }

        public StartWindow(object? currentModalViewModel, object? currentViewModel)
        {
            CurrentModalViewModel = currentModalViewModel;
            CurrentViewModel = currentViewModel;
        }
    }
}

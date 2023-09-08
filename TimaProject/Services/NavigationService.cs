using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Stores;
using TimaProject.ViewModels;

namespace TimaProject.Services
{
    internal class NavigationService<TStoreViewModel, TViewModel> : INavigationService
        where TStoreViewModel : ViewModelBase
        where TViewModel : ViewModelBase
    {
        private readonly INavigationStore<TStoreViewModel> _navigationStore;

        private readonly Func<Type, TStoreViewModel> _viewModelFactory;

        public NavigationService(INavigationStore<TStoreViewModel> navigationStore, Func<Type, TStoreViewModel> viewModelFactory)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }


        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _viewModelFactory(typeof(TViewModel));
        }
    }
}

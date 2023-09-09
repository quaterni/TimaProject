using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Stores;
using TimaProject.ViewModels;

namespace TimaProject.Services
{

    /// <summary>
    /// Navigate view model in INavigationStore attached type of TViewModel as a factory parameter
    /// </summary>
    /// <typeparam name="TStoreViewModel">Type of INavigationStore.CurrentViewModel</typeparam>
    /// <typeparam name="TViewModel">Type of new viewmodel</typeparam>
    internal class NavigationService<TStoreViewModel, TViewModel> : INavigationService
        where TStoreViewModel : ViewModelBase
        where TViewModel : ViewModelBase
    {
        private readonly INavigationStore<TStoreViewModel> _navigationStore;

        private readonly Func<Type, TStoreViewModel> _viewModelFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationStore">NavigationStore that is set CurrentViewModel by factory</param>
        /// <param name="viewModelFactory">factory of TStoreViewModel which takes type TViewModel as argument</param>
        public NavigationService(INavigationStore<TStoreViewModel> navigationStore, Func<Type, TStoreViewModel> viewModelFactory)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Set new TStoreViewModel at INavigationStore with injected viewmodel factory that takes type of TViewModel as a parameter
        /// </summary>
        /// <param name="parameter">not used, interface implementation</param>
        public void Navigate(object? parameter)
        {
            _navigationStore.CurrentViewModel = _viewModelFactory(typeof(TViewModel));
        }
    }
}

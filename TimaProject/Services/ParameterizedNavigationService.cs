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
    /// Navigate view model in INavigationStore with parameter
    /// </summary>
    /// <typeparam name="TStoreViewModel">Type of INavigationStore.CurrentViewModel</typeparam>
    /// <typeparam name="TParameterValue">Parameter type of factory</typeparam>
    internal class ParameterizedNavigationService<TStoreViewModel, TParameterValue> : INavigationService
        where TStoreViewModel : ViewModelBase
    {
        private readonly INavigationStore<TStoreViewModel> _navigationStore;
        private readonly Func<TParameterValue, TStoreViewModel> _viewModelFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationStore">NavigationStore that is set CurrentViewModel by factory</param>
        /// <param name="viewModelFactory">factory of TStoreViewModel which takes TParameterValue as argument</param>
        public ParameterizedNavigationService(INavigationStore<TStoreViewModel> navigationStore, Func<TParameterValue, TStoreViewModel> viewModelFactory)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Set new TStoreViewModel at INavigationStore with injected viewmodel factory that takes parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentException">Throw exception if parameter is not TParameterValue</exception>
        public void Navigate(object? parameter)
        {
            if(parameter is TParameterValue parameterValue)
            {
                _navigationStore.CurrentViewModel = _viewModelFactory(parameterValue);
            }
            throw new ArgumentException("parameter must be TParameterValue");
        }
    }
}

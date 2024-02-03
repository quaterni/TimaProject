using MvvmTools.Base;
using MvvmTools.Navigation.Stores;
using System;

namespace TimaProject.Desctop.ViewModels.Containers
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly INavigationStore<ViewModelBase> _navigationStore;

        private readonly INavigationStore<ModalViewModel> _modalStore;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase? CurrentModalViewModel => _modalStore.CurrentViewModel;

        public MainViewModel(INavigationStore<ViewModelBase> navigationStore, INavigationStore<ModalViewModel> modalStore)
        {
            _navigationStore = navigationStore;
            _modalStore = modalStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalStore.CurrentViewModelChanged += OnCurrentModalChanged;

        }

        private void OnCurrentModalChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
        }

        private void OnCurrentViewModelChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _modalStore.CurrentViewModelChanged -= OnCurrentModalChanged;
            base.Dispose();
        }
    }
}

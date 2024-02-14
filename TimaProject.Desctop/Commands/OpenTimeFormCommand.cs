using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Factories;

namespace TimaProject.Desctop.Commands
{
    public class OpenTimeFormCommand : CommandBase
    {
        private readonly IRecordViewModel _source;
        private readonly TimeFormViewModelFactory _factory;
        private readonly INavigationService _openTimeFormNavigationService;

        private readonly bool _isEndTimeEnabled;

        public OpenTimeFormCommand(
            IRecordViewModel source,
            TimeFormViewModelFactory factory,
            INavigationService openTimeFormNavigationService,
            bool isEndTimeEnabled = true)
        {
            _source = source;
            _factory = factory;
            _openTimeFormNavigationService = openTimeFormNavigationService;
            _isEndTimeEnabled = isEndTimeEnabled;
        }

        public override void Execute(object? parameter)
        {
            _openTimeFormNavigationService.Navigate(_factory.Create(_source, isEndTimeEnabled: _isEndTimeEnabled));
        }
    }
}

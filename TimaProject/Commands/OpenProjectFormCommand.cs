using MvvmTools.Base;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;
using TimaProject.ViewModels.Factories;

namespace TimaProject.Commands
{
    public class OpenProjectFormCommand :CommandBase
    {
        private readonly IRecordViewModel _source;
        private readonly ProjectFormViewModelFactory _factory;
        private readonly INavigationService _openProjectFormNavigationService;

        public OpenProjectFormCommand(
            IRecordViewModel recordViewModelWithEdit,
            ProjectFormViewModelFactory factory,
            INavigationService openProjectFormNavigationService)
        {
            _factory = factory;
            _source = recordViewModelWithEdit;
            this._openProjectFormNavigationService = openProjectFormNavigationService;
        }

        public override void Execute(object? parameter)
        {
            _openProjectFormNavigationService.Navigate(_factory.Create(_source));
        }
    }
}

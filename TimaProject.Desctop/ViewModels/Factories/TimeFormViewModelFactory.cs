using FluentValidation;
using MvvmTools.Navigation.Services;
using MvvmTools.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.ViewModels.Factories
{
    public class TimeFormViewModelFactory
    {
        private readonly AbstractValidator<ITimeBase> _validator;
        private readonly INavigationService _closeNavigationService;

        public TimeFormViewModelFactory(AbstractValidator<ITimeBase> validator, INavigationService closeNavigationService)
        {
            _validator = validator;
            _closeNavigationService = closeNavigationService;
        }

        public TimeFormViewModel Create(IRecordViewModel recordViewModel, bool isEndTimeEnabled = true)
        {
            return new TimeFormViewModel(
                recordViewModel,
                _validator,
                _closeNavigationService,
                isEndTimeEnabled: isEndTimeEnabled);
        }
    }
}

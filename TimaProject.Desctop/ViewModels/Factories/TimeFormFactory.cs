using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class TimeFormFactory : ITimeFormViewModelFactory
    {
        private readonly ITimeService _timeService;
        private readonly bool _canEndTimeEdit;

        public TimeFormFactory(ITimeService timeService, bool canEndTimeEdit)
        {
            _timeService = timeService;
            _canEndTimeEdit = canEndTimeEdit;
        }


        public ITimeFormViewModel Create(TimeDTO timeDTO)
        {
            return new TimeFormViewModel(timeDTO, _timeService, _canEndTimeEdit);
        }
    }
}

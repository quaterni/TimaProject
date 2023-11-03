using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Validators;

namespace TimaProject.ViewModels.Factories
{
    public class EditableRecordViewModelFactory
    {
        private readonly IRecordRepository recordRepository;
        private readonly Func<INavigationService> timeFromNavigationServiceFactory;
        private readonly Func<TimeFormViewModel> timeFormFactory;
        private readonly RecordValidator validator;

        public EditableRecordViewModelFactory(
            IRecordRepository recordRepository, 
            Func<INavigationService> timeFromNavigationServiceFactory,
            Func<TimeFormViewModel> timeFormFactory,
            RecordValidator validator)
        {
            this.recordRepository = recordRepository;
            this.timeFromNavigationServiceFactory = timeFromNavigationServiceFactory;
            this.timeFormFactory = timeFormFactory;
            this.validator = validator;
        }

        public EditableRecordViewModel Create(Record record)
        {
            return new EditableRecordViewModel(
                record,
                recordRepository,
                timeFromNavigationServiceFactory(),
                timeFormFactory,
                validator);
        }


    }
}

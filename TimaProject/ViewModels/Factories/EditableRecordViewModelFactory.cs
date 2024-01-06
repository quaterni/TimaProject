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
        private readonly INavigationService timeFromNavigationService;
        private readonly INavigationService projectFromNavigationService;
        private readonly TimeFormViewModelFactory timeFormFactory;
        private readonly ProjectFormViewModelFactory projectFormViewModelFactory;
        private readonly TimeValidator validator;

        public EditableRecordViewModelFactory(
            IRecordRepository recordRepository, 
            INavigationService timeFromNavigationService,
            INavigationService projectFromNavigationService,
            TimeFormViewModelFactory timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            TimeValidator validator)
        {
            this.recordRepository = recordRepository;
            this.timeFromNavigationService = timeFromNavigationService;
            this.projectFromNavigationService = projectFromNavigationService;
            this.timeFormFactory = timeFormFactory;
            this.projectFormViewModelFactory = projectFormViewModelFactory;
            this.validator = validator;
        }

        public EditableRecordViewModel Create(Record record)
        {
            return new EditableRecordViewModel(
                record,
                recordRepository,
                timeFromNavigationService,
                projectFromNavigationService,
                timeFormFactory,
                projectFormViewModelFactory,
                validator);
        }


    }
}

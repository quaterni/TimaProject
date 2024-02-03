using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels.Validators;

namespace TimaProject.Desctop.ViewModels.Factories
{
    public class EditableRecordViewModelFactory
    {
        private readonly IRecordRepository recordRepository;
        private readonly INavigationService timeFromNavigationService;
        private readonly INavigationService projectFromNavigationService;
        private readonly TimeFormViewModelFactory timeFormFactory;
        private readonly ProjectFormViewModelFactory projectFormViewModelFactory;
        private readonly TimeValidator validator;
        private readonly NoteFormViewModelFactory _noteFormViewModelFactory;
        private readonly ListingNoteViewModelFactory _listingNoteViewModelFactory;

        public EditableRecordViewModelFactory(
            IRecordRepository recordRepository,
            INavigationService timeFromNavigationService,
            INavigationService projectFromNavigationService,
            TimeFormViewModelFactory timeFormFactory,
            ProjectFormViewModelFactory projectFormViewModelFactory,
            TimeValidator validator,
            NoteFormViewModelFactory noteFormViewModelFactory,
            ListingNoteViewModelFactory listingNoteViewModelFactory)
        {
            this.recordRepository = recordRepository;
            this.timeFromNavigationService = timeFromNavigationService;
            this.projectFromNavigationService = projectFromNavigationService;
            this.timeFormFactory = timeFormFactory;
            this.projectFormViewModelFactory = projectFormViewModelFactory;
            this.validator = validator;
            _noteFormViewModelFactory = noteFormViewModelFactory;
            _listingNoteViewModelFactory = listingNoteViewModelFactory;
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
                validator,
                _noteFormViewModelFactory,
                _listingNoteViewModelFactory);
        }


    }
}

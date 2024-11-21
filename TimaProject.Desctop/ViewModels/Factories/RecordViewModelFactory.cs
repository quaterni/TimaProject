using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class RecordViewModelFactory : IRecordViewModelFactory
    {
        private readonly IAddNoteFormFactory _addNoteViewModelFactory;
        private readonly IListingNoteViewModelFactory _lisitngNoteViewModelFactory;
        private readonly ITimeFormViewModelFactory _timeFormViewModelFactory;
        private readonly IProjectFormViewModelFactory _projectFormViewModelFactory;
        private readonly IRecordService _recordService;

        public RecordViewModelFactory
            (IAddNoteFormFactory addNoteViewModelFactory,
            IListingNoteViewModelFactory lisitngNoteViewModelFactory,
            ITimeFormViewModelFactory timeFormViewModelFactory,
            IProjectFormViewModelFactory projectFormViewModelFactory,
            IRecordService recordService)
        {
            _addNoteViewModelFactory = addNoteViewModelFactory;
            _lisitngNoteViewModelFactory = lisitngNoteViewModelFactory;
            _timeFormViewModelFactory = timeFormViewModelFactory;
            _projectFormViewModelFactory = projectFormViewModelFactory;
            _recordService = recordService;
        }

        public IRecordViewModel Create(RecordDto record)
        {
            return new RecordViewModel(
                record,
                _timeFormViewModelFactory,
                _projectFormViewModelFactory,
                _addNoteViewModelFactory,
                _lisitngNoteViewModelFactory,
                _recordService);
        }
    }
}

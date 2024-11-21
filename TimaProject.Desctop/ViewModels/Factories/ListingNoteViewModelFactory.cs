using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class ListingNoteViewModelFactory : IListingNoteViewModelFactory
    {
        private readonly INoteService _noteService;
        private readonly INoteViewModelFactory _noteViewModelFactory;

        public ListingNoteViewModelFactory(INoteService noteService, INoteViewModelFactory noteViewModelFactory)
        {
            _noteService = noteService;
            _noteViewModelFactory = noteViewModelFactory;
        }

        public IListingNoteViewModel Create(Guid recordId)
        {
            return new ListingNoteViewModel(
                recordId,
                _noteService,
                _noteViewModelFactory);
        }
    }
}

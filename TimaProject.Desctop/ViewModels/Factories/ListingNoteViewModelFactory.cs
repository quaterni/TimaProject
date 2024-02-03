using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.Desctop.ViewModels.Factories
{
    public class ListingNoteViewModelFactory
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly EditableNoteViewModelFactory _editableNoteFactory;

        public ListingNoteViewModelFactory(IRepository<Note> noteRepository, EditableNoteViewModelFactory editableNoteFactory)
        {
            _noteRepository = noteRepository;
            _editableNoteFactory = editableNoteFactory;
        }

        public ListingNoteViewModel Create(Func<Note, bool> filterPredicate)
        {
            return new ListingNoteViewModel(filterPredicate,
                _noteRepository,
                _editableNoteFactory);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.Desctop.ViewModels.Factories
{
    public class EditableNoteViewModelFactory
    {
        private readonly IRepository<Note> _noteRepository;

        public EditableNoteViewModelFactory(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public EditableNoteViewModel Create(Note note)
        {
            return new EditableNoteViewModel(_noteRepository, note);
        }
    }
}

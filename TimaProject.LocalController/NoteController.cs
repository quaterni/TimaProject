using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;
using TimaProject.Domain.Models;

namespace TimaProject.LocalController
{
    public class NoteController
    {
        private readonly NoteRepository _repository;

        public NoteController()
        {
            _repository = Store.GetNoteRepository();
        }

        public void Add(Note note)
        {
            _repository.AddItem(note);
        }

        public bool Delete(Guid noteId)
        {
            return _repository.DeleteNote(noteId);
        }

        public IEnumerable<Note> GetNotes(Guid recordId)
        {
            return _repository.GetItems(recordId);
        }

        public void Update(Note note)
        {
            _repository.UpdateItem(note);
        }
    }
}

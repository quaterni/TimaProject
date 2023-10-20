using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.Tests
{
    internal class MockNoteRepository : INoteRepository
    {
        private readonly List<Note> _notes = new();

        public List<Note> Notes => _notes;

        public event EventHandler? NotesChanged;

        public void AddNote(Note note)
        {
            _notes.Add(note);
        }

        public IEnumerable<Note> GetAllNotes(Func<Note, bool>? wherePredicate = null)
        {
            throw new NotImplementedException();
        }

        public int GetNewId()
        {
            return 1;
        }

        public void UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}

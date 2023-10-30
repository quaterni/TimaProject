using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.Tests
{
    internal class MockRecordRepository : IRecordRepository
    {
        private readonly List<Record> _notes = new();

        public List<Record> Notes => _notes;

        public event EventHandler? NotesChanged;

        public void AddNote(Record note)
        {
            _notes.Add(note);
        }

        public IEnumerable<Record> GetAllNotes(Func<Record, bool>? wherePredicate = null)
        {
            throw new NotImplementedException();
        }

        public int GetNewId()
        {
            return 1;
        }

        public void UpdateNote(Record note)
        {
            _notes[0] = note;
        }
    }
}

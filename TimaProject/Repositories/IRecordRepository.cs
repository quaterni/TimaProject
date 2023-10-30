using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public interface IRecordRepository
    {
        public int GetNewId();

        public void AddNote(Record note);

        public void UpdateNote(Record note);

        public IEnumerable<Record> GetAllNotes(Func<Record, bool>? wherePredicate = null);

        public event EventHandler? NotesChanged;
    }
}

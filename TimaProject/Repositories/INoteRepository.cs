using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    internal interface INoteRepository
    {
        public int GetNewId();

        public void AddNote(TimaNote note);

        public void UpdateNote(TimaNote note);

        public IEnumerable<TimaNote> GetAllNotes(Func<TimaNote, bool>? wherePredicate = null);

        public event EventHandler? NotesChanged;
    }
}

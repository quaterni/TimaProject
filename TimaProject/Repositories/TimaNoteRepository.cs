using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    class TimaNoteRepository
    {
        public List<TimaNote> _notes;

        public TimaNoteRepository()
        {
            _notes = new();

            _notes.Add(new TimaNote(new DateTime(2023, 9, 14, 15, 45, 45), 1)
            {
                StoppingTime= new DateTime(2023, 9, 14, 18, 28, 19),
                Title = "Занимался программированием (нет)",
                Date = new DateOnly(2023, 9, 14), 
            });

            _notes.Add(new TimaNote(new DateTime(2023, 9, 12, 12, 45, 45), 1)
            {
                StoppingTime = new DateTime(2023, 9, 12, 12, 55, 19),
                Title = "Зарядка",
                Date = new DateOnly(2023, 9, 12),
            });

        }

        public int GetNewId()
        {
            return _notes.Count + 1;
        }

        public void AddNote(TimaNote note)
        {
            if(_notes.Find(t => t.Id == note.Id) is not null)
            {
                throw new Exception("Note with current id exist");
            }
            _notes.Add(note);
            OnNotesChanged();
        }

        public void UpdateNote(TimaNote note)
        {
            var n = _notes.Find(t => t.Id == note.Id);
            if(n is null)
            {
                throw new Exception("Note with current id not exist");
            }
            _notes.Remove(n);
            _notes.Add(note);
            OnNotesChanged();
        }

        internal IEnumerable<TimaNote> GetAllNotes()
        {
            return _notes.Where(t=> !t.IsActive);
        }

        public event EventHandler? NotesChanged;

        private void OnNotesChanged()
        {
            NotesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

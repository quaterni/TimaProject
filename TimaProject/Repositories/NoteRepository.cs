using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Exceptions;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class NoteRepository : IRepository<Note>
    {
        private readonly List<Note> _notes;

        public NoteRepository()
        {
            _notes = new List<Note>();
        }

        public event EventHandler<RepositoryChangedEventArgs<Note>>? RepositoryChanged;

        public void AddItem(Note item)
        {
            if (Contains(item.Id))
            {
                throw new AddingNotUniqueItemException();
            }
            _notes.Add(item);
            OnRepositoryChanged(RepositoryChangedOperation.Add, item);
        }

        public bool Contains(Note item)
        {
            return Contains(item.Id);
        }


        public bool Contains(Guid id)
        {
            return _notes.Any(note => note.Id.Equals(id));
        }

        public IEnumerable<Note> GetItems(Func<Note, bool> filterPredicate)
        {
            return _notes.Where(filterPredicate);
        }

        public bool RemoveItem(Note item)
        {
            var result = _notes.Remove(item);
            if (result)
            {
                OnRepositoryChanged(RepositoryChangedOperation.Remove, item);
            }
            return result;
        }

        public void UpdateItem(Note item)
        {
            var oldItem = _notes.Find(note => note.Id.Equals(item.Id));
            if (oldItem == null)
            {
                throw new NotFoundException<Note>(item);
            }
            _notes[_notes.IndexOf(oldItem)] = item;
            OnRepositoryChanged(RepositoryChangedOperation.Update, item);
        }

        protected void OnRepositoryChanged(RepositoryChangedOperation operation, Note note)
        {
            RepositoryChanged?.Invoke(this, new RepositoryChangedEventArgs<Note>(note, operation));
        }
    }
}

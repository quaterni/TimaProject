using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Domain.Models;
using TimaProject.LocalController;

namespace TimaProject.Desctop.LocalServices
{
    internal class NoteService : INoteService
    {
        private readonly NoteController _noteController;

        public NoteService()
        {
            _noteController = new NoteController();
        }

        public void Add(NoteDTO note)
        {
            _noteController.Add(FromTo(note));
        }

        public bool Delete(Guid noteId)
        {
            return _noteController.Delete(noteId);
        }

        public IEnumerable<NoteDTO> GetNotes(Guid recordId)
        {
            return _noteController.GetNotes(recordId).Select(ToFrom);
        }

        public void Update(NoteDTO note)
        {
            _noteController.Update(FromTo(note));
        }

        private Note FromTo(NoteDTO note)
        {
            return new Note(note.Text, note.Id, note.RecordId, note.Created)
            {
                LastEdited = note.LastEdit
            };
        }

        private NoteDTO ToFrom(Note note)
        {
            return new NoteDTO(note.Text, note.Id, note.RecordId, note.Created, note.LastEdited);
        }
    }
}

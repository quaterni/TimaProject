using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    public interface INoteService
    {
        void Add(NoteDTO note);
        void Update(NoteDTO note);
        bool Delete(Guid noteId);

        IEnumerable<NoteDTO> GetNotes(Guid recordId);
    }
}

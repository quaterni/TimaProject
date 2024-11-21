using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.DTOs
{
    public record NoteDTO(string Text, Guid Id, Guid RecordId, DateTime Created, DateTime LastEdit);
}

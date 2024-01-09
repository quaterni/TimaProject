using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Models
{
    public record Note(string Text, Guid Id, Guid RecordId, DateTime Created)
    {
        public DateTime LastEdited { get; set; } = Created;
    }
}

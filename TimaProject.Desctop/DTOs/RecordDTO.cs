using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.DTOs
{
    internal record RecordDTO(string StartTime, string Title, Guid RecordId, Guid ProjectId, bool IsActive)
    {
        public string? EndTime { get; set; }
        public string? Date { get; set; }
        public string? ProjectName { get; set; }
    }
}

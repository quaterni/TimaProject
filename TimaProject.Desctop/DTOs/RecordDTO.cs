using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.DTOs
{
    public record RecordDto(string StartTime, string Title, Guid RecordId)
    {
        public bool IsActive { get; init; }
        public Guid ProjectId { get; init; } = Guid.Empty;
        public string? EndTime { get; init; }
        public string Date { get; init; }
        public string ProjectName { get; init; }
        public string? Time {get; init;}
    }
}

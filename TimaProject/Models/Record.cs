using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Models
{
    public record Record(DateTimeOffset StartTime, DateOnly Date, ulong Id)
    {
        public DateTimeOffset? EndTime { get; init; } 

        public string Title { get; init; } = string.Empty;

        public Project Project { get; init; } = Project.Empty;

        public List<Note> Notes { get; } = new List<Note>();

        public TimeSpan? Time => IsActive ? null: EndTime - StartTime;

        public bool IsActive => EndTime is null;

    }
}

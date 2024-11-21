using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Domain.Models
{
    public record Record(DateTime StartTime, DateOnly Date, Guid Id)
    {
        public DateTime? EndTime { get; init; }

        public string Title { get; init; } = string.Empty;

        public Project Project { get; init; } = Project.Empty;

        public List<Note> Notes { get; init; } = new List<Note>();

        public bool IsActive => EndTime is null;

    }
}

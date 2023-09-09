using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Models
{
    public record TimaNote(DateTimeOffset StartingTime, ulong Id)
    {
        public DateTimeOffset? StoppingTime { get; init; } 

        public DateOnly Date { get; init; }

        public string? Title { get; init; } 

        public string? Text { get; init; } 

        public Project? Project { get; init; } 

        // поля которые не пойдут в базу данных

        public TimeSpan? Time => IsActive ? null: StoppingTime - StartingTime;

        public bool IsActive => StoppingTime is null;

    }
}

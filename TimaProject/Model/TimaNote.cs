using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Model
{
    public class TimaNote
    {
        public DateTimeOffset StartingTime { get; set; } 

        public DateTimeOffset? StoppingTime { get; set; } 

        public DateOnly Date { get; set; }

        public string? Title { get; set; } 

        public string? Text { get; set; } 

        public Project Project { get; set; } 

        // поля которые не пойдут в базу данных

        public TimeSpan? Time => IsActive ? null: StoppingTime - StartingTime;

        public bool IsActive => StoppingTime is null;

    }
}

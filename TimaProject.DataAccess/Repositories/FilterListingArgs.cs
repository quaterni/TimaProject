using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;

namespace TimaProject.DataAccess.Repositories
{
    public record FilterListingArgs
    {
        public DateTimeOffset? From { get; init; } = null;

        public DateTimeOffset? To { get; init; } = null;

        public DateOnly? Date { get; init; } = null;

        public List<Project>? Projects { get; init; } = null;

        public int? Count { get; init; } = null;

        public bool? IsActive { get; init; } = false;

        public bool IsRecordValid(Record record)
        {
            return (From == null || From >= record.EndTime)
                    && (To == null || To < record.EndTime)
                    && (Projects == null || Projects.Contains(record.Project))
                    && (Date == null || Date == record.Date)
                    && (IsActive == null || IsActive == record.IsActive);
        }
    }
}

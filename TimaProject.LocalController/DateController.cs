using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.LocalController
{
    public class DateController
    {
        private readonly RecordRepository _repository;

        public DateController()
        {
            _repository = Store.GetRecordRepository();
        }


        public DateOnly CurrentDate()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

        public TimeSpan GetTimeAmountPerDate(DateOnly date)
        {
            var records = _repository.GetItems(r => r.Date == date && !r.IsActive);

            return TimeSpan.FromMilliseconds(
                records.Select(r => (r.EndTime - r.StartTime ?? TimeSpan.Zero).TotalMilliseconds)
                .Sum());
        }
    }
}

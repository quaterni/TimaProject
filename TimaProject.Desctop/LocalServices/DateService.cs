using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.LocalController;

namespace TimaProject.Desctop.LocalServices
{
    internal class DateService : IDateService
    {
        private readonly DateController _controller;

        public DateService()
        {
            _controller = new DateController();
        }

        public DateOnly CurrentDate()
        {
            return _controller.CurrentDate();
        }

        public TimeSpan GetTimeAmountPerDate(DateOnly date)
        {
            return _controller.GetTimeAmountPerDate(date);
        }
    }
}

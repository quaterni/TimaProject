using System;
using System.Runtime.CompilerServices;
using TimaProject.Desctop.DTOs;
[assembly:InternalsVisibleTo("TimaProject.Desctop.Tests")]

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface ITimeFormViewModel : IDialog
    {
        string StartTime { get; set; }
        string EndTime { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string ComponentError { get; set; }

        bool CanEndTimeEdit { get; }
        event EventHandler<TimeDTO> TimeSelected;
        
    }
}
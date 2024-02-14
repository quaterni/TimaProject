using System;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    internal interface ITimeFormViewModel : ITimeEditable, IProjectEditable
    {
        string StartTime { get; set; }
        string EndTime { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        bool CanEndTimeEdit { get; }
        event EventHandler<TimeDTO> TimeChanged;
        
    }
}
using TimaProject.Models;
using TimaProject.ViewModels;

namespace TimaProject.Services.Factories
{
    public interface IRecordFactory
    {
        Record Create(RecordViewModel timeNoteViewModel);
        Record CreateActiveNote(RecordViewModel timeNoteViewModel);
    }
}
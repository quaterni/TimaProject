using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels
{
    internal class TimerListingViewModel :ViewModelBase
    {
        private INoteRepository _noteRepository;

        private ObservableCollection<TimaNote> _notes;

        public ObservableCollection<TimaNote> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                SetValue(ref _notes, value);
            }
        }

        public TimerListingViewModel(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            _notes = new(_noteRepository.GetAllNotes( t => !t.IsActive).OrderByDescending(n => n.StoppingTime));
            _noteRepository.NotesChanged += OnNotesChanged;
        }

        private void OnNotesChanged(object? sender, EventArgs e)
        {
            Notes= new(_noteRepository.GetAllNotes(t => !t.IsActive).OrderByDescending(n => n.StoppingTime));           
        }

        public override void Dispose()
        {
            _noteRepository.NotesChanged -= OnNotesChanged;
            base.Dispose();
        }
    }
}

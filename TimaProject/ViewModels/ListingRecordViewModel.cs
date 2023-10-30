using MvvmTools.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels
{
    internal class ListingRecordViewModel :ViewModelBase
    {
        private IRecordRepository _noteRepository;

        private ObservableCollection<Record> _notes;

        public ObservableCollection<Record> Notes
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

        public ListingRecordViewModel(IRecordRepository noteRepository)
        {
            _noteRepository = noteRepository;
            _notes = new(_noteRepository.GetAllNotes( t => !t.IsActive).OrderByDescending(n => n.EndTime));
            _noteRepository.NotesChanged += OnNotesChanged;
        }

        private void OnNotesChanged(object? sender, EventArgs e)
        {
            Notes= new(_noteRepository.GetAllNotes(t => !t.IsActive).OrderByDescending(n => n.EndTime));           
        }

        public override void Dispose()
        {
            _noteRepository.NotesChanged -= OnNotesChanged;
            base.Dispose();
        }
    }
}

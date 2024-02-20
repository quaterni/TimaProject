using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TimaProject.Desctop.Interfaces.ViewModels;
using System.ComponentModel.DataAnnotations;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.DTOs;
using CommunityToolkit.Mvvm.Input;

namespace TimaProject.Desctop.ViewModels
{
    public class NoteViewModel : ObservableValidator, INoteViewModel
    {
        private readonly INoteService _noteService;

        private string _text;

        [Required]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value, true);
            }
        }

        public Guid Id { get; }

        public Guid RecordId { get; }

        public ICommand DeleteNoteCommand { get; }

        public DateTime Created{ get; }

        private DateTime _lastEdit;

        public DateTime LastEdit
        {
            get
            {
                return _lastEdit;
            }
            private set
            {
                SetProperty(ref _lastEdit, value);
            }
        }

        public NoteViewModel(NoteDTO note, INoteService noteService)
        {
            Id = note.Id;
            RecordId = note.RecordId;
            _text = note.Text;
            _lastEdit = note.LastEdit;
            Created = note.Created;

            _noteService = noteService;

            DeleteNoteCommand = new RelayCommand(DeleteNote);

            PropertyChanged += OnTextChanged;
        }

        private void OnTextChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Text) && !string.IsNullOrEmpty(Text))
            {
                UpdateNote();
            }
        }

        private void UpdateNote()
        {
            _noteService.Update(new NoteDTO(
                Text,
                Id,
                RecordId,
                Created,
                DateTime.Now));
        }

        private void DeleteNote()
        {
            _noteService.Delete(Id);
        }
    }
}

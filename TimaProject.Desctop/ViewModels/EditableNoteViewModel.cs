using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Desctop.Commands;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.Desctop.ViewModels
{
    public class EditableNoteViewModel : NotifyDataErrorViewModel
    {
        private readonly IRepository<Note> _noteRepository;

        private Note _note;

        public Note Note
        {
            get { return _note; }
            private set
            {
                SetValue(ref _note, value);
            }
        }

        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetValue(ref _text, value);
            }
        }

        public ICommand RemoveNoteCommand { get; }

        public EditableNoteViewModel(IRepository<Note> noteRepository, Note note)
        {
            _noteRepository = noteRepository;
            _note = note;
            _text = note.Text;
            PropertyChanged += OnTextChanged;
            RemoveNoteCommand = new CommandCallback(
                _ => RemoveNote());
        }

        private void OnTextChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Text) && !HasPropertyErrors(e.PropertyName))
            {
                UpdateNote();
            }
        }

        private void UpdateNote()
        {
            var updatedNote = _note with { Text = Text, LastEdited = DateTime.Now };
            Note = updatedNote;
            _noteRepository.UpdateItem(Note);
        }

        private void RemoveNote()
        {
            _noteRepository.RemoveItem(_note);
        }

        protected override void Validate(string propertyName)
        {
            if (propertyName == nameof(Text))
            {
                ClearErrors(nameof(Text));

                if (string.IsNullOrEmpty(Text))
                {
                    AddError(nameof(Text), "Text must be not empty.");
                }
            }
        }
    }
}

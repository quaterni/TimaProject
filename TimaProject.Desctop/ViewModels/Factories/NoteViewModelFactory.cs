using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class NoteViewModelFactory : INoteViewModelFactory
    {
        private readonly INoteService _noteService;

        public NoteViewModelFactory(INoteService noteService)
        {
            _noteService = noteService;
        }

        public INoteViewModel Create(NoteDTO note)
        {
            return new NoteViewModel(
                note,
                _noteService
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels.Factories
{
    public class NoteFormViewModelFactory
    {
        private readonly IRepository<Note> _noteRepository;

        public NoteFormViewModelFactory(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public NoteFormViewModel Create(Record record)
        {
            return new NoteFormViewModel(
                record,
                _noteRepository);
        }
    }
}

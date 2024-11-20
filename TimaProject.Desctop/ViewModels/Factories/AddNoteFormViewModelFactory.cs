using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class AddNoteFormViewModelFactory : IAddNoteFormFactory
    {
        private readonly INoteService _service;

        public AddNoteFormViewModelFactory(INoteService service)
        {
            _service = service;
        }

        public IAddNoteFormViewModel Create(Guid recordId)
        {
            return new AddNoteFormViewModel(
                recordId,
                _service);
        }
    }
}

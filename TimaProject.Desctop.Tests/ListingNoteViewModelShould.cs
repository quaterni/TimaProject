using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Factories;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class ListingNoteViewModelShould
    {
        private readonly ListingNoteViewModel _sut;
        private readonly Mock<IRepository<Note>> _mockRepository;

        public ListingNoteViewModelShould()
        {
            _mockRepository = new Mock<IRepository<Note>>();
            _sut = new ListingNoteViewModel(
                note => true,
                _mockRepository.Object,
                new EditableNoteViewModelFactory(_mockRepository.Object));

        }

        [Fact]
        public void ContainFilteredNotes()
        {

            var notes = new List<Note>
            {
                new Note("Note1", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now),
                new Note("Note2", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now),
                new Note("Note3", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now),
                new Note("Note4", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now),
            };
            Func<Note, bool> filter = note => true;
            _mockRepository.Setup(x => x.GetItems(filter)).Returns(notes);
            ListingNoteViewModel sut = new ListingNoteViewModel(
                filter, 
                _mockRepository.Object, 
                new EditableNoteViewModelFactory(_mockRepository.Object));

            var result = sut.Notes.Select(editableNote => editableNote.Note).ToList();

            Assert.Equal(notes, result);
        }


    }
}

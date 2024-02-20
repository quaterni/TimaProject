using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests.ViewModels
{
    public class ListingNoteViewModelShould
    {
        private readonly ListingNoteViewModel _sut;

        private static Guid s_recordId = new Guid("80e680ac-7b06-46b6-bbed-39c7232d9896");

        private readonly Mock<INoteViewModelFactory> _mockNoteViewModelFactory;
        private readonly Mock<INoteService> _mockNoteService;

        public ListingNoteViewModelShould()
        {
            _mockNoteService = new Mock<INoteService>();
            _mockNoteViewModelFactory = new Mock<INoteViewModelFactory>();

            _sut = new ListingNoteViewModel(
                s_recordId,
                _mockNoteService.Object,
                _mockNoteViewModelFactory.Object);
        }

        [Fact]
        public void Notes_LazyInit()
        {
            _ = _sut.Notes;
            _ = _sut.Notes;

            _mockNoteService.Verify(s => s.GetNotes(s_recordId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void Notes_GotFromServiceOrderedByCreation(IEnumerable<NoteDTO> notes)
        {
            List<DateTime> creationDates = new();
            _mockNoteService
                .Setup(s=> s.GetNotes(s_recordId))
                .Returns(notes);
            _mockNoteViewModelFactory
                .Setup(s => s.Create(It.IsAny<NoteDTO>()))
                .Callback<NoteDTO>(note =>
                {
                    notes.Should().Contain(note);
                    creationDates.Add(note.Created);
                });

            _ = _sut.Notes;

            _mockNoteViewModelFactory
                .Verify(s => s.Create(It.IsAny<NoteDTO>()), Times.Exactly(notes.Count()));
            creationDates.Should().BeInAscendingOrder();
        }

    }
}

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests
{
    public class EditableNoteViewModelShould
    {
        private readonly EditableNoteViewModel _sut;
        private readonly Mock<IRepository<Note>> _mockNoteRepository;

        public EditableNoteViewModelShould()
        {
            _mockNoteRepository = new Mock<IRepository<Note>>();
            _sut = new EditableNoteViewModel(
                _mockNoteRepository.Object,
                new Note("MyNote", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddHours(-5)));
        }

        [Fact]
        public void UpdateTextOnNote()
        {
            var expectedText = "NewText";
            _mockNoteRepository
                .Setup(x => x.UpdateItem(It.IsAny<Note>()))
                .Callback<Note>(note =>
                {
                    Assert.Equal(expectedText, note.Text);
                    Assert.Equal(DateTime.Now, note.LastEdited, TimeSpan.FromSeconds(1));
                    Assert.Equal(note, _sut.Note);
                });

            _sut.Text = expectedText;

            _mockNoteRepository.Verify(x => x.UpdateItem(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public void NotUpdateNot_IfTextNotValid()
        {
            var expectedText = "";

            _sut.Text = expectedText;

            _mockNoteRepository.Verify(x => x.UpdateItem(It.IsAny<Note>()), Times.Never);
            Assert.True(_sut.HasPropertyErrors(nameof(EditableNoteViewModel.Text)));
        }

        [Fact]
        public void RemoveNote_WhenRemoveNoteCommandExecuted()
        {
            _mockNoteRepository
                .Setup(x => x.RemoveItem(It.IsAny<Note>()))
                .Callback<Note>(note =>
                {
                    Assert.Equal(_sut.Note, note);
                });

            _sut.RemoveNoteCommand.Execute(null);
            _mockNoteRepository.Verify(x => x.RemoveItem(It.IsAny<Note>()), Times.Once);
        }
    }
}

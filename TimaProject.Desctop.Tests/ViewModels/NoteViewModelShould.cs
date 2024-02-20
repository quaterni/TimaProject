using AutoFixture;
using FluentAssertions;
using Moq;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests.ViewModels
{
    public class NoteViewModelShould
    {
        private readonly NoteViewModel _sut;

        private readonly Mock<INoteService> _mockNoteService;

        private static NoteDTO s_note
        {
            get
            {
                return new NoteDTO(
                    "MyNote",
                    new Guid("80e680ac-7b06-46b6-bbed-39c7232d9896"),
                    new Guid("588a836c-ec2c-4489-9a76-760bb99fedd6"),
                    new DateTime( new DateOnly(2024, 2,20), new TimeOnly(5,22)),
                    new DateTime( new DateOnly(2024, 2,20), new TimeOnly(5,22)));
            }
        }

        public NoteViewModelShould()
        {
            _mockNoteService = new Mock<INoteService>();

            _sut = new NoteViewModel(
                s_note,
                _mockNoteService.Object);
        }

        [Fact]
        public void Init()
        {
            _sut.Text.Should().Be(s_note.Text);
            _sut.Id.Should().Be(s_note.Id);
            _sut.RecordId.Should().Be(s_note.RecordId);
            _sut.Created.Should().Be(s_note.Created);
            _sut.LastEdit.Should().Be(s_note.LastEdit);
        }

        [Fact]
        public void Text_WhenChanged_UpdateNote()
        {
            _sut.Text = new Fixture().Create<string>();

            _mockNoteService.Verify(s=> s.Update(It.IsAny<NoteDTO>()), Times.Once);
        }

        [Fact]
        public void Text_WhenChangedToEmpty_HasValidationError_NoUpdate()
        {
            _sut.Text = string.Empty;

            _sut.HasErrors.Should().BeTrue();
            _mockNoteService.Verify(s => s.Update(It.IsAny<NoteDTO>()), Times.Never);
        }

        [Fact]
        public void DeleteNoteCommand_WhenExecuted_DeleteNote()
        {
            _sut.DeleteNoteCommand.Execute(null);

            _mockNoteService.Verify(s => s.Delete(s_note.Id), Times.Once);
        }
    }
}

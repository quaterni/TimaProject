using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels;
using Xunit;

namespace TimaProject.Tests
{
    public class NoteFormViewModelShould
    {
        private readonly NoteFormViewModel _sut;
        private readonly Models.Record _source;
        private readonly Mock<IRepository<Note>> _mockRepository;

        public NoteFormViewModelShould()
        {
            _source = new Models.Record(DateTime.Now, DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid());
            _mockRepository = new Mock<IRepository<Note>>();

            _sut = new NoteFormViewModel(
                _source,
                _mockRepository.Object);
        }

        [Fact]
        public void AddNewNote_WhenInvokeAddNewNoteCommand()
        {
            var expectedText = "NewNote";
            var expectedRecordId = _source.Id;
            _mockRepository
                .Setup(x => x.AddItem(It.IsAny<Note>()))
                .Callback<Note>( note =>
                {
                    Assert.Equal(expectedText, note.Text);
                    Assert.Equal(expectedRecordId, note.RecordId);
                    Assert.Equal(DateTime.Now, note.Created, TimeSpan.FromSeconds(1));
                    Assert.Equal(DateTime.Now, note.LastEdited, TimeSpan.FromSeconds(1));
                });

            _sut.Text = expectedText;
            _sut.AddNewNoteCommand.Execute(null);

            _mockRepository.Verify(x => x.AddItem(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public void CanAddNewNote_WhenTextNotEmpty()
        {
            Assert.False(_sut.IsCanAdd);

            _sut.Text = "SomeText";
            Assert.True(_sut.IsCanAdd);

            _sut.Text = "";
            Assert.False(_sut.IsCanAdd);

            _sut.Text = "AndAgain";
            Assert.True(_sut.IsCanAdd);
        }
    }
}

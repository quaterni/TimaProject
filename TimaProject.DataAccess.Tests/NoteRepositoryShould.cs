using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Exceptions;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;
using Xunit;

namespace TimaProject.DataAccess.Tests
{
    public class NoteRepositoryShould
    {
        private readonly NoteRepository _sut;
        private readonly static Note _addedNote = new Note("Note", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

        public NoteRepositoryShould()
        {
            _sut = new NoteRepository();

            _sut.AddItem(_addedNote);
        }

        [Fact]
        public void AddNewNote()
        {
            var note = new Note("Added Note", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            _sut.AddItem(note);

            Assert.Contains(note, _sut.GetItems(_ => true));
        }

        [Fact]
        public void ThrowAddingNotUniqueItemException_IfAddedNoteWithSameId()
        {
            Assert.Throws<AddingNotUniqueItemException>(()=> _sut.AddItem(_addedNote));
        }

        [Theory, MemberData(nameof(ContainsTestData))]
        public void Contains_ReturnExpectedResult(Note note, bool expected)
        {
            var resultByNote = _sut.Contains(note);
            var resultById = _sut.Contains(note.Id);
            Assert.Equal(expected, resultByNote);
            Assert.Equal(expected, resultById);
        }

        public static IEnumerable<object[]> ContainsTestData()
        {
            yield return new object[] { _addedNote, true };
            yield return new object[] { new Note("", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now), false };
        }

        [Fact]
        public void UpdateNote()
        {
            var updatedNote = _addedNote with { Text = "NewText", LastEdited = DateTime.Now };
            _sut.UpdateItem(updatedNote);

            var notes = _sut.GetItems(_ => true);
            Assert.DoesNotContain(_addedNote, notes);
            Assert.Contains(updatedNote, notes);
        }

        [Fact]
        public void ThrowNotFoundException_WhenUpdatedItemNotFound()
        {
            var note = new Note("", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var exception = Assert.Throws<NotFoundException<Note>>(() => _sut.UpdateItem(note));
            Assert.Equal(note, exception.Item);
        }

        [Theory, MemberData(nameof(RemoveTestData))]
        public void RemoveItem_ReturnsCorrectResult(Note note, bool expected)
        {
            var result = _sut.RemoveItem(note);
            Assert.Equal(expected, result);

            var notes = _sut.GetItems(_ => true);
            Assert.DoesNotContain(note, notes);
        }

        public static IEnumerable<object[]> RemoveTestData()
        {
            yield return new object[] { _addedNote, true };
            yield return new object[] { new Note("", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now), false };
        }

        [Fact]
        public void RaiseRepositoryChanged_WhenAddNewItem()
        {
            var note = new Note("", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            var eventData= Assert.Raises<RepositoryChangedEventArgs<Note>>(
                (e) => _sut.RepositoryChanged += e,
                (e) => _sut.RepositoryChanged -= e,
                () => _sut.AddItem(note)
            );
            Assert.Equal(_sut, eventData.Sender);
            Assert.Equal(note, eventData.Arguments.Item);
            Assert.Equal(RepositoryChangedOperation.Add, eventData.Arguments.Operation);
        }

        [Fact]
        public void RaiseRepositoryChanged_WhenUpdateItem()
        {
            var updatedNote  = _addedNote with { Text = "NewText", LastEdited = DateTime.Now };

            var eventData = Assert.Raises<RepositoryChangedEventArgs<Note>>(
                (e) => _sut.RepositoryChanged += e,
                (e) => _sut.RepositoryChanged -= e,
                () => _sut.UpdateItem(updatedNote)
            );
            Assert.Equal(_sut, eventData.Sender);
            Assert.Equal(updatedNote, eventData.Arguments.Item);
            Assert.Equal(RepositoryChangedOperation.Update, eventData.Arguments.Operation);
        }


        [Fact]
        public void RaiseRepositoryChanged_WhenRemoveItem()
        {

            var eventData = Assert.Raises<RepositoryChangedEventArgs<Note>>(
                (e) => _sut.RepositoryChanged += e,
                (e) => _sut.RepositoryChanged -= e,
                () => _sut.RemoveItem(_addedNote)
            );
            Assert.Equal(_sut, eventData.Sender);
            Assert.Equal(_addedNote, eventData.Arguments.Item);
            Assert.Equal(RepositoryChangedOperation.Remove, eventData.Arguments.Operation);
        }
    }
}

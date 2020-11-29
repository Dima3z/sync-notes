using System;
using System.Collections.Generic;
using Notes.Core.Interfaces;
using Notes.Core.Models;

namespace Notes.Core
{
    public interface INotesRepository
    {
        public Guid AddNote(INoteCreateArgs args);
        public void UpdateNote(INoteUpdateArgs args);
        public void MarkAsDeleted(Guid id);
        public void RestoreNote(Guid id);
        public void DeleteMarked(Guid id);
        public INote GetById(Guid id);
        public IReadOnlyCollection<INote> GetAll();
        public IReadOnlyCollection<INote> GetAllDeleted();
        public IReadOnlyCollection<INote> GetByFilter(INoteFilter filter);
        public void Sync();
    }
}
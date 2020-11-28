using System;
using System.Collections.Generic;
using Notes.Core.Models;

namespace Notes.Core
{
    public interface INotesRepository
    {
        public INote GetById(Guid id);
        public IReadOnlyCollection<INote> GetAll();
        public IReadOnlyCollection<INote> GetByFilter(NoteFilter filter);
    }
}
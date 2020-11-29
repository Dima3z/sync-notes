using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Notes.Core.Interfaces;
using Notes.Core.Models;

namespace Notes.Core
{
    public class LocalNotesRepository : INotesRepository
    {
        private readonly IList<INote> _notes;
        private readonly IList<INote> _deletedNotes;

        public LocalNotesRepository()
        {
            _notes = new List<INote>();
            _deletedNotes = new List<INote>();
        }

        public LocalNotesRepository(IEnumerable<INote> notes, IEnumerable<INote> deletedNotes)
        {
            _notes = notes?.ToList() ?? new List<INote>();
            _deletedNotes = deletedNotes?.ToList() ?? new List<INote>();
        }
        
        public Guid AddNote(INoteCreateArgs args)
        {
            Ensure.That(args).IsNotNull();
            var note = Note.Create(
                Guid.NewGuid(), 
                args.Title ?? string.Empty,
                args.Content ?? string.Empty);
            _notes.Add(note);
            return note.Id;
        }

        public void UpdateNote(INoteUpdateArgs args)
        {
            Ensure.That(args).IsNotNull();
            var found = GetById(args.Id);
            found.Update(
                args.Title,
                args.Content);
        }

        public void MarkAsDeleted(Guid id)
        {
            var found = GetById(_notes, id);
            found.MarkAsDeleted();
            _notes.Remove(found);
            _deletedNotes.Add(found);
        }

        public void RestoreNote(Guid id)
        {
            var found = GetById(_deletedNotes, id);
            found.UnmarkAsDeleted();
            _deletedNotes.Remove(found);
            _notes.Add(found);
        }

        public void DeleteMarked(Guid id)
        {
            var found = GetById(_deletedNotes, id);
            _deletedNotes.Remove(found);
        }

        public INote GetById(Guid id)
        {
            return GetById(_notes, id);
        }

        public IReadOnlyCollection<INote> GetAll()
        {
            return _notes.ToArray();
        }

        public IReadOnlyCollection<INote> GetAllDeleted()
        {
            return _deletedNotes.ToArray();
        }

        public IReadOnlyCollection<INote> GetByFilter(INoteFilter filter)
        {
            throw new NotImplementedException();
        }

        public void Sync()
        {
            throw new NotImplementedException();
        }

        private INote GetById(IEnumerable<INote> collection, Guid id)
        {
            Ensure.That(collection).IsNotNull();
            Ensure.That(id).IsNotDefault();
            var found = collection.FirstOrDefault(note => note.Id == id);
            if (found == null)
            {
                throw new ArgumentException($"Note with identifier {id} not found");
            }
            return found;
        }
    }
}
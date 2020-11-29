using System;

namespace Notes.Core.Interfaces
{
    public interface INote
    {
        public Guid Id { get; }
        public bool Deleted { get; }
        public string Title { get; }
        public DateTimeOffset DateCreated { get; }
        public DateTimeOffset DateUpdated { get; }
        public string Content { get; }

        public INote Update(
            string title,
            string content);
        
        public INote MarkAsDeleted();
        public INote UnmarkAsDeleted();
    }
}
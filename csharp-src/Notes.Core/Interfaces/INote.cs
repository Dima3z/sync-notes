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
        public Guid? ContentId { get; }

        public INote Update(
            string title,
            Guid? contentId);
        
        public INote MarkAsDeleted();
        public INote UnmarkAsDeleted();
    }
}
using System;

namespace Notes.Core
{
    public interface INote
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public DateTimeOffset DateCreated { get; }
        public DateTimeOffset DateUpdated { get; }
        public string Content { get; set; }
    }
}
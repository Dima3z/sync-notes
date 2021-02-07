using System;

namespace Notes.Core.Interfaces
{
    public interface INoteUpdateArgs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ContentId { get; set; }
    }
}
using System;

namespace Notes.Core.Interfaces
{
    public interface INoteCreateArgs
    {
        public string Title { get; set; }
        public Guid? ContentId { get; set; }
    }
}
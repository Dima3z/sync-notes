using System;

namespace Notes.Core.Models
{
    public class NoteCreateArgs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
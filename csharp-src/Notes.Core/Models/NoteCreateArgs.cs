using System;
using Notes.Core.Interfaces;

namespace Notes.Core.Models
{
    public class NoteCreateArgs : INoteCreateArgs
    {
        public string Title { get; set; }
        public Guid? ContentId { get; set; }
    }
}
using System;
using Notes.Core.Interfaces;

namespace Notes.Core.Models
{
    public class NoteUpdateArgs : INoteUpdateArgs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ContentId { get; set; }
    }
}
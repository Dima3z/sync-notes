using System;
using System.Windows.Controls;
using Notes.Core.Interfaces;

namespace Notes.Desktop.Models
{
    public class NoteTreeItem : TreeViewItem
    {
        public Guid NoteId { get; set; }
    }
}
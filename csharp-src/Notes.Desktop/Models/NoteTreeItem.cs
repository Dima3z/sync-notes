using System;
using System.Windows.Controls;
using Notes.Core.Interfaces;

namespace Notes.Desktop.Models
{
    public class NoteTreeItem : TreeViewItem
    {
        public readonly Guid NoteId;
        public NoteTreeItem(MainWindow mainWindow, Guid noteId)
        {
            NoteId = noteId;
            Header = mainWindow.NotesRepository.GetById(noteId).Title;
            ContextMenu = new NoteContextMenu(noteId);
        }
    }
}
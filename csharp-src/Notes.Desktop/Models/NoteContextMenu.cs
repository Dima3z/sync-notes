using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using Notes.Desktop.UI;

namespace Notes.Desktop.Models
{
    public class NoteContextMenu : ContextMenu
    {
        private Guid _noteId;
        public NoteContextMenu(Guid noteId)
        {
            _noteId = noteId;
            AddOption(nameof(Rename), Rename);
            AddOption(nameof(Delete), Delete);
        }

        private void AddOption(string header, Action<NoteTreeItem, RoutedEventArgs> handler)
        {
            var item = new MenuItem
            {
                Header = header,
                Name = header
            };
            item.Click += (sender, args) =>
            {
                var castSender = (sender as MenuItem).Parent as NoteContextMenu;
                handler(castSender.PlacementTarget as NoteTreeItem, args);
            };
            Items.Add(item);
        }

        private static void Rename(NoteTreeItem sender, RoutedEventArgs e)
        {
            var dialog = new CreateNoteDialog();
            if (dialog.ShowDialog() == true)
            {
                var title = dialog.Result().Title;
                MainWindow.GetInstance.NotesRepository.UpdateNote(new NoteUpdateArgs
                {
                    Id = sender.NoteId,
                    Title = title
                });
                sender.Header = title;
            }
        }

        private static void Delete(NoteTreeItem sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(MainWindow.GetInstance.NotesTree.SelectedItem, sender))
            {
                MainWindow.GetInstance.SetActiveNote(null);
                sender.IsSelected = false;
            }
            MainWindow.GetInstance.NotesTree.Items.Remove(sender);
            MainWindow.GetInstance.NotesRepository.MarkAsDeleted(sender.NoteId);
        }
    }
}
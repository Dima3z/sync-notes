using System;
using System.Linq;
using System.Windows;
using Notes.Core;
using Notes.Core.Models;
using Notes.Core.Persistence;
using Notes.Desktop.Models;

namespace Notes.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private INotesRepository _repository;
        private readonly PersistenceManager _persistenceManager;
        public MainWindow()
        {
            Application.Current.Exit += Exit_Handler;
            InitializeComponent();
            _persistenceManager = new PersistenceManager("C:/data/");
            LoadData();
        }

        private void Exit_Handler(object sender, ExitEventArgs e)
        {
            SaveData();
        }
        
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SyncButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var id = _repository.AddNote(new NoteCreateArgs
            {
                Content = null,
                Title = $"Title #{new Random().Next()}"
            });
            var note = _repository.GetById(id);
            NotesTree.Items.Add(new NoteTreeItem
            {
                Header = note.Title,
                NoteId = id
            });
        }

        private void NotesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.OldValue is NoteTreeItem oldItem)
            {
                _repository.UpdateNote(new NoteUpdateArgs
                {
                    Id = oldItem.NoteId,
                    Title = null,
                    Content = MainTextBox.SaveToString()
                });
            }
            
            MainTextBox.Document.Blocks.Clear();

            if (!(e.NewValue is NoteTreeItem obj))
            {
                return;
            }
            
            var note = _repository.GetById(obj.NoteId);
            NoteTitle.Content = note.Title;
            if (note.Content != null && note.Content.Any())
            {
                MainTextBox.LoadFromString(note.Content);
            }
        }

        private void SaveData()
        {
            _persistenceManager.SaveNotes(_repository.GetAll().Concat(_repository.GetAllDeleted()));
        }
        private void LoadData()
        {
            var notes = _persistenceManager.LoadNotes().ToArray();
            _repository = new LocalNotesRepository(
                notes.Where(n => !n.Deleted).ToList(),
                notes.Where(n => n.Deleted).ToList());
            
            foreach (var loadedNote in _repository.GetAll())
            {
                NotesTree.Items.Add(new NoteTreeItem
                {
                    Header = loadedNote.Title,
                    NoteId = loadedNote.Id
                });
            }
        }
    }
}
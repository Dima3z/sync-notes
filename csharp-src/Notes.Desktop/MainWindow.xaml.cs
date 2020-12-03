using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Notes.Core;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using Notes.Core.Persistence;
using Notes.Desktop.Models;
using Notes.Desktop.UI;

namespace Notes.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow GetInstance { get; private set; }
        public INotesRepository NotesRepository;
        private readonly PersistenceManager _persistenceManager;
        public MainWindow()
        {
            Application.Current.Exit += Exit_Handler;
            _persistenceManager = new PersistenceManager("C:/data/");
            InitializeComponent();
            LoadData();
            SetActiveNote(null);
            GetInstance = this;
        }

        public void SetActiveNote(INote note)
        {
            if (note == null)
            {
                NoteTitle.Content = string.Empty;
                MainTextBox.Document.Blocks.Clear();
                return;
            }
            NoteTitle.Content = note.Title;
            if (note.Content != null && note.Content.Any())
            {
                MainTextBox.LoadFromString(note.Content);
            }
            else
            {
                MainTextBox.Document.Blocks.Clear();
            }
            MainTextBox.Focus();
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
            var dialog = new CreateNoteDialog();
            if (dialog.ShowDialog() == true)
            {
                var id = NotesRepository.AddNote(dialog.Result());
                var item = new NoteTreeItem(this, id);
                NotesTree.Items.Add(item);
                item.IsSelected = true;
            }
        }

        private void NotesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.OldValue is NoteTreeItem oldItem)
            {
                NotesRepository.UpdateNote(new NoteUpdateArgs
                {
                    Id = oldItem.NoteId,
                    Title = null,
                    Content = MainTextBox.SaveToString()
                });
            }
            if (!(e.NewValue is NoteTreeItem obj))
            {
                return;
            }
            SetActiveNote(NotesRepository.GetById(obj.NoteId));
        }

        private void SaveData()
        {
            _persistenceManager.SaveNotes(NotesRepository.GetAll().Concat(NotesRepository.GetAllDeleted()));
        }
        
        private void LoadData()
        {
            var notes = _persistenceManager.LoadNotes().ToArray();
            NotesRepository = new LocalNotesRepository(
                notes.Where(n => !n.Deleted).ToList(),
                notes.Where(n => n.Deleted).ToList());

            
            foreach (var loadedNote in NotesRepository.GetAll())
            {
                NotesTree.Items.Add(new NoteTreeItem(this, loadedNote.Id));
            }
        }
    }
}
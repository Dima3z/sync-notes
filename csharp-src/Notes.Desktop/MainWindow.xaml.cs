using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Notes.Core;
using Notes.Core.Interfaces;
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
            if (note.ContentId.HasValue)
            {
                LoadContent(note.ContentId.Value, note.Title);
            }
            else
            {
                MainTextBox.Document.Blocks.Clear();
            }
            MainTextBox.Focus();
        }
        
        public void CloseActiveNote(Guid id)
        {
            var note = NotesRepository.GetById(id);
            if (!note.ContentId.HasValue)
            {
                var contentId = Guid.NewGuid();
                NotesRepository.UpdateNote(new NoteUpdateArgs
                {
                    Id = id,
                    ContentId = contentId
                });
                SaveContent(contentId, NoteTitle.Content as string);
            }
            else
            {
                SaveContent(note.ContentId.Value, NoteTitle.Content as string);
            }
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
                var createArgs = dialog.Result();
                var id = NotesRepository.AddNote(createArgs);
                var item = new NoteTreeItem(this, id);
                NotesTree.Items.Add(item);
                item.IsSelected = true;
            }
        }

        private void NotesTree_OnSelectedItemChanged(
            object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.OldValue is NoteTreeItem oldItem)
            {
                CloseActiveNote(oldItem.NoteId);
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
            var notes = _persistenceManager.LoadAllNotes();
            NotesRepository = new LocalNotesRepository(
                notes.Where(n => !n.Deleted).ToList(),
                notes.Where(n => n.Deleted).ToList());

            foreach (var loadedNote in NotesRepository.GetAll())
            {
                NotesTree.Items.Add(new NoteTreeItem(this, loadedNote.Id));
            }
        }

        private void SaveContent(Guid contentId, string title)
        {                
            var t = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            var writeStream = _persistenceManager.GetContentWriteStream(contentId, title);
            t.Save(writeStream, DataFormats.Rtf);
            writeStream.Close();
        }

        private void LoadContent(Guid contentId, string title)
        {
            var data = _persistenceManager.GetContentReadStream(contentId, title);
            MainTextBox.SelectAll();
            MainTextBox.Selection.Load(data, DataFormats.Rtf);
            MainTextBox.Selection.Select(MainTextBox.Document.ContentStart,MainTextBox.Document.ContentStart);
            data.Close();
        }
    }
}
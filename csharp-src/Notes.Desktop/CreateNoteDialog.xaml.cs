using System;
using System.Windows;
using System.Windows.Input;
using Notes.Core.Interfaces;
using Notes.Core.Models;

namespace Notes.Desktop.UI
{
    public partial class CreateNoteDialog : Window
    {
        public CreateNoteDialog()
        {
            InitializeComponent();
            TitleTextBox.Text = "Note title";
            TitleTextBox.SelectAll();
            TitleTextBox.Focus();
        }

        public INoteCreateArgs Result()
        {
            return new NoteCreateArgs
            {
                Title = TitleTextBox.Text
            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonBase_OnClick(sender, null);
            }
        }
    }
}
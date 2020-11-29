using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notes.Desktop
{
    public static class RichTextHelper
    {
        public static string SaveToString(this RichTextBox rtb)
        {
            using var stream = new MemoryStream();
            var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            range.Save(stream, DataFormats.XamlPackage);
            return Convert.ToBase64String(stream.ToArray());
        }
        public static void LoadFromString(this RichTextBox rtb, string data)
        {
            using var stream = new MemoryStream();
            var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            stream.Write(Convert.FromBase64String(data));
            range.Load(stream, DataFormats.XamlPackage);
        }
    }
}
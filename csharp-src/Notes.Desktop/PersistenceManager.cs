using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notes.Core.Interfaces;
using Notes.Core.Models;

namespace Notes.Core.Persistence
{
    public class PersistenceManager
    {
        public PersistenceManager(string dir)
        {
            DirectoryPath = dir;
            ContentPath = Path.Combine(dir, ContentDir);
            if (!Directory.Exists(ContentPath))
            {
                Directory.CreateDirectory(ContentPath);
            }
        }
        private const string ContentDir = "content";
        public string DirectoryPath { get; private set; }
        public string ContentPath { get; private set; }

        public void SaveNotes(IEnumerable<INote> notes)
        {
            foreach (var note in notes)
            {
                SaveNote(note);
            }
        }

        public void SaveNote(INote note)
        {
            var path = Path.Combine(DirectoryPath, $"{note.Id}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(note, Formatting.Indented));
        }

        public IReadOnlyCollection<INote> LoadAllNotes()
            => GetNotesInDirectory().Select(LoadNote).ToList();

        public IReadOnlyCollection<Guid> GetNotesInDirectory()
        {
            var files = Directory.GetFiles(DirectoryPath);
            var result = new List<Guid>();
            foreach (var file in files)
            {
                var idString = file
                    .Split('/')
                    .Last()
                    .Split('.')
                    .First();
                if (!Guid.TryParse(idString, out var id))
                {
                    continue;
                }
                result.Add(id);
            }
            return result;
        }

        public INote LoadNote(Guid id)
        {
            var path = Path.Combine(DirectoryPath, $"{id}.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No note in path {path} found");
            }
            var data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Note>(data);
        }

        public FileStream GetContentWriteStream(Guid contentId, string title)
        {
            var filePath = Path.Combine(ContentPath, GetContentFileName(contentId, title));
            return File.Exists(filePath) ? File.OpenWrite(filePath) : File.Create(filePath);
        }

        public Stream GetContentReadStream(Guid contentId, string title) => File.OpenRead(Path.Combine(ContentPath, GetContentFileName(contentId, title)));

        private string GetContentFileName(Guid contentId, string title)
            =>
                new string($"{title}_{contentId}.rtf"
                .Where(x => !Path.GetInvalidFileNameChars().Contains(x))
                .Where(x => !Path.GetInvalidPathChars().Contains(x))
                .ToArray());
        
    }
}
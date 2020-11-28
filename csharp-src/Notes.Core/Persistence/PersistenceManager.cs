using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notes.Core.Models;

namespace Notes.Core.Persistence
{
    public class PersistenceManager
    {
        public PersistenceManager(string dir)
        {
            DirectoryPath = dir;
        }
        public string DirectoryPath { get; private set; }

        public void SaveNotes(IEnumerable<Note> notes)
        {
            foreach (var note in notes)
            {
                var path = Path.Combine(DirectoryPath, $"{note.Id}.json");
                File.WriteAllText(path, JsonConvert.SerializeObject(note, Formatting.Indented));
            }
        }

        public IEnumerable<Note> LoadNotes()
        {
            var files = Directory.GetFiles(DirectoryPath);
            var result = new List<Note>();
            foreach (var file in files)
            {
                var idString = file
                    .Split('/')
                    .Last()
                    .Split('.')
                    .First();
                Guid id;
                if (!Guid.TryParse(idString, out id))
                {
                    continue;
                }
                var data = File.ReadAllText(file);
                result.Add(JsonConvert.DeserializeObject<Note>(data));
                if (id != result.Last().Id)
                {
                    Console.WriteLine("WARNING file id not equal to data id");
                }
            }
            return result;
        }
    }
}
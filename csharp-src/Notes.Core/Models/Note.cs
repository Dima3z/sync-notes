using System;
using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notes.Core.JsonHelpers;

namespace Notes.Core.Models
{
    public class Note : INote
    {
        
        [JsonProperty(
            PropertyName = "id",
            Required = Required.Always)]
        public Guid Id { get; private set; }
        [JsonProperty(
            PropertyName = "title",
            Required = Required.Always)]
        public string Title { get; set; }
        [JsonProperty(
            PropertyName = "date_created")]
        public DateTimeOffset DateCreated { get; private set; }
        [JsonProperty(
            PropertyName = "date_updated",
            Required = Required.Always)]
        public DateTimeOffset DateUpdated { get; private set; }
        [JsonProperty(
            PropertyName = "content",
            Required = Required.Always)]
        [JsonConverter(typeof(ContentConverter))]
        public string Content { get; set; }

        public static Note Create(NoteCreateArgs args)
        {
            Ensure.That(args).IsNotNull();
            Ensure.That(args.Id).IsNotDefault();
            Ensure.That(args.Id).IsNot(Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"));
            Ensure.That(args.Title).IsNotNull();
            var date = DateTimeOffset.UtcNow;
            return new Note
            {
                Id = args.Id,
                Title = args.Title,
                DateCreated = date,
                DateUpdated = date,
                Content = args.Content
            };
        }

        public Note Update(NoteUpdateArgs args)
        {
            Ensure.That(args).IsNotNull();
            Title = args.Title ?? Title;
            Content = args.Content ?? Content;
            DateUpdated = DateTimeOffset.UtcNow;
            return this;
        }

        private Note()
        {
            
        }
    }
}
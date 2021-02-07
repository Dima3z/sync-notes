using System;
using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notes.Core.Interfaces;
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
            PropertyName = "deleted",
            Required = Required.Always)]
        public bool Deleted { get; private set; }
        [JsonProperty(
            PropertyName = "title",
            Required = Required.Always)]
        public string Title { get; private set; }
        [JsonProperty(
            PropertyName = "date_created",
            Required = Required.Always)]
        public DateTimeOffset DateCreated { get; private set; }
        [JsonProperty(
            PropertyName = "date_updated")]
        public DateTimeOffset DateUpdated { get; private set; }
        [JsonProperty(
            PropertyName = "content_id")]
        public Guid? ContentId { get; private set; }

        public INote MarkAsDeleted()
        {
            Deleted = true;
            return this;
        }

        public INote UnmarkAsDeleted()
        {
            Deleted = false;
            return this;
        }

        public static Note Create(
            Guid id,
            string title,
            Guid? contentId)
        {
            var date = DateTimeOffset.UtcNow;
            return new Note
            {
                Id = id,
                Title = title,
                DateCreated = date,
                DateUpdated = date,
                ContentId = contentId
            };
        }

        public INote Update(
            string title,
            Guid? contentId)
        {
            Title = title ?? Title;
            ContentId = contentId ?? ContentId;
            DateUpdated = DateTimeOffset.UtcNow;
            return this;
        }

        private Note()
        {
            
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}\r\n" +
                   $" {nameof(Deleted)}: {Deleted}\r\n" +
                   $" {nameof(Title)}: {Title}\r\n" +
                   $" {nameof(DateCreated)}: {DateCreated}\r\n" +
                   $" {nameof(DateUpdated)}: {DateUpdated}\r\n" +
                   $" {nameof(ContentId)}: {ContentId}\r\n\r\n";
        }
    }
}
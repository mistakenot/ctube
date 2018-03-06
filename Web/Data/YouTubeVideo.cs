using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.YouTube.v3.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.Data
{
    public class YouTubeVideo : IEntity
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement]
        public string ChannelId { get; set; }
        [BsonElement]
        public float Rating { get; set; }
        [BsonElement]
        public TimeSpan Length { get; set; }
        [BsonElement]
        public ulong? Views { get; set; }
        [BsonElement]
        public IEnumerable<string> Tags { get; set; }
        [BsonElement]
        public IEnumerable<string> MentionedVideos { get; set; }
        [BsonElement]
        public IEnumerable<string> MendionedChannels { get; set; }
        [BsonElement]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        

        public static YouTubeVideo From(Video item)
        {
            var mentionedVideos = Regex
                .Matches(item.Snippet.Description, @"(?<=(http[s]?(://))(youtu.be/)|(www.youtube.com/watch\?v=))([\w-]+)(\b)")
                .Cast<Match>()
                .Select(m => m.Value);

            var mentionedChannels = Regex
                .Matches(item.Snippet.Description, @"(?<=(http[s]?(://))(www.youtube.com/channel/))([\w-]+)(\b)")
                .Select(m => m.Value);

            var video = new YouTubeVideo
            {
                Id = item.Id,
                Views = item.Statistics.ViewCount,
                Rating = 
                    item.Statistics.LikeCount ?? 0L / 
                    (1L + item.Statistics.LikeCount ?? 0L + item.Statistics.DislikeCount ?? 0L),
                Length = Parse(item.ContentDetails.Duration),
                Tags = item.Snippet.Tags,
                MentionedVideos = mentionedVideos,
                MendionedChannels = mentionedChannels,
                ChannelId = item.Snippet.ChannelId
            };

            return video;
        }

        public static TimeSpan Parse(string youtubeDuration)
        {
            TimeSpan length = default(TimeSpan);

            try
            {
                length = TimeSpan.ParseExact(youtubeDuration, @"'PT'm'M's'S'", null);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Couldn't read time: {youtubeDuration}.", e);
            }

            return length;
        }
    }
}
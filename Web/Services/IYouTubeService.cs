using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.YouTube.v3.Data;

namespace Web.Services
{
    public interface IYouTubeService
    {
         YouTubeVideo GetVideoById(string id);
         IEnumerable<YouTubeVideo> GetVideosByChannelId(string id);
    }

    public class YouTubeVideo
    {
        public string Id { get; private set; }
        public float Rating { get; private set; }
        public TimeSpan Length { get; private set; }
        public ulong? Views { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
        public IEnumerable<string> MentionedVideos { get; private set; }
        public IEnumerable<string> MendionedChannels { get; private set; }

        public static YouTubeVideo From(Video item)
        {
            var matches = Regex
                .Matches(item.Snippet.Description, @"(http[s]?)(://youtu.be/)(\w+)(\b)")
                .Cast<Match>()
                .Select(m => m.Value);

            var video = new YouTubeVideo
            {
                Id = item.Id,
                Views = item.Statistics.ViewCount,
                Rating = 
                    item.Statistics.LikeCount ?? 0L / 
                    (1L + item.Statistics.LikeCount ?? 0L + item.Statistics.DislikeCount ?? 0L),
                Length = TimeSpan.Parse(item.ContentDetails.Duration),
                Tags = item.Snippet.Tags,
                MentionedVideos = matches
            };

            return video;
        }
    }
}
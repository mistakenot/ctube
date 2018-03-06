using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using Web;
using Web.Data;
using Web.Services;
using Xunit;

namespace Tests
{
    public class YouTubeServiceTests
    {
        [Fact]
        public async Task YouTubeService_GetVideoById()
        {
            var id = "ci1PJexnfNE";
            YouTubeService service = CreateService();

            var request = service.Videos.List("id,snippet,statistics,contentDetails");
            request.Id = id;
            var result = await request.ExecuteAsync();
            var item = Assert.Single(result.Items);

            Assert.Equal(id, item.Id);
            Assert.StartsWith("Why is C such an influential language?", item.Snippet.Description);
            Assert.Equal("UC9-y-6csu5WGm29I7JiwpnA", item.Snippet.ChannelId);
            Assert.NotNull(item.Statistics);
            Assert.NotNull(item.ContentDetails);
            // Assert.Empty(item.Snippet.Tags);

            var video = YouTubeVideo.From(item);

            Assert.Equal(2, video.MentionedVideos.Count());
            Assert.Equal("9T8A89jgeTI", video.MentionedVideos.First());
            Assert.Equal("rh7kpkwXnwA", video.MentionedVideos.Skip(1).Single());
            Assert.Equal(TimeSpan.Parse("00:10:50"), video.Length);
        }
        
        [Fact]
        public void YouTubeModel_CanParseShortDuration()
        {
            var duration = YouTubeVideo.Parse("PT5M6S");
            Assert.Equal(TimeSpan.Parse("00:05:06"), duration);
        }

        [Fact]
        public void YouTubeModel_CanParseLongDuration()
        {
            var duration = YouTubeVideo.Parse("PT10M50S");
            Assert.Equal(TimeSpan.Parse("00:10:50"), duration);
        }

        private static YouTubeService CreateService()
        {
            var config = new ConfigurationBuilder()
                            .AddUserSecrets<Startup>()
                            .Build();

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = config["YoutubeApiKey"] ?? throw new ArgumentException("API key not set."),
                ApplicationName = "coffeetube"
            });
            return service;
        }

        [Fact]
        public async Task YouTubeService_GetChannelById()
        {
            var service = CreateService();

            var channelRequest = service.Channels.List("id,snippet,statistics");
            var id = "UC9-y-6csu5WGm29I7JiwpnA";

            channelRequest.Id = id;

            var response = await channelRequest.ExecuteAsync();

            var channelItem = Assert.Single(response.Items);
            
            var channel = new YouTubeChannel(channelItem);
            
            Assert.Equal(id, channel.Id);
            Assert.Equal("Computerphile", channel.Title);
            Assert.True(channel.Subscribers > 100000);
        }
    }
}
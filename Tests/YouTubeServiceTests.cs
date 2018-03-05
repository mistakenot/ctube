using System;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Web;
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
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = config["YoutubeApiKey"] ?? throw new ArgumentException("API key not set."),
                ApplicationName = "coffeetube"
            });

            var request =  service.Videos.List("id,snippet,statistics");
            request.Id = id;
            var result = await request.ExecuteAsync();
            var item = Assert.Single(result.Items);
            
            Assert.Equal(id, item.Id);
            Assert.StartsWith("Why is C such an influential language?", item.Snippet.Description);
            Assert.Equal("UC9-y-6csu5WGm29I7JiwpnA", item.Snippet.ChannelId);
            Assert.NotNull(item.Statistics);
            Assert.NotNull(item.ContentDetails);

            var video = YouTubeVideo.From(item);

            Assert.Single(video.MentionedVideos);
        }
    }
}
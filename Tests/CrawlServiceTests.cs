using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using Moq;
using Web;
using Web.Data;
using Web.Models;
using Web.Services;
using Xunit;

namespace Tests
{
    public class CrawlServiceTests
    {
        [Fact]
        public async Task CrawlService_CanRetrieveInfoFromSingleVideoId()
        {
            var video = new YouTubeVideo()
            {
                Id = "id",
                MentionedVideos = new [] { "mentioned_id" }
            };

            var linkedVideo = new YouTubeVideo()
            {
                Id = "mentioned_id",
                MentionedVideos = Enumerable.Empty<string>()
            };

            var youtubeMock = new Mock<IYouTubeApi>();
            youtubeMock
                .Setup(s => s.GetVideoById("mentioned_id"))
                .Returns(Task.FromResult(linkedVideo));

            var dbMock = new Mock<IRepository<YouTubeVideo>>();

            var crawler = new CrawlerService(
                youtubeMock.Object,
                dbMock.Object,
                Mock.Of<ILogger<CrawlerService>>());

            await crawler.Crawl(video, 2);

            youtubeMock.Verify(m => m.GetVideoById("mentioned_id"));
            dbMock.Verify(m => m.Contains("id"));
            dbMock.Verify(m => m.Set(video, "id"));
            dbMock.Verify(m => m.Set(linkedVideo, "mentioned_id"));
        }

        [Fact]
        public async Task CrawlService_CanCrawlFromChannelIdAsync()
        {
            var video = new YouTubeVideo()
            {
                Id = "id",
                MentionedVideos = new string[] {  }
            };

            var videoIds = new [] { "id" };

            var youtubeMock = new Mock<IYouTubeApi>();
            youtubeMock
                .Setup(s => s.GetVideoById("id"))
                .ReturnsAsync(video);
            youtubeMock
                .Setup(s => s.GetVideoIdsByChannelId("channel_id"))
                .ReturnsAsync(videoIds.AsEnumerable());

            var dbMock = new Mock<IRepository<YouTubeVideo>>();
            dbMock.Setup(db => db.Contains("id")).ReturnsAsync(false);

            var crawler = new CrawlerService(
                youtubeMock.Object,
                dbMock.Object,
                Mock.Of<ILogger<CrawlerService>>());

            await crawler.CrawlChannelAsync("channel_id", 1);

            youtubeMock.Verify(y => y.GetVideoIdsByChannelId("channel_id"));
            youtubeMock.Verify(y => y.GetVideoById("id"));
            dbMock.Verify(m => m.Contains("id"));
            dbMock.Verify(m => m.Set(video, "id"));
        }
    }
}
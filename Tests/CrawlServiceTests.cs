using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
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
                MentionedVideos = new [] { "mentioned_id" }
            };

            var linkedVideo = new YouTubeVideo()
            {
                Id = "mentioned_id"
            };

            var youtubeMock = new Mock<IYouTubeService>();
            youtubeMock
                .Setup(s => s.GetVideoById("mentioned_id"))
                .Returns(Task.FromResult(linkedVideo));

            var db = new DatabaseContext();

            var crawler = new CrawlerService(
                youtubeMock.Object, 
                db);

            await crawler.Crawl(video, 1);

            Assert.Equal(2, db.YouTubeVideos.Count());
        }
    }
}
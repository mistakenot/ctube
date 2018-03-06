using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Data;

namespace Web.Services
{
    public class CrawlerService
    {
        private readonly IYouTubeApi _youtubeService;
        private readonly IRepository<YouTubeVideo> _db;
        private readonly ILogger<CrawlerService> _logger;

        public CrawlerService(
            IYouTubeApi youtubeService, 
            IRepository<YouTubeVideo> db,
            ILogger<CrawlerService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _youtubeService = youtubeService ?? throw new ArgumentNullException(nameof(youtubeService));
        }

        public async Task Crawl(string seedId, int depth)
        {
            var seed = await _youtubeService.GetVideoById(seedId);
            await CreateIfNotExists(seed);
            await Crawl(seed, depth);
        }

        public async Task Crawl(YouTubeVideo seed, int depth)
        {
            if (depth < 1)
            {
                _logger.LogInformation("Depth < 1. Exiting.");
                return;
            }

            await CreateIfNotExists(seed);

            foreach (var id in seed.MentionedVideos)
            {
                _logger.LogInformation($"Retrieving mentioned video id {id}.");

                var mentioned = await _youtubeService.GetVideoById(id);
                
                if (mentioned != null && depth > 0)
                {
                    await Crawl(mentioned, depth - 1);
                }
            }
        }

        private async Task CreateIfNotExists(YouTubeVideo video)
        {
            var seedExists = await _db.Contains(video.Id);

            if (!seedExists)
            {
                await _db.Set(video, video.Id);
                _logger.LogInformation($"Created video id {video.Id} in database.");
            }
            else
            {
                _logger.LogInformation($"Found video id {video.Id} in database. Not creating.");
            }
        }

        public async Task CrawlChannelAsync(string channelId, int depth)
        {
            var channelVideos = await _youtubeService.GetVideoIdsByChannelId(channelId);

            foreach (var id in channelVideos)
            {
                await Crawl(id, depth);
            }
        }
    }
}
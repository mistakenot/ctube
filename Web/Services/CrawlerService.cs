using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Data;

namespace Web.Services
{
    public class CrawlerService
    {
        private readonly IYouTubeService _youtubeService;
        private readonly IRepository<YouTubeVideo> _db;

        public CrawlerService(IYouTubeService youtubeService, IRepository<YouTubeVideo> db)
        {
            _db = db;
            _youtubeService = youtubeService;
        }

        public async Task Crawl(YouTubeVideo video, int depth)
        {
            if (depth < 1)
            {
                throw new ArgumentException("Depth must be > 0");
            }

            await CreateIfNotExists(video);

            foreach (var id in video.MentionedVideos)
            {
                var mentioned = await _youtubeService.GetVideoById(id);
                
                if (mentioned != null)
                {
                    await CreateIfNotExists(mentioned);   
                }

                if (depth > 0)
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
            }
        }
    }
}
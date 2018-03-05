using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Web.Data;

namespace Web.Services
{
    public class DefaultYouTubeService : IYouTubeService
    {
        private readonly YouTubeService _service;

        public DefaultYouTubeService(string apiKey)
        {
            _service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey ?? throw new ArgumentException("API key not set."),
                ApplicationName = "coffeetube"
            });
        }

        public async Task<YouTubeVideo> GetVideoById(string id)
        {
            var request = _service.Videos.List("id,snippet,statistics,contentDetails");
            request.Id = id;
            var result = await request.ExecuteAsync();
            var video = YouTubeVideo.From(result.Items.Single());

            return video;
        }

        public async Task<IEnumerable<YouTubeVideo>> GetVideosByChannelId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
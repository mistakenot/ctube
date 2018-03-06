using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Web.Data;

namespace Web.Services
{
    public class YouTubeApi : IYouTubeApi
    {
        private const string Part = "id,snippet,statistics,contentDetails";
        private readonly YouTubeService _service;

        public YouTubeApi(string apiKey)
        {
            _service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey ?? throw new ArgumentException("API key not set."),
                ApplicationName = "coffeetube"
            });
        }

        public async Task<YouTubeVideo> GetVideoById(string id)
        {
            var request = _service.Videos.List(Part);
            request.Id = id;
            var result = await request.ExecuteAsync();
            var video = YouTubeVideo.From(result.Items.Single());

            return video;
        }

        public async Task<IEnumerable<string>> GetVideoIdsByChannelId(string id)
        {
            var request = _service.Search.List("id");
            request.ChannelId = id;

            var result = await request.ExecuteAsync();
            var ids = result.Items.Select(sr => sr.Id.VideoId);
            
            return ids;
        }
    }
}
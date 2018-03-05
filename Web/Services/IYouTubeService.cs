using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Services
{
    public interface IYouTubeService
    {
         Task<YouTubeVideo> GetVideoById(string id);
         
         Task<IEnumerable<YouTubeVideo>> GetVideosByChannelId(string id);
    }
}
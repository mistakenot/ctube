using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IVideoService
    {
         Task<IEnumerable<VideoModel>> GetSuggestions(IEnumerable<string> topics);
    }

    public class MockVideoService : IVideoService
    {
        private static readonly IEnumerable<VideoModel> MockVideos = new []
        {
            new VideoModel { Title = "Quantum Physics for Dummies", Id = "1", Channel = "A Channel", Topic = "Physics"},
            new VideoModel { Title = "Particle Physics for Dummies", Id = "2", Channel = "A Channel", Topic = "Physics"},
            new VideoModel { Title = "Biology Physics for Dummies", Id = "1", Channel = "A Channel", Topic = "Biology"},
            new VideoModel { Title = "Bitcoin for dummies", Id = "1", Channel = "A Channel", Topic = "Crypto"}
        };

        public Task<IEnumerable<VideoModel>> GetSuggestions(IEnumerable<string> topics) 
            => Task.FromResult(
                MockVideos.Where(v => topics.Any(t => t.Equals(v.Topic))));
    }
}
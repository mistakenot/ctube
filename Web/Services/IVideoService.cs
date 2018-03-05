using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IVideoService
    {
         Task<IEnumerable<VideoModel>> GetSuggestions(IEnumerable<string> topics);
         Task<IEnumerable<string>> GetAllTopics();
    }

    public class MockVideoService : IVideoService
    {
        private static readonly IEnumerable<VideoModel> MockVideos = new []
        {
            new VideoModel { Title = "Expansion of presidential power", Id = "wL76BRa1EQ0", Channel = "Khan Academy", Topic = "Politics"},
            new VideoModel { Title = "Biology: Cell Structure", Id = "URUJD5NEXC8", Channel = "Nucleus Medical Media", Topic = "Biology"},
            new VideoModel { Title = "Spectre & Meltdown - Computerphile", Id = "I5mRwzVvFGE", Channel = "Computerphile", Topic = "CompSci" },
            new VideoModel { Title = "A Curious Pattern Indeen", Id = "84hEmGHw3J8", Channel = "3Blue1Brown", Topic = "Maths" }
        };

        public Task<IEnumerable<string>> GetAllTopics()
            => Task.FromResult(
                MockVideos.Select(v => v.Topic).Distinct());

        public Task<IEnumerable<VideoModel>> GetSuggestions(IEnumerable<string> topics) 
            => Task.FromResult(
                MockVideos.Where(v => topics.Any(t => t.Equals(v.Topic))));

    }
}
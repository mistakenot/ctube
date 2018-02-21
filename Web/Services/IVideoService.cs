using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IVideoService
    {
         Task<IEnumerable<Video>> GetSuggestions();
    }

    public class MockVideoService : IVideoService
    {
        public Task<IEnumerable<Video>> GetSuggestions()
        {
            return Task.FromResult(Enumerable.Empty<Video>());
        }
    }
}
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Web.Services
{
    public class MockRepository<T> : IRepository<T>
        where T : class
    {
        protected static readonly ConcurrentDictionary<string, string> Dict
             = new ConcurrentDictionary<string, string>();

        public Task<bool> Contains(string id) 
            => Task.FromResult(Dict.ContainsKey(id));

        public Task<T> Get(string id)
        {
            var item = Dict[id];

            if (item != null)
            {
                var video = JsonConvert.DeserializeObject<T>(item);
                return Task.FromResult(video);
            }

            return Task.FromResult<T>(null);
        }

        public Task Set(T item, string id)
        {
            Dict[id] = JsonConvert.SerializeObject(item);
            return Task.FromResult(0);
        }
    }
}
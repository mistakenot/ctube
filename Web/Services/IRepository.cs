using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Web.Data;

namespace Web.Services
{
    public interface IRepository<T>
        where T : class
    {
         Task<T> Get(string id);

         Task Set(T item, string id);

         Task<bool> Contains(string id);
    }

    public class MockRepository<T> : IRepository<T>
        where T : class
    {
        protected static readonly ConcurrentDictionary<string, string> Dict
             = new ConcurrentDictionary<string, string>();

        public async Task<bool> Contains(string id)
        {
            return Dict.ContainsKey(id);
        }

        public async Task<T> Get(string id)
        {
            var item = Dict[id];

            if (item != null)
            {
                var video = JsonConvert.DeserializeObject<T>(item);
                return video;
            }

            return null;
        }

        public async Task Set(T item, string id)
        {
            Dict[id] = JsonConvert.SerializeObject(item);
        }
    }
}
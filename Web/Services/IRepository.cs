using System.Threading.Tasks;

namespace Web.Services
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> Get(string id);

        Task Set(T item, string id);

        Task<bool> Contains(string id);
    }
}
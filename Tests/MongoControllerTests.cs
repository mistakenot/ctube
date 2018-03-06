using System.Threading.Tasks;
using Web.Data;
using Web.Services;
using Xunit;

namespace Tests
{
    public class MongoRepositoryTests
    {
        // [Fact] Requires DB to be running.
        public async Task MongoRepository_CanCreateAndRetrieve()
        {
            var connectionString = "mongodb://0.0.0.0:27017";
            var collection = "videos";
            var repository = new MongoRepository<YouTubeVideo>(connectionString, "testdb", collection);

            var entity = new YouTubeVideo
            {
                Id = "test_id"
            };

            await repository.Set(entity, "test_id");

            var actual = await repository.Get("test_id");

            Assert.Equal(entity.Id, actual.Id);
        }
    }
}
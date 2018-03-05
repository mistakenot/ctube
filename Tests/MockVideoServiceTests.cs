using Web.Services;
using Xunit;

namespace Tests
{
    public class MockVideoServiceTests
    {
        [Fact]
        public void MockVideoService_ReturnsRelevantVideos()
        {
            var service = new MockVideoService();
            var topics = new [] {"Physics"};

            var result = service.GetSuggestions(topics).Result;

            Assert.All(result, r => Assert.Equal("Physics", r.Topic));
        }
    }
}
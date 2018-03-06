using System.Linq;
using System.Threading.Tasks;
using Web.Services;
using Xunit;

namespace Tests
{
    public class YouTubeApiTests
    {
        private readonly YouTubeApi _subject;

        public YouTubeApiTests()
        {
            var key = Utils.ReadYouTubeApiKeyFromUserSecrets();
            _subject = new YouTubeApi(key);
        }

        [Fact]
        public async Task YouTubeApi_ReadsOneVideoById()
        {
            var id = "ci1PJexnfNE";
            var result = await _subject.GetVideoById(id);

            Assert.Equal(id, result.Id);
            Assert.NotNull(result.ChannelId);
            Assert.Equal(2, result.MentionedVideos.Count());
            Assert.NotEmpty(result.Tags);
        }

        [Fact]
        public async Task YouTubeApi_ReadsOneChannel()
        {
            var id = "UC9-y-6csu5WGm29I7JiwpnA";
            var result = await _subject.GetVideoIdsByChannelId(id);

            Assert.NotEmpty(result);
        }
    }
}
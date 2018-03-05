using Google.Apis.YouTube.v3.Data;

namespace Web.Data
{
    public class YouTubeChannel
    {
        public string Id { get; private set; }
        public ulong Subscribers { get; private set; }
        public string Title { get; private set; }

        public YouTubeChannel(Channel channel)
        {
            Id = channel.Id;
            Title = channel.Snippet.Title;
            Subscribers = channel.Statistics.SubscriberCount ?? 0;
        }
    }
}
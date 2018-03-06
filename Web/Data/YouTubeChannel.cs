using Google.Apis.YouTube.v3.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.Data
{
    public class YouTubeChannel
    {
        [BsonId]
        public string Id { get; private set; }
        [BsonElement]
        public ulong Subscribers { get; private set; }
        [BsonElement]
        public string Title { get; private set; }

        public YouTubeChannel(Channel channel)
        {
            Id = channel.Id;
            Title = channel.Snippet.Title;
            Subscribers = channel.Statistics.SubscriberCount ?? 0;
        }
    }
}
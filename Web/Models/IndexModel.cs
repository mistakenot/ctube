using System.Collections.Generic;

namespace Web.Models
{
    public class IndexModel
    {
        public IEnumerable<VideoModel> Suggestions { get; set; }

        public IEnumerable<TopicModel> Topics { get; set; }
    }
}
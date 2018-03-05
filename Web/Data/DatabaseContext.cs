using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<YouTubeChannel> YouTubeChannels { get; set; }
        public DbSet<YouTubeVideo> YouTubeVideos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("coffeetube");
        }
    }
}
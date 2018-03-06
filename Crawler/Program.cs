using System;
using Microsoft.Extensions.CommandLineUtils;
using Web.Services;
using Web.Data;
using Microsoft.Extensions.Logging;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = "Coffeetube Crawler"
            };

            app.Command("crawl", command => 
            {
                command.Description = "Begins to crawl from a seed video id.";

                var seedIdOption = command.Option("--seed", "Id of YouTube Seed Video", CommandOptionType.SingleValue);
                var connectionStringArg = command.Option("--mongo", "Mongo database connection string.", CommandOptionType.SingleValue);
                var youtubeApiKey = command.Option("--api", "Youtube api key.", CommandOptionType.SingleValue);
                var depth = command.Option("--depth", "Search depth.", CommandOptionType.SingleValue);
                
                command.OnExecute(async () =>
                {
                    var apiKey = youtubeApiKey.HasValue() 
                        ? youtubeApiKey.Value() 
                        : Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
                        
                    var logger = new LoggerFactory()
                        .AddConsole()
                        .CreateLogger<CrawlerService>();

                    var crawler = new CrawlerService(
                        new YouTubeApi(
                            apiKey),
                        new MongoRepository<YouTubeVideo>(
                            connectionStringArg.Value(),
                            "testdb",
                            "videos"),
                        logger);

                    logger.LogInformation($"Starting crawler...");

                    try
                    {
                        await crawler.Crawl(seedIdOption.Value(), int.Parse(depth.Value()));
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error during crawl.");
                    }

                    logger.LogInformation($"Crawl complete.");

                    return 0;
                });
            });

            app.Execute(args);
        }
    }
}

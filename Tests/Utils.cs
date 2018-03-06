using System;
using Microsoft.Extensions.Configuration;
using Web;

namespace Tests
{
    public static class Utils
    {
        public static string ReadYouTubeApiKeyFromUserSecrets() 
            => new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build()
                ["YouTubeApiKey"] ?? throw new ArgumentException("API key not set.");

    }
}
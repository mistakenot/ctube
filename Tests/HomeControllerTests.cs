using Web.Controllers;
using Xunit;
using Moq;
using Web.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Tess
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task HomeController_LoadReturnsVideos()
        {
            var topics = new [] {"Physics"};
            var videos = new [] {
                new VideoModel { Topic = "Physics" },
                new VideoModel { Topic = "Econ" }} as IEnumerable<VideoModel>;

            var videoServiceMock = new Mock<IVideoService>();
            videoServiceMock.Setup(s => s.GetSuggestions(topics))
                .ReturnsAsync(videos.Take(1));

            var controller = new HomeController(
                videoServiceMock.Object,
                Mock.Of<ILogger<HomeController>>());

            var result = await controller.Load(topics);

            var resultModel = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsType<IndexModel>(resultModel.Value);

            Assert.Single(actual.Suggestions);
            Assert.Equal("Physics", actual.Suggestions.Single().Topic);
        }
    }
}
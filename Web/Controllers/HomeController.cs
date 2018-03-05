using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IEnumerable<string> AllTopics 
            = new [] {"Physics", "Maths", "Econ", "Crypto", "Biology"};

        private readonly IVideoService _videoService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IVideoService videoService,
            ILogger<HomeController> logger)
        {
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Topics() => Json(AllTopics);

        public async Task<IActionResult> Load(string[] topics)
        {
            _logger.LogInformation($"Loading videos for topics {string.Join(",", topics)}.");

            var chosenTopics = topics == null || topics.Count() == 0 ? AllTopics.ToArray() : topics;
            
            _logger.LogInformation($"Loading videos for chosen topics {string.Join(",", chosenTopics)}.");

            var suggestions = await _videoService.GetSuggestions(chosenTopics);

            var model = new IndexModel
            {
                Suggestions = suggestions,
                Topics = AllTopics.Select(t => new TopicModel {Label = t, IsActive = chosenTopics.Contains(t)})
            };

            _logger.LogInformation($"Returning {model.Suggestions.Count()} suggestions.");

            return Json(model);
        }
    }
}

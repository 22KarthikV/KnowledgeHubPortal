using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Web.Models;
using KnowledgeHubPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KnowledgeHubPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var statistics = await _unitOfWork.Statistics.GetStatisticsDictionaryAsync();
            var lastUpdateTime = await _unitOfWork.Statistics.GetLastUpdateTimeAsync();

            var homeViewModel = new HomeViewModel
            {
                TopContributor = statistics.GetValueOrDefault("TopContributor", "N/A"),
                StarContributor = statistics.GetValueOrDefault("StarContributor", "N/A"),
                MostPopularCategory = statistics.GetValueOrDefault("MostPopularCategory", "N/A"),
                TotalApprovedUrls = int.Parse(statistics.GetValueOrDefault("TotalApprovedUrls", "0")),
                LastUpdateTime = lastUpdateTime,
                RecentUrls = await GetRecentApprovedUrls(5)
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<UrlViewModel>> GetRecentApprovedUrls(int count)
        {
            return await _unitOfWork.Urls.GetAllIncluding(u => u.Category, u => u.User)
                .Where(u => u.IsApproved)
                .OrderByDescending(u => u.ApprovedAt)
                .Take(count)
                .Select(u => new UrlViewModel
                {
                    Id = u.UrlId,
                    Title = u.Title,
                    Link = u.Link,
                    CategoryName = u.Category.CategoryName,
                    SubmitterName = u.User.Name,
                    ApprovalDate = u.ApprovedAt.Value
                })
                .ToListAsync();
        }
    }
}
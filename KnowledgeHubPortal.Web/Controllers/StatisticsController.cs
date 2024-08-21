// KnowledgeHubPortal.Web/Controllers/StatisticsController.cs
using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeHubPortal.Web.ViewModels;

namespace KnowledgeHubPortal.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var statistics = await _unitOfWork.Statistics.GetStatisticsDictionaryAsync();
            var lastUpdateTime = await _unitOfWork.Statistics.GetLastUpdateTimeAsync();

            var viewModel = new StatisticsViewModel
            {
                TopContributor = statistics.GetValueOrDefault("TopContributor", "N/A"),
                StarContributor = statistics.GetValueOrDefault("StarContributor", "N/A"),
                MostPopularCategory = statistics.GetValueOrDefault("MostPopularCategory", "N/A"),
                TotalApprovedUrls = int.Parse(statistics.GetValueOrDefault("TotalApprovedUrls", "0")),
                LastUpdateTime = lastUpdateTime,
                RecentApprovedUrls = await GetRecentApprovedUrls(5),
                SubmissionTrends = await GetSubmissionTrends(30),
                PopularCategories = await GetPopularCategories(5),
                TopContributors = await GetTopContributors(5)
            };

            return View(viewModel);
        }

        private async Task<List<RecentUrlViewModel>> GetRecentApprovedUrls(int count)
        {
            return await _unitOfWork.Urls.GetAllIncluding(u => u.Category, u => u.User)
                .Where(u => u.IsApproved)
                .OrderByDescending(u => u.ApprovedAt)
                .Take(count)
                .Select(u => new RecentUrlViewModel
                {
                    Title = u.Title,
                    Link = u.Link,
                    CategoryName = u.Category.CategoryName,
                    SubmitterName = u.User.Name,
                    ApprovalDate = u.ApprovedAt.Value
                })
                .ToListAsync();
        }

        private async Task<SubmissionTrendsViewModel> GetSubmissionTrends(int days)
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-days + 1);

            var submissions = await _unitOfWork.Urls.GetAllAsQueryable()
                .Where(u => u.SubmittedAt >= startDate && u.SubmittedAt <= endDate)
                .GroupBy(u => u.SubmittedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Count(),
                    ApprovedCount = g.Count(u => u.IsApproved)
                })
                .ToListAsync();

            var trends = new SubmissionTrendsViewModel
            {
                Dates = new List<string>(),
                TotalSubmissions = new List<int>(),
                ApprovedSubmissions = new List<int>()
            };

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var submission = submissions.FirstOrDefault(s => s.Date == date);
                trends.Dates.Add(date.ToString("MM/dd"));
                trends.TotalSubmissions.Add(submission?.TotalCount ?? 0);
                trends.ApprovedSubmissions.Add(submission?.ApprovedCount ?? 0);
            }

            return trends;
        }

        private async Task<List<CategoryViewModel>> GetPopularCategories(int count)
        {
            return await _unitOfWork.Urls.GetAllIncluding(u => u.Category)
                .Where(u => u.IsApproved)
                .GroupBy(u => u.CategoryId)
                .Select(g => new CategoryViewModel
                {
                    CategoryId = g.Key,
                    CategoryName = g.First().Category.CategoryName,
                    UrlCount = g.Count()
                })
                .OrderByDescending(c => c.UrlCount)
                .Take(count)
                .ToListAsync();
        }

        private async Task<List<ContributorViewModel>> GetTopContributors(int count)
        {
            return await _unitOfWork.Urls.GetAllIncluding(u => u.User)
                .Where(u => u.IsApproved)
                .GroupBy(u => u.UserId)
                .Select(g => new ContributorViewModel
                {
                    UserId = g.Key,
                    Username = g.First().User.Name,
                    ApprovedSubmissions = g.Count()
                })
                .OrderByDescending(c => c.ApprovedSubmissions)
                .Take(count)
                .ToListAsync();
        }
    }
}
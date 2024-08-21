// KnowledgeHubPortal.Web/Services/StatisticsUpdateService.cs
using KnowledgeHubPortal.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KnowledgeHubPortal.Web.Services
{
    public class StatisticsUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public StatisticsUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await UpdateStatistics(unitOfWork);
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task UpdateStatistics(IUnitOfWork unitOfWork)
        {
            var topContributor = await GetTopContributor(unitOfWork);
            var starContributor = await GetStarContributor(unitOfWork);
            var mostPopularCategory = await GetMostPopularCategory(unitOfWork);
            var totalApprovedUrls = await GetTotalApprovedUrls(unitOfWork);

            await unitOfWork.Statistics.UpdateStatisticAsync("TopContributor", topContributor);
            await unitOfWork.Statistics.UpdateStatisticAsync("StarContributor", starContributor);
            await unitOfWork.Statistics.UpdateStatisticAsync("MostPopularCategory", mostPopularCategory);
            await unitOfWork.Statistics.UpdateStatisticAsync("TotalApprovedUrls", totalApprovedUrls.ToString());
        }

        private async Task<string> GetTopContributor(IUnitOfWork unitOfWork)
        {
            var topContributor = await unitOfWork.Urls.GetAllIncluding(u => u.User)
                .Where(u => u.IsApproved)
                .GroupBy(u => u.UserId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().User.Name)
                .FirstOrDefaultAsync();

            return topContributor ?? "N/A";
        }

        private async Task<string> GetStarContributor(IUnitOfWork unitOfWork)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var starContributor = await unitOfWork.Urls.GetAllIncluding(u => u.User)
                .Where(u => u.IsApproved && u.ApprovedAt >= startOfMonth)
                .GroupBy(u => u.UserId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().User.Name)
                .FirstOrDefaultAsync();

            return starContributor ?? "N/A";
        }

        private async Task<string> GetMostPopularCategory(IUnitOfWork unitOfWork)
        {
            var mostPopularCategory = await unitOfWork.Urls.GetAllIncluding(u => u.Category)
                .Where(u => u.IsApproved)
                .GroupBy(u => u.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().Category.CategoryName)
                .FirstOrDefaultAsync();

            return mostPopularCategory ?? "N/A";
        }

        private async Task<int> GetTotalApprovedUrls(IUnitOfWork unitOfWork)
        {
            return await unitOfWork.Urls.CountAsync(u => u.IsApproved);
        }
    }
}
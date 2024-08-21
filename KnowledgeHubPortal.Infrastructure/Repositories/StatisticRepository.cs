// KnowledgeHubPortal.Infrastructure/Repositories/StatisticRepository.cs
using KnowledgeHubPortal.Core.Entities;
using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeHubPortal.Infrastructure.Repositories
{
    public class StatisticRepository : Repository<Statistic>, IStatisticRepository
    {
        public StatisticRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Statistic> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<IEnumerable<Statistic>> GetAllActiveAsync()
        {
            return await _dbSet.Where(s => s.UpdatedAt >= DateTime.UtcNow.AddDays(-1)).ToListAsync();
        }

        public async Task UpdateStatisticAsync(string name, string value)
        {
            var statistic = await GetByNameAsync(name);
            if (statistic == null)
            {
                statistic = new Statistic { Name = name };
                _dbSet.Add(statistic);
            }

            statistic.Value = value;
            statistic.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetStatisticsDictionaryAsync()
        {
            return await _dbSet.ToDictionaryAsync(s => s.Name, s => s.Value);
        }

        public async Task<DateTime> GetLastUpdateTimeAsync()
        {
            var lastUpdateTime = await _context.Statistics.MaxAsync(s => (DateTime?)s.UpdatedAt);
            return lastUpdateTime ?? DateTime.MinValue; // Provide a default value if null
        }
    }
}
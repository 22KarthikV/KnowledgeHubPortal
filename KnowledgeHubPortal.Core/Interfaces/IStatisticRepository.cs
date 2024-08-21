// KnowledgeHubPortal.Core/Interfaces/IStatisticRepository.cs
using KnowledgeHubPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeHubPortal.Core.Interfaces
{
    public interface IStatisticRepository : IRepository<Statistic>
    {
        Task<Statistic> GetByNameAsync(string name);
        Task<IEnumerable<Statistic>> GetAllActiveAsync();
        Task UpdateStatisticAsync(string name, string value);
        Task<Dictionary<string, string>> GetStatisticsDictionaryAsync();
        Task<DateTime> GetLastUpdateTimeAsync();
    }
}
﻿using KnowledgeHubPortal.Core.Entities;

namespace KnowledgeHubPortal.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Category> Categories { get; }
        IRepository<Url> Urls { get; }
        IStatisticRepository Statistics { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
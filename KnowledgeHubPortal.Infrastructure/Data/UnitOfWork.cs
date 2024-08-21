using KnowledgeHubPortal.Core.Entities;
using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Infrastructure.Repositories;

namespace KnowledgeHubPortal.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<User> Users { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Url> Urls { get; private set; }
        public IStatisticRepository Statistics { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            Categories = new Repository<Category>(_context);
            Urls = new Repository<Url>(_context);
            Statistics = new StatisticRepository(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
    }
}
using Microsoft.EntityFrameworkCore;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Interfaces.Repositories;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;

namespace MyApiTemplateCleanArchi.Infrastructure.Commons.Bases
{
    public abstract class BaseRepository<TEntity, TContext>
        : IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id)
            ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} #{id} not found");

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
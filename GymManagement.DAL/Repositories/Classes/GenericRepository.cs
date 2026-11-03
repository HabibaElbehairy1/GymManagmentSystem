using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAcync(bool tracking = false, CancellationToken ct = default)
         => tracking ? await _dbset.ToListAsync(ct) : await _dbset.AsNoTracking().ToListAsync(ct);

        public async Task<TEntity?> GetByIdAcync(int id, CancellationToken ct = default)
        => await _dbset.FindAsync(id, ct);

        public void Add(TEntity entity)
        { 
            _dbset.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _dbset.Update(entity);

        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, ct);

        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            return predicate is null
                ? await query.CountAsync(ct)
                : await query.CountAsync(predicate, ct);
        }
    }
}

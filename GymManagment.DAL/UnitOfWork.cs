using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly ISessionRepository _sessionRepository;
        private readonly Dictionary<string,object> _repositories = [];
        public UnitOfWork(GymDbContext context , ISessionRepository sessionRepository)
        {
            _context = context;
            _sessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository
        => _sessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out object? value))
                return value as IGenericRepository<TEntity>;


            var repo = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeName, repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    }
}

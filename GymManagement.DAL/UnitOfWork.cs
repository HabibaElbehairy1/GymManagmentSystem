using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly Dictionary<string,object> _repositories = [];
        public UnitOfWork(GymDbContext context , ISessionRepository sessionRepository , IMembershipRepository membershipRepository)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _membershipRepository = membershipRepository;
        }

        public ISessionRepository SessionRepository
          => _sessionRepository;
        public IMembershipRepository MembershipRepository
            => _membershipRepository;
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

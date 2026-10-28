using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagment.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity,new()
    {
        Task<IEnumerable<TEntity>> GetAllAcync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAcync(int id, CancellationToken ct = default);
        Task<int> AddAcync(TEntity entity, CancellationToken ct = default);
        Task<int> UpdateAcync(TEntity entity, CancellationToken ct = default);
        Task<int> DeleteAcync(TEntity entity, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate ,CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    }
}

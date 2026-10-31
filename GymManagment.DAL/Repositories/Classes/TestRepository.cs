using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagment.DAL.Repositories.Classes
{
    public class TestRepository : IPlanRepository
    {
        public void Add(Plan entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAcync(Plan p, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Plan entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAcync(Plan p, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FirstOrDefaultAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetAllAcync(bool tracking = false, CancellationToken ct = default)
        {
            var plans = new List<Plan>()
            {
                new Plan(){Name="TEST"}
            };
            return plans;

        }

        public Task<Plan?> GetByIdAcync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Plan entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAcync(Plan p, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        Task<Plan?> IGenericRepository<Plan>.FirstOrDefaultAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
